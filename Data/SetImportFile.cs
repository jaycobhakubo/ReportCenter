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
        private FileStream m_fsRptFile;
        private StreamReader m_fsSqlFile;
        private string m_rptSqlData;
        private string m_reportName;
        private string m_rptDataPath;
        private byte[] m_rptFileData;
        protected byte[] mbyteResponsePayload;
        protected byte[] mbytResponse;
        private int mintReturnCode;

        public SetImportFile(string rptSqlData, string rptReportName, byte[] rptFileData)
        {
            m_id = 18258;
            m_rptSqlData = rptSqlData;
            m_reportName = rptReportName;
            m_rptFileData = rptFileData;
        }

       

        protected override void PackRequest()
        {
            MemoryStream requestStream = new MemoryStream();            // Create the streams we will be writing to.
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

            requestWriter.Write((ushort)m_rptSqlData.Length);
            requestWriter.Write(m_rptSqlData.ToCharArray());
            requestWriter.Write((ushort)m_reportName.Length);
            requestWriter.Write(m_reportName.ToCharArray());            
            requestWriter.Write(m_rptFileData);
            
            m_requestPayload = requestStream.ToArray();
            requestWriter.Close();            // Close the streams.bn

        }



        protected override void UnpackResponse()
        {
            /*
            try
            {
                base.UnpackResponse();
            }
            catch
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
                    throw new ServerException("SetMotifMessage.UnpackResponse()..Server Error Code: " + mintReturnCode.ToString());*/

            base.UnpackResponse();
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
