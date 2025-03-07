using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class Currency : Item
    {
        public int Value { get; set; }

        public Currency(string name, int value) : base(name)
        {
            Value = value;
        }
    }
}
