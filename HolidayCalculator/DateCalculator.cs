using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Reflection;
using System.Xml.Schema;

namespace TestHolidayCalculation
{

	[XmlRoot("HolidaySet")]
	public class HolidaySet : List<Holiday>, IConfigurationSectionHandler
	{

		/// <summary>
		/// Determines if the given date is a holiday
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public bool IsHoliday(DateTime date)
		{
			//are there any holidays which next date is today
			return this.Any(holiday => holiday.NextDate(date.AddDays(-1)) == date);
		}

		/// <summary>
		/// Get all the holidays between two dates
		/// </summary>
		/// <param name="startDate">exclusive start date for range</param>
		/// <param name="endDate">inclusive end date for range</param>
		/// <returns></returns>
		public IEnumerable<DateTime> GetHolidays(DateTime startDate, DateTime endDate)
		{
			return this.SelectMany(h => h.NextDates(startDate).TakeWhile(d => d < endDate)).OrderBy(d => d);
		}

		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			XmlSerializer ser = new XmlSerializer(this.GetType());
			XmlNodeReader xNodeReader = new XmlNodeReader(section);
			return ser.Deserialize(xNodeReader);
		}
	}

	/// <summary>
	/// Base class for calculating complex dates
	/// </summary>
	public abstract class DateCalculator : IXmlSerializable
	{
		/// <summary>
		/// List of DateCalculator types to assist deserialization
		/// </summary>
		protected static Dictionary<string, Type> NameToDateCalculatorType = typeof(DateCalculator).Assembly.GetTypes()
			.Where(t => typeof(DateCalculator).IsAssignableFrom(t))
			.ToDictionary(t => t.Name, t => t);

		/// <summary>
		/// Gets the properties that will be used for attributes on serialization
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerable<PropertyInfo> GetPropertiesForXmlAttributes()
		{
			return this.GetType().GetProperties().Where(p => p.PropertyType.IsPrimitive || p.PropertyType == typeof (string) || p.PropertyType.IsEnum);
		}

		/// <summary>
		/// Gets the property that will be used for the content node
		/// </summary>
		/// <returns></returns>
		protected virtual PropertyInfo GetPropertyForXmlContent()
		{
			//first property that with a type of DateCalculator is our inner property
			return this.GetType().GetProperties()
				.FirstOrDefault(p => typeof (DateCalculator).IsAssignableFrom(p.PropertyType));
			
		}

		/// <summary>
		/// Returns the next occurance of the date after the given startDate
		/// </summary>
		/// <param name="startDate"></param>
		/// <returns></returns>
		public abstract DateTime? NextDate(DateTime startDate);

		/// <summary>
		/// Enumerates through all the next dates.
		/// WARNING: this enumerable may be never eneding do not try and enumerate
		/// over entire set!
		/// </summary>
		/// <param name="startDate">non-include date to start looking from</param>
		/// <returns></returns>
		public IEnumerable<DateTime> NextDates(DateTime startDate)
		{
			DateTime? newStartDate = this.NextDate(startDate);
			while (newStartDate.HasValue)
			{
				yield return newStartDate.Value;
				newStartDate = this.NextDate(newStartDate.Value);
			}
		}

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(System.Xml.XmlReader reader)
		{
			//populate our properteis from teh attribute
			foreach (var property in GetPropertiesForXmlAttributes())
			{
				string valueString = reader.GetAttribute(property.Name);
				object value;
				if(TryConvert(valueString, property.PropertyType, out value))
					property.SetValue(this, value, null);
			}

			//do some special handling to determine empty nodes and read our start element
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (isEmptyElement)
				return;

			reader.MoveToContent();

			//if we have an inner property we need to handle that
			var contentProperty = GetPropertyForXmlContent();
			if (contentProperty != null)
			{

				Type innerElementType;
				if (!NameToDateCalculatorType.TryGetValue(reader.Name, out innerElementType))
					throw new ConfigurationErrorsException(string.Format("Unknown element: '{0}'", reader.Name));
				var innerElement = (DateCalculator) Activator.CreateInstance(innerElementType);
				
				//let the element read its own XML
				innerElement.ReadXml(reader);
				
				contentProperty.SetValue(this, innerElement, null);

				
			}
			
			reader.ReadEndElement();
			reader.MoveToContent();

		}

		public void WriteXml(System.Xml.XmlWriter writer)
		{
			var attributeProperties = this.GetType().GetProperties().Where(p=>p.PropertyType.IsPrimitive || p.PropertyType == typeof(string));
			foreach (var property in attributeProperties)
			{
				var value = property.GetValue(this, null);
				if(value == null)
					continue;

				writer.WriteAttributeString(property.Name, null, value.ToString());
			}

			var contentProperties = this.GetType().GetProperties().Where(p => typeof(DateCalculator).IsAssignableFrom(p.PropertyType));
			foreach (var contentProperty in contentProperties)
			{
				var value = contentProperty.GetValue(this, null) as DateCalculator;
				if (value == null)
					continue;

				writer.WriteStartElement(value.GetType().Name);
				value.WriteXml(writer);
				writer.WriteEndElement();
			}

		}

		/// <summary>
		/// Attempts to convert the string to the given type 
		/// </summary>
		/// <param name="str">The string to convert</param>
		/// <param name="type">The type to convert the string into</param>
		/// <param name="result">The result of the conversion, null if conversion failed</param>
		/// <returns>true if the conversion was successful, otherwise false</returns>
		protected static bool TryConvert(string str, Type type, out object result)
		{

			result = null;
			try
			{
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (converter.CanConvertFrom(typeof(string)))
				{
					result = converter.ConvertFromInvariantString(str);
					return true;
				}

				//break out of our nullable type so we can handle enum types easier
				bool isNullable = false;
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					isNullable = true;
					type = Nullable.GetUnderlyingType(type);
				}

				//enums are not supported by default, so we will do our own parsing logic
				if (type.IsEnum)
				{
					result = Enum.Parse(type, str, true);
					if (isNullable)
						result = Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type), result);
					return true;
				}

				return false;
			}
			catch
			{
				//Unfortunatly catching exception is the only way to know if it failed
				return false;
			}
		}
	}

	/// <summary>
	/// Container for a holiday, has name and inner element to perform actual calculation
	/// </summary>
	public class Holiday : DateCalculator
	{
		public string Name { get; set; }
		public DateCalculator Inner { get; set; }

		public override DateTime? NextDate(DateTime startDate)
		{
			return Inner.NextDate(startDate);
		}
	}

	/// <summary>
	/// Represents a date that occurs the same date every year
	/// </summary>
	public class FixedDate : DateCalculator
	{
		public int Day { get; set; }
		public int Month { get; set; }
		public int? Year { get; set; }

		public override DateTime? NextDate(DateTime startDate)
		{
			//if we have a year then we really are a fixed date
			if(Year.HasValue)
			{
				var nextDate = new DateTime(Year.Value, Month, Day);
				if (nextDate <= startDate)
					return null;
				return nextDate;
			}

			int year = startDate.Year;
			if(startDate.Month > Month || (startDate.Month==Month && startDate.Day >= Day))
				year += 1;

			//make sure 29th of Feb dates are are used in leap years
			while (Day == 29 && Month == 2 && !DateTime.IsLeapYear(year))
				year += 1;

			return new DateTime(year, Month, Day);
		}
	}

	/// <summary>
	/// Finds dates that are so many days before (or after) the inner date
	/// </summary>
	public class DaysAfter : DateCalculator
	{
		public int Days { get; set; }
		public DateCalculator Inner { get; set; }

		public override DateTime? NextDate(DateTime startDate)
		{
			//make suer we step our inner item back, so we can catch days that are in teh future
			//but but the inner occurance day is in the pase
			foreach (var innerDate in Inner.NextDates(startDate.AddDays(-Days)))
			{
				var nextDate = innerDate.AddDays(Days);
				if (nextDate > startDate)
					return nextDate;
			}
			return null;
		}
	}

	/// <summary>
	/// Finds the date that falls on the given DayOfWeek that is closest to the inner date
	/// </summary>
	public class ClosestDayOfWeek : DateCalculator
	{
		public DayOfWeek DayOfWeek { get; set; }
		public DateCalculator Inner { get; set; }

		public override DateTime? NextDate(DateTime startDate)
		{
			foreach (var innerDate in Inner.NextDates(startDate.AddDays(-7)))
			{
				var innerDayOfWeek = innerDate.DayOfWeek;

				int dayOfWeekInt = (int) DayOfWeek;
				int innerDayOfWeekInt = (int) innerDayOfWeek;
				if (dayOfWeekInt < innerDayOfWeekInt)
					dayOfWeekInt += 7;

				int distFoward = dayOfWeekInt - innerDayOfWeekInt;
				int distBackward = 7 - distFoward;

				DateTime nextDate;
				if (distFoward < distBackward)
					nextDate = innerDate.AddDays(distFoward);
				else
					nextDate = innerDate.AddDays(-distBackward);

				if (nextDate > startDate)
					return nextDate;
			}
			return null;
		}
	}

	/// <summary>
	/// Gets date that falls on the first weekday on or after the inner date
	/// </summary>
	public class WeekdayOnOrAfter : DateCalculator
	{
		public DateCalculator Inner { get; set; }

		public override DateTime? NextDate(DateTime startDate)
		{
			//Need to stpe back if today is a saturday or sunday, because the inner day may
			//be excluded if our startDate is today, but if today is a weekend the actual date
			//will be hte monday and we would not have returned it.
			int stepBack = 0;
			if(startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
				stepBack += 1;
			if(startDate.DayOfWeek == DayOfWeek.Sunday)
				stepBack += 2;


			foreach (var innerDate in Inner.NextDates(startDate.AddDays(-stepBack)))
			{
				var nextDate = innerDate;
				while(nextDate.DayOfWeek == DayOfWeek.Saturday || nextDate.DayOfWeek == DayOfWeek.Sunday)
				{
					nextDate = nextDate.AddDays(1);
				}
				return nextDate;
			}
			return null;
		}
	}

	/// <summary>
	/// Calculates the date that falls on a day occurance within a month
	/// for example 1st Monday in June
	/// </summary>
	public class XthDayOfWeekInMonth : DateCalculator
	{

		public DayOfWeek DayOfWeek { get; set; }
		public int DayOccurance { get; set; }
		public int Month { get; set; }

		public override DateTime? NextDate(DateTime startDate)
		{
			var nextDate = new DateTime(startDate.Year, Month, 1);
			nextDate = nextDate.AddDays((DayOccurance - 1) * 7);

			while(nextDate.DayOfWeek != DayOfWeek)
			{
				nextDate = nextDate.AddDays(1);
			}

			if (nextDate > startDate)
				return nextDate;
			return NextDate(new DateTime(startDate.Year + 1, 1, 1));
		}
	}

	/// <summary>
	/// Calculate Easter Sunday
	/// </summary>
	public class EasterSunday : DateCalculator
	{
		public override DateTime? NextDate(DateTime startDate)
		{
			DateTime workDate =  new DateTime(startDate.Year, startDate.Month, 1);
			int year = workDate.Year;
			if (workDate.Month > 4)
				year = year + 1;


			int a = year % 19;
			int b = year / 100;
			int c = year % 100;
			int d = b / 4;
			int e = b % 4;
			int f = (b + 8) / 25;
			int g = (b - f + 1) / 3;
			int h = (19 * a + b - d - g + 15) % 30;
			int i = c / 4;
			int k = c % 4;
			int l = (32 + 2 * e + 2 * i - h - k) % 7;
			int m = (a + 11 * h + 22 * l) / 451;
			int easterMonth = (h + l - 7 * m + 114) / 31;
			int p = (h + l - 7 * m + 114) % 31;
			int easterDay = p + 1;
			DateTime nextDate = new DateTime(year, easterMonth, easterDay);
			if (nextDate > startDate)
				return new DateTime(year, easterMonth, easterDay);
			return NextDate(new DateTime(year + 1, 1, 1));

		}

	}

}
