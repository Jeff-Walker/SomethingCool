using System;
using System.Collections.Generic;
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
