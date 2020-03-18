using Microsoft.AspNetCore.Mvc;
using QAP4.Models;
using QAP4.ViewModels;
using QAP4.Extensions;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // methods for API
        [HttpPost]
        [Route("/api/comments")]
        public ActionResult Create(Comments commentViewModel)
        {
            if (string.IsNullOrEmpty(commentViewModel.Content))
                return BadRequest();

            if (commentViewModel.UserDisplayName == null)
                return RedirectToAction("login", "user", new MessageView(AppConstants.Warning.WAR_2003, AppConstants.Screen.POSTS_DETAIL));

            _commentService.AddComment(commentViewModel);
            return Json(new MessageView(commentViewModel.Id, AppConstants.Message.MSG_1000));
        }


        // GET: /commnets?po_i=1
        // po: postsId
        [HttpGet]
        [Route("/api/comments")]
        public IActionResult Get([FromQuery]int pageIndex = 0, [FromQuery]int pageSize = int.MaxValue, [FromQuery]int po_i = 0)
        {

            if (po_i == 0)
                return BadRequest();

            var comments = _commentService.GetComments(pageIndex, pageSize, po_i);

            //TODO: refactor to return Ok
            return Json(comments);
        }
    }
}