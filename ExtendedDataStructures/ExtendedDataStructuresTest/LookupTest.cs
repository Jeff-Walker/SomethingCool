using System.Collections;
using System.Collections.Generic;
using ExtendedDataStructures.Flexible;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomStringGenerator;

// ReSharper disable InconsistentNaming
namespace ExtendedDataStructuresTest {
    [TestClass]
    public class MultiListDictionary_IDictionary_Test {
        readonly StringGenerator _stringGenerator = new StringGenerator();
        private IList<string> _testStrings;
        private const int StringLength = 10;
        private const int StringCount = 10;

        [TestInitialize]
        public void SetupStrings() {
            _testStrings = new List<string>(StringCount);
            for (var i = 0; i < StringCount; i++) {
                _testStrings.Add(_stringGenerator.GenerateString(StringLength));
            }
        }

        [TestMethod]
        public void NewDictFromIDictInitializer_ShouldContainAllElements() {
            var sut = new MultiListDictionary<int, string> {
                {1, new[] {_testStrings[0], _testStrings[1]}},
                {2, new[] {_testStrings[2], _testStrings[3]}},
            };

            Assert.AreEqual(2, sut.Count, "dict should have 2 entries");

            CollectionAssert.AreEquivalent( new []{1, 2}, (ICollection) sut.Keys);

            Assert.AreEqual(2, sut[1].Count, "dict[1] should have two elements");
            Assert.AreEqual(2, sut[2].Count, "dict[2] should have two elements");

            CollectionAssert.AreEquivalent(new []{_testStrings[1], _testStrings[0]}, (ICollection) sut[1], "dict[1] should have the given strings");
            CollectionAssert.AreEquivalent(new []{_testStrings[3], _testStrings[2]}, (ICollection) sut[2], "dict[2] should have the given strings");
        }

        [TestMethod]
        public void NewEmptyDict_IDictAdd_ShouldContainAddedElements() {
// ReSharper disable once UseObjectOrCollectionInitializer
            var sut = new MultiListDictionary<int, string>();

            sut.Add(1, new []{_testStrings[0], _testStrings[1]});
            sut.Add(2, new []{_testStrings[2], _testStrings[3]});

            Assert.AreEqual(2, sut.Count, "dict should have 2 entries");

            CollectionAssert.AreEquivalent(new[] { 1, 2 }, (ICollection)sut.Keys);

            Assert.AreEqual(2, sut[1].Count, "dict[1] should have two elements");
            Assert.AreEqual(2, sut[2].Count, "dict[2] should have two elements");

            CollectionAssert.AreEquivalent(new[] { _testStrings[1], _testStrings[0] }, (ICollection)sut[1], "dict[1] should have the given strings");
            CollectionAssert.AreEquivalent(new[] { _testStrings[3], _testStrings[2] }, (ICollection)sut[2], "dict[2] should have the given strings");
        }

        [TestMethod]
        public void NewEmptyDict_AddNullValue_ShouldContainNullValue() {
            var sut = new MultiListDictionary<int, string>();

            sut.Add(1,(string[]) null);
        }
    }
}