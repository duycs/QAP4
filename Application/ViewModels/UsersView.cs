using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using QAP4.Models;

namespace QAP4.ViewModels
{
    public class UsersView
    {
        public Users User { get; set; }
        public bool IsCurrentUser { get; set; }
        public IEnumerable<Tags> TagsFeature { get; set; }
        public IEnumerable<Users> UsersFollowing { get; set; }
        public IEnumerable<Posts> PostsNewest { get; set; }
        public IEnumerable<Posts> QuestionsNewest { get; set; }
        public IEnumerable<Tests> TestsNewest { get; set; }
        public IEnumerable<Tests> TutorialsNewest { get; set; }
    }
}
