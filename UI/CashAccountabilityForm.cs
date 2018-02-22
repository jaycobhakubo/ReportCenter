using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using GTI.Modules.ReportCenter.Business;
using GTI.Modules.ReportCenter.Data;
using WPFControls;

namespace GTI.Modules.ReportCenter.UI
{
    public partial class CashAccountabilityForm : Form
    {
        public CashAccountabilityForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Handle the Form Load event by attaching to the Cash Accountability
        /// Control's events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CashAccountabilityForm_Load(object sender, EventArgs e)
        {
            cashAccountabilityControl1.Finished += CashAccountabilityControl1Finished;
            cashAccountabilityControl1.LoadCashInfo += CashAccountabilityControl1Load;
            cashAccountabilityControl1.SaveCashInfo += CashAccountabilityControl1Save;
            cashAccountabilityControl1.LoadSessionNumbers += CashAccountabilityControl1LoadSessionNumbers;
            Visible = true;
        }
        /// <summary>
        /// Handle the Finished event of the Cash Accountability Control.
        /// Here we unhook the events and close the form.
        /// </summary>
        private void CashAccountabilityControl1Finished()
        {
            Visible = false;
            cashAccountabilityControl1.Finished -= CashAccountabilityControl1Finished;
            cashAccountabilityControl1.LoadCashInfo -= CashAccountabilityControl1Load;
            cashAccountabilityControl1.SaveCashInfo -= CashAccountabilityControl1Save;
            cashAccountabilityControl1.LoadSessionNumbers -= CashAccountabilityControl1LoadSessionNumbers;
            Close();
        }
        /// <summary>
        /// Handle the LoadSessionNumbers event from the wpf control.
        /// We call the server to get the availiable sessions for the date specified in the event args,
        /// then update the session list in the CashInfo class used as the DataContext for the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CashAccountabilityControl1LoadSessionNumbers(object sender, CashReportSessionEventArgs e)
        {
            DateTime sessionDate = (DateTime)e.SessionDate;
            // FIX: TA7438 - Remove operator id from Get Session Number List.
            List<int> sessionNumbers = GetReportSessionList.GetList(sessionDate, sessionDate);

            List<KeyValuePair<int, string>> sessionList = new List<KeyValuePair<int, string>>
                                                          {new KeyValuePair<int, string>(0, "All Sessions")};
            foreach (int num in sessionNumbers)
                sessionList.Add(new KeyValuePair<int, string>(num, "Session " + num));

            CashInfo cashInfo = cashAccountabilityControl1.DataContext as CashInfo;
            if (cashInfo != null)
            {
                cashInfo.Sessions = new ReadOnlyCollection<KeyValuePair<int, string>>(sessionList);
            }
        }

        private Point m_origLocation;
        /// <summary>
        /// Prevent the user from moving the window by resetting the location whenever
        ///   the user attempts to move the window.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            if (m_origLocation.X == 0 && m_origLocation.Y == 0)
                m_origLocation = Location;
            else
                Location = m_origLocation;
        }
        /// <summary>
        /// Handle the LoadCashInfo event from the wpf control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Cash Load args (Session date and number)</param>
        private static void CashAccountabilityControl1Load(object sender, CashReportLoadEventArgs e)
        {
            DateTime? date = e.SessionDate;
            int sessionNum = e.SessionNumber;
            GetDailySalesData.GetSales(Configuration.operatorID, (DateTime)date, sessionNum, e.Info);
        }
        /// <summary>
        /// Handle the SaveCashInfo event from the wpf control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Cash Save args (CashInfo)</param>
        private static void CashAccountabilityControl1Save(object sender, CashReportSaveEventArgs e)
        {
            SetDailySalesData.DailySales(Configuration.operatorID, e.Info);
        }
    }
}
