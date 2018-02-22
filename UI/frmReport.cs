#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using GTI.Modules.Shared;

// FIX : DE3307 Memory Leak and other cleanup
namespace GTI.Modules.ReportCenter.UI
{
    public delegate void ReportErrorEventHandler(object sender, ReportEventArgs e);

    public partial class frmReport : GradientForm
    {
        private ReportDocument Doc { get; set; }

        public frmReport(ReportDocument rptDocument)
        {
            InitializeComponent();
            Doc = rptDocument;
            if (rptDocument != null)
            {
                reportViewer.ReportSource = rptDocument;
                Application.DoEvents();                
            }
        }
        
        private void closeImageButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reportViewer.PrintReport();    
        }

        private void frmReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeImageButton.Dispose();
            reportViewer.ReportSource = null;
            Doc.Dispose();
            Doc = null;
            reportViewer.ClearSelectedItems();
            reportViewer.ParameterFieldInfo = null;
        }

        private void drillDownDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(reportViewer.ToolPanelView.Equals(ToolPanelViewType.GroupTree))
                reportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            else
                reportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree;
        }
    }

    public class ReportEventArgs : EventArgs
    {
        public ReportEventArgs(string reportName, string message, string reportID)
        {
            ReportName = reportName;
            ErrorMsg = message;
            ReportID = reportID;
        }

        public string ReportName { get; private set; }
        public string ErrorMsg { get; private set; }
        public string ReportID { get; private set; }
    }
}
// END : DE3307 Memory Leak and other cleanup
