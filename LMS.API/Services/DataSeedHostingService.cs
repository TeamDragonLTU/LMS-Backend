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

        if (!await context.Courses.AnyAsync())
        {
            await AddCourseToDB(context);
        }
        if (!await context.Modules.AnyAsync())
        {
            await AddModuleToDB(context);
        }
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
            var activityTypes = GetActivityTypes();
            context.AddRange(activityTypes);
            var activities = GetActivities(activityTypes);
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

    private List<Activity> GetActivities(List<ActivityType> activityTypes)
    {
        DateTime dateTime = DateTime.UtcNow;

        var faker = new Faker<Activity>().Rules((faker, activity) =>
        {
            activity.Name = faker.Commerce.ProductName();
            activity.Description = faker.Commerce.ProductDescription();
            activity.StartTime = faker.Date.Past(1, dateTime);
            activity.EndTime = faker.Date.Future(1, dateTime);
            activity.ActivityType = activityTypes[faker.Random.Int(0, activityTypes.Count - 1)];

        });

        return faker.Generate(3);

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

    public async Task AddCourseToDB(ApplicationDbContext context)
    {
        var course = new Course
        {
            Name = "Test Course",
            Description = "This is a test course",
            StartDate = DateTime.UtcNow
        };

        context.Courses.Add(course);
        await context.SaveChangesAsync();
    }
    public async Task AddModuleToDB(ApplicationDbContext context)
    {
        var course = context.Courses.FirstOrDefault();
        if (course == null) throw new Exception("No course found for seeding module.");
        var module = new Module
        {
            Name = "Sample Module",
            Description = "This is a sample module",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(1),
            CourseId = course.Id,
        };
        context.Modules.Add(module);
        await context.SaveChangesAsync();
    }

}
