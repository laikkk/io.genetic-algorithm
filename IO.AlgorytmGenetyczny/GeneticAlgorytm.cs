using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.AlgorytmGenetyczny
{
    public class GeneticAlgorytm
    {
        public int WeightLimit { get; set; }
        List<Chromosom> generateInitialChromosomPopulation(int populationCount, int chromosomLength)
        {
            List<Chromosom> population = new List<Chromosom>();
            Random r = new Random();
            int maxValue = (int)Math.Pow(chromosomLength, 2); // eg 4x1 1111 = max value from these bits is 16 (4^2)
            for (int i = 0; i < populationCount; i++)
            {
                population.Add(new Chromosom { Value = r.Next(0, maxValue});
            }

            return population;
        }

        int fitness(Chromosom chromosom) {
            var productTable = Things.products;

            string s = chromosom.Value.ToString();
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

            chromosom.SurivatePoints = sumSurvivePoints;
            chromosom.Weight = sumOfWeight;

            return (sumOfWeight > WeightLimit) ? 0 : sumSurvivePoints;
        }

        void checkIfMetCriteria(List<Chromosom> population,bool isFinalRound=false) {
            population = population.OrderByDescending(chromosom => chromosom.SurivatePoints).ToList();
            //foreach (Chromosom chromosom in population)
            //{
            //    if (fitness(chromosom) > currentlyTheBestChomosom.SurivatePoints)
            //        currentlyTheBestChomosom = chromosom;
                    
            //}

            if (isFinalRound) {
                // show current chromosom as the best one
                Console.WriteLine("The best chomosom:");
                Console.WriteLine(population.Last().ToString()); // sprawdz czy last czy first
            }
        }

        public GeneticAlgorytm()
        {
            int chromosomLength = Things.products.Count;
            int populationCount = 6;

            var population = generateInitialChromosomPopulation(populationCount, chromosomLength);

        }
    }
}
