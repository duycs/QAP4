using System.Collections.Generic;
using System.Linq;
using QAP4.Domain.AggreatesModels.Users.Models;
using QAP4.Domain.Core.Repositories;
using QAP4.Extensions;
using QAP4.Infrastructure.Repositories;
using QAP4.Models;
using QAP4.ViewModels;

namespace QAP4.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(
             IRepository<User> userRepository
            )
        {
            _userRepository = userRepository;
        }

        public User FindUserById(int userId)
        {
            if (userId == 0)
                return null;

            return _userRepository.FindById(userId);
        }

    }
}