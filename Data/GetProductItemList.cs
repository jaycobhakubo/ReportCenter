
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
    internal class GetProductItemList : ServerMessage
    {
        internal GetProductItemList()
        {
            m_id = 18197;
        }

        #region Member Methods
        internal static List<ProductItemData> GetList()
        {
            GetProductItemList msg = new GetProductItemList();
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

            return msg.ProductItemList;
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
            try
            {
                base.UnpackResponse();

                // Create the streams we will be reading from.
                MemoryStream responseStream = new MemoryStream(m_responsePayload);
                BinaryReader responseReader = new BinaryReader(responseStream, Encoding.Unicode);
                
                // Seek past return code.
                responseReader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);
                ushort count = responseReader.ReadUInt16();
                List<ProductItemData> items = new List<ProductItemData>();
                ProductItemList = new List<ProductItemData>();
                UInt16 len = 0;
                int id = 0;
                string name = string.Empty, serialNo = string.Empty, itemName = string.Empty;

                for (ushort x = 0; x < count; x++)
                {
                    id = responseReader.ReadInt32();
                    len = responseReader.ReadUInt16();
                    name = new string(responseReader.ReadChars(len));
                    ProductItemList.Add(new ProductItemData(id, name));
                }

                // Close the streams.
                responseReader.Close();
            }
            catch (Exception ex)
            {
                // todo
                throw new Exception("GetProductItemList FAILED: " + ex.Message);
            }
        }
        #endregion

        internal List<ProductItemData> ProductItemList { get; private set; }

        private int myVar;

	public int MyProperty
	{
		get { return myVar;}
		set { myVar = value;}
	}
	

    }
}
