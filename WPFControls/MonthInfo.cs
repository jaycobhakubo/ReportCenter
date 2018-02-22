using System;
using System.ComponentModel;

namespace WPFControls
{
    public class MonthInfo : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal SysBingoSales { get; set; }
        public decimal SysOtherSales { get; set; }

        #region Entry Field Properties
        private int attendance;
        public int Attendance
        {
            get { return attendance; }
            set
            {
                if (!(attendance == value))
                {
                    attendance = value;
                    OnPropertyChanged("Attendance");
                }
            }
        }

        // Line 1
        private decimal bingoSales;
        public decimal BingoSales
        {
            get { return bingoSales; }
            set
            {
                if (!(bingoSales == value))
                {
                    bingoSales = value;
                    OnPropertyChanged("BingoSales");
                    UpdateBingoRevenue();
                }
            }
        }
        // Line 2
        private decimal jackpotSales;
        public decimal JackpotSales
        {
            get { return jackpotSales; }
            set
            {
                if (!(jackpotSales == value))
                {
                    jackpotSales = value;
                    OnPropertyChanged("JackpotSales");
                    UpdateBingoRevenue();
                }
            }
        }

        // Line 3
        private decimal otherSales;
        public decimal OtherSales
        {
            get { return otherSales; }
            set
            {
                if (!(otherSales == value))
                {
                    otherSales = value;
                    OnPropertyChanged("OtherSales");
                    UpdateBingoRevenue();
                }
            }
        }

        // Line 5
        private decimal jackpotPrizeExpense;
        public decimal JackpotPrizeExpense
        {
            get { return jackpotPrizeExpense; }
            set
            {
                if (!(jackpotPrizeExpense == value))
                {
                    jackpotPrizeExpense = value;
                    OnPropertyChanged("JackpotPrizeExpense");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 6
        private decimal otherPrizeExpense;
        public decimal OtherPrizeExpense
        {
            get { return otherPrizeExpense; }
            set
            {
                if (!(otherPrizeExpense == value))
                {
                    otherPrizeExpense = value;
                    OnPropertyChanged("OtherPrizeExpense");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 7
        private decimal workerCompExpense;
        public decimal WorkerCompExpense
        {
            get { return workerCompExpense; }
            set
            {
                if (!(workerCompExpense == value))
                {
                    workerCompExpense = value;
                    OnPropertyChanged("WorkerCompExpense");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 8
        private decimal advertisingExpense;
        public decimal AdvertisingExpense
        {
            get { return advertisingExpense; }
            set
            {
                if (!(advertisingExpense == value))
                {
                    advertisingExpense = value;
                    OnPropertyChanged("AdvertisingExpense");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 9
        private decimal hallRentalExpense;
        public decimal HallRentalExpense
        {
            get { return hallRentalExpense; }
            set
            {
                if (!(hallRentalExpense == value))
                {
                    hallRentalExpense = value;
                    OnPropertyChanged("HallRentalExpense");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 10
        private decimal equipmentExpense;
        public decimal EquipmentExpense
        {
            get { return equipmentExpense; }
            set
            {
                if (!(equipmentExpense == value))
                {
                    equipmentExpense = value;
                    OnPropertyChanged("EquipmentExpense");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 11
        private decimal otherExpenses;
        public decimal OtherExpenses
        {
            get { return otherExpenses; }
            set
            {
                if (!(otherExpenses == value))
                {
                    otherExpenses = value;
                    OnPropertyChanged("OtherExpenses");
                    UpdateBingoExpenses();
                }
            }
        }

        // Line 13
        private decimal ticketSales;
        public decimal TicketSales
        {
            get { return ticketSales; }
            set
            {
                if (!(ticketSales == value))
                {
                    ticketSales = value;
                    OnPropertyChanged("TicketSales");
                    UpdateCharityRevenue();
                }
            }
        }

        // Line 14
        private decimal prizeExpense;
        public decimal PrizeExpense
        {
            get { return prizeExpense; }
            set
            {
                if (!(prizeExpense == value))
                {
                    prizeExpense = value;
                    OnPropertyChanged("PrizeExpense");
                    UpdateCharityExpenses();
                }
            }
        }

        // Line 15
        private decimal ticketExpense;
        public decimal TicketExpense
        {
            get { return ticketExpense; }
            set
            {
                if (!(ticketExpense == value))
                {
                    ticketExpense = value;
                    OnPropertyChanged("TicketExpense");
                    UpdateCharityExpenses();
                }
            }
        }
        #endregion

        #region Calculated Properties
        // Line 4
        private void UpdateBingoRevenue()
        {
            BingoRevenue = BingoSales + JackpotSales + OtherSales;
            UpdateBingoNet();
        }
        private decimal bingoRevenue;
        public decimal BingoRevenue
        {
            get { return bingoRevenue; }
            set
            {
                if (!(bingoRevenue == value))
                {
                    bingoRevenue = value;
                    OnPropertyChanged("BingoRevenue");
                }
            }
        }
        // Line 12
        private void UpdateBingoExpenses()
        {
            BingoExpenses = JackpotPrizeExpense + OtherPrizeExpense + WorkerCompExpense + AdvertisingExpense
                            + HallRentalExpense + EquipmentExpense + OtherExpenses;
            UpdateBingoNet();
        }

        private decimal bingoExpenses;
        public decimal BingoExpenses
        {
            get { return bingoExpenses; }
            set
            {
                if (!(bingoExpenses == value))
                {
                    bingoExpenses = value;
                    OnPropertyChanged("BingoExpenses");
                }
            }
        }
        // Line 13
        private void UpdateCharityRevenue()
        {
            CharityRevenue = TicketSales;
            UpdateCharityNet();
        }
        private decimal charityRevenue;
        public decimal CharityRevenue
        {
            get { return charityRevenue; }
            set
            {
                if (!(charityRevenue == value))
                {
                    charityRevenue = value;
                    OnPropertyChanged("CharityRevenue");
                }
            }
        }
        // Line 16
        private void UpdateCharityExpenses()
        {
            CharityExpenses = PrizeExpense + TicketExpense;
            UpdateCharityNet();
        }
        private decimal charityExpenses;
        public decimal CharityExpenses
        {
            get { return charityExpenses; }
            set
            {
                if (!(charityExpenses == value))
                {
                    charityExpenses = value;
                    OnPropertyChanged("CharityExpenses");
                }
            }
        }
        // Net Profit / Loss for Bingo  (line 4 - line 12)
        private void UpdateBingoNet() { BingoNet = BingoRevenue - BingoExpenses; }
        private decimal bingoNet;
        public decimal BingoNet
        {
            get { return bingoNet; }
            set
            {
                if (!(bingoNet == value))
                {
                    bingoNet = value;
                    OnPropertyChanged("BingoNet");
                }
            }
        }
        // Net Profit / Loss for Charity  (line 16 - line 13)
        private void UpdateCharityNet() { CharityNet = CharityRevenue - CharityExpenses; }
        private decimal charityNet;
        public decimal CharityNet
        {
            get { return charityNet; }
            set
            {
                if (!(charityNet == value))
                {
                    charityNet = value;
                    OnPropertyChanged("CharityNet");
                }
            }
        }
        #endregion

        #region Header Field Properties
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (!(date == value))
                {
                    date = value;
                    OnPropertyChanged("Month");
                    OnPropertyChanged("Quarter");
                    OnPropertyChanged("MonthInQuarter");
                    OnPropertyChanged("Date");
                }
            }
        }

        public string Month
        {
            get
            {
                switch (Date.Month)
                {
                    case 1:
                        return "January";
                    case 2:
                        return "February";
                    case 3:
                        return "March";
                    case 4:
                        return "April";
                    case 5:
                        return "May";
                    case 6:
                        return "June";
                    case 7:
                        return "July";
                    case 8:
                        return "August";
                    case 9:
                        return "September";
                    case 10:
                        return "October";
                    case 11:
                        return "November";
                    default:
                        return "December";
                }
            }
        }
        public string Quarter
        {
            get { return String.Format("Q{0} - {1}", ((Date.Month - 1) / 3) + 1, Date.Year); }
        }
        public string Year { get { return Date.Year.ToString(); } }
        public string MonthInQuarter
        {
            get { return "Month " + (((Date.Month - 1) % 3) + 1); }
        }
        #endregion

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