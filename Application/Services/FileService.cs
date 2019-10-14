using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using QAP4.Application.DataTransferObjects;
using QAP4.Infrastructure.Extensions.File;

namespace QAP4.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IMapper _mapper;
        private readonly IAmazonS3Service _amazonS3Service;

        public FileService(
             IMapper mapper,
             IAmazonS3Service amazonS3Service)
        {
            _mapper = mapper;
            _amazonS3Service = amazonS3Service;
        }

        /// <summary>
        /// UploadFileToS3
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<FileDto> UploadFileToS3(string bucketName, IFormFile file)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
