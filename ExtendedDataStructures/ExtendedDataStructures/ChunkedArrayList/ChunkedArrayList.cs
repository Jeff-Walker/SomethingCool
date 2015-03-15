using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExtendedDataStructures.Annotations;

namespace ExtendedDataStructures.ChunkedArrayList {
    public class ChunkedArrayList<T> : IList<T> {
        const int DefaultChunkSize = 10;
        const IChunkingStrategy DefaultChunkingStrategy = null;
        readonly int _chunkSize;
        readonly T[] _members;
        int _end; // logical ending index
        int _start;
        ChunkedArrayList<T> _nextChunk;

        readonly Accounting _accounting;
            
        ChunkedArrayList(int start, Accounting accounting) {
            _start = start;
            _accounting = accounting;

            _chunkSize = _accounting.ChunkingStrategy.NextChunkSize();

            _members = new T[_chunkSize];
            _end = start;
        }

        class Accounting {
            public Accounting(IChunkingStrategy chunkingStrategy) {
                ChunkingStrategy = chunkingStrategy;
            }

            public IChunkingStrategy ChunkingStrategy { get; private set; }
//            volatile int _version;
            public int Version { get; private set; }
            public void NextVersion() {
                Version++;
            }
            public int Count { get; private set; }
            public void Add(int count = 1) {
                Count += count;
            }

            public void Remove(int count = 1) {
                Count -= count;
            }
        }

        ChunkedArrayList([NotNull] ChunkedArrayList<T> copyMe) {
            _members = copyMe._members;
            _nextChunk = copyMe._nextChunk;
            _end = copyMe._end;
            _start = copyMe._start;
            _accounting = copyMe._accounting;
            _chunkSize = _accounting.ChunkingStrategy.NextChunkSize();
        }

//        private ChunkedArrayList(int chunkSize, T[] members, ChunkedArrayList<T> nextChunk, int end, int start) {
//            _chunkSize = chunkSize;
//            _members = members;
//            _nextChunk = nextChunk;
//            _end = end;
//            _start = start;
//        }

        public ChunkedArrayList(int chunkSize) : this(0, new Accounting(new StaticChunkingStrategy(chunkSize))) {}
        public ChunkedArrayList(int chunkSize, int extent) : this(0, new Accounting(new ExtendingChunkingStrategy(chunkSize, extent))) {}
        public ChunkedArrayList() : this(0, new Accounting(new StaticChunkingStrategy(DefaultChunkSize))) { }
        public ChunkedArrayList(IChunkingStrategy chunkingStrategy) : this(0, new Accounting(chunkingStrategy)) { }


        int CurrentSize {
            get { return _end - _start; }
        }

        public void Add(T t) {
//            _accounting.NextVersion();
            if (CanTake(1) && _nextChunk == null) {
                _members[CurrentSize] = t;
                _end++;
                _accounting.Add();
                _accounting.NextVersion();
            } else {
                if (_nextChunk == null) {
                    _nextChunk = new ChunkedArrayList<T>(_end, _accounting);
                }
                _nextChunk.Add(t);
            }
        }

        int PhysicalIndex(int logicalIndex) {
            return logicalIndex - _start;
        }


        public T this[int index] {
            get {
                var translatedIndex = PhysicalIndex(index);
                if (InRange(index)) {
                    return _members[translatedIndex];
                }
                if (_nextChunk != null) {
                    return _nextChunk[index];
                }
                throw new IndexOutOfRangeException(String.Format("index {0}, max is {1}", index, _end));
            }
            set {
                if (InRange(index)) {
                    _accounting.NextVersion();
                    _members[PhysicalIndex(index)] = value;
                } else if (_nextChunk != null) {
                    _nextChunk[index] = value;
                } else {
                    throw new IndexOutOfRangeException(String.Format("index {0}, max is {1}", index, _end));
                }
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return new ChunkedEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public int IndexOf(T item) {
            var index = Array.IndexOf(_members, item);
            if (index < 0 && _nextChunk != null) {
                return _nextChunk.IndexOf(item);
            }
            return index;
        }

//        public void _Insert(int index, T item) {
//            _accounting.NextVersion();
//            if (index == 0) {
//                // inserting in front, this is a special case
//                
//            } else if (ShouldGoHere(index)) {//InRange(index) && CanTake(1)) {
//                
//            } else if (_nextChunk != null && index >= _nextChunk._start) {
////!InRange(index)) {
//                // some other chunk controls that
//                _nextChunk.Insert(index, item);
////                if (_nextChunk != null && index > _end) {
////                    _nextChunk.Insert(index, item);
////                } else {
////                    throw new IndexOutOfRangeException(String.Format("Can't insert at {0}", index));
////                }
//            } else {
//                SplitAt(PhysicalIndex(index));
////                Add(item);
//                _members[PhysicalIndex(index)] = item;
//                _end++;
//                _nextChunk.ShiftIndexBy(1);
//            }
//        }

        public void Insert(int index, T item) {
            var b = TryToInsertInFront(index, item)
                    || TryToTakeIt(index, item)
                    || TryToPassIt(index, item)
                    || Fail(index);
            _accounting.NextVersion();
        }

        bool Fail(int index) {
            throw new IndexOutOfRangeException(String.Format("Can't insert at {0}", index));
        }

        bool TryToPassIt(int index, T item) {
            if (_nextChunk != null) {
                _nextChunk.Insert(index, item);
                return true;
            }
            return false;
        }

        bool TryToInsertInFront(int index, T item) {
            if (index == _start && !CanTake(1)) {
                var newNext = new ChunkedArrayList<T>(this);

                _nextChunk = newNext;
                _end = _start + 1;
                _members[0] = item;
                _nextChunk.ShiftIndexBy(1);
                _accounting.Add();
                return true;
            }
            return false;
        }

        bool TryToTakeIt(int index, T item) {
            if (index >= _start && index < PhysicalEnd) {
                if (CanTake(1) && index != PhysicalEnd) {
                    Array.Copy(_members, PhysicalIndex(index), _members, PhysicalIndex(index) + 1, CurrentSize - PhysicalIndex(index));
                } else {
                    SplitAt(PhysicalIndex(index));
                }
                _members[PhysicalIndex(index)] = item;
                _end++;
                _accounting.Add();
                if (_nextChunk != null) {
                    _nextChunk.ShiftIndexBy(1);
                }
                return true;
            }
            return false;
        }

        int PhysicalEnd {
            get { return _start + _chunkSize; }
        }

        void SplitAt(int index) {
            var newNext = new ChunkedArrayList<T>(index, _accounting);
            Array.Copy(_members, index, newNext._members, 0, CurrentSize - index );
            newNext._start = index;
            newNext._end = _end;
            newNext._nextChunk = _nextChunk;

            _nextChunk = newNext;
            _end = index;
        }

        public StringBuilder DebugToString() {
            var b = new StringBuilder();
            b.Append(Count);
            DoDebugToString(b);
            return b;
        }

        void DoDebugToString(StringBuilder b) {
            b.AppendFormat("[{0}-{1}<", _start, _end - 1);
            for (var i = 0 ; i < CurrentSize ; i++) {
                b.Append("'" + _members[i] + "',");
            }
            b.Append(">];");
            if (_nextChunk != null) {
                b.Append(_nextChunk.DebugToString());
            }
            return;
        }

        public void RemoveAt(int index) {
            if (InRange(index)) {
//                Array.Copy(_members, PhysicalIndex(index), _members, PhysicalIndex(index) + 1, CurrentSize - PhysicalIndex(index));
                //
                Array.Copy(_members, PhysicalIndex(index) + 1, _members, PhysicalIndex(index), CurrentSize - index);
                _end--;
                if ( _nextChunk!= null ){ShiftIndexBy(-1);}
                _accounting.NextVersion();
                _accounting.Remove();
            } else if (_nextChunk != null) {
                _nextChunk.RemoveAt(index);
            } else {
                throw new ArgumentOutOfRangeException("index", index, "out of range");
            }
        }

        public void Clear() {
            _accounting.NextVersion(); 
            _end = _start;
            if (_nextChunk != null) {
                _nextChunk.Clear();
            }
        }

        public bool Contains(T item) {
            return _members.Contains(item) || _nextChunk == null || _nextChunk.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            if (array == null) {
                throw new ArgumentNullException("array");
            }
            if (arrayIndex < 0) {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }
            if (array.Length + arrayIndex < Count) {
                throw new ArgumentException("contents won't fit into array", "array");
            }

            DoCopyTo(array, arrayIndex);
//            var arrayStart = arrayIndex;
//            do {
//                Array.Copy(_members, 0, array, arrayStart, CurrentSize);
//                arrayStart += CurrentSize;
//            }
        }

        void DoCopyTo(T[] array, int arrayIndex) {
            Array.Copy(_members, 0, array, arrayIndex, CurrentSize);
            if (_nextChunk != null) {
                _nextChunk.DoCopyTo(array, arrayIndex + CurrentSize);
            }
        }


        public bool Remove(T item) {
            var index = IndexOf(item);
            if (index < 0) {
                return false;
            }
            RemoveAt(index);
            _accounting.NextVersion();
            return true;
        }

        public int Count {
            get { return _accounting.Count; }
//            get { return CurrentSize + (_nextChunk == null ? 0 : _nextChunk.Count); }
            //get { return _nextChunk == null ? _end : _nextChunk.Count; }
        }



        public bool IsReadOnly {
            get { return false; }
        }

        bool CanTake(int i) {
            return _members.Length >= (CurrentSize) + i;
        }

        bool InRange(int i) {
            return i >= _start && i < _end;
        }

        void ShiftIndexBy(int increase) {
            _start += increase;
            _end += increase;
            if (_nextChunk != null) {
                _nextChunk.ShiftIndexBy(increase);
            }
        }

        class ChunkedEnumerator<TE> : IEnumerator<TE> {
            readonly ChunkedArrayList<TE> _parent;
            IEnumerator<TE> _nextEnumerator;
            readonly IEnumerator _memberEnumerator;
            TE _current;
            readonly int _version;
            bool _lastMove;
            bool _isDisposed;

            public ChunkedEnumerator(ChunkedArrayList<TE> parent) {
                if (parent == null) {
                    throw new ArgumentNullException();
                }
                _parent = parent;
                _memberEnumerator = _parent._members.GetEnumerator();
                _version = _parent._accounting.Version;
            }

            void CheckState() {
                if (_version != _parent._accounting.Version) {
                    throw new InvalidOperationException("Invalid enumerator: Parent collection has changed");
                }
                if (_isDisposed) {
                    throw new InvalidOperationException("Invalid enumerator: Is disposed");
                }
            }

            public bool MoveNext() {
                CheckState();
                var moved = _memberEnumerator.MoveNext();
                if (!moved) {
                    if (_parent._nextChunk != null) {
                        if (_nextEnumerator == null) {
                            _nextEnumerator = _parent._nextChunk.GetEnumerator();
                        }
                        moved = _nextEnumerator.MoveNext();
                        if (moved) {
                            Current = _nextEnumerator.Current;
                        }
                    }
                } else {
                    Current = (TE) _memberEnumerator.Current;
                }
                _lastMove = moved;
                return moved;
            }

            public void Reset() {
                CheckState();
                _memberEnumerator.Reset();
                if (_nextEnumerator != null) {
                    _nextEnumerator.Reset();
                }
            }

            object IEnumerator.Current {
                get { return Current; }
            }


            public TE Current {
                get {
                    CheckInitialized();
                    return _current;
                }
                private set { _current = value; }
            }

            void CheckInitialized() {
                if (!_lastMove) {
                    throw new InvalidOperationException("Invalid enumerator: no current element");
                }
            }

            public void Dispose() {
                _isDisposed = true;
            }
        }


        internal int ChunkSize { get { return _chunkSize; } }
        internal int Start { get { return _start; } }
        internal int End { get { return _end; } }
        internal ChunkedArrayList<T> NextChunk { get { return _nextChunk; } }
        internal T[] Members { get { return _members; } }
    }

    public class DisplayChunkedArrayList<T> {
        readonly ChunkedArrayList<T> _chunk;

        public DisplayChunkedArrayList(ChunkedArrayList<T> chunk) {
            _chunk = chunk;
        }

        public string Display() {
            var writer = new StringWriter();
            Display(writer);
            return writer.GetStringBuilder().ToString();
            
        }

        public void Display(TextWriter writer) {
            writer.WriteLine("ChunkedArrayList; chunk size {0}, count {1}", _chunk.ChunkSize, _chunk.Count);
            var i = 0;
            var currentChunk = _chunk;
            do {
                DisplayChunkInfo(i, currentChunk, writer);
                i++;
                currentChunk = currentChunk.NextChunk;
            } while (currentChunk != null);
        }

        static void DisplayChunkInfo(int i, ChunkedArrayList<T> cc, TextWriter writer) {
            writer.Write("-> {0}: {1}-{2}["//"{3}]"
                , i, cc.Start, cc.End - 1
//                , string.Join(",", cc.Members)
                );
            for (int l = 0 ; l < cc.ChunkSize ; l++) {
                if (l != 0) {
                    writer.Write(",");
                }
                string str;
                var value = cc.Members[l];
                str = value == null ? "" : value.ToString();
                if (l >= cc.End - cc.Start) {
                    str = "_" + str;
                }
                writer.Write(str);
            }
            writer.WriteLine("]");
        }
    }
}