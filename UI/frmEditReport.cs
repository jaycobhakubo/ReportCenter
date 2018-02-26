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
        public frmReportCenterMDIParent MyParent { get; private set; }

        public frmEditReport(frmReportCenterMDIParent myParent, BindingList<ReportInfo> ReportListDataBind2)
        {
        
            InitializeComponent();
            MyParent = myParent;

            //dgReportList.DataSource = null;
            //dgReportList.Rows.Clear();
            dgReportList.AutoGenerateColumns = false;
            dgReportList.AllowUserToAddRows = false;
            //dgReportList.DataSource = ReportListDataBind; ;
            //dgReportList.ClearSelection();

            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.HeaderText = "Report Enabled";
            chk.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            chk.Name = "ReprotEnabled";
            chk.Width = 145;
            chk.DataPropertyName = "ReprotEnabled";
            dgReportList.Columns.Add(chk);

            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1 = new DataGridViewTextBoxColumn();
            column1.Name = "DisplayName";
            column1.HeaderText = "Report Name";
            column1.DataPropertyName = "DisplayName";
            column1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            column1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            column1.Width = 500;
            column1.ReadOnly = true;
            dgReportList.Columns.Add(column1);




             column1 = new DataGridViewTextBoxColumn();
            column1 = new DataGridViewTextBoxColumn();
            column1.Name = "FileName";
            column1.HeaderText = "Report Name";
            column1.DataPropertyName = "FileName";
            column1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            column1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            column1.Width = 500;
            column1.ReadOnly = true;
            dgReportList.Columns.Add(column1);
            //dgReportList.ClearSelection(); 

            dgReportList.DataSource = ReportListDataBind2;
           // dgReportList.Sort(this.dgReportList.Columns["FileName"], ListSortDirection.Ascending);
        }

        private void frmEditReport_Load(object sender, EventArgs e)
        {
            if (MyParent.userReportMenu != null)
                MyParent.userReportMenu.Visible = false;
        }

        public Dictionary<int, ReportInfo> ReportsDictionary
        {
            get;
            set;
        }

        public BindingList<ReportInfo> ReportListDataBind
        {
            get;
            set;
        }

        //Just for testing purposes lets create a public method just to load the data
        public void LoadDataIntoTheDataGridView()
        {
            //dgReportList.DataSource = null;
            //dgReportList.Rows.Clear();
            //dgReportList.AutoGenerateColumns = true;
            //dgReportList.AllowUserToAddRows = false;
            //dgReportList.DataSource = ReportListDataBind;;
            //dgReportList.ClearSelection();
        }


    }
}
