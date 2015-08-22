using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiP.EqualityAssertion.Helpers
{
    internal class EqualsObjectAsserter
    {
        public static void AssertEqualsObject(object first, object second, object third)
        {
            // ReSharper disable once EqualExpressionComparison
            if (!first.Equals(first))
                throw new AssertFailedException("Expected first.Equals((object) first) to return true.");

            if (!first.Equals(second))
                throw new AssertFailedException("Expected first.Equals((object) second) to return true.");

            if (first.Equals(third))
                throw new AssertFailedException("Expected first.Equals((object) third) to return false.");

            if (second.Equals(third))
                throw new AssertFailedException("Expected second.Equals((object) third) to return false.");

            if (first.Equals(null))
                throw new AssertFailedException("Expected first.Equals((object) null) to return false.");

            if (first.Equals(new object()))
                throw new AssertFailedException("Expected first.Equals(new object()) to return false.");
        }

        public static void AssertGetHashCode<T>(T first, T second, T third)
        {
            if (first.GetHashCode() != second.GetHashCode())
                throw new AssertFailedException("Expected first.GetHashCode() to be equal to second.GetHashCode().");

            if (first.GetHashCode() == third.GetHashCode())
                throw new AssertFailedException("Expected first.GetHashCode() to be not equal to third.GetHashCode().");

            if (second.GetHashCode() == third.GetHashCode())
                throw new AssertFailedException("Expected second.GetHashCode() to be not equal to third.GetHashCode().");

            if (first.GetHashCode() == 0)
                throw new AssertFailedException("Expected first.GetHashCode() not to be equal to zero (0).");
        }
    }
}