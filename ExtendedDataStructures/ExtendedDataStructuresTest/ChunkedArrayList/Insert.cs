using ExtendedDataStructures.ChunkedArrayList;
using Machine.Specifications;

namespace ExtendedDataStructuresTest.ChunkedArrayList
{
    [Subject(typeof(ChunkedArrayList<string>))]
    public class Insert_withinChunk : TestValues {
        static ChunkedArrayList<string> sut;

        Establish context = () => {
            sut = new ChunkedArrayList<string> {
                Strings[0],
                Strings[1],
                Strings[2],
            };

        };

        Because of = () => sut.Insert(2, Strings[3]);

        It should_have_4_elements = () => sut.Count.ShouldEqual(4);
        It should_have_1st_element_same = () => sut[0].ShouldEqual(Strings[0]);
        It should_have_2nd_element_same = () => sut[1].ShouldEqual(Strings[1]);
        It should_have_3rd_element_inserted_value = () => sut[3].ShouldEqual(Strings[3]);
        It should_have_4th_element_old_3rd_element = () => sut[4].ShouldEqual(Strings[2]);
    }
}