using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UI_Genetic_Algorythm
{
    public class GeneticAlgorytm : INotifyPropertyChanged
    {
        public string Test { get; set; } = "TEST23";
        public Chromosom TheBestChromosom { get; set; }

        private IList<DataPoint> _points;
        public IList<DataPoint> Points
        {
            get { return _points; }
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _maxValue;

        public string MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                OnPropertyChanged();
            }
        }

        private string _duration;

        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        private int _interation;

        public int Interation
        {
            get { return _interation; }
            set
            {
                _interation = value;
                OnPropertyChanged();
            }
        }

        private int _populationCount;

        public int PopulationCount
        {
            get { return _populationCount; }
            set
            {
                _populationCount = value;
                OnPropertyChanged();
            }
        }

        private int _maxWeight;

        public int MaxWeight
        {
            get { return _maxWeight; }
            set
            {
                _maxWeight = value;
                OnPropertyChanged();
            }
        }

        private int _chromosomLength;

        public int ChromosomLength
        {
            get { return _chromosomLength; }
            set
            {
                _chromosomLength = value;
                OnPropertyChanged();
            }
        }

        private bool _useElitism;

        public bool UseElitism
        {
            get { return _useElitism; }
            set
            {
                _useElitism = value;
                OnPropertyChanged();
            }
        }

        private double _mutationProp;

        public double MutationProp
        {
            get { return _mutationProp; }
            set
            {
                _mutationProp = value;
                OnPropertyChanged();
            }
        }

        private readonly Random _random = new Random();

        public GeneticAlgorytm(int chromosomLength, int populationCount, int numberOfIterations, int maxWeightWeight, double mutationPropability, bool useElityzm,int id=0)
        {
            Points = new List<DataPoint>();
            ChromosomLength = chromosomLength;
            PopulationCount = populationCount;
            Interation = numberOfIterations;
            MaxWeight = maxWeightWeight;
            MutationProp = mutationPropability;
            UseElitism = useElityzm;
            Id = id;
        }

        public void Compute()
        {
            Points = new List<DataPoint>();
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Chromosom> population = GenerateInitialChromosomPopulation(_populationCount, _chromosomLength);
            List<Chromosom> theBestParents = new List<Chromosom>();
            for (int i = 0; i < Interation; i++)
            {
                // count fitness value
                //foreach (var chromosom in population)
                //{
                //    chromosom.Fitness(_maxWeight);
                //}

                population.ForEach(chromosom => chromosom.Fitness(_maxWeight));
                population = population.OrderByDescending(chromosom => chromosom.SurivatePoints).ToList();
                if (UseElitism)
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

                if (UseElitism)
                {
                    // replace 2 random chromosom with elite parent
                    List<int> randomNumbers = Enumerable.Range(0, population.Count-1).Shuffle().Take(2).ToList();
                    population.RemoveAt(randomNumbers[0]);
                    population.RemoveAt(randomNumbers[1]);
                    population.AddRange(theBestParents);
                }
            }
            stopWatch.Stop();
            Duration = String.Format("{0}", stopWatch.Elapsed);

            //Plot

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
                if (randomShot <= MutationProp)
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
            if (Math.Abs(sumOfFitnessValues) <= 0) return population;

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

        #region MVVM stuff

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propName));
            }
        }

        #endregion
    }
}
