using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QAP4.Domain;
using QAP4.Models;

namespace QAP4.Infrastructure.Extensions.File
{
    public interface IAmazonS3Service
    {
        Task<FileUploadModel> UploadObject(string bucket, IFormFile file);
        Task<FileUploadModel> RemoveObject(string bucket, String fileName);
        string GeneratePreSignedURL(string bucket, string fileName, int expiresDay);
    }
}
