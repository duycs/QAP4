using System;
using System.Collections.Generic;
using System.Linq;
using QAP4.Extensions;
using QAP4.Models;

namespace QAP4.Application.Services
{
    public interface IPostsService
    {

        Posts AddPosts(Posts posts);
        void UpdatePosts(Posts posts);
        Posts GetPostsById(int id);

        IPagedList<Posts> GetPosts(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
       int userId = 0, int postsTypeId = 0, int parentId = 0);

        IEnumerable<Posts> SearchPosts(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
         string q = null, int postsTypeId = 0);

        IEnumerable<Posts> GetPostsSameTags(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
         int postsId = 0, string tagName = null, int postsTypeId = 0);

        IEnumerable<Posts> GetPostsSameAuthor(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
         int postsId = 0, int userId = 0, int postsTypeId = 0);

        IEnumerable<Posts> GetPostsByCreateDate(int pageIndex, int pageSize);

        IEnumerable<Posts> GetPostsByType(int pageIndex, int pageSize, int postsTypeId);

        IEnumerable<Posts> GetQuestionsQueue(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null, int userId = 0);

        IEnumerable<Posts> GetAnswersNewest(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null, int userId = 0);

        IEnumerable<Posts> GetChirldPosts(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null, int userId = 0, int postsTypeId = 0, int parentId = 0);

        IEnumerable<Posts> GetSameQuestion(int pageIndex = 0, int pageSize = 0, string orderBy = null,
         string title = null, int postsTypeId = AppConstants.PostsType.QUESTION);

        IEnumerable<Posts> GetPostsByOwnerUserId(int pageIndex, int pageSize, string orderBy, int userId);

        Posts GetByFriendlyUrl(string friendlyUrl);


        #region  PostsLink relation
        void AddPostsLink(PostLinks postsLinkViewModel);
        bool CreateOrDeletePostsLink(int? postsId, int? relatedPostsId, int? linkTypeId);
        void DeletePostsLink(int? postsId, int? relatedPostsId);
        void Update(PostLinks viewModel);

        #endregion PostsLink relation 
    }
}