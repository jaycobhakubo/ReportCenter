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
using GTI.Modules.ReportCenter.Business;
using GTI.Modules.Shared;

namespace GTI.Modules.ReportCenter
{
    public class FindStaffOrPlayerMessage : ServerMessage 
    {
        private byte mbyteSearchType = 0; //staff
        private int mintLoginNumber;
        private string mstrFirstName;
        private string mstrLastName;
        private string mMagCard;
        private Dictionary<int, string> mFoundPersons; //her ID, and her name

        public FindStaffOrPlayerMessage(bool searchPlayer, int loginNumber, string magCard,string firstName, string lastName)
        {
            m_id = 18104;

            if (searchPlayer == true)
            {
                mbyteSearchType = 1;
            }
            else
            {
                mbyteSearchType = 0;
            }
            mintLoginNumber = loginNumber;
            mstrLastName = lastName;
            mstrFirstName = firstName;
            mMagCard = magCard;

            mFoundPersons = new Dictionary<int, string>();
        }

        #region Member Methods
        /// <summary>
        /// Prepares the request to be sent to the server.
        /// </summary>
        protected override void PackRequest()
        {
            // Create the streams we will be writing to.
            MemoryStream requestStream = new MemoryStream();
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

            // FIX :RALLY DE3803
            // operatorID
            requestWriter.Write(Configuration.operatorID);
            // END :RALLY  DE3803
            // Report type
            requestWriter.Write(mbyteSearchType);
            requestWriter.Write(mintLoginNumber);
            
            if (string.IsNullOrEmpty(mstrFirstName) ==false  && mstrFirstName.Length > 0)
            {
                requestWriter.Write((ushort) mstrFirstName.Length);
                requestWriter.Write(mstrFirstName.ToCharArray());
            }
            else
            {
                requestWriter.Write((ushort)0);
            }

            if (string.IsNullOrEmpty(mstrLastName) == false && mstrLastName.Length > 0)
            {
                requestWriter.Write((ushort)mstrLastName.Length);
                requestWriter.Write(mstrLastName.ToCharArray());
            }
            else
            {
                requestWriter.Write((ushort)0);
            }

            if (string.IsNullOrEmpty (mMagCard) ==false && mMagCard.Length > 0)
            {
                if (int.Parse(mMagCard) == 0)
                {
                    requestWriter.Write((ushort)0);
                }
                else
                {
                    requestWriter.Write((ushort)mMagCard.Length);
                    requestWriter.Write(mMagCard.ToCharArray());
                }
            }
            else
            {
                requestWriter.Write((ushort) 0);
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

            // Check the response length.
            if (responseStream.Length < 6)
                throw new MessageWrongSizeException("Find Staff Or Player Message");

            // Try to unpack the data.

            // Seek past return code.
            responseReader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);

            // Get the count of players.
            ushort Count = responseReader.ReadUInt16();

            // Get all 
            UInt16 tempLen = 0;
            int tempID;
            string first, last;
            for (ushort x = 0; x < Count; x++)
            {
                // Report Id
                tempID = responseReader.ReadInt32();
                //The message is defined as returning first name then
                // last name, but the backing sp actually returns last then first
                // the sp was not modified so that other code does not break.
                tempLen = responseReader.ReadUInt16();
                last = new string(responseReader.ReadChars(tempLen));
                tempLen = responseReader.ReadUInt16();
                first = new string(responseReader.ReadChars(tempLen));
                mFoundPersons.Add(tempID, first + " " + last); //RALLY US 427 / DE 6001               
            }

            // Close the streams.
            responseReader.Close();
        }

        #endregion

        public Dictionary<int, string> FoundPersons
        { get { return mFoundPersons; } }

    }
}
