using System.Collections.Generic;
using QAP4.Models;

namespace QAP4.Application.Services
{
    public interface ICommentService
    {
        List<Comments> FindCommentsByPostId(int postId);
        int AddComment(Comments comment);
    }
}