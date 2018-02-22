#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using GTI.Modules.ReportCenter.Properties;
using GTI.Modules.Shared;
using GTI.Modules.ReportCenter.Business;
using GTI.Modules.ReportCenter.Data;

namespace GTI.Modules.ReportCenter
{
    internal class GetPositionList : ServerMessage
    {
        private int m_OperatorId;
        private DateTime m_end;

        internal GetPositionList(int opId)
        {
            m_id = 18192;
            m_OperatorId = opId;
        }

        #region Member Methods
        internal static List<PositionType> GetList(int opId)
        {
            GetPositionList msg = new GetPositionList(opId);
            try
            {
                msg.Send();
            }
            catch (Exception ex)
            {
                Utilities.Log(Properties.Resources.errorFailedToGetData + " " + ex.Message, LoggerLevel.Severe);
                MessageForm.Show(Properties.Resources.errorFailedToGetData, Properties.Resources.report_center);
                return null;
            }
            return msg.Positions;
        }
        /// <summary>
        /// Prepares the request to be sent to the server.
        /// </summary>
        protected override void PackRequest()
        {
            // Create the streams we will be writing to.
            MemoryStream requestStream = new MemoryStream();
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);
            requestWriter.Write(Configuration.operatorID);

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

            // Get the count of positions.
            ushort count = responseReader.ReadUInt16();

            Positions = new List<PositionType>();

            // Get all 
            UInt16 len = 0;
            int id = 0;
            string name = string.Empty;
            for (ushort x = 0; x < count; x++)
            {
                // Report Id
                id = responseReader.ReadInt32();
                len = responseReader.ReadUInt16();
                name = new string(responseReader.ReadChars(len));
                Positions.Add(new PositionType(id, name)); 
            }

            // Close the streams.
            responseReader.Close();
        }
        #endregion

        internal List<PositionType> Positions { get; private set; }
    }
}
