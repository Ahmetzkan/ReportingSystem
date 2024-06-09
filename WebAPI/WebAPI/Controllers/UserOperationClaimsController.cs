using Business.Abstracts;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Logging.SeriLog.Logger;
using Core.CrossCuttingConcerns.Logging;
using Core.DataAccess.Paging;
using Microsoft.AspNetCore.Mvc;
using Business.Dtos.Requests.OperationClaimRequests;
using Business.Rules.ValidationRules.FluentValidation.OperationClaimValidators;
using Business.Dtos.Requests.UserOperationClaimRequests;
using Core.CrossCuttingConcerns.Validation;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOperationClaimsController : ControllerBase
    {

        IUserOperationClaimService _userOperationClaimService;

        public UserOperationClaimsController(IUserOperationClaimService userOperationClaimService)
        {
            _userOperationClaimService = userOperationClaimService;
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [Cache(60)]
        [HttpGet("GetList")]
        public async Task<IActionResult> GetListAsync([FromQuery] PageRequest pageRequest)
        {
            var result = await _userOperationClaimService.GetListAsync(pageRequest);
            return Ok(result);
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [CacheRemove("UserOperationClaims.Get")]
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] CreateUserOperationClaimRequest createUserOperationClaimRequest)
        {
            var result = await _userOperationClaimService.AddAsync(createUserOperationClaimRequest);
            return Ok(result);
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [CacheRemove("UserOperationClaims.Get")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] Guid id)
        {
            var result = await _userOperationClaimService.DeleteAsync(id);
            return Ok(result);
        }

        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [CacheRemove("UserOperationClaims.Get")]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserOperationClaimRequest updateUserOperationClaimRequest)
        {
            var result = await _userOperationClaimService.UpdateAsync(updateUserOperationClaimRequest);
            return Ok(result);
        }



        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [Cache(60)]
        [HttpGet("GetByUserIdAndOperationClaimId")]
        public async Task<IActionResult> GetByUserIdAndOperationClaimId(Guid userId, Guid operationClaimId)
        {
            var result = await _userOperationClaimService.GetByUserIdAndOperationClaimId(userId, operationClaimId);
            return Ok(result);
        }


        [Logging(typeof(MsSqlLogger))]
        [Logging(typeof(FileLogger))]
        [Cache(60)]
        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId(Guid userId )
        {
            var result = await _userOperationClaimService.GetByUserId(userId);
            return Ok(result);
        }
        
    }
}
