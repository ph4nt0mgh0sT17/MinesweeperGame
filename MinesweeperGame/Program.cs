using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.Extensions.Logging;
using MinesweeperGame.Core;
using StudentSystem.Core;

namespace MinesweeperGame
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            //new MinesweeperConsoleGame().Run();
            Dictionary<string, int> dict = new Dictionary<string, int>(new[]
            {
                new KeyValuePair<string, int>("x", 1),
                new KeyValuePair<string, int>("y", 2) 
            });

            dict.OrderByDescending(x => x.Key).ToList().ForEach(x => Console.WriteLine(x));
        }
    }
}
