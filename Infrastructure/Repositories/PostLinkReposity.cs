using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;

namespace QAP4.Infrastructure.Repositories
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

        public bool CreateOrDelete(int? postId, int? relatedPostId, int? linkTypeId)
        {
            var model = postLinkEntity.Where(o => o.PostId.Equals(postId) && o.RelatedPostId.Equals(relatedPostId) && o.LinkTypeId.Equals(linkTypeId)).FirstOrDefault();

            //create
            if (model == null)
            {
                model = new PostLinks();
                model.RelatedPostId = (int)relatedPostId;
                model.PostId = (int)postId;
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

        public void Delete(int? postId, int? relatedPostId)
        {
            var sql = @"SELECT * FROM PostLinks WHERE RelatedPostId = " + relatedPostId + " AND PostsId = " + postId;
            var model = postLinkEntity.FromSql<PostLinks>(sql).FirstOrDefault();
            if (model != null)
            {
                postLinkEntity.Remove(model);
                context.SaveChanges();
            }
        }

        public bool IsPostLinkExist(int? postId, int? relatedPostId)
        {
            var result = false;
            var sql = @"SELECT * FROM PostLinks WHERE RelatedPostId = " + relatedPostId + " AND PostsId = " + postId;
            var model = postLinkEntity.FromSql<PostLinks>(sql).FirstOrDefault();
            if (model != null)
            {
                result = true;
            }
            return result;
        }

        public void Update(PostLinks model)
        {
            postLinkEntity.Update(model);
            context.SaveChanges();
        }
    }
}
