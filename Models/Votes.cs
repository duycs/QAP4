using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class Votes
    {
        public int Id { get; set; }
        public int? PostsId { get; set; }
        public byte? VoteTypeId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? BountyAmount { get; set; }
        public bool? IsOn { get; set; }
    }
}
