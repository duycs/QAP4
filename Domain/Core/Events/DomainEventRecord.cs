using System;

namespace QAP4.Domain.Core.Events
{
    public class DomainEventRecord 
    {
        public DateTime Created { get; set; }
        public string Type { get; set; }
        public Guid CorrelationId { get; set; }
        public string Content { get; set; }
    }
}
