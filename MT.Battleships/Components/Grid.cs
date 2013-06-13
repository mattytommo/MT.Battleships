using System;
using System.Collections.Generic;

namespace MT.Battleships.Components
{
    public class Grid
    {
        #region Constructors

        public Grid()
        {
            Rows = new List<Row>();

            for (int i = 1; i <= 10; i++)
            {
                Rows.Add(new Row(i.ToAlphabetString()));
            }
        }

        #endregion

        #region Public Properties

        public List<Row> Rows { get; set; }

        #endregion
    }
}