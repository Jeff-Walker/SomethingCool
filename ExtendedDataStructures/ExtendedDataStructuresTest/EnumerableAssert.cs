using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendedDataStructuresTest {
    public static class EnumerableAssert {
        public static void AreEquivalent<TExpectedCollection, TActualCollection, TValue>(
                TExpectedCollection expected,
                TActualCollection actual)
                where TExpectedCollection : IEnumerable<TValue>
                where TActualCollection : IEnumerable<TValue> {
            IList<TValue> expectedList = expected.ToList();
            IList<TValue> actuaList = actual.ToList();
            if (expectedList.Count != actuaList.Count) {
                throw new AssertFailedException("expected " + expectedList.Count + ", but got " + actuaList.Count);
            }
            var missing = expectedList.Except(actuaList).ToList();
            var unexpected = actuaList.Except(expectedList).ToList();

            if (missing.Any() || unexpected.Any() || expectedList.Count != actuaList.Count) {
                var builder = new StringBuilder("Sequences don't match. ");
                if (missing.Any()) {
                    builder.AppendFormat("Items missing: [{0}].", string.Join(",", missing));
                }
                if (unexpected.Any()) {
                    builder.AppendFormat("Items unexpected: [{0}].", string.Join(",", unexpected));
                }
                if (expectedList.Count != actuaList.Count) {
                    builder.AppendFormat("Expected {0} items, but got {1}.", expectedList.Count, actuaList.Count);
                }
                throw new AssertFailedException(builder.ToString());
            }
        } 


    }
}
