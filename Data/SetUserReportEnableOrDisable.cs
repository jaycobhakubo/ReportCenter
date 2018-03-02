
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTI.Modules.Shared;
using System.IO;

namespace GTI.Modules.ReportCenter.Data
{

    class SetUserReportEnableOrDisable : ServerMessage
    {
        private List<ReportInfo> mListOfListReportsEnableDisable;

        public SetUserReportEnableOrDisable(List<ReportInfo> ListOfListReportsEnableDisable)
        {
            m_id = 18252;
            mListOfListReportsEnableDisable = ListOfListReportsEnableDisable;
        }

        protected override void PackRequest()
        {
            MemoryStream requestStream = new MemoryStream();            // Create the streams we will be writing to.
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

            requestWriter.Write((ushort)mListOfListReportsEnableDisable.Count());

            foreach (ReportInfo rptInfo in mListOfListReportsEnableDisable)  //settings count
            {
                requestWriter.Write((int)rptInfo.ID);
                requestWriter.Write(1);
                requestWriter.Write((ushort)rptInfo.DisplayName.Length);
                requestWriter.Write(rptInfo.DisplayName.ToCharArray());
            }

            m_requestPayload = requestStream.ToArray();
            requestWriter.Close();            // Close the streams.bn
        }

        //protected override void UnpackResponse()
        //{
        //    base.UnpackResponse();

        //}
    }
}


