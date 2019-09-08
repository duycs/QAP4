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

namespace QAP4.Controllers
{
    public class HomeController : Controller
    {
        private IPostsRepository PostsRepo { get; set; }
        private ITagRepository TagRepo { get; set; }
        private IPostsTagRepository PostsTagRepo { get; set; }
        private IUserRepository UserRepo { get; set; }
        private IQuoteRepository QuoteRepo { get; set; }


        public HomeController(IPostsRepository _postsRepo, ITagRepository _tagRepo, IPostsTagRepository _postsTag, IUserRepository _userRepo, IQuoteRepository _quoteRepo)
        {
            PostsRepo = _postsRepo;
            TagRepo = _tagRepo;
            PostsTagRepo = _postsTag;
            UserRepo = _userRepo;
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
            var user = UserRepo.GetById(userId);


            HomeView homeView = new HomeView();
            homeView.User = user;
            int page = 0;
            homeView.PostsFeed = PostsRepo.GetPostsByCreateDate(page);
            homeView.Quote = QuoteRepo.GetAutoQuote();
            homeView.TagsFeature = TagRepo.GetTagsFeature();
            homeView.UsersFeature = UserRepo.GetUsersFeature();

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
