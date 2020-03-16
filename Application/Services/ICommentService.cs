using System;
using System.Collections.Generic;
using System.Linq;
using QAP4.Extensions;
using QAP4.Models;

namespace QAP4.Application.Services
{
    public interface ICommentService
    {

        public Comments AddComment(Comments Comments);
        public void UpdateComment(Comments Comments);
        public Comments GetCommentById(int id);

        public IPagedList<Comments> GetComments(int pageIndex = 0, int pageSize = int.MaxValue, int postsId = 0);
    }
}