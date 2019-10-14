using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;
using QAP4.Domain.AggreatesModels.Posts.Models;

namespace QAP4.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private DbSet<Comment> commentEntity;
        public CommentRepository(QAPContext context)
        {
            this.context = context;
            commentEntity = context.Set<Comments>();
        }

        public void Create(Comments model)
        {
            commentEntity.Add(model);
            context.SaveChanges();
        }

        public void Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comments> GetComment(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comments> GetCommentsByPosts(int? postsId)
        {
            return commentEntity.Where(o => o.PostsId.Equals(postsId)).AsEnumerable();
        }

        public void Update(Comments model)
        {
            throw new NotImplementedException();
        }
    }
}
