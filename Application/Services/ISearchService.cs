using System.Collections.Generic;
using QAP4.Models;
using QAP4.ViewModels;

namespace QAP4.Application.Services
{
    public interface ISearchService
    {
        List<Posts> FindPosts(string query, int postsType, int page, int size);
        SearchView FindAll(string obj, string query, int postType, int page, int size);
    }
}