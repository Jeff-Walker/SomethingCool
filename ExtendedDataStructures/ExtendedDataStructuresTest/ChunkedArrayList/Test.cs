using System;
using ExtendedDataStructures.ChunkedArrayList;
using Machine.Specifications;

namespace ExtendedDataStructuresTest.ChunkedArrayList {
    [Subject(typeof(ChunkedArrayList<string>))]
    public class CreatingWithinChunk : TestValues {
        static ChunkedArrayList<string> sut;

        Establish context = () => { };

        Because of = () => {
            sut = new ChunkedArrayList<string> {
                Strings[0],
                Strings[1],
                Strings[2]
            };
        };

        It should_have_3_elements = () => sut.Count.ShouldEqual(3);
//        It should_have_first_element_long = () => {
//            var expected = Strings[0];
//            var s = sut[0];
//            s.ShouldEqual(expected);
//        };
        It should_have_first_element = () => sut[0].ShouldEqual(Strings[0]);
    }

    [Subject(typeof (ChunkedArrayList<string>))]
    public class CreatingWithTwoChunks : TestValues {
        static ChunkedArrayList<string> sut;

        Establish context = () => {  };
        Because of = () => {
            sut = new ChunkedArrayList<string>(5);
            sut.Add(Strings[0]);
            sut.Add(Strings[1]);
            sut.Add(Strings[2]);
            sut.Add(Strings[3]);
            sut.Add(Strings[4]);
            sut.Add(Strings[5]);
            sut.Add(Strings[6]);
            sut.Add(Strings[7]);
        };


        It should_have_8_elements = () => sut.Count.ShouldEqual(8);
        It should_have_1st_element = () => sut[0].ShouldEqual(Strings[0]);
        It should_have_2nd_element = () => sut[1].ShouldEqual(Strings[1]);
        It should_have_3rd_element = () => sut[2].ShouldEqual(Strings[2]);
        It should_have_4th_element = () => sut[3].ShouldEqual(Strings[3]);
        It should_have_5th_element = () => sut[4].ShouldEqual(Strings[4]);
        It should_have_6th_element = () => sut[5].ShouldEqual(Strings[5]);
        It should_have_7th_element = () => sut[6].ShouldEqual(Strings[6]);
        It should_have_8th_element = () => sut[7].ShouldEqual(Strings[7]);
    }

    [Subject(typeof (ChunkedArrayList<string>))]
    public class CreatingWithLotsOfChunks : TestValues {
        static ChunkedArrayList<string> sut;
        const int NumberOfElements = 40;

        Establish context = () => {
            sut = new ChunkedArrayList<string>(5);
        };

        Because of = () => {
            for (var i = 0 ; i < NumberOfElements ; i++) {
                sut.Add(Strings[i]);
            }
        };

        It should_have_the_right_size = () => sut.Count.ShouldEqual(NumberOfElements);

        It should_have_all_of_the_elements = () => {
            for (var i = 0 ; i < NumberOfElements ; i++) {
                sut[i].ShouldEqual(Strings[i]);
            }
        };
    }
}