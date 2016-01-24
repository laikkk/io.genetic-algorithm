using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UI_Genetic_Algorythm
{
    public class Chromosom
    {
        public int Value { get; set; }
        public int SurivatePoints { get; set; }
        public int Weight { get; set; }
        public int NumberOfBits { get; set; }

        public BitArray Bits { get; set; }
        private readonly Random _random;

        // init new chromosom
        public Chromosom(int chromosomLength,Random random = null)
        {
            _random = random;
            Bits = new BitArray(chromosomLength);
            for (int i = 0; i < chromosomLength; i++)
            {
                Bits[i] = _random.Next(0, 11) % 2 == 0;
                //Console.Write((b[i] ? 1 : 0) + "");
            }
        }

        // use in cross
        public Chromosom(BitArray newBits)
        {
            Bits = newBits;
            Weight = 0;
            SurivatePoints = 0;
        }

        public override string ToString()
        {
            return String.Format("{0}-(Fitness={1}, Weight={2})", AsAStringBits(), SurivatePoints, Weight); ;
        }

        public string AsAStringBits()
        {
            //string chromosomAsStringOfBits = Convert.ToString(Value, 2); // 2 mean to binary system
            //while (chromosomAsStringOfBits.Length < NumberOfBits)
            //{
            //    chromosomAsStringOfBits = chromosomAsStringOfBits.Insert(0, "0");
            //}
            //return chromosomAsStringOfBits;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Bits.Length; i++)
            {
                sb.Append((Bits[i]) ? '1' : '0');
            }

            return sb.ToString();
        }

        public void Fitness(int weightLimit)
        {

            //string s = ToString();
            //var foundIndexes = new List<int>();
            int sumSurvivePoints = 0;
            int sumOfWeight = 0;
            //for (int i = 0; i < s.Length; i++)
            //{
            //    if (s[i] == '1') foundIndexes.Add(i);
            //}

            for (int i = 0; i < Bits.Length; i++)
            {
                if (Bits[i])
                {
                    //foundIndexes.Add(i);
                    sumSurvivePoints += Things.Products[i].SurvivalPoints;
                    sumOfWeight += Things.Products[i].Weight;
                }
            }


            //foreach (int indexOfOneInChromosom in foundIndexes)
            //{

            //}

            //SurivatePoints = sumSurvivePoints;
            Weight = sumOfWeight;

            SurivatePoints = (sumOfWeight > weightLimit) ? 0 : sumSurvivePoints;
        }
    }
}
