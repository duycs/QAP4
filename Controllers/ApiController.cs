using System.Collections.Generic;
using System.Linq;
using QAP4.Domain.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QAP4.Domain.Core.Notification;
using QAP4.Domain.Core.Notifications;

namespace QAP4.Controllers
{
    /// <summary>
    /// API Controler for only APIs extend
    /// </summary>
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        private readonly ICommandDispatcher _commandBus;
        private readonly DomainNotificationHandler _notificationHandler;

        protected IEnumerable<string> Errors => _notificationHandler.Notifications.Select(n => n.Value);
        protected ApiController(
            INotificationHandler<DomainNotification> notificationHandler, ICommandDispatcher commandBus)
        {
            _notificationHandler = (DomainNotificationHandler)notificationHandler;
            _commandBus = commandBus;
        }
        protected bool IsValidOperation()
        {
            return (!_notificationHandler.HasNotifications());
        }
    }
}