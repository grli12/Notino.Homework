using Homework.Services.Converts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Homework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertController : ControllerBase
    {
        private readonly IConvertService convertService;

        public ConvertController(IConvertService convertService)
        {
            this.convertService = convertService;
        }

        [HttpPost]
        public async Task<IActionResult> ConvertFile([Required]IFormFile formFile, [FromQuery][Required]string targetPath)
        {
            string fileExtension = Path.GetExtension(formFile.FileName);
            string targetExtension = Path.GetExtension(targetPath);
            
            if(formFile.Length > 0)
            {
                using(var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);
                    byte[] fileData = memoryStream.ToArray();

                    string pathToFile =
                        await this.convertService.ConvertAsync(fileExtension, targetExtension, fileData, targetPath);

                    return Ok();
                }
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
