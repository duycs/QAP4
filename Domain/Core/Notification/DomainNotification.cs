using System;
using QAP4.Domain.Core.Events;

namespace QAP4.Domain.Core.Notifications
{
    public class DomainNotification : DomainEvent
    {
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }
        public DomainNotification(string key, string value)
        {
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
            Key = key;
            Value = value;
            Type = typeof(DomainNotification).Name;
        }
        public override void Flatten()
        {
            Args.Add("DomainNotificationId", DomainNotificationId);
            Args.Add("Key", Key);
            Args.Add("Value", Value);
            Args.Add("Version", Version);
        }
    }
}
