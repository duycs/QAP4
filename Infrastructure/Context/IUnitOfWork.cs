using System;

namespace QAP4.Infrastructure.Context
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        void Rollback();
    }
}
