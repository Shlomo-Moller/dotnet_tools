using System;

namespace dotnet_tools
{
    /// <summary>
    /// Defines an ID-type which specifically affiliated
    /// to a specific given <typeparamref name="T"/> type.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The given type as the only appropriate user of this ID-type.
    /// </typeparam>
    /// 
    /// <remarks>
    /// The type-parameter <typeparamref name="T"/> is only used to define this <see cref="DedicatedID{T}"/>
    /// struct-type as an ID type specifically compatible with <typeparamref name="T"/> type instances.
    /// </remarks>

    public struct DedicatedID<T> : IEquatable<DedicatedID<T>>, IComparable, IComparable<DedicatedID<T>>
    {
        private int UnderlyingID { get; set; }

        public bool Equals(DedicatedID<T> other)
            => UnderlyingID.Equals(other.UnderlyingID);

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is DedicatedID<T> other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode() =>
            UnderlyingID.GetHashCode();

        public static bool Equals(DedicatedID<T> x, DedicatedID<T> y)
            => x.Equals(y);

        public static bool operator ==(DedicatedID<T> x, DedicatedID<T> y)
            => Equals(x, y);

        public static bool operator !=(DedicatedID<T> x, DedicatedID<T> y)
            => !(x == y);

        /// <summary>
        /// From <see cref="int"/> to <see cref="DedicatedID{T}"/>.
        /// <para> Example: </para>
        /// <c><see cref="DedicatedID{T}"/> id = (<see cref="DedicatedID{T}"/>)1234;</c>
        /// </summary>

        public static explicit operator DedicatedID<T>(int n) =>
            new DedicatedID<T> { UnderlyingID = n };

        /// <summary>
        /// From <see cref="DedicatedID{T}"/> to <see cref="int"/>.
        /// <para> Example: </para>
        /// <c><see cref="int"/> n = (<see cref="int"/>)(<see langword="new"/> <see cref="DedicatedID{T}()"/>);</c>
        /// </summary>

        public static explicit operator int(DedicatedID<T> id) =>
            id.UnderlyingID;

        public override string ToString() =>
            UnderlyingID.ToString();

        public int CompareTo(object obj)
        {
            // null is less anyway (obj's type doesn't matter):
            if (obj == null)
                return 1;

            if (obj is DedicatedID<T> other)
                return CompareTo(other);

            throw new ArgumentException("The parameter obj must be of type DedicatedID<T>.");
        }
        public int CompareTo(DedicatedID<T> other) =>
            UnderlyingID.CompareTo(other.UnderlyingID);

        public static bool operator <(DedicatedID<T> a, DedicatedID<T> b) => a.CompareTo(b) < 0;
        public static bool operator >(DedicatedID<T> a, DedicatedID<T> b) => a.CompareTo(b) > 0;
        public static bool operator <=(DedicatedID<T> a, DedicatedID<T> b) => a.CompareTo(b) <= 0;
        public static bool operator >=(DedicatedID<T> a, DedicatedID<T> b) => a.CompareTo(b) >= 0;

        public DedicatedID(int underlyingID = 0)
        {
            UnderlyingID = underlyingID;
        }
    }
}
