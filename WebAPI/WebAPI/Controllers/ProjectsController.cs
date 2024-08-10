using Business.Abstracts;
using Business.Dtos.Requests.ProjectRequests;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.SeriLog.Logger;
using Core.CrossCuttingConcerns.Transaction;
using Core.DataAccess.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Saml;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [Cache(60)]
        [HttpGet("GetList")]
        public async Task<IActionResult> GetListAsync([FromQuery] PageRequest pageRequest)
        {
            var result = await _projectService.GetListAsync(pageRequest);
            return Ok(result);
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [Cache]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _projectService.GetByIdAsync(id);
            return Ok(result);
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [CacheRemove("Projects.Get")]
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] CreateProjectRequest createProjectRequest)
        {
            var result = await _projectService.AddAsync(createProjectRequest);
            return Ok(result);
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [CacheRemove("Projects.Get")]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProjectRequest updateProjectRequest)
        {
            var result = await _projectService.UpdateAsync(updateProjectRequest);
            return Ok(result);
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [CacheRemove("Projects.Get")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteProjectRequest deleteProjectRequest)
        {
            var result = await _projectService.DeleteAsync(deleteProjectRequest);
            return Ok(result);
        }
    }
}