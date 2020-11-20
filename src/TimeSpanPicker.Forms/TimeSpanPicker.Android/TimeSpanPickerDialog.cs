using Android.App;
using Android.Content;
using Android.Widget;
using System;

namespace Rotorsoft.Forms.Platform.Android
{
    internal class TimeSpanPickerDialog : AlertDialog
	{
		private readonly EventHandler<TimeSpan> _callback;

		private readonly NumberPicker _hourPicker;
		private readonly NumberPicker _minutePicker;
		private readonly NumberPicker _secondPicker;

		public TimeSpanPickerDialog(Context context, EventHandler<TimeSpan> callback, int hours, int minutes, int seconds) : base(context)
		{
			_callback = callback;

			SetCancelable(true);
			SetCanceledOnTouchOutside(true);

			SetButton((int)DialogButtonType.Negative, "Cancel", OnCancelClicked);
			SetButton((int)DialogButtonType.Positive, "OK", OnOkClicked);

			var dialogContent = new LinearLayout(Context);
			dialogContent.SetHorizontalGravity(global::Android.Views.GravityFlags.Center);
			dialogContent.SetVerticalGravity(global::Android.Views.GravityFlags.Center);
			dialogContent.SetPadding(20, 20, 20, 0);

			var numberFormatter = new NumberPickerFormatter();

			_hourPicker = new NumberPicker(Context)
			{
				MinValue = 0,
				MaxValue = 23,
				WrapSelectorWheel = false,
			};
			_hourPicker.Value = hours;
			_hourPicker.SetFormatter(numberFormatter);

			_minutePicker = new NumberPicker(Context)
			{
				MinValue = 0,
				MaxValue = 59,
				WrapSelectorWheel = false,
			};
			_minutePicker.Value = minutes;
			_minutePicker.SetFormatter(numberFormatter);

			_secondPicker = new NumberPicker(Context)
			{
				MinValue = 0,
				MaxValue = 59,
				WrapSelectorWheel = false,
			};
			_secondPicker.Value = seconds;
			_secondPicker.SetFormatter(numberFormatter);

			dialogContent.AddView(_hourPicker);
			dialogContent.AddView(new TextView(Context)
			{
				Text = ":"
			});
			dialogContent.AddView(_minutePicker);
			dialogContent.AddView(new TextView(Context)
			{
				Text = ":"
			});
			dialogContent.AddView(_secondPicker);

			SetView(dialogContent);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void OnCancelClicked(object sender, DialogClickEventArgs args)
		{
		}

		private void OnOkClicked(object sender, DialogClickEventArgs args)
		{
			_callback?.Invoke(this, new TimeSpan(_hourPicker.Value, _minutePicker.Value, _secondPicker.Value));
		}
	}
}