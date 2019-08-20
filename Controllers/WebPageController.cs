using System.Collections.Generic;
using System.Linq;
using QAP4.Domain.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Domain.Core.Notification;
using QAP4.Domain.Core.Notifications;
using QAP4.Extensions;

namespace QAP4.Controllers
{
    /// <summary>
    /// Webpage base
    /// </summary>
    [Produces("application/json")]
    public class WebPageController : Controller
    {
        // public readonly string _userName;
        // public readonly int? _userId;
        protected WebPageController()
        {
        }
    }
}