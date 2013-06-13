using System;
using MT.Battleships.Helpers;

namespace MT.Battleships
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("****************************************");
            Console.WriteLine("* Welcome to Battleships - MT Edition! *");
            Console.WriteLine("****************************************");

            //First initialise the game (default players of 1 human 1 computer, 10x10 grids etc.
            BattleshipGame game = new BattleshipGame();
            //Provide the human the ability to place their battleships (automate the process for the computer)
            BattleshipGameHelper.PlaceShipsForPlayers(game);

            //While we haven't got a winner yet, offer up the next player's turn
            while (game.Winner == null)
            {
                BattleshipGameHelper.TakeTurn(game);
            }

            //Somebody has one, either congratulate or comiserate the human player
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("{0}, {1}",
                game.Winner.IsComputer ? "Comiserations" : "Congratulations",
                game.Winner.IsComputer ? "Better luck next time!" : "You are the winner!!!!!!!!!");

            //Leave the console open until the user wants to close it
            Console.ReadLine();
        }
    }
}