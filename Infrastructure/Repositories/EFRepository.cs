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
        protected DbSet<T> _entities;

        public IQueryable<T> Table => Entities;

        public IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        public EFRepository(QAPContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get an instance entity
        /// </summary>
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _dbContext.Set<T>();

                return _entities;
            }
        }

        public T FindById(int id)
        {
            return _entities.Find(id);
        }
        public T FindSingleBySpec(ISpecification<T> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));

            return Find(spec).FirstOrDefault();
        }
        public IEnumerable<T> Find(int page, int size)
        {
            int count = _entities.Count();
            if (size > count)
                return Find();

            return _entities.Skip(size * (page - 1)).Take(size).AsEnumerable();
        }

        public IEnumerable<T> Find(ISpecification<T> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));

            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_entities.AsQueryable(), (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes, (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult.Where(spec.Criteria).AsEnumerable();
        }

        public T Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                return _entities.Add(entity).Entity;
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }
        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }
        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            try
            {
                _entities.Remove(entity);
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> Find()
        {
            return _entities.AsEnumerable();
        }

        public void Add(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                _entities.AddRange(entities);
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }

        public void Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                _entities.UpdateRange(entities);
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }

        public void Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                _entities.RemoveRange(entities);
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }
    }
}
