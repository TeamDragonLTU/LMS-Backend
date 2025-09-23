using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Shared.DTOs.Module;

namespace LMS.Presentation.Controllers
{
    [Route("api/module")]
    [ApiController]
    [Produces("application/json")]
    public class ModuleController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ModuleController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllModules()
        {
            return Ok(await _serviceManager.ModuleService.GetAllModulesAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] CreateModuleDto dto)
        {
            var result = await _serviceManager.ModuleService.CreateModuleAsync(dto);
            return CreatedAtAction(nameof(GetAllModules), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(Guid id, [FromBody] UpdateModuleDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch");
            var result = await _serviceManager.ModuleService.UpdateModuleAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(Guid id)
        {
            await _serviceManager.ModuleService.DeleteModuleAsync(id);
            return NoContent();
        }
    }
}
