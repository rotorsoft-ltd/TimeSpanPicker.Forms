using System;
using UIKit;

namespace Rotorsoft.Forms.Platform.iOS
{
    internal class UITimeSpanPickerViewModel : UIPickerViewModel
    {
        private NumberPickerViewModel _hoursPickerModel = new NumberPickerViewModel();
        private NumberPickerViewModel _minutesPickerModel = new NumberPickerViewModel();
        private NumberPickerViewModel _secondsPickerModel = new NumberPickerViewModel();

        public UITimeSpanPickerViewModel()
        {
        }

        public TimeSpan Time
        {
            get => new TimeSpan(_hoursPickerModel.Value, _minutesPickerModel.Value, _secondsPickerModel.Value);
            set
            {
                _hoursPickerModel.SetValue(value.Hours);
                _minutesPickerModel.SetValue(value.Minutes);
                _secondsPickerModel.SetValue(value.Seconds);
            }
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return UITimeSpanPickerView.ComponentCount;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            if (component == UITimeSpanPickerView.HoursComponentIndex)
            {
                return new nint(_hoursPickerModel.RowCount);
            }
            else if (component == UITimeSpanPickerView.MinutesComponentIndex)
            {
                return new nint(_minutesPickerModel.RowCount);
            }
            else if (component == UITimeSpanPickerView.SecondsComponentIndex)
            {
                return new nint(_secondsPickerModel.RowCount);
            }

            return new nint(1);
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (component == UITimeSpanPickerView.HoursComponentIndex)
            {
                return (row + _hoursPickerModel.MinValue).ToString("D2");
            }
            else if (component == UITimeSpanPickerView.MinutesComponentIndex)
            {
                return (row + _minutesPickerModel.MinValue).ToString("D2");
            }
            else if (component == UITimeSpanPickerView.SecondsComponentIndex)
            {
                return (row + _secondsPickerModel.MinValue).ToString("D2");
            }

            return ":";
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            if (pickerView is UITimeSpanPickerView timeSpanPickerView)
            {
                if (component == UITimeSpanPickerView.HoursComponentIndex)
                {
                    _hoursPickerModel.SetValueFromRow(row);

                    UpdateMinutesPickerConstraints(timeSpanPickerView.MinTime, timeSpanPickerView.MaxTime);
                    UpdateSecondsPickerConstraints(timeSpanPickerView.MinTime, timeSpanPickerView.MaxTime);

                    pickerView.ReloadComponent(UITimeSpanPickerView.MinutesComponentIndex);
                    pickerView.ReloadComponent(UITimeSpanPickerView.SecondsComponentIndex);

                    pickerView.Select(_minutesPickerModel.Row, UITimeSpanPickerView.MinutesComponentIndex, false);
                    pickerView.Select(_secondsPickerModel.Row, UITimeSpanPickerView.SecondsComponentIndex, false);
                }
                else if (component == UITimeSpanPickerView.MinutesComponentIndex)
                {
                    _minutesPickerModel.SetValueFromRow(row);

                    UpdateSecondsPickerConstraints(timeSpanPickerView.MinTime, timeSpanPickerView.MaxTime);

                    pickerView.ReloadComponent(UITimeSpanPickerView.SecondsComponentIndex);

                    pickerView.Select(_secondsPickerModel.Row, UITimeSpanPickerView.SecondsComponentIndex, false);
                }
                else if (component == UITimeSpanPickerView.SecondsComponentIndex)
                {
                    _secondsPickerModel.SetValueFromRow(row);
                }
            }
        }

        public override nfloat GetComponentWidth(UIPickerView pickerView, nint component)
        {
            if (component != UITimeSpanPickerView.HoursComponentIndex &&
                component != UITimeSpanPickerView.MinutesComponentIndex &&
                component != UITimeSpanPickerView.SecondsComponentIndex)
            {
                return 25f;
            }

            return 50f;
        }

        public void UpdatePickersConstraints(TimeSpan minTime, TimeSpan maxTime)
        {
            UpdateHoursPickerConstraints(minTime, maxTime);
            UpdateMinutesPickerConstraints(minTime, maxTime);
            UpdateSecondsPickerConstraints(minTime, maxTime);
        }

        private void UpdateHoursPickerConstraints(TimeSpan minTime, TimeSpan maxTime)
        {
            _hoursPickerModel.MinValue = minTime.Hours;
            _hoursPickerModel.MaxValue = maxTime.Hours;
        }

        private void UpdateMinutesPickerConstraints(TimeSpan minTime, TimeSpan maxTime)
        {
            _minutesPickerModel.MinValue = (_hoursPickerModel.Value == minTime.Hours) ? minTime.Minutes : 0;
            _minutesPickerModel.MaxValue = (_hoursPickerModel.Value == maxTime.Hours) ? maxTime.Minutes : 59;
        }

        private void UpdateSecondsPickerConstraints(TimeSpan minTime, TimeSpan maxTime)
        {
            _secondsPickerModel.MinValue = ((_hoursPickerModel.Value == minTime.Hours) && (_minutesPickerModel.Value == minTime.Minutes)) ? minTime.Seconds : 0;
            _secondsPickerModel.MaxValue = ((_hoursPickerModel.Value == maxTime.Hours) && (_minutesPickerModel.Value == maxTime.Minutes)) ? maxTime.Seconds : 59;
        }
    }
}