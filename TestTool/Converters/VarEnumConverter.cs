using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Data;
using System.Windows.Markup;

namespace ProcessControlStandards.OPC.TestTool.Converters
{
    public class VarEnumConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (VarEnum)value == VarEnum.VT_EMPTY)
                return string.Empty;

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

        private static readonly VarEnumConverter Converter = new VarEnumConverter();
    }
}
