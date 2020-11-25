using Android.Widget;

namespace Rotorsoft.Forms.Platform.Android
{
    internal class NumberPickerFormatter : Java.Lang.Object, NumberPicker.IFormatter
    {
        public string Format(int value)
        {
            return value.ToString("D2");
        }
    }
}