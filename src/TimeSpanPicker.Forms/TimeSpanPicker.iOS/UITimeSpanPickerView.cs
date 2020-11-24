using System;
using UIKit;

namespace Rotorsoft.Forms.Platform.iOS
{
    internal class UITimeSpanPickerView : UIPickerView
    {
        public static readonly int ComponentCount = 5;
        public static readonly nint HoursComponentIndex = new nint(0);
        public static readonly nint MinutesComponentIndex = new nint(2);
        public static readonly nint SecondsComponentIndex = new nint(4);

        private TimeSpan _minTime;
        private TimeSpan _maxTime;

        public UITimeSpanPickerView()
            : base()
        {
            Model = new UITimeSpanPickerViewModel();
        }

        public UITimeSpanPickerViewModel TimeSpanPickerModel => Model as UITimeSpanPickerViewModel;

        public TimeSpan Time
        {
            get => TimeSpanPickerModel.Time;
            set
            {
                TimeSpanPickerModel.Time = value;

                UpdateComponentsSelection();
            }
        }

        public TimeSpan MinTime
        {
            get => _minTime;
            set
            {
                if (_minTime != value)
                {
                    _minTime = value;

                    TimeSpanPickerModel.UpdatePickersConstraints(value, MaxTime);
                    UpdateComponentsSelection();
                    ReloadAllComponents();
                }
            }
        }

        public TimeSpan MaxTime
        {
            get => _maxTime;
            set
            {
                if (_maxTime != value)
                {
                    _maxTime = value;

                    TimeSpanPickerModel.UpdatePickersConstraints(MinTime, value);
                    UpdateComponentsSelection();
                    ReloadAllComponents();
                }
            }
        }

        private void UpdateComponentsSelection()
        {
            var hoursRow = Math.Max(TimeSpanPickerModel.Time.Hours - MinTime.Hours, 0);
            var minutesRow = TimeSpanPickerModel.Time.Minutes;
            var secondsRow = TimeSpanPickerModel.Time.Seconds;

            if (hoursRow == 0)
            {
                minutesRow = Math.Max(minutesRow - MinTime.Minutes, 0);
            }

            if (minutesRow == 0)
            {
                secondsRow = Math.Max(secondsRow - MinTime.Seconds, 0);
            }

            Select(new nint(hoursRow), HoursComponentIndex, false);
            Select(new nint(minutesRow), MinutesComponentIndex, false);
            Select(new nint(secondsRow), SecondsComponentIndex, false);
        }
    }
}