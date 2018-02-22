#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion
// FIX : DE3252 Customized reports not working (entire source file updated)
using System.Windows.Forms;
using GTI.Modules.ReportCenter.Data;

namespace GTI.Modules.ReportCenter
{
    class UserReportTypeTreeNode : TreeNode 
    {

        internal bool IsNewNode { get; set; }
        internal UserReportType ReportType { get; private set; }

        public UserReportTypeTreeNode(string text, bool isNew)
            : base(text)
        {
            IsNewNode = isNew;
            ReportType = new UserReportType(0, text);            
        }

        public UserReportTypeTreeNode(UserReportType type, bool isNew)
            :base(type.UserReportTypeName)
        {
            IsNewNode = isNew;
            ReportType = type;            
        }        
    }
}
// END : DE3252 Customized reports not working (entire source file updated)
