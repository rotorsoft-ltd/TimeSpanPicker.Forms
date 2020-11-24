using System;
using Xamarin.Forms;

namespace Rotorsoft.Forms
{
	/// <summary>
	/// A <see cref="View"/> control that provides <see cref="TimeSpan"/> picking.
	/// </summary>
	public class TimeSpanPicker : TimePicker
	{
		private static readonly TimeSpan MAX_VALUE = new TimeSpan(0, 23, 59, 59, 999);

		/// <summary>
		/// Backing store for the <see cref="Time"/> property.
		/// </summary>
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

		/// <summary>
		/// Backing store for the <see cref="MinTime"/> property.
		/// </summary>
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
					minTime <= maxTime;
			},
			propertyChanged: ForceCoerceTime);

		/// <summary>
		/// Backing store for the <see cref="MaxTime"/> property.
		/// </summary>
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
					maxTime >= minTime;
			},
			propertyChanged: ForceCoerceTime);

		/// <summary>
		/// Backing store for the <see cref="Format"/> property.
		/// </summary>
		public new static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(TimeSpanPicker), "T");

		/// <summary>
		/// Gets or sets the displayed <see cref="TimeSpan"/>. This is a bindable property.
		/// </summary>
		/// <value>The <see cref="TimeSpan"/> displayed in the <see cref="TimeSpanPicker"/>.</value>
		/// <remarks>Valid values are in the 00:00:00 - 23:59:59 range and will be adjusted to fit the <see cref="MinTime"/> - <see cref="MaxTime"/> range.</remarks>
		public new TimeSpan Time
		{
			get => (TimeSpan)GetValue(TimeProperty);
			set => SetValue(TimeProperty, value);
		}

		/// <summary>
		/// Gets or sets the minimum <see cref="TimeSpan"/> value for picking. This is a bindable property.
		/// </summary>
		/// <value>The minimum <see cref="TimeSpan"/> value that will be selectable in the the <see cref="TimeSpanPicker"/>.</value>
		/// <remarks>Valid values are in the 00:00:00 - 23:59:59 range and must be less or equal than <see cref="MaxTime"/>.</remarks>
		public TimeSpan MinTime
		{
			get => (TimeSpan)GetValue(MinTimeProperty);
			set => SetValue(MinTimeProperty, value);
		}

		/// <summary>
		/// Gets or sets the maximum <see cref="TimeSpan"/> value for picking. This is a bindable property.
		/// </summary>
		/// <value>The maximum <see cref="TimeSpan"/> value that will be selectable in the the <see cref="TimeSpanPicker"/>.</value>
		/// <remarks>Valid values are in the 00:00:00 - 23:59:59 range and must be greater or equal than <see cref="MinTime"/>.</remarks>
		public TimeSpan MaxTime
		{
			get => (TimeSpan)GetValue(MaxTimeProperty);
			set => SetValue(MaxTimeProperty, value);
		}

		/// <summary>
		/// The format of the time to display to the user. This is a bindable property.
		/// </summary>
		/// <value>A valid time format string.</value>
		/// <remarks>Format string is the same is passed to <see cref="TimeSpan.ToString(string)"/>.</remarks>
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
