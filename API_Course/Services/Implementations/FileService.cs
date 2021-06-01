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

        public async Task<byte[]> GetFile(string fileName)
        {//
            var filePath = _basePath + fileName;

            if (File.Exists(filePath))
            {
                return await File.ReadAllBytesAsync(filePath);
            }
            return null;
        }

        public async Task<List<FileDetailsVO>> SaveFilesToDisk(IList<IFormFile> files)
        {
            List<FileDetailsVO> list = new();
            foreach (var item in files)
            {
                list.Add(await SaveFileToDisk(item));
            }
            return list;
        }

        public async Task<FileDetailsVO> SaveFileToDisk(IFormFile file)
        {
            FileDetailsVO fileDetailsVO = new();

            var fileType = Path.GetExtension(file.FileName.Trim());
            var baseUrl = _httpContext.HttpContext.Request.Host;

            if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
            {
                var docName = Path.GetFileName(file.FileName.Trim());
                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", docName);
                    fileDetailsVO.DocumentName = docName;
                    fileDetailsVO.DocType = fileType;
                    fileDetailsVO.DocUrl = Path.Combine(baseUrl + "/api/file/v1/" + fileDetailsVO.DocumentName.Trim());

                    using var stream = new FileStream(destination, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }

            return fileDetailsVO;
        }
    }
}
