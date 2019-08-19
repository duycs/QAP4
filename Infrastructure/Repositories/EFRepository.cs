using System;
using System.Collections.Generic;
using System.Linq;
using QAP4.Domain.Core.Models;
using QAP4.Domain.Core.Repositories;
using QAP4.Domain.Core.Specification;
using Microsoft.EntityFrameworkCore;
using QAP4.Models;

namespace QAP4.Infrastructure.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly QAPContext _dbContext;

        public EFRepository(QAPContext dbContext)
        {
            _dbContext = dbContext;
        }
        public T FindById(int? id)
        {
            return _dbContext.Set<T>().Find(id);
        }
        public T FindSingleBySpec(ISpecification<T> spec)
        {
            return Find(spec).FirstOrDefault();
        }
        public IEnumerable<T> Find(int page, int size)
        {
            int count = _dbContext.Set<T>().Count();
            if (size > count)
                return Find();

            return _dbContext.Set<T>().Skip(size * (page-1)).Take(size).AsEnumerable();
        }

        public IEnumerable<T> Find(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(), (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult.Where(spec.Criteria).AsEnumerable();
        }
        public T Add(T entity)
        {
            return _dbContext.Set<T>().Add(entity).Entity;
        }
        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public IEnumerable<T> Find()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }
    }
}
