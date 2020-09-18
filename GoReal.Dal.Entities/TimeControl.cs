using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Dal.Entities
{
    public class TimeControl
    {
        public int Id { get; set; }
        public string Speed { get; set; }
        public string OverTime { get; set; }
        public int? TimeLimit { get; set; }
        public int? TimePerPeriod { get; set; }
        public int? Period { get; set; }
        public int? InitialTime { get; set; }
    }
}
