using System;
using System.Collections;
using System.Collections.Generic;
using ExtendedDataStructures.Flexible;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendedDataStructuresTest.MultiListDict
{
    [TestClass]
    public class MultiListDictionaryTests {
        private readonly Random _random = new Random();

       
        [TestMethod]
        public void WithDuplicatesSeperateAdds_ShouldHaveBoth() {
            var key = _random.Next().ToString();
            var value1 = _random.Next();
            var sut = new MultiListDictionary<string, int> {
                {key, value1}, {key, value1}
            };

            var result = sut[key];

            CollectionAssert.AreEquivalent(new[] { value1, value1 }, (ICollection)result);
        } 
        [TestMethod]
        public void WithDuplicatesSingleAdd_ShouldHaveBoth() {
            var key = _random.Next().ToString();
            var value1 = _random.Next();
            var sut = new MultiListDictionary<string, int> {
                {key, value1, value1},
            };

            var result = sut[key];

            CollectionAssert.AreEquivalent(new[] { value1, value1 }, (ICollection)result);
        }
    }
}
