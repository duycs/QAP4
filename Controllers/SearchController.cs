using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QAP4.Infrastructure.Repositories;
using QAP4.ViewModels;
using QAP4.Models;
using QAP4.Extensions;
using System;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    [Route("/api/[controller]")]
    public class SearchController : Controller
    {
        private readonly IPostsRepository _postsRepository;
        private readonly ISearchService _searchService;

        public SearchController(IPostsRepository postsRepository,
             ISearchService searchService)
        {
            _postsRepository = postsRepository;
            _searchService = searchService;
        }

        // GET: api/search/posts?pg=1&q=abc&po_t=2

        /// <summary>
        /// route: /api/search/posts
        /// Search posts
        /// </summary>
        /// <param name="pg"></param>
        /// <param name="q"></param>
        /// <param name="po_t"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/search/posts")]
        public IActionResult SearchPosts(
            [FromQuery]int pg,
            [FromQuery]string q,
            [FromQuery]int po_t,
            [FromQuery]int page = 1,
            [FromQuery]int size = 10)
        {
            if (string.IsNullOrEmpty(q))
                return BadRequest();

            var posts = _searchService.FindPosts(q, po_t, page, size);

            if (posts == null || !posts.Any())
                return NoContent();

            return Ok(posts);
        }


    }
}
