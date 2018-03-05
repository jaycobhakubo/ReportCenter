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
        private List<ReportInfo> mListOfReportsEnable;
        private List<ReportInfo> mListOfReportsOriginal;
        ReportInfo SelectedRow = new ReportInfo();
        ReportInfo SelectedRowOriginalValue = new ReportInfo();

        public frmReportCenterMDIParent MyParent { get; private set; }

        public frmEditReport(frmReportCenterMDIParent myParent)
        {
            MyParent = myParent;
            InitializeComponent();
            mListOfReportsEnable = new List<ReportInfo>();
            mListOfReportsOriginal = new List<ReportInfo>();
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
                rptInfo.IsEnable = true;//Just for now
                mListOfAllReports.Add(rptInfo);
                SetDataGrid();
            }        
        }


        private void SetDataGrid()
        {
            dgReportList.DataSource = null;
            dgReportList.Rows.Clear();
            dgReportList.AutoGenerateColumns = false;
            dgReportList.AllowUserToAddRows = false;
            dgReportList.DataSource = mListOfAllReports;
            dgReportList.ClearSelection();   
        }
        private void dgReportList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {  
            if ((e.ColumnIndex == 2 || e.ColumnIndex == 1)  && e.RowIndex != -1)
            {
                int tempId;
                  bool res;
               
                DataGridViewRow selectedRow = dgReportList.Rows[e.RowIndex];              
                res = int.TryParse(selectedRow.Cells[0].Value.ToString(), out tempId);
                SelectedRow.ID = tempId;           
                SelectedRow.IsEnable = Convert.ToBoolean(selectedRow.Cells[1].Value);
                SelectedRow.DisplayName = selectedRow.Cells[2].Value.ToString();
                mListOfReportsEnable.Add(SelectedRow);
                mListOfReportsOriginal.Add(SelectedRowOriginalValue);
            }
        }

        private void btnSaveReportEdit_Click(object sender, EventArgs e)
        {
            SetUserReportEnableOrDisable msg = new SetUserReportEnableOrDisable(mListOfReportsEnable);
            msg.Send();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (ReportInfo x in mListOfReportsOriginal)
            {
                int temID = x.ID;
                mListOfAllReports.FirstOrDefault(l => l.ID == temID).DisplayName = x.DisplayName;
            }

            dgReportList.Update();
            dgReportList.RefreshEdit();
            dgReportList.Refresh();
        }

        private void dgReportList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRowOriginalValue = new ReportInfo();
            DataGridViewRow selectedRowUnchanged = dgReportList.Rows[e.RowIndex];
            int tempId;
            bool res = int.TryParse(selectedRowUnchanged.Cells[0].Value.ToString(), out tempId);
            SelectedRowOriginalValue.ID = tempId;
            SelectedRowOriginalValue.IsEnable = Convert.ToBoolean(selectedRowUnchanged.Cells[1].Value);
            SelectedRowOriginalValue.DisplayName = selectedRowUnchanged.Cells[2].Value.ToString();
           // mListOfReportsOriginal.Add(SelectedRow);             
        }
    }
}


