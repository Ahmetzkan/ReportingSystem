using Business.Abstracts;
using Business.Dtos.Requests.ProjectRequests;
using Core.DataAccess.Paging;
using Microsoft.AspNetCore.Mvc;

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


        //[Logging(typeof(MsSqlLogger))]
        //[Logging(typeof(FileLogger))]
        //[Cache(60)]
        [HttpGet("GetList")]
        public async Task<IActionResult> GetListAsync([FromQuery] PageRequest pageRequest)
        {
            var result = await _projectService.GetListAsync(pageRequest);
            return Ok(result);
        }


        //[Logging(typeof(MsSqlLogger))]
        //[Logging(typeof(FileLogger))]
        //[Cache]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _projectService.GetByIdAsync(id);
            return Ok(result);
        }


        //[Logging(typeof(MsSqlLogger))]
        //[Logging(typeof(FileLogger))]
        //[CacheRemove("Projects.Get")]
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] CreateProjectRequest createProjectRequest)
        {
            var result = await _projectService.AddAsync(createProjectRequest);
            return Ok(result);
        }


        //[Logging(typeof(MsSqlLogger))]
        //[Logging(typeof(FileLogger))]
        //[CacheRemove("Projects.Get")]
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProjectRequest updateProjectRequest)
        {
            var result = await _projectService.UpdateAsync(updateProjectRequest);
            return Ok(result);
        }

        //[Logging(typeof(MsSqlLogger))]
        //[Logging(typeof(FileLogger))]
        //[CacheRemove("Projects.Get")]
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] Guid id)
        {
            var result = await _projectService.DeleteAsync(id);
            return Ok(result);
        }
    }

}
