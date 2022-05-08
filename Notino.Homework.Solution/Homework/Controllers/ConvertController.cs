using Homework.Services.Converts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult Get()
        {
            this.convertService.Convert(".xml", ".json");

            return Ok();
        }
    }
}
