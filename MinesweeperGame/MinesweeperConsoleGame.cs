using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinesweeperGame.Core;
using MinesweeperGame.Core.Extensions;

namespace MinesweeperGame
{
    /// <summary>
    /// The internal entry point from which the minesweeper game is loaded into the Console application.
    /// </summary>
    internal class MinesweeperConsoleGame
    {
        private MineCell[,] exposedArray;
        private MineCell[,] hiddenArray;
        private readonly Random random;

        public MinesweeperConsoleGame()
        {
            random = new Random();
        }

        public void Run()
        {
            Console.WriteLine(Constants.MinesweeperMessages.IntroductionMessage);

            Console.Write("Type number of mines: ");
            int mineNumber = int.Parse(Console.ReadLine());

            Console.Write("Type number of fields: ");
            int fieldNumber = int.Parse(Console.ReadLine());

            exposedArray = new MineCell[fieldNumber,fieldNumber];
            hiddenArray = new MineCell[fieldNumber,fieldNumber];

            FillHiddenArray();
            FillExposedArray(mineNumber);

            PrintExposedMineField();
        }

        private void FillExposedArray(int mineNumber)
        {
            FillExposedArrayWithMines(mineNumber);
            FillNullMineCellsWithEmptyCells();
            CalculateMineCount();
        }

        private void FillNullMineCellsWithEmptyCells()
        {
            Enumerable.Range(0, exposedArray.GetLength(0)).Repeat(x =>
            {
                Enumerable.Range(0, exposedArray.GetLength(1)).Repeat(y =>
                {
                    exposedArray[x, y] = exposedArray[x, y] ?? new MineCell(isMine: false);
                });
            });
        }

        private void FillExposedArrayWithMines(int mineNumber)
        {
            Enumerable.Range(0, mineNumber).Repeat(x =>
            {
                exposedArray[random.Next(exposedArray.GetLength(0)), random.Next(exposedArray.GetLength(0))] = new MineCell(MineCellState.Exposed, true);
            });
        }
        private void CalculateMineCount()
        {
            for (int i = 0; i < exposedArray.GetLength(0); i++)
            {
                for (int j = 0; j < exposedArray.GetLength(1); j++)
                {
                    if (!exposedArray[i, j].IsMine)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++)
                            {
                                if (i + x >= 0 && j + y >= 0 && i + x <= exposedArray.GetLength(0) - 1 && j + y <= exposedArray.GetLength(0) - 1)
                                {
                                    if (exposedArray[i + x, j + y].IsMine)
                                    {
                                        exposedArray[i, j].MineCount++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PrintExposedMineField()
        {
            Console.Write("    |");
            for (int m = 1; m <= exposedArray.GetLength(0); m++)
            {
                Console.Write($"  {m}  ");
            }
            Console.WriteLine(); 
            Console.Write("----+");
            for (int m = 1; m <= exposedArray.GetLength(0); m++)
            {
                Console.Write("-----");
            }

            Console.WriteLine();

            for (int i = 0; i < exposedArray.GetLength(0); i++)
            {
                Console.Write($"{i + 1,-4}|");
                for (int j = 0; j < exposedArray.GetLength(1); j++)
                {
                    if (exposedArray[i, j].MineState == MineCellState.Hidden)
                    {
                        PrintColorText(ConsoleColor.Black, '#');
                    }

                    else if (exposedArray[i, j].IsMine)
                    {
                        PrintColorText(ConsoleColor.DarkRed, 'x');
                    }

                    else if (exposedArray[i, j].MineCount == 0)
                    {
                        PrintColorText(ConsoleColor.Green, (char)(exposedArray[i, j].MineCount + 48));
                    }

                    else if (exposedArray[i, j].MineCount > 0)
                    {
                        PrintColorText(ConsoleColor.Blue, (char)(exposedArray[i, j].MineCount + 48));
                    }
                }

                Console.WriteLine();
            }
        }

        private void PrintColorText(ConsoleColor color, char character)
        {
            ConsoleColor defaultColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
            Console.Write($"  {character}  ");
            Console.BackgroundColor = defaultColor;
        }

        private void FillHiddenArray()
        {
            for (int i = 0; i < hiddenArray.GetLength(0); i++)
            {
                for (int j = 0; j < hiddenArray.GetLength(1); j++)
                {
                    hiddenArray[i,j] = new MineCell(MineCellState.Hidden, false);
                }
            }
        }
    }
}
