using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class TutorialDetailView
    {
        public Users User { get; set; }
        public Posts Posts { get; set; }
        public Posts Tutorial { get; set; }
        public List<KeyValuePair<int, string>> RelatedPosts { get; set; }
        public IEnumerable<Posts> SameTutorials { get; set; }
    }
}
