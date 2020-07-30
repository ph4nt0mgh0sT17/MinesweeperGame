using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Mikrite.Core.DependencyInjection;
using Mikrite.Core.Extensions;
using MinesweeperGame.Core;
using MinesweeperGame.Core.Extensions;
using MinesweeperGame.DependencyInjection;

namespace MinesweeperGame
{
    /// <summary>
    /// The internal entry point from which the minesweeper game is loaded into the Console application.
    /// </summary>
    internal class MinesweeperConsoleGame
    {
        private readonly ILogger mLogger;
        
        private int fieldNumber;
        private int mineNumber;
        private MineCell[,] exposedArray;
        private MineCell[,] hiddenArray;
        private readonly Random random;

        public MinesweeperConsoleGame()
        {
            random = new Random();
            mLogger = DependencyInjectionProvider.Logger;
        }

        public void Run()
        {
            mLogger.LogInformationSource(Constants.MinesweeperMessages.IntroductionMessage);
            Console.WriteLine(Constants.MinesweeperMessages.IntroductionMessage);

            mineNumber = AskUserForNumber("Type number of mines: ");
            fieldNumber = AskUserForNumber("Type number of fields: ");

            InitializeMinesweeperBoards();
            FillAllMinesweeperBoards();

            PrintMinesweeperBoard(exposedArray);
            PrintMinesweeperBoard(hiddenArray);

            AskForCoordinates();
        }

        private int AskUserForNumber(string text)
        {
            Console.Write(text);
            return int.Parse(Console.ReadLine() ?? "0");
        }

        private void InitializeMinesweeperBoards()
        {
            exposedArray = new MineCell[fieldNumber, fieldNumber];
            hiddenArray = new MineCell[fieldNumber, fieldNumber];
        }

        private void FillAllMinesweeperBoards()
        {
            FillHiddenArray();
            FillExposedArray(mineNumber);
        }

        private void FillHiddenArray()
        {
            hiddenArray.ForEachMineCell((x, y) =>
            {
                hiddenArray[x,y] = new MineCell(MineCellState.Hidden, false);
            });
        }

        private void FillExposedArray(int mineNumber)
        {
            FillExposedArrayWithMines(mineNumber);
            FillNullMineCellsWithEmptyCells();
            CalculateMineCount();
        }

        private void FillNullMineCellsWithEmptyCells()
        {
            exposedArray.ForEachMineCell((x, y) =>
            {
                exposedArray[x, y] = exposedArray[x, y] ?? new MineCell();
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
            exposedArray.ForEachMineCell((i,j) =>
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
            });
        }

        private void PrintMinesweeperBoard(MineCell[,] minesweeperBoard)
        {
            Console.Write("    |");
            for (int m = 1; m <= minesweeperBoard.GetLength(0); m++)
            {
                Console.Write($"  {m}  ");
            }
            Console.WriteLine(); 
            Console.Write("----+");
            for (int m = 1; m <= minesweeperBoard.GetLength(0); m++)
            {
                Console.Write("-----");
            }

            Console.WriteLine();

            for (int i = 0; i < minesweeperBoard.GetLength(0); i++)
            {
                Console.Write($"{i + 1,-4}|");
                for (int j = 0; j < minesweeperBoard.GetLength(1); j++)
                {
                    if (minesweeperBoard[i, j].MineState == MineCellState.Hidden)
                    {
                        PrintColorText(ConsoleColor.Black, '#');
                    }

                    else if (minesweeperBoard[i, j].IsMine)
                    {
                        PrintColorText(ConsoleColor.DarkRed, 'x');
                    }

                    else if (minesweeperBoard[i, j].MineCount == 0)
                    {
                        PrintColorText(ConsoleColor.Green, (char)(exposedArray[i, j].MineCount + 48));
                    }

                    else if (minesweeperBoard[i, j].MineCount > 0)
                    {
                        PrintColorText(ConsoleColor.Blue, (char)(exposedArray[i, j].MineCount + 48));
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private void PrintColorText(ConsoleColor color, char character)
        {
            ConsoleColor defaultColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
            Console.Write($"  {character}  ");
            Console.BackgroundColor = defaultColor;
        }

        /// <summary>
        /// Asks for coordinates and chooses the mine cell...
        /// </summary>
        private void AskForCoordinates()
        {
            Console.Write("Type the number of the row: ");
            int x = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Type the number of the column: ");
            int y = int.Parse(Console.ReadLine() ?? "0");

            bool validCoordinates = false;

            while (!validCoordinates)
            {
                if (ValidateCoordinates(x, y))
                {
                    if (hiddenArray[x, y].MineState == MineCellState.Exposed)
                    {
                        Console.WriteLine(Constants.MinesweeperMessages.MineCellAlreadyExposed);
                    }

                    else
                    {
                        validCoordinates = true;
                    }
                }
            }

            int originalX = x - 1;
            int originalY = y - 1;

            // Has zero neighbour mines
            if (exposedArray[originalX, originalY].MineCount == 0)
            {
                CheckZeroes(originalX, originalY);
            }

            hiddenArray[originalX, originalY] = exposedArray[originalX, originalY];

            PrintMinesweeperBoard(exposedArray);
            PrintMinesweeperBoard(hiddenArray);


        }

        private bool ValidateCoordinates(int x, int y)
        {
            return x >= 0 && y >= 0 && x < fieldNumber && y < fieldNumber;
        }

        private void CheckZeroes(int x, int y)
        {
            // Toto číslo můžeme klidně odhalit protože z předešlé podmínky víme že na těchto iniciálních koordinátech se nachází právě nula
            hiddenArray[x, y] = exposedArray[x, y];

            // Příkazy které kontrolují všechny směry kde může být nula
            CheckUp(x, y);
            CheckLeft(x, y);
            CheckRight(x, y);
            CheckDown(x, y);

        }

        private void CheckUp(int x, int y)
        {
            // Pokud je index x 0, můžeme rovnou ukončit metodu protože není nikde výš kde bychom mohli jít, jinak by nám program vyhodil OutOfIndex exception
            if (x == 0)
            {
                return;
            }

            // Pokud číslo o 1 index  výše není nula, nebo to číslo bylo již odhaleno, nemá smysl dál pokračovat a ukončíme metodu
            if (exposedArray[x - 1, y].MineCount != 0 || hiddenArray[x - 1, y].MineState != MineCellState.Hidden)
            {
                return;
            }

            // Víme že na koordinátech x - 1, y se nachází nula tak ji rovnou odhalíme v poli 'b'...
            x = x - 1;
            hiddenArray[x, y] = exposedArray[x, y];

            CheckLeft(x, y);
            CheckRight(x, y);
            CheckUp(x, y);
        }

        private void CheckDown(int x, int y)
        {
            // Pokud je index 'x' na konci nemá smysl kde dál jít tak rovnou ukončíme metodu
            if (x == exposedArray.GetLength(0) - 1)
            {
                return;
            }

            // Pokud číslo o 1 index níže není nula nebo je již odhaleno tak nemá také smysl dál pokračovat
            if (exposedArray[x + 1, y].MineCount != 0 || hiddenArray[x + 1, y].MineState != MineCellState.Hidden)
            {
                return;
            }

            // Víme že číslo o 1 index níže je 0 tak to číslo rovnou odhalíme v poli 'b'
            x = x + 1;
            hiddenArray[x, y] = exposedArray[x, y];

            CheckLeft(x, y);
            CheckRight(x, y);
            CheckDown(x, y);
        }

        private void CheckLeft(int x, int y)
        {
            // To samé co platí u metody CheckUp
            if (y == 0)
            {
                return;
            }

            // To samé co platí u metody CheckUp
            if (exposedArray[x, y - 1].MineCount != 0 || hiddenArray[x, y - 1].MineState != MineCellState.Hidden)
            {
                return;
            }

            // Víme že číslo o 1 index vlevo je 0 tak to číslo rovnou odhalíme v poli 'b'
            y = y - 1;
            hiddenArray[x, y] = exposedArray[x, y];


            CheckUp(x, y);
            CheckDown(x, y);
            CheckLeft(x, y);
        }

        private void CheckRight(int x, int y)
        {
            // Pokud jsme již na konci pole nemá smysl dál pokračovat není co odhalovat => ukončíme metodu
            if (y == exposedArray.GetLength(0) - 1)
            {
                return;
            }

            // Pokud číslo o 1 index vpravo není 0 nebo je již odhaleno nemá smysl dál pokračovat => ukončíme metodu
            if (exposedArray[x, y + 1].MineCount != 0 || hiddenArray[x, y + 1].MineState != MineCellState.Hidden)
            {
                return;
            }

            // Víme že číslo o 1 index vpravo je 0 tak to číslo rovnou odhalíme v poli 'b'
            y = y + 1;
            hiddenArray[x, y] = exposedArray[x, y];


            CheckUp(x, y);
            CheckDown(x, y);
            CheckRight(x, y);
        }

        /// <summary>
        /// Checks the win 
        /// </summary>
        /// <returns></returns>
        private bool CheckWin()
        {

            bool state = true;
            Enumerable.Range(0, hiddenArray.GetLength(0)).Repeat(x =>
            {
                Enumerable.Range(0, hiddenArray.GetLength(1)).Repeat(y =>
                {
                    if (hiddenArray[x, y].MineState == MineCellState.Hidden)
                    {
                        state = false;
                    }
                });
            });

            return state;
        }
    }
}
