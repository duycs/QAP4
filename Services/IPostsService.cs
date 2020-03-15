using System;
using System.Collections.Generic;
using System.Linq;
using QAP4.Extensions;
using QAP4.Models;

namespace QAP4.Services
{
    public interface IPostsService
    {

        public Posts AddPosts(Posts posts);
        public void UpdatePosts(Posts posts);
        public Posts GetPostsById(int id);

        public IPagedList<Posts> GetPosts(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
       int userId = 0, int postsTypeId = 0, int parentId = 0);
       
        public IEnumerable<Posts> SearchPosts(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
         string q = null, int postsTypeId = 0);

        public IEnumerable<Posts> GetPostsSameTags(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
         int postsId = 0, string tagName = null, int postsTypeId = 0);

        public IEnumerable<Posts> GetPostsSameAuthor(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
         int postsId = 0, int userId = 0, int postsTypeId = 0);

        public IEnumerable<Posts> GetPostsByCreateDate(int pageIndex, int pageSize);

        public IEnumerable<Posts> GetPostsByType(int pageIndex, int pageSize, int postsTypeId);

        public IEnumerable<Posts> GetQuestionsQueue(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null, int userId = 0);

        public IEnumerable<Posts> GetAnswersNewest(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null, int userId = 0);
        
        public IEnumerable<Posts> GetChirldPosts(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null, int userId = 0, int postsTypeId = 0, int parentId = 0);

        public IEnumerable<Posts> GetSameQuestion(int pageIndex = 0, int pageSize = 0, string orderBy = null,
         string title = null, int postsTypeId = AppConstants.PostsType.QUESTION);

        public IEnumerable<Posts> GetPostsByOwnerUserId(int pageIndex, int pageSize, string orderBy, int userId);

        public Posts GetByFriendlyUrl(string friendlyUrl);
    }
}