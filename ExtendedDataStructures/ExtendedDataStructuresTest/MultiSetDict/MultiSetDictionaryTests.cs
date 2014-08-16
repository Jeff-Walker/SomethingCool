using System;
using System.Linq;
using ExtendedDataStructures.Flexible;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtendedDataStructuresTest.MultiSetDict {
    [TestClass]
    public class MultiSetDictionaryTests {
        private readonly Random _random = new Random();

        [TestMethod]
        public void DuplicateAdd_ShouldHaveOnlyOne() {
            var key = _random.Next().ToString();
            var value1 = _random.Next();
            var sut = new MultiSetDictionary<string, int> {
                {key, value1},
                {key, value1}
            };

            var result = sut[key];

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(value1, result.Single());
//            CollectionAssert.AreEquivalent(new[]{value1}, (ICollection)result);
        }

        [TestMethod]
        public void DuplicateAddMulti_ShouldHaveOnlyOne() {
            var key = _random.Next().ToString();
            var value1 = _random.Next();
            var sut = new MultiSetDictionary<string, int> {
                {key, value1, value1}
            };

            var result = sut[key];

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(value1, result.Single());
//            CollectionAssert.AreEquivalent(new[]{value1}, (ICollection)result);
        }
    }
}
