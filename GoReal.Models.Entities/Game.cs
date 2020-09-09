using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Models.Entities
{
    public class Game
    {
        public int GameId { get; set; }
        public DateTime Date { get; set; }
        public int BlackRank { get; set; }
        public int WhiteRank { get; set; }
        public string Result { get; set; }
        public int Size { get; set; }
        public int Komi { get; set; }
        public int Handicap { get; set; }
    }
}
