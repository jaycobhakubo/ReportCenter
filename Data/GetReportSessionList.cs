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

// FIX : TA4979 (mostly cleanup)
// FIX: TA7438 - Remove operator id from Get Session Number List.
namespace GTI.Modules.ReportCenter
{
    internal class GetReportSessionList : ServerMessage 
    {
        private DateTime m_start;
        private DateTime m_end;

        internal GetReportSessionList(DateTime start, DateTime end)
        {
            m_id = 18107;
            m_start = start;
            m_end = end;
        }

        #region Member Methods
        internal static List<int> GetList(DateTime start, DateTime end)
        {
            GetReportSessionList msg = new GetReportSessionList(start, end);
            try
            {
                msg.Send();
            }
            catch (Exception ex)
            {
                MessageForm.Show(Resources.errorLoadData + ex.Message, Resources.report_center);
            }
            return msg.SessionNumbers;
        }
        /// <summary>
        /// Prepares the request to be sent to the server.
        /// </summary>
        protected override void PackRequest()
        {
            // Create the streams we will be writing to.
            MemoryStream requestStream = new MemoryStream();
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

            if (m_start.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Length > 0)
            {
                requestWriter.Write((ushort)m_start.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Length);
                requestWriter.Write(m_start.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).ToCharArray());
            }
            else
            {
                requestWriter.Write((ushort)0);
            }

            if (m_end.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Length > 0)
            {
                requestWriter.Write((ushort)m_end.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Length);
                requestWriter.Write(m_end.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).ToCharArray());
            }
            else
            {
                requestWriter.Write((ushort)0);
            }

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

            // Get the count of players.
            ushort count = responseReader.ReadUInt16();

            SessionNumbers = new List<int>();
            // Get all 
            for (ushort iSession = 0; iSession < count; iSession++)
            {
                SessionNumbers.Add(responseReader.ReadInt32());
            }

            // Close the streams.
            responseReader.Close();
        }
        #endregion

        internal List<int> SessionNumbers { get; private set; }
    }
}
// END: TA7438 
// END : TA4979 (mostly cleanup)
