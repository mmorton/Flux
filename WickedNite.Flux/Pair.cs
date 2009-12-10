using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public class Pair<TFirst, TSecond>
    {
        public TFirst First { get; set; }
        public TSecond Second { get; set; }

        public Pair() 
        {
        }

        public Pair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }
    }
}
