
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QAP4.Domain;
using QAP4.Models;

namespace QAP4.Infrastructure.Extensions.File
{
    public class AmazonS3Service : IAmazonS3Service
    {
        private static string accessKey = "YOUR_ACCESS_KEY_ID";
        private static string accessSecret = "YOUR_SECRET_ACCESS_KEY";
        private static string signedURL = "SIGNED_URL";

        private readonly IConfiguration _configuration;

        public AmazonS3Service(IConfiguration configuration)
        {
            _configuration = configuration;
            accessKey = _configuration.GetSection("AWSS3:AWSAccessKeyId").Value;
            accessSecret = _configuration.GetSection("AWSS3:AWSSecretKey").Value;
            signedURL = _configuration.GetSection("AWSS3:BucketSignedURL").Value;

        }

        public async Task<FileUploadModel> UploadObject(string bucket, IFormFile file)
        {
            // connecting to the client
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.APSoutheast1);

            // get the file and convert it to the byte[]
            byte[] fileBytes = new Byte[file.Length];
            file.OpenReadStream().Read(fileBytes, 0, Int32.Parse(file.Length.ToString()));

            // create unique file name for prevent the mess
            string fileName = file.FileName.Trim();
            string fileExtension = Path.GetExtension(fileName);
            string fileNameWhithoutSpace = ReplaceAllSpaces(fileName, "+");

            var unique = Guid.NewGuid();
            string fileNameUnique = string.Format("{0}_{1}", unique, fileName);

            //get Object URL
            string fileNameFakeRealLUrl = string.Format("{0}_{1}", unique, fileNameWhithoutSpace);
            string url = string.Format("{0}/{1}/{2}", signedURL, bucket, fileNameFakeRealLUrl);

            PutObjectResponse response = null;

            using (var stream = new MemoryStream(fileBytes))
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = fileNameUnique,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                response = await client.PutObjectAsync(request);
            };

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                //get pre signed url
                string preSignedUrl = GeneratePreSignedURL(bucket, fileName, 7);

                //upload sucess
                return new FileUploadModel
                {
                    Success = true,
                    Url = url,
                    PreSignedURL = preSignedUrl,
                    FileName = fileName,
                    FileExtension = fileExtension
                };


            }
            else
            {
                //error
                return new FileUploadModel
                {
                    Success = false,
                    FileName = fileName,
                    FileExtension = fileExtension
                };
            }
        }

        public async Task<FileUploadModel> RemoveObject(string bucket, String fileName)
        {
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.APSoutheast1);

            var request = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = fileName
            };

            var response = await client.DeleteObjectAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return new FileUploadModel
                {
                    Success = true,
                    FileName = fileName
                };
            }
            else
            {
                return new FileUploadModel
                {
                    Success = false,
                    FileName = fileName
                };
            }
        }

        public string GeneratePreSignedURL(string bucket, string fileName, int expiresDay)
        {
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.APSoutheast1);
            string urlString = "";
            try
            {
                GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
                {
                    BucketName = bucket,
                    Key = fileName,
                    Expires = DateTime.Now.AddDays(expiresDay)
                };
                urlString = client.GetPreSignedURL(request1);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            return urlString;
        }

        private string ReplaceAllSpaces(string str, string charReplace)
        {
            return Regex.Replace(str, @"\s+", charReplace);
        }

        Task<FileUploadModel> IAmazonS3Service.UploadObject(string bucket, IFormFile file)
        {
            throw new NotImplementedException();
        }

        Task<FileUploadModel> IAmazonS3Service.RemoveObject(string bucket, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
