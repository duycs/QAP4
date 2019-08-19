using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class SuggestedEdits
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? RejectionDate { get; set; }
        public int? OwnerUserId { get; set; }
        public string Comment { get; set; }
        public string EditContent { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string RevisionGuid { get; set; }
    }
}
