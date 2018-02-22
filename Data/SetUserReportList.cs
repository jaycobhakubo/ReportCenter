#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion
// FIX : DE3252 Customized reports not working (entire source file updated)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using GTI.Modules.ReportCenter.Data;
using GTI.Modules.Shared;

namespace GTI.Modules.ReportCenter
{
   internal class SetUserReportList : ServerMessage 
    {
       internal int OperatorID { get; set; }
       internal int StaffID { get; set; }
       internal string Locale { get; set; }
       internal UserReportType ReportType { get; set; }

       internal SetUserReportList() 
       {
           m_id = 18110;
       }

       internal SetUserReportList(int operatorID, int staffID, string locale,
                                   UserReportType type)
       {
           m_id = 18110;
           OperatorID = operatorID;
           StaffID = staffID;
           Locale = locale;
           ReportType = type;
       }
       
       protected override void PackRequest()
       {
           try
           {
               Debug.WriteLine("SetUserReportList : " + ReportType.UserReportTypeName);
               // Create the streams we will be writing to.
               MemoryStream requestStream = new MemoryStream();
               BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

               // Operator Id
               requestWriter.Write(OperatorID);
               requestWriter.Write(StaffID);
               requestWriter.Write(0);//we do not use locale id
               requestWriter.Write((ushort)Locale.Length);
               requestWriter.Write(Locale.ToCharArray());
               requestWriter.Write(ReportType.UserReportTypeID);
               requestWriter.Write((ushort)ReportType.UserReportTypeName.Length);
               requestWriter.Write(ReportType.UserReportTypeName.ToCharArray());
               requestWriter.Write(ReportType.RemoveType);
               ushort groupCount = (ushort)(ReportType.UserReportGroups.Count + ReportType.NewReportLists.Count);
               requestWriter.Write(groupCount);
               ReportInfo tempReport;
               foreach (KeyValuePair<int, UserReportGroup> iGroup in ReportType.UserReportGroups)
               {
                   requestWriter.Write(iGroup.Value.UserGroupID);
                   requestWriter.Write((ushort)iGroup.Value.UserGroupName.Length);
                   requestWriter.Write(iGroup.Value.UserGroupName.ToCharArray());
                   if (ReportType.RemoveType == 1)
                   {
                       requestWriter.Write((byte)1);
                   }
                   else
                   {
                       requestWriter.Write(iGroup.Value.RemoveType);
                   }

                   requestWriter.Write((ushort)iGroup.Value.ReportsArray.Count);
                   for (int iReport = 0; iReport < iGroup.Value.ReportsArray.Count; iReport++)
                   {
                       tempReport = (ReportInfo)iGroup.Value.ReportsArray[iReport];
                       requestWriter.Write(tempReport.ID);
                       requestWriter.Write((ushort)tempReport.DisplayName.Length);
                       requestWriter.Write(tempReport.DisplayName.ToCharArray());

                       if (ReportType.RemoveType == 1 || iGroup.Value.RemoveType == 1)
                       {
                           requestWriter.Write((byte)1);
                       }
                       else
                       {
                           requestWriter.Write(tempReport.RemoveType);
                       }
                   }
               }
               //new groups
               foreach (UserReportGroup iGroup in ReportType.NewReportLists)
               {
                   requestWriter.Write(iGroup.UserGroupID);
                   requestWriter.Write((ushort)iGroup.UserGroupName.Length);
                   requestWriter.Write(iGroup.UserGroupName.ToCharArray());
                   if (ReportType.RemoveType == 1)
                   {
                       requestWriter.Write((byte)1);
                   }
                   else
                   {
                       requestWriter.Write(iGroup.RemoveType);
                   }

                   requestWriter.Write((ushort)iGroup.ReportsArray.Count);
                   for (int iReport = 0; iReport < iGroup.ReportsArray.Count; iReport++)
                   {
                       tempReport = (ReportInfo)iGroup.ReportsArray[iReport];
                       requestWriter.Write(tempReport.ID);
                       requestWriter.Write((ushort)tempReport.DisplayName.Length);
                       requestWriter.Write(tempReport.DisplayName.ToCharArray());

                       if (ReportType.RemoveType == 1 || iGroup.RemoveType == 1)
                       {
                           requestWriter.Write((byte)1);
                       }
                       else
                       {
                           requestWriter.Write(tempReport.RemoveType);
                       }
                   }

               }
               // Set the bytes to be sent.
               m_requestPayload = requestStream.ToArray();

               // Close the streams.
               requestWriter.Close();
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message );
           }
       }

    }
}
// END : DE3252 Customized reports not working (entire source file updated)
