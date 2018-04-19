using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GTI.Modules.Shared;

namespace GTI.Modules.ReportCenter.Data
{
    class SetImportFile : ServerMessage
    {
        FileStream m_fsRptFile;
        StreamReader m_fsSqlFile;
        string m_rptSqlData;
        string m_reportName;
        //FileStream m_rptFileData;
        string m_rptDataPath;
        Stream m_rptFileData;

        public SetImportFile(string rptSqlData, string rptReportName, Stream rptFileData)
        {
            m_id = 18258;


            m_rptSqlData = rptSqlData;
            m_reportName = rptReportName;
            //m_rptDataPath = rptDataPath;
            m_rptFileData = rptFileData;
        }

        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        protected override void PackRequest()
        {
            MemoryStream requestStream = new MemoryStream();            // Create the streams we will be writing to.
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);


            requestWriter.Write((ushort)m_rptSqlData.Length);
            requestWriter.Write(m_rptSqlData.ToCharArray());

            requestWriter.Write((ushort)m_reportName.Length);
            requestWriter.Write(m_reportName.ToCharArray());
            
     //       using (FileStream fs = File.OpenRead(m_rptDataPath))
     //       {
                
     //           byte[] b = new byte[fs.Length];
     //        //   UTF8Encoding temp = new UTF8Encoding(true);
     //           fs.Read(b, 0, b.Length);

     ////           requestWriter.Write(b.Length);
     //           requestWriter.Write(b);

     //       }

            //requestWriter.Write((ushort)m_rptFileData.Length);

            requestWriter.Write(ReadFully(m_rptFileData));
            
            //byte[] fileBytes = new byte[m_rptFileData.Length];
            //requestWriter.Write(fileBytes);


            m_requestPayload = requestStream.ToArray();
            requestWriter.Close();            // Close the streams.bn

        }

        protected byte[] mbyteResponsePayload = null;
        protected byte[] mbytResponse = null;

        private int mintReturnCode;
        protected override void UnpackResponse()
        {


            mbytResponse = base.m_responsePayload;

            // Check to see if we got the payload correctly.
            if (base.m_requestPayload == null)
                throw new ServerCommException("SetMotifMessage.UnpackResponse()..Server communication lost.");

            if (mbytResponse.Length < sizeof(int))
                throw new MessageWrongSizeException("SetMotifMessage.UnpackResponse()..Message payload size is too small.");

            // Check the return code.
            mintReturnCode = BitConverter.ToInt32(mbytResponse, 0);

            if (mintReturnCode != (int)GTIServerReturnCode.Success)
                throw new ServerException("SetMotifMessage.UnpackResponse()..Server Error Code: " + mintReturnCode.ToString());

            MemoryStream responseStream = new MemoryStream(m_responsePayload);
            BinaryReader responseReader = new BinaryReader(responseStream, Encoding.Unicode);

            // Try to unpack the data.
            int returnCode = responseReader.ReadInt32();

            if (returnCode != (int)GTIServerReturnCode.Success)
            {
                throw new ServerException("Server Error Code: " + returnCode.ToString());
            }

            responseReader.Close();

        }



    }
}
