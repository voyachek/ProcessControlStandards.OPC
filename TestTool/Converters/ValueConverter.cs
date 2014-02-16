using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ProcessControlStandards.OPC.TestTool.Converters
{
    public class ValueConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (value.GetType().IsArray)
            {
                var valueArray = (Array) value;
                var strings = new string[valueArray.Length];
                for (var i = 0; i < strings.Length; ++i)
                    strings[i] = System.Convert.ToString(valueArray.GetValue(i));
                return string.Join("; ", strings);
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Converter;
        }

        private static readonly ValueConverter Converter = new ValueConverter();
    }
}
