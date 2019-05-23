using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QAP4.Extensions
{
    public interface IAmazonS3Service
    {
        Task<UploadPhotoModel> UploadObject(string bucket, IFormFile file);
        Task<UploadPhotoModel> RemoveObject(string bucket, String fileName);
        string GeneratePreSignedURL(string bucket, string fileName);
    }
}
