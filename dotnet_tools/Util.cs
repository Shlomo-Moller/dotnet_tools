using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace dotnet_tools
{
    /// <summary>
    /// A collection of useful methods.
    /// </summary>

    public static class Util
    {
        #region Enum Extensions

        /// <summary>
        /// Returns the number of the constants in a specified enumeration, including repetitions of the same values if any exist.
        /// </summary>
        public static int Count<TEnum>() where TEnum : Enum => Values<TEnum>().Length;
        
        /// <summary>
        /// Returns the number of the constants in a specified enumeration, including repetitions of the same values if any exist.
        /// </summary>
        public static int Count<TEnum>(this TEnum @enum) where TEnum : Enum => Count<TEnum>();
        
        /// <summary>
        /// Returns the values of the constants in a specified enumeration, including repetitions of the same values if any exist.
        /// </summary>
        public static Array Values<TEnum>() where TEnum : Enum => Enum.GetValues(typeof(TEnum));
        
        /// <summary>
        /// Returns the values of the constants in a specified enumeration, including repetitions of the same values if any exist.
        /// </summary>
        public static Array Values<TEnum>(this TEnum @enum) where TEnum : Enum => Values<TEnum>();
        
        /// <summary>
        /// Returns the names of the constants in a specified enumeration.
        /// </summary>
        public static string[] Names<TEnum>() where TEnum : Enum => Enum.GetNames(typeof(TEnum));
        
        /// <summary>
        /// Returns the names of the constants in a specified enumeration.
        /// </summary>
        public static string[] Names<TEnum>(this TEnum @enum) where TEnum : Enum => Names<TEnum>();

        /// <summary>
        /// Returns the number of the constants in a specified enumeration, including repetitions of the same values if any exist.
        /// </summary>
        public static int Count(this Enum @enum) => @enum.Values().Length;
        
        /// <summary>
        /// Returns the values of the constants in a specified enumeration, including repetitions of the same values if any exist.
        /// </summary>
        public static Array Values(this Enum @enum) => @enum.GetType().GetEnumValues();
        
        /// <summary>
        /// Returns the names of the constants in a specified enumeration.
        /// </summary>
        public static string[] Names(this Enum @enum) => @enum.GetType().GetEnumNames();

        #endregion

        /// <summary>
        /// Checks whether <paramref name="obj"/> is in the closed (including bounds) range [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// 
        /// <typeparam name="T">
        /// The type of this object and the arguments.
        /// </typeparam>
        /// 
        /// <param name="obj">The instance to be checked.</param>
        /// <param name="min">The range' lower bound.</param>
        /// <param name="max">The range' upper bound. Must be <b>greater than or equal to</b> <paramref name="min"/>.</param>
        /// 
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/>'s value is in the range as specified; otherwise, <see langword="false"/>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.
        /// </exception>

        public static bool InRange<T>(this T obj, in T min, in T max) where T : IComparable
        {
            if (min.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException("min should not be greater than max");

            return obj.CompareTo(min) >= 0 && obj.CompareTo(max) <= 0;
        }

        /// <summary>
        /// Checks whether <paramref name="obj"/> is in the range [<paramref name="min"/>, <paramref name="max"/>);
        /// that is: including <paramref name="min"/> and <b>not</b> including <paramref name="max"/>.
        /// </summary>
        /// 
        /// <typeparam name="T">
        /// The type of this object and the arguments.
        /// </typeparam>
        /// 
        /// <param name="obj">The instance to be checked.</param>
        /// <param name="min">The range' lower (inclusive) bound.</param>
        /// <param name="max">The range' upper (exclusive) bound. Must be <b>greater than</b> <paramref name="min"/>.</param>
        /// 
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/>'s value is in the range as specified; otherwise, <see langword="false"/>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="min"/> is equal or greater than <paramref name="max"/>.
        /// </exception>

        public static bool InOpenRange<T>(this T obj, in T min, in T max) where T : IComparable
        {
            if (min.CompareTo(max) >= 0)
                throw new ArgumentOutOfRangeException("min should not be equal to or greater than max");

            return obj.CompareTo(min) >= 0 && obj.CompareTo(max) < 0;
        }

        /// <summary>
        /// Returns a <see cref="string"/> which describes a list of <c>name/value</c> pairs, of this instance's non-indexed properties.
        /// </summary>
        /// 
        /// <typeparam name="T">The calling instance's type.</typeparam>
        /// 
        /// <param name="t">The calling instance.</param>
        /// <param name="separator">Optional: A <see cref="string"/> that will separate between the properties' <see cref="string"/> representations. The default is: <c>new line</c>.</param>
        /// <param name="except">Optional: A list of specific properties names, to <b>not</b> include in the returned <see cref="string"/>.</param>
        /// 
        /// <returns>
        /// A <see cref="string"/> representation of this instance's non-indexed properties.
        /// </returns>

        public static string ToStringProperties<T>(this T t, in string separator = "\n", params string[] except)
        {
            string str = "";

            if (except.Length == 0)
            {
                foreach (PropertyInfo item in t.GetType().GetProperties())
                    if (!item.IsIndexedProperty())
                        str += item.Name + ": " + item.GetValue(t, null) + separator;
            }
            else
            {
                foreach (PropertyInfo item in t.GetType().GetProperties())
                    if (!item.IsIndexedProperty() && !Array.Exists(except, s => s == item.Name))
                        str += item.Name + ": " + item.GetValue(t, null) + separator;
            }

            // Cut off the last separation:
            int len = str.Length - separator.Length;

            return str.Substring(0, len);
        }

        /// <summary>
        /// Returns a value indicating whether this <see cref="PropertyInfo"/> is an indexed property.
        /// </summary>
        /// 
        /// <returns>
        /// <see langword="true"/> if this property results an empty <see cref="ParameterInfo"/> array; otherwise, <see langword="false"/>.
        /// </returns>

        public static bool IsIndexedProperty(this PropertyInfo prop)
        {
            return prop.GetIndexParameters().Length > 0;
        }

        //public static char[] WhiteSpaces = new char[] { ' ', '\n', '\t', '\r', '\f', '\v' };

        #region Random Extensions

        /// <summary>
        /// Returns a random <see cref="Date"/> between <paramref name="min"/> (including)
        /// and <paramref name="max"/> (not including).
        /// </summary>
        /// 
        /// <param name="min">
        /// The lower (inclusive) <see cref="Date"/> bound.
        /// </param>
        /// <param name="max">
        /// The upper (exclusive) <see cref="Date"/> bound. Must be greater than <paramref name="min"/>
        /// </param>
        /// 
        /// <remarks>
        /// If <paramref name="max"/> isn't greater than <paramref name="min"/>, then
        /// <paramref name="min"/> is returned.
        /// </remarks>

        public static Date NextDate(this Random rand, in Date min, in Date max)
        {
            if (max <= min)
                return min;

            // Date rang to numeric range:
            // min = '0', and max is the number of days to add:
            int diff = max - min;

            return min + rand.Next(diff);
        }

        /// <summary>
        /// Returns a random <see cref="bool"/> value.
        /// </summary>

        public static bool NextBoolean(this Random rand) =>
            rand.NextDouble() >= 0.5;

        /// <summary>
        /// Returns a random constant of the specified enumeration of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration (<see cref="Enum"/>).</typeparam>
        /// <param name="rand"></param>
        /// <returns></returns>

        public static T NextConstant<T>(this Random rand) where T : Enum
        {
            T result = default;

            var values = Values<T>();

            int x = rand.Next(values.Length + 1);

            int i = -1;
            foreach (var item in values)
            {
                if (x == (++i))
                {
                    result = (T)item;
                    break;
                }
            }

            return result;
        }

        #endregion

        public static Func<T, bool> ToFunc<T>(this Predicate<T> pred) =>
            new Func<T, bool>(pred);

        /// <summary>
        /// Removes the first occurance of an elements that matches the conditions defined by the specified predicate.
        /// </summary>
        /// 
        /// <typeparam name="T">
        /// The list' elements type. Also defines the <see cref="Predicate{T}"/> argument.
        /// </typeparam>
        /// 
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements to remove.
        /// </param>
        /// 
        /// <returns>
        /// <see langword="true"/> if found and removed. Otherwise <see langword="false"/>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Occures when <paramref name="list"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Occures when <paramref name="list"/> is an empty <see cref="List{T}"/>.
        /// </exception>

        public static bool RemoveFirst<T>(this List<T> list, Predicate<T> match = null)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (list.Count == 0)
                throw new ArgumentException("The List<T> argument is an empty list.", "list");

            if (match == null)
            {
                list.RemoveAt(0);
                return true;
            }

            int index = list.FindIndex(match);

            if (index == -1)
                return false;

            list.RemoveAt(index);

            return true;
        }

        #region PropertyInfo Extensions

        public static PropertyInfo[] GetProperties(this Type type, params string[] names)
        {
            if (names == null || names.Length == 0)
                return type.GetProperties();

            List<PropertyInfo> properties = new List<PropertyInfo>();

            foreach (var name in names)
                properties.Add(type.GetProperty(name));

            return properties.ToArray();
        }

        public static PropertyInfo GetProperty<T>(this T obj, string name)
            => obj.GetType().GetProperty(name);

        public static PropertyInfo[] GetProperties<T>(this T obj, params string[] names)
            => obj.GetType().GetProperties(names);

        #endregion

        public static Date Min(Date a, Date b)
        {
            return
                b < a ?
                b :
                a;
        }

        /// <summary>
        ///     Indicates whether <paramref name="collection"/> isn't <see langword="null"/>
        ///     and is empty.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of <paramref name="collection"/>'s elements.
        /// </typeparam>
        /// <param name="collection">
        ///     The collection to check.
        /// </param>
        public static bool IsEmptyCollection<T>(IEnumerable<T> collection) =>
            collection != null && collection.Count() == 0;

        /// <summary>
        ///     Indicates whether if there is any overlap between the two specified periods.
        /// </summary>
        /// <remarks>
        ///     <see cref="ArgumentOutOfRangeException"/> if the dates are out of order (start >= end).
        /// </remarks>
        public static bool Overlaps(this (Date start, Date end) period, (Date start, Date end) other)
        {
            if (period.start >= period.end)
                throw new ArgumentOutOfRangeException("period");

            if (other.start >= other.end)
                throw new ArgumentOutOfRangeException("other");

            if (period.end <= other.start || other.end <= period.start)
                return false;

            return true;
        }
    }
}
