namespace System
{
    public static class Int32Extensions
    {
        /// <summary>
        /// Calculate the corresponding alphabet letter(s) from a number (if higher than 27, follow the format "AA", etc.)
        /// </summary>
        /// <param name="item">The number to be transformed to a letter</param>
        /// <returns>A string representation of the corresponding alphabet letter(s)</returns>
        public static string ToAlphabetString(this int item)
        {
            string output = string.Empty;

            //Upper chase characters start at charcode 65 (hence the use of 64)
            if (item <= 26)
            {
                output = ((char)(64 + item)).ToString();
            }
            if (item > 26)
            {
                //Add on the format of an additional letter for each multiple of 26, so 27 is "AA", 53 is "AAA" etc.
                while (item != 0)
                {
                    if (item > 26)
                    {
                        output += ((char) (64 + 1)).ToString();
                        item = item - 26;
                    }
                    else
                    {
                        output += ((char)(64 + item)).ToString();
                        item = 0;
                    }
                }
            }

            return output;
        }
    }
}