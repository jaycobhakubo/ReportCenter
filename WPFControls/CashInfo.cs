using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WPFControls
{
    public class CashInfo : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ReadOnlyCollection<KeyValuePair<int, string>> m_sessions;
        public ReadOnlyCollection<KeyValuePair<int, string>> Sessions
        {
            get { return m_sessions; }
            set
            {
                m_sessions = value;
                OnPropertyChanged("Sessions");
            }
        }

        private int m_sessionNumber;
        public int SessionNumber
        {
            get { return m_sessionNumber; }
            set
            {
                if (!(m_sessionNumber == value))
                {
                    m_sessionNumber = value;
                    OnPropertyChanged("SessionNumber");
                }
            }
        }

        private DateTime? m_date;
        public DateTime? Date
        {
            get { return m_date; }
            set
            {
                if (m_date != value)
                {
                    m_date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        #region Entry Field Properties
        private int m_attendance;
        public int Attendance
        {
            get { return m_attendance; }
            set
            {
                if (!(m_attendance == value))
                {
                    m_attendance = value;
                    OnPropertyChanged("Attendance");
                }
            }
        }

        public int StartTransactionNumber { get; set; }
        public int EndTransactionNumber { get; set; }
        // Line 1a
        private decimal m_bingoMasterRevenue;
        public decimal BingoMasterRevenue
        {
            get { return m_bingoMasterRevenue; }
            set
            {
                if (!(m_bingoMasterRevenue == value))
                {
                    m_bingoMasterRevenue = value;
                    OnPropertyChanged("BingoMasterRevenue");
                    UpdateBingoRevenue();
                }
            }
        }
        // Line 1b
        private decimal m_bingoDisposablerRevenue;
        public decimal BingoDisposableRevenue
        {
            get { return m_bingoDisposablerRevenue; }
            set
            {
                if (!(m_bingoDisposablerRevenue == value))
                {
                    m_bingoDisposablerRevenue = value;
                    OnPropertyChanged("BingoDisposableRevenue");
                    UpdateBingoRevenue();
                }
            }
        }
        // Line 1c
        private decimal m_bingoElectronicRevenue;
        public decimal BingoElectronicRevenue
        {
            get { return m_bingoElectronicRevenue; }
            set
            {
                if (!(m_bingoElectronicRevenue == value))
                {
                    m_bingoElectronicRevenue = value;
                    OnPropertyChanged("BingoElectronicRevenue");
                    UpdateBingoRevenue();
                }
            }
        }
        // Line 2
        private decimal m_bingoJackpotSales;
        public decimal BingoJackpotSales
        {
            get { return m_bingoJackpotSales; }
            set
            {
                if (!(m_bingoJackpotSales == value))
                {
                    m_bingoJackpotSales = value;
                    OnPropertyChanged("BingoJackpotSales");
                    UpdateBingoRevenue();
                }
            }
        }

        // Line 3
        private decimal m_bingoOtherRevenue;
        public decimal BingoOtherRevenue
        {
            get { return m_bingoOtherRevenue; }
            set
            {
                if (!(m_bingoOtherRevenue == value))
                {
                    m_bingoOtherRevenue = value;
                    OnPropertyChanged("BingoOtherRevenue");
                    UpdateBingoRevenue();
                }
            }
        }

        // Line 5
        private decimal m_bingoJackpotPrizeExpense;
        public decimal BingoJackpotPrizeExpense
        {
            get { return m_bingoJackpotPrizeExpense; }
            set
            {
                if (!(m_bingoJackpotPrizeExpense == value))
                {
                    m_bingoJackpotPrizeExpense = value;
                    OnPropertyChanged("BingoJackpotPrizeExpense");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 6
        private decimal m_bingoOtherPrizeExpense;
        public decimal BingoOtherPrizeExpense
        {
            get { return m_bingoOtherPrizeExpense; }
            set
            {
                if (!(m_bingoOtherPrizeExpense == value))
                {
                    m_bingoOtherPrizeExpense = value;
                    OnPropertyChanged("BingoOtherPrizeExpense");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 7
        private decimal m_bingoWorkerCompExpense;
        public decimal BingoWorkerCompExpense
        {
            get { return m_bingoWorkerCompExpense; }
            set
            {
                if (!(m_bingoWorkerCompExpense == value))
                {
                    m_bingoWorkerCompExpense = value;
                    OnPropertyChanged("BingoWorkerCompExpense");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 10
        private decimal m_charityTicketSales;
        public decimal CharityTicketSales
        {
            get { return m_charityTicketSales; }
            set
            {
                if (!(m_charityTicketSales == value))
                {
                    m_charityTicketSales = value;
                    OnPropertyChanged("CharityTicketSales");
                    UpdateCharityGrossProfit();
                }
            }
        }

        // Line 11
        private decimal m_charityPrizes;
        public decimal CharityPrizes
        {
            get { return m_charityPrizes; }
            set
            {
                if (!(m_charityPrizes == value))
                {
                    m_charityPrizes = value;
                    OnPropertyChanged("CharityPrizes");
                    UpdateCharityGrossProfit();
                }
            }
        }

        // Line 14
        private decimal m_startCash;
        public decimal StartCash
        {
            get { return m_startCash; }
            set
            {
                if (!(m_startCash == value))
                {
                    m_startCash = value;
                    OnPropertyChanged("StartCash");
                    UpdateCalculatedDeposit();
                }
            }
        }

        // Line 16
        private decimal m_actualDeposit;
        public decimal ActualDeposit
        {
            get { return m_actualDeposit; }
            set
            {
                if (!(m_actualDeposit == value))
                {
                    m_actualDeposit = value;
                    OnPropertyChanged("ActualDeposit");
                    UpdateDiscrepancy();
                }
            }
        }

        #endregion

        #region Calculated Properties
        // Line 4
        private void UpdateBingoRevenue()
        {
            BingoRevenue = BingoMasterRevenue + BingoDisposableRevenue + BingoElectronicRevenue +
                BingoJackpotSales + BingoOtherRevenue;
        }
        private decimal m_bingoRevenue;
        public decimal BingoRevenue
        {
            get { return m_bingoRevenue; }
            set
            {
                if (!(m_bingoRevenue == value))
                {
                    m_bingoRevenue = value;
                    OnPropertyChanged("BingoRevenue");
                    UpdateBingoCashProceeds();
                }
            }
        }
        // Line 8
        private void UpdateBingoExpenses()
        {
            BingoExpenses = BingoJackpotPrizeExpense + BingoOtherPrizeExpense + BingoWorkerCompExpense;
        }

        private decimal m_bingoExpenses;
        public decimal BingoExpenses
        {
            get { return m_bingoExpenses; }
            set
            {
                if (!(m_bingoExpenses == value))
                {
                    m_bingoExpenses = value;
                    OnPropertyChanged("BingoExpenses");
                    UpdateBingoCashProceeds();
                }
            }
        }
        // Line 9
        private void UpdateBingoCashProceeds()
        {
            BingoCashProceeds = BingoRevenue - BingoExpenses;
        }

        private decimal m_bingoCashProceeds;
        public decimal BingoCashProceeds
        {
            get { return m_bingoCashProceeds; }
            set
            {
                if (!(m_bingoCashProceeds == value))
                {
                    m_bingoCashProceeds = value;
                    OnPropertyChanged("BingoCashProceeds");
                    UpdateNetProceeds();
                }
            }
        }
        // Line 12
        private void UpdateCharityGrossProfit()
        {
            CharityGrossProfit = CharityTicketSales - CharityPrizes;
        }
        private decimal m_charityGrossProfit;
        public decimal CharityGrossProfit
        {
            get { return m_charityGrossProfit; }
            set
            {
                if (!(m_charityGrossProfit == value))
                {
                    m_charityGrossProfit = value;
                    OnPropertyChanged("CharityGrossProfit");
                    UpdateNetProceeds();
                }
            }
        }
        // Line 13
        private void UpdateNetProceeds()
        {
            NetProceeds = BingoCashProceeds + CharityGrossProfit;
        }
        private decimal m_netProceeds;
        public decimal NetProceeds
        {
            get { return m_netProceeds; }
            set
            {
                if (!(m_netProceeds == value))
                {
                    m_netProceeds = value;
                    OnPropertyChanged("NetProceeds");
                    UpdateCalculatedDeposit();
                }
            }
        }
        // Line 15
        private void UpdateCalculatedDeposit()
        {
            CalculatedDeposit = NetProceeds + StartCash;
        }
        private decimal m_calculatedDeposit;
        public decimal CalculatedDeposit
        {
            get { return m_calculatedDeposit; }
            set
            {
                if (!(m_calculatedDeposit == value))
                {
                    m_calculatedDeposit = value;
                    OnPropertyChanged("CalculatedDeposit");
                    UpdateDiscrepancy();
                }
            }
        }
        // line 17. Discrepancy
        private void UpdateDiscrepancy() { Discrepancy = CalculatedDeposit - ActualDeposit; }
        private decimal m_discrepancy;
        public decimal Discrepancy
        {
            get { return m_discrepancy; }
            set
            {
                if (!(m_discrepancy == value))
                {
                    m_discrepancy = value;
                    OnPropertyChanged("Discrepancy");
                }
            }
        }
        #endregion

        #region Fields Read/Written but not used by this module
        public DateTime ModifiedDate { get; set; }
        public decimal BingoSales { get; set; }
        public decimal AdvertisingExpense { get; set; }
        public decimal HallRentalExpense { get; set; }
        public decimal EquipmentExpense { get; set; }
        public decimal OtherExpenses { get; set; }
        public decimal TicketExpense { get; set; }
        #endregion

        private bool m_invalidDateOrSession;
        public bool InvalidDateOrSession
        {
            get { return m_invalidDateOrSession; }
            set
            {
                if (!(m_invalidDateOrSession == value))
                {
                    m_invalidDateOrSession = value;
                    OnPropertyChanged("InvalidDateOrSession");
                }
            }
        }

        private bool m_showInstructions = true;
        public bool ShowInstructions
        {
            get { return m_showInstructions; }
            set
            {
                if (!(m_showInstructions == value))
                {
                    m_showInstructions = value;
                    OnPropertyChanged("ShowInstructions");
                }
            }
        }

        public string this[string name]
        {
            get
            {
                string result = null;

                if (name == "Attendance")
                {
                    if (Attendance < 0)
                    {
                        result = "Attendance must be greater than 0.";
                    }
                }
                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }
    }
}
