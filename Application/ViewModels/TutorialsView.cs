using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class TutorialsView
    {
        public User User { get; set; }
        public IEnumerable<Tags> TagsFeature { get; set; }
        public IEnumerable<User> UsersFeature { get; set; }
        public IEnumerable<Posts> TutorialsNewest { get; set; }
        public IEnumerable<Posts> TutorialsFeature { get; set; }
        public IEnumerable<Posts> TutorialsRequired { get; set; }
        public IEnumerable<Badges> BadgesFeature { get; set; }
        public int Total { get; set; }
    }
}
