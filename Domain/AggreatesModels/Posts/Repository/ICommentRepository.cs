using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.Core.Repositories;

namespace QAP4.Infrastructure.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        //IEnumerable<Comment> GetCommentById(int? id);
        IEnumerable<Comment> GetCommentsByPosts(int postsId);
        void Create(Comment model);
        void Update(Comment model);
        void Delete(int? id);
    }
}
