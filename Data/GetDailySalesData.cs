using System;
using System.Globalization;
using System.IO;
using System.Text;
using GTI.Modules.Shared;
using WPFControls;

namespace GTI.Modules.ReportCenter.Data
{
    public class GetDailySalesData : ServerMessage 
    {
        public int OperatorId { get; set; }
        public int SessionNum { get; set; }
        public DateTime SessionDate { get; set; }
        private CashInfo Info { get; set; }

        GetDailySalesData(int operatorId, DateTime sessionDate, int sessionNum, CashInfo info)
        {
            m_id = 18176;
            OperatorId = operatorId;
            SessionDate = sessionDate;
            SessionNum = sessionNum;
            Info = info;
        }

        public static void GetSales(int operatorId, DateTime sessionDate, int sessionNum, CashInfo info)
        {
            GetDailySalesData msg = new GetDailySalesData(operatorId, sessionDate, sessionNum, info);
            try
            {
                msg.Send();
            }
            catch (ServerCommException ex)
            {
                throw new Exception("GetDailySalesData: " + ex.Message);
            }
        }

        protected override void PackRequest()
        {
            MemoryStream requestStream = new MemoryStream();
            BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

            requestWriter.Write(OperatorId);
            requestWriter.Write(SessionNum);
            if (SessionDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Length > 0)
            {
                requestWriter.Write((ushort)SessionDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Length);
                requestWriter.Write(SessionDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).ToCharArray());
            }
            else
            {
                throw new Exception("GetDailySalesData: Invalid date.");
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

            if (responseStream.Length < 8)
            {
                // Close the streams.
                responseReader.Close();
                Info.InvalidDateOrSession = true;
                return;
            }

            // Modified date
            ushort strlen = responseReader.ReadUInt16();
            string tmpStr = new string(responseReader.ReadChars(strlen));
            if (!string.IsNullOrEmpty(tmpStr))
            {
                Info.ModifiedDate = DateTime.Parse(tmpStr, CultureInfo.InvariantCulture);
            }

            // Attendance
            Info.Attendance = responseReader.ReadInt32();

            // Bingo Sales
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal decValue;
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoSales = decValue;

            // Jackpot Sales
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoJackpotSales = decValue;

            // Other Sales
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoOtherRevenue = decValue;

            // Jackpot Prize Expense
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoJackpotPrizeExpense = decValue;

            // Other Prize Expense
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoOtherPrizeExpense = decValue;

            // Workers Comp Expense
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoWorkerCompExpense = decValue;

            // Advertising Expense
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.AdvertisingExpense = decValue;

            // Hall Rental Expense
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.HallRentalExpense = decValue;

            // Equipment Expense
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.EquipmentExpense = decValue;

            // Other Expense
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.OtherExpenses = decValue;

            // Ticket Sales
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.CharityTicketSales = decValue;

            // Prize Expense
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.CharityPrizes = decValue;

            // Ticket Expense
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.TicketExpense = decValue;

            // Master Control
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoMasterRevenue = decValue;

            // Disposable Bingo Card Account
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoDisposableRevenue = decValue;

            // Electronic Sales
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.BingoElectronicRevenue = decValue;

            // Net Proceeds
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.NetProceeds = decValue;

            // Starting Cash
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.StartCash = decValue;

            // Calculated Deposit
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.CalculatedDeposit = decValue;

            // Actual Deposit
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.ActualDeposit = decValue;

            // Discrepancy
            strlen = responseReader.ReadUInt16();
            tmpStr = new string(responseReader.ReadChars(strlen));
            decimal.TryParse(tmpStr, out decValue);
            Info.Discrepancy = decValue;

            // Beginning Transaction Number
            Info.StartTransactionNumber = responseReader.ReadInt32();
            
            // Ending Transaction Number
            Info.EndTransactionNumber = responseReader.ReadInt32();
            // Close the streams.
            responseReader.Close();
            Info.InvalidDateOrSession = false;
        }
    }
}
