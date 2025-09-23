using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presentation.Controllers
{
    [Route("api/activitytype")]
    [ApiController]
    [Produces("application/json")]

    public class ActivityTypeController: ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ActivityTypeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActivityTypesAsync() 
        {
            return Ok(await _serviceManager.ActivityTypeService.GetAllActivityTypesAsync());    
        }
    }
}
