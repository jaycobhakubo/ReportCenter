#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Security.Cryptography;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using GTI.Modules.Shared;
using GTI.Modules.ReportCenter.Data;
using GTI.Modules.ReportCenter.UI;
using GTI.Modules.ReportCenter.Properties;

// FIX : TA4979 (mostly cleanup)
namespace GTI.Modules.ReportCenter.Business
{
    /// <summary>
    ///   This class was added to handle report building. 
    /// It DOES NOT handle module start up task like many other 
    /// Business classes named with the Module Name.
    /// 
    /// </summary>
    public class ReportCenter
    {
        public event ReportErrorEventHandler RunReportsError;

        private const string StrClassName = "ReportCenter.";

        #region Declarations
        private Dictionary<int, object> m_reports;
        private BackgroundWorker m_worker;
        private WaitForm m_waitForm;
        private readonly NormalDisplayMode m_displayMode = new NormalDisplayMode();
        #endregion

        #region Properties
        public ReportInfo ReportInfo { get; set; }
        public string ErrMessage { get; set; }
        public bool PrintReport { get; set; }
        public int StaffID { get; set; }
        public int PlayerID { get; set; }
        public int Session { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public clsReport[] ReportObjects { get; set; }
        public bool IsCanceled { get; set; }
        #endregion

        #region Constructors & Destructor 

        public ReportCenter()
        {
            InitObject();
        }

        private static void InitObject()
        {
            Utilities.Log("ReportCenter()..Constructor fired...init database varibles.", LoggerLevel.Information);
        }
        
        #endregion
        
        public void FireReportThread(WaitForm frm, Dictionary<int, object> col)
        {
            try
            {
                Utilities.LogInfoIN();
                //BJS 5/13/11 US1831: new viewer has its own wait window 
                m_waitForm = frm;
                IsCanceled = false;
                m_waitForm.CancelButtonClick += MfrmWaitCancelButtonClick;
                m_waitForm.ProgressBarChanged += WaitForm_ProgressBarChanged;
                // Create the mthdWorker thread and run it.
                m_worker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                //m_worker = new BackgroundWorker { WorkerReportsProgress = false, WorkerSupportsCancellation = false };
                
                m_worker.DoWork += ReportWorkThread_DoWork;
                m_worker.ProgressChanged += m_waitForm.ReportProgress;
                m_worker.RunWorkerCompleted += ReportWorkThread_WorkCompleted;
                m_worker.RunWorkerAsync(col);
            }
            catch (Exception e)
            {
                MessageForm.Show(m_displayMode, "Start Report processing Error. FireReportThread()....Exception:" + e.Message, MessageFormTypes.OK);
            }
        }
        /// <summary>
        /// Update the message during the updating of the progress bar. 
        /// This is done on the UI thread of the waitform.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitForm_ProgressBarChanged(object sender, EventArgs e)
        {
            m_waitForm.Message = m_retrieveMessage;
        }

        private string m_retrieveMessage;

        private void ReportWorkThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Utilities.LogInfoIN();
            m_reports = (Dictionary<int, object>)e.Argument;

            if (m_reports.Count > 0)
            {
                float fltNumRpts = m_reports.Count;

                ReportObjects = new clsReport[m_reports.Count];

                for (int c = 0; c < m_reports.Count; c++)
                {
                    if (m_worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    ReportObjects[c] = (clsReport)m_reports[c];
                    m_retrieveMessage = "Retrieving:" + ReportObjects[c].ReportInfo.DisplayName;
                    m_worker.ReportProgress((int)((c / fltNumRpts) * 100.0));
                    BuildReportDocument(ReportObjects[c]);
                }
            }
        }

        private void ReportWorkThread_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ////US1831
            //return;

            m_waitForm.CancelButtonClick -= MfrmWaitCancelButtonClick;
            m_waitForm.ProgressBarChanged -= WaitForm_ProgressBarChanged;

            if (m_worker != null)
            {
                if (m_worker.IsBusy)
                {
                    m_worker.CancelAsync();
                }
            }
            
            if (m_waitForm != null)
            {
                m_waitForm.CloseForm();
                m_waitForm.Dispose();
                m_waitForm = null;
            }

            if (e.Error != null)
            {
                // This is an Error. 
                ReportEventArgs agrs = new ReportEventArgs(e.Error.Source, e.Error.Message, "");
                OnRunReportsError(agrs);
            }
        }

        private void MfrmWaitCancelButtonClick(object sender, EventArgs e)
        {
            if (m_worker != null)
            {
                if (m_worker.IsBusy)
                {
                    m_worker.CancelAsync();
                }
            }
            IsCanceled = true;
        }

        #region ReportDocument Methods

        private ReportDocument LoadReport(string fileName)
        {
            Utilities.LogInfoIN();
            ReportDocument docReport = new ReportDocument();
            
            lock (this)
            {
                try
                {
                    string reportFilePath = Configuration.mClientInstallDrive + Configuration.mClientinstallDirectory + Configuration.ReportDirectory + fileName;

                    if (!File.Exists(reportFilePath))
                    {
                        string msg = "LoadReport: " + reportFilePath + " not found.";
                        Utilities.Log(msg, LoggerLevel.Severe);
                        throw new Exception(msg);
                    }

                    docReport.Load(reportFilePath);
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    string msg = "LoadReport: " + ex.Message;
                    Utilities.Log(msg, LoggerLevel.Severe);
                    throw new Exception(msg);
                }
            }
            Utilities.LogInfoLeave();
            return docReport;
        }

        private void BuildReportDocument(clsReport report)
        {
            Utilities.LogInfoIN();
            ReportDocument doc = LoadReport(report.ReportInfo.FileName);
            try
            {
                //***********************  Code from Report  ************************//
                // loop from Report
                foreach (CrystalDecisions.Shared.IConnectionInfo connInfo in doc.DataSourceConnections)
                {
                    Utilities.Log(report.ReportInfo.DisplayName + ": " + 
                        string.Format("setting database, server={0}, dbname={1}", 
                                      Configuration.DBServer, Configuration.DBName), 
                                      LoggerLevel.Information);
                    connInfo.SetConnection(Configuration.DBServer, Configuration.DBName, Configuration.DBUser, Configuration.DBPassword);
                }

                //set parameters
                //
                // FIX: DE7167
                // Parameter order different between db and rpt file. User unable to view in report center. 
                // Vendor Code = 257 error thrown. Iterate over collection with proper order.
                // Original code: foreach (KeyValuePair<int, string> arg in report.ReportInfo.Parameters)
                Dictionary<int, string> rptParams = new Dictionary<int, string>();
                for (int i = 0; i < doc.ParameterFields.Count; i++)
                { 
                    foreach (KeyValuePair<int, string> arg in report.ReportInfo.Parameters)
                    {
                        if (arg.Value == doc.ParameterFields[i].Name)
                        {
                            if(!rptParams.ContainsKey(arg.Key))
                            {
                                rptParams.Add(arg.Key, arg.Value);
                                break;
                            }
                        }

                        if (doc.ParameterFields[i].Name == "@spRaffleOrDrawingSetting")
                        {                          
                            rptParams.Add(-10, "@spRaffleOrDrawingSetting");
                                break;                            
                        }
                    }
                }
                // END: DE7167
                
                //foreach (KeyValuePair<int, string> arg in report.ReportInfo.Parameters)
                foreach (KeyValuePair<int, string> arg in rptParams)
                {
                    Utilities.Log(report.ReportInfo.DisplayName + ": parameters: ID=" + arg.Key + ",name=" + arg.Value, LoggerLevel.Information);

                    switch ((ReportParamIDs)arg.Key)
                    { //the first two are have to 
                 
                        case ReportParamIDs.ContentLocale:
                            if (Configuration.mForceEnglish)
                            {
                                doc.SetParameterValue(arg.Value, "en-US");
                            }
                            else
                            {
                                doc.SetParameterValue(arg.Value, Thread.CurrentThread.CurrentCulture.Name);
                            }
                            break;
                        case ReportParamIDs.OperatorID:
                            doc.SetParameterValue(arg.Value, Configuration.operatorID);
                            break;
                        case ReportParamIDs.StaffID:
                            doc.SetParameterValue(arg.Value, report.StaffID);
                            break;
                        case ReportParamIDs.StartDate:
                            doc.SetParameterValue(arg.Value, report.StartDate); //.ToShortDateString());
                            break;
                        case ReportParamIDs.EndDate:
                            doc.SetParameterValue(arg.Value, report.EndDate); //.ToShortDateString());
                            break;
                        case ReportParamIDs.PlayerID:
                            doc.SetParameterValue(arg.Value, report.PlayerID);
                            break;
                        case ReportParamIDs.Session:
                            doc.SetParameterValue(arg.Value, report.Session);
                            break;
                        case ReportParamIDs.GamingDate:
                            doc.SetParameterValue(arg.Value, report.GamingDate);
                            break;
                        case ReportParamIDs.Month:
                            switch (report.Quarter)
                            {
                                case 1: doc.SetParameterValue(arg.Value, 1); break;
                                case 2: doc.SetParameterValue(arg.Value, 4); break;
                                case 3: doc.SetParameterValue(arg.Value, 7); break;
                                case 4: doc.SetParameterValue(arg.Value, 10); break;
                            }
                            break;
                        case ReportParamIDs.Year:
                            doc.SetParameterValue(arg.Value, report.Year);
                            break;
                        //START RALLY DE6958 add machine id paramter
                        case ReportParamIDs.MachineID:
                            doc.SetParameterValue(arg.Value, report.MachineID); 
                            break;
                        //END RALLY DE6958

                        // US1622
                        case ReportParamIDs.AccrualsName:
                            doc.SetParameterValue(arg.Value, report.AccrualsName);
                            break;
                        case ReportParamIDs.AccrualsStatus:
                            doc.SetParameterValue(arg.Value, report.AccrualsStatus);
                            break;
                        case ReportParamIDs.AccrualsType:
                            doc.SetParameterValue(arg.Value, report.AccrualsType);
                            break;
                        // END US1622

                        // US1814
                        // Allow user to print blank forms from the ReportCenter...
                        case ReportParamIDs.PlayerTaxID:
                            report.PlayerTaxID = -1;
                            doc.SetParameterValue(arg.Value, report.PlayerTaxID);
                            break;
                        // END US1814

                        case ReportParamIDs.PositionID: // us1850
                            doc.SetParameterValue(arg.Value, report.PositionID);
                            break;

                        case ReportParamIDs.ProductItemID: // us1754
                            doc.SetParameterValue(arg.Value, report.ProductItemID);
                            break;

                        case ReportParamIDs.CharityId: // US2715
                            doc.SetParameterValue(arg.Value, report.CharityId);
                            break;

                        case ReportParamIDs.POSMenu: // US2744
                            doc.SetParameterValue(arg.Value, report.POSMenuId);
                            break;

                        case ReportParamIDs.ProgramID: // US2744
                            doc.SetParameterValue(arg.Value, report.ProgramId);
                            break;

                        case ReportParamIDs.ProductTypeID:
                            doc.SetParameterValue(arg.Value, report.ProductTypeId);
                            break;

                        case ReportParamIDs.InvLocationID: // us1754
                            doc.SetParameterValue(arg.Value, report.LocationID);
                            break;

                        case ReportParamIDs.SerialNumber: // us1747
                            doc.SetParameterValue(arg.Value, report.SerialNo);
                            break;

                        case ReportParamIDs.ProductGroupID: // us1902
                            doc.SetParameterValue(arg.Value, report.ProductGroupID);
                            break;

                        case ReportParamIDs.SerialNbrDevice: // us1839
                            doc.SetParameterValue(arg.Value, report.SerialNbrDevice);
                            break;
                        case ReportParamIDs.AuditLogType:
                            doc.SetParameterValue(arg.Value, report.AuditLogType);
                            break;
                        case ReportParamIDs.ByPackage:  //us1808
                            doc.SetParameterValue(arg.Value, report.ByPackage);
                            break;
                        case ReportParamIDs.RaffleOrDrawingSetting:     
                            doc.SetParameterValue(arg.Value, Configuration.RaffleOrDrawing);
                            break;
                        case ReportParamIDs.CompID:  
                            doc.SetParameterValue(arg.Value, report.CompID);
                            break;
                        case ReportParamIDs.IsActive:  
                            doc.SetParameterValue(arg.Value, report.isActive);
                            break;
                        default:
                            Utilities.Log(report.ReportInfo.DisplayName + " parameters was not found " + arg.Key + " " + arg.Value, LoggerLevel.Severe);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(StrClassName + ": " + ex.Message);
            }
            report.CrystalRptDoc = doc;
        }

        #endregion

        #region Error Handling

        protected virtual void OnRunReportsError(ReportEventArgs e)
        {
            lock (this)
            {
                try
                {
                    if (m_worker != null)
                    {
                        if (m_worker.IsBusy)
                        {
                            m_worker.CancelAsync();
                        }
                    }

                    if (RunReportsError != null)
                    {
                        RunReportsError(null, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageForm.Show("ReportCenter.OnRunReportsError()....Exception: " + ex.Message, Resources.report_center);
                }
            }
        }
        
        #endregion

        /// <summary>
        /// Checks to see if all the reports passed in are on the local 
        /// computer, if not it will download them.
        /// </summary>
        /// <param name="splashScreen">The splash screen to use to report 
        /// progress, or null if no progess is required.</param>
        /// <param name="reports">A dictionary containing the reports to check 
        /// for.</param>
        /// <exception cref="System.ApplicationException">The file could not be 
        /// downloaded from the server.</exception>
        internal static void CacheReports(SplashScreen splashScreen, Dictionary<int, ReportInfo> reports)
        {
            // First check to see if our directory is present.
            string reportPath = Configuration.mClientInstallDrive + Configuration.mClientinstallDirectory + Configuration.ReportDirectory;

            if(!Directory.Exists(reportPath))
            {
                // Create it.
                Utilities.Log("Creating folder '" + reportPath + "'", LoggerLevel.Debug);
                Directory.CreateDirectory(reportPath);
            }

            // Loop through the reports to check to see if we have the latest
            // copy.
            foreach(ReportInfo report in reports.Values)
            {
                // Does the file exist?
                string reportFileName = reportPath + report.FileName;

                if(!File.Exists(reportFileName))
                {
                    Utilities.Log(reportFileName + " not found.  Downloading...", LoggerLevel.Debug);

                    if(splashScreen != null)
                        splashScreen.Status = string.Format(Resources.Downloading, report.DisplayName);

                    DownloadReport(report.ID, reportFileName);
                }

                // Check to see if the MD5 from the server matches this one.
                if(!CheckReportMd5(reportFileName, report.Hash))
                {
                    // The hash is wrong, try to download it.
                    Utilities.Log(reportFileName + " hash does not match.  Downloading...", LoggerLevel.Debug);

                    if(splashScreen != null)
                        splashScreen.Status = string.Format(Resources.Downloading, report.DisplayName);

                    DownloadReport(report.ID, reportFileName);

                    // Check it again.
                    if(!CheckReportMd5(reportFileName, report.Hash))
                    {
                        // Something is really wrong, error out.
                        Utilities.Log(reportFileName + " hash still does not match, terminating.", LoggerLevel.Debug);
                        throw new ApplicationException(string.Format(Resources.errorDownloadFailure, report.DisplayName));
                    }
                }
            }
        }

        /// <summary>
        /// Downloads a report from the server and saves it to the specified 
        /// path.
        /// </summary>
        /// <param name="reportId">The id of the report to download.</param>
        /// <param name="fileName">The path to save the report to.</param>
        internal static void DownloadReport(int reportId, string fileName)
        {
            // Send the message.
            GetReportMessage getReportMsg = new GetReportMessage(reportId);
            getReportMsg.Send();

            // Save the binary data to the path.
            Utilities.Log("Writing " + fileName, LoggerLevel.Debug);
            FileStream rptFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write);
            rptFile.Write(getReportMsg.ReportFile, 0, getReportMsg.ReportFile.Length);

            // Clean up.
            rptFile.Flush();
            rptFile.Close();
            rptFile.Dispose();
        }

        /// <summary>
        /// Compares the fileName's MD5 hash against the one passed in, if the 
        /// match true is returned.
        /// </summary>
        /// <param name="fileName">The path of the file to hash.</param>
        /// <param name="hash">The hash to compare against.</param>
        /// <returns>true if the hashes match; otherwise false.</returns>
        /// <exception cref="System.ArgumentException">hash is null or 0 
        /// length.</exception>
        internal static bool CheckReportMd5(string fileName, byte[] hash)
        {
            if(hash == null || hash.Length == 0)
                throw new ArgumentException("hash");

            // Check to see if the file exists.
            if(!File.Exists(fileName))
                return false;

            // Read the file in.
            Utilities.Log("Opening " + fileName + " for MD5.", LoggerLevel.Debug);
            FileStream rptFile = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] fileBytes = new byte[rptFile.Length];
            rptFile.Read(fileBytes, 0, fileBytes.Length);

            // Release the file.
            rptFile.Close();
            rptFile.Dispose();

            // Compare the hashes.
            Utilities.Log("Comparing hashes for " + fileName, LoggerLevel.Debug);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] fileHash = md5.ComputeHash(fileBytes);

            if(fileHash.Length != hash.Length)
                return false;

            for(int x = 0; x < fileHash.Length; x++)
            {
                if(fileHash[x] != hash[x])
                    return false;
            }

            return true;
        }
    }
}
// END : TA4979 (mostly cleanup)
