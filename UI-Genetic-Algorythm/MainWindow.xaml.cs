using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using OxyPlot;

namespace UI_Genetic_Algorythm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.PlotTitle = "Example 2";
            Interation = 100;
            PopulationCount = 6;
            MaxWeight = 20;
            this.Points = new List<DataPoint>
            {
                new DataPoint(0, 4),
                new DataPoint(10, 13),
                new DataPoint(20, 15),
                new DataPoint(30, 16),
                new DataPoint(40, 12),
                new DataPoint(50, 12)
            };
        }

        private void RunGeneticAlgorythm(object sender, RoutedEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            
            GeneticAlgorytm geneticAlgorytm = new GeneticAlgorytm(PopulationCount, Interation, MaxWeight,0.01, false);
            stopWatch.Start();
            geneticAlgorytm.Compute();
            stopWatch.Stop();

            Duration = String.Format("{0}", stopWatch.Elapsed);

            Points = geneticAlgorytm.Points;
            MaxValue = geneticAlgorytm.TheBestChromosom.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
