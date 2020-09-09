using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Models.Api
{
    public class Stone
    {
        public Stone(int row, int column, bool? color = null)
        {
            Row = row;
            Column = column;
            Color = color;
        }

        public int Row { get; set; }
        public int Column { get; set; }
        public bool? Color { get; set; }

    }
}
