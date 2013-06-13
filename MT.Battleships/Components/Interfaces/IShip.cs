using System.Collections.Generic;
using MT.Battleships.Enums;

namespace MT.Battleships.Components.Base
{
    public interface IShip
    {
         ShipType Type { get; }
         int Size { get; }
         List<Cell> OccupyingCells { get; }
         int ShipId { get; }
         ShipState State { get; set; }
    }
}