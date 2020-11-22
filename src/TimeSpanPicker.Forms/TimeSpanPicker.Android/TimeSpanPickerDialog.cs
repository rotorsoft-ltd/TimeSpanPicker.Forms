using Android.App;
using Android.Content;
using Android.Widget;
using System;

namespace Rotorsoft.Forms.Platform.Android
{
    internal class TimeSpanPickerDialog : AlertDialog
	{
		private readonly EventHandler<TimeSpan> _callback;

		private readonly NumberPicker _hoursPicker;
		private readonly NumberPicker _minutesPicker;
		private readonly NumberPicker _secondsPicker;

		private readonly TimeSpan _minTime;
		private readonly TimeSpan _maxTime;

		public TimeSpanPickerDialog(Context context, EventHandler<TimeSpan> callback, TimeSpan time, TimeSpan minTime, TimeSpan maxTime) : base(context)
		{
			_callback = callback;

			_minTime = minTime;
			_maxTime = maxTime;

			SetCancelable(true);
			SetCanceledOnTouchOutside(true);

			SetButton((int)DialogButtonType.Negative, "Cancel", OnCancelClicked);
			SetButton((int)DialogButtonType.Positive, "OK", OnOkClicked);

			var dialogContent = new LinearLayout(Context);
			dialogContent.SetHorizontalGravity(global::Android.Views.GravityFlags.Center);
			dialogContent.SetVerticalGravity(global::Android.Views.GravityFlags.Center);
			dialogContent.SetPadding(20, 20, 20, 0);

			var numberFormatter = new NumberPickerFormatter();

			_hoursPicker = new NumberPicker(Context)
			{
				MinValue = 0,
				MaxValue = 23,
				WrapSelectorWheel = false,
			};
			_hoursPicker.Value = time.Hours;
            _hoursPicker.ValueChanged += HourPicker_ValueChanged;
			_hoursPicker.SetFormatter(numberFormatter);

			_minutesPicker = new NumberPicker(Context)
			{
				MinValue = 0,
				MaxValue = 59,
				WrapSelectorWheel = false,
			};
			_minutesPicker.Value = time.Minutes;
            _minutesPicker.ValueChanged += MinutePicker_ValueChanged;
			_minutesPicker.SetFormatter(numberFormatter);

			_secondsPicker = new NumberPicker(Context)
			{
				MinValue = 0,
				MaxValue = 59,
				WrapSelectorWheel = false,
			};
			_secondsPicker.Value = time.Seconds;
			_secondsPicker.SetFormatter(numberFormatter);

			dialogContent.AddView(_hoursPicker);
			dialogContent.AddView(new TextView(Context)
			{
				Text = ":"
			});
			dialogContent.AddView(_minutesPicker);
			dialogContent.AddView(new TextView(Context)
			{
				Text = ":"
			});
			dialogContent.AddView(_secondsPicker);

			UpdateHoursPickerConstraints();
			UpdateMinutesPickerConstraints();
			UpdateSecondsPickerConstrains();

			SetView(dialogContent);
		}

        protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void HourPicker_ValueChanged(object sender, NumberPicker.ValueChangeEventArgs e)
		{
			UpdateMinutesPickerConstraints();
		}

		private void MinutePicker_ValueChanged(object sender, NumberPicker.ValueChangeEventArgs e)
		{
			UpdateSecondsPickerConstrains();
		}

		private void UpdateHoursPickerConstraints()
        {
			_hoursPicker.MinValue = _minTime.Hours;
			_hoursPicker.MaxValue = _maxTime.Hours;
        }

		private void UpdateMinutesPickerConstraints()
        {
			_minutesPicker.MinValue = (_hoursPicker.Value == _minTime.Hours) ? _minTime.Minutes : 0;
			_minutesPicker.MaxValue = (_hoursPicker.Value == _maxTime.Hours) ? _maxTime.Minutes : 59;
		}

		private void UpdateSecondsPickerConstrains()
        {
			_secondsPicker.MinValue = (_minutesPicker.Value == _minTime.Minutes) ? _minTime.Seconds : 0;
			_secondsPicker.MaxValue = (_minutesPicker.Value == _maxTime.Minutes) ? _maxTime.Seconds : 59;
		}

		private void OnCancelClicked(object sender, DialogClickEventArgs args)
		{
		}

		private void OnOkClicked(object sender, DialogClickEventArgs args)
		{
			_callback?.Invoke(this, new TimeSpan(_hoursPicker.Value, _minutesPicker.Value, _secondsPicker.Value));
		}
	}
}