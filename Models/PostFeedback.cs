using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class PostFeedback
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public bool? IsAnonymous { get; set; }
        public byte? VoteTypeId { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
