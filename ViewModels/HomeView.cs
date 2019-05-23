using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class HomeView
    {
        public Users User { get; set; }
        public IEnumerable<Posts> PostsFeed { get; set; }
        public IEnumerable<Tags> TagsFeature { get; set; }
        public IEnumerable<Users> UsersFeature { get; set; }
        public Quotes Quote { get; set; }
    }
}
