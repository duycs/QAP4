using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QAP4.Extensions;

namespace QAP4.Controllers
{
    public class GroupController : Controller
    {


        public ActionResult Group()
        {
            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            return View();
        }
    }
}
