using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Repository;
using QAP4.Models;
using QAP4.ViewModels;
using QAP4.Extensions;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class VoteController : Controller
    {
        private readonly IVoteService _voteService;
        private readonly IPostsService _postsService;
        private readonly IUserService _userService;

        public VoteController(IVoteService voteService, IPostsService postsService, IUserService userService)
        {
            _voteService = voteService;
            _postsService = postsService;
            _userService = userService;
        }

        // GET: /api/vote?po_i=1&vo_t=1&u_i=0
        [HttpGet]
        [Route("/api/vote")]
        public IActionResult Get([FromQuery]int po_i, [FromQuery]int vo_t, [FromQuery]int u_i)
        {
            if (po_i < 0 && vo_t < 1 && u_i < 1)
                return BadRequest();

            var votes = _voteService.GetVotes(u_i, po_i, vo_t);
            return Ok(votes);
        }

        // POST: /api/vote
        [HttpPost]
        [Route("/api/vote")]
        public IActionResult Post(Votes model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var user = _userService.GetUserById(userId);
            if (null == user)
            {
                return Json(new MessageView(AppConstants.Warning.WAR_2003));
            }

            //check voted
            bool userVoted = _voteService.IsUserVoted((int)userId, (int)model.PostsId, (int)model.VoteTypeId, (bool)model.IsOn);
            if (userVoted)
            {
                return Json(new MessageView(AppConstants.Warning.WAR_2002));
            }

            //create or update vote

            var voteCheck = _voteService.GetVote((int)userId, (int)model.PostsId, (int)model.VoteTypeId);
            if (voteCheck == null)
            {
                var vote = new Votes();
                vote.PostsId = model.PostsId;
                vote.VoteTypeId = model.VoteTypeId;
                vote.UserId = userId;
                vote.IsOn = model.IsOn;
                vote.CreationDate = DateTime.Now;

                _voteService.AddVote(vote);
            }
            else
            {
                voteCheck.IsOn = model.IsOn;
                voteCheck.CreationDate = DateTime.Now;
                _voteService.UpdateVote(voteCheck);
            }

            //update voteCount in posts
            var posts = _postsService.GetPostsById((int)model.PostsId);
            if (posts != null)
            {
                var count = posts.VoteCount;
                if (model.IsOn == true)
                {
                    count++;
                }
                else
                {
                    count--;
                }
                posts.VoteCount = count;
                _postsService.UpdatePosts(posts);
            }

            return Json(new MessageView(AppConstants.Message.MSG_1000));
        }

    }
}