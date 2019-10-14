using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Infrastructure.Repositories
{
    public interface IPostsTagRepository : IRepository<PostsTag>
    {
        void Create(int postsId, int tagId);
        void Delete(int? postsId, int? tagId);

        //handler: create return true, delete return false
        bool CreateOrDelete(int postsId, int tagId);
    }
}
