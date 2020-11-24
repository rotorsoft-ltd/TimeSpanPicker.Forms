using CoreGraphics;
using Rotorsoft.Forms;
using Rotorsoft.Forms.Platform.iOS;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TimeSpanPicker), typeof(TimeSpanPickerRenderer))]
namespace Rotorsoft.Forms.Platform.iOS
{
    public class TimeSpanPickerRenderer : ViewRenderer<TimeSpanPicker, UITextField>
	{
		private UITimeSpanPickerView _picker;
		private UIColor _defaultTextColor;
		private bool _disposed;

		[Xamarin.Forms.Internals.Preserve(Conditional = true)]
		public TimeSpanPickerRenderer()
		{
		}

		public static void Initialize()
        {
        }

		IElementController ElementController => Element as IElementController;

		protected override void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_disposed = true;

			if (disposing)
			{
				_defaultTextColor = null;

				if (_picker != null)
				{
					_picker.RemoveFromSuperview();
					_picker.Dispose();
					_picker = null;
				}

				if (Control != null)
				{
					Control.EditingDidBegin -= OnStarted;
					Control.EditingDidEnd -= OnEnded;
				}
			}

			base.Dispose(disposing);
		}


		protected override UITextField CreateNativeControl()
        {
			return new UITextField { BorderStyle = UITextBorderStyle.RoundedRect };
		}

		protected override void OnElementChanged(ElementChangedEventArgs<TimeSpanPicker> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					var entry = CreateNativeControl();

					entry.EditingDidBegin += OnStarted;
					entry.EditingDidEnd += OnEnded;

					_picker = new UITimeSpanPickerView();
					_picker.Time = e.NewElement.Time;
					_picker.MinTime = e.NewElement.MinTime;
					_picker.MaxTime = e.NewElement.MaxTime;

					var width = UIScreen.MainScreen.Bounds.Width;
					var toolbar = new UIToolbar(new CGRect(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
					var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
					var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
					{
						UpdateElementTime();
						entry.ResignFirstResponder();
					});

					toolbar.SetItems(new[] { spacer, doneButton }, false);

					entry.InputView = _picker;
					entry.InputAccessoryView = toolbar;

					entry.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
					entry.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

					entry.InputAssistantItem.LeadingBarButtonGroups = null;
					entry.InputAssistantItem.TrailingBarButtonGroups = null;

					_defaultTextColor = entry.TextColor;

					entry.AccessibilityTraits = UIAccessibilityTrait.Button;

					SetNativeControl(entry);
				}

				UpdateFont();
				UpdateTime();
				UpdateTimeConstraints();
				UpdateTextColor();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == TimeSpanPicker.TimeProperty.PropertyName || e.PropertyName == TimeSpanPicker.FormatProperty.PropertyName)
			{
				UpdateTime();
			}
			else if (e.PropertyName == TimeSpanPicker.MinTimeProperty.PropertyName || e.PropertyName == TimeSpanPicker.MaxTimeProperty.PropertyName)
            {
				UpdateTimeConstraints();
            }
			else if (e.PropertyName == TimeSpanPicker.TextColorProperty.PropertyName || e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
			{
				UpdateTextColor();
			}
			else if (e.PropertyName == TimeSpanPicker.FontFamilyProperty.PropertyName || e.PropertyName == TimeSpanPicker.FontSizeProperty.PropertyName)
			{
				UpdateFont();
			}
		}

		private void OnEnded(object sender, EventArgs eventArgs)
		{
			ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
		}

		private void OnStarted(object sender, EventArgs eventArgs)
		{
			ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
		}

		protected internal virtual void UpdateFont()
		{
			Control.Font = Font.OfSize(Element.FontFamily, Element.FontSize)
				.WithAttributes(Element.FontAttributes)
				.ToUIFont();
		}

		protected internal virtual void UpdateTextColor()
		{
			var textColor = Element.TextColor;

			if (textColor.IsDefault || !Element.IsEnabled)
			{
				Control.TextColor = _defaultTextColor;
			}
			else
			{
				Control.TextColor = textColor.ToUIColor();
			}

			// HACK This forces the color to update; there's probably a more elegant way to make this happen
			Control.Text = Control.Text;
		}

		private void UpdateTime()
		{
			_picker.Time = Element.Time;
			Control.Text = Element.Time.ToString(Element.Format);
		}

		private void UpdateTimeConstraints()
        {
			_picker.MinTime = Element.MinTime;
			_picker.MaxTime = Element.MaxTime;
		}

		private void UpdateElementTime()
		{
			ElementController.SetValueFromRenderer(TimeSpanPicker.TimeProperty, _picker.Time);
		}
	}
}