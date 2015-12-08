using System;
using System.Reflection;

namespace Mulholland.Core
{
	/// <summary>
	/// Provides general utilities.
	/// </summary>
	public class Utilities
	{
		static Utilities() {}


		/// <summary>
		/// Clones all the properties of an object that have read and write accesibility.
		/// </summary>
		/// <param name="source">Source object to clone from.</param>
		/// <param name="target">Object to write property values to.</param>
		public static void CloneProperties(object source, object target)
		{
			PropertyInfo[] properties = source.GetType().GetProperties();
			
			foreach (PropertyInfo property in properties)
			{
				if (property.CanRead && property.CanWrite)
				{					
					property.SetValue(target, property.GetValue(source,  property.GetIndexParameters()), property.GetIndexParameters());						
				}
			}			
		}


		/// <summary>
		/// Checks to see if a string exists in an array.
		/// </summary>
		/// <param name="check">Array to search.</param>
		/// <param name="searchFor">Value to seacrh for.</param>
		/// <returns>true if value exists, else false.</returns>
		public static bool IsStringInArray(string[] check, string searchFor)
		{
			bool result = false;

			foreach (string examine in check)
				if (examine == searchFor)
				{
					result = true;
					break;
				}

			return result;
		}
	}
}
