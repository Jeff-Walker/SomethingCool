using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedDataStructures {
    public class RegularDictionary {
        private readonly IDictionary<string, IEnumerable<int>> _dictionary;

        public RegularDictionary() {
            _dictionary = new Dictionary<string, IEnumerable<int>>();
        }

        public void Add(string key, IEnumerable<int> value) {
            IEnumerable<int> values;
            if (!_dictionary.TryGetValue(key, out values)) {
                values = new List<int>();
            }
            ((List<int>)values).AddRange(value);
        }

        public void Add(string key, int value) {
            IEnumerable<int> values;
            if (!_dictionary.TryGetValue(key, out values)) {
                values = new List<int>();
            }
            ((List<int>)values).Add(value);
        }
    }
    public class BetterRegularDictionary {
        private readonly IDictionary<string, IEnumerable<int>> _dictionary;

        public BetterRegularDictionary() {
            _dictionary = new Dictionary<string, IEnumerable<int>>();
        }

        public void Add(string key, IEnumerable<int> value) {
            IEnumerable<int> values;
            if (!_dictionary.TryGetValue(key, out values)) {
                values = new List<int>();
            }
            ((List<int>)values).AddRange(value);
        }

        public void Add(string key, int value) {
            GetValueList(key).Add(value);
        }

        private IList<int> GetValueList(string key) {
            IEnumerable<int> values;
            if (!_dictionary.TryGetValue(key, out values)) {
                values = new List<int>();
            }
            return (IList<int>) values;
        }
    }
}
