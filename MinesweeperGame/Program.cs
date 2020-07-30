using System;
using Mikrite.Core;
using Mikrite;
using Mikrite.Core.DependencyInjection;
using Mikrite.Core.Extensions;

namespace MinesweeperGame
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            MikriteProvider.Construct().AddFileLogger("Logs/MinesweeperLog.txt").Build();
            new MinesweeperConsoleGame().Run();
        }
    }
}
