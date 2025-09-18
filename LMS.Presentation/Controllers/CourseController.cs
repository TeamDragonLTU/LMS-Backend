using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace LMS.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public CourseController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }
        
    }
}
