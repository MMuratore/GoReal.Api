using GoReal.Models.Api.Forms;
using GoReal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoReal.Models.Api
{
    public class Board
    {
        public bool?[][] StoneMap { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int?[] Capture { get; set; }
        public Dictionary<bool, Stone> StoneDiff { get; set; }

        private bool[] Player { get; set; }
        private Stone KoInfo { get; set; }

        public Board(bool?[][] stoneMap)
        {
            this.StoneMap = stoneMap;
            this.Height = StoneMap.GetLength(0);
            this.Width = StoneMap[1].GetLength(0);
            this.Capture = new int?[] { null, null };
            this.Player = new bool[] { true, false };
            this.KoInfo = new Stone();
            this.StoneDiff = new Dictionary<bool, Stone>();

            foreach (bool?[] row in StoneMap)
            {
                if (row.Length != Width)
                    throw new ArgumentException(nameof(stoneMap));
            }
        }

        public Board MakeMove(Stone stone, bool preventSuicide = false, bool preventOverwrite = false, bool preventKo = false)
        {

            Board move = Clone();

            if (!Has(stone))
                return move;

            if (preventOverwrite && !(Get(stone) is null))
                throw new ArgumentException("Overwrite prevented");

            if (preventKo && KoInfo.Color == stone.Color && VertexEquals(KoInfo, stone))
                throw new ArgumentException("Ko prevented");

            move.Set(stone);

            // Remove captured stones

            List<Stone> neighbors = move.GetNeighbors(stone);
            List<Stone> deadStones = new List<Stone>();
            List<Stone> deadNeighbors = neighbors.Where(n => move.Get(n) == !stone.Color && !move.HasLiberties(n)).ToList();

            foreach (Stone deadNeighbor in deadNeighbors)
            {
                if (move.Get(deadNeighbor) is null) continue;

                foreach (Stone removeStone in move.GetChain(deadNeighbor))
                {
                    removeStone.Color = null;
                    move.Set(removeStone).SetCaptures(stone.Color, null, x => x + 1);
                    deadStones.Add(removeStone);
                }
            }

            // Detect future ko

            List<Stone> liberties = move.GetLiberties(stone);
            bool hasKo = deadStones.Count() == 1
                && liberties.Count() == 1
                && VertexEquals(liberties[0], deadStones[0])
                && neighbors.All(n => move.Get(n) != stone.Color);

            move.KoInfo.Color = hasKo ? !stone.Color : null;
            move.KoInfo.Column = hasKo ? deadStones[0].Column : -1;
            move.KoInfo.Row = hasKo ? deadStones[0].Row : -1;

            // Detect suicide

            if (deadStones.Count() == 0 && liberties.Count() == 0)
            {
                if (preventSuicide)
                    throw new ArgumentException("Suicide preventedd");

                foreach (Stone suicide in move.GetChain(stone))
                {
                    suicide.Color = null;
                    move.Set(suicide).SetCaptures(!stone.Color, null, x => x + 1);
                }
            }

            return move;
        }
        /*
        public List<Stone> GetHandicapPlacement(int count, bool tygem = false)
        {
            if (Math.Min(Width, Height) <= 6 || count < 2) return null;

            Stone near = new Stone(Width > 13 ? 3 : 2, Height > 13 ? 3 : 2);
            Stone far = new Stone(Width - near.Row - 1, Height - near.Column - 1);
            Stone middle = new Stone((Width - 1) / 2, (Height - 1) / 2);

            List<Stone> result = !tygem ? new List<Stone>() { new Stone(near.Row, far.Column), new Stone(far.Row, near.Column), new Stone(far.Row, far.Column), new Stone(near.Row, near.Column) }
            : new List<Stone>() { new Stone(near.Row, far.Column), new Stone(far.Row, near.Column), new Stone(near.Row, near.Column), new Stone(far.Row, far.Column) };

            if (Width % 2 != 0 && Height % 2 != 0 && Width != 7 && Height != 7)
            {
                if (count == 5) result.Add(middle);
                result.Add(new Stone(near.Row, middle.Column));
                result.Add(new Stone(far.Row, middle.Column));
                if (count == 7) result.Add(middle);
                result.Add(new Stone(middle.Row, near.Column));
                result.Add(new Stone(middle.Row, far.Column));
                result.Add(middle);
            }
            else if (Width % 2 != 0 && Width != 7)
            {
                result.Add(new Stone(middle.Row, near.Column));
                result.Add(new Stone(middle.Row, far.Column));
            }
            else if (Height % 2 != 0 && Height != 7)
            {
                result.Add(new Stone(near.Row, middle.Column));
                result.Add(new Stone(far.Row, middle.Column));
            }

            return result.GetRange(0, count);
        }
        */
        public Board Clear()
        {
            Array.Clear(StoneMap, 0, StoneMap.Length);
            return this;
        }

        public bool IsSquare()
        {
            return Width == Height;
        }

        public bool IsEmpty()
        {
            return StoneMap.All(row => row.All(x => x is null));
        }

        public bool IsValid()
        {
            bool?[][] liberties = InitalizeStoneMap(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Stone stone = new Stone() { Row = x, Column = y, Color = null };
                    if (Get(stone) is null || liberties[x][y] is true) continue;
                    if (!HasLiberties(stone)) return false;

                    GetChain(stone).ForEach(v => liberties[v.Row][v.Column] = true);
                }
            }

            return true;
        }

        public DiffForm Diff()
        {

            DiffForm diffForm = new DiffForm();
            diffForm.Action = new List<bool>(StoneDiff.Keys);
            diffForm.Stones = new List<Stone>(StoneDiff.Values);
            StoneDiff.Clear();
            diffForm.BlackCapture = GetCaptures(true);
            diffForm.WhiteCapture = GetCaptures(false);
            diffForm.KoInfo = KoInfo;

            return diffForm;
        }

        public bool? Get(Stone stone)
        {
            return StoneMap[stone.Column]?[stone.Row];
        }

        public Board Set(Stone stone)
        {
            if (Has(stone))
            {
                StoneMap[stone.Column][stone.Row] = stone.Color;
                if (stone.Color is null) StoneDiff.Add(false, stone);
                else StoneDiff.Add(true, stone);
            }

            return this;
        }

        public bool Has(Stone stone)
        {
            return (0 <= stone.Row) && (stone.Row < Width) && (0 <= stone.Column) && (stone.Column < Height);
        }

        public Board Clone()
        {
            Board result = new Board(StoneMap)
                .SetCaptures(true, GetCaptures(true))
                .SetCaptures(false, GetCaptures(false));

            result.KoInfo = KoInfo;

            return result;
        }

        public int? GetCaptures(bool player)
        {
            int index = Array.IndexOf(Player, player);
            if (index < 0) return null;

            return Capture[index];
        }

        public Board SetCaptures(bool? player, int? capture = null, Func<int?, int?> mutator = null)
        {
            int index = Array.IndexOf(Player, player);

            if (index >= 0)
                Capture[index] = !(mutator is null) ? mutator(Capture[index]) : capture;

            return this;
        }

        public bool VertexEquals(Stone stone1, Stone stone2)
        {
            return stone1.Row == stone2.Row && stone1.Column == stone2.Column;
        }

        public List<Stone> GetNeighbors(Stone stone)
        {
            if (!Has(stone)) return null;
            int x = stone.Row;
            int y = stone.Column;

            return new List<Stone> { 
                new Stone() { Row = x - 1, Column = y, Color = null }, 
                new Stone() { Row = x + 1, Column = y, Color = null }, 
                new Stone() { Row = x, Column = y - 1, Color = null }, 
                new Stone() { Row = x, Column = y + 1, Color = null } }.Where(v => Has(v)).ToList();
        }

        public bool HasLiberties(Stone stone, bool?[][] visitedParam = null)
        {
            if (!Has(stone)) return false;

            bool?[][] visited = visitedParam ?? InitalizeStoneMap(Width, Height);

            if (!(visited[stone.Column][stone.Row] is null)) return false;

            List<Stone> neighbors = GetNeighbors(stone);

            if (neighbors.Any(n => Get(n) is null))
                return true;

            visited[stone.Column][stone.Row] = true;

            return neighbors.Where(n => Get(n) == stone.Color).Any(n => HasLiberties(n, visited));
        }

        public List<Stone> GetChain(Stone stone)
        {
            return GetConnectedComponent(stone, v => Get(v) == stone.Color);
        }

        public List<Stone> GetConnectedComponent(Stone stone, Func<Stone, bool> predicate, List<Stone> result = null)
        {
            if (!Has(stone)) return null;
            if (result is null)
            {
                result = new List<Stone>();
                result.Add(stone);
            }
            // Recursive depth-first search

            foreach (Stone neighbor in GetNeighbors(stone))
            {
                if (!predicate(neighbor)) continue;
                if (result.Any(w => VertexEquals(w, neighbor))) continue;

                result.Add(neighbor);
                GetConnectedComponent(neighbor, predicate, result);
            }

            return result;
        }

        public List<Stone> GetLiberties(Stone stone)
        {
            if (!Has(stone) || Get(stone) is null) return null;

            List<Stone> chain = GetChain(stone);
            List<Stone> liberties = new List<Stone>();
            bool?[][] added = InitalizeStoneMap(Width, Height);

            foreach (Stone libertie in chain)
            {
                List<Stone> freeNeighbors = GetNeighbors(libertie).Where(n => Get(n) is null).ToList();

                freeNeighbors.ForEach(n => added[n.Column][n.Row] = true);
                liberties.AddRange(freeNeighbors.Where(n => !(added[n.Column][n.Row] is null)));
            }

            return liberties;
        }

        private bool?[][] InitalizeStoneMap(int width, int height)
        {
            bool?[][] map = new bool?[width][];
            for (int i = 0; i < width; i++)
            {
                map[i] = new bool?[height];
            }
            return map;
        }
    }
}
