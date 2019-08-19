using QAP4.Domain.Core.Events;

namespace QAP4.Domain.Core.Repositories
{
    public interface IDomainEventRepository
    {
        void Add<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent;

        //IEnumerable<DomainEventRecord> FindAll();
    }
}
