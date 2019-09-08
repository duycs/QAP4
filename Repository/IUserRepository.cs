using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Repository
{
    public interface IUserRepository
    {
        bool Add(Users item);
        IEnumerable<Users> GetAll();
        Users GetById(int? id);
        Users GetByEmailOrPhone(string key);
        Users GetByName(string key);
        Users FindUserByAccountName(string accountName);
        Users CheckLogin(string emailOrPhone, string password);
        void Delete(string id);
        void Update(Users item);

        void UpdateRange(IEnumerable<Users> users);
        IEnumerable<Users> GetUsersFeature();
        IEnumerable<Users> SearchInUsers(string key);
        IEnumerable<Users> GetUsersFollowing(int userFollowedId);
    }
}
