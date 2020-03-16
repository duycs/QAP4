using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Repository;
using QAP4.Models;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class XController : Controller
    {
        private readonly IPostsService _postsService;
        private readonly ITagService _tagService;
        private IPostsTagRepository PostsTagRepo { get; set; }
        private readonly IUserService _userService;

        public XController(IPostsService postsService, ITagService tagService, IPostsTagRepository _postsTag, IUserService userService)
        {
            _postsService = postsService;
            _tagService = tagService;
            PostsTagRepo = _postsTag;
            _userService = userService;
        }

        // GET: /x/sach
        [HttpGet]
        [Route("{name}")]
        public ActionResult SearchInTags(string name)
        {
            var result = _tagService.SearchInTags(name);
            return View("X", result);
        }

    }
}