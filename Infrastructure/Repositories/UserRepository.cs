
using System.Collections.Generic;
using System.Linq;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System;

namespace QAP4.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private QAPContext context;
        private DbSet<Users> userEntity;
        public UserRepository(QAPContext context)
        {
            this.context = context;
            userEntity = context.Set<Users>();
        }

        //add
        public bool Add(Users item)
        {
            //check email
            var email = userEntity.SingleOrDefault(o => o.Id.Equals(item.Email));
            var phone = userEntity.SingleOrDefault(o => o.Id.Equals(item.Phone));

            if (email == null && phone == null)
            {
                //TODO: can use getHash(password+salt) ?
                var passwordHash = GetHash(item.Password);
                item.Password = passwordHash;
                item.CreationDate = DateTime.Now;
                item.LastAccessDate = DateTime.Now;
                if (string.IsNullOrEmpty(item.DisplayName))
                {
                    item.DisplayName = item.FirstName + " " + item.LastName;
                }

                context.Entry(item).State = EntityState.Added;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //remove
        public void Delete(int id)
        {
            var item = userEntity.SingleOrDefault(o => o.Id.Equals(id));
            if (item != null)
            {
                userEntity.Remove(item);
                context.SaveChanges();
            }
        }

        //update
        public void Update(Users user)
        {
            userEntity.Update(user);
            context.SaveChanges();
        }

        //get and find
        public IEnumerable<Users> GetAll()
        {
            return userEntity.AsEnumerable();
        }

        public Users GetByName(string key)
        {
            return userEntity.Where(o => o.DisplayName.Equals(key)).FirstOrDefault();
        }

        public Users GetByEmailOrPhone(string key)
        {
            return userEntity.Where(o => o.Email.Equals(key) || o.Phone.Equals(key)).FirstOrDefault();
        }

        public Users GetById(int? id)
        {
            return userEntity.Where(o => o.Id.Equals(id)).FirstOrDefault();
        }

        //check login: check password, email or phone
        public Users CheckLogin(string emailOrPhone, string password)
        {
            var passwordHash = GetHash(password);
            return userEntity.Where(o => o.Password.Equals(passwordHash) && (o.Email.Equals(emailOrPhone) || o.Phone.Equals(emailOrPhone))).FirstOrDefault();
        }

        //statistic
        public IEnumerable<Users> GetUsersFeature()
        {
            return userEntity.OrderByDescending(o => o.UpVotes).Take(5).AsEnumerable();
        }


        //search
        public IEnumerable<Users> SearchInUsers(string key)
        {
            var sql = @"SELECT *  FROM Users WHERE FREETEXT (DisplayName, '" + key + "') or FREETEXT(Email,'" + key + "') or FREETEXT(Phone,'" + key + "')";
            return userEntity.FromSql<Users>(sql).AsEnumerable();
        }

        //get user follwing
        public IEnumerable<Users> GetUsersFollowing(int userFollowedId)
        {
            var sql = @"SELECT u.* FROM Users u INNER JOIN Following f ON u.Id=f.FollowingUserId WHERE f.FollowedUserId=" + userFollowedId;
            return userEntity.FromSql<Users>(sql).OrderBy(o => o.DisplayName).AsEnumerable();
        }

        //common method

        //get hash
        private static string GetHash(string text)
        {
            // SHA512 is disposable by inheritance.
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Get the hashed string.
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        //get salt
        private static string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);

                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }


    }
}
