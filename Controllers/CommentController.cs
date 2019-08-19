using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Models;
using QAP4.ViewModels;
using QAP4.Extensions;
using QAP4.Infrastructure.Repositories;
using MediatR;
using QAP4.Domain.Core.Commands;
using QAP4.Domain.Core.Notifications;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    /// <summary>
    /// Comment controller
    /// </summary>
    [Route("api/[controller]")]
    public class CommentController : ApiController
    {
        private readonly INotificationHandler<DomainNotification> _notificationHandler;
        private readonly ICommandDispatcher _commandBusHandler;
        private readonly ICommentService _commentService;

        public CommentController(
             INotificationHandler<DomainNotification> notificationHandler,
             ICommandDispatcher commandBusHandler,
             ICommentService commentService)
        : base(notificationHandler, commandBusHandler)
        {
            _notificationHandler = notificationHandler;
            _commandBusHandler = commandBusHandler;
            _commentService = commentService;
        }
        
        /// <summary>
        /// Post comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("comments")]
        public ActionResult Create(Comments comment)
        {
            if (comment == null || string.IsNullOrEmpty(comment.Content))
                return BadRequest();

            //redirect to login
            if (comment.UserId == null || comment.UserId == 0)
                return RedirectToAction("login", "user", new MessageView(AppConstants.Warning.WAR_2003, AppConstants.Screen.POSTS_DETAIL));

            int result = _commentService.AddComment(comment);

            return Ok(new MessageView(result, AppConstants.Message.MSG_1000));
        }

        /// <summary>
        /// Get comment by id
        /// </summary>
        /// <param name="po_i"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("comments")]
        public IActionResult Get([FromQuery]int po_i)
        {
            if (po_i == 0)
                return BadRequest();

            var comments = _commentService.FindCommentsByPostId(po_i);

            if (comments == null || !comments.Any())
                return NoContent();

            return Ok(comments);
        }
    }
}