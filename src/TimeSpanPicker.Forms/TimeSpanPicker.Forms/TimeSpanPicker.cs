using System;
using Xamarin.Forms;

namespace Rotorsoft.Forms
{
	public class TimeSpanPicker : TimePicker
	{
		private static readonly TimeSpan MAX_VALUE = new TimeSpan(0, 23, 59, 59, 999);

		public new static readonly BindableProperty TimeProperty = BindableProperty.Create(
			nameof(Time),
			typeof(TimeSpan),
			typeof(TimeSpanPicker),
			TimeSpan.Zero,
			BindingMode.TwoWay,
			validateValue: (bindable, value) =>
			{
				var time = (TimeSpan)value;

				return time >= TimeSpan.Zero &&
					time <= MAX_VALUE;
			},
			coerceValue: CoerceTime);

		public static readonly BindableProperty MinTimeProperty = BindableProperty.Create(
			nameof(MinTime),
			typeof(TimeSpan),
			typeof(TimeSpanPicker),
			TimeSpan.Zero,
			BindingMode.TwoWay,
			validateValue: (bindable, value) =>
			{
				var minTime = (TimeSpan)value;
				var maxTime = (TimeSpan)bindable.GetValue(MaxTimeProperty);

				return minTime >= TimeSpan.Zero &&
					minTime <= MAX_VALUE &&
					minTime < maxTime;
			},
			propertyChanged: ForceCoerceTime);

		public static readonly BindableProperty MaxTimeProperty = BindableProperty.Create(
			nameof(MaxTime),
			typeof(TimeSpan),
			typeof(TimeSpanPicker),
			MAX_VALUE,
			BindingMode.TwoWay,
			validateValue: (bindable, value) =>
			{
				var minTime = (TimeSpan)bindable.GetValue(MinTimeProperty);
				var maxTime = (TimeSpan)value;

				return maxTime >= TimeSpan.Zero &&
					maxTime <= MAX_VALUE &&
					maxTime > minTime;
			},
			propertyChanged: ForceCoerceTime);

		public new static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(TimeSpanPicker), "T");

		public new TimeSpan Time
		{
			get => (TimeSpan)GetValue(TimeProperty);
			set => SetValue(TimeProperty, value);
		}

		public TimeSpan MinTime
		{
			get => (TimeSpan)GetValue(MinTimeProperty);
			set => SetValue(MinTimeProperty, value);
		}

		public TimeSpan MaxTime
		{
			get => (TimeSpan)GetValue(MaxTimeProperty);
			set => SetValue(MaxTimeProperty, value);
		}

		public new string Format
		{
			get { return (string)GetValue(FormatProperty); }
			set { SetValue(FormatProperty, value); }
		}

		static object CoerceTime(BindableObject bindable, object value)
		{
			var time = (TimeSpan)value;
			var minTime = (TimeSpan)bindable.GetValue(MinTimeProperty);
			var maxTime = (TimeSpan)bindable.GetValue(MaxTimeProperty);

			if (time < minTime)
			{
				time = minTime;
			}
			else if (time > maxTime)
			{
				time = maxTime;
			}

			return time;
		}

		static void ForceCoerceTime(BindableObject bindable, object oldValue, object newValue)
		{
			// Workaround for BindableProperty.CoerceValue not working when explicitly called
			var timeSpanPicker = (TimeSpanPicker)bindable;
			var coercedTime = (TimeSpan)CoerceTime(timeSpanPicker, timeSpanPicker.Time);
			timeSpanPicker.Time = coercedTime;
		}
	}
}
