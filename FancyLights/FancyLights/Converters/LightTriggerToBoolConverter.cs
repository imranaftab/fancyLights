using FancyLights.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FancyLights.Converters
{
    public class LightTriggerToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lightTrigger = (LightTrigger)value;
            if (lightTrigger == LightTrigger.AlwaysOn)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (bool)value;
            if (result)
                return LightTrigger.AlwaysOn;
            else
                return LightTrigger.Auto;
        }
    }
}
