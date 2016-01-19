using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.AlgorytmGenetyczny
{
    public class Chromosom
    {
        public int Value { get; set; }
        public int SurivatePoints { get; set; }
        public int Weight { get; set; }

        public override string ToString()
        {
            return Convert.ToString(Value, 2); // 2 mean to binary system
        }
    }
}
