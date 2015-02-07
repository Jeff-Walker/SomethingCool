﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedDataStructures.ChunkedArrayList {
    public class ChunkedArrayList<T> : IList<T> {
        private const int DefaultChunkSize = 10;
        private readonly int _chunkSize;
        private readonly T[] _members;
        private int _end; // logical ending index
        private ChunkedArrayList<T> _nextChunk;
        private int _start;
        private volatile int _version;

        private ChunkedArrayList(int start, int chunkSize) {
            _start = start;
            _chunkSize = chunkSize;

            _members = new T[chunkSize];
            _end = start;
        }

        private ChunkedArrayList(ChunkedArrayList<T> copyMe) {
            _chunkSize = copyMe._chunkSize;
            _members = copyMe._members;
            _nextChunk = copyMe._nextChunk;
            _end = copyMe._end;
            _start = copyMe._start;
        }

//        private ChunkedArrayList(int chunkSize, T[] members, ChunkedArrayList<T> nextChunk, int end, int start) {
//            _chunkSize = chunkSize;
//            _members = members;
//            _nextChunk = nextChunk;
//            _end = end;
//            _start = start;
//        }

        public ChunkedArrayList(int chunkSize) : this(0, chunkSize) {}
        public ChunkedArrayList() : this(0, DefaultChunkSize) {}

        private int CurrentSize {
            get { return _end - _start; }
        }

        public void Add(T t) {
            _version++;
            if (CanTake(1) && _nextChunk == null) {
                _members[_end++] = t;
            }
            else {
                if (_nextChunk == null) {
                    _nextChunk = new ChunkedArrayList<T>(_end, _chunkSize);
                }
                _nextChunk.Add(t);
            }
        }

        public T this[int index] {
            get {
                if (InRange(index)) {
                    return _members[CurrentSize];
                }
                if (_nextChunk != null) {
                    return _nextChunk[index];
                }
                throw new IndexOutOfRangeException(String.Format("index {0}, max is {1}", index, _end));
            }
            set {
                _version++;
                if (InRange(index)) {
                    _members[index] = value;
                }
                else if (_nextChunk != null) {
                    _nextChunk[index] = value;
                }
                throw new IndexOutOfRangeException(String.Format("index {0}, max is {1}", index, _end));
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return new ChunkedEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public int IndexOf(T item) {
            int index = Array.IndexOf(_members, item);
            if (index < 0 && _nextChunk != null) {
                return _nextChunk.IndexOf(item);
            }
            return index;
        }

        public void Insert(int index, T item) {
            _version++;
            if (index == 0) {
                // inserting in front, this is a special case
                _nextChunk = new ChunkedArrayList<T>(this);
                _start = 0;
                _end = 1;
                _members[0] = item;
                _nextChunk.ShiftIndexBy(1);
            }
            else if (index >= _nextChunk._start) {//!InRange(index)) {
                // some other chunk controls that
                if (_nextChunk != null && index > _end) {
                    _nextChunk.Insert(index, item);
                }
                else {
                    throw new IndexOutOfRangeException(String.Format("Can't insert at {0}", index));
                }
            }
            else {
                SplitAt(index);
                Add(item);
            }
        }

        private void SplitAt(int index) {
            var newNext = new ChunkedArrayList<T>(_chunkSize);
            Array.Copy(_members, index + 1, newNext._members, 0, _end - index + 1);
            newNext._start = index + 1;
            newNext._end = _end;
            newNext._nextChunk = _nextChunk;

            _nextChunk = newNext;
            _end = index;
            
        }

        public void RemoveAt(int index) {
            _version++;
            throw new NotImplementedException();
        }

        public void Clear() {
            _version++;
            _end = _start;
            if (_nextChunk != null) {
                _nextChunk.Clear();
            }
        }

        public bool Contains(T item) {
            return _members.Contains(item) || _nextChunk == null || _nextChunk.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(T item) {
            _version++;
            var index = IndexOf(item);
            if (index < 0) {
                return false;
            }
            RemoveAt(index);
            return true;
        }

        public int Count {
            get { return _nextChunk == null ? _end : _nextChunk.Count; }
        }


        public bool IsReadOnly {
            get { return false; }
        }

        private bool CanTake(int i) {
            return _members.Length > (CurrentSize) + i;
        }

        private bool InRange(int i) {
            return i >= _start && i < _end;
        }

        private void ShiftIndexBy(int increase) {
            _start += increase;
            _end += increase;
            if (_nextChunk != null) {
                _nextChunk.ShiftIndexBy(increase);
            }
        }

        public class ChunkedEnumerator<TE> : IEnumerator<TE> {
            private readonly ChunkedArrayList<TE> _parent;
            private IEnumerator<TE> _nextEnumerator;
            private readonly IEnumerator<TE> _memberEnumerator;
            private TE _current;
            private readonly int _version;
            private bool _lastMove;
            private bool _isDesposed;

            public ChunkedEnumerator(ChunkedArrayList<TE> parent) {
                _parent = parent;
                _memberEnumerator = (IEnumerator<TE>) _parent._members.GetEnumerator();
                _version = _parent._version;
            }

            private void CheckState() {
                if (_version != _parent._version) {
                    throw new InvalidOperationException("Invalid enumerator: Parent collection has changed");
                }
                if (_isDesposed) {
                    throw new InvalidOperationException("Invalid enumerator: Is desposed");
                }
            }

            public bool MoveNext() {
                CheckState();
                var moved = _memberEnumerator.MoveNext();
                if (!moved && _parent._nextChunk != null) {
                    _nextEnumerator = _parent._nextChunk.GetEnumerator();
                    moved = _nextEnumerator.MoveNext();
                    Current = _nextEnumerator.Current;
                }
                else {
                    Current = _memberEnumerator.Current;
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

            private void CheckInitialized() {
                if (!_lastMove) {
                    throw new InvalidOperationException("Invalid enumerator: no current element");
                }
            }

            public void Dispose() {
                _isDesposed = true;
            }
        }

    }

}