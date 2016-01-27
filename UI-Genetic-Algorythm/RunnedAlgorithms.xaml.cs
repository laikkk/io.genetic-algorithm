using System;
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
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Wpf;

namespace UI_Genetic_Algorythm
{
    public partial class RunnedAlgorithms : Window, INotifyPropertyChanged
    {
        private ObservableCollection<GeneticAlgorytm> _geneticAlgorytms;
        private ObservableCollection<GeneticAlgorytm> GeneticAlgorytms
        {
            get { return _geneticAlgorytms; }
            set
            {
                _geneticAlgorytms = value;
                OnPropertyChanged();
            }
        }

        private string _totalDuration;
        public string TotalDuration
        {
            get { return _totalDuration; }
            set
            {
                _totalDuration = value;
                OnPropertyChanged();
            }
        }

        public RunnedAlgorithms()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public RunnedAlgorithms(List<GeneticAlgorytm> geneticAlgorytms, bool runAll = false, bool runInParallel = false)
        {
            InitializeComponent();
            this.DataContext = this;
            GeneticAlgorytms = new ObservableCollection<GeneticAlgorytm>(geneticAlgorytms);

            // Attach list of algorithms to TabControl
            TabControl.ItemsSource = geneticAlgorytms;
            // Select first tab
            TabControl.SelectedIndex = 0;

            if (runAll)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (runInParallel)
                {
                    Parallel.ForEach(geneticAlgorytms, al => al.Compute());
                }
                else
                {
                    geneticAlgorytms.ForEach(algorytm => algorytm.Compute());
                }
                stopwatch.Stop();
                TotalDuration = stopwatch.Elapsed.ToString("mm\\:ss\\:fff");
            }
        }
        private void RunChoosenGA(object sender, RoutedEventArgs e)
        {
            int i = TabControl.SelectedIndex;
            GeneticAlgorytms[i].Compute();
            Plot plot = new Plot();

            plot.ActualModel.InvalidatePlot(true);
            OnPropertyChanged("Points");
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
