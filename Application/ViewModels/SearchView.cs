using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class SearchView
    {
        public Users User{get;set;}
        public string ObjType { get; set; }
        public string Key { get; set; }
        public int Count { get; set; }
        public IEnumerable<Posts> Posts { get; set; }
        public IEnumerable<Posts> SimplePosts { get; set; }
        public IEnumerable<Posts> Questions { get; set; }
        public IEnumerable<Posts> Tutorials { get; set; }
        public IEnumerable<Tags> Tags { get; set; }
        public IEnumerable<Users> Users { get; set; }
        public IEnumerable<Tags> TagsRelation { get; set; }

    }
}
