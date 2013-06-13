using System.Collections.Generic;
using MT.Battleships.Components.Base;
using MT.Battleships.Helpers;

namespace MT.Battleships.Components
{
    public class Player
    {
        #region Constructors

        public Player(bool isComputer, int playerId)
        {
            PlayerId = playerId;
            IsComputer = isComputer;
            Fleet = ShipHelper.BuildFleet();
            Grid = new Grid();
        }

        #endregion

        #region Public Properties

        public int PlayerId { get; set; }

        public bool IsComputer { get; set; }

        public List<IShip> Fleet { get; set; }

        public Grid Grid { get; set; }

        #endregion
    }
}