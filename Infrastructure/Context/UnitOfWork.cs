using System;
using QAP4.Models;

namespace QAP4.Infrastructure.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QAPContext _dbContext;
        private bool _disposed = false;
        public UnitOfWork(QAPContext context)
        {
            _dbContext = context;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) _dbContext?.Dispose();

            _disposed = true;
        }
        public int Commit()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().FullName);
            return _dbContext.SaveChanges();
        }
        public void Rollback()
        {
            Dispose();
        }
    }
}
