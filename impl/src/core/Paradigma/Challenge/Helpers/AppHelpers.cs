using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Works.Paradigma.Challenge.Helpers
{
    
	public static class AppHelpers
	{
		public static void FireException(EnumTypeException error, string errorMessage = default)
		{
			var message = $"{error}.{GetDescription<EnumTypeException>(error)}";
			if (!string.IsNullOrEmpty(errorMessage))
			{
				message = $"{message} Source: {errorMessage} ";
			}

			throw new ArgumentException(message);
		}
		public static string RepeatChar(int n = 1, char value = ' ')
		{
			return string.Concat(Enumerable.Repeat(value, n));
		}

		public static string GetDescription<T>(this T enumValue) where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
				return null;

			var description = enumValue.ToString();
			var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

			if (fieldInfo != null)
			{
				var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
				if (attrs != null && attrs.Length > 0)
				{
					description = ((DescriptionAttribute)attrs[0]).Description;
				}
			}

			return description;
		}
	}
}
