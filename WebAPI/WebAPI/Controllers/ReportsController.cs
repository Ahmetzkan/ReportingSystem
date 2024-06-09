using Business.Abstracts;
using Business.Dtos.Requests.ReportRequests;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Logging.SeriLog.Logger;
using Core.CrossCuttingConcerns.Logging;
using Core.DataAccess.Paging;
using Microsoft.AspNetCore.Mvc;
using Core.CrossCuttingConcerns.Authorization;
using Core.CrossCuttingConcerns.Logging.SeriLog;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        IReportService _ReportService;
        LoggerServiceBase _LoggerService;

        public ReportsController(IReportService ReportService , IHttpContextAccessor httpContextAccessor)
        {
            _ReportService = ReportService;
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [Cache(60)]
        [HttpGet("GetList")]
        public async Task<IActionResult> GetListAsync([FromQuery] PageRequest pageRequest)
        {
            var result = await _ReportService.GetListAsync(pageRequest);
            return Ok(result);
        }


        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [Cache]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _ReportService.GetByIdAsync(id);
            
            return Ok(result);
        }


        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [CacheRemove("Reports.Get")]
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] CreateReportRequest createReportRequest)
        {
            var result = await _ReportService.AddAsync(createReportRequest);
            return Ok(result);
        }


        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [CacheRemove("Reports.Get")]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateReportRequest updateReportRequest)
        {
            var result = await _ReportService.UpdateAsync(updateReportRequest);
            return Ok(result);
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [CacheRemove("Reports.Get")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteReportRequest deleteReportRequest)
        {
            var result = await _ReportService.DeleteAsync(deleteReportRequest);
            return Ok(result);
        }
    }
}