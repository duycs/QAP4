using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QAP4.Extensions;
using Microsoft.AspNetCore.Http;
using QAP4.ViewModels;
using QAP4.Infrastructure.Repositories;
using System.Collections.Generic;
using QAP4.Domain.AggreatesModels.Posts.Models;
using System.Threading.Tasks;
using QAP4.Infrastructure.Extensions.File;
using Microsoft.Extensions.Configuration;
using MediatR;
using QAP4.Domain.Core.Commands;
using QAP4.Domain.Core.Notifications;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : ApiController
    {
        private readonly INotificationHandler<DomainNotification> _notificationHandler;
        private readonly ICommandDispatcher _commandBusHandler;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly IPostsService _postsService;
        private readonly IUserService _userService;

        //private readonly IPostsRepository _postsRepository;
        //private readonly ITagRepository _tagRepository;
        //private readonly IPostsTagRepository _postsTagRepository;
        //private readonly IUserRepository _userRepository;
        //private readonly IPostsLinkRepository _postLinkRepository;

        public PostsController(
            INotificationHandler<DomainNotification> notificationHandler,
            ICommandDispatcher commandBusHandler,
            IConfiguration configuration,
            IFileService fileService,
            IPostsService postsService,
            IUserService userService

            //IPostsRepository postsRepository,
            //ITagRepository tagRepository,
            //IPostsTagRepository postsTagRepository,
            //IUserRepository userRepository,
            //IPostsLinkRepository postLinkRepository
            )
            : base(notificationHandler, commandBusHandler)
        {
            _notificationHandler = notificationHandler;
            _commandBusHandler = commandBusHandler;
            _configuration = configuration;
            _fileService = fileService;
            _postsService = postsService;
            _userService = userService;

            //_postsRepository = postsRepository;
            //_tagRepository = tagRepository;
            //_postsTagRepository = postsTagRepository;
            //_userRepository = userRepository;
            //_postLinkRepository = postLinkRepository;
        }


        /// <summary>
        /// route: /api/posts?
        /// Get posts listing by conditions
        /// </summary>
        /// <param name="pg">page</param>
        /// <param name="or_b">order_branch</param>
        /// <param name="u_i">userId</param>
        /// <param name="po_t">postsType</param>
        /// <param name="pr_i">parentId</param>
        /// <returns>posts listing</returns>
        [HttpGet]
        [Route("")]
        public IActionResult FindPostsByUserAndType([FromQuery]int pg, [FromQuery]int pageSize,
            [FromQuery]DateTime? dateFrom, [FromQuery]DateTime? dateTo,
            [FromQuery]string or_b, [FromQuery]int u_i,
            [FromQuery]int po_t, [FromQuery]int pr_i)
        {
            try
            {
                var pageIndex = pg;
                var orderBy = or_b;
                var userId = u_i;
                var postsType = po_t;
                var parentId = pr_i;

                // Get posts listing by complex conditions
                var postsDtos = _postsService.FindPostsByUserAndType(userId, postsType, parentId, dateFrom, dateTo, pageIndex, pageSize);
                if (postsDtos == null)
                    return NoContent();

                return Ok(postsDtos);
                //return _postsRepository.GetPosts(pg, or_b, u_i, po_t, pr_i);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// route: /api/posts/{id}
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>posts</returns>
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult FindPostsById(int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest();

                var posts = _postsService.FindPostsById(id);
                if (posts == null)
                    return NoContent();

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// route: /api/posts
        /// Add posts
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("")]
        public IActionResult AddPosts(Posts posts)
        {
            try
            {
                if (posts == null)
                    return BadRequest();

                // Get this url then set to session
                var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                HttpContext.Session.SetString("thisUrl", thisUrl);

                // Get user by session
                var userId = (int)HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
                var user = _userService.FindUserById(userId);

                // May be user loss session, waring user to login again
                if (null == user)
                    return Ok(new MessageView(AppConstants.Warning.WAR_2003));

                _postsService.AddPosts(user.Id, posts);

                return Ok(new MessageView(posts.Id, AppConstants.Message.MSG_1000));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// route: /api/posts/{id}
        /// Update posts
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public IActionResult UpdatePosts(int id, Posts posts)
        {
            try
            {
                // Get this url then set to session
                var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                HttpContext.Session.SetString("thisUrl", thisUrl);

                // Get user by session
                var userId = (int)HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
                var user = _userService.FindUserById(userId);

                // May be user loss session, waring user to login again
                if (null == user)
                    return Ok(new MessageView(AppConstants.Warning.WAR_2003));


                // Update posts
                _postsService.UpdatePosts(user.Id, posts);

                // Update tag relations with posts
                

                return Ok(new MessageView(posts.Id, AppConstants.Message.MSG_1000));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        ///// <summary>
        ///// route: /api/posts
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("{id:int}")]
        //public ActionResult InsertOrUpdate(int id, Posts model)
        //{
        //    try
        //    {
        //        if (null == model)
        //            return BadRequest();

        //        var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        //        HttpContext.Session.SetString("thisUrl", thisUrl);

        //        var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
        //        var user = _userRepository.GetById(userId);
        //        int postTypeId = (int)model.PostTypeId;
        //        if (null == user)
        //            return Ok(new MessageView(AppConstants.Warning.WAR_2003));

        //        var posts = new Posts();

        //        if (id == 0)
        //        {
        //            //related posts or chirld posts
        //            List<string> relatedPosts = new List<string>();
        //            if (!string.IsNullOrEmpty(posts.RelatedPosts))
        //            {
        //                relatedPosts = posts.RelatedPosts.Split(',').ToList();
        //            }

        //            //insert
        //            var dateTime = DateTime.Now;
        //            posts = model;

        //            // edit some infor posts for correct
        //            posts.OwnerUserId = userId;
        //            posts.UserDisplayName = user.DisplayName;
        //            posts.UserAvatar = user.Avatar;
        //            posts.CreationDate = dateTime;
        //            posts.LastEditDate = dateTime;
        //            posts.AnswerCount = 0;
        //            posts.ViewCount = 0;
        //            posts.VoteCount = 0;
        //            posts.Score = 0;
        //            posts.CommentCount = 0;


        //            if (AppConstants.PostsType.POSTS.Equals(postTypeId))
        //            {
        //                if (string.IsNullOrEmpty(model.HtmlContent))
        //                {
        //                    return Ok(new MessageView(id, AppConstants.Error.ERR_3003));
        //                }
        //                posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
        //                posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);

        //            }
        //            else if (AppConstants.PostsType.QUESTION.Equals(postTypeId))
        //            {
        //                if (!string.IsNullOrEmpty(model.HtmlContent))
        //                {
        //                    posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
        //                    posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
        //                }
        //            }
        //            else if (AppConstants.PostsType.TUTORIAL.Equals(postTypeId))
        //            {
        //                if (!string.IsNullOrEmpty(model.HtmlContent))
        //                {
        //                    posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
        //                    posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
        //                }
        //            }
        //            //if posts answer, update answer count of parent posts
        //            else if (AppConstants.PostsType.ANSWER.Equals(postTypeId))
        //            {
        //                var postsParent = _postsRepository.GetPosts(model.ParentId);
        //                if (postsParent != null)
        //                {
        //                    var answerCount = postsParent.AnswerCount;
        //                    answerCount++;
        //                    postsParent.AnswerCount = answerCount;
        //                    _postsRepository.Update(postsParent);
        //                }
        //            }
        //            else if (postTypeId != 0)
        //            {

        //                if (string.IsNullOrEmpty(posts.RelatedPosts))
        //                {
        //                    return Ok(new MessageView(id, AppConstants.Error.ERR_3003));
        //                }

        //                relatedPosts = posts.RelatedPosts.Split(',').ToList();

        //                // handler same as handler tag
        //                UpdatePostLinks(id, relatedPosts, postTypeId);
        //            }

        //            // add  posts
        //            id = _postsRepository.Add(posts);

        //            //handle update parentId in related posts
        //            if (relatedPosts.Any())
        //            {
        //                UpdateParentIdChirldPosts(id, relatedPosts);
        //            }

        //            // handler tag
        //            if (!string.IsNullOrEmpty(posts.Tags))
        //            {
        //                string[] tags = posts.Tags.Split(',');
        //                UpdatePostsTags(userId, id, tags);
        //            }

        //            if (!AppConstants.PostsType.ANSWER.Equals(postTypeId) && !AppConstants.PostsType.POSTS.Equals(postTypeId))
        //            {
        //                // handler same as handler tag
        //                UpdatePostLinks(id, relatedPosts, postTypeId);

        //                // create table of content 
        //                string tableOfcontent = GetTableOfContent(posts.Id, relatedPosts);
        //                posts.TableContent = tableOfcontent;
        //                _postsRepository.Update(posts);
        //            }

        //        }
        //        else
        //        {
        //            //update
        //            posts = _postsRepository.GetPosts(id);
        //            if (null != posts)
        //            {
        //                //var oldPosts = posts;
        //                string[] oldTags = new string[0];
        //                string[] newTags = new string[0];
        //                if (!string.IsNullOrEmpty(posts.Tags))
        //                {
        //                    oldTags = posts.Tags.Split(',');
        //                }
        //                if (!string.IsNullOrEmpty(model.Tags))
        //                {
        //                    newTags = model.Tags.Split(',');
        //                }

        //                //can't use posts = model;
        //                //use copy object value
        //                //posts = (Posts)ReflectionExtensions.CopyObjectValue(model, posts);
        //                posts.Title = model.Title;
        //                if (!string.IsNullOrEmpty(model.UserAvatar))
        //                    posts.UserAvatar = model.UserAvatar;
        //                posts.HtmlContent = model.HtmlContent;
        //                posts.Description = model.Description;
        //                posts.Tags = model.Tags;
        //                posts.RelatedPosts = model.RelatedPosts;
        //                posts.TableContent = model.TableContent;
        //                posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
        //                posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
        //                posts.LastEditDate = DateTime.Now;
        //                posts.CoverImg = model.CoverImg;

        //                if (model.PostTypeId != 0 && !string.IsNullOrEmpty(posts.RelatedPosts))
        //                {
        //                    // handler create notebook same as handler tag
        //                    List<string> relatedPosts = posts.RelatedPosts.Split(',').ToList();
        //                    UpdatePostLinks(id, relatedPosts, (int)model.PostTypeId);

        //                    // create table of content 
        //                    string tableOfcontent = GetTableOfContent(posts.Id, relatedPosts);
        //                    posts.TableContent = tableOfcontent;
        //                }

        //                id = _postsRepository.Update(posts);

        //                //handler tag
        //                //if tag change
        //                if (!oldTags.Equals(newTags))
        //                {
        //                    //check tags add new and tags delete in table PostsTag
        //                    string[] tagsDiff = GetTagsDiff(oldTags, newTags);
        //                    UpdatePostsTags(userId, id, tagsDiff);
        //                }
        //            }
        //        }
        //        return Ok(new MessageView(id, AppConstants.Message.MSG_1000));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }

        //}

        /// <summary>
        /// route: /posts/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult RemovePostsById(int id)
        {
            try
            {
                if (id < 0)
                    return BadRequest();

                var posts = _postsService.FindPostsById(id);

                if (posts == null)
                    return NoContent();

                _postsService.RemovePosts(posts);

                return Ok(new MessageView(id, AppConstants.Message.MSG_1000));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// action: /api/posts/{id}/active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}/active")]
        public ActionResult Active(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var posts = _postsService.FindPostsById(id);

                if (posts == null)
                    return NoContent();

                // If have active date is meaning this posts be actives, otherwise
                var activeTime = DateTime.UtcNow;
                posts.LastActivityDate = activeTime;
                posts.LastEditDate = activeTime;

                _postsService.UpdatePosts(posts);

                return Ok(new MessageView(posts.Id, AppConstants.Message.MSG_1000));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// action: /api/posts/{id}/deActive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}/deActive")]
        public ActionResult DeActive(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var posts = _postsService.FindPostsById(id);

                if (posts == null)
                    return NoContent();

                // If have active date is meaning this posts be actives, otherwise
                posts.LastActivityDate = null;
                posts.LastEditDate = DateTime.UtcNow;

                _postsService.UpdatePosts(posts);

                return Ok(new MessageView(posts.Id, AppConstants.Message.MSG_1000));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        ///// <summary>
        ///// route: /api/posts/exportBook
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("exportBook")]
        //public IEnumerable<Book> ExportBook()
        //{
        //    return _postsRepository.GetAll().Where(w => w.DeletionDate == null && w.PostTypeId != 2 && w.PostTypeId != 3).Select(s => new Book
        //    {
        //        Uid = s.Id,
        //        Id = 0,
        //        Isbn = 0,
        //        CoverImage = s.CoverImg,
        //        Description = s.HeadContent,
        //        Title = s.Title,
        //        Subject = null,
        //        Publisher = null,
        //        Language = null,
        //        PageNumber = 0,
        //    }).ToList();
        //}

        /// <summary>
        /// TODO: route
        /// POST: /images
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("api/images/posts/{id:int}/coverimg")]
        public async Task<IActionResult> UploadCoverPosts(int id)
        {
            try
            {
                var posts = _postsService.FindPostsById(id);
                if (posts == null)
                    return NotFound();

                var file = this.Request.Form.Files[0];
                var imageUrl = string.Empty;

                // File type validation
                if (!file.ContentType.Contains("image"))
                    return StatusCode(500, "image file not found");

                var bucket = _configuration.GetSection("AWSS3:BucketPostStudy").Value;
                var imageResponse = await _fileService.UploadFileToS3(bucket, file);

                if (!imageResponse.Success)
                    return StatusCode(500, "can not upload image to S3");

                //imageUrl = AmazonS3Service.GeneratePreSignedURL(bucket, imageResponse.FileName);
                imageUrl = "https://s3-ap-southeast-1.amazonaws.com" + "/" + bucket + "/" + imageResponse.FileName;

                posts.CoverImg = imageUrl;

                _postsService.UpdatePosts(posts);

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


    }

    //public class Book
    //{
    //    public int Uid { get; set; }
    //    public int Id { get; set; }
    //    public int Isbn { get; set; }
    //    public string CoverImage { get; set; }
    //    public string Description { get; set; }
    //    public string Title { get; set; }
    //    public string Subject { get; set; }
    //    public string Publisher { get; set; }
    //    public string Language { get; set; }
    //    public int PageNumber { get; set; }
    //}

}
