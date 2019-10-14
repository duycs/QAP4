using Microsoft.AspNetCore.Http;
using QAP4.Application.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Application.Services
{
    public interface IFileService
    {
        Task<FileDto> UploadFileToS3(string bucketName, IFormFile file);
    }
}
