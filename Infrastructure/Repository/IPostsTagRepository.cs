using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Repository
{
    public interface IPostsTagRepository
    {
        void Create(int postsId, int tagId);
        void Delete(int? postsId, int? tagId);

        //handler: create return true, delete return false
        bool CreateOrDelete(int postsId, int tagId);
    }
}
