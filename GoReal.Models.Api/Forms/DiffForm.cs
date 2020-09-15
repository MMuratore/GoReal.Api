using GoReal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Models.Api.Forms
{
    public class DiffForm
    {
        public List<Stone> Stones { get; set; }
        public List<bool> Action { get; set; }
        public int? BlackCapture { get; set; }
        public int? WhiteCapture { get; set; }
        public Stone KoInfo { get; set; }
    };
}
