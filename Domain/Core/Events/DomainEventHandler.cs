using System;
using System.Threading;
using System.Threading.Tasks;
using QAP4.Domain.Core.Repositories;
using Newtonsoft.Json;

namespace QAP4.Domain.Core.Events
{
    public class DomainEventHandler<T> : IEventHandler<T> where T : DomainEvent
    {
        private readonly IDomainEventRepository _domainEventRepository;
        public DomainEventHandler(IDomainEventRepository domainEventRepository)
        {
            _domainEventRepository = domainEventRepository;
        }

        public Task Handle(T @event, CancellationToken cancellationToken)
        {
            @event.Flatten();
            @event.CorrelationId = Guid.NewGuid();
            @event.Content = JsonConvert.SerializeObject(@event.Args);

            _domainEventRepository.Add(@event);

            return Task.CompletedTask;
        }
    }
}
