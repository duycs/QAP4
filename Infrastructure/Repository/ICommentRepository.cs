using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;

namespace QAP4.Repository
{
    public interface ICommentRepository
    {

        IEnumerable<Comments> GetComment(int? id);
        IEnumerable<Comments> GetCommentsByPosts(int? postsId);
        void Create(Comments model);
        void Update(Comments model);
        void Delete(int? id);
    }
}
