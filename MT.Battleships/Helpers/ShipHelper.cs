using System.Collections.Generic;
using MT.Battleships.Components;
using MT.Battleships.Components.Base;

namespace MT.Battleships.Helpers
{
    public static class ShipHelper
    {
        /// <summary>
        /// Builds the default collection of Ships (1 x Battleship, 2 x Destroyers)
        /// </summary>
        /// <returns>A List containing the Ships (as the Interface)</returns>
        public static List<IShip> BuildFleet()
        {
            int counter = 1;
            List<IShip> fleet = new List<IShip>();

            //Build the default fleet, increment the counter so the Ids are unique
            fleet.Add(new Battleship(counter));
            counter++;
            fleet.Add(new Destroyer(counter));
            counter++;
            fleet.Add(new Destroyer(counter));

            return fleet;
        }
    }
}