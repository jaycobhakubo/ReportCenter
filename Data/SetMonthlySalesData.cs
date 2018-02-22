using System;
using System.IO;
using System.Text;
using GTI.Modules.Shared;
using WPFControls;

namespace GTI.Modules.ReportCenter.Data
{
    public class SetMonthlySalesData : ServerMessage 
    {
        public int OperatorId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        private MonthInfo Info { get; set; }

        SetMonthlySalesData(int operatorId, int month, int year, MonthInfo monthInfo)
        {
            m_id = 18173;
            OperatorId = operatorId;
            Month = month;
            Year = year;
            Info = monthInfo;
        }

        public static void MonthlySales(int operatorId, int month, int year, MonthInfo monthInfo)
        {
            var msg = new SetMonthlySalesData(operatorId, month, year, monthInfo);
            try
            {
                msg.Send();
            }
            catch (ServerCommException ex)
            {
                throw new Exception("SetMonthlySalesData: " + ex.Message);
            }
        }

        protected override void PackRequest()
        {
            try
            {
                MemoryStream requestStream = new MemoryStream();
                BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

                requestWriter.Write(OperatorId);
                requestWriter.Write(Month);
                requestWriter.Write(Year);

                // Attendance
                requestWriter.Write(Info.Attendance);

                // BingoSales
                string tmpStr = Info.BingoSales.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.JackpotSales.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.OtherSales.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.JackpotPrizeExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.OtherPrizeExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.WorkerCompExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.AdvertisingExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.HallRentalExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.EquipmentExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.OtherExpenses.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.TicketSales.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.PrizeExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // BingoSales
                tmpStr = Info.TicketExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Set the bytes to be sent.
                m_requestPayload = requestStream.ToArray();

                // Close the streams.
                requestWriter.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
