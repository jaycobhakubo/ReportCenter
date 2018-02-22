using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFControls
{
    public class MoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert from decimal to string
            if (value != null)
            {
                decimal number = (decimal)value;
                string decString = number.ToString();
                string moneyString = QuarterlyReportControl.DecimalStringToMoneyString(decString);
                return moneyString;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert from string to decimal
            if (value != null)
            {
                string moneyString = value as string;
                string decString = QuarterlyReportControl.MoneyStringToDecimalString(moneyString);
                decimal number;
                decimal.TryParse(decString, out number);
                return number;
            }
            return 0.0m;
        }
    }
}