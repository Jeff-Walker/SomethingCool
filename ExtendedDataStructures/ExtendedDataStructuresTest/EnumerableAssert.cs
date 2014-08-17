using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendedDataStructuresTest {
    static class EnumerableAssert {

        public static void AreEquivalent<TValue>(IEnumerable<TValue> expected, IEnumerable<TValue> actual, string message = null, params object[] parameters) {
            IList<TValue> expectedList = expected.ToList();
            IList<TValue> actuaList = actual.ToList();

            var missing = expectedList.Except(actuaList).ToList();
            var unexpected = actuaList.Except(expectedList).ToList();

            if (missing.Any() || unexpected.Any() || expectedList.Count != actuaList.Count) {
                var builder = new StringBuilder("EnumerableAssert.AreEquivalent() failed. Sequences don't match. ");
                if (missing.Any()) {
                    builder.AppendFormat("Item{0} missing: [{1}]. ", OptionalS(missing.Count), string.Join(",", missing));
                }
                if (unexpected.Any()) {
                    builder.AppendFormat("Item{0} unexpected: [{1}]. ", OptionalS(unexpected.Count), string.Join(",", unexpected));
                }
                if (expectedList.Count != actuaList.Count) {
                    builder.AppendFormat("Expected {0} item{1}, but got {2}.", expectedList.Count, OptionalS(expectedList.Count), actuaList.Count);
                }
                if (message != null) {
                    builder.AppendFormat(" " + message, parameters);
                }
                throw new AssertFailedException(builder.ToString());
            }
        }

        private static string OptionalS(int count) {
            return count > 1 ? "s" : "";
        }

//        public static void AreEquivalent<TExpectedCollection, TActualCollection, TValue>(
//                TExpectedCollection expected,
//                TActualCollection actual)
//                where TExpectedCollection : IEnumerable<TValue>
//                where TActualCollection : IEnumerable<TValue> {
//            IList<TValue> expectedList = expected.ToList();
//            IList<TValue> actuaList = actual.ToList();
////            if (expectedList.Count != actuaList.Count) {
////                throw new AssertFailedException("expected " + expectedList.Count + ", but got " + actuaList.Count);
////            }
//            var missing = expectedList.Except(actuaList).ToList();
//            var unexpected = actuaList.Except(expectedList).ToList();
//
//            if (missing.Any() || unexpected.Any() || expectedList.Count != actuaList.Count) {
//                var builder = new StringBuilder("Sequences don't match. ");
//                if (missing.Any()) {
//                    builder.AppendFormat("Items missing: [{0}]. ", string.Join(",", missing));
//                }
//                if (unexpected.Any()) {
//                    builder.AppendFormat("Items unexpected: [{0}]. ", string.Join(",", unexpected));
//                }
//                if (expectedList.Count != actuaList.Count) {
//                    builder.AppendFormat("Expected {0} items, but got {1}.", expectedList.Count, actuaList.Count);
//                }
//                throw new AssertFailedException(builder.ToString());
//            }
//        } 


    }
}
