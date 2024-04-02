using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class F
    {
        public int i1, i2, i3, i4, i5;
        public int Prop1 { get; set; }
        public static F Get() => new F() { i1=1, i2=2, i3=3, i4=4, i5=5, Prop1=6};
    }
}
