using GoReal.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Api.Models.DataTransfertObject
{
    public class MoveResult
    {
        public List<Stone> Stones { get; set; }
        public int BlackCapture { get; set; }
        public int WhiteCapture { get; set; }
        public Stone KoInfo { get; set; }
        public bool? BlackState { get; set; }
        public bool? WhiteState { get; set; }
        public string Result { get; set; }

        public MoveResult()
        {
            this.Stones = new List<Stone>();
        }
    };
}
