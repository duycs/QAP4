using System;
using System.Collections.Generic;
using QAP4.Domain.Core.Models;
using QAP4.Domain.Core.Specification;

namespace QAP4.Domain.Core.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        T FindById(int? id);

        T FindSingleBySpec(ISpecification<T> spec);

        IEnumerable<T> Find(int page, int size);
        IEnumerable<T> Find();
        IEnumerable<T> Find(ISpecification<T> spec);
        T Add(T entity);
        void Update(T entity);

        void Delete(T entity);
    }
}