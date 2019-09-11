using Microsoft.EntityFrameworkCore;
using QAP4.Extensions;
using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Repository
{
    public class PostsRepository : IPostsRepository
    {
        private QAPContext context;
        private DbSet<Posts> postsEntity;
        public PostsRepository(QAPContext context)
        {
            this.context = context;
            postsEntity = context.Set<Posts>();
        }

        //get methods

        public Posts GetPosts(int? id)
        {
            return postsEntity.Where(o => o.Id.Equals(id) && o.LastActivityDate !=null && o.DeletionDate ==null).FirstOrDefault();
        }

        public IEnumerable<Posts> GetPostsFeature(int page, int postsTypeId)
        {
            //var pageSize = AppConstants.Paging.PAGE_SIZE;
            //var skip = pageSize * (page - 1);
            //int total = postsEntity.Count();
            var sql = "";

            //if (pageSize < total)
            //    return postsEntity.OrderByDescending(o => o.VoteCount).Skip(skip).Take(pageSize).AsEnumerable();
            //else

            if (0 == postsTypeId)
            {
                sql = @"SELECT * FROM Posts ORDER BY VoteCount DESC";
                //OFFSET 1
                //FETCH NEXT 10 ROWS ONLY";
            }
            else
            {
                sql = @"SELECT * FROM Posts 
                        WHERE
                        PostTypeId=" + postsTypeId + " ORDER BY VoteCount DESC";
                //OFFSET 1
                //FETCH NEXT 10 ROWS ONLY";
            }
            return postsEntity.FromSql<Posts>(sql).Where(w=>w.LastActivityDate !=null && w.DeletionDate ==null).AsEnumerable();
        }

        public IEnumerable<Posts> GetPostsNewest(int page, int postsTypeId)
        {
            var pageSize = AppConstants.Paging.PAGE_SIZE;
            int total = postsEntity.Count();
            var skip = pageSize * (page - 1);
            var sql = "";


            if (0 == postsTypeId)
            {
                sql = @"SELECT * FROM Posts ORDER BY CreationDate ASC OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            }
            else
            {
                sql = @"SELECT * FROM Posts 
                        WHERE
                        PostTypeId=" + postsTypeId + " ORDER BY CreationDate ASC OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";

            }
            return postsEntity.FromSql<Posts>(sql).Where(w=>w.LastActivityDate !=null && w.DeletionDate ==null).AsEnumerable();
        }


        public IEnumerable<Posts> GetPostsByUser(int page, int? userId, int postsTypeId)
        {
            var pageSize = AppConstants.Paging.PAGE_SIZE;
            int total = postsEntity.Count();
            var skip = pageSize * (page - 1);
            var sql = "";
            if (0 == postsTypeId)
            {
                sql = @"SELECT * FROM Posts 
                        WHERE
                        OwnerUserId=" + userId + " ORDER BY CreationDate ASC OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";

            }
            else
            {
                sql = @"SELECT * FROM Posts 
                        WHERE
                        OwnerUserId=" + userId + " AND PostTypeId=" + postsTypeId + " ORDER BY CreationDate ASC OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            }
            return postsEntity.FromSql<Posts>(sql).Where(w=>w.LastActivityDate !=null && w.DeletionDate ==null).AsEnumerable();
        }

        //add method
        public int Add(Posts item)
        {
            context.Entry(item).State = EntityState.Added;
            context.SaveChanges();
            return item.Id;
        }
        //delete method
        public int Delete(int? id)
        {
            var item = postsEntity.Where(o => o.Id.Equals(id)).FirstOrDefault();
            postsEntity.Remove(item);
            context.SaveChanges();
            return item.Id;
        }

        //update method
        public int Update(Posts item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
            return item.Id;
        }

        //search full text
        public IEnumerable<Posts> SearchInPosts(int page, string q, int postsTypeId)
        {
            try
            {
                var pageSize = AppConstants.Paging.PAGE_SIZE;
                int total = postsEntity.Count();
                var skip = pageSize * (page - 1);

                var sql = "";
                if (0 == postsTypeId)
                {
                    sql = @"SELECT *  FROM Posts WHERE FREETEXT (Title, '" + q + "') OR FREETEXT (BodyContent, '" + q + "') ORDER BY Title OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                //else if (AppConstants.PostsType.QUESTION.Equals(postsTypeId))
                //{
                //    sql = @"SELECT *  FROM Posts WHERE FREETEXT (Title, '" + q + "') AND PostTypeId=" + postsTypeId + " ORDER BY Title OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                //}
                else
                {
                    sql = @"SELECT *  FROM Posts WHERE FREETEXT (Title, '" + q + "') OR FREETEXT (BodyContent, '" + q + "') AND PostTypeId=" + postsTypeId + " ORDER BY Title OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }

                return postsEntity.FromSql<Posts>(sql).Where(w=>w.LastActivityDate !=null && w.DeletionDate ==null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //get with relation orther object

        public IEnumerable<Posts> GetPostsSameTags(int postsId, string tags, int postsTypeId)
        {
            var topLst = new List<Posts>().AsEnumerable();
            if (!string.IsNullOrEmpty(tags))
            {
                string[] tagsLst = tags.Split(',');
                var postsLst = new List<Posts>();
                var sql = "";
                if (0 == postsTypeId)
                {
                    foreach (string tag in tagsLst)
                    {
                        sql = @"SELECT TOP 3 p.* FROM Posts p INNER JOIN PostsTag pt ON p.Id=pt.PostsId WHERE pt.PostsId!=" + postsId + " AND pt.TagId=(SELECT Id from Tags WHERE Name=N'" + tag + "') ORDER BY p.VoteCount DESC;";
                        var subPostsLst = postsEntity.FromSql<Posts>(sql).AsEnumerable();
                        postsLst.AddRange(subPostsLst);
                    }
                }
                else
                {
                    foreach (string tag in tagsLst)
                    {
                        sql = @"SELECT TOP 3 p.* FROM Posts p INNER JOIN PostsTag pt ON p.Id=pt.PostsId WHERE pt.PostsId!=" + postsId + " AND pt.TagId=(SELECT Id from Tags WHERE Name=N'" + tag + "') AND PostTypeId=" + postsTypeId + " ORDER BY p.VoteCount DESC;";
                        var subPostsLst = postsEntity.FromSql<Posts>(sql).AsEnumerable();
                        postsLst.AddRange(subPostsLst);
                    }
                }
                var distinctLst = postsLst.GroupBy(x => x.Id)
                           .Select(g => g.First())
                           .ToList();
                topLst = distinctLst.Where(w=>w.LastActivityDate !=null && w.DeletionDate ==null).OrderByDescending(o => o.VoteCount).Take(3);
            }
            return topLst;
        }

        public IEnumerable<Posts> GetPostsSameAuthor(int postsId, int? userId, int postsTypeId)
        {
            var sql = "";
            if (0 == postsTypeId)
            {
                sql = @"SELECT TOP 3.* FROM Posts 
                        WHERE
                        Id!=" + postsId + " AND OwnerUserId=" + userId + " ORDER BY CreationDate DESC";
                //OFFSET 1
                //FETCH NEXT 10 ROWS ONLY";
            }
            else
            {
                sql = @"SELECT TOP 3.* FROM Posts 
                        WHERE
                        Id!=" + postsId + " AND OwnerUserId=" + userId + " AND PostTypeId=" + postsTypeId + " ORDER BY CreationDate DESC";
                //OFFSET 1
                //FETCH NEXT 10 ROWS ONLY";
            }
            return postsEntity.FromSql<Posts>(sql).Where(w=>w.LastActivityDate !=null && w.DeletionDate ==null);
        }



        public IEnumerable<Posts> GetPosts(int page, string orderBy, int userId, int postsTypeId, int parentId)
        {
            var pageSize = AppConstants.Paging.PAGE_SIZE;
            int total = postsEntity.Count();
            var skip = pageSize * (page - 1);
            var sql = @"SELECT * FROM Posts ";

            if (userId != 0 || postsTypeId != 0 || parentId != 0)
            {
                sql += " WHERE ";
            }

            if (parentId != 0)
            {
                sql += " ParentId = " + parentId + " AND ";
            }

            if (userId != 0)
            {
                sql += " OwnerUserId = " + userId + " AND ";
            }

            // posts type
            if (postsTypeId != 0)
            {
                sql += " PostTypeId = " + postsTypeId;
            }

            //remove AND at last if exsist
            var end4Str = sql.Substring(sql.Length - 4);
            if (end4Str.Equals("AND "))
            {
                sql = sql.Remove(sql.Length - 4);
            }

            // order by
            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy)
                {
                    case AppConstants.QueryString.CREATION_DATE:
                        sql += " ORDER BY " + nameof(Posts.CreationDate) + " DESC ";
                        break;
                    case AppConstants.QueryString.VOTE_COUNT:
                        sql += " ORDER BY " + nameof(Posts.VoteCount) + " DESC ";
                        break;
                    case AppConstants.QueryString.VIEW_COUNT:
                        sql += " ORDER BY " + nameof(Posts.ViewCount) + " DESC ";
                        break;
                    case AppConstants.QueryString.ANSWER_COUNT:
                        sql += " ORDER BY " + nameof(Posts.AnswerCount) + " DESC ";
                        break;
                    case AppConstants.QueryString.SCORE:
                        sql += " ORDER BY " + nameof(Posts.Score) + " DESC ";
                        break;
                }
            }
            else
            {
                sql += " ORDER BY " + nameof(Posts.CreationDate) + " DESC ";
            }


            // paging
            if (page != 0)
            {
                sql += " OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            }

            return postsEntity.FromSql<Posts>(sql).AsEnumerable().Where(w=>w.LastActivityDate !=null && w.DeletionDate ==null);
        }


        public IEnumerable<Posts> GetPostsByCreateDate(int page)
        {
            return GetPosts(page, AppConstants.QueryString.CREATION_DATE, 0, 0, 0);
        }

        public IEnumerable<Posts> GetPostsByType(int page, int postTypeId)
        {
            return GetPosts(page, "", 0, postTypeId, 0);
        }

        public IEnumerable<Posts> GetQuestionsQueue(int page, int userId)
        {
            var pageSize = AppConstants.Paging.PAGE_SIZE;
            int total = postsEntity.Count();
            var skip = pageSize * (page - 1);
            var sql = @"SELECT * FROM Posts WHERE ";

            // owner user
            if (userId != 0)
            {
                sql += " OwnerUserId = " + userId + " AND ";
            }

            sql += " AnswerCount = 0 AND PostTypeId = 2 ORDER BY CreationDate DESC ";

            // paging
            if (page != 0)
            {
                sql += " OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            }

            return postsEntity.FromSql<Posts>(sql);
        }

        public IEnumerable<Posts> GetAnswersNewest(int page, int userId)
        {
            var pageSize = AppConstants.Paging.PAGE_SIZE;
            int total = postsEntity.Count();
            var skip = pageSize * (page - 1);
            var sql = @"SELECT * FROM Posts WHERE ";

            // owner user
            if (userId != 0)
            {
                sql += " OwnerUserId = " + userId + " AND ";
            }

            sql += " AnswerCount > 0 AND PostTypeId = 2 ORDER BY CreationDate DESC ";

            // paging
            if (page != 0)
            {
                sql += " OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            }

            return postsEntity.FromSql<Posts>(sql);
        }

        public IEnumerable<Posts> GetChirldPosts(int page, int parentId, int postsTypeId)
        {
            var pageSize = AppConstants.Paging.PAGE_SIZE;
            int total = postsEntity.Count();
            var skip = pageSize * (page - 1);
            var sql = @"SELECT * FROM Posts WHERE ";

            // owner user
            if (parentId <= 0)
            {
                return null;
            }

            sql += " ParentId = " + parentId + " AND PostTypeId = " + postsTypeId + " ORDER BY CreationDate DESC ";

            // paging
            if (page != 0)
            {
                sql += " OFFSET " + skip + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            }

            return postsEntity.FromSql<Posts>(sql);
        }

        public IEnumerable<Posts> GetSameQuestion(string title, int take)
        {
            var q1 = StringExtensions.GetWords(title, 0, 4);
            var q2 = StringExtensions.GetWords(title, 4, 8);
            var q3 = StringExtensions.GetWords(title, 8, 10);

            var resultSearch1 = SearchInPosts(1, q1, AppConstants.PostsType.QUESTION);
            var resultSearch2 = SearchInPosts(1, q2, AppConstants.PostsType.QUESTION);
            var resultSearch3 = SearchInPosts(1, q3, AppConstants.PostsType.QUESTION);
            var sameQuestions = new List<Posts>();
            sameQuestions.AddRange(resultSearch1);
            sameQuestions.AddRange(resultSearch2);
            sameQuestions.AddRange(resultSearch3);
            var distinctLst = sameQuestions.GroupBy(x => x.Id)
                       .Select(g => g.First())
                       .ToList();
            var topLst = distinctLst.OrderByDescending(o => o.VoteCount).Take(take);

            return topLst;
        }

        public IEnumerable<Posts> GetPostsFeed(int page, int? userId)
        {
            try
            {
                var sqlTopVote = @"SELECT TOP 3.* FROM Posts ORDER BY VoteCount DESC";
                var sqlLatest = @"SELECT * FROM Posts WHERE PostTypeId != 3 ORDER BY CreationDate DESC";

                var lstTopVote = postsEntity.FromSql<Posts>(sqlTopVote).AsEnumerable().ToList();
                var lstLatest = postsEntity.FromSql<Posts>(sqlLatest).AsEnumerable().ToList();
                var lstFeed = lstTopVote;
                foreach (var item in lstTopVote)
                {
                    lstLatest.Remove(item);
                }
                lstFeed.AddRange(lstLatest);

                return lstFeed.Where(w=>w.LastActivityDate !=null && w.DeletionDate ==null);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Posts> GetPostsByOwnerUserId(int page, int? userId)
        {
            try
            {
                var sqlPostsByOwnerUserId = @"SELECT * FROM Posts WHERE OwnerUserId='" + userId + "'";
                var postsList = postsEntity.FromSql<Posts>(sqlPostsByOwnerUserId).AsEnumerable().ToList();
                return postsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Posts> GetAll()
        {
            return postsEntity.ToList().Where(w=>w.LastActivityDate !=null && w.DeletionDate ==null);
        }

        public Posts GetByFriendlyUrl(string friendlyUrl)
        {
            return postsEntity.FirstOrDefault(w=>w.FriendlyUrl == friendlyUrl && w.LastActivityDate !=null && w.DeletionDate ==null);
        }

        public void UpdateRange(List<Posts> postsList)
        {
            postsEntity.UpdateRange(postsList);
            context.SaveChanges();
        }

        public void UpdateRange(IEnumerable<Posts> postsList)
        {
            postsEntity.UpdateRange(postsList);
            context.SaveChanges();
        }
    }
}
