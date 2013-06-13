using System.Collections.Generic;
using System.Linq;
using MT.Battleships.Components;
using MT.Battleships.Enums;

namespace MT.Battleships.Helpers
{
    public static class GridHelper
    {
        #region Private Methods

        /// <summary>
        /// Determines whether given the starting row, cell and ship size that there is a possibility to place the ship horizontally
        /// before it's starting point on the grid
        /// </summary>
        /// <param name="startingRow">Row that the ship starts on</param>
        /// <param name="startingCell">Cell that the ship starts on</param>
        /// <param name="shipSize">Size of the ship</param>
        /// <returns>Whether or not there is a possible end on the same row (before the starting cell)</returns>
        private static bool CanMoveBackwardHorizontally(Row startingRow, Cell startingCell, int shipSize)
        {
            bool output = false;
            //Work out where exactly the ship would end based on the current cell and the ship's size ( -1 is for zero based index)
            int requiredIndex = startingRow.Cells.FindIndex(c => c.CellId == startingCell.CellId) - (shipSize - 1);

            //Can we go horizontally backward?
            if (requiredIndex >= 0 && startingRow.Cells[requiredIndex].State == CellState.Open)
            {
                //Make sure no cells along the way are blocked
                Cell endingCell = startingRow.Cells[requiredIndex];
                List<Cell> crossingCells = startingRow.Cells
                    .Where(c => c.CellId > startingCell.CellId && c.CellId < endingCell.CellId)
                    .ToList();

                //If all cells that the ship crosses are open, it's allowed
                if (crossingCells.All(c => c.State == CellState.Open))
                {
                    output = true;
                }
            }

            return output;
        }

        /// <summary>
        /// Determines whether given the starting row, cell and ship size that there is a possibility to place the ship horizontally 
        /// after it's starting point on the grid
        /// </summary>
        /// <param name="startingRow">Row that the ship starts on</param>
        /// <param name="startingCell">Cell that the ship starts on</param>
        /// <param name="shipSize">Size of the ship</param>
        /// <returns>Whether or not there is a possible end on the same row (after the starting cell)</returns>
        private static bool CanMoveForwardHorizontally(Row startingRow, Cell startingCell, int shipSize)
        {
            bool output = false;
            //Work out where exactly the ship would end based on the current cell and the ship's size ( -1 is for zero based index)
            int requiredIndex = startingRow.Cells.FindIndex(c => c.CellId == startingCell.CellId) + (shipSize - 1);
            //Can we go horizontally forward?
            if (requiredIndex < 10 && startingRow.Cells[requiredIndex].State == CellState.Open)
            {
                //Make sure no cells along the way are blocked
                Cell endingCell = startingRow.Cells[requiredIndex];
                List<Cell> crossingCells = startingRow.Cells
                    .Where(c => c.CellId > startingCell.CellId && c.CellId < endingCell.CellId)
                    .ToList();

                //If all cells that the ship crosses are open, it's allowed
                if (crossingCells.All(c => c.State == CellState.Open))
                {
                    output = true;
                }
            }

            return output;
        }

        /// <summary>
        /// Determines whether given the starting row, cell and ship size that there is a possibility to place the ship vertically 
        /// before it's starting point on the grid
        /// </summary>
        /// <param name="startingRow">Row that the ship starts on</param>
        /// <param name="startingCell">Cell that the ship starts on</param>
        /// <param name="shipSize">Size of the ship</param>
        /// <param name="player">The current player</param>
        /// <returns>Whether or not there is a possible end in the same cell number on a different row (before the starting row)</returns>
        private static bool CanMoveBackwardVertically(Player player, Row startingRow, Cell startingCell, int shipSize)
        {
            bool output = false;
            //Work out where exactly the ship would end based on the current row and the ship's size ( -1 is for zero based index)
            int requiredIndex = player.Grid.Rows.FindIndex(r => r.RowId == startingRow.RowId) - (shipSize - 1);

            //Can we go backwards vertically?
            if (requiredIndex >= 0 && player.Grid.Rows[requiredIndex].Cells[startingCell.CellId - 1].State == CellState.Open)
            {
                //Make sure no cells along the way are blocked
                Row endingRow = player.Grid.Rows[requiredIndex];
                List<Cell> crossingCells = player.Grid.Rows
                    .Where(r => player.Grid.Rows.FindIndex(i => i.RowId == r.RowId) > player.Grid.Rows.FindIndex(i => i.RowId == startingRow.RowId))
                    .Where(r => player.Grid.Rows.FindIndex(i => i.RowId == r.RowId) < player.Grid.Rows.FindIndex(i => i.RowId == endingRow.RowId))
                    .Select(r => r.Cells[startingCell.CellId - 1])
                    .ToList();

                //If all cells that the ship crosses are open, it's allowed
                if (crossingCells.All(c => c.State == CellState.Open))
                {
                    output = true;
                }
            }

            return output;
        }

        /// <summary>
        /// Determines whether given the starting row, cell and ship size that there is a possibility to place the ship vertically 
        /// after it's starting point on the grid
        /// </summary>
        /// <param name="startingRow">Row that the ship starts on</param>
        /// <param name="startingCell">Cell that the ship starts on</param>
        /// <param name="shipSize">Size of the ship</param>
        /// <param name="player">The current player</param>
        /// <returns>Whether or not there is a possible end in the same cell number on a different row (after the starting row)</returns>
        private static bool CanMoveForwardVertically(Player player, Row startingRow, Cell startingCell, int shipSize)
        {
            bool output = false;
            //Work out where exactly the ship would end based on the current row and the ship's size ( -1 is for zero based index)
            int requiredIndex = player.Grid.Rows.FindIndex(r => r.RowId == startingRow.RowId) + (shipSize - 1);

            //Can we go forwards vertically?
            if (requiredIndex < 10 && player.Grid.Rows[requiredIndex].Cells[startingCell.CellId - 1].State == CellState.Open)
            {
                //Make sure no cells along the way are blocked
                Row endingRow = player.Grid.Rows[requiredIndex];
                List<Cell> crossingCells = player.Grid.Rows
                    .Where(r => player.Grid.Rows.FindIndex(i => i.RowId == r.RowId) > player.Grid.Rows.FindIndex(c => c.RowId == startingRow.RowId))
                    .Where(r => player.Grid.Rows.FindIndex(i => i.RowId == r.RowId) < player.Grid.Rows.FindIndex(c => c.RowId == endingRow.RowId))
                    .Select(r => r.Cells[startingCell.CellId - 1])
                    .ToList();

                //If all cells that the ship crosses are open, it's allowed
                if (crossingCells.All(c => c.State == CellState.Open))
                {
                    output = true;
                }
            }

            return output;
        }

        /// <summary>
        /// Determines the directions (if any) of the possible moves for a given starting row, cell, ship size and player's grid
        /// </summary>
        /// <param name="player">The current player</param>
        /// <param name="startingRow">The row to start on</param>
        /// <param name="startingCell">The cell to start on</param>
        /// <param name="possibleCells">The list of possible moves on all cells in the grid (which is updated)</param>
        /// <param name="shipSize">The size of the ship</param>
        /// <param name="isEnd">Whether or not the move is for the end of the ship (if that's the case, we return the end cell/row)</param>
        /// <returns>An updated list of possible moves given that cell and row</returns>
        private static List<Cell> PossibleMovesForRowAndCell(Player player, Row startingRow, Cell startingCell, List<Cell> possibleCells, int shipSize, bool isEnd)
        {
            //Can we move forward horizontally?
            if (CanMoveForwardHorizontally(startingRow, startingCell, shipSize))
            {
                Cell cell = new Cell(startingCell.CellId, startingRow.RowId);

                //If we're looking for the end point, we want the end cell
                if (isEnd)
                {
                    Cell endingCell = startingRow.Cells[startingRow.Cells.FindIndex(c => c.CellId == startingCell.CellId) + (shipSize - 1)];
                    cell.CellId = endingCell.CellId;
                }

                if (isEnd || !possibleCells.Any(c => c.CellId == cell.CellId && c.RowId == cell.RowId))
                {
                    possibleCells.Add(cell);
                }
            }

            //Can we moved backward horizontally?
            if (CanMoveBackwardHorizontally(startingRow, startingCell, shipSize))
            {
                Cell cell = new Cell(startingCell.CellId, startingRow.RowId);

                //If we're looking for the end point, we want the end cell
                if (isEnd)
                {
                    Cell endingCell = startingRow.Cells[startingRow.Cells.FindIndex(c => c.CellId == startingCell.CellId) - (shipSize - 1)];
                    cell.CellId = endingCell.CellId;
                }

                if (isEnd || !possibleCells.Any(c => c.CellId == cell.CellId && c.RowId == cell.RowId))
                {
                    possibleCells.Add(cell);
                }
            }

            //Can we move forward vertically?
            if (CanMoveForwardVertically(player, startingRow, startingCell, shipSize))
            {
                Cell cell = new Cell(startingCell.CellId, startingRow.RowId);

                //If we're looking for the end point, we want the end row
                if (isEnd)
                {
                    Row endingRow = player.Grid.Rows[(player.Grid.Rows.FindIndex(c => c.RowId == startingRow.RowId) + (shipSize - 1))];
                    cell.RowId = endingRow.RowId;
                }

                if (isEnd || !possibleCells.Any(c => c.CellId == cell.CellId && c.RowId == cell.RowId))
                {
                    possibleCells.Add(cell);
                }
            }

            //Can we move backward vertically?
            if (CanMoveBackwardVertically(player, startingRow, startingCell, shipSize))
            {
                Cell cell = new Cell(startingCell.CellId, startingRow.RowId);

                //If we're looking for the end point, we want the end row
                if (isEnd)
                {
                    Row endingRow = player.Grid.Rows[(player.Grid.Rows.FindIndex(r => r.RowId == startingRow.RowId) - (shipSize - 1))];
                    cell.RowId = endingRow.RowId;
                }

                if (isEnd || !possibleCells.Any(c => c.CellId == cell.CellId && c.RowId == cell.RowId))
                {
                    possibleCells.Add(cell);
                }
            }

            return possibleCells;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates all possible cells the ship can start on based on it's size
        /// </summary>
        /// <param name="player">The player who is placing the item</param>
        /// <param name="shipSize">The size of the ship to be placed</param>
        /// <returns>A list of cells that can accommodate the start of the ship (and have a valid end cell)</returns>
        public static List<Cell> CalculatePossibleStartingCells(Player player, int shipSize)
        {
            List<Cell> possibleCells = new List<Cell>();

            foreach (Row row in player.Grid.Rows)
            {
                //We can only add to the Cells that are not Occupied
                List<Cell> freeCells = row.Cells.Where(c => c.State == CellState.Open).ToList();

                //For each free cell, determine whether it's a possible movement
                possibleCells = freeCells.Aggregate(possibleCells, (current, cell) => PossibleMovesForRowAndCell(player, row, cell, current, shipSize, false));
            }

            //Order by letter then by actual number (Otherwise it will go A1, A10, A2 etc.)
            possibleCells = possibleCells
                .OrderBy(c => c.RowId)
                .ThenBy(c => c.CellId)
                .ToList();

            return possibleCells;
        }

        /// <summary>
        /// Calculates all possible cells the ship can end on based on it's size and where it started
        /// </summary>
        /// <param name="player">The player who is placing the item</param>
        /// <param name="shipSize">The size of the ship to be placed</param>
        /// <param name="startingCell"></param>
        /// <returns>A list of cells that can accommodate the start of the ship (and have a valid end cell)</returns>
        public static List<Cell> CalculatePossibleEndingCells(Player player, int shipSize, Cell startingCell)
        {
            List<Cell> possibleCells = new List<Cell>();
            Row startingRow = player.Grid.Rows.First(r => r.RowId == startingCell.RowId);

            possibleCells = PossibleMovesForRowAndCell(player, startingRow, startingCell, possibleCells, shipSize, true);

            //Order by letter then by actual number (Otherwise it will go A1, A10, A2 etc.)
            possibleCells = possibleCells
                .OrderBy(c => c.RowId)
                .ThenBy(c => c.CellId)
                .ToList();

            return possibleCells;
        }

        #endregion
    }
}