using GoReal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Models.Api.Helpers
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
