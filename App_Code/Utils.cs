using System;
using System.Data;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenBookAllegheny
{
	/// <summary>
	/// Class Methods for checking valid data formats
	/// </summary>
	public class Utils
	{
		/// <summary>
		/// Strips potentially harmful characters from strings. 
		/// Useful in verifying Query String input & minimizing 
		/// SQL Injection attacks.
		/// </summary>
		/// <param name="str">string to check</param>
		/// <returns>A cleaned string</returns>
		public static string CleanChars(string str)
		{
			string[] unsafechars = {
				"exe", "exec ", "xp_", "sp_",
				"drop", "delete ", "select ", "insert ", "update ", " from ", " into ",
				"declare ", "create ", "convert ", "cast ", "go ", 
				"set ", "where ",  "values ", "null", 
				"http", "js", "script ", "CRLF", "onvarchar", 
				"'", "/", 
				"&quot;", "&amp;", "&lt;", "&gt;",  
				"%3A",//;
				"%3B",//:
				"%3C",//<
				"%3D",//=
				"%3E",//>
				"%3F",//?
				"00100111",//'
				"00100010",//;
				"00111100",//<
				"0x"// hexadecimal prefix
			};
			// If unsafe character found 
			// reset string to nothing which can be discarded
			for (int i = 0; i < unsafechars.Length; i++)
			{
				if (str.Contains(unsafechars[i]))
					return str = "";
			}
			return str;
		}

		public static int IntFromQueryString(string paramName, int defaultValue)
		{
			int value;
			if (!int.TryParse(HttpContext.Current.Request.QueryString[paramName], out value))
				return defaultValue;
			return value;
		}
		public static string StringFromQueryString(string paramName, string defaultValue, string validList, bool CleanChars)
		{
			string value = (HttpContext.Current.Request.QueryString[paramName] ?? "").Trim();
			// Remove harmful characters - useful for keyword fields
			if (CleanChars)
			{
				value = Utils.CleanChars(value);
			}
			// Check against list - useful for drop downs
			if (!string.IsNullOrEmpty(validList))
			{
				if (!validList.Contains(value))
				{
					value = string.Empty;
				}
			}

			// Return Value
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}
			return value;
		}

		public static string RandomString(int size, bool lowerCase)
		{
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			char ch;
			for (int i = 0; i < size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
				builder.Append(ch);
			}
			if (lowerCase)
				return builder.ToString().ToLower();
			return builder.ToString();
		}
		
		
		public static string GetStringFromQueryString(string str, bool CleanChars)
		{
			// Remove leading and trailing spaces
			str = (str ?? "").Trim();
			
			// Remove harmful characters - useful for keyword fields
			if (CleanChars)
			{
				str = Utils.CleanChars(str);
			}
			return str;
		}
		public static string GetStringFromQueryString(string str, string validList)
		{
			// Remove leading and trailing spaces
			str = (str ?? "").Trim();

			// Check against known list of values supplied - useful for drop down items 
			// like Degrees or SearchOptions
			if(!validList.Contains(str))
			{
				str = string.Empty;
			}
			return str;
		}
		
		public static int GetIntFromQueryString(string str)
		{
			int value = 0;
			// Remove leading and trailing spaces
			str = (str ?? "").Trim();
			
			if (str != "")
			{
				Int32.TryParse(str, out value);
			}
			return value;
		}

        public static Boolean GetBooleanFromQueryString(string str)
        {
            Boolean value = false;
            // Remove leading and trailing spaces
            str = (str ?? "").Trim();

            if (str != "")
            {
                Boolean.TryParse(str, out value);
            }
            return value;
        }
        
        /// <summary>
		/// Verifies that the zip code is in correct format
		/// </summary>
		/// <param name="zip">zip</param>
		/// <returns>True if zip code is in 5 digit format</returns>
		public static bool IsValidZip(string zip)
		{
			// zip + 4
			//return Regex.IsMatch(zip, @"^\d{5}(\-\d{4})?$");
			return Regex.IsMatch(zip, @"^\d{5}?$");
			
		}
		/// <summary>
		/// Converts a string value into a currency 
		/// Valid decimal formats : "$00.00", "00.00", "0.0", "0"
		/// </summary>
		/// <param name="currency">currency string</param>
		/// <returns>returns 0 if things don't work out</returns>
		public static decimal ParseCurrency(string currency)
		{
			decimal value = 0;
			bool IsCurrency =
				decimal.TryParse(
					currency,
					System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowCurrencySymbol,
					null,
					out value 
				);
				
			return Math.Round(value, 2);
		}
		/// <summary>
		/// Checks GUID in correct format
		/// </summary>
		/// <param name="expression">expression string</param>
		/// <returns>True if item is a valid GUID, False otherwise</returns>
		public static bool IsGUID(string expression)
		{
			if (expression != null)
			{
				Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
				return guidRegEx.IsMatch(expression);
			}
			return false;
		}
	}
}