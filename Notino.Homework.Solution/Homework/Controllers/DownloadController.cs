using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Homework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        [HttpGet]
        public IActionResult Download([FromQuery]string sourcePath)
        {
            return PhysicalFile(sourcePath, MimeTypes.GetMimeType(sourcePath), Path.GetFileName(sourcePath));
        }
    }
}
