using System;
using System.Collections.Generic;
using MediatR;
using Newtonsoft.Json;

namespace QAP4.Domain.Core.Events
{
    public abstract class DomainEvent : INotification
    {
        [JsonIgnore]
        public DateTime Created { get; private set; }
        [JsonIgnore]
        public string Type { get; set; }
        public Dictionary<string, object> Args { get; private set; }
        public Guid CorrelationId { get; set; }
        public string Content { get; set; }
        protected DomainEvent()
        {
            Created = DateTime.Now;
            Args = new Dictionary<string, object>();
            Type = GetType().Name;
        }
        public abstract void Flatten();
    }
}
