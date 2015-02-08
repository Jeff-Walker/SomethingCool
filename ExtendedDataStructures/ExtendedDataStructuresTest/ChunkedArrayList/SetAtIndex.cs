using ExtendedDataStructures.ChunkedArrayList;
using Machine.Specifications;

namespace ExtendedDataStructuresTest.ChunkedArrayList {
    [Subject(typeof(ChunkedArrayList<string>))]
    public class SetAtIndex_WithinChunk : TestValues {
        static ChunkedArrayList<string> sut;

        Establish context = () => {
            sut = new ChunkedArrayList<string> {
                Strings[0],
                Strings[1],
                Strings[2],
            };
        };
        Because of = () => {
            sut[1] = Strings[3];
        };

        It should_have_2nd_element_different = () => sut[1].ShouldEqual(Strings[3]);
        It should_have_1st_element_same = () => sut[0].ShouldEqual(Strings[0]);
        It should_have_3rd_element_same = () => sut[2].ShouldEqual(Strings[2]);
    }
    
    [Subject(typeof(ChunkedArrayList<string>))]
    public class SetAtIndex_InNextChunk : TestValues {
        static ChunkedArrayList<string> sut;

        Establish context = () => {
            sut = new ChunkedArrayList<string>(3) {
                Strings[0],
                Strings[1],
                Strings[2],
                Strings[3],
                Strings[4],
            };
        };
        Because of = () => {
            sut[3] = Strings[5];
        };

        It should_have_4th_element_different = () => sut[3].ShouldEqual(Strings[5]);
        It should_have_1st_element_same = () => sut[0].ShouldEqual(Strings[0]);
        It should_have_2nd_element_same = () => sut[1].ShouldEqual(Strings[1]);
        It should_have_3rd_element_same = () => sut[2].ShouldEqual(Strings[2]);
        It should_have_5th_element_same = () => sut[4].ShouldEqual(Strings[4]);
    }
}