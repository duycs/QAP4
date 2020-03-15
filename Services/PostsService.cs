using System;
using System.Collections.Generic;
using System.Linq;
using QAP4.Extensions;
using QAP4.Models;
using QAP4.Repository;

namespace QAP4.Services
{

    public class PostsService : IPostsService
    {
        private readonly IRepository<Posts> _postsRepository;

        public PostsService(IRepository<Posts> postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public Posts AddPosts(Posts posts)
        {
            if (posts == null)
                throw new ArgumentNullException(nameof(posts));

            return _postsRepository.Insert(posts);

            // notify event

        }
        public void UpdatePosts(Posts posts)
        {
            if (posts == null)
                throw new ArgumentNullException(nameof(posts));

            _postsRepository.Update(posts);

            // notify event

        }

        public Posts GetPostsById(int id)
        {
            return _postsRepository.Table.Where(w => w.Id == id).FirstOrDefault();
        }

        public IPagedList<Posts> GetPosts(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
        int userId = 0, int postsTypeId = 0, int parentId = 0)
        {
            var query = _postsRepository.Table;

            query = query.Where(w => w.DeletionDate == null);

            if (parentId > 0)
                query = query.Where(w => w.ParentId == parentId);

            if (userId > 0)
                query = query.Where(w => w.OwnerUserId == userId);

            if (postsTypeId > 0)
                query = query.Where(w => w.PostTypeId == postsTypeId);


            switch (orderBy)
            {
                case AppConstants.QueryString.CREATION_DATE:
                    query = query.OrderBy(o => o.CreationDate);
                    break;

                case AppConstants.QueryString.VOTE_COUNT:
                    query = query.OrderBy(o => o.VoteCount);
                    break;

                case AppConstants.QueryString.VIEW_COUNT:
                    query = query.OrderBy(o => o.ViewCount);
                    break;

                case AppConstants.QueryString.ANSWER_COUNT:
                    query = query.OrderBy(o => o.AnswerCount);
                    break;

                case AppConstants.QueryString.SCORE:
                    query = query.OrderBy(o => o.Score);
                    break;

                default:
                    query = query.OrderBy(o => o.CreationDate);
                    break;
            }

            var posts = new PagedList<Posts>(query, pageIndex, pageSize);
            return posts;


            // var pageSize = AppConstants.Paging.PAGE_SIZE;
            // int total = postsEntity.Count();
            // var skip = pageSize * (page - 1);
            // var sql = @"SELECT * FROM Posts ";

            // if (userId != 0 || postsTypeId != 0 || parentId != 0)
            // {
            //     sql += " WHERE ";
            // }

            // if (parentId != 0)
            // {
            //     sql += " ParentId = " + parentId + " AND ";
            // }

            // if (userId != 0)
            // {
            //     sql += " OwnerUserId = " + userId + " AND ";
            // }

            // // posts type
            // if (postsTypeId != 0)
            // {
            //     sql += " PostTypeId = " + postsTypeId;
            // }

            // //remove AND at last if exsist
            // var end4Str = sql.Substring(sql.Length - 4);
            // if (end4Str.Equals("AND "))
            // {
            //     sql = sql.Remove(sql.Length - 4);
            // }

            // // order by
            // if (!string.IsNullOrEmpty(orderBy))
            // {
            //     switch (orderBy)
            //     {
            //         case AppConstants.QueryString.CREATION_DATE:
            //             sql += " ORDER BY " + nameof(Posts.CreationDate) + " DESC ";
            //             break;
            //         case AppConstants.QueryString.VOTE_COUNT:
            //             sql += " ORDER BY " + nameof(Posts.VoteCount) + " DESC ";
            //             break;
            //         case AppConstants.QueryString.VIEW_COUNT:
            //             sql += " ORDER BY " + nameof(Posts.ViewCount) + " DESC ";
            //             break;
            //         case AppConstants.QueryString.ANSWER_COUNT:
            //             sql += " ORDER BY " + nameof(Posts.AnswerCount) + " DESC ";
            //             break;
            //         case AppConstants.QueryString.SCORE:
            //             sql += " ORDER BY " + nameof(Posts.Score) + " DESC ";
            //             break;
            //     }
            // }
            // else
            // {
            //     sql += " ORDER BY " + nameof(Posts.CreationDate) + " DESC ";
            // }

            // paging
            // if (page != 0)
            // {
            //     sql += " OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            // }

            // return postsEntity.FromSqlRaw<Posts>(sql).AsEnumerable();
        }

        // Move many function to PostsService
        //search full text
        public IEnumerable<Posts> SearchPosts(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
         string q = null, int postsTypeId = 0)
        {
            // try
            // {
            //     var pageSize = AppConstants.Paging.PAGE_SIZE;
            //     int total = postsEntity.Count();
            //     var skip = pageSize * (page - 1);

            //     //var sql = "";
            //     if (0 == postsTypeId)
            //     {
            //         //sql = @"SELECT *  FROM Posts WHERE FREETEXT (Title, '" + q + "') OR FREETEXT (BodyContent, '" + q + "') ORDER BY Title OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            //         var allPosts = postsEntity.Where(w => w.Title.Contains(q) || w.BodyContent.Contains(q))
            //         .OrderBy(o => o.Title)
            //         .Skip(skip).Take(pageSize).ToList();

            //         return allPosts;
            //     }

            //     // else
            //     // {
            //     //     sql = @"SELECT *  FROM Posts WHERE FREETEXT (Title, '" + q + "') OR FREETEXT (BodyContent, '" + q + "') AND PostTypeId=" + postsTypeId + " ORDER BY Title OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            //     // }

            //     var posts = postsEntity.Where(w => w.PostTypeId == postsTypeId && (w.Title.Contains(q) || w.BodyContent.Contains(q)))
            //      .OrderBy(o => o.Title)
            //      .Skip(skip).Take(pageSize).ToList();

            //     return posts;

            //     //return postsEntity.FromSql<Posts>(sql);
            // }
            // catch (Exception ex)
            // {
            //     throw ex;
            // }

            // Search by title and body content

            var allPosts = GetPosts(pageIndex, pageSize, orderBy, 0, postsTypeId);
            var posts = allPosts.Where(w => w.PostTypeId == postsTypeId
                    && (w.Title.Contains(q) || w.BodyContent.Contains(q)))
                    .OrderBy(o => o.Title);

            return posts;
        }


        //get with relation orther object

        public IEnumerable<Posts> GetPostsSameTags(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
         int postsId = 0, string tagName = null, int postsTypeId = 0)
        {
            // var topLst = new List<Posts>().AsEnumerable();
            // if (!string.IsNullOrEmpty(tags))
            // {
            //     string[] tagsLst = tags.Split(',');
            //     var postsLst = new List<Posts>();
            //     // var sql = "";
            //     if (0 == postsTypeId)
            //     {
            //         foreach (string tagName in tagsLst)
            //         {
            //             var tag = context.Tags.Where(w => w.Name.Contains(tagName)).FirstOrDefault();
            //             //sql = @"SELECT TOP 3 p.* FROM Posts p INNER JOIN PostsTag pt ON p.Id=pt.PostsId WHERE pt.PostsId!=" + postsId + " AND pt.TagId=(SELECT Id from Tags WHERE Name=N'" + tag + "') ORDER BY p.VoteCount DESC;";
            //             //var subPostsLst = postsEntity.FromSql<Posts>(sql).AsEnumerable();
            //             var subPostsLst = (from a in context.Posts
            //                                join b in context.PostsTag
            //                                on a.Id equals b.PostsId
            //                                where b.PostsId != postsId && b.TagId == tag.Id
            //                                orderby a.VoteCount
            //                                select a).Take(3).ToList();

            //             postsLst.AddRange(subPostsLst);
            //         }
            //     }
            //     else
            //     {
            //         foreach (string tagName in tagsLst)
            //         {
            //             var tag = context.Tags.Where(w => w.Name.Contains(tagName)).FirstOrDefault();
            //             //sql = @"SELECT TOP 3 p.* FROM Posts p INNER JOIN PostsTag pt ON p.Id=pt.PostsId WHERE pt.PostsId!=" + postsId + " AND pt.TagId=(SELECT Id from Tags WHERE Name=N'" + tag + "') AND PostTypeId=" + postsTypeId + " ORDER BY p.VoteCount DESC;";
            //             //var subPostsLst = postsEntity.FromSql<Posts>(sql).AsEnumerable();
            //             var subPostsLst = (from a in context.Posts
            //                                join b in context.PostsTag
            //                                on a.Id equals b.PostsId
            //                                where b.PostsId != postsId && b.TagId == tag.Id && a.PostTypeId == postsTypeId
            //                                orderby a.VoteCount
            //                                select a).Take(3).ToList();
            //             postsLst.AddRange(subPostsLst);
            //         }
            //     }
            //     var distinctLst = postsLst.GroupBy(x => x.Id)
            //                .Select(g => g.First())
            //                .ToList();
            //     topLst = distinctLst.OrderByDescending(o => o.VoteCount).Take(3);
            // }
            // return topLst;

            var allPosts = GetPosts(pageIndex, pageSize, orderBy, 0, postsTypeId);
            var posts = allPosts.Where(w => w.Id != postsId && w.Tags.Contains(tagName)).OrderBy(o => o.Title);

            return posts;
        }

        public IEnumerable<Posts> GetPostsSameAuthor(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null,
         int postsId = 0, int userId = 0, int postsTypeId = 0)
        {
            //var sql = "";
            // if (0 == postsTypeId)
            // {
            //     // sql = @"SELECT TOP 3.* FROM Posts 
            //     //         WHERE
            //     //         Id!=" + postsId + " AND OwnerUserId=" + userId + " ORDER BY CreationDate DESC";

            //     //OFFSET 1
            //     //FETCH NEXT 10 ROWS ONLY";

            //     var posts = context.Posts.Where(w => w.Id != postsId && w.OwnerUserId == userId)
            //     .OrderBy(o => o.CreationDate).Take(3).ToList();

            //     return posts;
            // }

            // // sql = @"SELECT TOP 3.* FROM Posts 
            // //             WHERE
            // //             Id!=" + postsId + " AND OwnerUserId=" + userId + " AND PostTypeId=" + postsTypeId + " ORDER BY CreationDate DESC";
            // //OFFSET 1
            // //FETCH NEXT 10 ROWS ONLY";

            // var postsByType = context.Posts.Where(w => w.Id != postsId && w.OwnerUserId == userId && w.PostTypeId == postsTypeId)
            // .OrderBy(o => o.CreationDate).Take(3).ToList();

            // return postsByType;

            //return postsEntity.FromSql<Posts>(sql);

            var allPosts = GetPosts(pageIndex, pageSize, orderBy, userId, postsTypeId);
            var posts = allPosts.Where(w => w.Id != postsId).OrderBy(o => o.Title);

            return posts;
        }


        public IEnumerable<Posts> GetPostsByCreateDate(int pageIndex, int pageSize)
        {
            //return GetPosts(page, AppConstants.QueryString.CREATION_DATE, 0, 0, 0);
            return GetPosts(pageIndex, pageSize, AppConstants.QueryString.CREATION_DATE);
        }

        public IEnumerable<Posts> GetPostsByType(int pageIndex, int pageSize, int postsTypeId)
        {
            //return GetPosts(page, "", 0, postTypeId, 0);
            return GetPosts(pageIndex, pageSize, null, 0, postsTypeId);
        }

        public IEnumerable<Posts> GetQuestionsQueue(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null, int userId = 0)
        {

            // Get all posts then filter
            var allPosts = GetPosts(pageIndex, pageSize, orderBy, userId);
            var posts = allPosts.Where(w => w.AnswerCount == 0 && w.PostTypeId == AppConstants.PostsType.QUESTION);
            return posts;

            // var pageSize = AppConstants.Paging.PAGE_SIZE;
            // int total = postsEntity.Count();
            // var skip = pageSize * (page - 1);
            // var sql = @"SELECT * FROM Posts WHERE ";

            // // owner user
            // if (userId != 0)
            // {
            //     sql += " OwnerUserId = " + userId + " AND ";
            // }

            // sql += " AnswerCount = 0 AND PostTypeId = 2 ORDER BY CreationDate DESC ";

            // // paging
            // if (page != 0)
            // {
            //     sql += " OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            // }

            // return postsEntity.FromSqlRaw<Posts>(sql);
        }

        public IEnumerable<Posts> GetAnswersNewest(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null, int userId = 0)
        {
            // Get all posts then filter
            var allPosts = GetPosts(pageIndex, pageSize, orderBy, userId);
            var posts = allPosts.Where(w => w.AnswerCount > 0 && w.PostTypeId == AppConstants.PostsType.QUESTION);
            return posts;

            // var pageSize = AppConstants.Paging.PAGE_SIZE;
            // int total = postsEntity.Count();
            // var skip = pageSize * (page - 1);
            // var sql = @"SELECT * FROM Posts WHERE ";

            // // owner user
            // if (userId != 0)
            // {
            //     sql += " OwnerUserId = " + userId + " AND ";
            // }

            // sql += " AnswerCount > 0 AND PostTypeId = 2 ORDER BY CreationDate DESC ";

            // // paging
            // if (page != 0)
            // {
            //     sql += " OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            // }

            // return postsEntity.FromSqlRaw<Posts>(sql);
        }

        public IEnumerable<Posts> GetChirldPosts(int pageIndex = 0, int pageSize = int.MaxValue, string orderBy = null, int userId = 0, int postsTypeId = 0, int parentId = 0)
        {
            // Get all posts then filter
            return GetPosts(pageIndex, pageSize, orderBy, userId, postsTypeId, parentId);

            // var pageSize = AppConstants.Paging.PAGE_SIZE;
            // int total = postsEntity.Count();
            // var skip = pageSize * (page - 1);
            // var sql = @"SELECT * FROM Posts WHERE ";

            // // owner user
            // if (parentId <= 0)
            // {
            //     return null;
            // }

            // sql += " ParentId = " + parentId + " AND PostTypeId = " + postsTypeId + " ORDER BY CreationDate DESC ";

            // // paging
            // if (page != 0)
            // {
            //     sql += " OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            // }

            // return postsEntity.FromSqlRaw<Posts>(sql);
        }

        public IEnumerable<Posts> GetSameQuestion(int pageIndex = 0, int pageSize = 0, string orderBy = null,
         string title = null, int postsTypeId = AppConstants.PostsType.QUESTION)
        {
            if (string.IsNullOrEmpty(title))
                return null;

            var q1 = StringExtensions.GetWords(title, 0, 4);
            var q2 = StringExtensions.GetWords(title, 4, 8);
            var q3 = StringExtensions.GetWords(title, 8, 10);

            var resultSearch1 = SearchPosts(pageIndex, pageSize, orderBy, q1, postsTypeId);
            var resultSearch2 = SearchPosts(pageIndex, pageSize, orderBy, q2, postsTypeId);
            var resultSearch3 = SearchPosts(pageIndex, pageSize, orderBy, q3, postsTypeId);
            var sameQuestions = new List<Posts>();
            sameQuestions.AddRange(resultSearch1);
            sameQuestions.AddRange(resultSearch2);
            sameQuestions.AddRange(resultSearch3);
            var distinctLst = sameQuestions.GroupBy(x => x.Id)
                       .Select(g => g.First())
                       .ToList();
            //var topLst = distinctLst.OrderByDescending(o => o.VoteCount).Take(take);

            return distinctLst;
        }

        // public IEnumerable<Posts> GetPostsFeed(int page, int? userId)
        // {
        //     try
        //     {
        //         var sqlTopVote = @"SELECT TOP 3.* FROM Posts ORDER BY VoteCount DESC";
        //         var sqlLatest = @"SELECT * FROM Posts WHERE PostTypeId != 3 ORDER BY CreationDate DESC";

        //         var lstTopVote = postsEntity.FromSql<Posts>(sqlTopVote).AsEnumerable().ToList();
        //         var lstLatest = postsEntity.FromSql<Posts>(sqlLatest).AsEnumerable().ToList();
        //         var lstFeed = lstTopVote;
        //         foreach (var item in lstTopVote)
        //         {
        //             lstLatest.Remove(item);
        //         }
        //         lstFeed.AddRange(lstLatest);

        //         return lstFeed;
        //     }
        //     catch
        //     {
        //         return null;
        //     }
        // }

        public IEnumerable<Posts> GetPostsByOwnerUserId(int pageIndex, int pageSize, string orderBy, int userId)
        {
            // try
            // {
            //     // var sqlPostsByOwnerUserId = @"SELECT * FROM Posts WHERE OwnerUserId='" + userId + "'";
            //     // var postsList = postsEntity.FromSql<Posts>(sqlPostsByOwnerUserId).AsEnumerable().ToList();
            //     //var postsList = postsEntity.Where(w => w.OwnerUserId == userId).ToList();
            //     //return postsList;

            // }
            // catch (Exception ex)
            // {
            //     throw ex;
            // }

            return GetPosts(pageIndex, pageSize, orderBy, userId);
        }

        // public IEnumerable<Posts> GetAll()
        // {
        //     return postsEntity.ToList();
        // }

        public Posts GetByFriendlyUrl(string friendlyUrl)
        {
            //return postsEntity.FirstOrDefault(w => w.FriendlyUrl == friendlyUrl);
            var posts = _postsRepository.Table.Where(w => w.FriendlyUrl == friendlyUrl).FirstOrDefault();
            return posts;
        }

    }
}