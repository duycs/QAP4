using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Models;
using QAP4.Repository;
using QAP4.ViewModels;
using QAP4.Extensions;

namespace QAP4.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private ICommentRepository CommentRepo { get; set; }
        private IPostsRepository PostsRepo { get; set; }
        private ITagRepository TagRepo { get; set; }
        private IUserRepository UserRepo { get; set; }

        public CommentController(ICommentRepository _commentRepo, IPostsRepository _postsRepo, ITagRepository _tagRepo, IUserRepository _userRepo)
        {
            CommentRepo = _commentRepo;
            PostsRepo = _postsRepo;
            TagRepo = _tagRepo;
            UserRepo = _userRepo;
        }

        // methods for MVC



        // methods for API
        [HttpPost]
        [Route("/api/comments")]
        public ActionResult Create(Comments model)
        {
            if (string.IsNullOrEmpty(model.Content))
            {
                return BadRequest();
            }
            //try
            //{
            if (model.UserDisplayName != null) { 
                var userComment = UserRepo.GetById(model.UserId);
                var time = DateTime.Now;
                model.UserDisplayName = userComment.DisplayName;
                model.CreationDate = model.CreationDate == null ? time : model.CreationDate;
                model.ModificationDate = time;
                model.ParentId = model.ParentId == 0 ? null : model.ParentId;
                model.UpvoteCount = model.UpvoteCount == null ? 0 : model.UpvoteCount;

                CommentRepo.Create(model);

                //update posts
                var posts = PostsRepo.GetPosts(model.PostsId);
                var commentCount = posts.CommentCount;
                commentCount++;
                posts.CommentCount = commentCount;
                PostsRepo.Update(posts);
            }else { 
            //catch (Exception)
            //{
                //return Json(e.Message);
                return RedirectToAction("login", "user", new MessageView(AppConstants.Warning.WAR_2003, AppConstants.Screen.POSTS_DETAIL));
                //return View("login");
            }
            return Json(new MessageView(model.Id, AppConstants.Message.MSG_1000));
        }


        // GET: /commnets?po_i=1
        // po: postsId
        [HttpGet]
        [Route("/api/comments")]
        public IActionResult Get([FromQuery]int po_i)
        {

            if (po_i == 0)
            {
                return BadRequest();
            }
            var comments = CommentRepo.GetCommentsByPosts(po_i);

            return Json(comments);
        }
    }
}