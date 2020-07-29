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
            Console.WriteLine(Constants.MinesweeperMessages.IntroductionMessage);
            Mikrite.Construct().AddFileLogger("Logs/MinesweeperGame.txt").Build();
            Mikrite.Service<ILogger>().LogInformationSource("The minesweeper log has started.");
        }
    }
}
