using System;
using System.Collections.Generic;
using ExtendedDataStructures.ChunkedArrayList;
using Machine.Specifications;

namespace ExtendedDataStructuresTest.ChunkedArrayList {
    [Subject(typeof (ChunkedArrayList<string>))]
    public class CreatingWithLotsOfChunks_WithEnumerator : TestValues {
        static ChunkedArrayList<string> sut;
        const int NumberOfElements = 40;
        static IEnumerator<string> enumerator;

        Establish context = () => {
            sut = new ChunkedArrayList<string>(5);
            for (var i = 0 ; i < NumberOfElements ; i++) {
                sut.Add(Strings[i]);
            }
        };

        Because of = () => {
        };

//        It should_have_the_right_size = () => sut.Count.ShouldEqual(NumberOfElements);
//
//        It should_have_all_of_the_elements = () => {
//            var i = 0;
//            foreach (var s in sut) {
//                Console.WriteLine("s=" + s + " , strings[" + i + "]=" + Strings[i]);
//                try {
//                    s.ShouldEqual(Strings[i]);
//                } catch (SpecificationException e) {
//                    throw new SpecificationException(" fail at i=" + i + ";" + e.Message, e);
//                }
//                i++;
//            }
//
//            i.ShouldEqual(NumberOfElements);
//        };

        

        It should_throw_if_Current_before_MoveNext = () => {
            enumerator = sut.GetEnumerator();
            Catch.Only<InvalidOperationException>(() => { var current = enumerator.Current; })
                    .ShouldBeOfExactType<InvalidOperationException>();
        };

        It should_throw_if_MoveNext_after_Dispose = () => {
            enumerator = sut.GetEnumerator();
            enumerator.MoveNext().ShouldEqual(true);
            enumerator.MoveNext().ShouldEqual(true);

            enumerator.Dispose();
            Catch.Only<InvalidOperationException>(() => enumerator.MoveNext())
                .ShouldBeOfExactType<InvalidOperationException>();
        };

        It should_still_allow_Current_after_Dispose = () => {
            enumerator = sut.GetEnumerator();
            enumerator.MoveNext().ShouldEqual(true);
            enumerator.Current.ShouldEqual(Strings[0]);
            enumerator.Dispose();
            enumerator.Current.ShouldEqual(Strings[0]);
        };

        It should_start_over_on_Reset = () => {
            enumerator = sut.GetEnumerator();
            enumerator.MoveNext().ShouldEqual(true);
            enumerator.Current.ShouldEqual(Strings[0]);
            
            enumerator.MoveNext().ShouldEqual(true);
            enumerator.Current.ShouldEqual(Strings[1]);

            enumerator.Reset();

            enumerator.MoveNext().ShouldEqual(true);
            enumerator.Current.ShouldEqual(Strings[0]);
        };

       

        It should_throw_ioe_if_add_to_collection_in_other_chunk = () => {
            enumerator = sut.GetEnumerator();
        };

        It should_stop_at_the_end_of_the_collection = () => {
            enumerator = sut.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext()) {
                enumerator.Current.ShouldEqual(Strings[i++]);
            }
            i.ShouldEqual(NumberOfElements);
        };

//        It should_be_ok_after_insert = () => {
//            sut.Insert(2, "inserted");
//            enumerator = sut.GetEnumerator();
//        };
        It should_throw_ioe_if_Add_to_collection = () => {
            enumerator = sut.GetEnumerator();
            sut.Add("bad news");

            Catch.Only<InvalidOperationException>(() => enumerator.MoveNext())
                .ShouldBeOfExactType<InvalidOperationException>();
        };
    }

    [Subject(typeof (ChunkedArrayList<string>))]
    public class EnumeratorAfterInsert : TestValues {
        const int NumberOfElements = 8;
        static ChunkedArrayList<string> sut;
        static IEnumerator<string> enumerator;

        Establish context = () => {
            sut = new ChunkedArrayList<string>(50);
            for (var i = 0 ; i < NumberOfElements ; i++) {
                sut.Add(Strings[i]);
            }
            sut.Insert(5, "inserted");
        };

        Because of = () => {  };
        It should_enumerate_it_all = () =>
            sut.ShouldEqual<IEnumerable<String>>(new[] {
                Strings[0],
                Strings[2],
                Strings[3],
                Strings[4],
                "inserted",
                Strings[5],
                Strings[6],
                Strings[7],
            }
        );
    }

}