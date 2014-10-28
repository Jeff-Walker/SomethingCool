using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuriousEnums;

namespace Run {
    class Program {
        class ThingEnumeration : TypeSafeEnumeration<int> {
            public static ThingEnumeration Tree = new ThingEnumeration(1, "Tree");
            public static ThingEnumeration Cliff = new ThingEnumeration(2, "Cliff");
            public static ThingEnumeration Basin = new ThingEnumeration(3, "Basin");
            private ThingEnumeration(int value, string display) : base(value, display) {}
        }
        static void Main(string[] args) {
 
            Enum2 things = Enum2.Basin;

            Console.WriteLine("things: " + things);

            things = (Enum2) Enum1.Five;

            Console.WriteLine("things is now: " + things);

            things = (Enum2) 134;
            Console.WriteLine("things is now: " + things);

            ThingEnumeration thing = ThingEnumeration.Basin;

            Console.WriteLine("thing is: " + thing);

            Console.WriteLine("any key..");
            Console.ReadKey();
        }
    }
}
