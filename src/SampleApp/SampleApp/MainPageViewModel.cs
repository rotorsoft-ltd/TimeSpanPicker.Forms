using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SampleApp
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private TimeSpan _constraintValue = TimeSpan.Zero;

        public MainPageViewModel()
        {
        }

        public TimeSpan ConstraintValue
        {
            get => _constraintValue;
            set
            {
                if (_constraintValue != value)
                {
                    _constraintValue = value;
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
