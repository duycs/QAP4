using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QAP4.Application.DataTransferObjects;
using QAP4.Infrastructure.Extensions;
using QAP4.Infrastructure.Extensions.File;

namespace QAP4.Controllers
{

    [Route("api/v1/[controller]")]
    public class UploadController: Controller
    {
        private readonly IAmazonS3Service _amazonS3Service;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UploadController(
          IAmazonS3Service amazonS3Service,
           IConfiguration configuration,
           IMapper mapper
            ) 
        {
            _amazonS3Service = amazonS3Service;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("UploadImageToS3")]
        public async Task<IActionResult> UploadImageFileToS3()
        {

            try
            {
                var file = this.Request.Form.Files[0];

                //check validate only image
                if (null == file || !file.ContentType.Contains("image"))
                {
                    return BadRequest();
                }

                string bucketName = _configuration.GetSection("AWSS3:BucketPostStudyLibraryImage").Value;
                var fileDto = await UploadFileToS3(bucketName, file);

                if (fileDto == null)
                    return StatusCode(500, "error when upload file");

                return Ok(fileDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("UploadEbookToS3")]
        public async Task<IActionResult> UploadEbookFileToS3()
        {

            try
            {
                var file = Request.Form.Files[0];

                // File type validation
                if (null == file)
                {
                    return BadRequest();
                }

                string bucketName = _configuration.GetSection("AWSS3:BucketPostStudyLibraryEbook").Value;
                var fileDto = await UploadFileToS3(bucketName, file);

                if (fileDto == null)
                    return StatusCode(500, "error when upload file");

                return Ok(fileDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<FileDto> UploadFileToS3(string bucketName, IFormFile file)
        {
            var response = await _amazonS3Service.UploadObject(bucketName, file);
            if (!response.Success)
            {
                return null;
            }

            var fileDto = _mapper.Map<FileDto>(response);

            if (fileDto == null)
                return null;

            return fileDto;
        }
    }
}