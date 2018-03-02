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

        public frmReportCenterMDIParent MyParent { get; private set; }
        public frmEditReport(frmReportCenterMDIParent myParent)
        {
            MyParent = myParent;
            InitializeComponent();
        }

        private void frmEditReport_Load(object sender, EventArgs e)
        {
            if (MyParent.userReportMenu != null)
                MyParent.userReportMenu.Visible = false;
        }

        private void LoadDataIntoTheDataGrid()
        {
            mListOfAllReports = new List<ReportInfo>();
            foreach (ReportInfo x in MyParent.ReportsDictionary.Values)
            {
                mListOfAllReports.Add(x);
            }

            dgReportList.DataSource = mListOfAllReports;

        }
    }
}





    //public partial class frmEditReport :  GradientForm
    //{
    //    public frmReportCenterMDIParent MyParent { get; private set; }
    //    private List<ReportInfo> mListOfAllAvailableReports;
    //    private List<ReportInfo> mListOfReportToBeModiFied;

    //    public frmEditReport(frmReportCenterMDIParent myParent, List<ReportInfo> ListOfAllAvailableReports)
    //    {       
    //        InitializeComponent();
    //        MyParent = myParent;
    //        dgReportList.AutoGenerateColumns = false;                                                                                                                                                           //dgReportList.DataSource = null;//dgReportList.Rows.Clear();//dgReportList.DataSource = ReportListDataBind; ;//dgReportList.ClearSelection();
    //        dgReportList.AllowUserToAddRows = false;

    //        //DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
    //        //chk.HeaderText = "Report Enabled";
    //        //chk.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
    //        //chk.Name = "ReprotEnabled";
    //        //chk.Width = 145;
    //        //chk.DataPropertyName = "ReprotEnabled";
    //        //dgReportList.Columns.Add(chk);

    //        //DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
    //        //column1 = new DataGridViewTextBoxColumn();
    //        //column1.Name = "DisplayName";
    //        //column1.HeaderText = "Report Name";
    //        //column1.DataPropertyName = "DisplayName";
    //        //column1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
    //        //column1.SortMode = DataGridViewColumnSortMode.NotSortable;
    //        //column1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
    //        //column1.Width = 500;
    //        //column1.ReadOnly = true;
    //        //dgReportList.Columns.Add(column1);

    //        //column1 = new DataGridViewTextBoxColumn();
    //        //column1 = new DataGridViewTextBoxColumn();
    //        //column1.Name = "FileName";
    //        //column1.HeaderText = "Report Name";
    //        //column1.DataPropertyName = "FileName";
    //        //column1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
    //        //column1.SortMode = DataGridViewColumnSortMode.NotSortable;
    //        //column1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
    //        //column1.Width = 500;
    //        //column1.ReadOnly = true;
    //        //dgReportList.Columns.Add(column1);   
            
    //      //dgReportList.ClearSelection(); 
    //        mListOfAllAvailableReports = ListOfAllAvailableReports;
    //        dgReportList.DataSource = mListOfAllAvailableReports;

    //    }

    //    private void frmEditReport_Load(object sender, EventArgs e)
    //    {
    //        if (MyParent.userReportMenu != null)
    //            MyParent.userReportMenu.Visible = false;
    //    }

    //    public Dictionary<int, ReportInfo> ReportsDictionary
    //    {
    //        get;
    //        set;
    //    }

    //    public BindingList<ReportInfo> ReportListDataBind
    //    {
    //        get;
    //        set;
    //    }

    //    public List<ReportInfo> ListOfAvailableReports
    //    {
    //        get;
    //        set;
    //    }


    //    public void LoadDataIntoTheDataGridView()          /*TEMP only for testing*/                                                                                                                                                                                     //dgReportList.DataSource = null;//dgReportList.Rows.Clear();//dgReportList.AutoGenerateColumns = true;//dgReportList.AllowUserToAddRows = false;//dgReportList.DataSource = ReportListDataBind;;//dgReportList.ClearSelection();
    //    {
       
    //    }

    //    private void acceptImageButton_Click(object sender, EventArgs e)
    //    {
    //        var temp = c;
    //        SetReportDisableOrEnable msg = new SetReportDisableOrEnable(mListOfReportToBeModiFied);
    //        msg.Send();
    //    }

    //    private void dgReportList_CellContentClick(object sender, DataGridViewCellEventArgs e)
    //    {
            
    //        ////string CurrentDisplayName;
    //        ////DataGridCell.
    //        ////MessageBox.Show(dgReportList.SelectedCells[0].Value.ToString());
    //        //var x = dgReportList.SelectedRows;
    //        //var y = dgReportList.SelectedColumns;
    //        //var z = dgReportList.SelectedCells;


    //    }

    //    DataGridViewRow c = new DataGridViewRow();


    //    private void dgReportList_CellClick(object sender, DataGridViewCellEventArgs e)
    //    {
    //        int index = e.RowIndex;
    //        DataGridViewRow selectedRow = dgReportList.Rows[index];
    //        c = dgReportList.Rows[index];
    //        var y = dgReportList.SelectedRows;
    //        ReportInfo x = new ReportInfo();
    //        x.ID = 33;
    //        x.IsEnable = true;
    //        x.DisplayName = selectedRow.Cells[1].Value.ToString();
    //        x.IsModified = true;
    //        mListOfReportToBeModiFied = new List<ReportInfo>();
    //        mListOfReportToBeModiFied.Add(x);

    //       // MessageBox.Show("Hello");
    ////         if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
    ////{
    ////   MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
    //    }


    //}

 