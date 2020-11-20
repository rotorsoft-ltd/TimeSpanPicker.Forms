using System;
using UIKit;

namespace Rotorsoft.Forms.Platform.iOS
{
    internal class UITimeSpanPicker : UIPickerView
	{
		public UITimeSpanPicker()
			: base()
		{
			Model = new UITimeSpanPickerViewModel();
		}

		public TimeSpan Time
		{
			get => new TimeSpan(
				(int)SelectedRowInComponent(0),
				(int)SelectedRowInComponent(2),
				(int)SelectedRowInComponent(4));
			set
			{
				if (value < TimeSpan.Zero ||
					value > new TimeSpan(23, 59, 59))
				{
					throw new ArgumentOutOfRangeException("Time", value, "Time must be between 00:00:00 and 23:59:59");
				}

				Select(new nint(value.Hours), 0, false);
				Select(new nint(value.Minutes), 2, false);
				Select(new nint(value.Seconds), 4, false);
			}
		}

		public event EventHandler<TimeSpan> TimeSpanChanged;

		internal void RaiseTimeChanged()
		{
			TimeSpanChanged?.Invoke(this, Time);
		}
	}
}