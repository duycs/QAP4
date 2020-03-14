using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;

namespace QAP4.Repository
{
    public class PostLinkReposity : IPostLinkRepository
    {
        private QAPContext context;
        private DbSet<PostLinks> postLinkEntity;
        public PostLinkReposity(QAPContext context)
        {
            this.context = context;
            postLinkEntity = context.Set<PostLinks>();
        }

        public void Create(PostLinks model)
        {
            postLinkEntity.Add(model);
            context.SaveChanges();
        }

        public bool CreateOrDelete(int? postsId, int? relatedPostsId, int? linkTypeId)
        {
            var model = postLinkEntity.Where(w => w.PostId.Equals(postsId) && w.RelatedPostId.Equals(relatedPostsId) && w.LinkTypeId.Equals(linkTypeId)).FirstOrDefault();

            //create
            if (model == null)
            {
                model = new PostLinks();
                model.RelatedPostId = (int)relatedPostsId;
                model.PostId = (int)postsId;
                model.LinkTypeId = (byte)linkTypeId;
                model.CreationDate = DateTime.Now;
                postLinkEntity.Add(model);
                context.SaveChanges();
                return true;
            }
            else
            {
                //delele
                postLinkEntity.Remove(model);
                context.SaveChanges();
                return false;
            }
        }

        public void Delete(int? postsId, int? relatedPostsId)
        {
            //var sql = @"SELECT * FROM PostLinks WHERE relatedPostsId = " + relatedPostsId + " AND PostsId = " + postsId;
            //var model = postLinkEntity.FromSql<PostLinks>(sql).FirstOrDefault();
            var postLink = postLinkEntity.Where(w=>w.PostId == postsId && w.RelatedPostId == relatedPostsId).FirstOrDefault();
            if (postLink != null)
            {
                postLinkEntity.Remove(postLink);
                context.SaveChanges();
            }
        }

        // public bool IsPostLinkExist(int? postsId, int? relatedPostsId)
        // {
        //     var result = false;
        //     var sql = @"SELECT * FROM PostLinks WHERE relatedPostsId = " + relatedPostsId + " AND PostsId = " + postsId;
        //     var model = postLinkEntity.FromSql<PostLinks>(sql).FirstOrDefault();
        //     if (model != null)
        //     {
        //         result = true;
        //     }
        //     return result;
        // }

        public void Update(PostLinks model)
        {
            postLinkEntity.Update(model);
            context.SaveChanges();
        }
    }
}
