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
    }
}
