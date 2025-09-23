using Bogus;
using LMS.Infractructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services;

//Add in secret.json
//{
//   "password" :  "YourSecretPasswordHere"
//}
public class DataSeedHostingService : IHostedService
{
    private readonly IServiceProvider serviceProvider;
    private readonly IConfiguration configuration;
    private readonly ILogger<DataSeedHostingService> logger;
    private UserManager<ApplicationUser> userManager = null!;
    private RoleManager<IdentityRole> roleManager = null!;
    private const string TeacherRole = "Teacher";
    private const string StudentRole = "Student";

    public DataSeedHostingService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DataSeedHostingService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.configuration = configuration;
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        if (!env.IsDevelopment()) return;

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (await context.Users.AnyAsync(cancellationToken)) return;

        userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        ArgumentNullException.ThrowIfNull(roleManager, nameof(roleManager));
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));

        try
        {
            await AddRolesAsync([TeacherRole, StudentRole]);
            await AddDemoUsersAsync();
            await AddUsersAsync(20);
            var courses = GetCourses(1);
            context.AddRange(courses);
            var modules = GetModules(courses, 4);
            context.AddRange(modules);
            var activityTypes = GetActivityTypes();
            context.AddRange(activityTypes);
            var activities = GetActivities(activityTypes, modules, 3);
            context.AddRange(activities);
            await context.SaveChangesAsync();
            logger.LogInformation("Seed complete");
        }
        catch (Exception ex)
        {
            logger.LogError($"Data seed fail with error: {ex.Message}");
            throw;
        }
    }

    private List<Module> GetModules(List<Course> courses, int nrOfModules)
    {
        var faker = new Faker<Module>().Rules((faker, module) =>
        {
            module.Name = faker.Commerce.ProductName();
            module.Description = faker.Commerce.ProductDescription();
            module.StartDate = faker.Date.Past(1);
            module.EndDate = faker.Date.Future(1);
            module.Course = faker.PickRandom(courses);
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
        });

        return faker.Generate(nrOfCourses);
    }

    private List<Activity> GetActivities(List<ActivityType> activityTypes, List<Module> modules, int nrOfActivities)
    {
        DateTime dateTime = DateTime.UtcNow;

        var faker = new Faker<Activity>().Rules((faker, activity) =>
        {
            activity.Name = faker.Commerce.ProductName();
            activity.Description = faker.Commerce.ProductDescription();
            activity.StartTime = faker.Date.Past(1, dateTime);
            activity.EndTime = faker.Date.Future(1, dateTime);
            activity.ActivityType = activityTypes[faker.Random.Int(0, activityTypes.Count - 1)];
            activity.Module = faker.PickRandom(modules);
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
            if (await roleManager.RoleExistsAsync(rolename)) continue;
            var role = new IdentityRole { Name = rolename };
            var res = await roleManager.CreateAsync(role);

            if (!res.Succeeded) throw new Exception(string.Join("\n", res.Errors));
        }
    }
    private async Task AddDemoUsersAsync()
    {
        var teacher = new ApplicationUser
        {
            UserName = "teacher@test.com",
            Email = "teacher@test.com"
        };

        var student = new ApplicationUser
        {
            UserName = "student@test.com",
            Email = "student@test.com"
        };

        await AddUserToDb([teacher, student]);

        var teacherRoleResult = await userManager.AddToRoleAsync(teacher, TeacherRole);
        if (!teacherRoleResult.Succeeded) throw new Exception(string.Join("\n", teacherRoleResult.Errors));

        var studentRoleResult = await userManager.AddToRoleAsync(student, StudentRole);
        if (!studentRoleResult.Succeeded) throw new Exception(string.Join("\n", studentRoleResult.Errors));
    }

    private async Task AddUsersAsync(int nrOfUsers)
    {
        var faker = new Faker<ApplicationUser>("sv").Rules((f, e) =>
        {
            e.Email = f.Person.Email;
            e.UserName = f.Person.Email;
        });

        await AddUserToDb(faker.Generate(nrOfUsers));
    }

    private async Task AddUserToDb(IEnumerable<ApplicationUser> users)
    {
        var passWord = configuration["password"];
        ArgumentNullException.ThrowIfNull(passWord, nameof(passWord));

        foreach (var user in users)
        {
            var result = await userManager.CreateAsync(user, passWord);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}


