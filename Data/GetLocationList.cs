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
    internal class GetLocationList : ServerMessage
    {
        private int m_LocationTypeId = 1;   // Default to Physical Locations

        internal GetLocationList(int id)
        {
            m_id = 36004;
            m_LocationTypeId = id;
        }

        #region Member Methods
        internal static List<LocationType> GetList(int id)
        {
            GetLocationList msg = new GetLocationList(id);
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
            return msg.Locations;
        }
        /// <summary>
        /// Prepares the request to be sent to the server.
        /// </summary>
        protected override void PackRequest()
        {
            // Create the streams we will be writing to.
            MemoryStream requestStream = new MemoryStream();
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);
            // DE8678 - Message should gets it by location type not operator.
            requestWriter.Write(m_LocationTypeId);

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
            Locations = new List<LocationType>();
            UInt16 len = 0, typeCount = 0;
            byte isActive, isDefault;
            int id = 0, typeId = 0, locId = 0, staffId = 0;
            Byte active = 1;
            string name = string.Empty;

            try
            {
                // Seek past return code.
                responseReader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);
                ushort count = responseReader.ReadUInt16();

                // Resultset is ordered by invLocationId asc, but msg contains groups of invLocationTypeId. 2 loops needed!
                for (ushort x = 0; x < count; x++)
                {
                    typeId = responseReader.ReadInt32();
                    typeCount = responseReader.ReadUInt16();
                    for (ushort y = 0; y < typeCount; y++)
                    {
                        locId = responseReader.ReadInt32();
                        len = responseReader.ReadUInt16();
                        name = new string(responseReader.ReadChars(len));
                        staffId = responseReader.ReadInt32();
                        isActive = responseReader.ReadByte();
                        isDefault = responseReader.ReadByte();

                        if (isActive.CompareTo(active) == 0)
                        {
                            Locations.Add(new LocationType(locId, name));
                        }
                    }
                }

                // Close the streams.
                responseReader.Close();

            }
            catch (Exception ex)
            {
                Utilities.Log("GetLocationList FAILED: " + ex.Message, LoggerLevel.Severe);
                //throw;
            }
        }
        #endregion

        internal List<LocationType> Locations { get; private set; }
    }
}
