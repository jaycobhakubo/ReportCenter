using System;
using System.ComponentModel;

namespace WPFControls
{
    public class QuarterInfo : INotifyPropertyChanged
    {
        public QuarterInfo()
        {
            Month1 = new MonthInfo();
            Month2 = new MonthInfo();
            Month3 = new MonthInfo();
            Month1.PropertyChanged += MonthPropertyChanged;
            Month2.PropertyChanged += MonthPropertyChanged;
            Month3.PropertyChanged += MonthPropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private int quarter;
        public int Quarter
        {
            get { return quarter; }
            set
            {
                if (!(quarter == value))
                {
                    quarter = value;
                    Update();
                    OnPropertyChanged("Quarter");
                }
            }
        }

        private int year;
        public int Year
        {
            get { return year; }
            set
            {
                if (!(year == value))
                {
                    year = value;
                    Update();
                    OnPropertyChanged("Year");
                }
            }
        }

        private void Update()
        {
            switch (quarter)
            {
                case 0:
                    return;
                case 1:
                    Month1.Date = new DateTime(year, 1, 1);
                    Month2.Date = new DateTime(year, 2, 1);
                    Month3.Date = new DateTime(year, 3, 1);
                    break;
                case 2:
                    Month1.Date = new DateTime(year, 4, 1);
                    Month2.Date = new DateTime(year, 5, 1);
                    Month3.Date = new DateTime(year, 6, 1);
                    break;
                case 3:
                    Month1.Date = new DateTime(year, 7, 1);
                    Month2.Date = new DateTime(year, 8, 1);
                    Month3.Date = new DateTime(year, 9, 1);
                    break;
                case 4:
                    Month1.Date = new DateTime(year, 10, 1);
                    Month2.Date = new DateTime(year, 11, 1);
                    Month3.Date = new DateTime(year, 12, 1);
                    break;
            }
        }

        public void Load()
        {
            Month1.Attendance = 0;
            Month1.BingoSales = 0m;
            Month1.JackpotSales = 0m;
            Month1.OtherSales = 0m;
            Month1.JackpotPrizeExpense = 0m;
            Month1.OtherPrizeExpense = 0m;
            Month1.WorkerCompExpense = 0m;
            Month1.AdvertisingExpense = 0m;
            Month1.HallRentalExpense = 0m;
            Month1.EquipmentExpense = 0m;
            Month1.OtherExpenses = 0m;
            Month1.TicketSales = 0m;
            Month1.PrizeExpense = 0m;
            Month1.TicketExpense = 0m;

            Month2.Attendance = 0;
            Month2.BingoSales = 0m;
            Month2.JackpotSales = 0m;
            Month2.OtherSales = 0m;
            Month2.JackpotPrizeExpense = 0m;
            Month2.OtherPrizeExpense = 0m;
            Month2.WorkerCompExpense = 0m;
            Month2.AdvertisingExpense = 0m;
            Month2.HallRentalExpense = 0m;
            Month2.EquipmentExpense = 0m;
            Month2.OtherExpenses = 0m;
            Month2.TicketSales = 0m;
            Month2.PrizeExpense = 0m;
            Month2.TicketExpense = 0m;

            Month3.Attendance = 0;
            Month3.BingoSales = 0m;
            Month3.JackpotSales = 0m;
            Month3.OtherSales = 0m;
            Month3.JackpotPrizeExpense = 0m;
            Month3.OtherPrizeExpense = 0m;
            Month3.WorkerCompExpense = 0m;
            Month3.AdvertisingExpense = 0m;
            Month3.HallRentalExpense = 0m;
            Month3.EquipmentExpense = 0m;
            Month3.OtherExpenses = 0m;
            Month3.TicketSales = 0m;
            Month3.PrizeExpense = 0m;
            Month3.TicketExpense = 0m;
        }

        
        private void MonthPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
            switch (e.PropertyName)
            {
                case "BingoSales":
                case "JackpotSales":
                case "OtherSales":
                    OnPropertyChanged("BingoRevenue");
                    OnPropertyChanged("BingoNet");
                    break;
                case "JackpotPrizeExpense":
                case "OtherPrizeExpense":
                case "WorkerCompExpense":
                case "AdvertisingExpense":
                case "HallRentalExpense":
                case "EquipmentExpense":
                case "OtherExpenses":
                    OnPropertyChanged("BingoExpenses");
                    OnPropertyChanged("BingoNet");
                    break;
                case "TicketSales":
                    OnPropertyChanged("CharityRevenue");
                    OnPropertyChanged("CharityNet");
                    break;
                case "PrizeExpense":
                case "TicketExpense":
                    OnPropertyChanged("CharityExpenses");
                    OnPropertyChanged("CharityNet");
                    break;
            }
        }


        public MonthInfo Month1 { get; set; }
        public MonthInfo Month2 { get; set; }
        public MonthInfo Month3 { get; set; }

        public int Attendance { get { return Month1.Attendance + Month2.Attendance + Month3.Attendance; } }
        public decimal BingoSales { get { return Month1.BingoSales + Month2.BingoSales + Month3.BingoSales; } }
        public decimal JackpotSales { get { return Month1.JackpotSales + Month2.JackpotSales + Month3.JackpotSales; } }
        public decimal OtherSales { get { return Month1.OtherSales + Month2.OtherSales + Month3.OtherSales; } }
        public decimal JackpotPrizeExpense { get { return Month1.JackpotPrizeExpense + Month2.JackpotPrizeExpense + Month3.JackpotPrizeExpense; } }
        public decimal OtherPrizeExpense { get { return Month1.OtherPrizeExpense + Month2.OtherPrizeExpense + Month3.OtherPrizeExpense; } }
        public decimal WorkerCompExpense { get { return Month1.WorkerCompExpense + Month2.WorkerCompExpense + Month3.WorkerCompExpense; } }
        public decimal AdvertisingExpense { get { return Month1.AdvertisingExpense + Month2.AdvertisingExpense + Month3.AdvertisingExpense; } }
        public decimal HallRentalExpense { get { return Month1.HallRentalExpense + Month2.HallRentalExpense + Month3.HallRentalExpense; } }
        public decimal EquipmentExpense { get { return Month1.EquipmentExpense + Month2.EquipmentExpense + Month3.EquipmentExpense; } }
        public decimal OtherExpenses { get { return Month1.OtherExpenses + Month2.OtherExpenses + Month3.OtherExpenses; } }
        public decimal TicketSales { get { return Month1.TicketSales + Month2.TicketSales + Month3.TicketSales; } }
        public decimal PrizeExpense { get { return Month1.PrizeExpense + Month2.PrizeExpense + Month3.PrizeExpense; } }
        public decimal TicketExpense { get { return Month1.TicketExpense + Month2.TicketExpense + Month3.TicketExpense; } }

        // Line 4
        public decimal BingoRevenue { get { return BingoSales + JackpotSales + OtherSales; } }
        // Line 12
        public decimal BingoExpenses
        {
            get
            {
                return JackpotPrizeExpense + OtherPrizeExpense + WorkerCompExpense + AdvertisingExpense
                       + HallRentalExpense + EquipmentExpense + OtherExpenses;
            }
        }
        // Line 13
        public decimal CharityRevenue { get { return TicketSales; } }
        // Line 16
        public decimal CharityExpenses { get { return PrizeExpense + TicketExpense; } }
        // Net Profit / Loss for Bingo  (line 4 - line 12)
        public decimal BingoNet { get { return (BingoRevenue - BingoExpenses); } }
        // Net Profit / Loss for Charity  (line 16 - line 13)
        public decimal CharityNet { get { return (CharityRevenue - CharityExpenses); } }
    }
}