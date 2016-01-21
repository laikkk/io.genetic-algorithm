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

        public int NumberOfBits { get; set; }

        public override string ToString()
        {
            return String.Format("{0}-(Fitness={1}, Weight={2})", AsAStringBits(), SurivatePoints, Weight); ;
        }

        public string AsAStringBits()
        {
            string chromosomAsStringOfBits = Convert.ToString(Value, 2); // 2 mean to binary system
            while (chromosomAsStringOfBits.Length < NumberOfBits)
            {
                chromosomAsStringOfBits = chromosomAsStringOfBits.Insert(0, "0");
            }
            return chromosomAsStringOfBits;
        }

        public void Fitness(int WeightLimit)
        {
            var productTable = Things.products;

            string s = ToString();
            var foundIndexes = new List<int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '1') foundIndexes.Add(i);
            }

            int sumSurvivePoints = 0;
            int sumOfWeight = 0;
            foreach (int indexOfOneInChromosom in foundIndexes)
            {
                sumSurvivePoints += productTable[indexOfOneInChromosom].SurvivalPoints;
                sumOfWeight += productTable[indexOfOneInChromosom].Weight;
            }

            SurivatePoints = sumSurvivePoints;
            Weight = sumOfWeight;

            SurivatePoints = (sumOfWeight > WeightLimit) ? 0 : sumSurvivePoints;
        }
    }
}
