using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedDataStructures.ChunkedArrayList;
using ExtendedDataStructuresTest;
using ExtendedDataStructuresTest.ChunkedArrayList.Benchmarks;

namespace ConsoleApplication1 {
    class Program {
        static ChunkedArrayList<string> sut;
        static string insertedValue;
        const int NumberOfElements = 6;
        const int InsertPoint = 2;

        static void Main(string[] args) {
            const int totalCount = 1000000;
            const int iterations = 5;

            DoDebuggingAction();
            return;

            Console.WriteLine("warming up");
            WarmUp();


//            Console.WriteLine("static @1000");
//            DoIterations(() => MakeList(new StaticChunkingStrategy(1000)), DoMeasured, totalCount, iterations);
//            Console.WriteLine("static @10000");
//            DoIterations(() => MakeList(new StaticChunkingStrategy(10000)), DoMeasured, totalCount, iterations);
//            Console.WriteLine("static @100000");
//            DoIterations(() => MakeList(new StaticChunkingStrategy(100000)), DoMeasured, totalCount, iterations);

//            DoAdds(totalCount, iterations);
            DoInserts(totalCount, iterations);
            Console.WriteLine("...");
            Console.ReadKey();
        }

        class RandomInsertionPoints : BoundlessValues<int> {
            static readonly Random Random = new Random();
            static int _currentMax = 1000;
            static readonly Func<int> NextFunc = () => Random.Next(_currentMax++);

            public RandomInsertionPoints(int initalCount) : base(NextFunc) {
                _currentMax = initalCount;
            }
        }

        class RandomValues : BoundlessValues<int> {
            static readonly Random Random = new Random();
            static readonly Func<int> NextFunc = () => Random.Next();
            public RandomValues() : base(NextFunc) {}
        }

        class SequentialValues : BoundlessValues<int> {
            static int _value = 1;
            static readonly Func<int> NextFunc = () => _value++;
            public SequentialValues() : base(NextFunc) {}
        }
        static IList<int> MakeListAndPrepopulate(IChunkingStrategy cs, IValues<int> values, int initalCount) {
            var list = MakeList<int>(cs);
            Prepopulate(list, values, initalCount);
            return list;
        }
        static void Prepopulate(ICollection<int> list, IValues<int> values, int initalCount) {
            for (var i = 0 ; i < initalCount ; i++) {
                list.Add(values[i]);
            }
        }
        static void DoInserts(int totalCount, int iterations) {
            const int initialCount = 1000;
//            var values = new RandomValues();
            var values = new SequentialValues();
            var insertPoints = new RandomInsertionPoints(initialCount);
            Action<IList<int>, int> measuredAction = (list, i1) => list.Insert(insertPoints[i1], values[i1]);
            Console.WriteLine("\nInserts");
            foreach (var kvp in _stratList) {
                Console.WriteLine(kvp.Key);
                var chunkingStrategy = kvp.Value;
                DoIterations(() => MakeListAndPrepopulate(chunkingStrategy, values, initialCount), measuredAction, totalCount, iterations);
            }
        }

        static void DoDebuggingAction() {
            var list = MakeListAndPrepopulate(new StaticChunkingStrategy(20), new SequentialValues(), 20);
            
            list.Insert(4, -1);
        }


        static IList<KeyValuePair<string, IChunkingStrategy>> _stratList = new[] {
            new KeyValuePair<string, IChunkingStrategy>("geo @1000 x 1.5", new GeometricChunkingStrategy(1000, 1.5f)),
            new KeyValuePair<string, IChunkingStrategy>("geo @1000 x 2", new GeometricChunkingStrategy(1000, 2f)),
            new KeyValuePair<string, IChunkingStrategy>("geo @1000 x 3", new GeometricChunkingStrategy(1000, 3f)),
            new KeyValuePair<string, IChunkingStrategy>("exp @10", new ExponentialChunkingStrategy(10)),
            new KeyValuePair<string, IChunkingStrategy>("exp @100", new ExponentialChunkingStrategy(100)),
            new KeyValuePair<string, IChunkingStrategy>("exp @1000", new ExponentialChunkingStrategy(1000)),
        };
        static void DoAdds(int totalCount, int iterations) {
            Action<IList<string>, int> measuredAction = (list, i1) => list.Add(TestValues.Strings[i1]);
            Console.WriteLine("\nAdds");
            Console.WriteLine("geo @1000 x 1.5");
            DoIterations(() => MakeList<string>(new GeometricChunkingStrategy(1000, 1.5f)), measuredAction, totalCount, iterations);
            Console.WriteLine("geo @1000 x 2");
            DoIterations(() => MakeList<string>(new GeometricChunkingStrategy(1000, 2f)), measuredAction, totalCount, iterations);
            Console.WriteLine("geo @1000 x 3");
            DoIterations(() => MakeList<string>(new GeometricChunkingStrategy(1000, 3f)), measuredAction, totalCount, iterations);

            Console.WriteLine("exp @10");
            DoIterations(() => MakeList<string>(new ExponentialChunkingStrategy(10)), measuredAction, totalCount, iterations);
            Console.WriteLine("exp @100");
            DoIterations(() => MakeList<string>(new ExponentialChunkingStrategy(100)), measuredAction, totalCount, iterations);
            Console.WriteLine("exp @1000");
            DoIterations(() => MakeList<string>(new ExponentialChunkingStrategy(1000)), measuredAction, totalCount, iterations);

            Console.WriteLine("list");
            DoIterations(() => new List<string>(), measuredAction, totalCount, iterations);
            Console.WriteLine("list @100");
            DoIterations(() => new List<string>(100), measuredAction, totalCount, iterations);
            Console.WriteLine("list @1000");
            DoIterations(() => new List<string>(1000), measuredAction, totalCount, iterations);
        }

        static IList<T> MakeList<T>(IChunkingStrategy chunkingStrategy) {
            return new ChunkedArrayList<T>(chunkingStrategy);
        }

        static void WarmUp() {
            new Benchmark(50000).AddStrings(new ChunkedArrayList<string>(), new Stopwatch());
        }

        static void DoIterations<T>(Func<IList<T>> makeList, Action<IList<T>, int> measuredAction, int totalCount, int iterations) {
            for (var i = 0 ; i < iterations ; i++) {
                var list = makeList();

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                for (var i1 = 0 ; i1 < totalCount ; i1++) {
//                    DoMeasured(list, i1);
                    measuredAction(list, i1);
                }
                stopwatch.Stop();
                Console.WriteLine("{0}: @{1}: {2}", i, totalCount, stopwatch.Elapsed);
            }
        }

        static void zMain(string[] args) {

            sut = new ChunkedArrayList<string>(4);
            int i;
            for (i = 0 ; i < NumberOfElements ; i++) {
                sut.Add(TestValues.Strings[i]);
            }

            insertedValue = "inserting";

            var d = new DisplayChunkedArrayList<string>(sut);
            d.Display();

            Console.WriteLine("inserting '" + insertedValue + "'");

            sut.Insert(InsertPoint, insertedValue);

            d.Display();
            Console.ReadKey();
        }
    }
}
