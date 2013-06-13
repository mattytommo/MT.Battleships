using System.Collections.Generic;
using MT.Battleships.Components.Base;
using MT.Battleships.Enums;

namespace MT.Battleships.Components
{
    public class Destroyer : IShip
    {
        #region Constructors

        public Destroyer(int shipId)
        {
            ShipId = shipId;
            OccupyingCells = new List<Cell>();
            State = ShipState.ReadyToPlace;
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
                return ShipType.Destroyer;
            }
        }

        public int Size
        {
            get
            {
                return (int)ShipType.Destroyer;
            }
        }

        #endregion
    }
}