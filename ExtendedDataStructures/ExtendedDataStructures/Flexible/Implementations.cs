using System.Collections.Generic;

namespace ExtendedDataStructures.Flexible {
    public class MultiListDictionary<TK, TV> : FlexiMultiDictionary<TK, TV, IList<TV>> {
        protected override IList<TV> ProvideNewCollection() {
            return new List<TV>();
        }

        protected override void BulkAdd(IList<TV> valueCollection, IEnumerable<TV> toAdd) {
            ((List<TV>)valueCollection).AddRange(toAdd);
        }
    }

    public class MultiSetDictionary<TK, TV> : FlexiMultiDictionary<TK, TV, ISet<TV>> {
        protected override ISet<TV> ProvideNewCollection() {
            return new HashSet<TV>();
        }

        protected override void BulkAdd(ISet<TV> valueCollection, IEnumerable<TV> toAdd) {
            valueCollection.UnionWith(toAdd);
        }
    }
}