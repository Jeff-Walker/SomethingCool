using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedDataStructures.BiDictionary;
using Machine.Specifications;

namespace Specs {
    public static class Test {
        private const int ItemCount = 10;
        static Test() {
            var random = new Random();
            Strings = new List<string>(ItemCount);
            for (var i = 0 ; i < ItemCount ; i++) {
                Strings.Add("" + random.Next());
            }

            Ints = new List<int>(ItemCount);
            for (var i = 0 ; i < ItemCount ; i++) {
                Ints.Add(random.Next());
            }
        }

        public static IList<string> Strings { get; private set; }
        public static IList<int> Ints { get; private set; }

     
    }
    [Subject(typeof(BiDictionary<int,string>))]
    public class CreatingABiDictionary {
        private static IBiDictionary<int, string> sut;

        Establish context = () => {  };
        Because of = () => {
            sut = new BiDictionary<int, string> {
                {Test.Ints[0], Test.Strings[0]},
                {Test.Ints[1], Test.Strings[1]},
            };

        };

        It should_have_first_mapping = () => sut[Test.Ints[0]].ShouldEqual(Test.Strings[0]);
        It should_have_the_second_mapping = () => sut[Test.Ints[1]].ShouldEqual(Test.Strings[1]);

        It inverse_should_have_the_first_mapping_reversed =
                () => sut.Inverse[Test.Strings[0]].ShouldEqual(Test.Ints[0]);
        It inverse_should_have_the_second_mapping_reversed =
                () => sut.Inverse[Test.Strings[1]].ShouldEqual(Test.Ints[1]);

        It inverse_of_inverse_should_be_same_as_original =
                () => sut.Inverse.Inverse.ShouldBeTheSameAs(sut);
    }

    [Subject(typeof (IBiDictionary<int, string>))]
    public class When_Removing_From_BiDictionary {
        private static IBiDictionary<int, string> sut;
        Establish context = () => {
            sut = new BiDictionary<int, string> {
                {Test.Ints[0], Test.Strings[0]},
                {Test.Ints[1], Test.Strings[1]},
            };
        };

        private Because of = () => sut.Remove(Test.Ints[0]);

        It should_only_have_one_entry = () => { sut.Count.ShouldEqual(1); };
        It should_contain_only_the_remaining_entry = () => { sut.ShouldContainOnly(new KeyValuePair<int, string>(Test.Ints[1],Test.Strings[1])); };
    }
}
