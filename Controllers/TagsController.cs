using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QAP4.Models;
using QAP4.Infrastructure.Repositories;
using QAP4.ViewModels;
using QAP4.Extensions;
using System.Linq;
using System;

namespace QAP4.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        private ITagRepository _tagRepository { get; set; }

        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        /// <summary>
        /// route: /api/tags/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetTagById(int id)
        {
            var tag = _tagRepository.GetTag(id);
            return Ok(tag);
        }

        /// <summary>
        /// route: /api/tags/{name}
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}")]
        public IActionResult GetTagsByName(string name)
        {
            var tags = _tagRepository.GetTagsByName(name);

            if (tags == null || !tags.Any())
                return NoContent();

            return Ok(tags);
        }

        /// <summary>
        /// route: /api/tags
        /// </summary>
        /// <param name="po_i"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IActionResult GetTagsByPostsId([FromQuery]int po_i)
        {
            var tags = _tagRepository.GetTagsByPosts(po_i);
            if (tags == null || !tags.Any())
                return NoContent();

            return Ok(tags);
        }

        /// <summary>
        /// route: /api/tags
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateTag([FromBody]Tags viewModel)
        {
            try
            {
                if (null == viewModel)
                    return BadRequest();

                _tagRepository.Create(viewModel);

                return Ok(Json(new MessageView(viewModel.Id, AppConstants.Message.MSG_1000)));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// route: /api/tags/{id}
        /// Update Tag
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateTag(int id, [FromBody]Tags viewModel)
        {
            try
            {
                var tag = _tagRepository.GetTag(id);
                if (null == tag)
                    return BadRequest();

                tag = (Tags)ReflectionExtensions.CopyObjectValue(viewModel, tag);

                _tagRepository.UpdateTag(tag);

                return Ok(Json(new MessageView(id, AppConstants.Message.MSG_1000)));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// route: /api/tags/{name}
        /// </summary>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{name}")]
        public IActionResult UpdateTagByName(string name, [FromBody]Tags viewModel)
        {
            try
            {
                var tag = _tagRepository.GetTagByName(name);
                if (null == tag)
                    return BadRequest();

                tag = (Tags)ReflectionExtensions.CopyObjectValue(viewModel, tag);
                _tagRepository.UpdateTag(tag);

                return Ok(Json(new MessageView(tag.Id, AppConstants.Message.MSG_1000)));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// route: /api/tags/{id}
        /// Delete Tag
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult DeleteTag(int id)
        {
            try
            {
                if (0 == id)
                    return BadRequest();

                _tagRepository.DeleteTag(id);

                return Ok(Json(new MessageView(id, AppConstants.Message.MSG_1000)));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// route: /api/tags/{name}
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        public IActionResult DeleteTagByName(string name)
        {
            var tag = _tagRepository.GetTagByName(name);
            if (null == tag)
                return BadRequest();

            _tagRepository.DeleteTagByName(name);

            return Ok(Json(new MessageView(tag.Id, AppConstants.Message.MSG_1000)));
        }

    }
}
