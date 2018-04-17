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
        FileStream m_fs;

        public SetImportFile(FileStream fs)
        {
            m_fs = fs;
        }

        protected override void PackRequest()
        {
            MemoryStream requestStream = new MemoryStream();            // Create the streams we will be writing to.
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

            byte[] fileBytes = new byte[m_fs.Length];
            requestWriter.Write(fileBytes);

            m_requestPayload = requestStream.ToArray();
            requestWriter.Close();            // Close the streams.bn

        }



    }
}
