using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QAP4.Models;
using QAP4.Repository;
using Microsoft.AspNetCore.Http;
using QAP4.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using QAP4.ViewModels;
using Microsoft.Extensions.Configuration;
using QAP4.Infrastructure.Helpers.File;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostsService _postsService;
        private readonly ITagService _tagService;
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3Service AmazonS3Service;

        public UserController(IConfiguration configuration,
        IAmazonS3Service amazonS3Service,
        IPostsService postsService,
        ITagService tagService,
        IUserService userService)
        {
            _configuration = configuration;
            AmazonS3Service = amazonS3Service;
            _postsService = postsService;
            _tagService = tagService;
            _userService = userService;
        }

        // methods for MVC

        [HttpGet("login")]
        public ActionResult Login(UsersView item, [FromQuery]string sc)
        {
            //TODO: Migrate
            //var users = GetUserUpdatedAccountName();
            //_userService.UpdateUserRange(users);
            ViewBag.Screen = sc;
            return View();
        }


        // POST: /checklogin
        // posts can be a posts normal (1), a question (2) or an answer (postType=3)
        [HttpPost("checklogin")]
        public ActionResult CheckLogin(LoginView item, [FromQuery]string sc)
        {
            var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.Path}";

            if (item == null || string.IsNullOrEmpty(item.EmailOrPhone) || string.IsNullOrEmpty(item.Password))
            {
                return BadRequest();
            }
            else
            {
                var thatUrl = HttpContext.Session.GetString("thisUrl");

                var user = _userService.GetUserIsLogin(item.EmailOrPhone.Trim(), item.Password.Trim());
                //var sc = item.Screen;
                if (user != null)
                {
                    var userId = @user.AccountName;
                    //set session
                    HttpContext.Session.SetInt32(AppConstants.Session.USER_ID, user.Id);
                    HttpContext.Session.SetString(AppConstants.Session.USER_NAME, user.AccountName.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.USER_NAME, user.DisplayName.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.FIRST_NAME, user.FirstName.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.LAST_NAME, user.LastName.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.EMAIL, user.Email.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.AVARTAR, user.Avatar.EmptyIfNull());

                    if (!string.IsNullOrEmpty(thatUrl) && thisUrl != thatUrl)
                    {
                        return Redirect(thatUrl);
                    }

                    //if (AppConstants.Screen.POSTS_MANAGER.Equals(sc))
                    //{
                    //    // GET: /posts/manager?t=1&u=6&sc=manager
                    //    return RedirectToAction("manager", "posts", new { pg = 1, or_b = "cr_d", u_i = userId, po_t = 1 });
                    //}
                    //else if (AppConstants.Screen.POSTS_ASK.Equals(sc))
                    //{
                    //    return RedirectToAction("ask", "posts", new { u_i = userId });
                    //}
                    //else if (AppConstants.Screen.POSTS_QUESTION.Equals(sc))
                    //{
                    //    return RedirectToAction("question", "posts", new { pg = 1, or_b = "cr_d", u_i = userId, po_t = 2 });
                    //}
                    //else if (AppConstants.Screen.POSTS_ANSWER.Equals(sc))
                    //{
                    //    //return RedirectToAction("answer", "posts", new { pg = 1, or_b = "cr_d", u_i = userId, po_t = 2 });
                    //}
                    return RedirectToAction("", "Home", new MessageView(AppConstants.Message.MSG_1004));
                }
            }
            return RedirectToAction("login", "user", new MessageView(AppConstants.Warning.WAR_2001));
        }

        //create
        //[ActionName("Create")]
        [HttpPost]
        public ActionResult Create(RegisterView model)
        {
            if (model == null || string.IsNullOrEmpty(model.EmailOrPhone) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest();
            }
            //check confirm password
            if (model.Password != model.PasswordConfirm)
            {
                return RedirectToAction("Register", "User", new MessageView(AppConstants.Error.ERR_3000));
            }

            //check phone or email
            var emailOrPhone = model.EmailOrPhone;
            bool isUserHaveEmail = false;

            if (ValidateExtensions.IsValidPhone(emailOrPhone))
                model.Phone = emailOrPhone;
            else if (ValidateExtensions.IsValidEmail(emailOrPhone))
            {
                model.Email = emailOrPhone;
                isUserHaveEmail = true;
            }

            // Check email or phone is registed
            var userRegisted = _userService.GetUserByEmailOrPhone(emailOrPhone);
            if (userRegisted != null)
            {
                return RedirectToAction("Register", "User", new MessageView("User is registed by this email or phone"));
            }

            Users user = new Users();

            // Create new account
            var accountName = string.Empty;

            if (isUserHaveEmail)
            {
                accountName = emailOrPhone.Split('@')[0];
                user.AccountName = accountName;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DisplayName = model.DisplayName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Password = model.Password;

            //add
            Users userAdded = _userService.AddUser(user);
            if (userAdded != null)
            {
                // Create account if It havent been created
                if (!isUserHaveEmail && (!string.IsNullOrEmpty(user.FirstName) || !string.IsNullOrEmpty(user.LastName)))
                {
                    string fullName = $"{user.FirstName} {user.LastName}";
                    accountName = $"{StringExtensions.StringToSlug(fullName)}---{@user.AccountName}";
                    user.AccountName = accountName;
                    _userService.UpdateUser(user);
                }

                if (user != null)
                {
                    //set session
                    HttpContext.Session.SetString(AppConstants.Session.USER_NAME, user.DisplayName.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.FIRST_NAME, user.FirstName.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.LAST_NAME, user.LastName.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.EMAIL, user.Email.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.AVARTAR, user.Avatar.EmptyIfNull());
                    HttpContext.Session.SetString(AppConstants.Session.AVARTAR, user.AccountName.EmptyIfNull());
                    HttpContext.Session.SetInt32(AppConstants.Session.USER_ID, user.Id);

                    //redirect
                    return RedirectToAction("Index", "Home", new { message = "Hello" });
                }

                //if error get user
                //TODO: return error system
                return View();
            }
            else
            {
                return Register();
            }
        }

        // private IEnumerable<Users> GetUserUpdatedAccountName()
        // {
        //     var users = _userService.GetAll();
        //     foreach (var user in users)
        //     {
        //         var accountName = string.Empty;
        //         if (!string.IsNullOrEmpty(user.Email))
        //         {
        //             accountName = user.Email.Split('@')[0];
        //         }
        //         else if (!string.IsNullOrEmpty(user.FirstName) || !string.IsNullOrEmpty(user.LastName))
        //         {
        //             string fullName = $"{user.FirstName} {user.LastName}";
        //             accountName = $"{StringExtensions.StringToSlug(fullName)}---{@user.AccountName}";
        //         }
        //         user.AccountName = accountName;
        //         //_userService.UpdateUser(user);
        //         yield return user;
        //     }
        // }

        // GET: /logout
        [HttpGet("logout")]
        public ActionResult Logout()
        {
            //remove session
            HttpContext.Session.Clear();

            return RedirectToAction("", "Home", new MessageView(0, AppConstants.Message.MSG_1005, ""));
        }

        [HttpGet("register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet("users/{id}")]
        public IActionResult Personal(int id, [FromQuery]int pageIndex, [FromQuery]int pageSize, [FromQuery]string orderBy)
        {
            if (id < 0)
                return BadRequest();

            var user = _userService.GetUserById(id);
            if (user == null)
                return NoContent();

            var userView = new UsersView();

            // Check if current user or other user view profile
            var currentUserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            if (currentUserId == id)
                userView.IsCurrentUser = true;
            else
                userView.IsCurrentUser = false;

            userView.User = user;
            userView.TagsFeature = _tagService.GetTagsFeature(5);
            userView.PostsNewest = _postsService.GetPostsSameAuthor(pageIndex, pageSize, orderBy, 0, id, 1);
            userView.QuestionsNewest = _postsService.GetPostsSameAuthor(pageIndex, pageSize, orderBy, 0, id, 2);
            userView.UsersFollowing = _userService.GetUsersFollowing(id);

            return View(userView);
        }

        [HttpGet]
        [Route("users/@{accountName}")]
        public IActionResult FindUserByAccountName(string accountName, [FromQuery]int pageIndex, [FromQuery]int pageSize, [FromQuery]string orderBy)
        {
            if (string.IsNullOrEmpty(accountName))
                return BadRequest();

            var user = _userService.GetUserByAccountName(accountName);
            if (user == null)
                return NoContent();

            var userView = new UsersView();

            // Check if current user or other user view profile
            var currentUserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);

            if (currentUserId == user.Id)
                userView.IsCurrentUser = true;
            else
                userView.IsCurrentUser = false;

            userView.User = user;
            userView.TagsFeature = _tagService.GetTagsFeature(5);
            userView.PostsNewest = _postsService.GetPostsSameAuthor(pageIndex, pageSize, orderBy, 0, user.Id, 1);
            userView.QuestionsNewest = _postsService.GetPostsSameAuthor(pageIndex, pageSize, orderBy, 0, user.Id, 2);
            //userView.TestsNewest=
            userView.UsersFollowing = _userService.GetUsersFollowing(user.Id);

            return View("Personal", userView);
        }

        // methods for API

        // [HttpGet]
        // public IEnumerable<Users> ApiUsers()
        // {
        //     return UserRepo.GetAll();
        // }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            if (id < 1)
                return BadRequest();

            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete]
        public void Delete(string id)
        {
            _userService.DeleteUser(id);
        }


        //update avatar
        [HttpPost("api/user/{id:int}/avatar")]
        public async Task<IActionResult> UpdateAvatar(int id)
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            if (userId != id)
                return Unauthorized();

            var file = this.Request.Form.Files[0];
            var imageUrl = string.Empty;

            // File type validation
            if (!file.ContentType.Contains("image"))
            {
                return StatusCode(500, "image file not found");
            }

            var bucket = _configuration.GetSection("AWSS3:BucketPostStudy").Value;
            var imageResponse = await AmazonS3Service.UploadObject(bucket, file);

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

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Avatar = imageUrl;
            _userService.UpdateUser(user);

            //update userAvatar in posts
            UpdateUserAvatarInPosts(user.Id, imageUrl);

            var response = new
            {
                Success = true,
                ImageUrl = imageUrl
            };
            return Ok(response);

        }

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
                var imageResponse = await AmazonS3Service.UploadObject(bucket, file);

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

                var posts = _postsService.GetPostsById(id);
                if (posts == null)
                {
                    return NotFound();
                }
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

        //update banner
        [HttpPost("api/user/{id:int}/banner")]
        public async Task<IActionResult> UpdateBanner(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
                if (userId != id)
                    return Unauthorized();

                var file = this.Request.Form.Files[0];
                var imageUrl = string.Empty;

                // File type validation
                if (!file.ContentType.Contains("image"))
                {
                    return StatusCode(500, "image file not found");
                }

                var bucket = _configuration.GetSection("AWSS3:BucketPostStudy").Value;
                var imageResponse = await AmazonS3Service.UploadObject(bucket, file);
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

                var user = _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
                user.BannerImg = imageUrl;
                _userService.UpdateUser(user);

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

        private void UpdateUserAvatarInPosts(int userId, string userAvatar)
        {
            try
            {
                var postsByOwnerId = _postsService.GetPostsByOwnerUserId(0, 0, null, userId);
                if (!postsByOwnerId.Any()) return;

                foreach (var posts in postsByOwnerId)
                {
                    posts.UserAvatar = userAvatar;
                    _postsService.UpdatePosts(posts);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //delete

    }
}
