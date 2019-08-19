using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class Tags
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Count { get; set; }
        public string Description { get; set; }
        public int? ExcerptPostId { get; set; }
        public int? WikiPostId { get; set; }
        public int? UserCreatedId { get; set; }
    }
}
