using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Dal.Entities
{
    public class Rule
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public bool Overwrite { get; set; }
        public bool Suicide { get; set; }
        public bool Ko { get; set; }
    }
}
