using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;
using QAP4.Extensions;
using QAP4.Repository;

namespace QAP4.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<Users> _userRepository;
        private readonly IRepository<Tags> _tagRepository;
        private readonly IRepository<Posts> _postsRepository;
        private readonly IRepository<PostsTag> _postsTagRepository;

        private readonly IRepository<Following> _followingRepository;

        public UserService(IRepository<Users> userRepository,
            IRepository<Tags> tagRepository,
            IRepository<Posts> postsRepository,
            IRepository<PostsTag> postsTagRepository,
            IRepository<Following> followingRepository)
        {
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _postsRepository = postsRepository;
            _postsTagRepository = postsTagRepository;
            _followingRepository = followingRepository;
        }

        public Users AddUser(Users userViewModel)
        {
            if (userViewModel == null)
                throw new ArgumentNullException(nameof(userViewModel));

            var user = userViewModel;

            return _userRepository.Insert(user);
        }

        public void UpdateUser(Users userViewModel)
        {
            if (userViewModel == null)
                throw new ArgumentNullException(nameof(userViewModel));

            var user = userViewModel;

            _userRepository.Update(user);
        }

        public void DeleteUser(string id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _userRepository.Delete(user);
        }

        public Users GetUserByAccountName(string accountName)
        {
            var user = _userRepository.Table.Where(w => w.AccountName == accountName).FirstOrDefault();
            return user;
        }

        public Users GetUserByEmailOrPhone(string key)
        {
            return _userRepository.Table.Where(o => o.Email.Equals(key) || o.Phone.Equals(key)).FirstOrDefault();
        }

        public Users GetUserById(int? id)
        {
            return _userRepository.Table.Where(w => w.Id == id).FirstOrDefault();
        }

        public Users GetUserByName(string key)
        {
            return _userRepository.Table.Where(o => o.DisplayName.Equals(key)).FirstOrDefault();
        }

        public Users GetUserIsLogin(string emailOrPhone, string password)
        {
            var passwordHash = StringExtensions.GetHash(password);
            return _userRepository.Table.Where(o => o.Password.Equals(passwordHash) && (o.Email.Equals(emailOrPhone)
            || o.Phone.Equals(emailOrPhone))).FirstOrDefault();
        }

        public IEnumerable<Users> GetUsersFeature(int? take = 5)
        {
            var users = _userRepository.Table.OrderByDescending(o => o.UpVotes).Take((int)take).ToList();
            return users;
        }

        public IEnumerable<Users> GetUsersFollowing(int userFollowedId)
        {
            var query = from a in _userRepository.Table
                        join b in _followingRepository.Table
                        on a.Id equals b.FollowingUserId
                        where b.FollowedUserId == userFollowedId
                        orderby a.DisplayName
                        select a;

            return query.AsEnumerable().OrderBy(a => a.DisplayName).ToList();
        }

        public IEnumerable<Users> SearchInUsers(string key)
        {
            //var sql = @"SELECT *  FROM Users WHERE FREETEXT (DisplayName, '" + key + "') or FREETEXT(Email,'" + key + "') or FREETEXT(Phone,'" + key + "')";
            //return userEntity.FromSql<Users>(sql).AsEnumerable();
            var users = _userRepository.Table.Where(w => w.DisplayName.Contains(key) || w.Email.Contains(key) || w.Phone.Contains(key)).ToList();
            return users;
        }
    }
}
