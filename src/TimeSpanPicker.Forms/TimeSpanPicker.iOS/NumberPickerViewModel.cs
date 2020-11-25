using System;

namespace Rotorsoft.Forms.Platform.iOS
{
    internal class NumberPickerViewModel
    {
        private int _minValue;
        private int _maxValue;

        public int Value
        {
            get;
            private set;
        }

        public int MinValue
        {
            get => _minValue;
            set
            {
                if (_minValue != value)
                {
                    _minValue = value;

                    if (Value < _minValue)
                    {
                        Value = _minValue;
                    }
                }
            }
        }

        public int MaxValue
        {
            get => _maxValue;
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;

                    if (Value > _maxValue)
                    {
                        Value = _maxValue;
                    }
                }
            }
        }

        public int RowCount => (MaxValue - MinValue) + 1;

        public int Row => Value - MinValue;

        public void SetValueFromRow(nint row)
        {
            Value = Math.Clamp((int)row + MinValue, MinValue, MaxValue);
        }

        public void SetValue(int value)
        {
            Value = Math.Clamp(value, MinValue, MaxValue);
        }
    }
}