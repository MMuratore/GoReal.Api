using GoReal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Models.Api
{
    public class MoveResult
    {
        public List<Stone> Stones { get; set; }
        public int? BlackCapture { get; set; }
        public int? WhiteCapture { get; set; }
        public Stone KoInfo { get; set; }

        public MoveResult()
        {
            this.Stones = new List<Stone>();
        }
    };
}
