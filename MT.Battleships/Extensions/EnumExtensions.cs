using System.ComponentModel;
using System.Reflection;

namespace System
{
    public static class EnumExtensions
    {
        #region Private Methods

        /// <summary>
        /// Uses reflection to retrieve the value in the Description attribute of a given Enum value
        /// </summary>
        /// <param name="value">The enum value to retrieve the Description attribute from</param>
        /// <returns>A string retrieved from the value in the Description attribute, or just the value if no attribute is present</returns>
        private static string GetEnumDescription(Enum value)
        {
            //Retrieve the corresponding field using reflection
            FieldInfo fi = value.GetType().GetField(value.ToString());

            //Get the associated Description attributes for that field
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            //If there's more than one, just return the first one, otherwise return the value
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the value in the Description attribute of an Enum
        /// </summary>
        /// <typeparam name="TEnum">The type of enum</typeparam>
        /// <param name="enumValue">The value of the enum type</param>
        /// <returns></returns>
        public static string Description<TEnum>(this TEnum enumValue) where TEnum : struct
        {
            return GetEnumDescription((Enum)(object)(enumValue));
        }

        #endregion
    }
}