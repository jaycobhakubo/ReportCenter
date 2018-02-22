#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System.Windows.Forms;

namespace GTI.Modules.ReportCenter.Data
{
    class UserReportGroupTreeNode : TreeNode
    {
        internal bool IsNewNode { get; private set; }
        internal UserReportGroup ReportGroup { get; private set; }

        public UserReportGroupTreeNode(string text, bool isNew)
            : base(text)
        {
            IsNewNode = isNew;
            ReportGroup = new UserReportGroup(0, text);
        }

        public UserReportGroupTreeNode(UserReportGroup group, bool isNew)
            :base(group.UserGroupName)
        {
            IsNewNode = isNew;
            ReportGroup = group;
        }        
    }
}