using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiP.EqualityAssertion.Helpers
{
    internal class EqualityOperatorAsserter
    {
        public static void AssertOperatorEquality<T>(T first, T second, T third)
        {
            // ReSharper disable once InconsistentNaming
            var op_equality = typeof (T).GetMethod("op_Equality", new[] {typeof (T), typeof (T)});
            if (op_equality == null)
                return; // operator== not implemented

            if (!GetBoolResult(op_equality, first, first))
                throw new AssertFailedException("Expected (first == first) to return true.");

            if (!GetBoolResult(op_equality, first, second))
                throw new AssertFailedException("Expected (first == second) to return true.");

            if (GetBoolResult(op_equality, first, third))
                throw new AssertFailedException("Expected (first == third) to return false.");

            if (GetBoolResult(op_equality, second, third))
                throw new AssertFailedException("Expected (second == third) to return false.");

            if (GetBoolResult(op_equality, first, default(T)))
                throw new AssertFailedException("Expected (first == null) to return false.");
        }

        public static void AssertOperatorInequality<T>(T first, T second, T third)
        {
            // ReSharper disable once InconsistentNaming
            var op_inequality = typeof (T).GetMethod("op_Inequality", new[] {typeof (T), typeof (T)});
            if (op_inequality == null)
                return; // operator!= not implemented

            if (GetBoolResult(op_inequality, first, first))
                throw new AssertFailedException("Expected (first != first) to return false.");

            if (GetBoolResult(op_inequality, first, second))
                throw new AssertFailedException("Expected (first != second) to return false.");

            if (!GetBoolResult(op_inequality, first, third))
                throw new AssertFailedException("Expected (first != third) to return true.");

            if (!GetBoolResult(op_inequality, second, third))
                throw new AssertFailedException("Expected (second != third) to return true.");

            if (!GetBoolResult(op_inequality, first, default(T)))
                throw new AssertFailedException("Expected (first != null) to return true.");
        }

        private static bool GetBoolResult<T>(MethodInfo method, T first, T second)
        {
            return (bool) method.Invoke(null, new object[] {first, second});
        }
    }
}