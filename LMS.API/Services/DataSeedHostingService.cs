using Bogus;
using LMS.Infractructure.Data;
using LMS.Shared.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace LMS.API.Services;

//Add in secret.json
//{
//   "password" :  "YourSecretPasswordHere"
//}
public class DataSeedHostingService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DataSeedHostingService> _logger;
    private UserManager<ApplicationUser> _userManager = null!;
    private RoleManager<IdentityRole> _roleManager = null!;
    private UserRole _TeacherRole = UserRole.Teacher;
    private UserRole _StudentRole = UserRole.Student;
    private const string teacherDemoUserNameEmail = "teacher@test.com";
    private const string studentDemoUserNameEmail = "student@test.com";
    private List<ActivityType> _activityTypes = null!;
    private List<Course> _courses = null!;


    public DataSeedHostingService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DataSeedHostingService> logger)
    {
        this._serviceProvider = serviceProvider;
        this._configuration = configuration;
        this._logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        if (!env.IsDevelopment()) return;

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (await context.Users.AnyAsync(cancellationToken)) return;

        _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        ArgumentNullException.ThrowIfNull(_roleManager, nameof(_roleManager));
        ArgumentNullException.ThrowIfNull(_userManager, nameof(_userManager));

        try
        {
            _activityTypes = GetActivityTypes();
            context.AddRange(_activityTypes);
            _courses = GetCourses(7);
            context.AddRange(_courses);

            await AddRolesAsync(Enum.GetNames(typeof(UserRole)));
            await AddDemoUsersAsync();
            await AddUsersAsync(90);

            await context.SaveChangesAsync();
            _logger.LogInformation("Seed complete");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Data seed fail with error: {ex.Message}");
            throw;
        }
    }

    private List<Course> GetCourses(int nrOfCourses)
    {
    var faker = new Faker<Course>("sv").Rules((faker, course) =>
        {
            var currentYear = DateTime.UtcNow.Year;
            var currentMonth = DateTime.UtcNow.Month;

            int startYear;
            if (currentMonth >= 8)
            {
                startYear = currentYear;
            }
            else
            {
                startYear = currentYear - 1;
            }

            var earliestStart = new DateTime(startYear, 8, 25);
            var latestStart = new DateTime(startYear, 9, 25);
            course.StartDate = faker.Date.Between(earliestStart, latestStart);

            // IT-teknisk kurslista på svenska
            var itCourseNames = new[]
            {
                "Grundläggande programmering",
                "Webbutveckling",
                "Systemarkitektur",
                "Databasteknik",
                "Nätverk och säkerhet",
                "Molntjänster och DevOps",
                "Mjukvarutestning och kvalitetssäkring",
                "Maskininlärning för utvecklare",
                "Programmeringsparadigmer",
                "Frontend-utveckling med modern JavaScript"
            };

            course.Name = faker.PickRandom(itCourseNames);
            course.Description = faker.Lorem.Sentence(8);

            var nrOfModules = faker.PickRandom(4, 8);

            var minEndDate = new DateTime(startYear + 1, 3, 31);
            var maxEndDate = new DateTime(startYear + 1, 6, 30);

            var courseEndDate = faker.Date.Between(minEndDate, maxEndDate);

            course.Modules = GetModules(nrOfModules, course.StartDate, courseEndDate);
        });

        return faker.Generate(nrOfCourses);
    }

    private List<Module> GetModules(int nrOfModules, DateTime courseStartDate, DateTime courseEndDate)
    {
        var modules = new List<Module>();
        var faker = new Faker("sv");

        var totalCourseDays = (courseEndDate - courseStartDate).TotalDays;
        var currentDate = courseStartDate;

        for (int i = 0; i < nrOfModules; i++)
        {
            var remainingDays = (courseEndDate - currentDate).TotalDays;
            var remainingModules = nrOfModules - i;

            var avgDaysPerRemainingModule = remainingDays / remainingModules;

            var minDuration = 21;
            var maxDuration = 56;


            var constrainedMax = (int)Math.Floor(avgDaysPerRemainingModule * 1.2);
            maxDuration = Math.Min(maxDuration, constrainedMax);

            if (i == nrOfModules - 1)
            {
                maxDuration = (int)Math.Floor(remainingDays);
                minDuration = Math.Min(minDuration, maxDuration);
            }
            else
            {
                var daysNeededForOthers = (remainingModules - 1) * minDuration;
                maxDuration = Math.Min(maxDuration, (int)Math.Floor(remainingDays - daysNeededForOthers));
            }

            maxDuration = Math.Max(minDuration, maxDuration);

            var moduleDurationDays = faker.Random.Int(minDuration, maxDuration);

            var moduleStartDate = currentDate;
            var moduleEndDate = moduleStartDate.AddDays(moduleDurationDays);

            if (moduleEndDate > courseEndDate)
            {
                moduleEndDate = courseEndDate;
            }

            var itModuleNames = new[]
            {
                "Introduktion och utvecklingsmiljö",
                "Versionshantering (Git)",
                "Datamodellering och SQL",
                "API-design och REST",
                "Säkerhet och kryptering",
                "CI/CD och automatisering",
                "Prestanda och skalbarhet",
                "Testdriven utveckling",
                "Frontend-ramverk och verktyg",
                "Molnplattformar och deployment"
            };

            var module = new Module
            {
                Name = faker.PickRandom(itModuleNames),
                Description = faker.Lorem.Sentence(6),
                StartDate = moduleStartDate,
                EndDate = moduleEndDate,
                Activities = GetActivities(moduleStartDate, moduleEndDate)
            };

            modules.Add(module);

            currentDate = moduleEndDate.AddDays(1);

            if (currentDate >= courseEndDate)
            {
                break;
            }
        }

        return modules;
    }

    private List<Activity> GetActivities(DateTime moduleStartDate, DateTime moduleEndDate)
    {
        var activities = new List<Activity>();
        var faker = new Faker("sv");

        var workingDays = new List<DateTime>();
        var currentDate = moduleStartDate.Date;

        while (currentDate <= moduleEndDate.Date)
        {
            if (currentDate.DayOfWeek != DayOfWeek.Saturday &&
                currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                workingDays.Add(currentDate);
            }
            currentDate = currentDate.AddDays(1);
        }

        var totalActivitiesToGenerate = workingDays.Count * 2;

        var activitiesPerDay = new Dictionary<DateTime, int>();

        foreach (var day in workingDays)
        {
            activitiesPerDay[day] = 1;
        }

        var remainingActivities = totalActivitiesToGenerate - workingDays.Count;
        for (int i = 0; i < remainingActivities; i++)
        {
            var randomDay = faker.PickRandom(workingDays);
            activitiesPerDay[randomDay]++;
        }

        foreach (var day in workingDays)
        {
            var activitiesForThisDay = activitiesPerDay[day];
            var occupiedTimeRanges = new List<(int startHour, int endHour)>();

            for (int i = 0; i < activitiesForThisDay; i++)
            {
                var activityDurationHours = faker.Random.Int(1, 4);
                var possibleStartHours = new List<int>();

                if (activityDurationHours <= 3) possibleStartHours.Add(9);
                if (activityDurationHours <= 2) possibleStartHours.Add(10);
                if (activityDurationHours <= 1) possibleStartHours.Add(11);

                if (activityDurationHours <= 4) possibleStartHours.Add(13);
                if (activityDurationHours <= 3) possibleStartHours.Add(14);
                if (activityDurationHours <= 2) possibleStartHours.Add(15);
                if (activityDurationHours <= 1) possibleStartHours.Add(16);

                var validStartHours = new List<int>();
                foreach (var startHour in possibleStartHours)
                {
                    var endHour = startHour + activityDurationHours;
                    bool hasOverlap = false;

                    foreach (var (occupiedStart, occupiedEnd) in occupiedTimeRanges)
                    {
                        if (startHour < occupiedEnd && endHour > occupiedStart)
                        {
                            hasOverlap = true;
                            break;
                        }
                    }

                    if (!hasOverlap)
                    {
                        validStartHours.Add(startHour);
                    }
                }

                if (validStartHours.Count == 0)
                {
                    continue;
                }

                var randomStartHour = faker.PickRandom(validStartHours);
                var activityStartTime = day.AddHours(randomStartHour);
                var activityEndTime = activityStartTime.AddHours(activityDurationHours);

                if (activityEndTime > moduleEndDate)
                {
                    break;
                }

                occupiedTimeRanges.Add((randomStartHour, randomStartHour + activityDurationHours));

                var itActivityNames = new[]
                {
                    "Kodgenomgång",
                    "Laboration",
                    "Kodgranskning",
                    "Workshop",
                    "Projektarbete",
                    "Föreläsning",
                    "Övning",
                    "Debug-session"
                };

                var activity = new Activity
                {
                    Name = faker.PickRandom(itActivityNames),
                    Description = faker.Lorem.Sentence(6),
                    StartTime = activityStartTime,
                    EndTime = activityEndTime,
                    ActivityType = _activityTypes[faker.Random.Int(0, _activityTypes.Count - 1)]
                };

                activities.Add(activity);
            }
        }

        return activities.OrderBy(a => a.StartTime).ToList();
    }

    private List<ActivityType> GetActivityTypes()
    {
        var activityTypes = new List<ActivityType>()
        {
            new ActivityType
            {
                 Name = "Lecture"
            },
            new ActivityType
            {
                 Name = "Assignment"
            },
            new ActivityType
            {
                 Name = "Test"
            }
        };

        return activityTypes;
    }

    private async Task AddRolesAsync(string[] rolenames)
    {
        foreach (string rolename in rolenames)
        {
            if (await _roleManager.RoleExistsAsync(rolename)) continue;
            var role = new IdentityRole { Name = rolename };
            var res = await _roleManager.CreateAsync(role);

            if (!res.Succeeded) throw new Exception(string.Join("\n", res.Errors));
        }
    }
    private async Task AddDemoUsersAsync()
    {
        var random = new Random();


        var teacher = new ApplicationUser
        {
            UserName = teacherDemoUserNameEmail,
            Email = teacherDemoUserNameEmail,
            Course = _courses[random.Next(0, _courses.Count)]
        };

        var student = new ApplicationUser
        {
            UserName = studentDemoUserNameEmail,
            Email = studentDemoUserNameEmail,
            Course = _courses[random.Next(0, _courses.Count)]
        };

        await AddUserToDb([teacher, student]);

        await SetUserRole(teacher, _TeacherRole);
        await SetUserRole(student, _StudentRole);

    }

    private async Task AddUsersAsync(int nrOfUsers)
    {
        if (nrOfUsers <= 0)
            throw new ArgumentOutOfRangeException(nameof(nrOfUsers), "Number of users must be a positive integer.");

        var random = new Random();
        int evenDistributionThreshold = Math.Min(nrOfUsers, _courses.Count);

        var faker = new Faker<ApplicationUser>("sv").Rules((faker, user) =>
        {
            user.Email = faker.Person.Email;
            user.UserName = faker.Person.Email;

            Course course;
            if (faker.IndexGlobal < evenDistributionThreshold)
            {
                course = _courses[faker.IndexGlobal % _courses.Count];
            }
            else
            {
                course = _courses[random.Next(_courses.Count)];
            }
            user.Course = course;
        });

        var users = faker.Generate(nrOfUsers);

        await AddUserToDb(users);

        // Go through each course and assign roles based on the number of users per course
        foreach (var course in _courses.Where(c => c.Users != null && c.Users.Any()))
        {
            var courseUsers = course.Users.ToList();
            int totalUsersInCourse = courseUsers.Count;

            int nrOfTeachers = CalculateTeachersForCourse(totalUsersInCourse);
            int nrOfStudents = totalUsersInCourse - nrOfTeachers;

            // Shuffle users so that names of teachers gets more varied alphabetically
            var shuffledUsers = courseUsers.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < nrOfTeachers; i++)
            {
                if (shuffledUsers[i].UserName == teacherDemoUserNameEmail || shuffledUsers[i].UserName == studentDemoUserNameEmail) continue;
                await SetUserRole(shuffledUsers[i], _TeacherRole);
            }

            for (int i = nrOfTeachers; i < shuffledUsers.Count; i++)
            {
                if (shuffledUsers[i].UserName == teacherDemoUserNameEmail || shuffledUsers[i].UserName == studentDemoUserNameEmail) continue;
                await SetUserRole(shuffledUsers[i], _StudentRole);
            }

            _logger.LogInformation($"Course '{course.Name}': Assigned {nrOfTeachers} teachers and {nrOfStudents} students");
        }
    }

    private int CalculateTeachersForCourse(int totalUsersInCourse)
    {
        if (totalUsersInCourse > 3 && totalUsersInCourse <= 21)
            return 3;
        else if (totalUsersInCourse > 21 && totalUsersInCourse <= 71)
            return 5;
        else if (totalUsersInCourse > 71)
            return 12;
        else
            return totalUsersInCourse;
    }

    private async Task SetUserRole(ApplicationUser user, UserRole role)
    {
        var roleResult = await _userManager.AddToRoleAsync(user, role.ToString());
        ArgumentNullException.ThrowIfNull(roleResult, nameof(roleResult));

        if (!roleResult.Succeeded)
        {
            _logger.LogError($"Failed to add user '{user.UserName}' to role '{role}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            throw new Exception(string.Join("\n", roleResult.Errors.Select(e => e.Description)));
        }
    }

    private async Task AddUserToDb(IEnumerable<ApplicationUser> users)
    {
        var passWord = _configuration["password"];
        ArgumentNullException.ThrowIfNull(passWord, nameof(passWord));

        foreach (var user in users)
        {
            var result = await _userManager.CreateAsync(user, passWord);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }


    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}


