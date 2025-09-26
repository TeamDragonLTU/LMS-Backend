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

    private List<Module> GetModules(int nrOfModules)
    {
        var faker = new Faker<Module>().Rules((faker, module) =>
        {
            module.Name = faker.Commerce.ProductName();
            module.Description = faker.Commerce.ProductDescription();
            module.StartDate = faker.Date.Past(1);
            module.EndDate = faker.Date.Future(1);
            var nrOfActivities = faker.PickRandom(2, 8);
            module.Activities = GetActivities(nrOfActivities);
        });

        return faker.Generate(nrOfModules);
    }

    private List<Course> GetCourses(int nrOfCourses)
    {
        var faker = new Faker<Course>().Rules((faker, course) =>
        {
            course.Name = faker.Commerce.Department();
            course.Description = faker.Commerce.ProductDescription();
            course.StartDate = faker.Date.Past(1);
            var nrOfModules = faker.PickRandom(2, 8);
            course.Modules = GetModules(nrOfModules);
        });

        return faker.Generate(nrOfCourses);
    }

    private List<Activity> GetActivities(int nrOfActivities)
    {
        DateTime dateTime = DateTime.UtcNow;

        var faker = new Faker<Activity>().Rules((faker, activity) =>
        {
            activity.Name = faker.Commerce.ProductName();
            activity.Description = faker.Commerce.ProductDescription();
            activity.StartTime = faker.Date.Past(1, dateTime);
            activity.EndTime = faker.Date.Future(1, dateTime);
            activity.ActivityType = _activityTypes[faker.Random.Int(0, _activityTypes.Count - 1)];
        });

        return faker.Generate(nrOfActivities);

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


