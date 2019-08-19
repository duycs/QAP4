using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Infrastructure.Repositories;
using QAP4.Models;

namespace QAP4.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class XController : Controller
    {
        private IPostsRepository PostsRepo { get; set; }
        private ITagRepository TagRepo { get; set; }
        private IPostsTagRepository PostsTagRepo { get; set; }
        private IUserRepository UserRepo { get; set; }


        public XController(IPostsRepository _postsRepo, ITagRepository _tagRepo, IPostsTagRepository _postsTag, IUserRepository _userRepo)
        {
            PostsRepo = _postsRepo;
            TagRepo = _tagRepo;
            PostsTagRepo = _postsTag;
            UserRepo = _userRepo;
        }

        // GET: /x/s�ch
        [HttpGet]
        [Route("{name}")]
        public ActionResult SearchInTags(string name)
        {
            var result = TagRepo.SearchInTags(name);
            return View("X", result);
        }

    }
}