using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class Tests
    {
        public int Id { get; set; }
        public int? CreatorId { get; set; }
        public string CloseDate { get; set; }
        public string CommentCount { get; set; }
        public string Comments { get; set; }
        public string CreationDate { get; set; }
        public string Description { get; set; }
        public string DeletionDate { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public byte? TestTypeId { get; set; }
        public int? VoteCount { get; set; }
        public int? ViewCount { get; set; }
        public int? ExcuteCount { get; set; }
        public DateTime? LastEditDate { get; set; }
        public DateTime? LastActiveDate { get; set; }
        public string UserDisplayName { get; set; }
        public int? LastEditorUserId { get; set; }
        public byte? Level { get; set; }
        public int? Score { get; set; }
    }
}
