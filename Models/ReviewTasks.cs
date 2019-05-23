using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class ReviewTasks
    {
        public int Id { get; set; }
        public byte? ReviewTaskTypeId { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public byte? ReviewTaskStateId { get; set; }
        public int? PostId { get; set; }
        public int? SuggestedEditId { get; set; }
        public int? CompletedByReviewTaskId { get; set; }
    }
}
