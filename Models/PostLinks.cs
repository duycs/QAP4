using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class PostLinks
    {
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? PostId { get; set; }
        public int? RelatedPostId { get; set; }
        public byte? LinkTypeId { get; set; }
    }
}
