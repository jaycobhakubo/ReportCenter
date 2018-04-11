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
using System.Text.RegularExpressions;
using GTI.Modules.ReportCenter.Properties;

namespace GTI.Modules.ReportCenter.UI
{
    public partial class frmEditReport : GradientForm
    {
        //Private Member's
        private List<ReportData> mListOfAllReports;
        private List<ReportData> mListOfReportsEnable;
        private List<ReportData> mListOfReportsOriginal;
        ReportData SelectedRow = new ReportData();
        ReportData SelectedRowOriginalValue = new ReportData();
        public frmReportCenterMDIParent MyParent { get; private set; }
        private GetUserDefineReports getUserDefineReportsMsg;


        //Constructors
        public frmEditReport(frmReportCenterMDIParent myParent)
        {
            MyParent = myParent;
            InitializeComponent();
            mListOfReportsEnable = new List<ReportData>();
            mListOfReportsOriginal = new List<ReportData>();
        }

        #region Method

        public void HideReportMenu()  //Delete tool strip menu. //UI keep shifting UI when this is active it should only be active in standard report.
        {
            if (MyParent.userReportMenu != null)
            {

                MyParent.userReportMenu.Visible = false;
                MyParent.userReportMenu.Hide();
                MyParent.userReportMenu.Dispose();
                MyParent.userReportMenu = null;
                dgReportList.Refresh();
              
            }
        }

        //Get all list of report 
        public void LoadDataIntoTheDataGrid()
        {
            mListOfAllReports = new List<ReportData>();
            getUserDefineReportsMsg = new GetUserDefineReports();
            getUserDefineReportsMsg.Send();
            mListOfAllReports = getUserDefineReportsMsg.mListRptData;
            SetDataGrid();
        }

        //Update the whole report data
        private void UpdateOtherReportUI()
        {
            IsRefreshRequired = true;
            Cursor.Current = Cursors.WaitCursor;
            MyParent.RefreshReport();
            Cursor.Current = Cursors.Default;
        }


        //display UI 
        private void SetDataGrid()
        {
            dgReportList.DataSource = null;
            dgReportList.Rows.Clear();
            dgReportList.AutoGenerateColumns = false;
            dgReportList.AllowUserToAddRows = false;
            dgReportList.DataSource = mListOfAllReports;
            Sort("ReportDisplayName", SortOrder.Ascending);
            dgReportList.ClearSelection();
        }

        //ADD original value and modifieed vale
        private void AddRowData(ReportData SelectedRow)
        {
            var f = mListOfReportsEnable.Exists(l => l.ReportId == SelectedRow.ReportId);
            if (f == true)
            {
                mListOfReportsEnable.RemoveAll(l => l.ReportId == SelectedRow.ReportId);
            }
           
            mListOfReportsEnable.Add(SelectedRow);
            mListOfReportsOriginal.Add(SelectedRowOriginalValue);
        }

        //SORT 
        private void Sort(string column, SortOrder sortOrder)
        {
            switch (column)
            {
                case "ReportDisplayName":
                    {
                        if (sortOrder == SortOrder.Ascending)
                        {
                            dgReportList.DataSource = mListOfAllReports.OrderBy(x => x.ReportDisplayName).ToList();
                        }
                        else
                        {
                            dgReportList.DataSource = mListOfAllReports.OrderByDescending(x => x.ReportDisplayName).ToList();
                        }
                        break;
                    }
                case "ReportType":
                    {
                        if (sortOrder == SortOrder.Ascending)
                        {
                            dgReportList.DataSource = mListOfAllReports.OrderBy(x => x.ReportType).ToList();
                        }
                        else
                        {
                            dgReportList.DataSource = mListOfAllReports.OrderByDescending(x => x.ReportType).ToList();
                        }
                        break;
                    }
            }
        }

        public bool IsModified()
        {
            bool result = false;
            if (selectedRowUnchanged.Index != -1)
            {
                bool Original = Convert.ToBoolean(selectedRowUnchanged.Cells[1].Value);
                bool Edited = Convert.ToBoolean(selectedRowUnchanged.Cells[1].EditedFormattedValue);
                if (Original != Edited)
                {
                    result = true;
                }

                if (selectedRowUnchanged.Cells[1].Value.ToString() != selectedRowUnchanged.Cells[2].EditedFormattedValue.ToString())
                {
                    result = true;
                }
            }
            return result;
        }


        private void Saved()
        {
            if (mListOfReportsEnable.Count > 0)
            {
                SetUserDefineReports msg = new SetUserDefineReports(mListOfReportsEnable);
                msg.Send();
                UpdateOtherReportUI();
                mListOfReportsEnable = new List<ReportData>();
                selectedRowUnchanged = new DataGridViewRow();
                mListOfReportsEnable = new List<ReportData>();
                mListOfReportsOriginal = new System.Collections.Generic.List<ReportData>();
            }
        }

        private void RevertAllChanges()
        {
            foreach (ReportData x in mListOfReportsOriginal)
            {
                int temID = x.ReportId;
                mListOfAllReports.FirstOrDefault(l => l.ReportId == temID).ReportDisplayName = x.ReportDisplayName;
                mListOfAllReports.FirstOrDefault(l => l.ReportId == temID).IsActive = x.IsActive;
            }
            selectedRowUnchanged = new DataGridViewRow();
            mListOfReportsEnable = new List<ReportData>();
            mListOfReportsOriginal = new System.Collections.Generic.List<ReportData>();
            dgReportList.Update();
            dgReportList.RefreshEdit();
            dgReportList.Refresh();
        }

        #endregion

        #region Event's
        private void btnSaveReportEdit_Click(object sender, EventArgs e)
        {
            Saved();
        }

        //CANCEL
        private void btnCancel_Click(object sender, EventArgs e)
        {
            RevertAllChanges();
        }

        //Validate user input no blank name report
        private void dgReportList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
            {
                dgReportList.CancelEdit();
                e.Cancel = true;
            }
        }

        //Cell Click
        private DataGridViewRow selectedRowUnchanged = new DataGridViewRow();
        private void dgReportList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                SelectedRowOriginalValue = new ReportData();
                selectedRowUnchanged = dgReportList.Rows[e.RowIndex];
                int tempId;
                bool res = int.TryParse(selectedRowUnchanged.Cells[0].Value.ToString(), out tempId);
                SelectedRowOriginalValue.ReportId = tempId;
                SelectedRowOriginalValue.IsActive = Convert.ToBoolean(selectedRowUnchanged.Cells[1].Value);
                SelectedRowOriginalValue.ReportDisplayName = selectedRowUnchanged.Cells[2].Value.ToString();
                SelectedRowOriginalValue.ReportFileName = selectedRowUnchanged.Cells[3].Value.ToString();
            }
        }

    

        //Header click for sorting
        private void dgReportList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            DataGridView grid = (DataGridView)sender;
            SortOrder so = SortOrder.None;
            if (grid.Columns[e.ColumnIndex].Name == "ReportDisplayName" || grid.Columns[e.ColumnIndex].Name == "ReportType")
            {
                if (grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ||
                    grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    so = SortOrder.Descending;
                }
                else
                {
                    so = SortOrder.Ascending;
                }
                //set SortGlyphDirection after databinding otherwise will always be none 
                Sort(grid.Columns[e.ColumnIndex].Name, so);
                grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = so;
            }
        }

  

        //Cell value changed
        private void dgReportList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 2 || e.ColumnIndex == 1) && e.RowIndex != -1)
            {
                int tempId;
                bool res;
                DataGridViewRow selectedRow = new DataGridViewRow();
                selectedRow = dgReportList.Rows[e.RowIndex];
                SelectedRow = new ReportData();
                res = int.TryParse(selectedRow.Cells[0].Value.ToString(), out tempId);
                SelectedRow.ReportId = tempId;
                SelectedRow.IsActive = Convert.ToBoolean(selectedRow.Cells[1].Value);
                if (selectedRow.Cells[2].Value != null)
                {
                    SelectedRow.ReportDisplayName = selectedRow.Cells[2].Value.ToString();
                }

                SelectedRow.ReportFileName = selectedRow.Cells[3].Value.ToString();
                AddRowData(SelectedRow);
            }

        }




        private void frmEditReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelClosingEvent = false;
            if (mListOfReportsEnable.Count > 0)
            {
                DialogResult result = MessageForm.Show(this, Resources.SaveChangesMessage, Resources.SaveChangesHeader, MessageFormTypes.YesNoCancel);
                this.Refresh();
                if (result == DialogResult.Yes)
                {                  
                    Saved();
                }
                else if (result == DialogResult.Cancel)
                {
                    CancelClosingEvent = true;
                    e.Cancel = true;
                }
                else if (result == DialogResult.No)
                {
                    RevertAllChanges();
                }

               
            }
        }
        #endregion


        //Properties
        public bool IsRefreshRequired { get; set; }
        public bool Is_IsActiveModify { get; set; }
        public bool CancelClosingEvent { get; set; }

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


