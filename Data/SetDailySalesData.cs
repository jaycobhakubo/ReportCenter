using System;
using System.Globalization;
using System.IO;
using System.Text;
using GTI.Modules.Shared;
using WPFControls;

namespace GTI.Modules.ReportCenter.Data
{
    public class SetDailySalesData : ServerMessage 
    {
        public int OperatorId { get; set; }
        private CashInfo Info { get; set; }

        SetDailySalesData(int operatorId, CashInfo info)
        {
            m_id = 18177;
            OperatorId = operatorId;
            Info = info;
        }

        public static void DailySales(int operatorId, CashInfo info)
        {
            var msg = new SetDailySalesData(operatorId, info);
            try
            {
                msg.Send();
            }
            catch (ServerCommException ex)
            {
                throw new Exception("SetDailySalesData: " + ex.Message);
            }
        }

        protected override void PackRequest()
        {
            try
            {
                MemoryStream requestStream = new MemoryStream();
                BinaryWriter requestWriter = new BinaryWriter(requestStream, Encoding.Unicode);

                // Operator Id
                requestWriter.Write(OperatorId);

                // Gaming Session
                requestWriter.Write(Info.SessionNumber);

                // Gaming date
                DateTime sessionDate = (DateTime)Info.Date;
                if (sessionDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Length > 0)
                {
                    requestWriter.Write((ushort)sessionDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Length);
                    requestWriter.Write(sessionDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).ToCharArray());
                }
                else
                {
                    throw new Exception("SetDailySalesData given invalid date.");
                }

                // Attendance
                requestWriter.Write(Info.Attendance);

                // BingoSales
                string tmpStr = Info.BingoSales.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Jackpot Sales
                tmpStr = Info.BingoJackpotSales.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Other Sales
                tmpStr = Info.BingoOtherRevenue.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Jackpot Prize Expense
                tmpStr = Info.BingoJackpotPrizeExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Other Prize Expense
                tmpStr = Info.BingoOtherPrizeExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Workers Comp Expense
                tmpStr = Info.BingoWorkerCompExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Advertising Expense
                tmpStr = Info.AdvertisingExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Hall Rental Expense
                tmpStr = Info.HallRentalExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Equipment Expense
                tmpStr = Info.EquipmentExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Other Expense
                tmpStr = Info.OtherExpenses.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Ticket Sales
                tmpStr = Info.CharityTicketSales.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Prize Expense
                tmpStr = Info.CharityPrizes.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Ticket Expense
                tmpStr = Info.TicketExpense.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Master Control
                tmpStr = Info.BingoMasterRevenue.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Disposable Bingo Card Account
                tmpStr = Info.BingoDisposableRevenue.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Electronic Sales
                tmpStr = Info.BingoElectronicRevenue.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Net Proceeds
                tmpStr = Info.NetProceeds.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Starting Cash
                tmpStr = Info.StartCash.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Calculated Deposit
                tmpStr = Info.CalculatedDeposit.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Actual Deposit
                tmpStr = Info.ActualDeposit.ToString();
                requestWriter.Write((ushort)tmpStr.Length);
                requestWriter.Write(tmpStr.ToCharArray());

                // Discrepancy
                tmpStr = Info.Discrepancy.ToString();
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
