using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using QAP4.Domain.Core.Events;
using QAP4.Domain.Core.Notifications;

namespace QAP4.Domain.Core.Notification
{
    public class DomainNotificationHandler : IEventHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public ReadOnlyCollection<DomainNotification> Notifications => _notifications.AsReadOnly();
                public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }
        public virtual bool HasNotifications()
        {
            return Notifications.Count <= 0;
        }

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }
    }
}
