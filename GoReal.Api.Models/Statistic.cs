using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Api.Models
{
    public class Statistic
    {
        public int UserId { get; set; }
        public int GameNumber { get; set; }
        public decimal VictoryRatio { get; set; }
        public int PlayTime { get; set; }
    }
}
