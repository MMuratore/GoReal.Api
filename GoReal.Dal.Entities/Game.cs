using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Dal.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BlackRank { get; set; }
        public int WhiteRank { get; set; }
        public string Result { get; set; }
        public int Size { get; set; }
        public int Komi { get; set; }
        public int Handicap { get; set; }
        public int BlackCapture { get; set; }
        public int WhiteCapture { get; set; }
        public bool? BlackState { get; set; }
        public bool? WhiteState { get; set; }
        public string KoInfo { get; set; }
        public int TimeControlId { get; set; }
        public int RuleId { get; set; }
        public int BlackPlayerId { get; set; }
        public int WhitePlayerId { get; set; }
    }
}
