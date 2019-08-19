using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class PostsDetailView
    {
        public Users User { get; set; }
        public Posts Posts { get; set; }
        public IEnumerable<Posts> PostsSameTags { get; set; }
        public IEnumerable<Posts> PostsSameAuthor { get; set; }
    }
}
