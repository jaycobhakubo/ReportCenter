using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTI.Modules.Shared;
using GTI.Modules.ReportCenter.Data;

namespace GTI.Modules.ReportCenter.UI
{
    public partial class frmEditReport :  GradientForm
    {

        private List<ReportInfo> mListOfAllReports;
        private List<ReportInfo> mRevertReportList;
        private List<ReportInfo> mListOfReportsEnable;
        ReportInfo SelectedRow = new ReportInfo();
        public frmReportCenterMDIParent MyParent { get; private set; }

        public frmEditReport(frmReportCenterMDIParent myParent)
        {
            MyParent = myParent;
            InitializeComponent();
            mListOfReportsEnable = new List<ReportInfo>();
            mRevertReportList = new List<ReportInfo>();
         }

        private void frmEditReport_Load(object sender, EventArgs e)
        {
            if (MyParent.userReportMenu != null)
                MyParent.userReportMenu.Visible = false;
        }

        public void LoadDataIntoTheDataGrid()
        {
            mListOfAllReports = new List<ReportInfo>();
            foreach (ReportInfo rptInfo in MyParent.ReportsDictionary.Values)
            {
                mListOfAllReports.Add(rptInfo);
                mRevertReportList.Add(rptInfo);
            }
            dgReportList.DataSource = null;
            dgReportList.Rows.Clear();
            dgReportList.AutoGenerateColumns = false;
            dgReportList.AllowUserToAddRows = false;
            dgReportList.DataSource = mListOfAllReports;
            dgReportList.ClearSelection();   
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

        private void btnSaveReportEdit_Click(object sender, EventArgs e)
        {
            SetUserReportEnableOrDisable msg = new SetUserReportEnableOrDisable(mListOfReportsEnable);
            msg.Send();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mListOfAllReports = mRevertReportList;
            dgReportList.DataSource = null;
            dgReportList.Rows.Clear();
            dgReportList.AutoGenerateColumns = false;
            dgReportList.AllowUserToAddRows = false;
            dgReportList.DataSource = mListOfAllReports;
            dgReportList.ClearSelection();   


        }

        //private void dgReportList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        //{
        //    e.Cancel = true;
        //}

        //private void dgReportList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        //{
        //    e.Cancel = true;

        //}
    }
}


