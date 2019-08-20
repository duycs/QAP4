using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QAP4.Extensions;
using Microsoft.AspNetCore.Http;
using QAP4.Models;
using QAP4.ViewModels;
using QAP4.Infrastructure.Repositories;
using System.Collections.Generic;
using QAP4.Domain.AggreatesModels.Posts.Models;
using System.Threading.Tasks;
using QAP4.Infrastructure.Extensions.File;
using Microsoft.Extensions.Configuration;

namespace QAP4.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IPostsRepository _postsRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IPostsTagRepository _postsTagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostLinkRepository _postLinkRepository;

        private readonly IConfiguration _configuration;
        private readonly IAmazonS3Service _amazonS3Service;

        public PostsController(
             IConfiguration configuration,
             IAmazonS3Service amazonS3Service,
             IPostsRepository postsRepository,
             ITagRepository tagRepository,
             IPostsTagRepository postsTagRepository,
             IUserRepository userRepository,
             IPostLinkRepository postLinkRepository)
        {
            _configuration = configuration;
            _amazonS3Service = amazonS3Service;

            _postsRepository = postsRepository;
            _tagRepository = tagRepository;
            _postsTagRepository = postsTagRepository;
            _userRepository = userRepository;
            _postLinkRepository = postLinkRepository;
        }

        // GET: /posts?u=6&t=1&p=1
        // posts can be a posts normal (1), a question (2) or an answer (postType=3)

        /// <summary>
        /// route: /api/posts
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
        public IEnumerable<Posts> GetPosts([FromQuery]int pg, [FromQuery]string or_b, [FromQuery]int u_i, [FromQuery]int po_t, [FromQuery]int pr_i)
        {
            return _postsRepository.GetPosts(pg, or_b, u_i, po_t, pr_i);
        }


        /// <summary>
        /// route: /api/posts/{id}
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>posts</returns>
        [HttpGet]
        [Route("{id:int}")]
        public Posts Posts(int id)
        {
            return _postsRepository.GetPosts(id);
        }

        /// <summary>
        /// route: /api/posts
        /// Add posts
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("")]
        public IActionResult AddPosts(Posts viewModel)
        {

            if (null == viewModel)
                return BadRequest();

            // Get this url then set to session
            var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            HttpContext.Session.SetString("thisUrl", thisUrl);

            // Get user by session
            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var user = _userRepository.GetById(userId);

            // May be user loss session, waring user to login again
            if (null == user)
                return Json(new MessageView(AppConstants.Warning.WAR_2003));

            int postTypeId = (int)viewModel.PostTypeId;

            var posts = new Posts();

            // Related posts or chirld posts
            List<string> relatedPosts = new List<string>();
            if (!string.IsNullOrEmpty(posts.RelatedPosts))
            {
                relatedPosts = posts.RelatedPosts.Split(',').ToList();
            }

            // Set posts
            var dateTime = DateTime.Now;
            posts = viewModel;
            posts.OwnerUserId = userId;
            posts.UserDisplayName = user.DisplayName;
            posts.UserAvatar = user.Avatar;
            posts.CreationDate = dateTime;
            posts.LastEditDate = dateTime;
            posts.AnswerCount = 0;
            posts.ViewCount = 0;
            posts.VoteCount = 0;
            posts.Score = 0;
            posts.CommentCount = 0;


            if (PostsType.POSTS.Equals(postTypeId))
            {
                if (string.IsNullOrEmpty(posts.HtmlContent))
                {
                    return Json(new MessageView(AppConstants.Error.ERR_3003));
                }
                posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);

            }
            else if (PostsType.QUESTION.Equals(postTypeId))
            {
                if (!string.IsNullOrEmpty(posts.HtmlContent))
                {
                    posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                    posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
                }
            }
            else if (PostsType.TUTORIAL.Equals(postTypeId))
            {
                if (!string.IsNullOrEmpty(posts.HtmlContent))
                {
                    posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                    posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
                }
            }
            // If posts is answer, update answer count of parent posts
            else if (PostsType.ANSWER.Equals(postTypeId))
            {
                var postsParent = _postsRepository.GetPosts(posts.ParentId);
                if (postsParent != null)
                {
                    var answerCount = postsParent.AnswerCount;
                    answerCount++;
                    postsParent.AnswerCount = answerCount;
                    _postsRepository.Update(postsParent);
                }
            }
            else if (postTypeId != 0)
            {

                if (string.IsNullOrEmpty(posts.RelatedPosts))
                {
                    return Json(new MessageView(AppConstants.Error.ERR_3003));
                }

                relatedPosts = posts.RelatedPosts.Split(',').ToList();

                // handler same as handler related tag
                UpdatePostLinks(0, relatedPosts, postTypeId);
            }

            // Add the posts
            var postsId = _postsRepository.Add(posts);

            // Handle update parentId in related posts
            if (relatedPosts.Any())
            {
                UpdateParentIdChirldPosts(postsId, relatedPosts);
            }

            // Handler tag
            if (!string.IsNullOrEmpty(posts.Tags))
            {
                string[] tags = posts.Tags.Split(',');
                UpdatePostsTags(userId, postsId, tags);
            }

            if (!PostsType.ANSWER.Equals(postTypeId) && !PostsType.POSTS.Equals(postTypeId))
            {
                // Handler same as handler related tag
                UpdatePostLinks(0, relatedPosts, postTypeId);

                // Create table of content 
                string tableOfcontent = GetTableOfContent(postsId, relatedPosts);
                posts.TableContent = tableOfcontent;
                _postsRepository.Update(posts);
            }

            return Ok(Json(new MessageView(postsId, AppConstants.Message.MSG_1000)));
        }

        /// <summary>
        /// route: /api/posts/{id}
        /// Update posts
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public IActionResult UpdatePosts(int id, Posts viewModel)
        {
            // Get this url then set to session
            var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            HttpContext.Session.SetString("thisUrl", thisUrl);

            // Get user by session
            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var user = _userRepository.GetById(userId);

            // May be user loss session, waring user to login again
            if (null == user)
                return Json(new MessageView(AppConstants.Warning.WAR_2003));

            var posts = _postsRepository.GetPosts(id);
            if (null == posts)
                return BadRequest();

            //var oldPosts = posts;
            string[] oldTags = new string[0];
            string[] newTags = new string[0];
            if (!string.IsNullOrEmpty(posts.Tags))
            {
                oldTags = posts.Tags.Split(',');
            }
            if (!string.IsNullOrEmpty(viewModel.Tags))
            {
                newTags = viewModel.Tags.Split(',');
            }

            //can't use posts = model;
            //use copy object value
            //posts = (Posts)ReflectionExtensions.CopyObjectValue(model, posts);
            posts.Title = viewModel.Title;
            if (!string.IsNullOrEmpty(viewModel.UserAvatar))
                posts.UserAvatar = viewModel.UserAvatar;
            posts.HtmlContent = viewModel.HtmlContent;
            posts.Description = viewModel.Description;
            posts.Tags = viewModel.Tags;
            posts.RelatedPosts = viewModel.RelatedPosts;
            posts.TableContent = viewModel.TableContent;
            posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
            posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
            posts.LastEditDate = DateTime.Now;
            posts.CoverImg = viewModel.CoverImg;

            if (viewModel.PostTypeId != 0 && !string.IsNullOrEmpty(posts.RelatedPosts))
            {
                // handler create notebook same as handler tag
                List<string> relatedPosts = posts.RelatedPosts.Split(',').ToList();
                UpdatePostLinks(id, relatedPosts, (int)viewModel.PostTypeId);

                // create table of content 
                string tableOfcontent = GetTableOfContent(posts.Id, relatedPosts);
                posts.TableContent = tableOfcontent;
            }

            id = _postsRepository.Update(posts);

            //handler tag
            //if tag change
            if (!oldTags.Equals(newTags))
            {
                //check tags add new and tags delete in table PostsTag
                string[] tagsDiff = GetTagsDiff(oldTags, newTags);
                UpdatePostsTags(userId, id, tagsDiff);
            }

            return Ok(Json(new MessageView(id, AppConstants.Message.MSG_1000)));
        }

        /// <summary>
        /// route: /api/posts
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:int}")]
        public ActionResult InsertOrUpdate(int id, Posts model)
        {
            if (null == model)
            {
                return BadRequest();
            }

            var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            HttpContext.Session.SetString("thisUrl", thisUrl);

            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var user = _userRepository.GetById(userId);
            int postTypeId = (int)model.PostTypeId;
            if (null == user)
            {
                return Json(new MessageView(AppConstants.Warning.WAR_2003));
            }

            var posts = new Posts();

            if (id == 0)
            {
                //related posts or chirld posts
                List<string> relatedPosts = new List<string>();
                if (!string.IsNullOrEmpty(posts.RelatedPosts))
                {
                    relatedPosts = posts.RelatedPosts.Split(',').ToList();
                }

                //insert
                var dateTime = DateTime.Now;
                posts = model;

                // edit some infor posts for correct
                posts.OwnerUserId = userId;
                posts.UserDisplayName = user.DisplayName;
                posts.UserAvatar = user.Avatar;
                posts.CreationDate = dateTime;
                posts.LastEditDate = dateTime;
                posts.AnswerCount = 0;
                posts.ViewCount = 0;
                posts.VoteCount = 0;
                posts.Score = 0;
                posts.CommentCount = 0;


                if (AppConstants.PostsType.POSTS.Equals(postTypeId))
                {
                    if (string.IsNullOrEmpty(model.HtmlContent))
                    {
                        return Json(new MessageView(id, AppConstants.Error.ERR_3003));
                    }
                    posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                    posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);

                }
                else if (AppConstants.PostsType.QUESTION.Equals(postTypeId))
                {
                    if (!string.IsNullOrEmpty(model.HtmlContent))
                    {
                        posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                        posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
                    }
                }
                else if (AppConstants.PostsType.TUTORIAL.Equals(postTypeId))
                {
                    if (!string.IsNullOrEmpty(model.HtmlContent))
                    {
                        posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                        posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
                    }
                }
                //if posts answer, update answer count of parent posts
                else if (AppConstants.PostsType.ANSWER.Equals(postTypeId))
                {
                    var postsParent = _postsRepository.GetPosts(model.ParentId);
                    if (postsParent != null)
                    {
                        var answerCount = postsParent.AnswerCount;
                        answerCount++;
                        postsParent.AnswerCount = answerCount;
                        _postsRepository.Update(postsParent);
                    }
                }
                else if (postTypeId != 0)
                {

                    if (string.IsNullOrEmpty(posts.RelatedPosts))
                    {
                        return Json(new MessageView(id, AppConstants.Error.ERR_3003));
                    }

                    relatedPosts = posts.RelatedPosts.Split(',').ToList();

                    // handler same as handler tag
                    UpdatePostLinks(id, relatedPosts, postTypeId);
                }

                // add  posts
                id = _postsRepository.Add(posts);

                //handle update parentId in related posts
                if (relatedPosts.Any())
                {
                    UpdateParentIdChirldPosts(id, relatedPosts);
                }

                // handler tag
                if (!string.IsNullOrEmpty(posts.Tags))
                {
                    string[] tags = posts.Tags.Split(',');
                    UpdatePostsTags(userId, id, tags);
                }

                if (!AppConstants.PostsType.ANSWER.Equals(postTypeId) && !AppConstants.PostsType.POSTS.Equals(postTypeId))
                {
                    // handler same as handler tag
                    UpdatePostLinks(id, relatedPosts, postTypeId);

                    // create table of content 
                    string tableOfcontent = GetTableOfContent(posts.Id, relatedPosts);
                    posts.TableContent = tableOfcontent;
                    _postsRepository.Update(posts);
                }

            }
            else
            {
                //update
                posts = _postsRepository.GetPosts(id);
                if (null != posts)
                {
                    //var oldPosts = posts;
                    string[] oldTags = new string[0];
                    string[] newTags = new string[0];
                    if (!string.IsNullOrEmpty(posts.Tags))
                    {
                        oldTags = posts.Tags.Split(',');
                    }
                    if (!string.IsNullOrEmpty(model.Tags))
                    {
                        newTags = model.Tags.Split(',');
                    }

                    //can't use posts = model;
                    //use copy object value
                    //posts = (Posts)ReflectionExtensions.CopyObjectValue(model, posts);
                    posts.Title = model.Title;
                    if (!string.IsNullOrEmpty(model.UserAvatar))
                        posts.UserAvatar = model.UserAvatar;
                    posts.HtmlContent = model.HtmlContent;
                    posts.Description = model.Description;
                    posts.Tags = model.Tags;
                    posts.RelatedPosts = model.RelatedPosts;
                    posts.TableContent = model.TableContent;
                    posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                    posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
                    posts.LastEditDate = DateTime.Now;
                    posts.CoverImg = model.CoverImg;

                    if (model.PostTypeId != 0 && !string.IsNullOrEmpty(posts.RelatedPosts))
                    {
                        // handler create notebook same as handler tag
                        List<string> relatedPosts = posts.RelatedPosts.Split(',').ToList();
                        UpdatePostLinks(id, relatedPosts, (int)model.PostTypeId);

                        // create table of content 
                        string tableOfcontent = GetTableOfContent(posts.Id, relatedPosts);
                        posts.TableContent = tableOfcontent;
                    }

                    id = _postsRepository.Update(posts);

                    //handler tag
                    //if tag change
                    if (!oldTags.Equals(newTags))
                    {
                        //check tags add new and tags delete in table PostsTag
                        string[] tagsDiff = GetTagsDiff(oldTags, newTags);
                        UpdatePostsTags(userId, id, tagsDiff);
                    }
                }
            }
            return Json(new MessageView(id, AppConstants.Message.MSG_1000));

        }

        /// <summary>
        /// route: /posts/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            if (0 == id)
            {
                return BadRequest();
            }
            var posts = _postsRepository.GetPosts(id);
            var dateTime = DateTime.Now;
            posts.LastEditDate = dateTime;
            posts.DeletionDate = dateTime;
            _postsRepository.Update(posts);
            return Json(new MessageView(id, AppConstants.Message.MSG_1000));
        }

        /// <summary>
        /// action: /api/posts/{id}/active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}/active")]
        public ActionResult Active(int postsId)
        {
            try
            {
                if (postsId == 0)
                    return BadRequest();

                var posts = _postsRepository.GetPosts(postsId);

                if (null == posts)
                    return BadRequest();

                var activeTime = DateTime.UtcNow;

                // If have active date is meaning this posts be actives, otherwise
                posts.LastActivityDate = activeTime;
                posts.LastEditDate = activeTime;

                _postsRepository.Update(posts);

                return Json(new MessageView(postsId, AppConstants.Message.MSG_1000));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// action: /api/posts/{id}/deActive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}/deActive")]
        public ActionResult DeActive(int postsId)
        {
            try
            {
                if (postsId == 0)
                    return BadRequest();

                var posts = _postsRepository.GetPosts(postsId);

                if (null == posts)
                    return BadRequest();

                // If have active date is meaning this posts be actives, otherwise
                posts.LastActivityDate = null;
                posts.LastEditDate = DateTime.UtcNow;

                _postsRepository.Update(posts);

                return Json(new MessageView(postsId, AppConstants.Message.MSG_1000));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// route: /api/posts/exportBook
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("exportBook")]
        public IEnumerable<Book> ExportBook()
        {
            return _postsRepository.GetAll().Where(w => w.DeletionDate == null && w.PostTypeId != 2 && w.PostTypeId != 3).Select(s => new Book
            {
                Uid = s.Id,
                Id = 0,
                Isbn = 0,
                CoverImage = s.CoverImg,
                Description = s.HeadContent,
                Title = s.Title,
                Subject = null,
                Publisher = null,
                Language = null,
                PageNumber = 0,
            }).ToList();
        }


        /// <summary>
        /// POST: /image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("api/images/posts/{id:int}/coverimg")]
        public async Task<IActionResult> UploadCoverPosts(int id)
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

                var posts = _postsRepository.GetPosts(id);
                if (posts == null)
                {
                    return NotFound();
                }
                posts.CoverImg = imageUrl;
                _postsRepository.Update(posts);

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

        #region Private methods

        private void UpdateParentIdChirldPosts(int parentPostsId, List<string> relatedPosts)
        {
            try
            {
                foreach (var chirldPostId in relatedPosts)
                {
                    int postsId = Int32.Parse(chirldPostId);
                    var posts = _postsRepository.GetPosts(postsId);
                    if (posts != null)
                    {
                        posts.ParentId = parentPostsId;
                        _postsRepository.Update(posts);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdatePostsTags(int? userId, int postsId, string[] tags)
        {
            foreach (string tag in tags)
            {
                //method handler: if tag not exist, create new tag and tag exist then get tag id
                var tagId = _tagRepository.CreateOrGetTagId(userId, tag);

                //method handler: if object exist then remove and return false, not exist then create and return true
                bool isCreate = _postsTagRepository.CreateOrDelete(postsId, tagId);

                //statistic count tag: is create then is up, do not reverse
                bool isUp = isCreate;
                _tagRepository.UpdateTagCount(isUp, tagId);
            }
        }

        private string[] GetTagsDiff(string[] oldTags, string[] newTags)
        {
            string[] diff1 = newTags.Except(oldTags).ToArray();
            string[] diff2 = oldTags.Except(newTags).ToArray();
            if (diff1.Length > 0)
                return diff1;
            else
                return diff2;
        }

        private void UpdatePostLinks(int relatedPostId, List<string> relatedPosts, int linkTypeId)
        {
            foreach (string postId in relatedPosts)
            {
                int postsId = Int32.Parse(postId);
                bool isCreate = _postLinkRepository.CreateOrDelete(relatedPostId, postsId, linkTypeId);
                if (isCreate)
                {
                    //TODO: statistic 

                }
            }
        }

        private int[] GetPostsDiff(int[] oldPosts, int[] newPosts)
        {
            int[] diff1 = newPosts.Except(oldPosts).ToArray();
            int[] diff2 = oldPosts.Except(newPosts).ToArray();
            if (diff1.Length > 0)
                return diff1;
            else
                return diff2;
        }

        private string GetTableOfContent(int relatedPostId, List<string> relatedPosts)
        {
            string[] tableOfContent = new string[relatedPosts.Count()];
            for (int i = 0; i < relatedPosts.Count(); ++i)
            {
                int postsId = Int32.Parse(relatedPosts[i]);
                var model = _postsRepository.GetPosts(postsId);
                if (model != null)
                {
                    tableOfContent[i] = model.Title;
                }
            }
            return String.Join(",", tableOfContent);
        }

        #endregion Private methods

    }

    public class Book
    {
        public int Uid { get; set; }
        public int Id { get; set; }
        public int Isbn { get; set; }
        public string CoverImage { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public int PageNumber { get; set; }
    }

}
