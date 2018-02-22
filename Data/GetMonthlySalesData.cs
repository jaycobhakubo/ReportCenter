using System;
using System.IO;
using System.Text;
using GTI.Modules.Shared;
using WPFControls;

namespace GTI.Modules.ReportCenter.Data
{
    public class GetMonthlySalesData : ServerMessage 
    {
        public int OperatorId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        private MonthInfo Info { get; set; }

        GetMonthlySalesData(int operatorId, int month, int year, MonthInfo monthInfo)
        {
            m_id = 18170;
            OperatorId = operatorId;
            Month = month;
            Year = year;
            Info = monthInfo;
        }

        public static void MonthlySales(int operatorId, int month, int year, MonthInfo monthInfo)
        {
            var msg = new GetMonthlySalesData(operatorId, month, year, monthInfo);
            try
            {
                msg.Send();
            }
            catch (ServerCommException ex)
            {
                throw new Exception("GetMontlySalesData: " + ex.Message);
            }
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

            if (responseStream.Length < 8)
            {
                // Close the streams.
                responseReader.Close();
                return;
            }

            Info.Attendance = responseReader.ReadInt32();

            ushort strlen = responseReader.ReadUInt16();
            string tmpStr = new string(responseReader.ReadChars(strlen));
            decimal decValue;
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoSales = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.JackpotSales = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.OtherSales = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.JackpotPrizeExpense = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.OtherPrizeExpense = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.WorkerCompExpense = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.AdvertisingExpense = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.HallRentalExpense = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.EquipmentExpense = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.OtherExpenses = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.TicketSales = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.PrizeExpense = decValue;

            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.TicketExpense = decValue;

            // Close the streams.
            responseReader.Close();
        }
    }
}
