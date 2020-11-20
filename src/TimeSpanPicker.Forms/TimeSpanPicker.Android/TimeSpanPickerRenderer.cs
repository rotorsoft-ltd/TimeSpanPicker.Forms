using Android.App;
using Android.Content;
using Android.Util;
using Android.Widget;
using Rotorsoft.Forms;
using Rotorsoft.Forms.Platform.Android;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TimeSpanPicker), typeof(TimeSpanPickerRenderer))]
namespace Rotorsoft.Forms.Platform.Android
{
    public abstract class TimeSpanPickerRenderer : ViewRenderer<TimeSpanPicker, EditText>, IPickerRenderer
    {
        private AlertDialog _dialog;
        private bool _disposed;
        private TextColorSwitcher _textColorSwitcher;

        public TimeSpanPickerRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        IElementController ElementController => Element as IElementController;

        protected override void OnElementChanged(ElementChangedEventArgs<TimeSpanPicker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var textField = CreateNativeControl();

                SetNativeControl(textField);
            }

            SetTimeSpan(e.NewElement.Time);
            UpdateTextColor();
            UpdateFont();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_disposed || Element == null)
            {
                return;
            }

            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == TimeSpanPicker.TimeProperty.PropertyName || e.PropertyName == TimeSpanPicker.FormatProperty.PropertyName)
            {
                SetTimeSpan(Element.Time);
            }
            else if (e.PropertyName == TimeSpanPicker.TextColorProperty.PropertyName)
            {
                UpdateTextColor();
            }
            else if (e.PropertyName == TimeSpanPicker.FontFamilyProperty.PropertyName || e.PropertyName == TimeSpanPicker.FontSizeProperty.PropertyName)
            {
                UpdateFont();
            }
        }

        protected override void OnFocusChangeRequested(object sender, VisualElement.FocusRequestArgs e)
        {
            base.OnFocusChangeRequested(sender, e);

            if (e.Focus)
            {
                if (Clickable)
                {
                    CallOnClick();
                }
                else
                {
                    ((IPickerRenderer)this)?.OnClick();
                }
            }
            else if (_dialog != null)
            {
                _dialog.Hide();
                ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);

                _dialog?.Dispose();
                _dialog = null;
            }
        }

        protected virtual AlertDialog CreateTimeSpanPickerDialog(EventHandler<TimeSpan> callback, int hours, int minutes, int seconds)
        {
            return new TimeSpanPickerDialog(Context, callback, hours, minutes, seconds);
        }

        protected override EditText CreateNativeControl()
        {
            return new PickerEditText(Context);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposing)
            {
                _dialog?.Dispose();
                _dialog = null;
            }

            base.Dispose(disposing);
        }

        void IPickerRenderer.OnClick()
        {
            if (_dialog != null && _dialog.IsShowing)
            {
                return;
            }

            TimeSpanPicker view = Element;
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);

            _dialog = CreateTimeSpanPickerDialog(OnTimeSpanPicked, view.Time.Hours, view.Time.Minutes, view.Time.Seconds);
            _dialog.Show();
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            Element.Unfocus();
        }

        private void SetTimeSpan(TimeSpan time)
        {
            Control.Text = DateTime.Today.Add(time).ToString(Element.Format);
        }

        private void UpdateFont()
        {
            Control.Typeface = Font.OfSize(Element.FontFamily, Element.FontSize)
                .WithAttributes(Element.FontAttributes)
                .ToTypeface();

            Control.SetTextSize(ComplexUnitType.Sp, (float)Element.FontSize);
        }

        private void UpdateTextColor()
        {
			_textColorSwitcher = _textColorSwitcher ?? new TextColorSwitcher(Control.TextColors, false);
			_textColorSwitcher.UpdateTextColor(Control, Element.TextColor);
        }

        private void OnTimeSpanPicked(object sender, TimeSpan args)
        {
            ElementController.SetValueFromRenderer(TimeSpanPicker.TimeProperty, args);
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);

            SetTimeSpan(args);

            _dialog?.Dispose();
            _dialog = null;
        }
    }
}
