using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SampleApp
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private TimeSpan _startingTimeSpan = TimeSpan.Zero;
        private TimeSpan _valueToAdd = TimeSpan.Zero;
        private TimeSpan _result = TimeSpan.Zero;

        public MainPageViewModel()
        {
        }

        public TimeSpan StartingTimeSpan
        {
            get => _startingTimeSpan;
            set
            {
                if (_startingTimeSpan != value)
                {
                    _startingTimeSpan = value;
                    RaisePropertyChanged();
                }
            }
        }

        public TimeSpan ValueToAdd
        {
            get => _valueToAdd;
            set
            {
                if (_valueToAdd != value)
                {
                    _valueToAdd = value;
                    RaisePropertyChanged();
                }
            }
        }

        public TimeSpan Result
        {
            get => _result;
            private set
            {
                if (_result != value)
                {
                    _result = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
