using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class Questions
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string Answer { get; set; }
        public byte? CorrectAnswer { get; set; }
        public int? VoteCount { get; set; }
        public int? ViewCount { get; set; }
        public string Tags { get; set; }
        public byte? Level { get; set; }
        public byte? TestTypeId { get; set; }
    }
}
