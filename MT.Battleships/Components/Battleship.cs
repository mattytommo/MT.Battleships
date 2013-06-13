using System.Collections.Generic;
using MT.Battleships.Components.Base;
using MT.Battleships.Enums;

namespace MT.Battleships.Components
{
    public class Battleship : IShip
    {
        #region Constructors

        public Battleship(int shipId)
        {
            ShipId = shipId;
            State = ShipState.ReadyToPlace;
            OccupyingCells = new List<Cell>();
        }

        #endregion

        #region IShip Implementation

        public int ShipId { get; set; }

        public ShipState State { get; set; }

        public List<Cell> OccupyingCells { get; set; }

        public ShipType Type
        {
            get
            {
                return ShipType.Battleship;
            }
        }

        public int Size
        {
            get
            {
                return (int)ShipType.Battleship;
            }
        }

        #endregion
    }
}