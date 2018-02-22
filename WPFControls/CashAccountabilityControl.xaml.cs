using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace WPFControls
{
    /// <summary>
    /// Interaction logic for CashAccountabilityControl.xaml
    /// </summary>
    public partial class CashAccountabilityControl
    {
        public CashAccountabilityControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Set DataContext on the view to our data class
        /// </summary>
        /// <param name="sender">this class</param>
        /// <param name="e">event args</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new CashInfo();
            LoadButton.IsEnabled = false;
        }
        /// <summary>
        /// Specify the delegate for the "Exit" button event handler.
        /// </summary>
        public delegate void DoneHandler();
        /// <summary>
        /// Specify the event for the "Exit" button.
        /// </summary>
        public event DoneHandler Finished;
        /// <summary>
        /// Handle the click event for the "Exit" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (Finished != null) Finished();
        }

        #region WPF Money Text field handling
        private static NumberFormatInfo GetNumberFormat()
        {
            NumberFormatInfo myFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            switch (myFormat.CurrencyNegativePattern)
            {
                case 0: myFormat.CurrencyNegativePattern = 2;
                    break;
                case 4: myFormat.CurrencyNegativePattern = 5;
                    break;
                case 14: myFormat.CurrencyNegativePattern = 12;
                    break;
                case 15: myFormat.CurrencyNegativePattern = 8;
                    break;
            }
            return myFormat;
        }

        public static string DecimalStringToMoneyString(string decimalInput)
        {
            string decimalString = string.IsNullOrEmpty(decimalInput) ? "0.00" : decimalInput;
            decimalString = decimalString.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, "");
            decimal result;
            decimal.TryParse(decimalString, out result);
            return result.ToString("c", GetNumberFormat());
        }

        public static string MoneyStringToDecimalString(string moneyInput)
        {
            return moneyInput.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, "");
        }

        private void Money_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && tb.IsFocused)
            {
                tb.Text = MoneyStringToDecimalString(tb.Text);
                tb.SelectAll();
            }
        }

        private void Money_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && !tb.IsFocused)
            {
                tb.Text = DecimalStringToMoneyString(tb.Text);
            }
        }

        private void Decimal_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && tb.IsFocused)
            {
                tb.SelectAll();
            }
        }
        #endregion

        /// <summary>
        /// Working copy of Cash Accountability information.
        /// </summary>
        private CashInfo m_loadedCashInfo;
        /// <summary>
        /// Handle the click event for the Load Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            // When quarterly data is load, we show the Quit and Save buttons
            ExitButton.Visibility = Visibility.Hidden;
            LoadButton.Visibility = Visibility.Hidden;
            QuitButton.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Visible;
            // disable the save button till something changes.
            SaveButton.IsEnabled = false;
            m_loadedCashInfo = (CashInfo)DataContext;
            // fill the local working copy from the database
            if (LoadCashInfo != null) 
                LoadCashInfo(this, 
                    new CashReportLoadEventArgs(m_loadedCashInfo.Date, m_loadedCashInfo.SessionNumber, m_loadedCashInfo));
            // hook the propertyChanged event so that we can enable the Save button.
            m_loadedCashInfo.PropertyChanged += LoadedCashInfoPropertyChanged;
            comboBoxSession.IsEnabled = false;
            datePicker1.IsEnabled = false;
        }
        /// <summary>
        /// Enable the save button if one changes the information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadedCashInfoPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveButton.IsEnabled = true;
        }
        /// <summary>
        /// Handle the Quit button, allow one to change session and date. data is NOT saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            ExitButton.Visibility = Visibility.Visible;
            LoadButton.Visibility = Visibility.Visible;
            QuitButton.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Hidden;
            m_loadedCashInfo.PropertyChanged -= LoadedCashInfoPropertyChanged;
            comboBoxSession.IsEnabled = true;
            datePicker1.IsEnabled = true;
            m_loadedCashInfo.ShowInstructions = true;
            LoadButton.IsEnabled = false;
        }
        /// <summary>
        /// Emit an event to inform the user (CashAccountability form) to save the changes to the database.
        /// </summary>
        /// <param name="sender">this control</param>
        /// <param name="e">event args</param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ExitButton.Visibility = Visibility.Visible;
            LoadButton.Visibility = Visibility.Visible;
            QuitButton.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Hidden;
            if (SaveCashInfo != null) SaveCashInfo(this, new CashReportSaveEventArgs(m_loadedCashInfo));
            m_loadedCashInfo.PropertyChanged -= LoadedCashInfoPropertyChanged;
            comboBoxSession.IsEnabled = true;
            datePicker1.IsEnabled = true;
        }
        /// <summary>
        /// Delegate used to load the Cash Report. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Load Event Args</param>
        public delegate void CashReportLoadHandler(object sender, CashReportLoadEventArgs e);
        /// <summary>
        /// The event indicating we need a Cash Report loaded.
        /// </summary>
        public event CashReportLoadHandler LoadCashInfo;
        /// <summary>
        /// Delegate used to save the Cash Report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Save Event args.</param>
        public delegate void CashReportSaveHandler(object sender, CashReportSaveEventArgs e);
        /// <summary>
        /// The event indicating we need a Cash Report Saved.
        /// </summary>
        public event CashReportSaveHandler SaveCashInfo;
        /// <summary>
        /// Delegate used to load the session for the specified date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Load session event args</param>
        public delegate void CashReportSessionHandler(object sender, CashReportSessionEventArgs e);
        /// <summary>
        /// The event indicating we need session numbers for the specified date.
        /// </summary>
        public event CashReportSessionHandler LoadSessionNumbers;
        /// <summary>
        /// Handle the closing of the DatePicker. The LoadSessionNumber event called to get the 
        /// list of sessions for the specified date and loaded into the combobox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datePicker1_CalendarClosed(object sender, RoutedEventArgs e)
        {
            DatePicker picker = sender as DatePicker;
            if (picker != null)
            {
                DateTime? dt = picker.SelectedDate;
                if (dt != null)
                {
                    if (LoadSessionNumbers != null)
                    {
                        LoadSessionNumbers(this, new CashReportSessionEventArgs(dt));
                        CashInfo ci = DataContext as CashInfo;
                        if (ci != null)
                        {
                            ci.SessionNumber = 0;
                            comboBoxSession.IsEnabled = true;
                            LoadButton.IsEnabled = true;
                            ci.ShowInstructions = false;
                        }
                    }
                }
            }
        }

        private void datePicker1_CalendarOpened(object sender, RoutedEventArgs e)
        {
            CashInfo ci = DataContext as CashInfo;
            if (ci != null)
            {
                ci.InvalidDateOrSession = false;
            }
        }

        private void comboBoxSession_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CashInfo ci = DataContext as CashInfo;
            if (ci != null)
            {
                if (ci.Date != null)
                {
                    ci.ShowInstructions = ci.InvalidDateOrSession = false;
                    LoadButton.IsEnabled = true;
                }
            }
        }
    }
    /// <summary>
    /// Event Args for the Save Cash Report event.
    /// </summary>
    public class CashReportSaveEventArgs : EventArgs
    {
        public CashInfo Info { get; private set; }
        public CashReportSaveEventArgs(CashInfo infomation)
        {
            Info = infomation;
        }
    }
    /// <summary>
    /// Args for getting the session numbers for the specified date.
    /// </summary>
    public class CashReportSessionEventArgs : EventArgs
    {
        public DateTime? SessionDate { get; private set; }
        public CashReportSessionEventArgs(DateTime? sessionDate)
        {
            SessionDate = sessionDate;
        }
    }
    /// <summary>
    ///  Args for loading a new Cash Report from the server.
    /// </summary>
    public class CashReportLoadEventArgs : EventArgs
    {
        public DateTime? SessionDate { get; private set; }
        public int SessionNumber { get; private set; }
        public CashInfo Info { get; private set; }
        public CashReportLoadEventArgs(DateTime? sessionDate, int sessionNumber, CashInfo info)
        {
            SessionDate = sessionDate;
            SessionNumber = sessionNumber;
            Info = info;
        }
    }
}