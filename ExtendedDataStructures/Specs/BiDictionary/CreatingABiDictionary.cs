using System;
using ExtendedDataStructures.BiDictionary;
using Machine.Specifications;

namespace ExtendedDataStructuresSpecifications.BiDictionary {
    [Subject(typeof(BiDictionary<int,string>))]
    public class CreatingABiDictionary : TestValues {
        private static IBiDictionary<int, string> sut;

        Establish context = () => {  };
        Because of = () => {
            sut = new BiDictionary<int, string> {
                {Ints[0], Strings[0]},
                {Ints[1], Strings[1]},
            };

        };

        It should_have_first_mapping = () => sut[Ints[0]].ShouldEqual(Strings[0]);
        It should_have_the_second_mapping = () => sut[Ints[1]].ShouldEqual(Strings[1]);

        It inverse_should_have_the_first_mapping_reversed =
                () => sut.Inverse[Strings[0]].ShouldEqual(Ints[0]);
        It inverse_should_have_the_second_mapping_reversed =
                () => sut.Inverse[Strings[1]].ShouldEqual(Ints[1]);

        It inverse_of_inverse_should_be_same_as_original =
                () => sut.Inverse.Inverse.ShouldBeTheSameAs(sut);
    }

    [Subject(typeof (BiDictionary<int, string>))]
    public class When_Removing_From_BiDictionary : TestValues {
        private static IBiDictionary<int, string> sut;
        Establish context = () => {
            sut = new BiDictionary<int, string> {
                {Ints[0], Strings[0]},
                {Ints[1], Strings[1]},
            };
        };

        Because of = () => sut.Remove(Ints[0]);

        It should_only_have_one_entry = () => sut.Count.ShouldEqual(1);
        It should_contain_only_the_remaining_entry = () => sut.ShouldContainOnly(NewKvp(Ints[1],Strings[1]));
        It inverse_should_have_one_entry = () => sut.Inverse.Count.ShouldEqual(1);
        It inverse_should_contain_the_remaining_entry =()=> sut.Inverse.ShouldContainOnly(NewKvp(Strings[1], Ints[1]));
    }

    [Subject(typeof (BiDictionary<int, string>))]
    public class When_Adding_Duplicate_Value : TestValues {
        private static IBiDictionary<int, string> sut;
        private static Exception exception;

        private Establish context = () => {
            sut = new BiDictionary<int, string> {
                {Ints[0], Strings[0]},
                {Ints[1], Strings[1]},
            };
        };

        Because of = () => exception = Catch.Only<ArgumentException>(() => sut.Add(Ints[2], Strings[0]));

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }
    [Subject(typeof (BiDictionary<int, string>))]
    public class When_Force_Adding_Duplicate_Value : TestValues {
        private static IBiDictionary<int, string> sut;

        private Establish context = () => {
            sut = new BiDictionary<int, string> {
                {Ints[0], Strings[0]},
                {Ints[1], Strings[1]},
            };
        };

        Because of = () => sut.ForceAdd(Ints[2], Strings[0]);

        It should_have_two_entries = () => sut.Count.ShouldEqual(2);
        It should_have_the_forceAdded_mapping = () => sut.ShouldContain(NewKvp(Ints[2], Strings[0]));
        It should_have_the_mapping_that_was_not_affected = () => sut.ShouldContain(NewKvp(Ints[1], Strings[1]));
        It should_not_have_the_mapping_that_caused_the_collision = () => sut.ShouldNotContain(NewKvp(Ints[0], Strings[0]));
        It should_not_have_the_key_of_the_removed_mapping = () => sut.ContainsKey(Ints[0]).ShouldBeFalse();

        It inverse_should_have_two_entries = () => sut.Inverse.Count.ShouldEqual(2);
        It inverse_should_have_the_forceAdded_mapping = () => sut.Inverse.ShouldContain(NewKvp(Strings[0], Ints[2]));
        It inverse_should_have_the_mapping_that_was_not_affected = () => sut.Inverse.ShouldContain(NewKvp(Strings[1], Ints[1]));
        It inverse_should_not_have_the_mapping_that_caused_the_collision = () => sut.Inverse.ShouldNotContain(NewKvp(Strings[0], Ints[0]));
    }

    [Subject(typeof (BiDictionary<int, string>))]
    public class When_Adding_via_Inverse : TestValues {
        private static IBiDictionary<int, string> sut;

        private Establish context = () => {
            sut = new BiDictionary<int, string>();
        };

        Because of = () => sut.Inverse.Add(Strings[0], Ints[0]);

        It should_have_the_mapping_in_original = () => sut.ShouldContainOnly(NewKvp(Ints[0], Strings[0]));
        It should_have_the_mapping_in_the_inverse = () => sut.Inverse.ShouldContainOnly(NewKvp(Strings[0], Ints[0]));

    }
    [Subject(typeof(BiDictionary<int, string>))]
    public class When_Adding_Duplicate_Value_Through_inverse : TestValues {
        private static IBiDictionary<int, string> sut;
        private static Exception exception;

        private Establish context = () => {
            sut = new BiDictionary<int, string> {
                {Ints[0], Strings[0]},
                {Ints[1], Strings[1]},
            };
        };

        Because of = () => exception = Catch.Only<ArgumentException>(() => sut.Inverse.Add(Strings[2], Ints[0]));

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
    }
    [Subject(typeof(BiDictionary<int, string>))]
    public class When_Force_Adding_Duplicate_Value_Through_Inverse : TestValues {
        private static IBiDictionary<int, string> sut;

        private Establish context = () => {
            sut = new BiDictionary<int, string> {
                {Ints[0], Strings[0]},
                {Ints[1], Strings[1]},
            };
        };

        Because of = () => sut.Inverse.ForceAdd(Strings[2], Ints[0]);

        It should_have_two_entries = () => sut.Inverse.Count.ShouldEqual(2);
        It should_have_the_forceAdded_mapping = () => sut.Inverse.ShouldContain(NewKvp(Strings[2], Ints[0]));
        It should_have_the_mapping_that_was_not_affected = () => sut.Inverse.ShouldContain(NewKvp(Strings[1], Ints[1]));
        It should_not_have_the_mapping_that_caused_the_collision = () => sut.Inverse.ShouldNotContain(NewKvp(Strings[0], Ints[0]));
        It should_not_have_the_key_of_the_removed_mapping = () => sut.Inverse.ContainsKey(Strings[0]).ShouldBeFalse();

        It inverse_should_have_two_entries = () => sut.Inverse.Inverse.Count.ShouldEqual(2);
        It inverse_should_have_the_forceAdded_mapping = 
                () => sut.Inverse.Inverse.ShouldContain(NewKvp(Ints[0], Strings[2]));
        It inverse_should_have_the_mapping_that_was_not_affected = 
                () => sut.Inverse.Inverse.ShouldContain(NewKvp(Ints[1], Strings[1]));
        It inverse_should_not_have_the_mapping_that_caused_the_collision = 
                () => sut.Inverse.Inverse.ShouldNotContain(NewKvp(Ints[0], Strings[0]));
    }
}
