using MT.Battleships.Enums;

namespace MT.Battleships.Components
{
    public class Cell
    {
        #region Constructors

        public Cell(int cellId, string rowId)
        {
            CellId = cellId;
            State = CellState.Open;
            RowId = rowId;
        }

        #endregion

        #region Public Properties

        public int CellId { get; set; }

        public string RowId { get; set; }

        public CellState State { get; set; }

        #endregion
    }
}
