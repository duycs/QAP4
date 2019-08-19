using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class Images
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public byte[] Name { get; set; }
    }
}
