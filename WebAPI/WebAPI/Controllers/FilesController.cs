using Core.Utilities.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileHelper _fileHelper;

        public FileController(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _fileHelper.Add(file);
            return Ok(new { FilePath = result });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(IFormFile file, string oldFilePath)
        {
            await _fileHelper.Update(file, oldFilePath);
            return NoContent();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string filePath)
        {
            await _fileHelper.Delete(filePath);
            return NoContent();
        }
    }

}
