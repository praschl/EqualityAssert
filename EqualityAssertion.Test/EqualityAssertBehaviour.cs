using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiP.EqualityAssertion.Helpers;

namespace MiP.EqualityAssertion.Test
{
    [TestClass]
    public class EqualityAssertBehaviour
    {
        // Code Coverage

        [TestMethod]
        public void CoverageOfEqualityMembers_Should_Be100Percent()
        {
            var first = new CorrectlyImplemented(1);
            var second = new CorrectlyImplemented(1);
            var third = new CorrectlyImplemented(3);

            EqualityAssert.EqualityMembers(first, second, third);

            // sorry, cant assert that code coverage is 100%, please check yourself.
        }

        [TestMethod]
        public void CoverageOfEqualityComparer_Should_Be100Percent()
        {
            var first = new CorrectlyImplemented(1);
            var second = new CorrectlyImplemented(1);
            var third = new CorrectlyImplemented(3);

            EqualityAssert.EqualityComparer(CorrectlyImplemented.ValueComparer, first, second, third);
            EqualityAssert.EqualityComparer(CorrectlyImplemented.ValueComparer, first, second,
                new DerivedFromCorrectlyImplemented(4));

            // sorry, cant assert that code coverage is 100%, please check yourself.
        }

        // Example usages with .NET classes

        [TestMethod]
        public void EqualityMembers_With_String()
        {
            // otherwise, both strings would be the same reference after compile.
            var string1 = new StringBuilder("string1").ToString();
            var string2 = new StringBuilder("string1").ToString();

            EqualityAssert.EqualityMembers(string1, string2, "String3");
        }

        [TestMethod]
        public void EqualityMembers_With_Int32()
        {
            EqualityAssert.EqualityMembers(1337, 1337, 4711);
        }

        [TestMethod]
        public void EqualityMembers_With_KeyValuePair()
        {
            var pair1 = new KeyValuePair<string, int>("1", 1);
            var pair2 = new KeyValuePair<string, int>("1", 1);
            var pair3 = new KeyValuePair<string, int>("2", 2);

            EqualityAssert.EqualityMembers(pair1, pair2, pair3);
        }

        [TestMethod]
        public void EqualityComparer_With_StringComparer_OrdinalIgnoreCase()
        {
            EqualityAssert.EqualityComparer(StringComparer.OrdinalIgnoreCase, "STRiNG1", "strIng1", "string3");
        }

        // Argument checks

        [TestMethod]
        public void EqualityMembers_ShouldThrow_ArgumentNullException_When_FirstIsNull()
        {
            Action assert = () => EqualityAssert.EqualityMembers(null, new object(), new object());

            assert.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void EqualityMembers_ShouldThrow_ArgumentNullException_When_SecondIsNull()
        {
            Action assert = () => EqualityAssert.EqualityMembers(new object(), null, new object());

            assert.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void EqualityMembers_ShouldThrow_ArgumentNullException_When_ThirdIsNull()
        {
            Action assert = () => EqualityAssert.EqualityMembers(new object(), new object(), null);

            assert.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void EqualityMembers_ShouldThrow_ArgumentException_When_FirstReferenceEqualsSecond()
        {
            var first = new object();
            var third = new object();

            Action assert = () => EqualityAssert.EqualityMembers(first, first, third);

            assert.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void EqualityMembers_ShouldThrow_ArgumentException_When_FirstReferenceEqualsThird()
        {
            var first = new object();
            var second = new object();

            Action assert = () => EqualityAssert.EqualityMembers(first, second, first);

            assert.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void EqualityMembers_ShouldThrow_ArgumentException_When_SecondReferenceEqualsThird()
        {
            var first = new object();
            var second = new object();

            Action assert = () => EqualityAssert.EqualityMembers(first, second, second);

            assert.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void EqualityComparer_ShouldThrow_ArgumentNullException_When_ComparerIsNull()
        {
            Action assert = () => EqualityAssert.EqualityComparer(null, new object(), new object(), new object());

            assert.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void EqualityComparer_ShouldThrow_ArgumentNullException_When_FirstArgumentIsNull()
        {
            Action assert =
                () =>
                    EqualityAssert.EqualityComparer(FakedEquality.Comparer, null, FakedEquality.Empty,
                        FakedEquality.Empty);

            assert.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void EqualityComparer_ShouldThrow_ArgumentNullException_When_SecondArgumentIsNull()
        {
            Action assert =
                () =>
                    EqualityAssert.EqualityComparer(FakedEquality.Comparer, FakedEquality.Empty, null,
                        FakedEquality.Empty);

            assert.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void EqualityComparer_ShouldThrow_ArgumentNullException_When_ThirdArgumentIsNull()
        {
            Action assert =
                () =>
                    EqualityAssert.EqualityComparer(FakedEquality.Comparer, FakedEquality.Empty, FakedEquality.Empty,
                        null);

            assert.ShouldThrow<ArgumentNullException>();
        }

        // TargetInvokationException

        [TestMethod]
        public void EqualityMembers_Should_ThrowInnerOfTargetInvokationException()
        {
            Func<bool> firstFunc =
                () => { throw new TargetInvocationException(new InvalidOperationException("this.is.expected")); };

            var first = new DelegatingEquality(firstFunc);

            Action assert =
                () => EqualityAssert.EqualityMembers(first, DelegatingEquality.Empty, DelegatingEquality.Empty);

            assert.ShouldThrow<InvalidOperationException>().WithMessage("this.is.expected");
        }

        // EqualsObject

        [TestMethod]
        public void EqualsObject_ShouldThrow_AssertFailed_When_FirstUnequalsFirst()
        {
            //if (!first.Equals(first))
            //    throw new AssertFailedException("Expected first.Equals((object) first) to return true.");

            var first = new FakedEquality(false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertEqualsObject(first, second, third, "Expected first.Equals((object) first) to return true.");
        }

        [TestMethod]
        public void EqualsObject_ShouldThrow_AssertFailed_When_FirstUnequalsSecond()
        {
            //if (!first.Equals(second))
            //    throw new AssertFailedException("Expected first.Equals((object) second) to return true.");

            var first = new FakedEquality(true, false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertEqualsObject(first, second, third, "Expected first.Equals((object) second) to return true.");
        }

        [TestMethod]
        public void EqualsObject_ShouldThrow_AssertFailed_When_FirstEqualsThird()
        {
            //if (first.Equals(third))
            //    throw new AssertFailedException("Expected first.Equals((object) third) to return false.");

            var first = new FakedEquality(true, true, true);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertEqualsObject(first, second, third, "Expected first.Equals((object) third) to return false.");
        }

        [TestMethod]
        public void EqualsObject_ShouldThrow_AssertFailed_When_SecondEqualsThird()
        {
            //if (second.Equals(third))
            //    throw new AssertFailedException("Expected second.Equals((object) third) to return false.");

            var first = new FakedEquality(true, true, false);
            var second = new FakedEquality(true);
            var third = FakedEquality.Empty;

            AssertEqualsObject(first, second, third, "Expected second.Equals((object) third) to return false.");
        }

        [TestMethod]
        public void EqualsObject_ShouldThrow_AssertFailed_When_FirstEqualsNull()
        {
            //if (first.Equals(null))
            //    throw new AssertFailedException("Expected first.Equals((object) null) to return false.");

            var first = new FakedEquality(true, true, false, true);
            var second = new FakedEquality(false);
            var third = FakedEquality.Empty;

            AssertEqualsObject(first, second, third, "Expected first.Equals((object) null) to return false.");
        }

        [TestMethod]
        public void EqualsObject_ShouldThrow_AssertFailed_When_FirstEqualsObject()
        {
            //if (first.Equals(new object()))
            //    throw new AssertFailedException("Expected first.Equals(new object()) to return false.");

            var first = new FakedEquality(true, true, false, false, true);
            var second = new FakedEquality(false);
            var third = FakedEquality.Empty;

            AssertEqualsObject(first, second, third, "Expected first.Equals(new object()) to return false.");
        }

        // GetHashCode

        [TestMethod]
        public void GetHashCode_ShouldThrow_AssertFailed_When_HashCodeIsZero()
        {
            //if (first.GetHashCode() == 0)
            //      throw new AssertFailedException("Expected first.GetHashCode() not to be equal to zero (0).");

            var first = new FakedEquality(1, 1, 0);
            var second = new FakedEquality(1, 1);
            var third = new FakedEquality(3);

            AssertHashCode(first, second, third, "Expected first.GetHashCode() not to be equal to zero (0).");
        }

        [TestMethod]
        public void GetHashCode_ShouldThrow_AssertFailed_When_FirstDiffersFromSecond()
        {
            //if (first.GetHashCode() != second.GetHashCode())
            //      throw new AssertFailedException("Expected first.GetHashCode() to be equal to second.GetHashCode().");

            var first = new FakedEquality(1);
            var second = new FakedEquality(2);
            var third = new FakedEquality(3);

            AssertHashCode(first, second, third, "Expected first.GetHashCode() to be equal to second.GetHashCode().");
        }

        [TestMethod]
        public void GetHashCode_ShouldThrow_AssertFailed_When_FirstEqualsThird()
        {
            //if (first.GetHashCode() == third.GetHashCode())
            //    throw new AssertFailedException("Expected first.GetHashCode() to be not equal to third.GetHashCode().");

            var first = new FakedEquality(1, 3);
            var second = new FakedEquality(1);
            var third = new FakedEquality(3);

            AssertHashCode(first, second, third, "Expected first.GetHashCode() to be not equal to third.GetHashCode().");
        }

        [TestMethod]
        public void GetHashCode_ShouldThrow_AssertFailed_When_SecondEqualsThird()
        {
            //if (second.GetHashCode() == third.GetHashCode())
            //    throw new AssertFailedException("Expected second.GetHashCode() to be not equal to third.GetHashCode().");

            var first = new FakedEquality(1);
            var second = new FakedEquality(1, 3);
            var third = new FakedEquality(3);

            AssertHashCode(first, second, third, "Expected second.GetHashCode() to be not equal to third.GetHashCode().");
        }

        // EqualsGeneric

        [TestMethod]
        public void EqualsGeneric_ShouldNotThrow_When_FirstIsNull()
        {
            //FakedEquality first = new FakedEquality(false);
            var second = new FakedEquality(true);
            var third = new FakedEquality(false);

            Action assert = () => GenericEqualsAsserter.AssertGenericEquals(null, second, third);

            assert.ShouldNotThrow();
        }

        [TestMethod]
        public void EqualsGeneric_ShouldNotThrow_When_SecondIsNull()
        {
            var first = new FakedEquality(false);
            //FakedEquality second = new FakedEquality(true);
            var third = new FakedEquality(false);

            Action assert = () => GenericEqualsAsserter.AssertGenericEquals(first, null, third);

            assert.ShouldNotThrow();
        }

        [TestMethod]
        public void EqualsGeneric_ShouldNotThrow_When_ThirdIsNull()
        {
            var first = new FakedEquality(false);
            var second = new FakedEquality(true);
            //FakedEquality third = new FakedEquality(false);

            Action assert = () => GenericEqualsAsserter.AssertGenericEquals(first, second, null);

            assert.ShouldNotThrow();
        }

        [TestMethod]
        public void EqualsGeneric_ShouldThrow_AssertFailed_When_FirstUnequalsFirst()
        {
            //if (!first.Equals((T)first))
            //    throw new AssertFailedException("Expected first.Equals<T>(T first) to return true.");

            var first = new FakedEquality(false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertEqualsGeneric(first, second, third, "Expected first.Equals<T>(T first) to return true.");
        }

        [TestMethod]
        public void EqualsGeneric_ShouldThrow_AssertFailed_When_FirstUnequalsSecond()
        {
            //if (!first.Equals((T)second))
            //    throw new AssertFailedException("Expected first.Equals<T>(T second) to return true.");

            var first = new FakedEquality(true, false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertEqualsGeneric(first, second, third, "Expected first.Equals<T>(T second) to return true.");
        }

        [TestMethod]
        public void EqualsGeneric_ShouldThrow_AssertFailed_When_FirstEqualsThird()
        {
            //if (first.Equals((T)third))
            //    throw new AssertFailedException("Expected first.Equals<T>(T third) to return false.");

            var first = new FakedEquality(true, true, true);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertEqualsGeneric(first, second, third, "Expected first.Equals<T>(T third) to return false.");
        }

        [TestMethod]
        public void EqualsGeneric_ShouldThrow_AssertFailed_When_SecondEqualsThird()
        {
            //if (second.Equals((T)third))
            //    throw new AssertFailedException("Expected second.Equals<T>(T third) to return false.");

            var first = new FakedEquality(true, true, false);
            var second = new FakedEquality(true);
            var third = FakedEquality.Empty;

            AssertEqualsGeneric(first, second, third, "Expected second.Equals<T>(T third) to return false.");
        }

        [TestMethod]
        public void EqualsGeneric_ShouldThrow_AssertFailed_When_FirstEqualsDefault()
        {
            //if (first.Equals(default(T)))
            //    throw new AssertFailedException("Expected first.Equals(default(T)) to return false.");

            var first = new FakedEquality(true, true, false, true);
            var second = new FakedEquality(false);
            var third = FakedEquality.Empty;

            AssertEqualsGeneric(first, second, third, "Expected first.Equals(default(T)) to return false.");
        }

        // Operators

        [TestMethod]
        public void OperatorEquals_ShouldNotThrow_When_OperatorNotImplemented()
        {
            Action assert =
                () => EqualityOperatorAsserter.AssertOperatorEquality(new object(), new object(), new object());

            assert.ShouldNotThrow();
        }

        [TestMethod]
        public void OperatorEquals_ShouldThrow_AssertFailed_When_FirstUnequalsFirst()
        {
            //if (!GetBoolResult(op_equality, first, first))
            //    throw new AssertFailedException("Expected (first == first) to return true.");

            var first = new FakedEquality(false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertEqualsOperator(first, second, third, "Expected (first == first) to return true.");
        }

        [TestMethod]
        public void OperatorEquals_ShouldThrow_AssertFailed_When_FirstUnequalsSecond()
        {
            //if (!GetBoolResult(op_equality, first, second))
            //    throw new AssertFailedException("Expected (first == second) to return true.");

            var first = new FakedEquality(true, false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertEqualsOperator(first, second, third, "Expected (first == second) to return true.");
        }

        [TestMethod]
        public void OperatorEquals_ShouldThrow_AssertFailed_When_FirstEqualsThird()
        {
            //if (GetBoolResult(op_equality, first, third))
            //    throw new AssertFailedException("Expected (first == third) to return false.");

            var first = new FakedEquality(true, true, true);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertEqualsOperator(first, second, third, "Expected (first == third) to return false.");
        }

        [TestMethod]
        public void OperatorEquals_ShouldThrow_AssertFailed_When_SecondEqualsThird()
        {
            //if (GetBoolResult(op_equality, second, third))
            //    throw new AssertFailedException("Expected (second == third) to return false.");

            var first = new FakedEquality(true, true, false);
            var second = new FakedEquality(true);
            var third = FakedEquality.Empty;

            AssertEqualsOperator(first, second, third, "Expected (second == third) to return false.");
        }

        [TestMethod]
        public void OperatorEquals_ShouldThrow_AssertFailed_When_FirstEqualsDefault()
        {
            //if (GetBoolResult(op_equality, first, default(T)))
            //    throw new AssertFailedException("Expected (first == null) to return false.");

            var first = new FakedEquality(true, true, false, true);
            var second = new FakedEquality(false);
            var third = FakedEquality.Empty;

            AssertEqualsOperator(first, second, third, "Expected (first == null) to return false.");
        }

        [TestMethod]
        public void OperatorUnequals_ShouldNotThrow_When_OperatorNotImplemented()
        {
            Action assert =
                () => EqualityOperatorAsserter.AssertOperatorInequality(new object(), new object(), new object());

            assert.ShouldNotThrow();
        }

        [TestMethod]
        public void OperatorUnequals_ShouldThrow_AssertFailed_When_FirstUnequalsFirst()
        {
            //if (!GetBoolResult(op_inequality, first, first))
            //    throw new AssertFailedException("Expected (first != first) to return false.");

            var first = new FakedEquality(false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertUnqualsOperator(first, second, third, "Expected (first != first) to return false.");
        }

        [TestMethod]
        public void OperatorUnequals_ShouldThrow_AssertFailed_When_FirstUnequalsSecond()
        {
            //if (GetBoolResult(op_inequality, first, second))
            //    throw new AssertFailedException("Expected (first != second) to return false.");

            var first = new FakedEquality(true, false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertUnqualsOperator(first, second, third, "Expected (first != second) to return false.");
        }

        [TestMethod]
        public void OperatorUnequals_ShouldThrow_AssertFailed_When_FirstEqualsThird()
        {
            //if (!GetBoolResult(op_inequality, first, third))
            //    throw new AssertFailedException("Expected (first != third) to return true.");

            var first = new FakedEquality(true, true, true);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertUnqualsOperator(first, second, third, "Expected (first != third) to return true.");
        }

        [TestMethod]
        public void OperatorUnequals_ShouldThrow_AssertFailed_When_SecondEqualsThird()
        {
            //if (!GetBoolResult(op_inequality, second, third))
            //    throw new AssertFailedException("Expected (second != third) to return true.");

            var first = new FakedEquality(true, true, false);
            var second = new FakedEquality(true);
            var third = FakedEquality.Empty;

            AssertUnqualsOperator(first, second, third, "Expected (second != third) to return true.");
        }

        [TestMethod]
        public void OperatorUnequals_ShouldThrow_AssertFailed_When_FirstEqualsDefault()
        {
            //if (!GetBoolResult(op_inequality, first, default(T)))
            //    throw new AssertFailedException("Expected (first != null) to return true.");

            var first = new FakedEquality(true, true, false, true);
            var second = new FakedEquality(false);
            var third = FakedEquality.Empty;

            AssertUnqualsOperator(first, second, third, "Expected (first != null) to return true.");
        }

        // ComparerEquals

        [TestMethod]
        public void ComparerEquals_ShouldThrow_AssertFailed_When_FirstUnequalsFirst()
        {
            //if (!comparer.Equals(first, first))
            //    throw new AssertFailedException("Expected comparer.Equals(T first, T first) to return true.");

            var first = new FakedEquality(false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertComparerEquals(FakedEquality.Comparer, first, second, third,
                "Expected comparer.Equals(T first, T first) to return true.");
        }

        [TestMethod]
        public void ComparerEquals_ShouldThrow_AssertFailed_When_FirstUnequalsSecond()
        {
            //if (!comparer.Equals(first, second))
            //    throw new AssertFailedException("Expected comparer.Equals(T first, T second) to return true.");

            var first = new FakedEquality(true, false);
            var second = FakedEquality.Empty;
            var third = FakedEquality.Empty;

            AssertComparerEquals(FakedEquality.Comparer, first, second, third,
                "Expected comparer.Equals(T first, T second) to return true.");
        }

        [TestMethod]
        public void ComparerEquals_ShouldThrow_AssertFailed_When_FirstEqualsThird()
        {
            //if (comparer.Equals(first, third))
            //    throw new AssertFailedException("Expected comparer.Equals(T first, T third) to return false.");

            var first = new FakedEquality(true, true);
            var second = new FakedEquality(true);
            var third = FakedEquality.Empty;

            AssertComparerEquals(FakedEquality.Comparer, first, second, third,
                "Expected comparer.Equals(T first, T third) to return false.");
        }

        [TestMethod]
        public void ComparerEquals_ShouldThrow_AssertFailed_When_SecondEqualsThird()
        {
            //if (comparer.Equals(second, third))
            //    throw new AssertFailedException("Expected comparer.Equals(T second, T third) to return false.");

            var first = new FakedEquality(true, true, false);
            var second = new FakedEquality(true);
            var third = FakedEquality.Empty;

            AssertComparerEquals(FakedEquality.Comparer, first, second, third,
                "Expected comparer.Equals(T second, T third) to return false.");
        }

        [TestMethod]
        public void ComparerEquals_ShouldThrow_AssertFailed_When_FirstEqualsDefault()
        {
            //if (comparer.Equals(first, default(T)))
            //    throw new AssertFailedException("Expected comparer.Equals(T first, T default(T)) to return false.");

            var first = new FakedEquality(true, true, false, true);
            var second = new FakedEquality(false);
            var third = FakedEquality.Empty;

            AssertComparerEquals(FakedEquality.Comparer, first, second, third,
                "Expected comparer.Equals(T first, T default(T)) to return false.");
        }

        [TestMethod]
        public void ComparerEquals_ShouldThrow_AssertFailed_When_SecondEqualsDefault()
        {
            //if (comparer.Equals(default(T), second))
            //    throw new AssertFailedException("Expected comparer.Equals(default(T), T second) to return false.");

            var first = new FakedEquality(true, true, false, false);
            var second = new FakedEquality(false, true);
            var third = FakedEquality.Empty;

            AssertComparerEquals(FakedEquality.Comparer, first, second, third,
                "Expected comparer.Equals(default(T), T second) to return false.");
        }

        // ComparerGetHashCode

        [TestMethod]
        public void ComparerGetHashCode_ShouldThrow_AssertFailed_When_HashCodeIsZero()
        {
            var first = new FakedEquality(1, 1, 0);
            var second = new FakedEquality(1, 1);
            var third = new FakedEquality(3);

            AssertComparerHashCode(FakedEquality.Comparer, first, second, third,
                "Expected comparer.GetHashCode(first) not to be equal to zero (0).");
        }

        [TestMethod]
        public void ComparerGetHashCode_ShouldThrow_AssertFailed_When_FirstDiffersFromSecond()
        {
            //if (comparer.GetHashCode(first) != comparer.GetHashCode(second))
            //    throw new AssertFailedException("Expected comparer.GetHashCode(first) to be equal to comparer.GetHashCode(second).");

            var first = new FakedEquality(1);
            var second = new FakedEquality(2);
            var third = new FakedEquality(3);

            AssertComparerHashCode(FakedEquality.Comparer, first, second, third,
                "Expected comparer.GetHashCode(first) to be equal to comparer.GetHashCode(second).");
        }

        [TestMethod]
        public void ComparerGetHashCode_ShouldThrow_AssertFailed_When_FirstEqualsThird()
        {
            //if (comparer.GetHashCode(first) == comparer.GetHashCode(third))
            //    throw new AssertFailedException("Expected comparer.GetHashCode(first) to be not equal to comparer.GetHashCode(third).");

            var first = new FakedEquality(1, 3);
            var second = new FakedEquality(1);
            var third = new FakedEquality(3);

            AssertComparerHashCode(FakedEquality.Comparer, first, second, third,
                "Expected comparer.GetHashCode(first) to be not equal to comparer.GetHashCode(third).");
        }

        [TestMethod]
        public void ComparerGetHashCode_ShouldThrow_AssertFailed_When_SecondEqualsThird()
        {
            //if (comparer.GetHashCode(second) == comparer.GetHashCode(third))
            //    throw new AssertFailedException("Expected comparer.GetHashCode(second) to be not equal to comparer.GetHashCode(third).");

            var first = new FakedEquality(1);
            var second = new FakedEquality(1, 3);
            var third = new FakedEquality(3);

            AssertComparerHashCode(FakedEquality.Comparer, first, second, third,
                "Expected comparer.GetHashCode(second) to be not equal to comparer.GetHashCode(third).");
        }

        private static void AssertComparerEquals(IEqualityComparer<FakedEquality> comparer, FakedEquality first,
            FakedEquality second, FakedEquality third, string expectedMessage)
        {
            Action assert = () => EqualityComparerAsserter.AssertEqualityComparerEquals(comparer, first, second, third);

            assert.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        private static void AssertUnqualsOperator(FakedEquality first, FakedEquality second, FakedEquality third,
            string expectedMessage)
        {
            Action assert = () => EqualityOperatorAsserter.AssertOperatorInequality(first, second, third);

            assert.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        private static void AssertEqualsOperator(FakedEquality first, FakedEquality second, FakedEquality third,
            string expectedMessage)
        {
            Action assert = () => EqualityOperatorAsserter.AssertOperatorEquality(first, second, third);

            assert.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        private static void AssertEqualsGeneric(FakedEquality first, FakedEquality second, FakedEquality third,
            string expectedMessage)
        {
            Action assert = () => GenericEqualsAsserter.AssertGenericEquals(first, second, third);

            assert.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        private static void AssertHashCode(FakedEquality first, FakedEquality second, FakedEquality third,
            string expectedMessage)
        {
            Action assert = () => EqualsObjectAsserter.AssertGetHashCode(first, second, third);

            assert.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        private static void AssertComparerHashCode(IEqualityComparer<FakedEquality> comparer, FakedEquality first,
            FakedEquality second, FakedEquality third, string expectedMessage)
        {
            Action assert = () => EqualityComparerAsserter.AssertComparerGetHashCode(comparer, first, second, third);

            assert.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }

        private static void AssertEqualsObject(FakedEquality first, FakedEquality second, FakedEquality third,
            string expectedMessage)
        {
            Action assert = () => EqualsObjectAsserter.AssertEqualsObject(first, second, third);

            assert.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage);
        }
    }
}