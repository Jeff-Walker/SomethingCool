using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtendedDataStructures.Flexible;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable InconsistentNaming
namespace ExtendedDataStructuresTest.MultiListDict {
    [TestClass]
    public class MultiListDictionary_IDictionary_Test {
        private IList<string> _testStrings;
        private const int StringCount = 10;

        [TestInitialize]
        public void SetupStrings() {
            _testStrings = new List<string>(StringCount);
            var random = new Random();
            for (var i = 0 ; i < StringCount ; i++) {
                _testStrings.Add("" + random.Next());
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
        [ExpectedException(typeof(ArgumentNullException))]

        public void NewEmptyDict_AddNullCollection_ShouldThrow() {
// ReSharper disable once UseObjectOrCollectionInitializer
            var sut = new MultiListDictionary<int, string>();

            sut.Add(1,(string[]) null);

        }

        [TestMethod]
        public void NewEmptyDict_AddNullValue_ShouldContainNullValue() {
// ReSharper disable once UseObjectOrCollectionInitializer
            var sut = new MultiListDictionary<int, string>();

            sut.Add(1, new string[]{null});

            Assert.AreEqual(1, sut.Count, "dict should have one entry");
            Assert.AreEqual(1, sut[1].Count, "dict[1] should have one entry");
            CollectionAssert.AreEquivalent(new string[] { null }, (ICollection)sut[1], "dict[1] should have one null value");
        }
        [TestMethod]
        public void NewDictFromInitializer_ShouldContainAllElements() {
            var sut = new MultiListDictionary<int, string> {
                {1, _testStrings[0], _testStrings[1]},
                {2, _testStrings[2], _testStrings[3]},
            };

            Assert.AreEqual(2, sut.Count, "dict should have 2 entries");
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, (ICollection)sut.Keys);

            Assert.AreEqual(2, sut[1].Count, "dict[1] should have two elements");
            Assert.AreEqual(2, sut[2].Count, "dict[2] should have two elements");

            CollectionAssert.AreEquivalent(new[] { _testStrings[1], _testStrings[0] }, (ICollection)sut[1], "dict[1] should have the given strings");
            CollectionAssert.AreEquivalent(new[] { _testStrings[3], _testStrings[2] }, (ICollection)sut[2], "dict[2] should have the given strings");
        }

        [TestMethod]
        public void NewEmptyDict_Add_ShouldContainAddedElements() {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var sut = new MultiListDictionary<int, string>();

            sut.Add(1, _testStrings[0], _testStrings[1]);
            sut.Add(2, _testStrings[2], _testStrings[3]);

            Assert.AreEqual(2, sut.Count, "dict should have 2 entries");

            CollectionAssert.AreEquivalent(new[] { 1, 2 }, (ICollection)sut.Keys);

            Assert.AreEqual(2, sut[1].Count, "dict[1] should have two elements");
            Assert.AreEqual(2, sut[2].Count, "dict[2] should have two elements");

            CollectionAssert.AreEquivalent(new[] { _testStrings[1], _testStrings[0] }, (ICollection)sut[1], "dict[1] should have the given strings");
            CollectionAssert.AreEquivalent(new[] { _testStrings[3], _testStrings[2] }, (ICollection)sut[2], "dict[2] should have the given strings");
        }
       

        [TestMethod]
        public void NewDict_ILookupIndexer_ShouldReturnIEnumerableOfContents() {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var sut = new MultiListDictionary<int, string>();

            sut.Add(1, _testStrings[0], _testStrings[1]);
            sut.Add(2, _testStrings[2], _testStrings[3]);

            var result = ((ILookup<int, string>)sut)[1];

            CollectionAssert.AreEquivalent(new[] { _testStrings[0], _testStrings[1] }, result.ToList());
        }
    }
}