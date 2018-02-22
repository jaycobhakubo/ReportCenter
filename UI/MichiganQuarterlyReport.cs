using System;
using System.Drawing;
using System.Windows.Forms;
using GTI.Modules.ReportCenter.Business;
using GTI.Modules.ReportCenter.Data;
using WPFControls;

namespace GTI.Modules.ReportCenter.UI
{
    public partial class MichiganQuarterlyReport : Form
    {
        public MichiganQuarterlyReport()
        {
            InitializeComponent();
        }

        private void Report_Finished()
        {
            Visible = false;
            quarterlyReportControl1.Finished -= Report_Finished;
            quarterlyReportControl1.LoadQuarter -= QuarterlyReportControl1LoadQuarter;
            quarterlyReportControl1.SaveQuarter -= QuarterlyReportControl1SaveQuarter;
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            quarterlyReportControl1.Finished += Report_Finished;
            quarterlyReportControl1.LoadQuarter += QuarterlyReportControl1LoadQuarter;
            quarterlyReportControl1.SaveQuarter += QuarterlyReportControl1SaveQuarter;
            Visible = true;
        }

        private Point origLocation;
        /// <summary>
        /// Prevent the user from moving the window by resetting the location whenever
        ///   the user attempts to move the window.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            if (origLocation.X == 0 && origLocation.Y == 0) 
                origLocation = Location;
            else
                Location = origLocation;
        }

        private static void QuarterlyReportControl1SaveQuarter(object sender, WPFControls.ReportEventArgs e)
        {
            QuarterInfo qi = e.Info;
            SetMonthlySalesData.MonthlySales(Configuration.operatorID, qi.Month1.Date.Month, qi.Month1.Date.Year, qi.Month1);
            SetMonthlySalesData.MonthlySales(Configuration.operatorID, qi.Month2.Date.Month, qi.Month2.Date.Year, qi.Month2);
            SetMonthlySalesData.MonthlySales(Configuration.operatorID, qi.Month3.Date.Month, qi.Month3.Date.Year, qi.Month3);
        }

        private static void QuarterlyReportControl1LoadQuarter(object sender, WPFControls.ReportEventArgs e)
        {
            QuarterInfo qi = e.Info;
            GetMonthlySalesData.MonthlySales(Configuration.operatorID, qi.Month1.Date.Month, qi.Month1.Date.Year, qi.Month1);
            GetMonthlySalesData.MonthlySales(Configuration.operatorID, qi.Month2.Date.Month, qi.Month2.Date.Year, qi.Month2);
            GetMonthlySalesData.MonthlySales(Configuration.operatorID, qi.Month3.Date.Month, qi.Month3.Date.Year, qi.Month3);
            qi.Month1.SysBingoSales = GetMontlyBingoSalesData.MonthlyBingoSales(Configuration.operatorID,
                                                                             qi.Month1.Date.Month, qi.Month1.Date.Year);
            qi.Month2.SysBingoSales = GetMontlyBingoSalesData.MonthlyBingoSales(Configuration.operatorID,
                                                                             qi.Month2.Date.Month, qi.Month2.Date.Year);
            qi.Month3.SysBingoSales = GetMontlyBingoSalesData.MonthlyBingoSales(Configuration.operatorID,
                                                                             qi.Month3.Date.Month, qi.Month3.Date.Year);
            qi.Month1.SysOtherSales = GetMonthlyOtherSalesData.MonthlyOtherSales(Configuration.operatorID,
                                                                             qi.Month1.Date.Month, qi.Month1.Date.Year);
            qi.Month2.SysOtherSales = GetMonthlyOtherSalesData.MonthlyOtherSales(Configuration.operatorID,
                                                                             qi.Month2.Date.Month, qi.Month2.Date.Year);
            qi.Month3.SysOtherSales = GetMonthlyOtherSalesData.MonthlyOtherSales(Configuration.operatorID,
                                                                             qi.Month3.Date.Month, qi.Month3.Date.Year);
        }
    }
}
