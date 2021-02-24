using System;

namespace dotnet_tools
{
    /// <summary>
    /// Wraps up the <see cref="DateTime"/> type in order to work only with the date portion,
    /// to support casts from and to <see cref="string"/> and <see cref="DateTime"/> instances,
    /// and to provide various operators on the <see cref="Date"/> type.
    /// </summary>

    public struct Date : IComparable, IComparable<Date>, IEquatable<Date>
    {
        #region Static Properties

        public static Date MinValue => DateTime.MinValue;
        public static Date MaxValue => DateTime.MaxValue;
        public static Date Today => DateTime.Today;
        public static int CurrentYear => Today.Year;

        #endregion

        #region Parameter Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> structure to the year, month and day specified by the 
        /// <see cref="DateTime"/> <paramref name="d"/> argument.
        /// </summary>
        /// 
        /// <param name="d">The <see cref="DateTime"/> parameter providing the <c>date</c> portion.</param>
        public Date(DateTime d) => DateTimeInstance = d.Date; // DateTime is a struct, which is a ValueType, and variable values are copied.

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> structure to the specified date represented by the
        /// <paramref name="s"/> <see cref="string"/>.
        /// </summary>
        /// 
        /// <param name="s">
        /// A <see cref="string"/> that contains a date to convert.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Occures when <paramref name="s"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// Occures when <paramref name="s"/> does not contain a valid string representation of a date.
        /// </exception>
        public Date(string s)
        {
            try
            {
                DateTimeInstance = DateTime.Parse(s);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            catch (FormatException ex)
            {
                throw new FormatException(ex.Message);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> structure to the specified year, month and day.
        /// </summary>
        /// 
        /// <param name="year"> The year  (1 through 9999).                       </param>
        /// <param name="month">The month (1 through 12).                         </param>
        /// <param name="day">  The day   (1 through the number of days in month).</param>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when <paramref name="year"/> is less than 1 or greater than 9999.
        /// -or-
        /// <paramref name="month"/> is less than 1 or greater than 12.
        /// -or-
        /// <paramref name="day"/> is less than 1 or greater than the number of days in month.
        /// </exception>
        public Date(int year, int month, int day)
        {
            try
            {
                DateTimeInstance = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException(ex.Message);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets (and privately sets) the DateTime component of the date represented by this instance.
        /// </summary>
        public DateTime DateTimeInstance { get; private set; }

        /// <summary>
        /// Gets (and privately sets) the year component of the date represented by this instance.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures on set when the given <see langword="value"/> is less than 1 or greater than 9999.
        /// </exception>
        public int Year
        {
            get => DateTimeInstance.Year;
            private set
            {
                try
                {
                    DateTimeInstance = new DateTime(value, Month, Day);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new ArgumentOutOfRangeException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Gets (and privately sets) the month component of the date represented by this instance.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures on set when the given <see langword="value"/> is less than 1 or greater than 12.
        /// </exception>
        public int Month
        {
            get => DateTimeInstance.Month;
            private set
            {
                try
                {
                    DateTimeInstance = new DateTime(Year, value, Day);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new ArgumentOutOfRangeException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Gets (or privately sets) the day of the month represented by this instance.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures on set when the given <see langword="value"/> is less than 1 or greater than
        /// the number of days in month.
        /// </exception>
        public int Day
        {
            get => DateTimeInstance.Day;
            private set
            {
                try
                {
                    DateTimeInstance = new DateTime(Year, Month, value);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new ArgumentOutOfRangeException(ex.Message);
                }
            }
        }

        public DayOfWeek DayOfWeek => DateTimeInstance.DayOfWeek;

        public int DayOfYear => DateTimeInstance.DayOfYear;

        #endregion

        #region Interfaces Implementations

        /// <summary></summary>
        /// <exception cref="ArgumentException">
        /// Occures when <paramref name="obj"/> isn't of type <see cref="Date"/>.
        /// </exception>
        public int CompareTo(object obj)
        {
            // null is less anyway (obj's type doesn't matter):
            if (obj == null)
                return 1;

            if (obj is Date other)
                return CompareTo(other);

            throw new ArgumentException("The parameter obj must be of type Date.");
        }

        public int CompareTo(Date other) =>
            DateTimeInstance.CompareTo(other.DateTimeInstance);

        public bool Equals(Date other) => CompareTo(other) == 0;

        #endregion

        #region More Methods

        public static int Compare(Date d1, Date d2) => d1.CompareTo(d2);
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when The resulting <see cref="Date"/> is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>.
        /// </exception>
        public Date AddDays(in int days) => DateTimeInstance.AddDays(days);
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when The resulting <see cref="Date"/> is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>, or <paramref name="months"/> is less than
        /// -120,000 or greater than 120,000.
        /// </exception>
        public Date AddMonths(in int months) => DateTimeInstance.AddMonths(months);
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when The resulting <see cref="Date"/> is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>.
        /// </exception>
        public Date AddYears(in int years) => DateTimeInstance.AddYears(years);
        /// <summary>
        /// Returns the number of days in the specified month and year.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="month"/> is less than 1 or greater than 12. -or-
        /// <paramref name="year"/> is less than 1 or greater than 9999.
        /// </exception>
        public static int DaysInMonth(int year, int month) =>
            DateTime.DaysInMonth(year, month);
        public static bool Equals(Date a, Date b) =>
            a.Equals(b);
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when <paramref name="year"/> is less than 1 or greater than 9999.
        /// </exception>
        public static bool IsLeapYear(int year) =>
            DateTime.IsLeapYear(year);

        #endregion

        #region More Methods - Overrides

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Date other)
                return Equals(other);

            return false;
        }
        public override int GetHashCode() => DateTimeInstance.GetHashCode();
        public override string ToString() =>
            DateTimeInstance.ToShortDateString();

        #endregion

        #region Operators

        #region Increment and Decrement

        /// <summary>
        /// Adds one day to this <see cref="Date"/>.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when The resulting <see cref="Date"/> is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>.
        /// </exception>
        public static Date operator ++(Date date) => date.AddDays(1);

        /// <summary>
        /// Subtracts one day from this <see cref="Date"/>.
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when The resulting <see cref="Date"/> is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>.
        /// </exception>
        public static Date operator --(Date date) => date.AddDays(-1);

        #endregion

        #region Date Algebra

        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when The resulting <see cref="Date"/> is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>.
        /// </exception>
        public static Date operator +(Date date, int days) => date.AddDays(days);
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when The resulting <see cref="Date"/> is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>.
        /// </exception>
        public static Date operator +(int days, Date date) => date.AddDays(days);
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when The resulting <see cref="Date"/> is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>.
        /// </exception>
        public static Date operator -(Date date, int days) => date.AddDays(-days);
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when The resulting <see cref="Date"/> is less than <see cref="MinValue"/>
        /// or greater than <see cref="MaxValue"/>.
        /// </exception>
        public static Date operator -(int days, Date date) => date.AddDays(-days);

        /// <summary>
        /// Subtracts a specified date from another specified date and returns a time interval, in days.
        /// </summary>
        /// <param name="lhs">The date value to subtract from (the minuend).</param>
        /// <param name="rhs">The date value to subtract (the subtrahend).</param>
        /// <returns>The time difference between the dates, in number of days.</returns>
        public static int operator -(Date lhs, Date rhs) =>
            (int)(lhs.DateTimeInstance - rhs.DateTimeInstance).TotalDays;

        #endregion

        #region Comparison

        public static bool operator <(Date a, Date b) => a.CompareTo(b) < 0;
        public static bool operator >(Date a, Date b) => a.CompareTo(b) > 0;
        public static bool operator <=(Date a, Date b) => a.CompareTo(b) <= 0;
        public static bool operator >=(Date a, Date b) => a.CompareTo(b) >= 0;
        public static bool operator ==(Date a, Date b) => a.CompareTo(b) == 0;
        public static bool operator !=(Date a, Date b) => a.CompareTo(b) != 0;

        #endregion

        #endregion

        #region Casts

        /// <summary>
        /// From <see cref="DateTime"/> to <see cref="Date"/>.
        /// <para> Example: </para>
        /// <c><see cref="Date"/> date = <see langword="new"/> <see cref="DateTime"/>();</c>
        /// </summary>
        public static implicit operator Date(DateTime d) => new Date(d);

        /// <summary>
        /// From <see cref="Date"/> to <see cref="DateTime"/>.
        /// <para> Example: </para>
        /// <c><see cref="DateTime"/> d = <see langword="new"/> <see cref="Date"/>();</c>
        /// </summary>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occures when <paramref name="d"/>'s year is less than 1 or greater than 9999.
        /// -or-
        /// <paramref name="d"/>'s month is less than 1 or greater than 12.
        /// -or-
        /// <paramref name="d"/>'s day is less than 1 or greater than the number of days in month.
        /// </exception>
        public static implicit operator DateTime(Date d) => new DateTime(d.Year, d.Month, d.Day);

        /// <summary>
        /// From <see cref="string"/> to <see cref="Date"/>.
        /// <para> Example: </para>
        /// <c><see cref="Date"/> date = "1.1.2000";</c>
        /// </summary>
        /// 
        /// <param name="s">
        /// A <see cref="string"/> that contains a date to convert.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Occures when <paramref name="s"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// Occures when <paramref name="s"/> does not contain a valid string representation of a date.
        /// </exception>
        public static implicit operator Date(string s)
        {
            try
            {
                return new Date(s);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            catch (FormatException ex)
            {
                throw new FormatException(ex.Message);
            }
        }

        /// <summary>
        /// From <see cref="Date"/> to <see cref="string"/>.
        /// <para> Example: </para>
        /// <c><see cref="string"/> s = <see langword="new"/> <see cref="Date()"/>;</c>
        /// </summary>
        public static implicit operator string(Date d) => d.ToString();

        #endregion
    }
}
