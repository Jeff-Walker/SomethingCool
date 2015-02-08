using System;
using ExtendedDataStructures.ChunkedArrayList;
using Machine.Specifications;

namespace ExtendedDataStructuresTest.ChunkedArrayList {
    [Subject(typeof(ChunkedArrayList<string>))]
    public class Creating : TestValues {
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
}