using System.Collections.Generic;
using QAP4.Models;

namespace QAP4.Application.Services
{
    public interface IPostsService
    {
        List<Posts> FindPostsByCreateDate(int page);
    }
}