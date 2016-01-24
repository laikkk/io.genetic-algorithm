using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UI_Genetic_Algorythm.Models;

namespace UI_Genetic_Algorythm
{
    public class GaComposerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GeneticAlgorithmModel> _geneticAlgorytms1;
        public ObservableCollection<GeneticAlgorithmModel> GeneticAlgorytms1
        {
            get
            {
                if (_geneticAlgorytms1 == null)
                {
                    _geneticAlgorytms1 =
                        new ObservableCollection<GeneticAlgorithmModel>();
                }
                return _geneticAlgorytms1;
            }
            set { _geneticAlgorytms1 = value; OnPropertyChanged(); }
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

        private readonly List<int> _usedId = new List<int>();
        private int _idCounter = 0;
        public GaComposerViewModel()
        {
            Id = AvaiableId();
            Interation = 100;
            ChromosomLength = 7;
            PopulationCount = 100;
            MaxWeight = 20;
            UseElitism = true;
            MutationProp = 0.01;
        }

        public List<GeneticAlgorytm> ComposeGaList()
        {
            List<GeneticAlgorytm> generGeneticAlgorytms = new List<GeneticAlgorytm>();

            foreach (GeneticAlgorithmModel geneticAlgorithmModel in GeneticAlgorytms1)
            {
                generGeneticAlgorytms.Add(
                    new GeneticAlgorytm(
                        geneticAlgorithmModel.ChromosomLength,
                        geneticAlgorithmModel.PopulationCount,
                        geneticAlgorithmModel.Interation,
                        geneticAlgorithmModel.MaxWeight,
                        geneticAlgorithmModel.MutationProp,
                        geneticAlgorithmModel.UseElitism,
                        geneticAlgorithmModel.Id));
            }

            return generGeneticAlgorytms;
        }

        public void AddToQueue()
        {
            GeneticAlgorytms1.Add(new GeneticAlgorithmModel
            {
                Id = AvaiableId(),
                ChromosomLength = ChromosomLength,
                PopulationCount = PopulationCount,
                Interation = Interation,
                MaxWeight = MaxWeight,
                MutationProp = MutationProp,
                UseElitism = UseElitism
            });
            OnPropertyChanged("GeneticAlgorytms1");
        }

        private int AvaiableId()
        {
            while (_usedId.Any(x => x == _idCounter))
            {
                _idCounter++;
            }

            _usedId.Add(_idCounter);

            return _idCounter;
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
