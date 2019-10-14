using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.AggreatesModels.Users.Models;
using QAP4.Domain.Core.Repositories;
using QAP4.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Domain.Services
{
    public class TagRelationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Posts> _postsRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<PostsTag> _postsTagRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<PostsLink> _postsLinkRepository;

        public TagRelationService(
        IUnitOfWork unitOfWork,
         IRepository<Posts> postsRepository,
         IRepository<Tag> tagRepository,
         IRepository<PostsTag> postsTagRepository,
         IRepository<User> userRepository,
         IRepository<PostsLink> postsLinkRepository)
        {
            _unitOfWork = unitOfWork;
            _postsRepository = postsRepository;
            _tagRepository = tagRepository;
            _postsTagRepository = postsTagRepository;
            _userRepository = userRepository;
            _postsLinkRepository = postsLinkRepository;
        }



    }
}
