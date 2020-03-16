using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QAP4.Models;
using QAP4.Repository;
using QAP4.ViewModels;
using QAP4.Extensions;
using QAP4.Application.Services;

namespace QAP4.Controllers
{
    [Route("[controller]")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: /tag/1
        [HttpGet]
        [Route("/tag/{id:int}")]
        public IActionResult GetTag(int id)
        {
            if (id < 1)
                return BadRequest();

            var tag = _tagService.GetTagById(id);

            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        // GET: /tag/sách
        [HttpGet]
        [Route("{name}")]
        public IActionResult GetTagsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var tags = _tagService.GetTagsByName(name);

            if (tags == null)
                return NotFound();

            return Ok(tags);
        }

        // GET: /tag?po=2
        [HttpGet]
        [Route("/tag")]
        public IActionResult GetTagsByPosts([FromQuery]int po_i)
        {
            if (po_i < 1)
                return BadRequest();

            var tags = _tagService.GetTagsByPostsId(po_i);

            if (tags == null)
                return NotFound();

            return Ok(tags);
        }

        // POST: /tag
        [HttpPost]
        public ActionResult CreateTag([FromBody]Tags tagViewModel)
        {
            if (tagViewModel == null)
                return BadRequest();

            var tagAdded = _tagService.AddTag(tagViewModel);

            return Ok(new MessageView(tagAdded.Id, AppConstants.Message.MSG_1000));
        }

        // UPDATE: /tag/1
        [HttpPut]
        [Route("/tag/{id:int}")]
        public ActionResult UpdateTag(int id, [FromBody]Tags tagViewModel)
        {
            if (id < 1)
                return BadRequest();

            var tag = _tagService.GetTagById(id);

            if (tag == null)
                return BadRequest();

            tag = (Tags)ReflectionExtensions.CopyObjectValue(tagViewModel, tag);

            _tagService.UpdateTag(tag);

            return Ok(new MessageView(id, AppConstants.Message.MSG_1000));
        }

        // UPDATE: /tag/sách
        [HttpPut]
        [Route("/tag/{name}")]
        public ActionResult UpdateTagByName(string name, [FromBody]Tags tagViewModel)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var tag = _tagService.GetTagByName(name);

            if (null == tag)
                return BadRequest();

            tag = (Tags)ReflectionExtensions.CopyObjectValue(tagViewModel, tag);
            _tagService.UpdateTag(tag);

            return Ok(new MessageView(tag.Id, AppConstants.Message.MSG_1000));
        }

        // DELETE: /tag/1
        [HttpDelete("{id:int}")]
        public ActionResult DeleteTag(int id)
        {
            if (id < 1)
                return BadRequest();

            _tagService.DeleteTagById(id);

            return Ok(new MessageView(id, AppConstants.Message.MSG_1000));
        }

        // DELETE: /tag/sách
        [HttpDelete("{name}")]
        public ActionResult DeleteTagByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var tag = _tagService.GetTagByName(name);
            if (tag == null)
                return BadRequest();

            _tagService.DeleteTagByName(name);

            return Ok(new MessageView(tag.Id, AppConstants.Message.MSG_1000));
        }

    }
}
