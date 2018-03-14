using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using GTI.Modules.Shared;
using GTI.Modules.ReportCenter.Data;
using GTI.Modules.ReportCenter.Business;
using System.Threading;

namespace GTI.Modules.ReportCenter.UI
{
    public partial class frmEditReport :  GradientForm
    {

        private List<ReportData> mListOfAllReports;
        private List<ReportData> mListOfReportsEnable;
        private List<ReportData> mListOfReportsOriginal;
        ReportData SelectedRow = new ReportData();
        ReportData SelectedRowOriginalValue = new ReportData();
        public frmReportCenterMDIParent MyParent { get; private set; }
        private GetUserDefineReports getUserDefineReportsMsg;

        public frmEditReport(frmReportCenterMDIParent myParent)
        {
            MyParent = myParent;            
            InitializeComponent();
            mListOfReportsEnable = new List<ReportData>();
            mListOfReportsOriginal = new List<ReportData>();
         }

        private void frmEditReport_Load(object sender, EventArgs e)
        {
            if (MyParent.userReportMenu != null)
                MyParent.userReportMenu.Visible = false;
        }

        public void LoadDataIntoTheDataGrid()
        {
            mListOfAllReports = new List<ReportData>();
            getUserDefineReportsMsg = new GetUserDefineReports();
            getUserDefineReportsMsg.Send();
            mListOfAllReports = getUserDefineReportsMsg.mListRptData;
            SetDataGrid();
        }


        private void UpdateOtherReportUI()
        {       
            IsRefreshRequired = true;
            Cursor.Current = Cursors.WaitCursor;
            MyParent.RefreshReport();
            Cursor.Current = Cursors.Default;
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
                DataGridViewRow selectedRow = new DataGridViewRow();
                selectedRow = dgReportList.Rows[e.RowIndex];
                SelectedRow = new ReportData();
                res = int.TryParse(selectedRow.Cells[0].Value.ToString(), out tempId);              
                SelectedRow.ReportId = tempId;           
                SelectedRow.IsActive = Convert.ToBoolean(selectedRow.Cells[1].Value);
                SelectedRow.ReportDisplayName = selectedRow.Cells[2].Value.ToString();
                SelectedRow.ReportFileName = selectedRow.Cells[3].Value.ToString();
                AddRowData(SelectedRow);
            }
        }

        private void AddRowData(ReportData SelectedRow)
        {
            mListOfReportsEnable.Add(SelectedRow);
            mListOfReportsOriginal.Add(SelectedRowOriginalValue);
        }

        private void btnSaveReportEdit_Click(object sender, EventArgs e)
        {
            SetUserDefineReports msg = new SetUserDefineReports(mListOfReportsEnable);
            msg.Send();
            UpdateOtherReportUI();
            mListOfReportsEnable = new List<ReportData>();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (ReportData x in mListOfReportsOriginal)
            {
                int temID = x.ReportId;
                mListOfAllReports.FirstOrDefault(l => l.ReportId == temID).ReportDisplayName= x.ReportDisplayName;
            }

            dgReportList.Update();
            dgReportList.RefreshEdit();
            dgReportList.Refresh();
        }

        private void dgReportList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRowOriginalValue = new ReportData();
            DataGridViewRow selectedRowUnchanged = dgReportList.Rows[e.RowIndex];
            int tempId;
            bool res = int.TryParse(selectedRowUnchanged.Cells[0].Value.ToString(), out tempId);
            SelectedRowOriginalValue.ReportId = tempId;
            SelectedRowOriginalValue.IsActive = Convert.ToBoolean(selectedRowUnchanged.Cells[1].Value);
            SelectedRowOriginalValue.ReportDisplayName = selectedRowUnchanged.Cells[2].Value.ToString();
            SelectedRowOriginalValue.ReportFileName = selectedRowUnchanged.Cells[3].Value.ToString();
        }


        public bool IsRefreshRequired { get; set; }
        public bool Is_IsActiveModify { get; set; }
    }


    public class ReportData
    {
        public int ReportId { get; set; }
        public bool IsActive { get; set; }
        public string ReportDisplayName { get; set; }
        public string ReportFileName { get; set; }
        public int ReportTypeId { get; set; }
        public string ReportType { get; set; }
    }
}


