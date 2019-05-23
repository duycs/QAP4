using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QAP4.Models;
using QAP4.Repository;
using QAP4.ViewModels;
using QAP4.Extensions;

namespace QAP4.Controllers
{
    [Route("[controller]")]
    public class TagController : Controller
    {
        //private QAPContext DBContext;
        private ITagRepository TagRepo { get; set; }

        public TagController(ITagRepository _repo)
        {
            TagRepo = _repo;
        }


        //----- methods for API

        // GET: /tag/1
        [HttpGet]
        [Route("/tag/{id:int}")]
        public Tags GetTag(int id)
        {
            return TagRepo.GetTag(id);
        }

        // GET: /tag/sách
        [HttpGet]
        [Route("{name}")]
        public IEnumerable<Tags> GetTagsByName(string name)
        {
            return TagRepo.GetTagsByName(name);
        }

        // GET: /tag?po=2
        [HttpGet]
        [Route("/tag")]
        public IEnumerable<Tags> GetTagsByPosts([FromQuery]int po_i)
        {
            return TagRepo.GetTagsByPosts(po_i);
        }

        // POST: /tag
        [HttpPost]
        public ActionResult CreateTag([FromBody]Tags model)
        {
            if (null == model)
            {
                return BadRequest();
            }
            TagRepo.Create(model);
            return Json(new MessageView(model.Id, AppConstants.Message.MSG_1000));
        }

        // UPDATE: /tag/1
        [HttpPut]
        [Route("/tag/{id:int}")]
        public ActionResult UpdateTag(int id, [FromBody]Tags model)
        {
            var tag = TagRepo.GetTag(id);
            if (null == tag)
            {
                return BadRequest();
            }
            tag = (Tags)ReflectionExtensions.CopyObjectValue(model, tag);
            TagRepo.UpdateTag(tag);
            return Json(new MessageView(id, AppConstants.Message.MSG_1000));
        }

        // UPDATE: /tag/sách
        [HttpPut]
        [Route("/tag/{name}")]
        public ActionResult UpdateTagByName(string name, [FromBody]Tags model)
        {
            var tag = TagRepo.GetTagByName(name);
            if (null == tag)
            {
                return BadRequest();
            }
            tag = (Tags)ReflectionExtensions.CopyObjectValue(model, tag);
            TagRepo.UpdateTag(tag);
            return Json(new MessageView(tag.Id, AppConstants.Message.MSG_1000));
        }

        // DELETE: /tag/1
        [HttpDelete("{id:int}")]
        public ActionResult DeleteTag(int id)
        {
            if (0 == id)
            {
                return BadRequest();
            }
            TagRepo.DeleteTag(id);
            return Json(new MessageView(id, AppConstants.Message.MSG_1000));
        }

        // DELETE: /tag/sách
        [HttpDelete("{name}")]
        public ActionResult DeleteTagByName(string name)
        {
            var tag = TagRepo.GetTagByName(name);
            if (null == tag)
            {
                return BadRequest();
            }
            TagRepo.DeleteTagByName(name);
            return Json(new MessageView(tag.Id, AppConstants.Message.MSG_1000));
        }

    }
}
