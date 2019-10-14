using System;
using System.Collections.Generic;
using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.Core.Specification;

namespace QAP4.Application.Services
{
    /// <summary>
    /// TODO: remove userId in Add, Update
    /// </summary>
    public interface IPostsService
    {
        /// <summary>
        /// Find posts by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Posts FindPostsById(int id);

        /// <summary>
        /// Find list posts by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        IList<Posts> FindPostsByIds(int[] ids);

        /// <summary>
        /// Find posts by complex conditions
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postsType"></param>
        /// <param name="parentId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Posts> FindPostsByUserAndType(int userId, int? postsType, int? parentId,
            DateTime? dateFrom = null, DateTime? dateTo = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Add posts
        /// </summary>
        /// <param name="posts"></param>
        void AddPosts(int userId, Posts posts);
        void UpdatePosts(Posts posts);

        /// <summary>
        /// Update posts
        /// </summary>
        /// <param name="posts"></param>
        void UpdatePostsInEditor(int userId, Posts posts);

        /// <summary>
        /// Remove posts
        /// </summary>
        /// <param name="posts"></param>
        void RemovePosts(Posts posts);

    }
}