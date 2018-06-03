using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	/// <summary>
	/// An overly simplified DateTime and TimeSpan
	/// (Ignores time zones, leap years, DST, calendars,  etc.)
	/// </summary>
	public class SDateTime
	{
		//Constants
		public const int MinuteToSeconds = 60;
		public const int HourToMinutes = 60;
		public const int DayToHours = 24;
		//TODO: Months?
		public const int YearToDays = 356;

		/// <summary>
		/// 1 tick = 1 second
		/// </summary>
		[Serialize]
		public long Ticks { get; protected set; }

		[NoSerialize]
		public int Seconds
		{
			get => (int)(Ticks % MinuteToSeconds);
		}

		[NoSerialize]
		public int Minutes
		{
			get => (int)((Ticks / MinuteToSeconds) % HourToMinutes);
		}

		[NoSerialize]
		public int Hours
		{
			get => (int)((Ticks / MinuteToSeconds / HourToMinutes) % DayToHours);
		}

		[NoSerialize]
		public int Days
		{
			get => (int)((Ticks / MinuteToSeconds / HourToMinutes / DayToHours) % YearToDays);
		}

		//TODO: 5 Billion BC? (int vs long)
		[NoSerialize]
		public int Years
		{
			get => (int)(Ticks / MinuteToSeconds / HourToMinutes / DayToHours / YearToDays);
		}

		public SDateTime()
		{

		}

		public SDateTime(int years)
		{
			Ticks = ((long)years) * YearToDays * DayToHours * HourToMinutes * MinuteToSeconds;
		}

		public SDateTime(int years, int days)
		{
			Ticks = (((long)years) * YearToDays + days) * DayToHours * HourToMinutes * MinuteToSeconds;
		}

		public SDateTime(int years, int days, int hours)
		{
			Ticks = ((((long)years) * YearToDays + days) * DayToHours + hours) * HourToMinutes * MinuteToSeconds;
		}


		public SDateTime(int years, int days, int hours, int minutes)
		{
			Ticks = (((((long)years) * YearToDays + days) * DayToHours + hours) * HourToMinutes + minutes) * MinuteToSeconds;
		}

		public SDateTime(int years, int days, int hours, int minutes, int seconds)
		{
			Ticks = (((((long)years) * YearToDays + days) * DayToHours + hours) * HourToMinutes + minutes) * MinuteToSeconds + seconds;
		}

		public static SDateTime FromTicks(long ticks)
		{
			return new SDateTime()
			{
				Ticks = ticks
			};
		}

		public void Decompose(out int years, out int days, out int hours, out int minutes, out int seconds)
		{
			long t = this.Ticks;
			seconds = (int)(t % MinuteToSeconds);
			t /= MinuteToSeconds;
			minutes = (int)(t % HourToMinutes);
			t /= HourToMinutes;
			hours = (int)(t % DayToHours);
			t /= DayToHours;
			days = (int)(t % YearToDays);
			t /= YearToDays;
			years = (int)t; //TODO: 5 billion BC?
		}

		/// <summary>
		/// Checks if two SDateTimes are mathematically equal 
		/// (compares the ticks)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (!(obj is SDateTime other))
			{
				return false;
			}
			return other.Ticks == this.Ticks;
		}

		public override int GetHashCode()
		{
			return this.Ticks.GetHashCode();
		}

		public override string ToString()
		{
			Decompose(out int years, out int days, out int hours, out int minutes, out int seconds);
			//TODO: Months
			return $"{years}-mm-{days} {hours}:{minutes}:{seconds}";
		}

		public static SDateTime operator +(SDateTime a, SDateTime b)
		{
			return FromTicks(a.Ticks + b.Ticks);
		}

		public static SDateTime operator -(SDateTime a, SDateTime b)
		{
			return FromTicks(a.Ticks - b.Ticks);
		}

		public static bool operator ==(SDateTime a, SDateTime b)
		{
			return a.Ticks == b.Ticks;
		}

		public static bool operator !=(SDateTime a, SDateTime b)
		{
			return a.Ticks != b.Ticks;
		}

		public static bool operator <(SDateTime a, SDateTime b)
		{
			return a.Ticks < b.Ticks;
		}

		public static bool operator >(SDateTime a, SDateTime b)
		{
			return a.Ticks > b.Ticks;
		}

		public static bool operator <=(SDateTime a, SDateTime b)
		{
			return a.Ticks <= b.Ticks;
		}

		public static bool operator >=(SDateTime a, SDateTime b)
		{
			return a.Ticks >= b.Ticks;
		}
	}
}
