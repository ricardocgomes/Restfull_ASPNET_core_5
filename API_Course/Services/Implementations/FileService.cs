using Microsoft.AspNetCore.Http;
using Mvc.Data.VO;
using Mvc.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mvc.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _httpContext;

        public FileService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
            _basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
        }

        public byte[] GetFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<List<FileDetailsVO>> SaveFilesToDisk(IList<IFormFile> files)
        {
            throw new NotImplementedException();
        }

        public async Task<FileDetailsVO> SaveFileToDisk(IFormFile file)
        {
            FileDetailsVO fileDetailsVO = new();

            var fileType = Path.GetExtension(file.FileName);
            var baseUrl = _httpContext.HttpContext.Request.Host;

            if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
            {
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", docName);
                    fileDetailsVO.DocumentName = docName;
                    fileDetailsVO.DocType = fileType;
                    fileDetailsVO.DocUrl = Path.Combine(baseUrl + "/api/file/v1" + fileDetailsVO.DocumentName);

                    using var stream = new FileStream(destination, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }

            return fileDetailsVO;
        }
    }
}
