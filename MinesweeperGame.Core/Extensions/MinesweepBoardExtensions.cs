using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinesweeperGame.Core.Extensions
{
    public static class MinesweepBoardExtensions
    {
        public static void ForEachMineCell(this MineCell[,] minesweeperBoard, Action<int, int> operationForMineCell)
        {
            if (operationForMineCell == null)
            {
                throw new ArgumentNullException(nameof(operationForMineCell), "The parameter cannot be null.");
            }

            Enumerable.Range(0, minesweeperBoard.GetLength(0)).Repeat(x =>
            {
                Enumerable.Range(0, minesweeperBoard.GetLength(1)).Repeat(y =>
                {
                    operationForMineCell.Invoke(x, y);
                });
            });
        } 
    }
}
