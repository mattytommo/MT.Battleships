using System;
using System.Collections.Generic;
using System.Linq;
using MT.Battleships.Components;

namespace MT.Battleships.Helpers
{
    public static class ConsoleHelper
    {
        /// <summary>
        /// Based on a list of possible values, validates whether or not the user has input a valid cell
        /// It will recurse until a valid value is entered
        /// </summary>
        /// <param name="possibleValues">List of allowed cell that the user can input</param>
        /// <returns>Returns the cell the user chose</returns>
        public static Cell ValidUserCellInputFromList(List<Cell> possibleValues)
        {
            bool validInput = false;
            Cell output = null;

            //While it's invalid input, keep asking for it
            while (!validInput)
            {
                string input = Console.ReadLine();
                bool showError = false;

                //If they've actually entered something longer than 1 character, it could be valid
                if (!string.IsNullOrWhiteSpace(input) && input.Length > 1)
                {
                    int outputCell;
                    string outputRow = input[0].ToString().ToUpper();
                    int.TryParse(input.Substring(1), out outputCell);
                    //If the first character in the string is a valid row and the remaining characters are a valid cell, it's valid
                    if (possibleValues.Any(p => p.CellId == outputCell && p.RowId == outputRow))
                    {
                        validInput = true;
                        output = possibleValues.First(p => p.CellId == outputCell && p.RowId == outputRow);
                    }
                    else
                    {
                        showError = true;
                    }
                }
                else
                {
                    showError = true;
                }

                //It's invalid, tell them to enter it again
                if (showError)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(
                        "Please enter an item from the following: {0}{1}",
                        Environment.NewLine + Environment.NewLine,
                        string.Join(", ", possibleValues.Select(c => c.RowId + c.CellId)));
                }
            }

            return output;
        }
    }
}