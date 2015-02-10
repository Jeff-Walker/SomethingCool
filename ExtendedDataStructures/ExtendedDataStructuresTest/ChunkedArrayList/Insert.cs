using System;
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

    public class Blah : TestValues {
        static int[] _members;
        Establish context = () => {
            _members = new int[5];
//            _members[0] = 0;
//            _members[1] = 1;
//            _members[2] = 2;
//            _members[3] = 3;
//            _members[4] = -1;
            _members[0] = 0;
            _members[1] = 1;
            _members[2] = 2;
            _members[3] = _members[4] = -1;
        };
        Because of = () => {
            int index = 2;
            int CurrentSize = 3;
Array.Copy(_members, index, _members, index+1, CurrentSize - index );
            _members[index] = 100;
        };

        It should_observation = () => {
//            _members[0].ShouldEqual(0);
//            _members[1].ShouldEqual(1);
//            _members[2].ShouldEqual(100);
//            _members[3].ShouldEqual(2);
//            _members[4].ShouldEqual(3);
            _members[0].ShouldEqual(0);
            _members[1].ShouldEqual(1);
            _members[2].ShouldEqual(100);
            _members[3].ShouldEqual(2);
            _members[4].ShouldEqual(-1);
        };
    }
}