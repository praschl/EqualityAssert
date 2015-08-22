using System;
using System.Collections.Generic;
using System.Linq;

namespace MiP.EqualityAssertion.Test
{
    public class FakedEquality : IEquatable<FakedEquality>
    {
        private static readonly IEqualityComparer<FakedEquality> _fakedEqualityComparer = new FakedEqualityComparer();
        private readonly List<bool> _equalsResults;
        private readonly List<int> _hashResults;

        private FakedEquality()
        {
        }

        public FakedEquality(params bool[] equalsResults)
        {
            _equalsResults = equalsResults.ToList();
        }

        public FakedEquality(params int[] hashResults)
        {
            _hashResults = hashResults.ToList();
        }

        public static FakedEquality Empty
        {
            get { return new FakedEquality(); }
        }

        public static IEqualityComparer<FakedEquality> Comparer
        {
            get { return _fakedEqualityComparer; }
        }

        public bool Equals(FakedEquality other)
        {
            return Equals((object) other);
        }

        public override bool Equals(object obj)
        {
            var result = _equalsResults.First();

            _equalsResults.RemoveAt(0);
            _equalsResults.Add(result);

            return result;
        }

        public static bool operator ==(FakedEquality first, FakedEquality second)
        {
            // ReSharper disable once PossibleNullReferenceException
            return first.Equals(second);
        }

        public static bool operator !=(FakedEquality first, FakedEquality second)
        {
            // ReSharper disable once PossibleNullReferenceException
            return !first.Equals(second);
        }

        public override int GetHashCode()
        {
            var result = _hashResults.First();

            _hashResults.RemoveAt(0);
            _hashResults.Add(result);

            return result;
        }

        private class FakedEqualityComparer : IEqualityComparer<FakedEquality>
        {
            public bool Equals(FakedEquality x, FakedEquality y)
            {
                if (!ReferenceEquals(x, null))
                    return x.Equals(y);

                return y.Equals(x);
            }

            public int GetHashCode(FakedEquality obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}