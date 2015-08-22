using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiP.EqualityAssertion.Helpers
{
    internal class EqualityComparerAsserter
    {
        public static void AssertEqualityComparerEquals<T>(IEqualityComparer<T> comparer, T first, T second, T third)
        {
            if (!comparer.Equals(first, first))
                throw new AssertFailedException("Expected comparer.Equals(T first, T first) to return true.");

            if (!comparer.Equals(first, second))
                throw new AssertFailedException("Expected comparer.Equals(T first, T second) to return true.");

            if (comparer.Equals(first, third))
                throw new AssertFailedException("Expected comparer.Equals(T first, T third) to return false.");

            if (comparer.Equals(second, third))
                throw new AssertFailedException("Expected comparer.Equals(T second, T third) to return false.");

            if (comparer.Equals(first, default(T)))
                throw new AssertFailedException("Expected comparer.Equals(T first, T default(T)) to return false.");

            if (comparer.Equals(default(T), second))
                throw new AssertFailedException("Expected comparer.Equals(default(T), T second) to return false.");
        }

        public static void AssertComparerGetHashCode<T>(IEqualityComparer<T> comparer, T first, T second, T third)
        {
            if (comparer.GetHashCode(first) != comparer.GetHashCode(second))
                throw new AssertFailedException(
                    "Expected comparer.GetHashCode(first) to be equal to comparer.GetHashCode(second).");

            if (comparer.GetHashCode(first) == comparer.GetHashCode(third))
                throw new AssertFailedException(
                    "Expected comparer.GetHashCode(first) to be not equal to comparer.GetHashCode(third).");

            if (comparer.GetHashCode(second) == comparer.GetHashCode(third))
                throw new AssertFailedException(
                    "Expected comparer.GetHashCode(second) to be not equal to comparer.GetHashCode(third).");

            if (comparer.GetHashCode(first) == 0)
                throw new AssertFailedException("Expected comparer.GetHashCode(first) not to be equal to zero (0).");
        }
    }
}