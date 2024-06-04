namespace WebAPI.Controllers
{
    using Business.Abstracts;
    using Business.Dtos.Requests.TaskRequests;
    using Core.CrossCuttingConcerns.Caching;
    using Core.CrossCuttingConcerns.Logging;
    using Core.CrossCuttingConcerns.Logging.SeriLog.Logger;
    using Core.DataAccess.Paging;
    using Microsoft.AspNetCore.Mvc;

    namespace WebAPI.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class TasksController : ControllerBase
        {
            ITaskService _taskService;

            public TasksController(ITaskService taskService)
            {
                _taskService = taskService;
            }


            [Logging(typeof(MsSqlLogger))]
            [Logging(typeof(FileLogger))]
            [Cache(60)]
            [HttpGet("GetList")]
            public async Task<IActionResult> GetListAsync([FromQuery] PageRequest pageRequest)
            {
                var result = await _taskService.GetListAsync(pageRequest);
                return Ok(result);
            }


            [Logging(typeof(MsSqlLogger))]
            [Logging(typeof(FileLogger))]
            [Cache]
            [HttpGet("GetById")]
            public async Task<IActionResult> GetByIdAsync(Guid id)
            {
                var result = await _taskService.GetByIdAsync(id);
                return Ok(result);
            }


            [Logging(typeof(MsSqlLogger))]
            [Logging(typeof(FileLogger))]
            [CacheRemove("Tasks.Get")]
            [HttpPost("Add")]
            public async Task<IActionResult> AddAsync([FromBody] CreateTaskRequest createTaskRequest)
            {
                var result = await _taskService.AddAsync(createTaskRequest);
                return Ok(result);
            }


            [Logging(typeof(MsSqlLogger))]
            [Logging(typeof(FileLogger))]
            [CacheRemove("Tasks.Get")]
            [HttpPost("Update")]
            public async Task<IActionResult> UpdateAsync([FromBody] UpdateTaskRequest updateTaskRequest)
            {
                var result = await _taskService.UpdateAsync(updateTaskRequest);
                return Ok(result);
            }

            [Logging(typeof(MsSqlLogger))]
            [Logging(typeof(FileLogger))]
            [CacheRemove("Tasks.Get")]
            [HttpPost("Delete")]
            public async Task<IActionResult> DeleteAsync([FromBody] Guid id)
            {
                var result = await _taskService.DeleteAsync(id);
                return Ok(result);
            }
        }

    }

}
