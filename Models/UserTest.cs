using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class UserTest
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? TestId { get; set; }
        public int? Total { get; set; }
        public int? Pass { get; set; }
        public int? ScoreUnit { get; set; }
        public string UserDisplayName { get; set; }
        public byte? CertifiedId { get; set; }
    }
}
