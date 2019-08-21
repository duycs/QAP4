using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QAP4.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using QAP4.Infrastructure.Extensions.File;

namespace QAP4.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostsRepository _postsRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IPostsTagRepository _postsTagRepository;

        private readonly IConfiguration _configuration;
        private readonly IAmazonS3Service _amazonS3Service;

        public UsersController(
            IConfiguration configuration,
            IAmazonS3Service amazonS3Service,
            IPostsRepository postsRepository,
            ITagRepository tagRepository,
            IPostsTagRepository postsTagRepository,
            IUserRepository userRepository)
        {
            _configuration = configuration;
            _amazonS3Service = amazonS3Service;
            _postsRepository = postsRepository;
            _tagRepository = tagRepository;
            _postsTagRepository = postsTagRepository;
            _userRepository = userRepository;
        }


        /// <summary>
        /// GET: /api/users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _userRepository.GetAll();
                if (users == null || !users.Any())
                    return NoContent();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// GET: /api/users/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest();

                var user = _userRepository.GetById(id);
                if (user == null)
                    return NoContent();

                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// POST: /api/users/{id}/avatar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id:int}/avatar")]
        public async Task<IActionResult> UpdateUserAvatar(int id)
        {
            try
            {
                var file = this.Request.Form.Files[0];
                var imageUrl = string.Empty;

                // File type validation
                if (!file.ContentType.Contains("image"))
                {
                    return StatusCode(500, "image file not found");
                }

                var bucket = _configuration.GetSection("AWSS3:BucketPostStudy").Value;
                var imageResponse = await _amazonS3Service.UploadObject(bucket, file);

                if (!imageResponse.Success)
                {
                    return StatusCode(500, "can not upload image to S3");
                }
                else
                {
                    //imageUrl = AmazonS3Service.GeneratePreSignedURL(bucket, imageResponse.FileName);
                    imageUrl = "https://s3-ap-southeast-1.amazonaws.com" + "/" + bucket + "/" + imageResponse.FileName;
                }

                if (string.IsNullOrEmpty(imageUrl))
                {
                    return BadRequest();
                }

                var user = _userRepository.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }
                user.Avatar = imageUrl;
                _userRepository.Update(user);

                //update userAvatar in posts
                UpdateUserAvatarInPosts(user.Id, imageUrl);

                var response = new
                {
                    Success = true,
                    ImageUrl = imageUrl
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// POST: /api/users/{id}/banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id:int}/banner")]
        public async Task<IActionResult> UpdateUserBanner(int id)
        {
            try
            {
                var file = this.Request.Form.Files[0];
                var imageUrl = string.Empty;

                // File type validation
                if (!file.ContentType.Contains("image"))
                {
                    return StatusCode(500, "image file not found");
                }

                var bucket = _configuration.GetSection("AWSS3:BucketPostStudy").Value;
                var imageResponse = await _amazonS3Service.UploadObject(bucket, file);
                if (!imageResponse.Success)
                {
                    return StatusCode(500, "can not upload image to S3");
                }
                else
                {
                    //imageUrl = AmazonS3Service.GeneratePreSignedURL(bucket, imageResponse.FileName);
                    imageUrl = "https://s3-ap-southeast-1.amazonaws.com" + "/" + bucket + "/" + imageResponse.FileName;
                }

                if (string.IsNullOrEmpty(imageUrl))
                {
                    return BadRequest();
                }

                var user = _userRepository.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }
                user.BannerImg = imageUrl;
                _userRepository.Update(user);

                var response = new
                {
                    Success = true,
                    ImageUrl = imageUrl
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// DELETE: /api/users/{id}
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest();

                _userRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        private void UpdateUserAvatarInPosts(int userId, string userAvatar)
        {
            try
            {
                var postsByOwnerId = _postsRepository.GetPostsByOwnerUserId(0, userId);
                if (!postsByOwnerId.Any()) return;

                foreach (var posts in postsByOwnerId)
                {
                    posts.UserAvatar = userAvatar;
                    _postsRepository.Update(posts);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
