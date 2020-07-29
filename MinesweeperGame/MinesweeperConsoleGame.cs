using System;
using System.Collections.Generic;
using System.Text;
using MinesweeperGame.Core;

namespace MinesweeperGame
{
    /// <summary>
    /// The internal entry point from which the minesweeper game is loaded into the Console application.
    /// </summary>
    internal class MinesweeperConsoleGame
    {
        private MineCell[,] exposedArray;
        private MineCell[,] hiddenArray;
        private Random random;

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
            for (int i = 0; i < mineNumber; i++)
            {
                exposedArray[random.Next(hiddenArray.GetLength(0)), random.Next(hiddenArray.GetLength(1))] = new MineCell(isMine: true);
            }

            for (int i = 0; i < exposedArray.GetLength(0); i++)
            {
                for (int j = 0; j < exposedArray.GetLength(1); j++)
                {
                    exposedArray[i, j] = exposedArray[i, j] ?? new MineCell(isMine: false);
                }
            }

            for (int i = 0; i < exposedArray.GetLength(0); i++)
            {
                for (int j = 0; j < exposedArray.GetLength(1); j++)
                {
                    if (!exposedArray[i,j].IsMine)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++)
                            {
                                if (i + x >= 0 && j + y >= 0 && i + x <= exposedArray.GetLength(0) - 1 && j + y <= exposedArray.GetLength(0) - 1)
                                {
                                    if ( exposedArray[i + x, j + y].IsMine)
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
            for (int m = 1; m <= exposedArray.GetLength(0); m++) { Console.Write($"  {m}  "); }
            Console.WriteLine(); Console.Write("----+"); for (int m = 1; m <= exposedArray.GetLength(0); m++) { Console.Write("-----"); }
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

        private void CalculateMineCount()
        {
            
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
