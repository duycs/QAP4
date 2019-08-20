using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Infrastructure.Repositories;
using QAP4.Models;
using QAP4.ViewModels;
using QAP4.Extensions;
using MediatR;
using QAP4.Domain.Core.Notifications;
using QAP4.Domain.Core.Commands;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    [Route("api/[controller]")]
    public class VotesController : ApiController
    {
        private readonly INotificationHandler<DomainNotification> _notificationHandler;
        private readonly ICommandDispatcher _commandBusHandler;
        private readonly ICommentService _commentService;

        private readonly IPostsRepository _postsRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly IUserRepository _userRepository;

        public VotesController(INotificationHandler<DomainNotification> notificationHandler,
             ICommandDispatcher commandBusHandler,
             IPostsRepository postsRepository,
             IVoteRepository voteRepository,
             IUserRepository userRepository) : base(notificationHandler, commandBusHandler)
        {
            _commandBusHandler = commandBusHandler;
            _notificationHandler = notificationHandler;

            _postsRepository = postsRepository;
            _voteRepository = voteRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// route: /api/votes
        /// </summary>
        /// <param name="po_i">postsId</param>
        /// <param name="vo_t">voteTypeId</param>
        /// <param name="u_i">userId</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IActionResult Get([FromQuery]int po_i, [FromQuery]int vo_t, [FromQuery]int u_i)
        {
            try
            {
                var votes = _voteRepository.GetVotes(po_i, vo_t, u_i);

                if (votes == null || !votes.Any())
                    return NotFound();

                return Ok(votes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// POST: /api/votes
        /// Create new vote or update vote by user
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IActionResult UpOrDownVote(Votes viewModel)
        {
            try
            {
                // Validate
                if (viewModel == null)
                    return BadRequest();

                // Get user by session
                var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
                var user = _userRepository.GetById(userId);

                if (null == user)
                    return Ok(new MessageView(AppConstants.Warning.WAR_2003));

                // Check voted
                bool userVoted = _voteRepository.CheckUserVoted((int)userId, (int)viewModel.PostsId, (int)viewModel.VoteTypeId, (bool)viewModel.IsOn);
                if (userVoted)
                {
                    return Ok(new MessageView(AppConstants.Warning.WAR_2002));
                }

                // Create or update vote
                var voteCheck = _voteRepository.GetVote((int)userId, (int)viewModel.PostsId, (int)viewModel.VoteTypeId);
                if (voteCheck == null)
                {
                    var vote = new Votes();
                    vote.PostsId = viewModel.PostsId;
                    vote.VoteTypeId = viewModel.VoteTypeId;
                    vote.UserId = userId;
                    vote.IsOn = viewModel.IsOn;
                    vote.CreationDate = DateTime.Now;
                    _voteRepository.Create(vote);
                }
                else
                {
                    voteCheck.IsOn = viewModel.IsOn;
                    voteCheck.CreationDate = DateTime.Now;
                    _voteRepository.Update(voteCheck);
                }

                // Update voteCount in posts
                var posts = _postsRepository.GetPosts(viewModel.PostsId);
                if (posts != null)
                {
                    var count = posts.VoteCount;
                    if (viewModel.IsOn == true)
                    {
                        count++;
                    }
                    else
                    {
                        count--;
                    }
                    posts.VoteCount = count;
                    _postsRepository.Update(posts);
                }

                return Ok(new MessageView(AppConstants.Message.MSG_1000));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}