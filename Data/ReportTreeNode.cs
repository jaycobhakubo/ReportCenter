#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System.Windows.Forms;
using GTI.Modules.Shared;

namespace GTI.Modules.ReportCenter.Data
{
    internal class ReportTreeNode : TreeNode
    {
        public ReportTreeNode(ReportInfo report, bool isNew)
            : base(report.DisplayName)
        {
            IsNewNode = isNew;
            ReportInfo = report;
        }

        public bool IsNewNode { get; set; }

        public ReportInfo ReportInfo { get; set; }
    }

    internal class ReportSetTreeNode : TreeNode
    {
        public ReportTypes ReportType { get; set; }
        public ReportSetTreeNode(string displayName, ReportTypes reportType)
            : base(displayName)
        {
            ReportType = reportType;
        }
    }


}