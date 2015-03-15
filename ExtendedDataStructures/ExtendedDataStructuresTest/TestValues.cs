using System;
using System.Collections.Generic;
using ExtendedDataStructuresTest.ChunkedArrayList;
using Machine.Specifications.Annotations;

namespace ExtendedDataStructuresTest {
    public class SequentialStringGenerator : IStringGenerator {
//        static char _nextChar = 'a';
        static int _nextInt = 0;
        [NotNull]
        public string GenerateString() {
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

    public interface IIntGenerator {
        int GenerateInt();
    }

    public interface IStringGenerator {
        string GenerateString();
    }
    public class SequentialIntGenerator : IIntGenerator {
        static int _nextInt = 1;
        public int GenerateInt() {
//            return TestValues.Random.Next();
            return _nextInt++;
        }
    }
    public class TestValues {
        private static readonly IValues<int> _ints = new BoundlessValues<int>(new SequentialIntGenerator().GenerateInt);
        private static readonly IValues<string> _strings = new BoundlessValues<string>(new SequentialStringGenerator().GenerateString);

        [NotNull]
        public static IValues<string> Strings {
            get { return TestValues._strings; }
        }

        [NotNull]
        public static IValues<int> Ints {
            get { return TestValues._ints; }
        }

      
    }


    public interface IValues<out TValueType> {
        TValueType this[int i] { get; }
    }

    public class BoundlessValues<TValueType> : IValues<TValueType> {
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