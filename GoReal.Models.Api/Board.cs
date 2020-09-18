using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using GoReal.Models.Api.Helpers;
using GoReal.Models.Api.Exceptions;
using GoReal.Models.Api.DataTransfertObject;

namespace GoReal.Models.Api
{
    public class Board
    {
        public bool?[][] StoneMap { get; set; }
        public Stone KoInfo { get; set; }
        
        private int Height { get; set; }
        private int Width { get; set; }
        private Dictionary<bool, int> Capture { get; set; }
        private Dictionary<User, bool> Player { get; set; }

        public Board(List<Stone> stones, int height, int width, User blackPlayer, User whitePlayer)
        {
            StoneMap = JaggedArray.InitalizeStoneMap(height, width, stones);
            KoInfo = new Stone();
            Height = height;
            Width = width;
            Capture = new Dictionary<bool, int> { { true, 0 }, { false, 0 } };
            Player = new Dictionary<User, bool> { { whitePlayer, true }, { blackPlayer, false } };
        }

        public Board(List<Stone> stones, int height, int width, User blackPlayer, User whitePlayer, int blackCapture, int witheCapture, Stone koInfo) 
            : this(stones, height, width, blackPlayer, whitePlayer)
        {
            Capture = new Dictionary<bool, int> { { true, witheCapture }, { false, blackCapture } };
            KoInfo = koInfo;
        }

        public Board MakeMove(Stone stone, bool preventSuicide = false, bool preventOverwrite = false, bool preventKo = false)
        {

            Board move = Clone();

            if (!Has(stone))
                throw new GameException(GameResult.PointNotExist, "Point do not exist");

            if (preventOverwrite && !(Get(stone) is null))
                throw new GameException(GameResult.PreventOverwrite, "Prevent Overwrite");

            if (preventKo && KoInfo.Color == stone.Color && VertexEquals(KoInfo, stone))
                throw new GameException(GameResult.PreventKo, "Prevent Ko");

            move = Set(move, stone);

            // Remove captured stones

            List<Stone> neighbors = move.GetNeighbors(stone);
            List<Stone> deadStones = new List<Stone>();
            List<Stone> deadNeighbors = neighbors.Where(n => move.Get(n) == !stone.Color && !move.HasLiberties(n)).ToList();

            foreach (Stone deadNeighbor in deadNeighbors)
            {
                deadNeighbor.Color = move.Get(deadNeighbor);

                if (deadNeighbor.Color is null) continue;
                List<Stone> deadChain = move.GetChain(deadNeighbor);
                foreach (Stone removeStone in deadChain)
                {
                    removeStone.Color = null;
                    move = Set(move, removeStone).SetCaptures(Player.First(x => x.Value == stone.Color).Key, x => x + 1);
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
                    throw new GameException(GameResult.PreventSuicide, "Prevent Suicide");

                foreach (Stone suicide in move.GetChain(stone))
                {
                    suicide.Color = null;
                    move = Set(move, suicide).SetCaptures(Player.First(x => x.Value == !stone.Color).Key, x => x + 1);
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
            bool?[][] liberties = JaggedArray.InitalizeStoneMap(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Stone stone = new Stone() { Row = x, Column = y, Color = null };
                    stone.Color = Get(stone);
                    if (stone.Color is null || liberties[x][y] is true) continue;
                    if (!HasLiberties(stone)) return false;

                    GetChain(stone).ForEach(v => liberties[v.Row][v.Column] = true);
                }
            }

            return true;
        }

        public MoveResult Diff(Board board)
        {
            if (board.Width != Width || board.Height != Height)
                return null;

            MoveResult result = new MoveResult();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Stone stone = new Stone() { Row = x, Column = y, Color = null };
                    stone.Color = board.Get(stone);
                    if (Get(stone) != stone.Color) result.Stones.Add(stone);
                }
            }

            result.BlackCapture = board.GetCaptures(Player.First(x => x.Value == false).Key);
            result.WhiteCapture = board.GetCaptures(Player.First(x => x.Value == true).Key);
            result.KoInfo = board.KoInfo;

            return result;
        }

        private bool? Get(Stone stone)
        {
            return StoneMap[stone.Column]?[stone.Row];
        }

        private Board Set(Board board, Stone stone)
        {
            if (board.Has(stone))
            {
                board.StoneMap[stone.Column][stone.Row] = stone.Color;
            }

            return board;
        }

        private bool Has(Stone stone)
        {
            return (0 <= stone.Row) && (stone.Row < Width) && (0 <= stone.Column) && (stone.Column < Height);
        }

        private Board Clone()
        {
            Board result = new Board(GetStones(), Height, Width, Player.First(x => x.Value == false).Key, Player.First(x => x.Value == true).Key)
                .SetCaptures(Player.First(x => x.Value == true).Key, GetCaptures(Player.First(x => x.Value == true).Key))
                .SetCaptures(Player.First(x => x.Value == false).Key, GetCaptures(Player.First(x => x.Value == false).Key));

            result.KoInfo = KoInfo;

            return result;
        }

        private List<Stone> GetStones()
        {
            List<Stone> result = new List<Stone>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Stone stone = new Stone() { Row = x, Column = y, Color = null };
                    stone.Color = Get(stone);

                    result.Add(stone);
                }
            }

            return result;
        }

        public int GetCaptures(User player)
        {
            return Capture[Player[player]];
        }

        public Board SetCaptures(User player, int capture)
        {
            Capture[Player[player]] = capture;

            return this;
        }

        private Board SetCaptures(User player, Func<int, int> mutator = null)
        {
            Capture[Player[player]] = !(mutator is null) ? mutator(Capture[Player[player]]) : Capture[Player[player]];

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

            List<Stone> neighbors = new List<Stone> { 
                new Stone() { Row = x - 1, Column = y, Color = null }, 
                new Stone() { Row = x + 1, Column = y, Color = null }, 
                new Stone() { Row = x, Column = y - 1, Color = null }, 
                new Stone() { Row = x, Column = y + 1, Color = null } }.Where(v => Has(v)).ToList();

            foreach (Stone neigbhbor in neighbors)
            {
                neigbhbor.Color = Get(neigbhbor);
            }
            return neighbors;
        }

        private bool HasLiberties(Stone stone, bool?[][] visitedParam = null)
        {
            if (!Has(stone)) return false;

            bool?[][] visited = visitedParam ?? JaggedArray.InitalizeStoneMap(Width, Height);

            if (!(visited[stone.Column][stone.Row] is null)) return false;

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

        private List<Stone> GetConnectedComponent(Stone stone, Func<Stone, bool> predicate, List<Stone> result = null)
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

        private List<Stone> GetLiberties(Stone stone)
        {
            if (!Has(stone) || Get(stone) is null) return null;

            List<Stone> chain = GetChain(stone);
            List<Stone> liberties = new List<Stone>();
            bool?[][] added = JaggedArray.InitalizeStoneMap(Width, Height);

            foreach (Stone libertie in chain)
            {
                List<Stone> freeNeighbors = GetNeighbors(libertie).Where(n => Get(n) is null).ToList();

                freeNeighbors.ForEach(n => added[n.Column][n.Row] = true);
                liberties.AddRange(freeNeighbors.Where(n => !(added[n.Column][n.Row] is null)));
            }

            return liberties;
        }
    }
    
}
