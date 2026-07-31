using System;
using System.Collections.Generic;
using System.Text;

namespace LAB3
{
    internal class Swapper
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
