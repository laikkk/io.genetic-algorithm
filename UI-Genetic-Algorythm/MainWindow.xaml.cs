﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using Newtonsoft.Json;
using OxyPlot;
using System.IO;
using System.Text.RegularExpressions;
using UI_Genetic_Algorythm.Models;

namespace UI_Genetic_Algorythm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string FileName = "items.json";
        private const string DirectoryName = @"Items\";
        public string PlotTitle { get; private set; }

        private IList<DataPoint> _points;
        public IList<DataPoint> Points
        {
            get { return _points; }
            set { _points = value; OnPropertyChanged(); }
        }

        private string _maxValue;
        public string MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; OnPropertyChanged(); }
        }

        private string _duration;
        public string Duration
        {
            get { return _duration; }
            set { _duration = value; OnPropertyChanged(); }
        }

        private int _interation;
        public int Interation
        {
            get { return _interation; }
            set { _interation = value; OnPropertyChanged(); }
        }

        private int _populationCount;
        public int PopulationCount
        {
            get { return _populationCount; }
            set { _populationCount = value; OnPropertyChanged(); }
        }

        private int _maxWeight;
        public int MaxWeight
        {
            get { return _maxWeight; }
            set { _maxWeight = value; OnPropertyChanged(); }
        }

        private int _chromosomLength;
        public int ChromosomLength
        {
            get { return _chromosomLength; }
            set { _chromosomLength = value; OnPropertyChanged(); }
        }

        private bool _useElitism;
        public bool UseElitism
        {
            get { return _useElitism; }
            set { _useElitism = value; OnPropertyChanged(); }
        }

        private double _mutationProp;
        public double MutationProp
        {
            get { return _mutationProp; }
            set { _mutationProp = value; OnPropertyChanged(); }
        }

        //private ObservableCollection<Product> _items;
        public ObservableCollection<Product> Items
        {
            get
            {
                return new ObservableCollection<Product>(Things.Products);
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.PlotTitle = "Example 2";
            Interation = 100;
            PopulationCount = 6;
            MaxWeight = 20;
            ChromosomLength = 6;
            UseElitism = true;
            MutationProp = 0.01;

            this.Points = new List<DataPoint>
            {
                new DataPoint(0, 4),
                new DataPoint(10, 13),
                new DataPoint(20, 15),
                new DataPoint(30, 16),
                new DataPoint(40, 12),
                new DataPoint(50, 12)
            };

            //ReadRandomNouns();
        }

        private void RunGeneticAlgorythm(object sender, RoutedEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();

            GeneticAlgorytm geneticAlgorytm = new GeneticAlgorytm(ChromosomLength,PopulationCount, Interation, MaxWeight, MutationProp, UseElitism);
            stopWatch.Start();
            geneticAlgorytm.Compute();
            stopWatch.Stop();

            Duration = String.Format("{0}", stopWatch.Elapsed);

            Points = geneticAlgorytm.Points;
            MaxValue = geneticAlgorytm.TheBestChromosom.ToString();
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

        private void SaveItemsToFile(object sender, RoutedEventArgs e)
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
            //var json = JsonConvert.SerializeObject(Items);
            ////string path = Path.Combine(Environment.CurrentDirectory, DirectoryName, FileName);
            //File.WriteAllText(PathToItemsJsonFile(), json);
        }

        private String PathToItemsJsonFile() => Path.Combine(Environment.CurrentDirectory, DirectoryName, FileName);

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

        private void ReadRandomNouns()
        {
            //int counter = 0;
            string line;
            Random random = new Random();
            // Read the file and display it line by line.
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
                //counter++;
            }

            file.Close();
        }

        private void ShowColumnId(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
