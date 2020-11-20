using System;
using UIKit;

namespace Rotorsoft.Forms.Platform.iOS
{
	internal class UITimeSpanPickerViewModel : UIPickerViewModel
	{
		public static int ComponentCount => 5;

		public UITimeSpanPickerViewModel()
		{
		}

		public override nint GetComponentCount(UIPickerView pickerView)
		{
			return new nint(ComponentCount);
		}

		public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
		{
			if (component == 0)
			{
				return new nint(24);
			}

			if (component == 1 || component == 3)
			{
				return new nint(1);
			}

			return new nint(60);
		}

		public override string GetTitle(UIPickerView pickerView, nint row, nint component)
		{
			if (component == 1 ||
				component == 3)
			{
				return ":";
			}

			return row.ToString("D2");
		}

		public override void Selected(UIPickerView pickerView, nint row, nint component)
		{
			if (pickerView is UITimeSpanPicker timeSpanPicker)
			{
				timeSpanPicker.RaiseTimeChanged();
			}
		}

		public override nfloat GetComponentWidth(UIPickerView pickerView, nint component)
		{
			if (component == 1 ||
				component == 3)
			{
				return 25f;
			}

			return 50f;
		}
	}
}