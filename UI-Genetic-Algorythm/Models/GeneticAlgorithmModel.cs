using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UI_Genetic_Algorythm.Models
{
    public class GeneticAlgorithmModel : INotifyPropertyChanged
    {
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
