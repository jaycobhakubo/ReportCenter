#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using CrystalDecisions.CrystalReports.Engine;
using ReportInfo=GTI.Modules.Shared.ReportInfo;

// FIX : TA4979 (mostly cleanup)

namespace GTI.Modules.ReportCenter.Data
{
    /// <summary>
    ///   This Class contains all the parameters needed for
    /// a Report.
    /// </summary>
   public class clsReport
    {
        #region Properties
        public ReportDocument CrystalRptDoc { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public ReportInfo ReportInfo { get; set; }
        public string ErrMessage { get; set; }
        public bool PrintReport { get; set; }
        public int StaffID { get; set; }
        public int PlayerID { get; set; }
        public int Session { get; set; }
        public int GamingDate { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
        public int MachineID { get; set; }

        public string AccrualsName { get; set; }    // US1622
        public string AccrualsType { get; set; }    // US1622
        public int AccrualsStatus { get; set; }     // US1622

        public int PlayerTaxID { get; set; }        // US1814  allow user to print blank forms
        public int PositionID { get; set; }         // US1850
        //public int SerialNo { get; set; }           // US1747
        public string SerialNo { get; set; }           // DE12064
        public int ProductItemID { get; set; }      // US1754
        public int CharityId { get; set; }          // US2715
        public int ProgramId { get; set; }          // US2744
        public int POSMenuId { get; set; }          // US2744
        public int ProductTypeId { get; set; }
        public int LocationID { get; set; }         // US1754
        public int ProductGroupID { get; set; }     // US1902
        public string SerialNbrDevice { get; set; }    // US1839
        public int AuditLogType { get; set; }
        public int ByPackage { get; set; }          //US1808
        public int CompID { get; set; }//knc_3
        public int isActive { get; set; }
        #endregion

        public clsReport()
        {

        }
        public clsReport(ReportInfo obj)
        {
            ReportInfo = obj;
        }

        public override string ToString()
        {
            return ReportInfo.DisplayName != "" ? ReportInfo.DisplayName : "clsReport";
        }
    }
}
// END : TA4979 (mostly cleanup)
