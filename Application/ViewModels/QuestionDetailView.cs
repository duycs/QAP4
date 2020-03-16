using QAP4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class QuestionDetailView
    {
        public Users User { get; set; }
        public Posts Posts { get; set; }
        public IEnumerable<Posts> Answers { get; set; }
        public IEnumerable<Posts> SameQuestions { get; set; }
    }
}
