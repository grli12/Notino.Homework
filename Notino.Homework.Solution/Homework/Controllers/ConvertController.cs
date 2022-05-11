using Homework.Services.Converts;
using Homework.Services.Converts.Exceptions;
using Homework.Services.Emails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Homework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertController : ControllerBase
    {
        private readonly IConvertService convertService;
        private readonly IEmailService emailService;

        public ConvertController(IConvertService convertService, IEmailService emailService)
        {
            this.convertService = convertService;
            this.emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> ConvertFile([Required]IFormFile formFile, [FromQuery][Required]string targetPath)
        {
            string fileExtension = Path.GetExtension(formFile.FileName);
            string targetExtension = Path.GetExtension(targetPath);

            using(var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                byte[] fileData = memoryStream.ToArray();

                try
                {
                    string pathToFile =
                    await this.convertService.ConvertAsync(fileExtension, targetExtension, fileData, targetPath);
                    //await this.emailService.SendEmail("some@address.com", pathToFile);

                    return Ok();
                }
                catch(ConvertValidationException convertValidationException)
                {
                    string message = GetInnerMessage(convertValidationException);

                    return BadRequest(message);
                }
                catch(UnsupportedConvertException unsupportedConvertException)
                {
                    string message = GetInnerMessage(unsupportedConvertException);

                    return BadRequest(message);
                }
                catch(ConvertedFileSaveFailedException convertedFileSaveFailedException)
                {
                    string message = GetInnerMessage(convertedFileSaveFailedException);

                    return BadRequest(message);
                }
                catch(ConvertFailedException convertFailedException)
                {
                    string message = GetInnerMessage(convertFailedException);

                    return BadRequest(message);
                }
                catch(Exception exception)
                {
                    return Problem(exception.Message);
                }
            }
        }

        //Len v rychlosti nastrel
        private async Task SendEmail(string to, string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            List<string> addresses = new List<string>();
            addresses.Add(to);
            AttachmentCollection attachments = new AttachmentCollection();
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            attachments.Add(fileName, bytes);
            await this.emailService.SendEmail(
                addresses,
                "converted file",
                "<p>Some body :) </p>",
                attachments
                );

        }

        private static string GetInnerMessage(Exception exception) =>
             exception.InnerException?.Message ?? exception.Message;

    }
}
