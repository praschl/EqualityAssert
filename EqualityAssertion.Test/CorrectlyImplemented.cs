using System;
using System.Collections.Generic;

namespace MiP.EqualityAssertion.Test
{
    public class CorrectlyImplemented : IEquatable<CorrectlyImplemented>
    {
        private static readonly IEqualityComparer<CorrectlyImplemented> _valueComparerInstance =
            new ValueEqualityComparer();

        private readonly int _value;

        public CorrectlyImplemented(int value)
        {
            _value = value;
        }

        public static IEqualityComparer<CorrectlyImplemented> ValueComparer
        {
            get { return _valueComparerInstance; }
        }

        public bool Equals(CorrectlyImplemented other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((CorrectlyImplemented) obj);
        }

        public override int GetHashCode()
        {
            return _value;
        }

        public static bool operator ==(CorrectlyImplemented left, CorrectlyImplemented right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CorrectlyImplemented left, CorrectlyImplemented right)
        {
            return !Equals(left, right);
        }

        private sealed class ValueEqualityComparer : IEqualityComparer<CorrectlyImplemented>
        {
            public bool Equals(CorrectlyImplemented x, CorrectlyImplemented y)
            {
                if (ReferenceEquals(x, y))
                    return true;
                if (ReferenceEquals(x, null))
                    return false;
                if (ReferenceEquals(y, null))
                    return false;
                if (x.GetType() != y.GetType())
                    return false;
                return x._value == y._value;
            }

            public int GetHashCode(CorrectlyImplemented obj)
            {
                return obj._value;
            }
        }
    }
}