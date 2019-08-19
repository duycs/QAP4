using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class Quotes
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? UserId { get; set; }
        public string AuthorDisplayName { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
