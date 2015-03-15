using System;
using Machine.Specifications;
using ExtendedDataStructures.ChunkedArrayList;

namespace ExtendedDataStructuresTest.ChunkedArrayList {
    [Subject(typeof(ChunkedArrayList<string>))]
    public class CopyTo_TooSmalArray :TestValues {
        static ChunkedArrayList<string> sut;
        static string[] _destination;
        static ArgumentException exception;
        Establish context = () => {
            sut = new ChunkedArrayList<string> {
                Strings[0],
                Strings[1],
                Strings[2],
                Strings[3],
                Strings[4],
            };
            _destination = new string[3];
        };

        Because of = () => {
            exception = Catch.Only<ArgumentException>(() => sut.CopyTo(_destination, 0));
        };

        It should_catch_argumentexception = () => exception.ShouldBeOfExactType<ArgumentException>();
        It should_have_argument_name = () => exception.ParamName.ShouldEqual("array");
    }

    [Subject(typeof(ChunkedArrayList<string>))]
    public class CopyTo_NegativeArrayIndex : TestValues {
        static ChunkedArrayList<string> sut;
        static string[] _destination;
        static int destinationIndex;
        static ArgumentException exception;
     
        Establish context = () => {
            sut = new ChunkedArrayList<string> {
                Strings[0],
                Strings[1],
                Strings[2],
                Strings[3],
                Strings[4],
            };
            _destination = new string[5];
            destinationIndex = -2;
        };

        Because of = () => {
            exception = Catch.Only<ArgumentException>(() => sut.CopyTo(_destination, destinationIndex));
        };

        It should_catch_argumentexception = () => exception.ShouldBeOfExactType<ArgumentOutOfRangeException>();
        It should_have_argument_name = () => exception.ParamName.ShouldEqual("arrayIndex");
    }
    [Subject(typeof(ChunkedArrayList<string>))]
    public class CopyTo_nullArray : TestValues {
        static ChunkedArrayList<string> sut;
        static string[] _destination;
        static int destinationIndex;
        static Exception exception;
     
        Establish context = () => {
            sut = new ChunkedArrayList<string> {
                Strings[0],
                Strings[1],
                Strings[2],
                Strings[3],
                Strings[4],
            };
            _destination = null;//new string[5];
            destinationIndex = 2;
        };

        Because of = () => {
            exception = Catch.Only<ArgumentException>(() => sut.CopyTo(_destination, destinationIndex));
        };

        It should_catch_argumentexception = () => exception.ShouldBeOfExactType<ArgumentNullException>();
        It should_have_argument_name = () => ((ArgumentNullException)exception).ParamName.ShouldEqual("array");
    }


    [Subject(typeof (ChunkedArrayList<string>))]
    public class CopyTo_within_signle_chunk : TestValues {
        static ChunkedArrayList<string> sut;
        static string[] destination;

        Establish context = () => {
            sut = new ChunkedArrayList<string> {
                Strings[0],
                Strings[1],
                Strings[2]
            };
            destination = new string[3];

        };

        Because of = () => sut.CopyTo(destination, 0);

        It should_copy_elements_to_destination = () => 
            destination.ShouldContainOnly(
                Strings[0],
                Strings[1],
                Strings[2]);
    }
    [Subject(typeof (ChunkedArrayList<string>))]
    public class CopyTo_multichunk : TestValues {
        static ChunkedArrayList<string> sut;
        static string[] destination;

        Establish context = () => {
            sut = new ChunkedArrayList<string>(4) {
                Strings[0],
                Strings[1],
                Strings[2],
                Strings[3],
                Strings[4],
                Strings[5],
                Strings[6],
                Strings[7],
                Strings[8],
                Strings[9],
            };
            destination = new string[10];

        };

        Because of = () => sut.CopyTo(destination, 0);

        It should_copy_elements_to_destination = () => 
            destination.ShouldContainOnly(
                Strings[0],
                Strings[1],
                Strings[2],
                Strings[3],
                Strings[4],
                Strings[5],
                Strings[6],
                Strings[7],
                Strings[8],
                Strings[9]);
    }
}
