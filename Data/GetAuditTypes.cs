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
    internal class GetAuditTypes : ServerMessage
    {
       
        internal GetAuditTypes()
        {
            m_id = 18042;
        }

        #region Member Methods
        internal static List<AuditType> GetList()
        {
            GetAuditTypes msg = new GetAuditTypes();
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
            return msg.AuditTypes;
        }
        /// <summary>
        /// Prepares the request to be sent to the server.
        /// </summary>
        protected override void PackRequest()
        {
            // Create the streams we will be writing to.
            MemoryStream requestStream = new MemoryStream();
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);
            
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
            AuditTypes = new List<AuditType>();
            
            try
            {
                // Seek past return code.
                responseReader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);
                ushort count = responseReader.ReadUInt16();

                // Resultset is ordered by invLocationId asc, but msg contains groups of invLocationTypeId. 2 loops needed!
                for (ushort x = 0; x < count; x++)
                {
                    AuditType auditType = new AuditType();
                    auditType.Type = responseReader.ReadInt32();
                    ushort length = responseReader.ReadUInt16();
                    if(length > 0)
                    {
                        auditType.Description = new string(responseReader.ReadChars(length));
                    }
                    AuditTypes.Add(auditType);
                }

                // Close the streams.
                responseReader.Close();

            }
            catch (Exception ex)
            {
                Utilities.Log("GetAuditTypes FAILED: " + ex.Message, LoggerLevel.Severe);
                //throw;
            }
        }
        #endregion

        internal List<AuditType> AuditTypes { get; private set; }
    }
}
