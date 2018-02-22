using System;
using System.IO;
using System.Text;
using GTI.Modules.Shared;

namespace GTI.Modules.ReportCenter.Data
{
    public class GetMontlyBingoSalesData : ServerMessage 
    {
        public int OperatorId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal SalesData { get; private set; }

        GetMontlyBingoSalesData(int operatorId, int month, int year)
        {
            m_id = 18171;
            OperatorId = operatorId;
            Month = month;
            Year = year;
        }

        public static decimal MonthlyBingoSales(int operatorId, int month, int year)
        {
            var msg = new GetMontlyBingoSalesData(operatorId, month, year);
            try
            {
                msg.Send();
            }
            catch (ServerCommException ex)
            {
                throw new Exception("GetMontlyBingoSalesData: " + ex.Message);
            }
            return msg.SalesData;
        }

        protected override void PackRequest()
        {
            MemoryStream requestStream = new MemoryStream();
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

            requestWriter.Write(OperatorId);
            requestWriter.Write(Month);
            requestWriter.Write(Year);

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

            ushort strlen = responseReader.ReadUInt16();
            string salesData = new string(responseReader.ReadChars(strlen));
            decimal decValue;
            decimal.TryParse(salesData, out decValue);
            SalesData = decValue;

            // Close the streams.
            responseReader.Close();
        }
    }
}
