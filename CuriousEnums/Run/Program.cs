using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuriousEnums;

namespace Run {
    class Program {
        static void Main(string[] args) {
 
            Enum2 things = Enum2.Basin;

            Console.WriteLine("things: " + things);

            things = (Enum2) Enum1.Five;

            Console.WriteLine("things is now: " + things);

            things = (Enum2) 134;
            Console.WriteLine("things is now: " + things);

            Console.WriteLine("any key..");
            Console.ReadKey();
        }
    }
}
