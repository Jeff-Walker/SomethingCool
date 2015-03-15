using ExtendedDataStructures.ChunkedArrayList;

namespace ExtendedDataStructuresTest.ChunkedArrayList {
    internal static class Maker {
        public static ChunkedArrayList<T> NewChunkedArrayList<T>(params T[] values) {
            IChunkingStrategy strat = new ExtendingChunkingStrategy(10, 30);
            return NewChunkedArrayList(strat, values);
        }

        static ChunkedArrayList<T> NewChunkedArrayList<T>(IChunkingStrategy strat, params T[] values) {
            var chunkedArrayList = new ChunkedArrayList<T>(strat);
            foreach (var value in values) {
                chunkedArrayList.Add(value);
            }
            return chunkedArrayList;
        }
    }
}