using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QAP4.Repository;
using QAP4.ViewModels;
using QAP4.Models;
using QAP4.Extensions;
using System;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    [Route("[controller]")]
    public class SearchController : Controller
    {
        //private QAPContext DBContext;
        private readonly IPostsService _postsService;
        private readonly ITagService _tagService;
        private IPostsTagRepository PostsTagRepo { get; set; }
        private readonly IUserService _userService;


        public SearchController(IPostsService postsService, ITagService tagService, IPostsTagRepository _postsTag, IUserService userService)
        {
            _postsService = postsService;
            _tagService = tagService;
            PostsTagRepo = _postsTag;
            _userService = userService;
        }


        // GET: /search/posts?q=sách&t=1&o=1&p=2
        // object: posts, user, tag, test
        // q: question
        // t: id type of posts, user, test, tag
        // o: order
        // p: page

        // search all: object=all, t=0
        // GET: /search/all?q=sách&t=0&p=2
        // search an object:
        // GET: /search/posts?q=sách&t=1&p=2
        [HttpGet]
        [Route("/search/{obj}")]
        public IActionResult SearchView(string obj, [FromQuery]int pg, [FromQuery]int pageSize, [FromQuery]string orderBy,
        [FromQuery]string q, [FromQuery]int po_t)
        {
            try
            {
                //TODO: order
                if (string.IsNullOrEmpty(obj) || string.IsNullOrEmpty(q))
                {
                    return BadRequest();
                }

                //default page=1
                int page = pg == 0 ? 1 : pg;

                var SearchView = new SearchView();
                if (AppConstants.ObjectType.ALL.Equals(obj))
                {
                    SearchView = GetSearchView(page, pageSize, orderBy, q);
                }
                else
                {
                    SearchView = GetSearchView(page, pageSize, orderBy, q, po_t);
                }
                return View("Search", SearchView);
            }
            catch (Exception)
            {
                return View("Search");
            }
        }

        // GET: api/search/posts?pg=1&q=abc&po_t=2
        [HttpGet]
        [Route("/api/search/posts")]
        public IActionResult SearchPosts([FromQuery]int pg, [FromQuery]int pageSize, [FromQuery]string orderBy, [FromQuery]string q, [FromQuery]int po_t)
        {
            if (string.IsNullOrEmpty(q))
            {
                return BadRequest();
            }
            //default page=1
            int page = pg == 0 ? 1 : pg;

            var PostsList = _postsService.SearchPosts(page, pageSize, orderBy, q, po_t).Where(w => w.LastActivityDate != null);
            return new ObjectResult(PostsList);
        }


        //private method

        //search all
        private SearchView GetSearchView(int pageIndex, int pageSize, string orderBy, string query)
        {
            var SearchView = new SearchView();
            SearchView.Key = query;

            //tags relation
            var tagsRelation = GetTagsRelation(query);
            if (tagsRelation.Any())
                SearchView.TagsRelation = tagsRelation;

            //get all posts
            var posts = _postsService.SearchPosts(pageIndex, pageSize, orderBy, query, 0);


            if (posts.Any())
            {
                var simplePosts = posts.Where(w => w.PostTypeId == AppConstants.PostsType.POSTS).Where(w => w.LastActivityDate != null);
                var questions = posts.Where(w => w.PostTypeId == AppConstants.PostsType.QUESTION);
                var tutorials = posts.Where(w => w.PostTypeId == AppConstants.PostsType.TUTORIAL).Where(w => w.LastActivityDate != null);

                SearchView.SimplePosts = simplePosts;
                SearchView.Questions = questions;
                SearchView.Tutorials = tutorials;

                SearchView.Count += simplePosts.Count() + questions.Count() + tutorials.Count();
            }

            //tags
            var tags = _tagService.SearchInTags(query);
            if (tags.Any())
            {
                SearchView.Tags = tags;
                SearchView.Count += tags.Count();
            }

            //users
            var users = _userService.SearchInUsers(query);
            if (users.Any())
            {
                SearchView.Users = users;
                SearchView.Count += users.Count();
            }

            return SearchView;
        }


        private SearchView GetSearchView(int page, int pageSize, string orderBy, string query, int postsTypeId)
        {
            var SearchView = new SearchView();
            SearchView.Key = query;

            //tags relation
            var tagsRelation = GetTagsRelation(query);
            if (tagsRelation.Any())
                SearchView.TagsRelation = tagsRelation;

            //get all posts
            var posts = _postsService.SearchPosts(page, pageSize, orderBy, query, postsTypeId).Where(w => w.LastActivityDate != null);


            if (posts.Any())
            {
                SearchView.Posts = posts;
                SearchView.Count += posts.Count();
            }

            //tags
            var tags = _tagService.SearchInTags(query);
            if (tags.Any())
            {
                SearchView.Tags = tags;
                SearchView.Count += tags.Count();
            }

            //users
            var users = _userService.SearchInUsers(query);
            if (users.Any())
            {
                SearchView.Users = users;
                SearchView.Count += users.Count();
            }

            return SearchView;
        }


        private IEnumerable<Tags> GetTagsRelation(string key)
        {
            return _tagService.SearchInTags(key);
        }

    }
}
