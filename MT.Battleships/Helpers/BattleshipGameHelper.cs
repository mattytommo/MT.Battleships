using System;
using System.Collections.Generic;
using System.Linq;
using MT.Battleships.Components;
using MT.Battleships.Components.Base;
using MT.Battleships.Enums;

namespace MT.Battleships.Helpers
{
    public static class BattleshipGameHelper
    {
        #region Private Methods

        /// <summary>
        /// For each ship in a Computer Player's fleet, auto place them on the game grid
        /// </summary>
        /// <param name="player">The computer player to place the ships for</param>
        private static void AutoPlaceShipsForComputer(Player player)
        {
            foreach (IShip ship in player.Fleet)
            {
                //First, get the possible cells
                List<Cell> possibleCells = GridHelper.CalculatePossibleStartingCells(player, ship.Size);

                //Randomly pick a cell
                Cell startingCell = possibleCells.Random();

                //Based on the picked cell, determine the possible ending cells
                List <Cell> possibleEndingCells = GridHelper.CalculatePossibleEndingCells(player, ship.Size, startingCell);

                Cell endingCell = possibleEndingCells.Random();

                //Set the ship to be occupying the cells (including the ones between the start and end)
                PlaceShipInCells(player, ship, startingCell, endingCell);
            }
        }

        /// <summary>
        /// For each ship in a Human Player's fleet, prompt them to place them on the game grid
        /// </summary>
        /// <param name="player">The computer player to place the ships for</param>
        private static void PlaceShipsForHuman(Player player)
        {
            foreach (IShip ship in player.Fleet)
            {
                List<Cell> possibleCells = GridHelper.CalculatePossibleStartingCells(player, ship.Size);

                Console.WriteLine(Environment.NewLine);
                //Ask the user for a move, present them with the possible moves
                Console.WriteLine(
                    "Please specify the row and cell that your {0} starts in (it occupies {1} squares) from the following: {2}{3}",
                    ship.Type.Description(),
                    ship.Size,
                    Environment.NewLine + Environment.NewLine,
                    string.Join(", ", possibleCells.Select(c => c.RowId + c.CellId)));

                //When they enter a valid move, we take that as the starting cell
                Cell startingCell = ConsoleHelper.ValidUserCellInputFromList(possibleCells);

                //Now we can work out the possible ending cells
                List<Cell> possibleEndingCells = GridHelper.CalculatePossibleEndingCells(player, ship.Size, startingCell);

                //Prompt the user to choose from the list of possible ending cells
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(
                    "Please specify the row and cell that your {0} ends in (it occupies {1} squares) from the following: {2}{3}",
                    ship.Type.Description(),
                    ship.Size,
                    Environment.NewLine + Environment.NewLine,
                    string.Join(", ", possibleEndingCells.Select(c => c.RowId + c.CellId)));

                Cell endingCell = ConsoleHelper.ValidUserCellInputFromList(possibleEndingCells);

                //Place the ship in those cells, set them to occupied (and add them to the Ship's OccupyingCells collection)
                PlaceShipInCells(player, ship, startingCell, endingCell);
            }
        }

        /// <summary>
        /// For each of the cells from the start and end, set them to occupied and the set the ship's occupying cells
        /// </summary>
        /// <param name="player">Player who is placing the ship</param>
        /// <param name="ship">Ship that is being placed</param>
        /// <param name="startingCell">Cell the ship starts at</param>
        /// <param name="endingCell">Cell the ship ends at</param>
        private static void PlaceShipInCells(Player player, IShip ship, Cell startingCell, Cell endingCell)
        {
            IShip correspondingShip = player.Fleet.First(s => s.ShipId == ship.ShipId);
            correspondingShip.State = ShipState.Placed;

            List<Cell> crossingCells;
            startingCell.State = CellState.Occupied;
            endingCell.State = CellState.Occupied;

            Row startingRow = player.Grid.Rows.First(r => r.RowId == startingCell.RowId);
            Row endingRow = player.Grid.Rows.First(r => r.RowId == endingCell.RowId);

            int startRowIndex = player.Grid.Rows.FindIndex(i => i.RowId == startingRow.RowId);
            int endRowIndex = player.Grid.Rows.FindIndex(i => i.RowId == endingRow.RowId);

            //If we're going horzontally
            if (startingRow.RowId == endingRow.RowId)
            {
                //If we're going horizontally forward
                if (startingCell.CellId < endingCell.CellId)
                {
                    crossingCells = startingRow.Cells
                         .Where(c => c.CellId > startingCell.CellId && c.CellId < endingCell.CellId)
                         .ToList();
                }
                else
                {
                    //Otherwise we're going horizontally backwards
                    crossingCells = startingRow.Cells
                         .Where(c => c.CellId < startingCell.CellId && c.CellId > endingCell.CellId)
                         .ToList();
                }
            }
            else
            {
                //Otherwise, we're going vertically
                if (startRowIndex < endRowIndex)
                {
                    //If the ending row is after the starting one (if we're going vertically forwards)
                    crossingCells = player.Grid.Rows
                        .Where(r => player.Grid.Rows.FindIndex(i => i.RowId == r.RowId) > startRowIndex)
                        .Where(r => player.Grid.Rows.FindIndex(i => i.RowId == r.RowId) < endRowIndex)
                        .Select(r => r.Cells[startingCell.CellId - 1])
                        .ToList();
                }
                else
                {
                    //Otherwise the ending row is before the starting one (we're going vertically backwards)
                    crossingCells = player.Grid.Rows
                        .Where(r => player.Grid.Rows.FindIndex(i => i.RowId == r.RowId) < startRowIndex)
                        .Where(r => player.Grid.Rows.FindIndex(i => i.RowId == r.RowId) > endRowIndex)
                        .Select(r => r.Cells[startingCell.CellId - 1])
                        .ToList();
                }
            }

            //Add the start and end cell, otherwise they'll not be set as occupied
            crossingCells.AddRange(new [] { startingCell, endingCell });

            //Set them all to occupied in the player's grid, then add them to the Ship's OccupyingCells list
            foreach (Cell crossingCell in crossingCells)
            {
                player.Grid.Rows
                    .First(r => r.RowId == crossingCell.RowId).Cells
                    .First(c => c.CellId == crossingCell.CellId).State = CellState.Occupied;
            }

            ship.OccupyingCells.AddRange(crossingCells);
        }

        /// <summary>
        /// Automates a random turn for the computer player
        /// </summary>
        /// <param name="game">The current game being played</param>
        /// <param name="currentPlayer">The current computer player</param>
        private static void AutoTakeTurnComputer(BattleshipGame game, Player currentPlayer)
        {
            //We need to know the other player as we actually operate on their grid (that's where their ships are placed)
            Player otherPlayer = game.Players.First(p => p.PlayerId != currentPlayer.PlayerId);

            List<Cell> possibleMoves = otherPlayer.Grid.Rows
                .SelectMany(r => r.Cells)
                .Where(r => r.State != CellState.Hit)
                .ToList();

            Cell moveMade = possibleMoves.Random();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Computer goes for {0}.", moveMade.RowId + moveMade.CellId);

            //If the cell picked is occupied, we've hit a ship
            if (moveMade.State == CellState.Occupied)
            {
                moveMade.State = CellState.Hit;

                IShip shipHit = otherPlayer.Fleet
                    .First(s => s.OccupyingCells.Any(c => c.CellId == moveMade.CellId && c.RowId == moveMade.RowId));

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("BOOM, the computer has hit your {0}!", shipHit.Type.Description());

                //Set the ship and the cell to be hit
                shipHit.State = ShipState.Hit;
                Cell cellHit = shipHit.OccupyingCells.First(c => c.CellId == moveMade.CellId && c.RowId == moveMade.RowId);
                cellHit.State = CellState.Hit;

                //if all cells that ship is occupying have now been hit, the ship has been sunk
                if (shipHit.OccupyingCells.All(c => c.State == CellState.Hit))
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Uh oh!!!! The computer has sunk your {0}!!!", shipHit.Type.Description());
                    shipHit.State = ShipState.Sunk;
                }

                //If all other player's ships have been sunk, the current player has won
                if (otherPlayer.Fleet.All(s => s.State == ShipState.Sunk))
                {
                    game.Winner = currentPlayer;
                }
            }
            else
            {
                //There's nothing in the cell, but set it to Hit so it can't be chosen again
                moveMade.State = CellState.Hit;
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Luckily, it's a miss this time!");
            }
        }

        /// <summary>
        /// Handles a Human user taking a turn
        /// </summary>
        /// <param name="game">The current game being played</param>
        /// <param name="currentPlayer">The current user whose turn it is</param>
        private static void TakeTurnHuman(BattleshipGame game, Player currentPlayer)
        {
            Player otherPlayer = game.Players.First(p => p.PlayerId != currentPlayer.PlayerId);

            List<Cell> possibleMoves = otherPlayer.Grid.Rows
                .SelectMany(r => r.Cells)
                .Where(r => r.State != CellState.Hit)
                .ToList();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(
                "Please enter your move from the following options: {0}{1}",
                Environment.NewLine,
                string.Join(", ", possibleMoves.Select(c => c.RowId + c.CellId)));

            Cell moveMade = ConsoleHelper.ValidUserCellInputFromList(possibleMoves);

            Console.WriteLine(Environment.NewLine);

            if (moveMade.State == CellState.Occupied)
            {
                moveMade.State = CellState.Hit;
                Console.WriteLine("BOOM. It's a hit!!!!!!!");

                IShip shipHit = otherPlayer.Fleet
                    .First(s => s.OccupyingCells.Any(c => c.CellId == moveMade.CellId && c.RowId == moveMade.RowId));

                shipHit.State = ShipState.Hit;

                Cell cellHit = shipHit.OccupyingCells.First(c => c.CellId == moveMade.CellId && c.RowId == moveMade.RowId);
                cellHit.State = CellState.Hit;

                if (shipHit.OccupyingCells.All(c => c.State == CellState.Hit))
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Congratulations! You sunk the computer's {0}!!!", shipHit.Type.Description());
                    shipHit.State = ShipState.Sunk;
                }

                if (otherPlayer.Fleet.All(s => s.State == ShipState.Sunk))
                {
                    game.Winner = currentPlayer;
                }
            }
            else
            {
                moveMade.State = CellState.Hit;
                Console.WriteLine("D'oh, it's a miss this time!");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// First part of the game, either allow the human player to place their ships on the grid, or auto-place them
        /// for computer players
        /// </summary>
        /// <param name="game">The game that holds the players, grid, etc.</param>
        public static void PlaceShipsForPlayers(BattleshipGame game)
        {
            //Place ships for all players, depending on whether it's the computer we'll automate this step
            foreach (Player player in game.Players)
            {
                if (player.IsComputer)
                {
                    AutoPlaceShipsForComputer(player);
                }
                else
                {
                    PlaceShipsForHuman(player);
                }
            }
        }

        /// <summary>
        /// Handles taking the next turn in the game (based on the Current Player's turn in the passed in game object)
        /// </summary>
        /// <param name="game">The current game being played</param>
        public static void TakeTurn(BattleshipGame game)
        {
            //Get the current player who's turn it is
            Player currentPlayer = game.Players.First(p => p.PlayerId == game.CurrentPlayersTurn);

            //If they're computer, automate it, otherwise prompt for a choice
            if (currentPlayer.IsComputer)
            {
                AutoTakeTurnComputer(game, currentPlayer);
            }
            else
            {
                TakeTurnHuman(game, currentPlayer);
            }

            //Now the current player has taken their turn, switch to the other player (there's only 2)
            game.CurrentPlayersTurn = game.Players.First(p => p.PlayerId != currentPlayer.PlayerId).PlayerId;
        }

        #endregion
    }
}