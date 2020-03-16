using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Application.Services
{
    public interface IUserService
    {
        Users GetUserById(int? id);
        Users GetUserByEmailOrPhone(string key);
        Users GetUserByName(string key);
        Users GetUserByAccountName(string accountName);
        Users GetUserIsLogin(string emailOrPhone, string password);
        Users AddUser(Users item);

        void DeleteUser(string id);
        void UpdateUser(Users item);

        IEnumerable<Users> GetUsersFeature(int? take);
        IEnumerable<Users> SearchInUsers(string key);
        IEnumerable<Users> GetUsersFollowing(int userFollowedId);
    }
}
