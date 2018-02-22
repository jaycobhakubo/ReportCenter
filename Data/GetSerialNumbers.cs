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
    internal class GetSerialNumbers : ServerMessage
    {
        private int m_OperatorId;

        internal GetSerialNumbers(int opId)
        {
            m_id = 36034;
            m_OperatorId = opId;
        }

        #region Member Methods
        internal static List<SerialNumbersType> GetList(int opId)
        {
            GetSerialNumbers msg = new GetSerialNumbers(opId);
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
            return msg.SerialNumbers;
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
            SerialNumbers = new List<SerialNumbersType>();

            // Get all 
            UInt16 len = 0;
            string nbr = string.Empty;
            for (ushort x = 0; x < count; x++)
            {
                len = responseReader.ReadUInt16();
                nbr = new string(responseReader.ReadChars(len));
                SerialNumbers.Add(new SerialNumbersType(nbr)); 
            }

            // Close the streams.
            responseReader.Close();
        }
        #endregion

        internal List<SerialNumbersType> SerialNumbers { get; private set; }
    }
}
