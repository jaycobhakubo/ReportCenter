#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

    ///-----------------------------------------------------------------------------
    /// Project		: SecurityCenter
    /// Class		    : Utilities 
    ///
    ///-----------------------------------------------------------------------------
    /// <summary>
    /// an utility class for this assembly, such as logging, localization, etc. 
    /// </summary>
    /// <history>
    /// 	[Bing Li] 	7/2/2007	Created
    /// </history>
    ///--------------------------------------------------------------------------------

namespace GTI.Modules.ReportCenter.Business
{

        using System;
        using System.Collections.Generic;
        using System.Text;
        using System.ComponentModel;
        using System.Diagnostics;
        using GTI.Modules.Shared;
        internal sealed class Utilities
        {
            private const string LogPrefix = "Report Center - ";
            //private static ModuleFileLog mLog;
            private const string IN = "In   ...";
            private const string LEAVE = "Leave...";
            private static StackFrame mStackFrame;

            //not allow new instance
            private Utilities() { }
            
            public static void LogInfoIN()
            {
                StackFrame frame = new StackFrame(1, true);
                string method = frame.GetMethod().ToString();
                 Log(IN + method,LoggerLevel.Information );
            }
            public static void LogInfoLeave()
            {
                StackFrame frame = new StackFrame(1, true);
                string method = frame.GetMethod().ToString ();
                Log(LEAVE+ method, LoggerLevel.Information);
            }
            internal static void Log(string message, LoggerLevel level)
            {
                object logSync = new object();
                lock (logSync)
                {
                    StackFrame frame = new StackFrame(1, true);
                    string fileName = frame.GetFileName();
                    int lineNumber = frame.GetFileLineNumber();
                    message = LogPrefix + message;

                    if (Configuration.m_enableLogging)
                    {
                        switch (level)
                        {
                            case LoggerLevel.Severe:
                                Logger.LogSevere(message, fileName, lineNumber);
                                break;

                            case LoggerLevel.Warning:
                                Logger.LogWarning(message, fileName, lineNumber);
                                break;

                            default:
                            case LoggerLevel.Information:
                                Logger.LogInfo(message, fileName, lineNumber);
                                break;

                            case LoggerLevel.Configuration:
                                Logger.LogConfig(message, fileName, lineNumber);
                                break;

                            case LoggerLevel.Debug:
                                Logger.LogDebug(message, fileName, lineNumber);
                                break;

                            case LoggerLevel.Message:
                                Logger.LogMessage(message, fileName, lineNumber);
                                break;

                            case LoggerLevel.SQL:
                                Logger.LogSql(message, fileName, lineNumber);
                                break;
                        }
                    }
                }
            }

            public static StackFrame LogStackFrame
            {
                get
                {
                    if (object.ReferenceEquals(mStackFrame, null))
                    {
                        mStackFrame = new StackFrame(1, true);
                    }
                    return mStackFrame;
                }
            }
            public static void InitLog()
            {
                if (Configuration.mIsLogStarted == false)
                {
                    Configuration.mIsLogStarted = true;                   
                    if (Configuration.mFileLogLevel == -1)
                    {
                        Configuration.EnableFileLog(0, Configuration.mRecycleDays);
                    }
                    else
                    {
                        Configuration.EnableFileLog(Configuration.mFileLogLevel, Configuration.mRecycleDays);
                    } 
                    Logger.StartLogger(Logger.StandardPrefix);
                }   
            }     
       
        }
    }
