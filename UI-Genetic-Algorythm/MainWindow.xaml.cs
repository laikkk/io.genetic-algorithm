using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI_Genetic_Algorythm.Models;

namespace UI_Genetic_Algorythm
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        
        private const string DirectoryName = @"Items\";
        public string PlotTitle { get; private set; }

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

        readonly GaComposerViewModel _gaComposerViewModel = new GaComposerViewModel();

        public ObservableCollection<Product> Items
        {
            get { return new ObservableCollection<Product>(Things.Products); }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            PlotTitle = "Simple 2";


            Interation = 100;
            PopulationCount = 6;
            MaxWeight = 20;
            ChromosomLength = 7;
            UseElitism = true;
            MutationProp = 0.01;

            this.Points = new List<DataPoint>
            {
                new DataPoint(0, 4),
                new DataPoint(10, 85),
                new DataPoint(20, 95),
                new DataPoint(30, 102),
                new DataPoint(40, 102),
                new DataPoint(50, 102)
            };

            // Attach data context for 3th tab
            RunGAs.DataContext = _gaComposerViewModel;
        }

        #region Plot tab

        private void RunGeneticAlgorythm(object sender, RoutedEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();

            GeneticAlgorytm geneticAlgorytm = new GeneticAlgorytm(ChromosomLength, PopulationCount, Interation,
                MaxWeight, MutationProp, UseElitism);
            stopWatch.Start();
            geneticAlgorytm.Compute();
            stopWatch.Stop();

            Duration = String.Format("{0}", stopWatch.Elapsed);

            Points = geneticAlgorytm.Points;
            MaxValue = geneticAlgorytm.TheBestChromosom.ToString();
        }

        #endregion

        #region Items tab

        private void AddAndSaveItemsToFile(object sender, RoutedEventArgs e)
        {
            String newItemName = ItemNameBox.Text;
            int newItemWeight = 0;
            int newItemSurvivalPoints = 0;

            #region validation

            if (String.IsNullOrWhiteSpace(newItemName) || String.IsNullOrEmpty(newItemName))
            {
                MessageBox.Show("ERROR wrong NAME: tried to add " + newItemName);
                return;
            }

            try
            {
                newItemWeight = Convert.ToInt32(WeightBox.Text);
                if (newItemWeight < 0) throw new Exception("Negative weight");
            }
            catch
            {
                MessageBox.Show("ERROR wrong WEIGHT: tried to add " + WeightBox.Text);
                return;
            }

            try
            {
                newItemSurvivalPoints = Convert.ToInt32(SurvivalPointsBox.Text);
                if (newItemSurvivalPoints < 0) throw new Exception("Negative Survival points");
            }
            catch
            {
                MessageBox.Show("ERROR wrong SURVIVAL POINTS: tried to add " + SurvivalPointsBox.Text);
                return;
            }

            #endregion

            Things things = new Things();
            things.AddProductIfNoExistis(new Product()
            {
                Name = newItemName,
                SurvivalPoints = newItemSurvivalPoints,
                Weight = newItemWeight
            });
            OnPropertyChanged("Items");
        }

        private void LoadRandomNouns(object sender, RoutedEventArgs e)
        {
            string line;
            Random random = new Random();
            StreamReader file = new StreamReader(Path.Combine(Environment.CurrentDirectory, DirectoryName, "random150nouns.txt"));
            while ((line = file.ReadLine()) != null)
            {
                Product product = new Product
                {
                    Name = Regex.Replace(line, @"\s+", ""),
                    SurvivalPoints = random.Next(0, 30),
                    Weight = random.Next(0, 30)
                };

                Things things = new Things();
                things.AddProductIfNoExistis(product);
                Console.WriteLine(line);
            }

            file.Close();
            OnPropertyChanged("Items");
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var grid = (DataGrid)sender;
            if (Key.Delete == e.Key)
            {
                Things things = new Things();
                foreach (var row in grid.SelectedItems)
                {
                    Product productToDelete = (Product)row;
                    things.RemoveProduct(productToDelete);
                }
                OnPropertyChanged("Items");
            }
        }

        private void ShowColumnId(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Run GAs tab

        private void Run_GAs(object sender, RoutedEventArgs e)
        {
            RunnedAlgorithms runnedAlgorithms = new RunnedAlgorithms(_gaComposerViewModel.ComposeGaList(), true);
            runnedAlgorithms.Show();
        }

        private void AddToGaComposer(object sender, RoutedEventArgs e)
        {
            _gaComposerViewModel.AddToQueue();
            OnPropertyChanged("GeneticAlgorytms1");
        }

        private void Run_GAs_In_Parallel(object sender, RoutedEventArgs e)
        {
            RunnedAlgorithms runnedAlgorithms = new RunnedAlgorithms(_gaComposerViewModel.ComposeGaList(), true, true);
            runnedAlgorithms.Show();
        }

        private void Open_GAs(object sender, RoutedEventArgs e)
        {
            RunnedAlgorithms runnedAlgorithms = new RunnedAlgorithms(_gaComposerViewModel.ComposeGaList());
            runnedAlgorithms.Show();
        }

        #endregion

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
