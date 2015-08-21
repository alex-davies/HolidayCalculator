using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestHolidayCalculation
{
	[TestClass]
	public class TestDateCalculation
	{
		public List<object> AucklandHolidays = new List<object>
		{
		        new DateTime(2010, 1, 1), "New Years Day",
				new DateTime(2010, 1, 4), "Day after New Years Day",
				new DateTime(2010, 2, 1), "Auckland Anniversary",
				new DateTime(2010, 2, 6), "Waitangi Day",
				new DateTime(2010, 4, 2), "Good Friday",
				new DateTime(2010, 4, 5), "Easter Monday",
				new DateTime(2010, 4, 25), "Anzac Day",
				new DateTime(2010, 6, 7), "Queens Birthday",
				new DateTime(2010, 10, 25), "Labour Day",
				new DateTime(2010, 12, 27), "Christmas",
				new DateTime(2010, 12, 28), "Boxing Day",

				new DateTime(2011, 1, 3), "New Years Day",
				new DateTime(2011, 1, 4), "Day after New Years Day",
				new DateTime(2011, 1, 31), "Auckland Anniversary",
				new DateTime(2011, 2, 6), "Waitangi Day",
				new DateTime(2011, 4, 22), "Good Friday",
				new DateTime(2011, 4, 25), "Easter Monday",
				new DateTime(2011, 4, 25), "Anzac Day",
				new DateTime(2011, 6, 6), "Queens Birthday",
				new DateTime(2011, 10, 24), "Labour Day",
				new DateTime(2011, 12, 26), "Christmas",
				new DateTime(2011, 12, 27), "Boxing Day",

				new DateTime(2012, 1, 2), "New Years Day",
				new DateTime(2012, 1, 3), "Day after New Years Day",
				new DateTime(2012, 1, 30), "Auckland Anniversary",
				new DateTime(2012, 2, 6), "Waitangi Day",
				new DateTime(2012, 4, 6), "Good Friday",
				new DateTime(2012, 4, 9), "Easter Monday",
				new DateTime(2012, 4, 25), "Anzac Day",
				new DateTime(2012, 6, 4), "Queens Birthday",
				new DateTime(2012, 10, 22), "Labour Day",
				new DateTime(2012, 12, 25), "Christmas",
				new DateTime(2012, 12, 26), "Boxing Day",

				new DateTime(2013, 1, 1), "New Years Day",
				new DateTime(2013, 1, 2), "Day after New Years Day",
				new DateTime(2013, 1, 28), "Auckland Anniversary",
				new DateTime(2013, 2, 6), "Waitangi Day",
				new DateTime(2013, 3, 29), "Good Friday",
				new DateTime(2013, 4, 1), "Easter Monday",
				new DateTime(2013, 4, 25), "Anzac Day",
				new DateTime(2013, 6, 3), "Queens Birthday",
				new DateTime(2013, 10, 28), "Labour Day",
				new DateTime(2013, 12, 25), "Christmas",
				new DateTime(2013, 12, 26), "Boxing Day",

				new DateTime(2014, 1, 1), "New Years Day",
				new DateTime(2014, 1, 2), "Day after New Years Day",
				new DateTime(2014, 1, 27), "Auckland Anniversary",
				new DateTime(2014, 2, 6), "Waitangi Day",
				new DateTime(2014, 4, 18), "Good Friday",
				new DateTime(2014, 4, 21), "Easter Monday",
				new DateTime(2014, 4, 25), "Anzac Day",
				new DateTime(2014, 6, 2), "Queens Birthday",
				new DateTime(2014, 10, 27), "Labour Day",
				new DateTime(2014, 12, 25), "Christmas",
				new DateTime(2014, 12, 26), "Boxing Day",

				new DateTime(2015, 1, 1), "New Years Day",
				new DateTime(2015, 1, 2), "Day after New Years Day",
				new DateTime(2015, 1, 26), "Auckland Anniversary",
				new DateTime(2015, 2, 6), "Waitangi Day",
				new DateTime(2015, 4, 3), "Good Friday",
				new DateTime(2015, 4, 6), "Easter Monday",
				new DateTime(2015, 4, 25), "Anzac Day",
				new DateTime(2015, 6, 1), "Queens Birthday",
				new DateTime(2015, 10, 26), "Labour Day",
				new DateTime(2015, 12, 25), "Christmas",
				new DateTime(2015, 12, 28), "Boxing Day",

				new DateTime(2016, 1, 1), "New Years Day",
				new DateTime(2016, 1, 4), "Day after New Years Day",
				new DateTime(2016, 2, 1), "Auckland Anniversary",
				new DateTime(2016, 2, 6), "Waitangi Day",
				new DateTime(2016, 3, 25), "Good Friday",
				new DateTime(2016, 3, 28), "Easter Monday",
				new DateTime(2016, 4, 25), "Anzac Day",
				new DateTime(2016, 6, 6), "Queens Birthday",
				new DateTime(2016, 10, 24), "Labour Day",
				new DateTime(2016, 12, 26), "Christmas",
				new DateTime(2016, 12, 27), "Boxing Day",

				new DateTime(2017, 1, 2), "New Years Day",
				new DateTime(2017, 1, 3), "Day after New Years Day",
				new DateTime(2017, 1, 30), "Auckland Anniversary",
				new DateTime(2017, 2, 6), "Waitangi Day",
				new DateTime(2017, 4, 14), "Good Friday",
				new DateTime(2017, 4, 17), "Easter Monday",
				new DateTime(2017, 4, 25), "Anzac Day",
				new DateTime(2017, 6, 5), "Queens Birthday",
				new DateTime(2017, 10, 23), "Labour Day",
				new DateTime(2017, 12, 25), "Christmas",
				new DateTime(2017, 12, 26), "Boxing Day",

				new DateTime(2018, 1, 1), "New Years Day",
				new DateTime(2018, 1, 2), "Day after New Years Day",
				new DateTime(2018, 1, 29), "Auckland Anniversary",
				new DateTime(2018, 2, 6), "Waitangi Day",
				new DateTime(2018, 3, 30), "Good Friday",
				new DateTime(2018, 4, 2), "Easter Monday",
				new DateTime(2018, 4, 25), "Anzac Day",
				new DateTime(2018, 6, 4), "Queens Birthday",
				new DateTime(2018, 10, 22), "Labour Day",
				new DateTime(2018, 12, 25), "Christmas",
				new DateTime(2018, 12, 26), "Boxing Day",

				new DateTime(2019, 1, 1), "New Years Day",
				new DateTime(2019, 1, 2), "Day after New Years Day",
				new DateTime(2019, 1, 28), "Auckland Anniversary",
				new DateTime(2019, 2, 6), "Waitangi Day",
				new DateTime(2019, 4, 19), "Good Friday",
				new DateTime(2019, 4, 22), "Easter Monday",
				new DateTime(2019, 4, 25), "Anzac Day",
				new DateTime(2019, 6, 3), "Queens Birthday",
				new DateTime(2019, 10, 28), "Labour Day",
				new DateTime(2019, 12, 25), "Christmas",
				new DateTime(2019, 12, 26), "Boxing Day",

				new DateTime(2020, 1, 1), "New Years Day",
				new DateTime(2020, 1, 2), "Day after New Years Day",
				new DateTime(2020, 1, 27), "Auckland Anniversary",
				new DateTime(2020, 2, 6), "Waitangi Day",
				new DateTime(2020, 4, 10), "Good Friday",
				new DateTime(2020, 4, 13), "Easter Monday",
				new DateTime(2020, 4, 25), "Anzac Day",
				new DateTime(2020, 6, 1), "Queens Birthday",
				new DateTime(2020, 10, 26), "Labour Day",
				new DateTime(2020, 12, 25), "Christmas",
				new DateTime(2020, 12, 28), "Boxing Day"
		};

		public void TestEaster()
		{
			AssertDates(new EasterSunday().NextDates(new DateTime(2010, 1, 1)),
				new DateTime(2010, 4, 4),
				new DateTime(2011, 4, 24),
				new DateTime(2012, 4, 8),
				new DateTime(2013, 3, 31),
				new DateTime(2014, 4, 20),
				new DateTime(2015, 4, 5),
				new DateTime(2016, 3, 27));
		}

		[TestMethod]
		public void TestEasterOnAndAfterTheDay()
		{
			Assert.AreEqual(new DateTime(2011, 4, 24), new EasterSunday().NextDate(new DateTime(2010, 4, 4)));
			Assert.AreEqual(new DateTime(2011, 4, 24), new EasterSunday().NextDate(new DateTime(2010, 10, 4)));
		}

		[TestMethod]
		public void TestFixedDates()
		{
			var complexDate = new FixedDate() {Day = 6, Month = 2};
			
			AssertDates(complexDate.NextDates(new DateTime(2011, 2, 6)), 
				new DateTime(2012, 2, 6),
				new DateTime(2013, 2, 6),
				new DateTime(2014, 2, 6),
				new DateTime(2015, 2, 6),
				new DateTime(2016, 2, 6));
		}

		[TestMethod]
		public void TestFixedDateWithYear()
		{
			var complexDate = new FixedDate() { Day = 6, Month = 2, Year = 2010};

			Assert.AreEqual(
				new DateTime(2010, 2, 6), 
				complexDate.NextDate(new DateTime(2009, 2, 6)));
			
			Assert.AreEqual(
				null, 
				complexDate.NextDate(new DateTime(2010, 2, 6)));

		}

		[TestMethod]
		public void TestFixedDateOnLeapYear()
		{
			var complexDate = new FixedDate() { Day = 29, Month = 2 };

			AssertDates(complexDate.NextDates(new DateTime(2011, 1, 1)), 
				new DateTime(2012, 2, 29),
				new DateTime(2016, 2, 29),
				new DateTime(2020, 2, 29),
				new DateTime(2024, 2, 29),
				new DateTime(2028, 2, 29));

			Assert.AreEqual(new DateTime(2016, 2, 29), complexDate.NextDate(new DateTime(2012, 2, 29)));
			Assert.AreEqual(new DateTime(2012, 2, 29), complexDate.NextDate(new DateTime(2011, 3, 1)));
			Assert.AreEqual(new DateTime(2016, 2, 29), complexDate.NextDate(new DateTime(2012, 3, 1)));
		}

		[TestMethod]
		public void TestClosestDay()
		{
			var complexDate = new ClosestDayOfWeek()
			                  	{
			                  		DayOfWeek = DayOfWeek.Monday,
			                  		Inner = new FixedDate() {Day = 29, Month = 1}
			                  	};


			

			AssertDates(complexDate.NextDates(new DateTime(2012, 1, 1)),
				new DateTime(2012, 1, 30),
				new DateTime(2013, 1, 28),
				new DateTime(2014, 1, 27),
				new DateTime(2015, 1, 26),
				new DateTime(2016, 2, 1));
		}

		[TestMethod]
		public void TestClosestDayOnAndAfterDay()
		{
			var complexDate = new ClosestDayOfWeek()
			{
				DayOfWeek = DayOfWeek.Monday,
				Inner = new FixedDate() { Day = 29, Month = 1 }
			};

			Assert.AreEqual(new DateTime(2013, 1, 28), complexDate.NextDate(new DateTime(2012, 1, 30)));
			Assert.AreEqual(new DateTime(2014, 1, 27), complexDate.NextDate(new DateTime(2013, 1, 28)));
		}

		[TestMethod]
		public void TestWeekdayOnOrAfterDay()
		{
			var complexDate = new WeekdayOnOrAfter()
			{
				Inner = new FixedDate() { Day = 25, Month = 12 }
			};


			AssertDates(complexDate.NextDates(new DateTime(2012, 1, 1)),
				new DateTime(2012, 12, 25),
				new DateTime(2013, 12, 25),
				new DateTime(2014, 12, 25),
				new DateTime(2015, 12, 25),
				new DateTime(2016, 12, 26)); //rolls foward to the monday
		}

		[TestMethod]
		public void TestComplexNesting()
		{


			var complexDate = new WeekdayOnOrAfter()
			{
				Inner = new DaysAfter()
				        	{
				        		Days = 1,
								Inner = new WeekdayOnOrAfter()
									{
										Inner = new FixedDate() { Day = 25, Month = 12 }
									}
								}
				        	
			};


			AssertDates(complexDate.NextDates(new DateTime(2012, 1, 1)),
				new DateTime(2012, 12, 26),
				new DateTime(2013, 12, 26),
				new DateTime(2014, 12, 26),
				new DateTime(2015, 12, 28),
				new DateTime(2016, 12, 27)); //rolls foward to the monday
		}

		[TestMethod]
		public void TestXthDayOfWeekInMonth()
		{


			var complexDate = new XthDayOfWeekInMonth()
			                  	{
			                  		DayOccurance = 1,
									DayOfWeek = DayOfWeek.Monday,
									Month = 6
			                  	};


			AssertDates(complexDate.NextDates(new DateTime(2012, 1, 1)),
				new DateTime(2012, 6, 4),
				new DateTime(2013, 6, 3),
				new DateTime(2014, 6, 2),
				new DateTime(2015, 6, 1),
				new DateTime(2016, 6, 6));
		}



		[TestMethod]
		public void TestSerialization()
		{
			var easterMonday = new Holiday(){Name = "Easter Monday", Inner = new DaysAfter() {Days = 1, Inner = new EasterSunday()}};
			var boxingDay = new Holiday()
			{
				Name = "Boxing Day", Inner = new WeekdayOnOrAfter()
				{
					Inner = new DaysAfter()
					{
						Days = 1,
						Inner = new WeekdayOnOrAfter()
						{
							Inner = new FixedDate() { Day = 25, Month = 12 }
						}
					}

				}
			};

			var dateSet = new HolidaySet() {easterMonday, boxingDay};

			StringBuilder builder = new StringBuilder();
			XmlSerializer s = new XmlSerializer(dateSet.GetType());
			using (StringWriter writer = new StringWriter(builder))
			{
				s.Serialize(writer, dateSet);
			}
			string myXml = builder.ToString();
			
		}

		[TestMethod]
		public void TestReadFromConfigAndGetHolidays()
		{
			var holidaySet = (HolidaySet)ConfigurationManager.GetSection("HolidaySet");
			var holidays = holidaySet.GetHolidays(new DateTime(2009, 12, 31), new DateTime(2020, 12, 31));
			AssertDates(holidays, AucklandHolidays);
		}

		[TestMethod]
		public void TestReadFromConfigAndIsHoliday()
		{
			var holidaySet = (HolidaySet)ConfigurationManager.GetSection("HolidaySet");

			var startDate = AucklandHolidays.OfType<DateTime>().Min().AddDays(-1);
			var endDate = AucklandHolidays.OfType<DateTime>().Max();

			DateTime currentDate = startDate;
			while(currentDate <= endDate)
			{
				Assert.AreEqual(AucklandHolidays.Contains(currentDate), holidaySet.IsHoliday(currentDate));
				currentDate = currentDate.AddDays(1);
			}
		}

		[TestMethod]
		public void TestReadFromConfigAndIsHolidayX()
		{
			var holidaySet = (HolidaySet)ConfigurationManager.GetSection("HolidaySet");

			IEnumerable<DateTime> NextYearsHolidays = holidaySet.GetHolidays(DateTime.Now, DateTime.Now.AddYears(1));
			bool isItAHoliday = holidaySet.IsHoliday(DateTime.Now);

			var startDate = AucklandHolidays.OfType<DateTime>().Min().AddDays(-1);
			var endDate = AucklandHolidays.OfType<DateTime>().Max();

			DateTime currentDate = startDate;
			while (currentDate <= endDate)
			{
				Assert.AreEqual(AucklandHolidays.Contains(currentDate), holidaySet.IsHoliday(currentDate));
				currentDate = currentDate.AddDays(1);
			}
		}





		private void AssertDates(IEnumerable<DateTime> dates , params object[] datesToCheck)
		{
			var en = dates.GetEnumerator();
			foreach (DateTime dateToCheck in datesToCheck.OfType<DateTime>())
			{
				Assert.IsTrue(en.MoveNext());
				Assert.AreEqual(dateToCheck, en.Current);
			}
		}
	}
}
