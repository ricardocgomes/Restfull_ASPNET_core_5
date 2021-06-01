using Microsoft.AspNetCore.Http;
using Mvc.Data.VO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mvc.Services.Interfaces
{
    public interface IFileService
    {
        public byte[] GetFile(string fileName);
        public Task<FileDetailsVO> SaveFileToDisk(IFormFile file);
        public Task<List<FileDetailsVO>> SaveFilesToDisk(IList<IFormFile> files);
    }
}
