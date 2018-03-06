﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using GTI.Modules.ReportCenter.Properties;
using GTI.Modules.Shared;
using GTI.Modules.ReportCenter.UI;


namespace GTI.Modules.ReportCenter.Data
{
    public class GetUserDefineReports : ServerMessage
    {
        public  List<ReportData> mListRptData {get; set;}

        public GetUserDefineReports()
        {
            m_id = 18253;
            mListRptData = new List<ReportData>();
        }


        protected override void PackRequest()
        {
            MemoryStream requestStream = new MemoryStream();
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);
            m_requestPayload = requestStream.ToArray();
            requestWriter.Close();
        }


        protected override void UnpackResponse()
        {
            base.UnpackResponse();
            MemoryStream responseStream = new MemoryStream(m_responsePayload);
            BinaryReader responseReader = new BinaryReader(responseStream, Encoding.Unicode);
            responseReader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);

        
            ushort Count = responseReader.ReadUInt16();
            string tempName = string.Empty;
            ushort tempLength = 0;

            for (int iType = 0; iType < Count; iType++)
            {
                var tempValue = new ReportData();
                tempValue.ReportId = responseReader.ReadInt32();
                byte tIsActive = 0;
                tIsActive = responseReader.ReadByte();
                tempValue.IsActive = tIsActive == 0 ? false : true;
                tempLength = responseReader.ReadUInt16();
                tempValue.ReportDisplayName = new string(responseReader.ReadChars(tempLength));
                tempLength = responseReader.ReadUInt16();
                tempValue.ReportFileName = new string(responseReader.ReadChars(tempLength));
                mListRptData.Add(tempValue);    
            }

            // Close the streams.
            responseReader.Close();
        }

    }
}