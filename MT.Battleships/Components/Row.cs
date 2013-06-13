using System.Collections.Generic;

namespace MT.Battleships.Components
{
    public class Row
    {
        #region Constructors

        public Row(string rowId)
        {
            Cells = new List<Cell>();

            for (int i = 1; i <= 10; i++)
            {
                Cells.Add(new Cell(i, rowId));
            }

            RowId = rowId;
        }

        #endregion

        #region Public Properties

        public string RowId { get; set; }

        public List<Cell> Cells { get; set; }

        #endregion
    }
}
