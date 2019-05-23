using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class ReviewTaskResults
    {
        public int Id { get; set; }
        public int? ReviewTaskId { get; set; }
        public byte? ReviewTaskResultTypeId { get; set; }
        public DateTime? CreationDate { get; set; }
        public byte? RejectionReationId { get; set; }
        public string Comment { get; set; }
    }
}
