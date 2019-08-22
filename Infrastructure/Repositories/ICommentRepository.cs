using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Domain.AggreatesModels.Posts;
using QAP4.Domain.Core.Repositories;

namespace QAP4.Infrastructure.Repositories
{
    public interface ICommentRepository : IRepository<Comments>
    {
        IEnumerable<Comments> GetCommentById(int? id);
        IEnumerable<Comments> GetCommentsByPosts(int? postsId);
        void Create(Comments model);
        void Update(Comments model);
        void Delete(int? id);
    }
}
