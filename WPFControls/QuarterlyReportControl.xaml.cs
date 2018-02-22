using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace WPFControls
{
    /// <summary>
    /// Interaction logic for QuarterlyReportControl.xaml
    /// </summary>
    public partial class QuarterlyReportControl
    {
        public QuarterlyReportControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the YearControl and QuarterControl to today.
            DateTime dt = DateTime.Now;
            DataContext = new QuarterInfo { Year = dt.Year, Quarter = ((dt.Month - 1) / 3 + 1) };
            // Disable the actual sales buttons until valid data has been loaded.
            SalesBorder.IsEnabled = false;
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
        /// Working copy of Quarterly Report information.
        /// </summary>
        private QuarterInfo m_loadedQuarter;
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
            // update the YearControl and the QuarterControl with the users selection
            m_loadedQuarter = new QuarterInfo { Year = Year.Value, Quarter = Quarter.Value };
            // set the datacontext of the control to the latest information
            DataContext = m_loadedQuarter;
            m_loadedQuarter.Load();
            Quarter.IsEnabled = false;
            Year.IsEnabled = false;
            // fill the local working copy from the database
            if (LoadQuarter != null) LoadQuarter(this, new ReportEventArgs(m_loadedQuarter));
            // hook the propertyChanged event so that we can enable the Save button.
            m_loadedQuarter.PropertyChanged += LoadedQuarterPropertyChanged;
            // enable the border around the sales buttons then
            // individually enable/disable each depending on the actual values.
            SalesBorder.IsEnabled = true;
            chkBingo1.IsEnabled = m_loadedQuarter.Month1.SysBingoSales != 0m;
            chkBingo2.IsEnabled = m_loadedQuarter.Month2.SysBingoSales != 0m;
            chkBingo3.IsEnabled = m_loadedQuarter.Month3.SysBingoSales != 0m;
            chkOther1.IsEnabled = m_loadedQuarter.Month1.SysOtherSales != 0m;
            chkOther2.IsEnabled = m_loadedQuarter.Month2.SysOtherSales != 0m;
            chkOther3.IsEnabled = m_loadedQuarter.Month3.SysOtherSales != 0m;
        }
        /// <summary>
        /// Enable the save button if one changes the quarterly information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadedQuarterPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveButton.IsEnabled = true;
        }
        /// <summary>
        /// Handle the Quit button, allow one to change quarter and year. data is NOT saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            ExitButton.Visibility = Visibility.Visible;
            LoadButton.Visibility = Visibility.Visible;
            QuitButton.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Hidden;
            Quarter.IsEnabled = true;
            Year.IsEnabled = true;
            m_loadedQuarter.PropertyChanged -= LoadedQuarterPropertyChanged;
        }
        /// <summary>
        /// Emit an event to inform the user (MichiganQuarterlyReport form) to save the changes to the database.
        /// </summary>
        /// <param name="sender">this control</param>
        /// <param name="e">event args</param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ExitButton.Visibility = Visibility.Visible;
            LoadButton.Visibility = Visibility.Visible;
            QuitButton.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Hidden;
            Quarter.IsEnabled = true;
            Year.IsEnabled = true;
            // Inform any listeners of the changed QuarterInfo.
            if (SaveQuarter != null) SaveQuarter(this, new ReportEventArgs(m_loadedQuarter));
            m_loadedQuarter.PropertyChanged -= LoadedQuarterPropertyChanged;
        }
        /// <summary>
        /// Delegate used to load/save QuarterInfo data.
        /// </summary>
        /// <param name="sender">this control</param>
        /// <param name="e">Event args with encapsulated QuarterInfo</param>
        public delegate void ReportLoadHandler(object sender, ReportEventArgs e);
        /// <summary>
        /// Event used to load the QuarterInfo class.
        /// </summary>
        public event ReportLoadHandler LoadQuarter;
        /// <summary>
        /// Event used to save the QuarterInfo class.
        /// </summary>
        public event ReportLoadHandler SaveQuarter;
        /// <summary>
        /// button click handler for the 6 sales buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActualClicked(object sender, RoutedEventArgs e)
        {
            Button cb = sender as Button;
            if (cb != null)
            {
                {
                    switch (cb.Name)
                    {
                        case "chkBingo1":
                            m_loadedQuarter.Month1.BingoSales = m_loadedQuarter.Month1.SysBingoSales;
                            break;
                        case "chkBingo2":
                            m_loadedQuarter.Month2.BingoSales = m_loadedQuarter.Month2.SysBingoSales;
                            break;
                        case "chkBingo3":
                            m_loadedQuarter.Month3.BingoSales = m_loadedQuarter.Month3.SysBingoSales;
                            break;
                        case "chkOther1":
                            m_loadedQuarter.Month1.OtherSales = m_loadedQuarter.Month1.SysOtherSales;
                            break;
                        case "chkOther2":
                            m_loadedQuarter.Month2.OtherSales = m_loadedQuarter.Month2.SysOtherSales;
                            break;
                        case "chkOther3":
                            m_loadedQuarter.Month3.OtherSales = m_loadedQuarter.Month3.SysOtherSales;
                            break;
                    }
                }
            }
        }

    }
    /// <summary>
    /// Event args class that encapsulates the QuarterInfo class and used by the Load and Save methods.
    /// </summary>
    public class ReportEventArgs : EventArgs
    {
        public QuarterInfo Info { get; private set; }

        public ReportEventArgs(QuarterInfo infomation)
        {
            Info = infomation;
        }
    }
}