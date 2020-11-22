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

        private bool _isAddSelected = true;
        private bool _isSubtractSelected = false;

        public MainPageViewModel()
        {
            PropertyChanged += MainPageViewModel_PropertyChanged;
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

        public TimeSpan MaxValueToAdd => new TimeSpan(23, 59, 59) - StartingTimeSpan;

        public TimeSpan Result
        {
            get => _result;
            private set
            {
                if (_result != value)
                {
                    _result = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(MaxValueToAdd));
                }
            }
        }

        public bool IsAddSelected
        {
            get => _isAddSelected;
            set
            {
                if (_isAddSelected != value)
                {
                    _isAddSelected = value;
                    RaisePropertyChanged();

                    if (_isAddSelected)
                    {
                        IsSubtractSelected = false;
                    }
                }
            }
        }

        public bool IsSubtractSelected
        {
            get => _isSubtractSelected;
            set
            {
                if (_isSubtractSelected != value)
                {
                    _isSubtractSelected = value;
                    RaisePropertyChanged();

                    if (_isSubtractSelected)
                    {
                        IsAddSelected = false;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MainPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StartingTimeSpan) || e.PropertyName == nameof(ValueToAdd))
            {
                Result = StartingTimeSpan + ValueToAdd;
            }
        }

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
