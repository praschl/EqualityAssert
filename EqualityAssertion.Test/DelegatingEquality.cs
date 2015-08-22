using System;

namespace MiP.EqualityAssertion.Test
{
    public class DelegatingEquality : IEquatable<DelegatingEquality>
    {
        private readonly Func<bool> _comparisonFunc;

        private DelegatingEquality()
        {
        }

        public DelegatingEquality(Func<bool> comparisonFunc)
        {
            _comparisonFunc = comparisonFunc;
        }

        public static DelegatingEquality Empty
        {
            get { return new DelegatingEquality(); }
        }

        public bool Equals(DelegatingEquality other)
        {
            return Equals((object) other);
        }

        public override bool Equals(object obj)
        {
            return _comparisonFunc();
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static bool operator ==(DelegatingEquality first, DelegatingEquality second)
        {
            // ReSharper disable once PossibleNullReferenceException
            return first.Equals(second);
        }

        public static bool operator !=(DelegatingEquality first, DelegatingEquality second)
        {
            // ReSharper disable once PossibleNullReferenceException
            return !first.Equals(second);
        }
    }
}