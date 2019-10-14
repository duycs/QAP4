using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.AggreatesModels.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class HomeView
    {
        public User User { get; set; }
        public IEnumerable<Posts> PostsFeed { get; set; }
        public IEnumerable<Tag> TagsFeature { get; set; }
        public IEnumerable<User> UsersFeature { get; set; }
        public Quote Quote { get; set; }
    }
}
