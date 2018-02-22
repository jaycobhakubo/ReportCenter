using System.Windows;
using System.Windows.Controls;

namespace WPFControls
{
    /// <summary>
    /// Interaction logic for YearControl.xaml
    /// </summary>
    public partial class QuarterControl
    {
        public QuarterControl()
        {
            InitializeComponent();
        }
        #region Value
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int),
            typeof(QuarterControl), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(OnValueChanged), CoerceValue));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            int newValue = (int)value;
            newValue = newValue > 4 ? 4 : newValue;
            newValue = newValue < 1 ? 1 : newValue;
            return newValue;
        }

        public static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            QuarterControl adjCtl = (QuarterControl)sender;
            adjCtl.QuarterText.Text = "Q" + adjCtl.Value;
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button rb = (Button)sender;
            if (rb.Name == "PrevQuarterButton")
                Value--;
            else
                Value++;
        }

    }
}