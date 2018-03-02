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

        private List<ReportInfo> mListOfAllReports;
        private List<ReportInfo> mListOfReportsEnable;

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
            mListOfAllReports = new List<ReportInfo>();
            foreach (ReportInfo x in MyParent.ReportsDictionary.Values)
            {
                mListOfAllReports.Add(x);
            }
            dgReportList.DataSource = null;
            dgReportList.Rows.Clear();
            dgReportList.AutoGenerateColumns = false;
            dgReportList.AllowUserToAddRows = false;
            dgReportList.DataSource = mListOfAllReports;
            dgReportList.ClearSelection();   

        }

        ReportInfo SelectedRow = new ReportInfo();

        private void dgReportList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        
        }





        private void dgReportList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
   
            if (e.ColumnIndex == 1 && e.RowIndex != -1)
            {
                DataGridViewRow selectedRow = dgReportList.Rows[e.RowIndex];
                int tempId;
                bool res = int.TryParse(selectedRow.Cells[0].Value.ToString(), out tempId);
                SelectedRow.ID = tempId;
                SelectedRow.IsEnable = true;
                SelectedRow.DisplayName = selectedRow.Cells[1].Value.ToString();
                mListOfReportsEnable.Add(SelectedRow);
              
            }

        }
    }
}


