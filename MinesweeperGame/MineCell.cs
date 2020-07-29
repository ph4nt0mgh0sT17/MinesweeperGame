using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperGame
{
    /// <summary>
    /// The class that represents the mine element in the field.
    /// </summary>
    internal class MineCell
    {
        /// <summary>
        /// The state of the mine if hidden or already exposed;
        /// </summary>
        public MineCellState MineState { get; set; }

        public bool IsMine { get; private set; }

        public int MineCount { get; set; }

        /// <summary>
        /// The constructor that creates the minecell with the state and if it's the mine or not...
        /// </summary>
        /// <param name="mineState">The state of the mine cell.</param>
        /// <param name="isMine">The boolean state if it's the mine </param>
        public MineCell(MineCellState mineState = MineCellState.Exposed, bool isMine = false)
        {
            MineState = mineState;
            IsMine = isMine;
            MineCount = 0;
        }
    }
}
