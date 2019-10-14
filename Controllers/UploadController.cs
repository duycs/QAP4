using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QAP4.Application.DataTransferObjects;
using QAP4.Application.Services;
using QAP4.Infrastructure.Extensions;
using QAP4.Infrastructure.Extensions.File;

namespace QAP4.Controllers
{

    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UploadController(
          IFileService fileService,
           IConfiguration configuration,
           IMapper mapper
            )
        {
            _fileService = fileService;
            _configuration = configuration;
            _mapper = mapper;
        }


        /// <summary>
        /// POST: /api/UploadImageToS3
        /// </summary>
        /// <returns></returns>
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

                string bucketName = _configuration.GetSection("AWSS3:BucketPostStudyImage").Value;
                var fileDto = await _fileService.UploadFileToS3(bucketName, file);

                if (fileDto == null)
                    return StatusCode(500, "error when upload file");

                return Ok(fileDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// POST: /api/UploadEbookToS3
        /// </summary>
        /// <returns></returns>
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
                var fileDto = await _fileService.UploadFileToS3(bucketName, file);

                if (fileDto == null)
                    return StatusCode(500, "error when upload file");

                return Ok(fileDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}