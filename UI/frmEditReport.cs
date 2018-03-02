using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTI.Modules.Shared;

namespace GTI.Modules.ReportCenter.UI
{
    public partial class frmEditReport :  GradientForm
    {

        private List<ReportInfo> ListOfAllReports;

        public frmReportCenterMDIParent MyParent { get; private set; }
        public frmEditReport(frmReportCenterMDIParent myParent)
        {
            MyParent = myParent;
            InitializeComponent();
            //LoadDataIntoTheDataGrid();
        }

        private void frmEditReport_Load(object sender, EventArgs e)
        {
            if (MyParent.userReportMenu != null)
                MyParent.userReportMenu.Visible = false;
        }

        public void LoadDataIntoTheDataGrid()
        {
            ListOfAllReports = new List<ReportInfo>();
            foreach (ReportInfo x in MyParent.ReportsDictionary.Values)
            {
                ListOfAllReports.Add(x);
            }
            dgReportList.DataSource = null;
            dgReportList.Rows.Clear();
            dgReportList.AutoGenerateColumns = false;
            dgReportList.AllowUserToAddRows = false;
            dgReportList.DataSource = ListOfAllReports;
            dgReportList.ClearSelection();   

        }

        private void dgReportList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgReportList_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}


