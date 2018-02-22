using System.Windows;
using System.Windows.Controls;

namespace WPFControls
{
    /// <summary>
    /// Interaction logic for YearControl.xaml
    /// </summary>
    public partial class YearControl
    {
        public YearControl()
        {
            InitializeComponent();
        }

        #region Value
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int),
            typeof(YearControl), new FrameworkPropertyMetadata(2009, new PropertyChangedCallback(OnValueChanged), CoerceValue));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            int newValue = (int)value;
            YearControl adjCtl = (YearControl)element;
            newValue = newValue > adjCtl.Maximum ? adjCtl.Maximum : newValue;
            newValue = newValue < adjCtl.Minimum ? adjCtl.Minimum : newValue;
            return newValue;
        }

        public static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            YearControl adjCtl = (YearControl)sender;
            adjCtl.UpdateVisibles();
        }
        #endregion
        #region Minimum
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(YearControl),
                                        new FrameworkPropertyMetadata(1990, new PropertyChangedCallback(OnMinimumChanged)));

        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        private static void OnMinimumChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            element.CoerceValue(MaximumProperty);
            element.CoerceValue(ValueProperty);
        }
        #endregion
        #region Maximum
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(YearControl),
                                        new FrameworkPropertyMetadata(2100, new PropertyChangedCallback(OnMaximumChanged), CoerceMaximum));

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static void OnMaximumChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            element.CoerceValue(ValueProperty);
        }

        private static object CoerceMaximum(DependencyObject element, object value)
        {
            YearControl adjCtl = (YearControl)element;
            int newMax = (int)value > adjCtl.Minimum ? (int)value : adjCtl.Minimum;
            return newMax;
        }
        #endregion
		
        private void UpdateVisibles()
        {
            YearText.Text = Value.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button rb = (Button)sender;
            if (rb.Name == "PrevYearButton")
                Value--;
            else
                Value++;
        }

    }
}