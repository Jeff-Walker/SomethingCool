using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExtendedDataStructures.ChunkedArrayList;

namespace ExtendedDataStructuresTest.ChunkedArrayList.Benchmarks {
    public class Benchmark {
        readonly int _totalCount;

        public Benchmark(int totalCount) {
            _totalCount = totalCount;
        }

        public void AddStrings(IList<string> sut, Stopwatch stopwatch) {
            stopwatch.Start();
            for (var i = 0 ; i < _totalCount ; i++) {
                sut.Add(TestValues.Strings[i]);
            }
            stopwatch.Stop();
        }
    }
}