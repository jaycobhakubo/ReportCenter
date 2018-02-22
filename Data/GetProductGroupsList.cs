
#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

using GTI.Modules.ReportCenter.Properties;
using GTI.Modules.Shared;
using GTI.Modules.ReportCenter.Business;

namespace GTI.Modules.ReportCenter
{
    internal class GetProductGroupsList : ServerMessage
    {
        private int m_OperatorId;

        internal GetProductGroupsList(int opId)
        {
            // US1902: Allow certain reports (Door Sales) to filter on product group.
            m_id = 18166;
            m_OperatorId = opId;
        }

        #region Member Methods
        internal static List<ProductGroupData> GetList(int opId)
        {
            GetProductGroupsList msg = new GetProductGroupsList(opId);
            
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

            return msg.ProductGroupsList;
        }

        /// <summary>
        /// Prepares the request to be sent to the server.
        /// </summary>
        protected override void PackRequest()
        {
            // Create the streams we will be writing to.
            MemoryStream requestStream = new MemoryStream();
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

            requestWriter.Write(0);                 // 0=returns all active product items by type            
            requestWriter.Write(0);                 // 0=skip retired items

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
            try
            {
                base.UnpackResponse();

                // Create the streams we will be reading from.
                MemoryStream responseStream = new MemoryStream(m_responsePayload);
                BinaryReader responseReader = new BinaryReader(responseStream, Encoding.Unicode);

                // Seek past return code.
                responseReader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);
                ushort count = responseReader.ReadUInt16();
                //List<ProductGroupData> items = new List<ProductGroupData>();
                ProductGroupsList = new List<ProductGroupData>();
                UInt16 len = 0;
                int id = 0;
                byte activeField = 0;
                bool isActive = false;
                string name = string.Empty;

                for (ushort x = 0; x < count; x++)
                {
                    id = responseReader.ReadInt32();
                    len = responseReader.ReadUInt16();
                    name = new string(responseReader.ReadChars(len));
                    activeField = responseReader.ReadByte();
                    isActive = activeField == 0 ? false : true;
                    ProductGroupsList.Add(new ProductGroupData(isActive, id, name));
                }

                // Close the streams.
                responseReader.Close();
            }
            catch (Exception ex)
            {
                // todo
                throw new Exception("GetProductGroupsList FAILED: " + ex.Message);
            }
        }
        #endregion

        internal List<ProductGroupData> ProductGroupsList { get; private set; }

        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }


    }
}
