using System;
using System.Collections.Generic;
using Machine.Specifications.Annotations;

namespace ExtendedDataStructuresTest {
    public static class StringGenerator {
//        static char _nextChar = 'a';
        static int _nextInt = 0;
        [NotNull]
        public static string GenerateString() {
//            return "" + TestValues.Random.Next();
//            return new String(new []{_nextChar++});
            return GetExcelStyleSequential(_nextInt++);
        }

        [NotNull]
        static String GetExcelStyleSequential(int column) {
            var col = Convert.ToString((char) ('a' + (column % 26)));
            while (column >= 26) {
                column = (column / 26) - 1;
                col = Convert.ToString((char) ('a' + (column % 26))) + col;
            }
            return col;
        }
    }
    public static class IntGenerator {
        static int _nextInt = 1;
        public static int GenerateInt() {
//            return TestValues.Random.Next();
            return _nextInt++;
        }
    }
    public class TestValues {
        internal static readonly Random Random = new Random();
        private static readonly IValues<int> _ints = new BoundlessValues<int>(IntGenerator.GenerateInt);

        private static readonly IValues<string> _strings = new BoundlessValues<string>(StringGenerator.GenerateString);

        [NotNull]
        protected static IValues<string> Strings {
            get { return TestValues._strings; }
        }

        [NotNull]
        protected static IValues<int> Ints {
            get { return TestValues._ints; }
        }

        protected static KeyValuePair<TK,TV> NewKvp<TK,TV>(TK key, TV value) {
            return new KeyValuePair<TK,TV>(key, value);
        }
    }


    public interface IValues<out TValueType> {
        TValueType this[int i] { get; }
    }

    internal class BoundlessValues<TValueType> : IValues<TValueType> {
        private readonly Func<TValueType> _nextValue;
        private readonly IList<TValueType> _values = new List<TValueType>();

        public BoundlessValues(Func<TValueType> nextValue) {
            _nextValue = nextValue;
        }

        public TValueType this[int i] {
            get {
                while (_values.Count <= i) {
                    _values.Add(_nextValue());
                }
                return _values[i];
            }
        }
    }
}