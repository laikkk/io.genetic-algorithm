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
        readonly Random _random = new Random();
        public int WeightLimit { get; set; }
        List<Chromosom> GenerateInitialChromosomPopulation(int populationCount, int chromosomLength)
        {
            List<Chromosom> population = new List<Chromosom>();

            int maxValue = (int)Math.Pow(chromosomLength, 2); // eg 4x1 1111 = max value from these bits is 16 (4^2)
            for (int i = 0; i < populationCount; i++)
            {
                population.Add(new Chromosom { Value = _random.Next(0, maxValue), NumberOfBits = chromosomLength });
            }

            return population;
        }



        void checkIfMetCriteria(List<Chromosom> population, bool isFinalRound = false)
        {
            population = population.OrderByDescending(chromosom => chromosom.SurivatePoints).ToList();
            //foreach (Chromosom chromosom in population)
            //{
            //    if (fitness(chromosom) > currentlyTheBestChomosom.SurivatePoints)
            //        currentlyTheBestChomosom = chromosom;

            //}

            if (isFinalRound)
            {
                // show current chromosom as the best one
                Console.WriteLine("The best chomosom:");
                Console.WriteLine(population.Last().ToString()); // sprawdz czy last czy first
            }
        }

        private List<Chromosom> GetListOfChromosomsUsinRuletteMethod(List<Chromosom> population)
        {
            double sumOfAllChromosomsFitnessValues = population.Sum(x => x.SurivatePoints);
            double meanOfChromosomsFitnessValues = sumOfAllChromosomsFitnessValues / population.Count;

            List<Chromosom> tmpPopulation = new List<Chromosom>(population.Count);

            foreach (var chromosom in population)
            {
                int numberOfChromosomsInNewGeneration = (int)Math.Round(chromosom.SurivatePoints / meanOfChromosomsFitnessValues);
                Console.WriteLine("Chomosom: {0} is going to be add {1}-times to new generation", chromosom, numberOfChromosomsInNewGeneration);
                for (int i = 0; i < numberOfChromosomsInNewGeneration; i++)
                {
                    tmpPopulation.Add(chromosom);
                }
            }

            int countDiffence = tmpPopulation.Count - population.Count;
            
            if (countDiffence < 0 )
            {
                var sortedPopulation = population.OrderByDescending(chromosom => chromosom.SurivatePoints);
                tmpPopulation.AddRange(sortedPopulation.Take(Math.Abs(countDiffence)));
            }
            else if (tmpPopulation.Count - population.Count > 0)
            {
                tmpPopulation = tmpPopulation.Shuffle().Take(population.Count).ToList();
            }

            return tmpPopulation;
            // do cross
        }

        private Tuple<Chromosom, Chromosom> CrossChromosoms(Chromosom chromosomOne, Chromosom chromosomTwo)
        {
            int lengthOfChromosom = chromosomOne.AsAStringBits().Length;
            int crossPointIndex = _random.Next(0, lengthOfChromosom - 1);

            string chromosomOneAsString = chromosomOne.AsAStringBits();
            string chromosomTwoAsString = chromosomTwo.AsAStringBits();

            string newOneChromosm = chromosomOneAsString.Substring(0, crossPointIndex);
            newOneChromosm += chromosomTwoAsString.Substring(crossPointIndex);

            string newTwoChromosm = chromosomTwoAsString.Substring(0, crossPointIndex);
            newTwoChromosm += chromosomOneAsString.Substring(crossPointIndex);

            Chromosom newChromosomOne = new Chromosom() { Value = Convert.ToInt32(newOneChromosm, 2), NumberOfBits = chromosomOne.NumberOfBits };
            Chromosom newChromosomTwo = new Chromosom() { Value = Convert.ToInt32(newTwoChromosm, 2), NumberOfBits = chromosomOne.NumberOfBits };

            Console.WriteLine("Old(Parent) Chromosoms:");
            Console.WriteLine("1: " + chromosomOne);
            Console.WriteLine("2: " + chromosomTwo);
            Console.WriteLine("New ones");
            Console.WriteLine("1: " + newChromosomOne);
            Console.WriteLine("2: " + newChromosomTwo);
            Console.WriteLine("*************");

            return Tuple.Create(newChromosomOne, newChromosomTwo);
        }

        private List<Chromosom> CrossPopulation(List<Chromosom> population)
        {
            int numberOfPairsToCross = population.Count / 2;
            List<int> listOfNumbersOfEachChromosomWillBeCrossed = Enumerable.Range(1, population.Count).Shuffle().Take(numberOfPairsToCross * 2).ToList();

            List<Chromosom> newPopulation = new List<Chromosom>(population.Count);

            for (int i = 0; i < listOfNumbersOfEachChromosomWillBeCrossed.Count - 1; i += 2)
            {
                var children = CrossChromosoms(population[i], population[i + 1]);
                newPopulation.Add(children.Item1);
                newPopulation.Add(children.Item2);
            }

            return newPopulation;
        }

        public GeneticAlgorytm()
        {
            int chromosomLength = Things.products.Count;
            int populationCount = 6;
            int numberOfIterations = 100;
            int wheightMax = 20;

            var population = GenerateInitialChromosomPopulation(populationCount, chromosomLength);

            for (int i = 0; i < numberOfIterations; i++)
            {
                // count fitness value
                foreach (var chromosom in population)
                {
                    chromosom.Fitness(wheightMax);
                }

                population = GetListOfChromosomsUsinRuletteMethod(population);

                population = CrossPopulation(population);
            }

            population.ForEach(chromosom => chromosom.Fitness(wheightMax));
            var sortedPopulation = population.OrderByDescending(chromosom => chromosom.SurivatePoints);
            Chromosom theBestSChromosom = sortedPopulation.First();
            Console.WriteLine("KONIEC");
            Console.WriteLine("THE BEST SOLUTION");
            Console.WriteLine(theBestSChromosom);
            Console.ReadLine();
        }
    }
}
