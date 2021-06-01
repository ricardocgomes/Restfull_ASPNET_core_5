using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mvc.Data.VO;
using Mvc.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPut("UploadFile")]
        [ProducesResponseType(200, Type = typeof(FileDetailsVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadOneFile([FromForm] IFormFile file)
        {
            FileDetailsVO fileDetails = await _fileService.SaveFileToDisk(file);
            return new OkObjectResult(fileDetails);
        }

        [HttpPut("UploadMultiFile")]
        [ProducesResponseType(200, Type = typeof(List<FileDetailsVO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadMultiFile([FromForm] List<IFormFile> files)
        {
            List<FileDetailsVO> fileDetails = await _fileService.SaveFilesToDisk(files);
            return new OkObjectResult(fileDetails);
        }

        [HttpGet("DownloadFile/{fileName}")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            byte[] buffer = await _fileService.GetFile(fileName);
            if (buffer == null)
            {
                return NotFound("File not found.");
            }

            HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
            HttpContext.Response.Headers.Add("content-length", buffer.Length.ToString());
            await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);

            return new ContentResult();
        }
    }
}
