using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using ExtendedDataStructures.Flexible;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomStringGenerator;

namespace ExtendedDataStructuresTest.MultiListDict {
    [TestClass]
    public class SpecificTests {
        readonly StringGenerator _stringGenerator = new StringGenerator();
        private IList<string> _testStrings;
        private const int StringLength = 10;
        private const int StringCount = 10;

        [TestInitialize]
        public void SetupStrings() {
            _testStrings = new List<string>(StringCount);
            var random = new Random();
            for (var i = 0 ; i < StringCount ; i++) {
//                _testStrings.Add(_stringGenerator.GenerateString(StringLength));
                _testStrings.Add("" + random.Next());
            }
        }

        [TestMethod]
        public void NewDictFromInitializer_AddSingle_ShouldContainElementsFromBothSets() {
            var sut = new MultiListDictionary<int, string> {
                {1, _testStrings[0], _testStrings[1]},
//                {2, _testStrings[2], _testStrings[3]}
            };

            sut.Add(1, _testStrings[4]);

            var expected = new[] { _testStrings[0], _testStrings[1], _testStrings[4]};
            CollectionAssert.AreEquivalent(expected, (ICollection)sut[1]);
        }
        [TestMethod]
        public void NewDictFromInitializer_AddMulti_ShouldContainElementsFromBothSets() {
            var sut = new MultiListDictionary<int, string> {
                {1, _testStrings[0], _testStrings[1]},
//                {2, _testStrings[2], _testStrings[3]}
            };

            sut.Add(1, _testStrings[4], _testStrings[5]);

            var expected = new[] { _testStrings[0], _testStrings[1], _testStrings[4], _testStrings[5]};
            CollectionAssert.AreEquivalent(expected, (ICollection)sut[1]);
        }
        [TestMethod]
        public void NewDictFromInitializer_AddIEnumerable_ShouldContainElementsFromBothSets() {
            var sut = new MultiListDictionary<int, string> {
                {1, _testStrings[0], _testStrings[1]},
//                {2, _testStrings[2], _testStrings[3]}
            };

            IEnumerable<string> toAdd = new[] {_testStrings[4], _testStrings[5]};
            sut.Add(1, toAdd);

            var expected = new[] { _testStrings[0], _testStrings[1], _testStrings[4], _testStrings[5]};
            CollectionAssert.AreEquivalent(expected, (ICollection)sut[1]);
        }

        [TestMethod]
        public void NewDictFromInit_Contains_ShouldReturnTrueForExistingPair() {
            var sut = new MultiListDictionary<int, string> {
                {1, _testStrings[0], _testStrings[1]},
            };

            var result = sut.Contains(1, _testStrings[1]);

            Assert.IsTrue(result, "dict should contain pair");
        }
        [TestMethod]
        public void NewDictFromInit_ContainsWithNonMatching_ShouldReturnFalseForExistingPair() {
            var sut = new MultiListDictionary<int, string> {
                {1, _testStrings[0], _testStrings[1]},
            };

            var result = sut.Contains(1, _testStrings[4]);

            Assert.IsFalse(result, "dict shouldn't contain pair");
        }
    }
}
