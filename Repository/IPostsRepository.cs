using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Repository
{
    public interface IPostsRepository
    {
        IEnumerable<Posts> GetAll();
        Posts GetPosts(int? id);
        Posts GetByFriendlyUrl(string friendlyUrl);
        // IEnumerable<Posts> GetPostsFeature(int page, int postsTypeId);
        // IEnumerable<Posts> GetPostsNewest(int page, int postsTypeId);
        // IEnumerable<Posts> GetPostsByUser(int page, int? userId, int postsTypeId);
        IEnumerable<Posts> GetPosts(int page, string orderBy, int userId, int postsTypeId, int parentId);
        IEnumerable<Posts> GetPostsByCreateDate(int page);
        IEnumerable<Posts> GetPostsByType(int page, int postTypeId);

        IEnumerable<Posts> GetQuestionsQueue(int page, int userId);
        IEnumerable<Posts> GetAnswersNewest(int page, int userId);
        IEnumerable<Posts> GetChirldPosts(int page, int parentId, int postsTypeId);
        IEnumerable<Posts> GetSameQuestion(string title, int take);

        int Add(Posts item);
        int Delete(int? id);
        int Update(Posts item);

        // void UpdateRange(IEnumerable<Posts> postsList);

        //get with relation other object
        IEnumerable<Posts> GetPostsSameTags(int postsId, string tags, int postsTypeId);
        IEnumerable<Posts> GetPostsSameAuthor(int postsId, int? userId, int postsTypeId);
        IEnumerable<Posts> SearchInPosts(int page, string key, int postsTypeId);
        // IEnumerable<Posts> GetPostsFeed(int page, int? userId);
        IEnumerable<Posts> GetPostsByOwnerUserId(int page, int? userId);

    }
}
