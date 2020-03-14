using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;

namespace QAP4.Repository
{
    public interface IPostLinkRepository
    {
        // bool IsPostLinkExist(int? postId, int? RelatedPostId);
        void Create(PostLinks model);
        void Update(PostLinks model);
        void Delete(int? postId, int? RelatedPostId);

        //handler: create return true, delete return false
        bool CreateOrDelete(int? postId, int? RelatedPostId, int? linkTypeId);
    }
}
