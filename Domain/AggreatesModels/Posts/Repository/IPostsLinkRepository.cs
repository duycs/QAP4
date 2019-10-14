using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.Core.Repositories;

namespace QAP4.Infrastructure.Repositories
{
    public interface IPostsLinkRepository : IRepository<PostsLink>
    {
        bool IsPostLinkExist(int? postId, int? RelatedPostId);
        void Create(PostsLink model);
        void Update(PostsLink model);
        void Delete(int? postId, int? RelatedPostId);

        //handler: create return true, delete return false
        bool CreateOrDelete(int? postId, int? RelatedPostId, int? linkTypeId);
    }
}
