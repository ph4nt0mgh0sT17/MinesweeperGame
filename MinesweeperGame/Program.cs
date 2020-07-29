using System;
using Microsoft.Extensions.Logging;
using MinesweeperGame.Core;
using StudentSystem.Core;

namespace MinesweeperGame
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            MinesweeperConsoleGame game = new MinesweeperConsoleGame();
            game.Run();
        }
    }
}
