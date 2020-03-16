using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class PostsView
    {
        public Users User { get; set; }
        public IEnumerable<Tags> TagsFeature { get; set; }
        public IEnumerable<Users> UsersFeature { get; set; }
        public IEnumerable<Posts> PostsSimple { get; set; }
        public IEnumerable<Posts> QuestionsAnswer { get; set; }
        public IEnumerable<Posts> TutorialsAnswer { get; set; }

    }
}
