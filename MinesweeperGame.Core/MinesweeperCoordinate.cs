using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperGame.Core
{
    public class MinesweeperCoordinate
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public MinesweeperCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
