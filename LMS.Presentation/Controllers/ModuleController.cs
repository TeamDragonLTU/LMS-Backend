using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.Module;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [SwaggerOperation(Summary = "Get all module", Description = "Gets all module")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllModules()
        {
            return Ok(await _serviceManager.ModuleService.GetAllModulesAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get module by ID", Description = "Gets a module by its ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetModule(Guid id)
        {
            var module = await _serviceManager.ModuleService.GetModuleAsync(id, false);
            if (module == null)
                return NotFound();
            return Ok(module);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create module", Description = "Creates a new module.", Tags = ["Module"])]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateModule([FromBody] CreateModuleDto dto)
        {
            try
            {
                var result = await _serviceManager.ModuleService.CreateModuleAsync(dto);
                return CreatedAtAction(nameof(GetAllModules), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update module", Description = "Updates an existing module by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateModule(Guid id, [FromBody] UpdateModuleDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch");
            var result = await _serviceManager.ModuleService.UpdateModuleAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete module", Description = "Deletes a module by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteModule(Guid id)
        {
            await _serviceManager.ModuleService.DeleteModuleAsync(id);
            return NoContent();
        }
    }
}
