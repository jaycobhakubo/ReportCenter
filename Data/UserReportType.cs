#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System.Collections;
using System.Collections.Generic;

namespace GTI.Modules.ReportCenter.Data
{
    internal class UserReportType
    {
        internal UserReportType(int typeID, string typeName) : this(typeID, typeName, 0) { }
        internal UserReportType(int typeID, string typeName, byte removeType)
        {
            UserReportTypeID = typeID;
            UserReportTypeName = typeName;
            RemoveType = removeType;
            UserReportGroups = new Dictionary<int, UserReportGroup>();
            NewReportLists = new ArrayList();
        }

        internal ArrayList NewReportLists { get; set; }
        internal int UserReportTypeID { get; set; }
        internal string UserReportTypeName { get; set; }
        internal Dictionary<int, UserReportGroup> UserReportGroups { get; private set; }
        internal byte RemoveType { get; set; }
    }

    internal class UserReportGroup
    {
        internal UserReportGroup(int groupID, string groupName) : this(groupID, groupName, 0) { }
        internal UserReportGroup(int groupID, string groupName, byte removeType)
        {
            UserGroupID = groupID;
            UserGroupName = groupName;
            RemoveType = removeType;
            ReportsArray = new ArrayList();
        }

        internal ArrayList ReportsArray { get; set; }
        internal int UserGroupID { get; set; }
        internal string UserGroupName { get; set; }
        internal byte RemoveType { get; set; }
    }
}