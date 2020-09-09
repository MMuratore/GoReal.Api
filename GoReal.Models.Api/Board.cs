using G = GoReal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GoReal.Models.Api
{
    public class Board
    {
        public bool?[][] StoneMap { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        private int?[] Capture { get; set; }
        private bool[] Player { get; set; }
        private Stone KoInfo { get; set; }

        public Board(bool?[][] stoneMap)
        {
            this.StoneMap = stoneMap;
            this.Height = StoneMap.GetLength(0);
            this.Width = StoneMap.GetLength(1);
            this.Capture = new int?[] { null , null};
            this.Player = new bool[] { true , false };

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
            bool[][] liberties = null;
    
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Stone stone = new Stone(x, y);
                    if (Get(stone) is null || liberties[x][y]) continue;
                    if (!HasLiberties(stone)) return false;

                    GetChain(stone).ForEach(v => liberties[v.Row][v.Column] = true);
                }
            }

            return true;
        }

        public (List<Stone>, List<bool>) Diff(Board board)
        {
            if (board.Width != Width || board.Height != Height)
                return (null, null);

            List<Stone> result = new List<Stone>();
            List<bool> action = new List<bool>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Stone newStone = new Stone(x, y);
                    newStone.Color = board.Get(newStone);
                    if (Get(newStone) != newStone.Color)
                    {
                        result.Add(newStone);
                        if (newStone.Color is null) action.Add(false);
                        else action.Add(true);
                    }

                }
            }

            return (result, action);
        }

        public bool? Get(Stone stone)
        {
            return StoneMap[stone.Column]?[stone.Row];
        }

        private Board Set(Stone stone)
        {
            if (Has(stone)) 
                StoneMap[stone.Column][stone.Row] = stone.Color;

            return this;
        }

        private bool Has(Stone stone)
        {
            return 0 <= stone.Row && stone.Row < Width && 0 <= stone.Column && stone.Column < Height;
        }

        private Board Clone()
        {
            Board result = new Board(StoneMap)
                .SetCaptures(true, GetCaptures(true))
                .SetCaptures(false, GetCaptures(false));

            result.KoInfo = KoInfo;

            return result;
        }

        private int? GetCaptures(bool player)
        {
            int index = Array.IndexOf(Player, player);
            if (index < 0) return null;

            return Capture[index];
        }

        private Board SetCaptures(bool? player, int? capture = null, Func<int?,int?> mutator = null)
        {
            int index = Array.IndexOf(Player, player);

            if (index >= 0)
                Capture[index] = !(mutator is null) ? mutator(Capture[index]) : capture;

            return this;
        }

        private bool VertexEquals(Stone stone1, Stone stone2)
        {
            return stone1.Row == stone2.Row && stone1.Column == stone2.Column;
        }

        private List<Stone> GetNeighbors(Stone stone)
        {
            if (!Has(stone)) return null;
            int x = stone.Row;
            int y = stone.Column;

            return new List<Stone> { new Stone(x - 1, y), new Stone(x + 1, y), new Stone(x, y - 1), new Stone(x, y + 1) }.Where(v => Has(v)).ToList();
        }

        private bool HasLiberties(Stone stone, bool[][] visited = null)
        {
            if (!Has(stone)) return false;

            if (visited[stone.Column][stone.Row]) return false;

            List<Stone> neighbors = GetNeighbors(stone);

            if (neighbors.Any(n => Get(n) is null))
                return true;

            visited[stone.Column][stone.Row] = true;

            return neighbors.Where(n => Get(n) == stone.Color).Any(n => HasLiberties(n, visited));
        }

        private List<Stone> GetChain(Stone stone)
        {
            return GetConnectedComponent(stone, v => Get(v) == stone.Color);
        }

        private List<Stone> GetConnectedComponent(Stone stone, Func<Stone,bool> predicate, List<Stone> result = null)
        {
            if (!Has(stone)) return null;
            if (result is null) result.Add(stone);

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

        private List<Stone> GetLiberties(Stone stone)
        {
            if (!Has(stone) || Get(stone) is null) return null;

            List<Stone> chain = GetChain(stone);
            List<Stone> liberties = new List<Stone>();
            bool[][] added = null;

            foreach (Stone libertie in chain)
            {
                List<Stone> freeNeighbors = GetNeighbors(libertie).Where(n => Get(n) is null).ToList();

                liberties.AddRange(freeNeighbors.Where(n => !(added[n.Column][n.Row])));
                freeNeighbors.ForEach(n => added[n.Column][n.Row] = true);
            }

            return liberties;
        }
    }
}
