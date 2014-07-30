using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Camelizer;

namespace CamelizerTest {
    [TestClass]
    public class UnitTest1 {
 
        [TestMethod]
        public void TestUpper_AlreadyCamel() {
            Assert.AreEqual("RegularCamelWord", "RegularCamelWord".CamelCase());
        }

        [TestMethod]
        public void TestUpper_3Words() {
            Assert.AreEqual("OneTwoThree", "one two three".CamelCase());
        }

        [TestMethod]
        public void TestUpper_1WordAndLowerCamel() {
            Assert.AreEqual("OneTwoThree", "one twoThree".CamelCase());
        }

        [TestMethod]
        public void TestUpper_3WordsOneUpper() {
            Assert.AreEqual("OneTwoThree", "one two Three".CamelCase());
        }

        [TestMethod]
        public void TestUpper_3WordsWithUnderscores() {
            Assert.AreEqual("OneTwoThree", "one_two_Three".CamelCase());
        }

        [TestMethod]
        public void TestUpper_3WordsAllUpperWithUnderscores() {
            Assert.AreEqual("OneTwoThree", "ONE_TWO_THREE".CamelCase());
        }

        [TestMethod]
        public void TestUpper_3WordsSeparatedByNonitentifiers() {
            Assert.AreEqual("OneTwoThree", "one$two!three".CamelCase());
        }

        [TestMethod]
        public void TestUpper_SingleLowerLetter() {
            Assert.AreEqual("O", "o".CamelCase());
        }

        [TestMethod]
        public void TestUpper_SingleUpperLetter() {
            Assert.AreEqual("O", "O".CamelCase());
        }


        [TestMethod]
        public void TestLowerCamel() {}

        [TestMethod]
        public void TestLower_AlreadyLowerCamel() {
            Assert.AreEqual("regularCamelWord", "regularCamelWord".CamelCase(Camelizer.Camelizer.CamelStyle.Lower));
        }

        [TestMethod]
        public void TestLower_3Words() {
            Assert.AreEqual("oneTwoThree", "one two three".CamelCase(Camelizer.Camelizer.CamelStyle.Lower));
        }

        [TestMethod]
        public void TestLower_1WordAndLowerCamel() {
            Assert.AreEqual("oneTwoThree", "one twoThree".CamelCase(Camelizer.Camelizer.CamelStyle.Lower));
        }

        [TestMethod]
        public void TestLower_3WordsOnUpper() {
            Assert.AreEqual("oneTwoThree", "one two Three".CamelCase(Camelizer.Camelizer.CamelStyle.Lower));
        }

        [TestMethod]
        public void TestLower_3WordsWithUnderscores() {
            Assert.AreEqual("oneTwoThree", "one_two_Three".CamelCase(Camelizer.Camelizer.CamelStyle.Lower));
        }

        [TestMethod]
        public void TestLower_3WordsAllUpperWithUnderscores() {
            Assert.AreEqual("oneTwoThree", "ONE_TWO_THREE".CamelCase(Camelizer.Camelizer.CamelStyle.Lower));
        }

        [TestMethod]
        public void TestLower_3WordsSeparatedByNonitentifiers() {
            Assert.AreEqual("oneTwoThree", "one$two!three".CamelCase(Camelizer.Camelizer.CamelStyle.Lower));
        }

        [TestMethod]
        public void TestLower_SingleLowerLetter() {
            Assert.AreEqual("o", "o".CamelCase(Camelizer.Camelizer.CamelStyle.Lower));
        }

        [TestMethod]
        public void TestLower_SingleUpperLetter() {
            Assert.AreEqual("o", "O".CamelCase(Camelizer.Camelizer.CamelStyle.Lower));
        }
    }
}