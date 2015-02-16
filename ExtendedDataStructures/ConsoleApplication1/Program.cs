using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedDataStructures.ChunkedArrayList;
using ExtendedDataStructuresTest;

namespace ConsoleApplication1 {
    class Program : TestValues{
        static ChunkedArrayList<string> sut;
        static string insertedValue;
        const int NumberOfElements = 6;
        const int InsertPoint = 2;
        static void Main(string[] args) {

            sut = new ChunkedArrayList<string>(4);
            int i;
            for (i = 0 ; i < NumberOfElements ; i++) {
                sut.Add(Strings[i]);
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
