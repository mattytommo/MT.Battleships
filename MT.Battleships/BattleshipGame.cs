using System.Collections.Generic;
using System.Linq;
using MT.Battleships.Components;

namespace MT.Battleships
{
    public class BattleshipGame
    {
        #region Constructors

        public BattleshipGame()
        {
            Players = new List<Player>
                {
                    new Player(false, 1),
                    new Player(true, 2)
                };

            CurrentPlayersTurn = Players.First().PlayerId;
        }

        #endregion

        #region Public Properties

        public List<Player> Players { get; set; }

        public int CurrentPlayersTurn { get; set; }

        public Player Winner { get; set; }

        #endregion
    }
}
