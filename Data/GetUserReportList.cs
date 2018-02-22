#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GTI.Modules.ReportCenter.Data;
using GTI.Modules.Shared;

namespace GTI.Modules.ReportCenter
{
   internal  class GetUserReportList : ServerMessage 
    {
       private int mOperatorID;
       private int mStaffID;
       private string mLocale;
       private Dictionary<int, UserReportType> mReportTypes; //reportypeID, usrreporttype
       internal GetUserReportList(int operatorID, int staffID, string locale)
       {
           m_id = 18109;
           mOperatorID = operatorID;
           mStaffID = staffID;
           mLocale = locale;
           mReportTypes = new Dictionary<int, UserReportType>();
       }

       #region Member Methods
       /// <summary>
       /// Prepares the request to be sent to the server.
       /// </summary>
       protected override void PackRequest()
       {
           // Create the streams we will be writing to.
           MemoryStream requestStream = new MemoryStream();
           BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

           requestWriter.Write(mOperatorID);
           requestWriter.Write(mStaffID);
           requestWriter.Write((int)0); //locale id we do not use it
           requestWriter.Write((ushort)mLocale.Length);
           requestWriter.Write (mLocale.ToCharArray());
           // Set the bytes to be sent.
           m_requestPayload = requestStream.ToArray();

           // Close the streams.
           requestWriter.Close();
       }

       /// <summary>
       /// Parses the response received from the server.
       /// </summary>
       protected override void UnpackResponse()
       {
           base.UnpackResponse();

           // Create the streams we will be reading from.
           MemoryStream responseStream = new MemoryStream(m_responsePayload);
           BinaryReader responseReader = new BinaryReader(responseStream, Encoding.Unicode);

           // Seek past return code.
           responseReader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);
         
           ushort groupCount = 0, reportCount = 0, tempLength=0, parameterCount=0;
           int tempID = 0, tempTypeID=0;
           string tempName = string.Empty;
           UserReportType tempType;
           UserReportGroup tempGroup;
           ReportInfo tempReport;
           ushort typeCount = responseReader.ReadUInt16();

           for (int iType = 0; iType < typeCount; iType++)
           {
               tempID = responseReader.ReadInt32();
               tempLength = responseReader.ReadUInt16();
               tempName = new string(responseReader.ReadChars (tempLength));
               tempType = new UserReportType(tempID, tempName);
               groupCount = responseReader.ReadUInt16();
               for (int iGroup = 0; iGroup < groupCount; iGroup++)
               {
                   tempID = responseReader.ReadInt32();
                   tempLength = responseReader.ReadUInt16();
                   tempName = new string(responseReader.ReadChars(tempLength));          
                   tempGroup = new UserReportGroup(tempID, tempName);
                   reportCount = responseReader.ReadUInt16();                   
                   for (int iReport = 0; iReport < reportCount; iReport++)
                   {
                       tempID = responseReader.ReadInt32();
                       tempTypeID = responseReader.ReadInt32();
                       tempLength = responseReader.ReadUInt16();
                       tempName = new string(responseReader.ReadChars(tempLength));
                       tempReport = new ReportInfo(tempID,tempTypeID,tempName);
                       parameterCount = responseReader.ReadUInt16();
                       for (int iParameter = 0; iParameter < parameterCount; iParameter++)
                       {
                           tempID = responseReader.ReadInt32();
                           tempLength = responseReader.ReadUInt16();
                           tempName = new string(responseReader.ReadChars(tempLength));          
                   
                           tempReport.Parameters.Add(tempID, tempName);
                       }
                       tempGroup.ReportsArray.Add(tempReport);
                   }
                   tempType.UserReportGroups.Add(tempGroup.UserGroupID, tempGroup);
               }

               mReportTypes.Add(tempType.UserReportTypeID, tempType);
           }          

           // Close the streams.
           responseReader.Close();
       }
       #endregion

       internal Dictionary<int, UserReportType> ReportTypes
       { get { return mReportTypes; } }
   }
}
