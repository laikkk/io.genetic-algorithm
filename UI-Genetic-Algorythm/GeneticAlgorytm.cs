using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;

namespace UI_Genetic_Algorythm
{
    public class GeneticAlgorytm
    {
        public IList<DataPoint> Points { get; }
        public Chromosom TheBestChromosom { get; set; }

        private readonly Random _random = new Random();
        private readonly int _populationCount;
        private readonly int _numberOfIterations;
        private readonly int _maxWeight;
        private readonly int _chromosomLength;
        private readonly double _mutationPropability;
        private readonly bool _useElityzm;

        public GeneticAlgorytm(int chromosomLength, int populationCount, int numberOfIterations, int maxWeightWeight, double mutationPropability, bool useElityzm)
        {
            Points = new List<DataPoint>();
            _chromosomLength = chromosomLength;
            _populationCount = populationCount;
            _numberOfIterations = numberOfIterations;
            _maxWeight = maxWeightWeight;
            _mutationPropability = mutationPropability;
            _useElityzm = useElityzm;
        }

        public void Compute()
        {
            List<Chromosom> population = GenerateInitialChromosomPopulation(_populationCount, _chromosomLength);
            List<Chromosom> theBestParents = new List<Chromosom>();
            for (int i = 0; i < _numberOfIterations; i++)
            {
                // count fitness value
                //foreach (var chromosom in population)
                //{
                //    chromosom.Fitness(_maxWeight);
                //}

                population.ForEach(chromosom => chromosom.Fitness(_maxWeight));
                population = population.OrderByDescending(chromosom => chromosom.SurivatePoints).ToList();
                if (_useElityzm)
                {
                    theBestParents = population.Take(2).ToList();
                }

                Chromosom theBestCurrentChromosom = population.First();
                if (TheBestChromosom == null || TheBestChromosom.SurivatePoints < theBestCurrentChromosom.SurivatePoints)
                {
                    TheBestChromosom = theBestCurrentChromosom;
                }
                Points.Add(new DataPoint(i, theBestCurrentChromosom.SurivatePoints));

                population = RouletteSelection(population);
                
                //population = GetListOfChromosomsUsinRuletteMethod(population);
                //TODO
                population = CrossPopulation(population);

                population = Mutation(population);

                if (_useElityzm)
                {
                    // replace 2 random chromosom with elite parent
                    List<int> randomNumbers = Enumerable.Range(0, population.Count-1).Shuffle().Take(2).ToList();
                    population.RemoveAt(randomNumbers[0]);
                    population.RemoveAt(randomNumbers[1]);
                    population.AddRange(theBestParents);
                }
            }

            //population.ForEach(chromosom => chromosom.Fitness(_maxWeight));
            //var sortedPopulation = population.OrderByDescending(chromosom => chromosom.SurivatePoints);
            //Chromosom theBestSChromosom = sortedPopulation.First();
            //Console.WriteLine("KONIEC");
            //Console.WriteLine("THE BEST SOLUTION");
            //Console.WriteLine(theBestSChromosom);
            //Console.ReadLine();
        }

        private List<Chromosom> Mutation(List<Chromosom> population)
        {
            int lengthOfChromosom = population.First().Bits.Length;
            for (int i = 0; i < population.Count; i++)
            {
                double randomShot = _random.NextDouble();
                if (randomShot <= _mutationPropability)
                {
                    int crossPointIndex = _random.Next(0, lengthOfChromosom);
                    population[i].Bits[crossPointIndex] = !population[i].Bits[crossPointIndex];
                }
            }

            return population;
        }

        private List<Chromosom> GenerateInitialChromosomPopulation(int populationCount, int chromosomLength)
        {
            List<Chromosom> population = new List<Chromosom>();

            //int maxValue = (int)Math.Pow(2, chromosomLength) - 1; // eg 4x1 1111 = max value from these bits is 15 [(2^4)-1]
            for (int i = 0; i < populationCount; i++)
            {
                //population.Add(new Chromosom { Value = _random.Next(0, maxValue), NumberOfBits = chromosomLength });
                population.Add(new Chromosom(chromosomLength, _random));
            }

            return population;
        }

        private List<Chromosom> RouletteSelection(List<Chromosom> population)
        {
            double sumOfFitnessValues = population.Sum(x => x.SurivatePoints);
            double[] rulette = new double[population.Count + 1];

            //set up roulette
            for (int i = 1; i <= population.Count; i++)
            {
                rulette[i] = population[i - 1].SurivatePoints / sumOfFitnessValues;
                // rulette[0] = 0 % (0-0)
                // rulette[1] = 38 % (0-38)
                // rulette[2] = 43 % (38-43)
                // rulette[3] = 74 % (43-74)
                // ...
            }

            List<Chromosom> selectedChromosoms = new List<Chromosom>(population.Count);

            for (int i = 0; i < population.Count; i++)
            {
                double shot = _random.NextDouble();
                double sumOfPropability = 0;
                for (int j = 0; j < rulette.Length - 1; j++)
                {
                    sumOfPropability += rulette[j + 1];
                    if (shot < sumOfPropability)
                    {
                        selectedChromosoms.Add(population[j]);
                        break;
                    }
                }
            }

            return selectedChromosoms;
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

            if (countDiffence < 0)
            {
                //var sortedPopulation = population.OrderByDescending(chromosom => chromosom.SurivatePoints);
                // population should be sorted here
                tmpPopulation.AddRange(population.Take(Math.Abs(countDiffence)));
            }
            else if (tmpPopulation.Count - population.Count > 0)
            {
                tmpPopulation = tmpPopulation.Shuffle().Take(population.Count).ToList();
            }

            return tmpPopulation;
            // do cross
        }

        private List<Chromosom> CrossPopulation(List<Chromosom> population)
        {
            int numberOfPairsToCross = population.Count / 2;
            List<int> randomNumbers = Enumerable.Range(0, population.Count).Shuffle().Take(numberOfPairsToCross * 2).ToList();

            List<Chromosom> crossedPopulation = new List<Chromosom>(population.Count);

            for (int i = 0; i < randomNumbers.Count - 1; i += 2)
            {
                var children = CrossChromosoms(population[randomNumbers[i]], population[randomNumbers[i + 1]]);
                crossedPopulation.Add(children.Item1);
                crossedPopulation.Add(children.Item2);
            }

            return crossedPopulation;
        }

        private Tuple<Chromosom, Chromosom> CrossChromosoms(Chromosom chromosomOne, Chromosom chromosomTwo)
        {
            int lengthOfChromosom = chromosomOne.Bits.Length; //TODO
            int crossPointIndex = _random.Next(0, lengthOfChromosom); //TODO check if - 1 is needed

            //bool[] firstPartBitsOfOneChromosm = new bool[chromosomOne.Bits.Length];
            //bool[] secondPartBitsOfOneChromosm = new bool[chromosomOne.Bits.Length];
            //chromosomOne.Bits.CopyTo(firstPartBitsOfOneChromosm, crossPointIndex);
            //chromosomOne.Bits.

            var firstPartOfChromosomA = chromosomOne.Bits.Cast<bool>().Take(crossPointIndex).ToArray();
            var secondPartOfChromosomA = chromosomOne.Bits.Cast<bool>().Skip(crossPointIndex).ToArray();

            var firstPartOfChromosomB = chromosomTwo.Bits.Cast<bool>().Take(crossPointIndex).ToArray();
            var secondPartOfChromosomB = chromosomTwo.Bits.Cast<bool>().Skip(crossPointIndex).ToArray();

            BitArray newChildA = new BitArray(firstPartOfChromosomA.Concat(secondPartOfChromosomB).ToArray());
            BitArray newChildB = new BitArray(firstPartOfChromosomB.Concat(secondPartOfChromosomA).ToArray());

            //string chromosomOneAsString = chromosomOne.AsAStringBits(); // rewrite this function
            //string chromosomTwoAsString = chromosomTwo.AsAStringBits();

            //string newOneChromosm = chromosomOneAsString.Substring(0, crossPointIndex);
            //newOneChromosm += chromosomTwoAsString.Substring(crossPointIndex);

            //string newTwoChromosm = chromosomTwoAsString.Substring(0, crossPointIndex);
            //newTwoChromosm += chromosomOneAsString.Substring(crossPointIndex);

            //Chromosom newChromosomOne = new Chromosom() { Value = Convert.ToInt32(newOneChromosm, 2), NumberOfBits = chromosomOne.NumberOfBits };
            //Chromosom newChromosomTwo = new Chromosom() { Value = Convert.ToInt32(newTwoChromosm, 2), NumberOfBits = chromosomOne.NumberOfBits };

            Chromosom newChromosomOne = new Chromosom(newChildA);//; { Value = Convert.ToInt32(newOneChromosm, 2), NumberOfBits = chromosomOne.NumberOfBits };
            Chromosom newChromosomTwo = new Chromosom(newChildB);

            //Console.WriteLine("Old(Parent) Chromosoms:");
            //Console.WriteLine("1: " + chromosomOne);
            //Console.WriteLine("2: " + chromosomTwo);
            //Console.WriteLine("New ones");
            //Console.WriteLine("1: " + newChromosomOne);
            //Console.WriteLine("2: " + newChromosomTwo);
            //Console.WriteLine("*************");

            return Tuple.Create(newChromosomOne, newChromosomTwo);
        }
    }
}
