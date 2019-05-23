
using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace QAP4.Extensions
{
    public class AmazonS3Service : IAmazonS3Service
    {
        private static String accessKey = "YOUR_ACCESS_KEY_ID";
        private static String accessSecret = "YOUR_SECRET_ACCESS_KEY";
        //private static String bucket = "YOUR_S3_BUCKET";
        private readonly IConfiguration Configuration;

        public AmazonS3Service(IConfiguration configuration)
        {
            Configuration = configuration;
            accessKey = Configuration.GetSection("AWSS3:AWSAccessKeyId").Value;
            accessSecret = Configuration.GetSection("AWSS3:AWSSecretKey").Value;
        }

        public async Task<UploadPhotoModel> UploadObject(string bucket, IFormFile file)
        {
            // connecting to the client
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.APSoutheast1);

            // get the file and convert it to the byte[]
            byte[] fileBytes = new Byte[file.Length];
            file.OpenReadStream().Read(fileBytes, 0, Int32.Parse(file.Length.ToString()));

            // create unique file name for prevent the mess
            var fileName = Guid.NewGuid() + file.FileName;

            PutObjectResponse response = null;

            using (var stream = new MemoryStream(fileBytes))
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                response = await client.PutObjectAsync(request);
            };

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                // this model is up to you, in my case I have to use it following;
                return new UploadPhotoModel
                {
                    Success = true,
                    FileName = fileName
                };
            }
            else
            {
                // this model is up to you, in my case I have to use it following;
                return new UploadPhotoModel
                {
                    Success = false,
                    FileName = fileName
                };
            }
        }

        public async Task<UploadPhotoModel> RemoveObject(string bucket, String fileName)
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
                return new UploadPhotoModel
                {
                    Success = true,
                    FileName = fileName
                };
            }
            else
            {
                return new UploadPhotoModel
                {
                    Success = false,
                    FileName = fileName
                };
            }
        }

        public string GeneratePreSignedURL(string bucket, string fileName)
        {
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.APSoutheast1);
            string urlString = "";
            try
            {
                GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
                {
                    BucketName = bucket,
                    Key = fileName,
                    Expires = DateTime.Now.AddMinutes(60)
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
    }

    public class UploadPhotoModel
    {
        public bool Success { get; set; }
        public string FileName { get; set; }
    }
}
