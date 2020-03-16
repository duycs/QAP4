using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QAP4.Repository;
using Microsoft.AspNetCore.Http;
using QAP4.Extensions;
using QAP4.Models;
using Microsoft.AspNetCore.Http.Features;
using QAP4.ViewModels;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostsService _postsService;
        private readonly ITagService _tagService;
        private IPostsTagRepository PostsTagRepo { get; set; }
        private readonly IUserService _userService;
        private IQuoteRepository QuoteRepo { get; set; }


        public HomeController(IPostsService postsService, 
        ITagService tagService, 
        IPostsTagRepository _postsTag, 
        IUserService userService, 
        IQuoteRepository _quoteRepo)
        {
            _postsService = postsService;
            _tagService = tagService;
            PostsTagRepo = _postsTag;
            _userService = userService;
            QuoteRepo = _quoteRepo;
        }


        // methods for MVC

        [HttpGet]
        public IActionResult GetPost()
        {
            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            return View();
        }

        
        [HttpGet]
        [Route("@{accountName}")]
        public IActionResult FindUserByAccountName(string accountName)
        {
            if(string.IsNullOrEmpty(accountName))
                return BadRequest();

            var thisHost = $"{this.Request.Scheme}://{this.Request.Host}";
            var url = $"{thisHost}/users/@{accountName}";
            return Redirect(url);
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = userId;
            var user = _userService.GetUserById(userId);


            HomeView homeView = new HomeView();
            homeView.User = user;
            int pageIndex = 0;
            int pageSize = AppConstants.Paging.PAGE_SIZE;

            homeView.PostsFeed = _postsService.GetPostsByCreateDate(pageIndex, pageSize).Where(w=>w.LastActivityDate != null);
            homeView.Quote = QuoteRepo.GetAutoQuote();
            homeView.TagsFeature = _tagService.GetTagsFeature(5);
            homeView.UsersFeature = _userService.GetUsersFeature(5);

            return View(homeView);
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            return View();
        }


        [HttpGet("about-us")]
        public IActionResult AboutUs()
        {
            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            return View();
        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            return View();
        }

    }
}
