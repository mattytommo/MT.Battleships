using System.Linq;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        #region Private Properties

        private static readonly Random random;

        #endregion

        #region Constructors

        static EnumerableExtensions()
        {
            //The random is initialised in this way so that it will only be seeded once
            //Usages of this class in quick succession without this parameter could result in returning the same seed
            random = new Random();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method for returning a random item out of an IEnumerable
        /// </summary>
        /// <typeparam name="T">Type of items in the collection</typeparam>
        /// <param name="input">Collection to take random element from</param>
        /// <returns>Random element taken from collection</returns>
        public static T Random<T>(this IEnumerable<T> input)
        {
            //Retrieve the random element (specifying the limit of Random to be the size of the collection)
            return input.ElementAt(random.Next(input.Count()));
        }

        #endregion
    }
}
