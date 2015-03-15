using System;
using ExtendedDataStructures.ChunkedArrayList;
using Machine.Specifications;

namespace ExtendedDataStructuresTest.ChunkedArrayList
{
    [Subject(typeof(ChunkedArrayList<string>))]
    public class RemoveAt_tooSmall : TestValues {
        static ChunkedArrayList<string> sut;
        static Exception e;

        Establish context = () => {
            sut = new ChunkedArrayList<string>();
        };

        Because of = () => {
            e = Catch.Only<ArgumentOutOfRangeException>(() => sut.RemoveAt(-1));
        };

        It should_throw_exception = () => e.ShouldBeOfExactType<ArgumentOutOfRangeException>();
    }
    [Subject(typeof(ChunkedArrayList<string>))]
    public class RemoveAt_tooBig : TestValues {
        static ChunkedArrayList<string> sut;
        static Exception e;

        Establish context = () => {
            sut = new ChunkedArrayList<string> {
                Strings[0]
            };
        };

        Because of = () => {
            e = Catch.Only<ArgumentOutOfRangeException>(() => sut.RemoveAt(5));
        };

        It should_throw_exception = () => e.ShouldBeOfExactType<ArgumentOutOfRangeException>();
    }

    [Subject(typeof (ChunkedArrayList<string>))]
    public class RemoveAt_singleChunk : TestValues {
        static ChunkedArrayList<string> sut;
        static int removeLocation;

        Establish context = () => {
            sut = new ChunkedArrayList<string> {
                Strings[0],
                Strings[1],
                Strings[2],
                Strings[3]
            };
            removeLocation = 2;
        };

        Because of = () => sut.RemoveAt(removeLocation);

        It should_have_removed_the_item = () => sut.ShouldContainOnly(
                Strings[0],
                Strings[1],
                Strings[3]
            );

        It should_have_first_item = () => sut[0].ShouldEqual(Strings[0]);
        It should_have_second_item = () => sut[1].ShouldEqual(Strings[1]);
        It should_have_fourth_item = () => sut[2].ShouldEqual(Strings[3]);
        It should_have_count_3 = () => sut.Count.ShouldEqual(3);
    }
}