using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Dal.Entities
{
    public class Statistic
    {
        public int UserId { get; set; }
        public int GameNumber { get; set; }
        public decimal VictoryRatio { get; set; }
        public int PlayTime { get; set; }
    }
}
