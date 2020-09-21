using GoReal.Dal.Entities;
using System.Collections.Generic;

namespace GoReal.Api.Models.Helpers
{
    public static class JaggedArray
    {
        public static bool?[][] InitalizeStoneMap(int width, int height, List<Stone> stones = null)
        {
            bool?[][] map = new bool?[width][];
            for (int i = 0; i < width; i++)
            {
                map[i] = new bool?[height];
            }
            if (!(stones is null))
            {
                foreach (Stone stone in stones)
                {
                    map[stone.Column][stone.Row] = stone.Color;
                }
            }
            
            return map;
        }
    }
}
