using System.Collections.Generic;
using System.Linq;
using QAP4.Extensions;
using QAP4.Infrastructure.Repositories;
using QAP4.Models;
using QAP4.ViewModels;

namespace QAP4.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IPostsRepository _postsRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IPostsTagRepository _postsTagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuoteRepository _quoteRepository;

        public SearchService(
            IPostsRepository postsRepository,
            ITagRepository tagRepository,
            IPostsTagRepository postsTagRepository,
            IUserRepository userRepository,
            IQuoteRepository quoteRepository
            )
        {
            _postsRepository = postsRepository;
            _tagRepository = tagRepository;
            _postsTagRepository = postsTagRepository;
            _userRepository = userRepository;
            _quoteRepository = quoteRepository;
        }

        public List<Posts> FindPosts(string query, int postsType, int page, int size)
        {
            var posts = _postsRepository.SearchInPosts(page, query, postsType).ToList();
            return posts;
        }

        public SearchView FindAll(string obj, string query, int postType, int page, int size)
        {
            var SearchView = new SearchView();

            // Search all or search by postType
            if (AppConstants.ObjectType.ALL.Equals(obj))
            {
                SearchView = GetSearchView(page, query);
            }
            else
            {
                SearchView = GetSearchView(page, query, postType);
            }

            return SearchView;
        }


        private SearchView GetSearchView(int page, string query)
        {
            var SearchView = new SearchView();
            SearchView.Key = query;

            //tags relation
            var tagsRelation = GetTagsRelation(query);
            if (tagsRelation.Any())
                SearchView.TagsRelation = tagsRelation;

            //get all posts
            var posts = _postsRepository.SearchInPosts(page, query, 0);


            if (posts.Any())
            {
                var simplePosts = posts.Where(w => w.PostTypeId == AppConstants.PostsType.POSTS);
                var questions = posts.Where(w => w.PostTypeId == AppConstants.PostsType.QUESTION);
                var tutorials = posts.Where(w => w.PostTypeId == AppConstants.PostsType.TUTORIAL);

                SearchView.SimplePosts = simplePosts;
                SearchView.Questions = questions;
                SearchView.Tutorials = tutorials;

                SearchView.Count += posts.Count();
            }

            //tags
            var tags = _tagRepository.SearchInTags(query);
            if (tags.Any())
            {
                SearchView.Tags = tags;
                SearchView.Count += tags.Count();
            }

            //users
            var users = _userRepository.SearchInUsers(query);
            if (users.Any())
            {
                SearchView.Users = users;
                SearchView.Count += users.Count();
            }

            return SearchView;
        }


        private SearchView GetSearchView(int page, string query, int postsTypeId)
        {
            var SearchView = new SearchView();
            SearchView.Key = query;

            //tags relation
            var tagsRelation = GetTagsRelation(query);
            if (tagsRelation.Any())
                SearchView.TagsRelation = tagsRelation;

            //get all posts
            var posts = _postsRepository.SearchInPosts(page, query, postsTypeId);


            if (posts.Any())
            {
                SearchView.Posts = posts;
                SearchView.Count += posts.Count();
            }

            //tags
            var tags = _tagRepository.SearchInTags(query);
            if (tags.Any())
            {
                SearchView.Tags = tags;
                SearchView.Count += tags.Count();
            }

            //users
            var users = _userRepository.SearchInUsers(query);
            if (users.Any())
            {
                SearchView.Users = users;
                SearchView.Count += users.Count();
            }

            return SearchView;
        }

        private IEnumerable<Tags> GetTagsRelation(string key)
        {
            return _tagRepository.SearchInTags(key);
        }

    }
}