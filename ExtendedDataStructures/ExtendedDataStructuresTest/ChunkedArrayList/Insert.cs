using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ExtendedDataStructures.ChunkedArrayList;
using Machine.Specifications;

namespace ExtendedDataStructuresTest.ChunkedArrayList {
    [Subject(typeof (ChunkedArrayList<string>))]
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
        It should_have_3rd_element_inserted_value = () => sut[2].ShouldEqual(Strings[3]);
        It should_have_4th_element_old_3rd_element = () => sut[3].ShouldEqual(Strings[2]);
    }

    [Subject(typeof (ChunkedArrayList<string>))]
    public class Insert_inFirstOfManyChunks : TestValues {
        static ChunkedArrayList<string> sut;
        static string insertedValue;
        const int NumberOfElements = 6;
        const int InsertPoint = 2;

        Establish context = () => {
            sut = new ChunkedArrayList<string>(4);
            int i;
            for (i = 0 ; i < NumberOfElements ; i++) {
                sut.Add(Strings[i]);
            }
            insertedValue = Strings[i];
        };

        Because of = () => sut.Insert(InsertPoint, insertedValue);

        It should_have_number_plus_1_elements = () => sut.Count.ShouldEqual(NumberOfElements + 1);

        It should_have_the_right_elements_up_to_insert_point =
                () => ShouldHaveTheRightElementsUpToInsertPoint(InsertPoint);

        public static void ShouldHaveTheRightElementsUpToInsertPoint(int insertPoint) {
            for (var i = 0 ; i < insertPoint ; i++) {
                try {
                    sut[i].ShouldEqual(Strings[i]);
                } catch (SpecificationException e) {
                    throw new SpecificationException(" fail at i=" + i + ";" + e.Message, e);
                }
            }
        }

        It should_have_the_inserted_value_at_insertionpoint = () => sut[InsertPoint].ShouldEqual(insertedValue);
        It should_have_the_right_elements_after_insertion = () => ShouldHaveTheRightElementsAfterInsertion(InsertPoint);

        public static void ShouldHaveTheRightElementsAfterInsertion(int insertPoint) {
            for (var i = insertPoint + 1 ; i < sut.Count ; i++) {
                try {
                    sut[i].ShouldEqual(Strings[i - 1]);
                } catch (SpecificationException e) {
                    throw new SpecificationException(" fail at i=" + i + ";" + e.Message, e);
                }
            }
        }
    }

    [Subject(typeof (ChunkedArrayList<string>))]
    public class Insert_firstInserSplitsThenNextInsertInFirstChunk : TestValues {
        static ChunkedArrayList<string> sut;
        const int NumberOfElements = 6;
        const int InsertPoint = 4;
        const int InsertPointSecond = 2;
        const string InsertMessage1 = "insert 1";
        const string InsertMessage2 = "insert 2";

        Establish context = () => {
            sut = new ChunkedArrayList<string>(10);
            for (var i = 0 ; i < NumberOfElements ; i++) {
                sut.Add(Strings[i]);
            }
            sut.Insert(InsertPoint, InsertMessage1);
        };

        Because of = () => sut.Insert(InsertPointSecond, InsertMessage2);

        It should_have_8_elements = () => sut.Count.ShouldEqual(NumberOfElements + 2);
        // second insert moved it over 1:
        It should_have_inserted_value_at_first_insert_point = () => sut[InsertPoint + 1].ShouldEqual(InsertMessage1);
        It should_have_inserted_value_at_second_insert_point = () => sut[InsertPointSecond].ShouldEqual(InsertMessage2);
//        It should_have_the_right_elements = () => {
//            int sutIndex = 0, stringsIndex = 0;
//            sut[sutIndex++].ShouldEqual(Strings[stringsIndex++]);
//            sut[sutIndex++].ShouldEqual(Strings[stringsIndex++]);
//            sut[sutIndex++].ShouldEqual(InsertMessage2);
//            sut[sutIndex++].ShouldEqual(Strings[stringsIndex++]);
//            sut[sutIndex++].seq
//        };

        It should_have_the_right_elements = () => sut.ShouldEqual<IEnumerable<string>>(new[] {
            Strings[0], Strings[1], InsertMessage2, Strings[2], Strings[3], InsertMessage1, Strings[4], Strings[5]
        });
    }

    [Subject(typeof (ChunkedArrayList<string>))]
    public class Insert_inFirstChunkThenMoreInsertsPastChunk : TestValues {
        static ChunkedArrayList<string> sut;
        const int NumberOfElements = 6;
        const int InsertPoint = 2;
        const int NumberOfInserts = 4;
        static List<string> insertedValues = new List<string>(NumberOfInserts);

        Establish context = () => {
            sut = new ChunkedArrayList<string>(4);
            int i;
            for (i = 0 ; i < NumberOfElements ; i++) {
                sut.Add(Strings[i]);
            }

        };

        Because of = () => {
//            for (var i = 0 ; i < NumberOfInserts ; i++) {
            var i = 0;
            var item = Strings[i + NumberOfElements];
            sut.Insert(InsertPoint + i, item);
            insertedValues.Add(item);
            i++;

            item = Strings[i + NumberOfElements];
            sut.Insert(InsertPoint + i, item);
            insertedValues.Add(item);
            i++;

            item = Strings[i + NumberOfElements];
            sut.Insert(InsertPoint + i, item);
            insertedValues.Add(item);
            i++;

            item = Strings[i + NumberOfElements];
            sut.Insert(InsertPoint + i, item);
            insertedValues.Add(item);
            i++;

//            }
        };

        It should_have_number_plus_inserted__elements = () => sut.Count.ShouldEqual(NumberOfElements + NumberOfInserts);

        It should_have_the_right_elements_up_to_insert_point = () => {
            var i = 0;
            sut[i].ShouldEqual(Strings[i++]);
            sut[i].ShouldEqual(Strings[i++]);
        };

        It should_have_the_inserted_values = () => {
            int i = InsertPoint, j = 0;
            sut[i++].ShouldEqual(insertedValues[j++]);
            sut[i++].ShouldEqual(insertedValues[j++]);
            sut[i++].ShouldEqual(insertedValues[j++]);
            sut[i++].ShouldEqual(insertedValues[j++]);
        };

        It should_have_the_rest_of_the_values_after_inserted_values = () => {
            int i = InsertPoint + insertedValues.Count, j = InsertPoint;
            sut[i++].ShouldEqual(Strings[j++]);
            sut[i++].ShouldEqual(Strings[j++]);
            sut[i++].ShouldEqual(Strings[j++]);
            sut[i++].ShouldEqual(Strings[j++]);

        };
    }
}
