using D = GoReal.Models.Entities;
using System;

namespace GoReal.Models.Api
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
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
        public D.TimeControl TimeControl { get; set; }
        public D.Rule Rule { get; set; }
        public User BlackPlayer { get; set; }
        public User WhitePlayer { get; set; }
        public Board Board { get; set; }
    }
}
