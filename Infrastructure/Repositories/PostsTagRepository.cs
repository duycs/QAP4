using Microsoft.EntityFrameworkCore;
using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Infrastructure.Repositories
{
    public class PostsTagRepository : IPostsTagRepository
    {
        private QAPContext context;
        private DbSet<PostsTag> postsTagEntity;
        public PostsTagRepository(QAPContext context)
        {
            this.context = context;
            postsTagEntity = context.Set<PostsTag>();
        }

        public void Create(int postsId, int tagId)
        {
            var model = postsTagEntity.Where(o => o.PostsId.Equals(postsId) && o.TagId.Equals(tagId)).FirstOrDefault();
            if (model == null)
            {
                model = new PostsTag();
                model.TagId = tagId;
                model.PostsId = postsId;
                postsTagEntity.Add(model);
                context.SaveChanges();
            }
        }

        public void Delete(int? postsId, int? tagId)
        {
            var model = postsTagEntity.Where(o => o.PostsId.Equals(postsId) && o.TagId.Equals(tagId)).FirstOrDefault();
            postsTagEntity.Remove(model);
            context.SaveChanges();
        }

        public bool CreateOrDelete(int postsId, int tagId)
        {

            var model = postsTagEntity.Where(o => o.PostsId.Equals(postsId) && o.TagId.Equals(tagId)).FirstOrDefault();

            //create
            if (model == null)
            {
                model = new PostsTag();
                model.TagId = tagId;
                model.PostsId = postsId;
                postsTagEntity.Add(model);
                context.SaveChanges();
                return true;
            }
            else
            {
                //delele
                postsTagEntity.Remove(model);
                context.SaveChanges();
                return false;
            }
        }
    }
}
