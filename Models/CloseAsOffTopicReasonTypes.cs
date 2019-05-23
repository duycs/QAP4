using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class CloseAsOffTopicReasonTypes
    {
        public byte Id { get; set; }
        public bool? IsUniversal { get; set; }
        public string MarkdownMini { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? CreationModeratorId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int? ApprovalModeratorId { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public int? DeactivationModeratorId { get; set; }
    }
}
