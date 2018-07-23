#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using System.Drawing;
using System.Threading;
using System.Globalization;
using GTI.Modules.Shared;

namespace GTI.Modules.ReportCenter.Business
{
    class Configuration
    {
        //common configuration
        internal static bool mForceEnglish = false;
        private static string mFontName = "Trebuchet MS";
        private static Font mUIUniversualFont = new Font(mFontName, 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        private static Size mScreenSize = new Size(1024, 710);
        internal static int operatorID = -1;
        internal static DateTime GamingDate; // Rally DE525 - Date for reports need to be gaming date not the current date.
        internal static int mMachineID = 0;
        internal static int mMachineLocationID = 0;//todo: initialize it
        internal static int LoginStaffID = -1;
        internal static int StaffLoginNumber = -1;
        internal static bool m_showCursor = false;
        internal static string mClientInstallDrive = "c:";
        internal static string mClientinstallDirectory;
        internal const string ReportDirectory = @"\Reports\";
        internal static string mGlobalPrinterName = string.Empty;
        internal static string mReceiptPrinterName = string.Empty;
        internal static bool mMachineAccounts;
        // PDTS 1064
        internal static MagneticCardReaderMode mMagCardReaderMode;
        internal static MSRSettings mMagCardReaderSettings = new MSRSettings();
 
        internal static int PreDefinedCustomizeTypeCount = 10;
        internal static int PreDefinedCustomizeTypeNameLength = 15;
        //db related
        internal static string DBServer;
        internal static string DBName;
        internal static string DBUser;
        internal static string DBPassword;

        //Logger related
        public static bool mIsLogStarted = false;
        public static bool m_enableLogging = false;
        public static string mLoggerName = "ReportCenter_Logger";
        public static int mWindowLogLevel = -1; //not set yet, it should be 0-7
        public static int mFileLogLevel = -1;
        public static int mConsoleLogLevel = -1;
        public static int mDebugLogLevel = -1;
        public static int mDatabaseLogLevel = -1;
        public static int mSocketLogLevel = -1;
        public static int mUDPLogLevel = -1;
        public static int mEventLogLevel = -1;
        public static string mLogServerName = "";
        public static string mLogDatabaseName = "";
        public static string mLogUserName = "";
        public static string mLogPassword = "";
        public static string mLogSocketIPAddress = "";
        public static int mLogSocketPort = -1;
        public static string mUPDIPAdress = "";
        public static int mUDPPort = -1;
        public static long mRecycleDays = 7;
        
        public static bool m_playWithPaper; //DE 4505
        public static bool m_accrualsEnabled = false;
        public static bool m_txPayoutsEnabled = false;
        public static int m_byPackage = 1;
        public static int RaffleOrDrawing;


        #region Public Properties



        public static void EnableFileLog(int level,long recycleDays)
        {
            mFileLogLevel = level;
            mRecycleDays = recycleDays;
            Logger.EnableFileLog(level, recycleDays);
        }
        public static void EnableConsoleLog(int level)
        {
            mConsoleLogLevel = level;
            Logger.EnableConsoleLog(level);
        }
        public static void EnableDebugLog(int level)
        {
            mDebugLogLevel = level;
            Logger.EnableDebugLog(level);
        }
        public static void EnableDatabaseLog(int level, string serverName, string dbName, string users, string pwd)
        {
            mDatabaseLogLevel = level;
            mLogServerName = serverName;
            mLogPassword = pwd;
            mLogUserName = users;
            mLogDatabaseName = dbName;
            Logger.EnableDatabaseLog(level, serverName, dbName, users, pwd);
        }
        public static void EnableSocketLog(int level, string ipAddress, int port)
        {
            mSocketLogLevel = level;
            mLogSocketIPAddress = ipAddress;
            mLogSocketPort = port;
            Logger.EnableSocketLog(level, ipAddress, port);            
        }
        public static void EnableUdpLog(int level, string udpAdress, int port)
        {
            mUDPLogLevel = level;
            mUPDIPAdress = udpAdress ;
            mUDPPort = port;
            Logger.EnableUdpLog(level, udpAdress, port);
        }
        public static void EnableEventLog(int level)
        {
            mEventLogLevel = level;
            Logger.EnableEventLog(level);
        }
        #endregion

        #region Public Methods
        public static bool ForceEnglish()
        {
            mForceEnglish = true;
            return ForceLocale("en-US");
        }
        public static bool ForceLocale(string locale)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(locale);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                return true;
            }
            catch
            {
                return false;
            }
        }
        // FIX : TA7892
        /// <summary>
        /// Parses a setting from the server and loads it into the 
        /// POSSettings, if valid.
        /// </summary>
        /// <param name="setting">The setting to parse.</param>
        public static void LoadSetting(LicenseSettingValue setting)
        {
            try
            {
                var param = (LicenseSetting)setting.Id;

                switch (param)
                {
                    case LicenseSetting.EnableAnonymousMachineAccounts:
                        mMachineAccounts = Convert.ToBoolean(setting.Value, CultureInfo.InvariantCulture);
                        break;

                    case LicenseSetting.PlayWithPaper:
                        m_playWithPaper = Convert.ToBoolean(setting.Value, CultureInfo.InvariantCulture);
                        break;

                    case LicenseSetting.AccrualEnabled:
                        m_accrualsEnabled = Convert.ToBoolean(setting.Value, CultureInfo.InvariantCulture);
                        break;

                    case LicenseSetting.EnableTXPayouts:
                        m_txPayoutsEnabled = Convert.ToBoolean(setting.Value, CultureInfo.InvariantCulture);
                        break;
                }
            }
            catch (Exception)
            {
            }
        }
        // END : TA7892

        public static void LoadSetting(SettingValue setting)
        {
            try
            {
                Setting param = (Setting)setting.Id;

                switch (param)
                {
                    case Setting.MagneticCardFilters:
                        mMagCardReaderSettings.setFilters(setting.Value);
                        break;

                    case Setting.ShowMouseCursor:
                        m_showCursor = Convert.ToBoolean(setting.Value);
                        break;

                    case Setting.ForceEnglish:
                        mForceEnglish = Convert.ToBoolean(setting.Value);
                        break;
                    case Setting.LoggingLevel :
                        mFileLogLevel = int.Parse(setting.Value);
#if DEBUG
                        mFileLogLevel =0;
#endif

                        break;
                    case Setting.LogRecycleDays:
                        mRecycleDays = int.Parse(setting.Value);
                        break;

                    case Setting.EnableLogging:
                        m_enableLogging = Convert.ToBoolean(setting.Value);
#if DEBUG
                        m_enableLogging = true;
#endif
                        break;
                    case Setting.GlobalPrinterName:
                        Configuration.mGlobalPrinterName = setting.Value;
                        break;
                    case Setting.POSReceiptPrinterName:
                        Configuration.mReceiptPrinterName = setting.Value;
                        break;
                    case Setting.DatabaseServer:
                        Configuration.DBServer = setting.Value;
                        break;
                    case Setting.DatabaseName:
                        Configuration.DBName = setting.Value;
                        break;
                    case Setting.DatabaseUser:
                        Configuration.DBUser = setting.Value;
                        break;
                    case Setting.DatabasePassword:
                        Configuration.DBPassword = setting.Value;
                        break;
                    case Setting.ClientInstallDrive:
                        Configuration.mClientInstallDrive = setting.Value;
                        break;
                    case Setting.ClientInstallRootDirectory:
                        Configuration.mClientinstallDirectory = setting.Value;
                        break;
                    // FIX : TA7892
                    //case Setting.EnableAnonymousMachineAccounts:
                    //    Configuration.mMachineAccounts = bool.Parse(setting.Value);
                    //    break;
                    // END : TA7892
                    // PDTS 1064
                    case Setting.MagneticCardReaderMode:
                        Configuration.mMagCardReaderMode = (MagneticCardReaderMode)Convert.ToInt32(setting.Value, CultureInfo.InvariantCulture);
                        break;
//                    case Setting.MagneticCardReaderParameters:
//                        Configuration.mMagCardReaderSettings. = setting.Value;
//                        break;
                    // DE4505
                    // FIX : TA7892
                    //case Setting.PlayWithPaper:
                    //    m_playWithPaper = bool.Parse(setting.Value);
                    //    break;
                    // END : TA7892
                    case Setting.PrintRegisterReportByPackage:
                        Configuration.m_byPackage = (setting.Value == bool.TrueString) ? 1 : 0;
                        break;

                    case Setting.RaffleDrawingSetting:
                        Configuration.RaffleOrDrawing = int.Parse(setting.Value);//1 = Raffle; 2 = Drawing
                       // GetReportListExMessage.SystemSettingDrawingOrRaffle = Configuration.RaffleOrDrawing;
                        break;
                    case Setting.MSRReadTriggers:
                        mMagCardReaderSettings.setReadTriggers(setting.Value);
                        break;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
