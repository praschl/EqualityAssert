using System;
using System.Collections.Generic;
using System.Reflection;
using MiP.EqualityAssertion.Helpers;

// ReSharper disable UnusedParameter.Local

namespace MiP.EqualityAssertion
{
    /// <summary>
    /// Can be used to assert the correctness of equality members or an equality comparer.
    /// </summary>
    public class EqualityAssert
    {
        /// <summary>
        ///     Asserts that all equality members are implemented correctly.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="first">First instance of the {T}.</param>
        /// <param name="second">
        ///     Second instance of {T}, which should be value-equal to the <paramref name="first" />, but not the
        ///     same instance.
        /// </param>
        /// <param name="third">
        ///     Third instance of {T}, which should NOT be value-equal, to neither <paramref name="first" />, nor
        ///     <paramref name="second" />.
        /// </param>
        public static void EqualityMembers<T>(T first, T second, T third)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));

            if (ReferenceEquals(first, second))
                throw new ArgumentException("first and second argument should be equal, but not the same.");
            if (ReferenceEquals(first, third))
                throw new ArgumentException("first and third argument should be different.");
            if (ReferenceEquals(second, third))
                throw new ArgumentException("second and third argument should be different.");

            try
            {
                EqualsObjectAsserter.AssertEqualsObject(first, second, third);
                EqualsObjectAsserter.AssertGetHashCode(first, second, third);

                GenericEqualsAsserter.AssertGenericEquals(first as IEquatable<T>, second as IEquatable<T>,
                    third as IEquatable<T>);

                EqualityOperatorAsserter.AssertOperatorEquality(first, second, third);
                EqualityOperatorAsserter.AssertOperatorInequality(first, second, third);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        ///     Asserts that the equality comparer is implemented correctly.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="comparer">An instance of the comparer to test.</param>
        /// <param name="first">First instance of the {T}.</param>
        /// <param name="second">
        ///     Second instance of {T}, which should be value-equal to the <paramref name="first" />, but not the
        ///     same instance.
        /// </param>
        /// <param name="third">
        ///     Third instance of {T}, which should NOT be value-equal, to neither <paramref name="first" />, nor
        ///     <paramref name="second" />.
        /// </param>
        public static void EqualityComparer<T>(IEqualityComparer<T> comparer, T first, T second, T third)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));

            EqualityComparerAsserter.AssertEqualityComparerEquals(comparer, first, second, third);
            EqualityComparerAsserter.AssertComparerGetHashCode(comparer, first, second, third);
        }
    }
}