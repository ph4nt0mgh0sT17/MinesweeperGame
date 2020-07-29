using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperGame.Core.Extensions
{
    public static class IterationExtensions
    {
        public static void Repeat(this IEnumerable<int> enumerable, Action<int> process)
        {
            if (process == null)
            {
                throw new ArgumentException("The process needs to be specified.");
            }

            foreach (int item in enumerable)
            {
                process.Invoke(item);
            }
        }
    }
}
