using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiP.EqualityAssertion.Helpers
{
    internal class GenericEqualsAsserter
    {
        public static void AssertGenericEquals<T>(IEquatable<T> first, IEquatable<T> second, IEquatable<T> third)
        {
            if (first == null || second == null || third == null)
                return; // if not implemented, no check required.

            // ReSharper disable once EqualExpressionComparison
            if (!first.Equals((T) first))
                throw new AssertFailedException("Expected first.Equals<T>(T first) to return true.");

            if (!first.Equals((T) second))
                throw new AssertFailedException("Expected first.Equals<T>(T second) to return true.");

            if (first.Equals((T) third))
                throw new AssertFailedException("Expected first.Equals<T>(T third) to return false.");

            if (second.Equals((T) third))
                throw new AssertFailedException("Expected second.Equals<T>(T third) to return false.");

            if (first.Equals(default(T)))
                throw new AssertFailedException("Expected first.Equals(default(T)) to return false.");
        }
    }
}