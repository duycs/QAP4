using System;
using System.Collections.Generic;
using System.Linq;
using QAP4.Domain.Core.Models;
using QAP4.Domain.Core.Specification;

namespace QAP4.Domain.Core.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        #region Properties
        /// <summary>
        /// Get a table with entity
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Get table no tracking only for read data
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Find by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T FindById(int id);

        /// <summary>
        /// Find single by spec condition
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        T FindSingleBySpec(ISpecification<T> spec);

        /// <summary>
        /// Find by page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        IEnumerable<T> Find(int page, int size);

        /// <summary>
        /// Find all
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Find();

        /// <summary>
        /// Find list by spec condition
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        IEnumerable<T> Find(ISpecification<T> spec);

        /// <summary>
        /// Add single
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// Add list
        /// </summary>
        /// <param name="entities"></param>
        void Add(IEnumerable<T> entities);

        /// <summary>
        /// Update single
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Update list
        /// </summary>
        /// <param name="entities"></param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Delete single
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="entities"></param>
        void Delete(IEnumerable<T> entities);
        #endregion Methods
    }
}