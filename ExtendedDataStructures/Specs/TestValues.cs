using System;
using System.Collections.Generic;

namespace ExtendedDataStructuresSpecifications {
    public class TestValues {
        private static readonly Random Random = new Random();

        private static readonly IValues<int> _ints = new BoundlessValues<int>(() => Random.Next());
        private static readonly IValues<string> _strings = new BoundlessValues<string>(() => "" + Random.Next());

        protected static IValues<string> Strings {
            get { return _strings; }
        }

        protected static IValues<int> Ints {
            get { return _ints; }
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