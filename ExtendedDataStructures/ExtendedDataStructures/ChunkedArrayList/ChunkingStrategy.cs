namespace ExtendedDataStructures.ChunkedArrayList {
    public interface IChunkingStrategy {
//        int NewChunkSize<T>(ChunkedArrayList<T> originatingChunk);
        int NextChunkSize();
//        int InitialChunkSize { get; set; }
    }

    public class StaticChunkingStrategy : IChunkingStrategy {
        readonly int _chunkSize;

        public StaticChunkingStrategy(int chunkSize) {
            _chunkSize = chunkSize;
        }

        public int NextChunkSize() {
            return _chunkSize;
        }
    }

    public class ExtendingChunkingStrategy : IChunkingStrategy {
        readonly int _initial;
        readonly int _extent;
        bool _first = true;

        public ExtendingChunkingStrategy(int initial, int extent) {
            _initial = initial;
            _extent = extent;
        }

        public int NextChunkSize() {
            if (_first) {
                _first = false;
                return _initial;
            }
            return _extent;
        }
    }

    public class GeometricChunkingStrategy : IChunkingStrategy {
        readonly float _factor;
        int _current;

        public GeometricChunkingStrategy(int initial, float factor) {
            _factor = factor;
            _current = initial;
        }

        public int NextChunkSize() {
            var next = _current;
            _current = (int) (_current * _factor);
            return next;
        }
    }

    public class ExponentialChunkingStrategy : IChunkingStrategy {
        int _current;

        public ExponentialChunkingStrategy(int initial) {
            _current = initial;
        }

        public int NextChunkSize() {
            var next = _current;
            _current *= _current;
            return next;
        }
    }
}