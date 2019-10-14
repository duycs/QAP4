using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        bool Add(User item);
        IEnumerable<User> GetAll();
        User GetById(int? id);
        User GetByEmailOrPhone(string key);
        User GetByName(string key);
        User CheckLogin(string emailOrPhone, string password);
        void Delete(int id);
        void Update(User item);
        IEnumerable<User> GetUsersFeature();
        IEnumerable<User> SearchInUsers(string key);
        IEnumerable<User> GetUsersFollowing(int userFollowedId);
    }
}
