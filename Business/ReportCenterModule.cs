#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using GTI.Modules.Shared;
using GTI.Controls;
using GTI.Modules.ReportCenter.UI;
using GTI.Modules.ReportCenter.Properties;


namespace GTI.Modules.ReportCenter.Business
{
    /// <summary>
    /// The implementation of the IGTIModule COM interface.
    /// Debug Command Line: -guid={1E91225C-D482-452d-910C-5C4FE41A0484} -server=GTISERVERLATIN -window
    /// </summary>
    [
        ComVisible(true),
        Guid("1E91225C-D482-452d-910C-5C4FE41A0484"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(_IGTIModuleEvents)),
        ProgId("GTI.Modules.ReportCenter.ReportCenterModule")
    ]
    public sealed class ReportCenterModule : IGTIModule
    {
        #region Constants and Data Types
        private const string ModuleName = "GameTech Edge Bingo System Report Center Module"; // Rally TA8833
        #endregion

        #region Events
        /// <summary>
        /// The signature of the 'Stopped' COM connection point handler.
        /// </summary>
        /// <param name="moduleId">The id of the stopped module.</param>
        public delegate void IGTIModuleStoppedEventHandler(int moduleId);

        /// <summary>
        /// The event that will translate to the COM connection point.
        /// </summary>
        public event IGTIModuleStoppedEventHandler Stopped;

        /// <summary>
        /// Occurs when something wants to stop itself.
        /// </summary>
        internal event EventHandler StopMe;

        internal event EventHandler BringToFront;

        // PDTS 966
        /// <summary>
        /// Occurs when a server initiated message was received from the 
        /// server.
        /// </summary>
        internal event MessageReceivedEventHandler ServerMessageReceived; 
        #endregion

        #region Member Variables
        private object m_syncRoot = new object();
        private int m_moduleId = 0;
        private bool m_isStopped = true;
        private Thread m_reportThread = null;
        SplashScreen mSplashScreen = null;
        frmReportCenterMDIParent mReportMDI = null;
        #endregion

        #region Member Methods
        /// <summary>
        /// Starts the module.  If the module is already started nothing
        /// happens.  This method will block if another thread is currently
        /// executing it.
        /// </summary>
        /// <param name="moduleId">The id to be given to the module.</param>
        public void StartModule(int moduleId)
        {
            lock (m_syncRoot)
            {
                // Don't start again if we are already started.
                if (!m_isStopped)
                    return;

                
                // Assign the id.
                m_moduleId = moduleId;

                // Create a thread to run.
                m_reportThread = new Thread(Run);

                // Change the thread regional settings to the current OS 
                // globalization info.
                m_reportThread.CurrentUICulture = CultureInfo.CurrentCulture;
                m_reportThread.SetApartmentState(ApartmentState.STA);

                // Start it.
                m_reportThread.Start();

                // Mark the module as started.
                m_isStopped = false;
            }
        }

        /// <summary>
        /// Creates the Security Center object and blocks until the Report Center is told to close
        /// or the user closes the Report Center.
        /// </summary>
        private void Run()
        {
           
            mReportMDI = new frmReportCenterMDIParent(); //ReportCenter();

            try
            {
                
                if (mSplashScreen == null)
                {
                    SetSplashScreen();
                }
                mSplashScreen.Show();
                mSplashScreen.Status = Resources.splashInfoLoadStationSettings;
                //get intial module level data
                ModuleComm comm = new ModuleComm();
                Configuration.operatorID = comm.GetOperatorId();
                Configuration.LoginStaffID = comm.GetStaffId();
                Configuration.mMachineID = comm.GetMachineId();
                GetGamingDate(); // Rally DE525
                GetSettings();
                Utilities.InitLog();

                mSplashScreen.Status = Resources.infoCheckingReports;

                if(mReportMDI.LoadReports(mSplashScreen))
                {
                    // Listen for the event where something wants the Security Center to stop.
                    StopMe += new EventHandler(mReportMDI.ExitReportCenter);
                    BringToFront += new EventHandler(ComeToFrontEvent);
                    mSplashScreen.CloseForm();
                    mSplashScreen.Dispose();
                    Utilities.Log("Start running...", LoggerLevel.Information);
                    Application.Run(mReportMDI);
                }
            }
            catch (Exception e)
            {
                Utilities.Log("ReportCenterModule.Run()...Exception.Message =" + e.Message, LoggerLevel.Severe);
                Utilities.Log("Exception.Stack=" + e.StackTrace, LoggerLevel.Severe);

                MessageForm.Show(Resources.errorStart + "\n" + e.Message + "\n\n" + e.StackTrace, Resources.report_center);
            }
            finally 
            { // Shutdown

                if (mSplashScreen != null)
                {
                    mSplashScreen.Close();
                    mSplashScreen.Dispose();
                }

                if (mReportMDI != null)
                {
                    mReportMDI.Close();
                    mReportMDI = null;
                    
                }

                OnStop();
                lock (m_syncRoot)
                {
                    // Mark the module as stopped.
                    m_isStopped = true;
                }

            }
        }

        private void SetMDIFormNormal()
        {
            mReportMDI.WindowState = FormWindowState.Normal;
            mReportMDI.Activate();
        }
        private void ComeToFrontEvent(object sender, EventArgs e)
        {
            Utilities.LogInfoIN();
            
            if (mReportMDI != null)
            {
                Logger.LogInfo ("mReportMDI is not null, calling ComeToFrontEvent", (new StackFrame(true)).GetFileName(), (new StackFrame(true)).GetFileLineNumber());
            
                if (mReportMDI.InvokeRequired)
                {                    
                    Logger.LogInfo("mReportMDI is not null, calling  InvokeRequired", (new StackFrame(true)).GetFileName(), (new StackFrame(true)).GetFileLineNumber());
                    mReportMDI.Invoke(new MethodInvoker (SetMDIFormNormal));
                }
                else
                {
                    Logger.LogInfo("mReportMDI is not null, calling Activate", (new StackFrame(true)).GetFileName(), (new StackFrame(true)).GetFileLineNumber());

                    SetMDIFormNormal();
                }
            }
            Utilities.LogInfoLeave();

        }
        
        /// <summary>
        /// Gets the settings from the server.
        /// </summary>
        private void GetSettings()
        {
            // FIX  TA7892
            //////////////////////////////////////////////
            // Send message for license file settings.
            GetLicenseFileSettingsMessage licenseSettingMsg = new GetLicenseFileSettingsMessage(true);
            licenseSettingMsg.Send();

            // Loop through each setting and parse the value.
            foreach (LicenseSettingValue setting in licenseSettingMsg.LicenseSettings)
            {
                Configuration.LoadSetting(setting);
            }
            // END TA7892

            // Send message for global settings.
            GetSettingsMessage settingsMsg = new GetSettingsMessage(Configuration.mMachineID, Configuration.operatorID, SettingsCategory.GlobalSystemSettings);
            settingsMsg.Send();           

            // Loop through each setting and parse the value.
            SettingValue[] stationSettings = settingsMsg.Settings;
           
            foreach (SettingValue setting in stationSettings)
            {
                if (setting.Id == 182)
                {
                    RaffleSettingDisplayText.valuerf = Convert.ToInt32(setting.Value);
                }
                Configuration.LoadSetting(setting);
            }
        }

        // Rally DE525
        /// <summary>
        /// Gets the operator's gaming date from the server.
        /// </summary>
        private void GetGamingDate()
        {
            GetGamingDateMessage dateMsg = new GetGamingDateMessage(Configuration.operatorID);
            dateMsg.Send();

            Configuration.GamingDate = dateMsg.GamingDate;
        }

        private void SetSplashScreen()
        {
            if (mSplashScreen == null)
            {
                mSplashScreen = new SplashScreen();
            }

            object[] attributes;
            SplashScreen splashing = new SplashScreen();
            attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes != null && attributes[0] != null)
            {
                // Rally US1492
                mSplashScreen.ApplicationName = Resources.report_center;
            }
            attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            mSplashScreen.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
           

            //set the labe, and picture images 
        }
        /// <summary>
        /// This method blocks until the module is stopped.  If the module is 
        /// already stopped nothing happens.
        /// </summary>
        public void StopModule()
        {
            if (m_reportThread != null)
            {
                // Send the stop event to module's controller.
                EventHandler stopHandler = StopMe;

                if (stopHandler != null)
                    stopHandler(this, new EventArgs());

                m_reportThread.Join();
            }
        }

        /// <summary>
        /// Signals the COM connection point that we have stopped.
        /// </summary>
        internal void OnStop()
        {
            IGTIModuleStoppedEventHandler handler = Stopped;

            if (handler != null)
                handler(m_moduleId);
        }

        /// <summary>
        /// Returns the name of this GTI module.
        /// </summary>
        /// <returns>The module's name.</returns>
        public string QueryModuleName()
        {
            return ModuleName;
        }

        public void ComeToFront()
        {
            Utilities.LogInfoIN();
            
            EventHandler handler = BringToFront;

            if (handler != null)
            {
                Logger.LogInfo("handler is not null, call all events", (new StackFrame(true)).GetFileName(), (new StackFrame(true)).GetFileLineNumber());
                handler(this, new EventArgs());
            }
            Utilities.LogInfoLeave();
        }

        // PDTS 966
        /// <summary>
        /// Tells the module that a server initiated message was received.
        /// </summary>
        /// <param name="commandId">The id of the message received.</param>
        /// <param name="messageData">The payload data of the message or null 
        /// if the message has no data.</param>
        public void MessageReceived(int commandId, object msgData)
        {
            MessageReceivedEventArgs args = new MessageReceivedEventArgs(commandId, msgData);

            MessageReceivedEventHandler handler = ServerMessageReceived;

            if (handler != null)
                handler(this, args);
        }
        #endregion
    }

    public class RaffleSettingDisplayText
    {
        public static int valuerf; 
    }
}
