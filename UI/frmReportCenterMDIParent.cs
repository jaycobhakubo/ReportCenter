#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using GTI.Modules.ReportCenter.Data;
using GTI.Modules.Shared;
using GTI.Modules.ReportCenter.Business;
using GTI.Modules.ReportCenter.Properties;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace GTI.Modules.ReportCenter.UI
{
    public partial class frmReportCenterMDIParent : Form
    {
        #region Declarations
        //private FrmReportCenter mCenter;
        private FrmReportManager mCenter;       //US1622
        private frmCustomizeReport mCustomizeReport;
        private frmEditReport mEditReport;
        private MichiganQuarterlyReport m_michiganQuarterlyReport;
        private CashAccountabilityForm m_cashAccountability;
        public ToolStrip userReportMenu;
        GetUserReportList mUserReports;
        #endregion
        
        internal Dictionary<int, ReportInfo> ReportsDictionary
        {
            get { return mCenter.ReportsDictionary; }
        }

        internal Dictionary<int, UserReportType> UserReportTypesDictionary
        {
            get
            {
                if (mUserReports == null || mUserReports.ReportTypes == null) return null;
                return mUserReports.ReportTypes;
            }
        }

        #region Constructors
        public frmReportCenterMDIParent()
        {
            InitializeComponent();          
            mCenter = new FrmReportManager { Dock = DockStyle.Fill, MyParent = this };      //US1622
            mCustomizeReport = new frmCustomizeReport(this) {Dock = DockStyle.Fill};
            mCustomizeReport.Closed += CustomizeUserReport_Closed;
        }

        private void ReportCenterMDIParent_Load(object sender, EventArgs e)
        {
            //US1831
            //standardReportsMenu_Click(this, null);
            ReportManagerMenu_Click(this, null);
        }
        #endregion

        #region Events
        public void ExitReportCenter(object sender, EventArgs e)
        {
            Debug.WriteLine("ExitReportCenter");
            Close();
        }

        private void ExitMenu_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("ExitMenu_Click");
            Close();
        }

        #region Close Events
        private void CustomizeUserReport_Closed(object sender, EventArgs e)
        {
            Debug.WriteLine("CustomizeUserReport_Closed");
        }


        private void RefreshAllForm()
        {

        }

        private void mEditReport_Closed(object sender, EventArgs e)
        {
            //Update the whole report
            Debug.WriteLine("mEditReport_Closed");
        }

        #endregion 

        #region ToolStrip Events
        /// <summary>
        /// handles all user defined type click, sender.Tag is the type ID, load all user defined group and its 
        /// Report by the typeID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempToolStripButtonClick(object sender, EventArgs e)
        {
            ToolStripButton tempStripButton = (ToolStripButton)sender;

            UserReportType type;
            UserReportTypesDictionary.TryGetValue(int.Parse(tempStripButton.Tag.ToString()), out type);
            
            //US1831
            //LoadTarget(mCenter);
            //mCenter.LoadUserReportToTreeView(type);
            LoadTarget(mCenter);
            mCenter.LoadUserReportToTreeView(type);

            SelectButton(tempStripButton);
            Cursor.Current = Cursors.Default;
        }

        private void aboutMenu_Click(object sender, EventArgs e)
        {
            ToolStripButtonClick(sender.ToString(),sender);

            //about window
            AboutBox about = new AboutBox
                             {
                                 AssemblyDescription = AssemblyDescription,
                                 AssemblyProduct = AssemblyProduct,
                                 AssemblyVersion = AssemblyVersion,
                                 AssemblyTitle = AssemblyTitle
                             };
            about.ShowDialog();
        }


        private Stream TestStream(string fileLocation)
        {
            Stream fs = File.OpenRead(fileLocation);
            return fs;
        }



        //private Stream TestStream()
        //{
        //    Stream fs = File.OpenRead(@"c:\testdocument.docx");
        //    return fs;
        //}

        //// This method converts the filestream into a byte array so that when it is 
        //// used in my ASP.Net project the file can be sent using response.Write
        //private void Test()
        //{
        //    System.IO.MemoryStream data = new System.IO.MemoryStream();
        //    System.IO.Stream str = TestStream();

        //    str.CopyTo(data);
        //    byte[] buf = new byte[data.Length];
        //    data.Read(buf, 0, buf.Length);
        //}


        private void importFileMenu_Click(object sender, EventArgs e)
        {
            var m_openFileDialog = new OpenFileDialog();
            m_openFileDialog.Title = "Please select a zip file to import.";
            m_openFileDialog.Filter = "Report File |*.zip";

            if (m_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //lets get the full location.
                var fullpathfilename = m_openFileDialog.FileName;
                FileInfo fileNameFileInfo = new FileInfo(fullpathfilename);
                var pathLocation = fileNameFileInfo.Directory.FullName;

                FileStream fsRptFile;
                FileStream fsSqlFile;
                string sqlReportData = "";
                string rptReportName = "";
                Stream rptReportFile = null;





                string rptFile = "";
              

                using (ZipArchive archive = ZipFile.OpenRead(fullpathfilename))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(".rpt", StringComparison.OrdinalIgnoreCase))
                        {

                            entry.ExtractToFile(Path.Combine(pathLocation, entry.FullName));
                            rptFile = Path.Combine(pathLocation, entry.FullName);//check if the file exists
                            // string x = entry.FullName

                            //using (FileStream fsSource = File.OpenRead(rptFile))
                            //{
                            //    byte[] b = new byte[1024];
                            //    UTF8Encoding temp = new UTF8Encoding(true);
                            //    while (fsSource.Read(b,0,b.Length)>0)
                            //    {
                            //       // temp.GetString(b)
                            //    }

                            //}


                            //convert this file to file stream
                            fsRptFile = File.OpenRead(rptFile);
                            //FileStream fsSource = new FileStream("D:\\csharpfile.txt", FileMode.Open, FileAccess.Read);
                            //using (StreamReader sr = new StreamReader(fsSource))
                            //{
                            //    data = sr.ReadToEnd();
                            //}




                            rptReportFile = TestStream(rptFile);

                            //rptReportName = Path.GetFileNameWithoutExtension(rptFile);
                            rptReportName = Path.GetFileName(rptFile);

                            //Send it to server message
                            //var SetImportFile = new SetImportFile(fs);
                            //SetImportFile.Send();
                            continue;
                        }

                        if (entry.FullName.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
                        {

                            entry.ExtractToFile(Path.Combine(pathLocation, entry.FullName));
                            var sqlFile = Path.Combine(pathLocation, entry.FullName);//check if the file exists

                            //convert this file to file stream
                            fsSqlFile = File.OpenRead(sqlFile);

                            StreamReader sr = new StreamReader(sqlFile);

                            sqlReportData = sr.ReadToEnd();
                            MessageBox.Show(sqlReportData);

                            //Send it to server message
                            //var SetImportFile = new SetImportFile(fs);
                            //SetImportFile.Send();
                            continue;
                        }
                    }
                }

                var SetImportFile = new SetImportFile(sqlReportData, rptReportName, rptReportFile);
                SetImportFile.Send();

        
            }
              
        }

        private void ToolStripButtonClick(string strMode,object sender)
        {
            //All=0,
            //Sales=1,
            //Paper=2,
            //Player=3,
            //Misc=4,
            //Staff=5,
            //POS=6,
            //Bingo=7,
            //Electronics=8,
            //Exceptions=9,
            //Custom=10
            ToolStripButton btn = null;

            if (sender is ToolStripButton)
            {
                btn = (ToolStripButton)sender;
            }
            
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                switch (strMode)
                {
                    case "Electronics":
                        LoadTarget(mCenter);
                        mCenter.LoadReportToTreeView(ReportTypes.Electronics );
                        break;
                    case "Exceptions":
                        LoadTarget(mCenter);
                        mCenter.LoadReportToTreeView(ReportTypes.Exceptions);
                        break;
                    case "Players":
                        LoadTarget(mCenter);
                        mCenter.LoadReportToTreeView(ReportTypes.Player );
                        break;
                    case "Bingo":
                        LoadTarget(mCenter);
                        mCenter.LoadReportToTreeView(ReportTypes.Bingo);
                        break;
                    case "Misc":
                        LoadTarget(mCenter);
                        mCenter.LoadReportToTreeView(ReportTypes.Misc);
                        break;
                    case "Sales":
                        LoadTarget(mCenter);
                        mCenter.LoadReportToTreeView(ReportTypes.Sales);
                        break;

                    case "Standard Reports":
                        LoadTarget(mCenter);
                        mCenter.LoadPredefinedReports();
                        break;

                        //US1622
                    case "Report Manager":
                        LoadTarget(mCenter);
                        mCenter.LoadPredefinedReports();
                        break;
                    
                    case "Customize":
                        if (mCustomizeReport == null || mCustomizeReport.Disposing ||
                            mCustomizeReport.IsDisposed)
                        {
                            mCustomizeReport = new frmCustomizeReport(this);
                            mCustomizeReport.Closed += CustomizeUserReport_Closed;
                            mCustomizeReport.Dock = DockStyle.Fill;
                        }
                        LoadTarget(mCustomizeReport);
                        return; 

                    default :
                        break;
                }

                if (btn != null)
                {
                    SelectButton(btn);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageForm.Show(this,"ToolStripButtonClick()..Exception: " + ex.Message, Resources.report_center );
            }
        }

        #endregion
        #endregion
        
        #region Methods

        private void SelectButton(ToolStripButton toolButton)
        {
            try
            {
                if (toolButton == null)
                    return;

                DiableButtons();
                toolButton.CheckState = CheckState.Checked;

                if (!toolButton.Name.Equals("customizeToolStripButton"))
                {
                }
                else if (!toolButton.Name.Equals("standardToolStripButton"))
                {
                }
            }
            catch (Exception ex)
            {
                MessageForm.Show(this,"SelectButton()..Exception: " + ex.Message,Resources.report_center );
            }
        }

        public void DiableButtons()
        {
            if (userReportMenu != null && userReportMenu.Items.Count > 0)
            {
                foreach (ToolStripItem item in userReportMenu.Items)
                {
                    if (item is ToolStripButton)
                        ((ToolStripButton)item).CheckState = CheckState.Unchecked;
                }
            }
        }

        public void LoadUserDefinedReports(bool showCustomButtons)
        {
            string locale = Configuration.mForceEnglish ? "en-US" : Thread.CurrentThread.CurrentCulture.Name;
            mUserReports = new GetUserReportList(Configuration.operatorID, Configuration.LoginStaffID,
                                               locale);
            try
            {
                mUserReports.Send();
            }
            catch (Exception ex)
            {
                MessageForm.Show(this, Resources.errorLoadData + "...Exception: " + ex.Message, Resources.report_center);
                return;
            }

            // FIX: DE6958 Crash when database has no custom reports
            if (mUserReports.ReportTypes.Count > 0)
            // END: DE6958 Crash when database has no custom reports
            {
                // Update the MD5s and file names from the data we already have.
                foreach (KeyValuePair<int, UserReportType> userType in mUserReports.ReportTypes)
                {
                    foreach (KeyValuePair<int, UserReportGroup> userGroups in userType.Value.UserReportGroups)
                    {
                        for (int x = 0; x < userGroups.Value.ReportsArray.Count; x++)
                        {
                            ReportInfo info = (ReportInfo)userGroups.Value.ReportsArray[x];

                            if (ReportsDictionary.ContainsKey(info.ID))
                            {
                                // MD5 Hash
                                if (ReportsDictionary[info.ID].Hash != null)
                                {
                                    info.Hash = new byte[ReportsDictionary[info.ID].Hash.Length];
                                    Array.Copy(ReportsDictionary[info.ID].Hash, info.Hash, info.Hash.Length);
                                }
                                else
                                    info.Hash = null;

                                // File Name
                                info.FileName = ReportsDictionary[info.ID].FileName;
                                userGroups.Value.ReportsArray[x] = info;
                            }
                        }
                    }
                }
            }

            if (mUserReports.ReportTypes.Count < 1 )
            {
                //clean up if there is old types there            
                menuStrip.SuspendLayout();
                SuspendLayout();
                if (userReportMenu != null)
                {
                    int buttons = userReportMenu.Items.Count;
                    for (int iButton = 0; iButton < buttons; iButton++)
                    {
                        userReportMenu.Items[0].Click -= TempToolStripButtonClick;
                        userReportMenu.Items.RemoveAt(0);
                    }
                    Controls.Remove(userReportMenu);
                    userReportMenu.Dispose();
                    userReportMenu = null;
                    GC.Collect();
                }
                //reorder it as backword
                Control[] myControls = new Control[Controls.Count];
                int iCounter = 0, controlCount = Controls.Count;
                for (int iControl = 0; iControl < controlCount; iControl++)
                {
                    myControls[iCounter++] = Controls[0];
                    Controls.RemoveAt(0);
                }
                //add it back
                for (int iControl = 0; iControl < myControls.Length; iControl++)
                {
                    Controls.Add(myControls[iControl]);
                }
                
                
                menuStrip.ResumeLayout(false);
                ResumeLayout(true);
                PerformLayout();
                if (mCenter != null)
                {
                    mCenter.Dock = DockStyle.Fill;
                    mCenter.Refresh();
                }
                Refresh();
            }
            else
            {
                if (showCustomButtons) //RALLY DE 6239
                {
                    InitializeUserReport();
                }         
            }
        }

        private void InitializeUserReport()
        {
            //create a toolbar and user Report types
            userReportMenu = new ToolStrip();
            userReportMenu.SuspendLayout();
            SuspendLayout();
            userReportMenu.Location = new Point(0, 62);//toolStrip.Location.Y + toolStrip.Size.Height); // new System.Drawing.Point(0, 61);
            userReportMenu.Size = new Size(1016, 29);
            userReportMenu.Name = "userReportMenu";
            userReportMenu.TabIndex = 2;
            userReportMenu.Text = "userReportMenu";
            userReportMenu.GripStyle = ToolStripGripStyle.Hidden;
            ToolStripButton tempToolStripButton;
            int separatorCounter = mUserReports.ReportTypes.Count;
            //add all user types toolstripbuttons
            foreach (KeyValuePair<int, UserReportType> userType in mUserReports.ReportTypes)
            {
                tempToolStripButton = new ToolStripButton
                                      {
                                          DisplayStyle = ToolStripItemDisplayStyle.Text,
                                          Font = new Font("Trebuchet MS", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                                          ImageTransparentColor = Color.ForestGreen,
                                          Name = "button1",
                                          Text = userType.Value.UserReportTypeName,
                                          Tag = userType.Value.UserReportTypeID
                                      };
                tempToolStripButton.Click += TempToolStripButtonClick;
                userReportMenu.Items.Add(tempToolStripButton);
                separatorCounter--;
                if (separatorCounter > 0)
                {
                    ToolStripSeparator tempSeparator = new ToolStripSeparator();
                    userReportMenu.Items.Add(tempSeparator);
                }
            }

            //reorder it as backword
            Control[] myControls = new Control[Controls.Count];
            int iCounter = 0, controlCount = Controls.Count;
            for (int iControl = 0; iControl < controlCount; iControl++)
            {
                if (Controls[0].Name != "userReportMenu") //we only add this once
                {
                    myControls[iCounter++] = Controls[0];
                }

                Controls.RemoveAt(0);
            }
            Controls.Add(userReportMenu);
            //add it back
            for (int iControl = 0; iControl < myControls.Length; iControl++)
            {
                Controls.Add(myControls[iControl]);
            }

            userReportMenu.ResumeLayout(false);
            userReportMenu.PerformLayout();
            ResumeLayout(true);
            PerformLayout();
        }

        public void RefreshReport()
        {
            mCenter.RefreshReport();
        }

        public bool LoadTarget(Form target)
        {
            bool result = false;
            try
            {
                SuspendLayout();
                if (mEditReport != null)
                {
                    if (mEditReport.IsMdiChild && mEditReport.IsModified())
                    {

                        mEditReport.Close();
                        if (mEditReport.CancelClosingEvent == true)
                        {
                            result = true;
                            return result;
                        }

                        if (mEditReport.IsRefreshRequired)
                        {
                            target.Name = "";
                            mEditReport.IsRefreshRequired = false;
                        }

                    }
                  
                }

                if (MdiChildren.Length > 0)
                {
                    foreach (Form frmTest in MdiChildren)
                    {
                        if (frmTest.Name.Equals(target.Name))
                        {
                            SetMDIFormValues(target);
                            target.BringToFront();
                            ResumeLayout(true);
                            PerformLayout();
                            return result;
                        }
                    }
                }
                SetMDIFormValues(target);
                target.Show();
                ResumeLayout(true);
                PerformLayout();
            }
            catch (Exception ex)
            {
                MessageForm.Show(this, "LoadTarget()...Exception: " + ex.Message, "Report Selection");
            }

            if (mEditReport != null)
            {

                mEditReport.IsRefreshRequired = false;
            }

            return result;
        }

        private void SetMDIFormValues(Form frmTemp)
        {
            try
            {

                frmTemp.AutoScroll = false;
                frmTemp.ControlBox = false;
                if (frmTemp.Name.ToUpper() != "frmReport")
                {
                    frmTemp.MdiParent = this;
                    frmTemp.WindowState = FormWindowState.Maximized;
                    frmTemp.Dock = DockStyle.Fill;

                }                
                else
                {
                    frmTemp.Height = Height;
                    frmTemp.Width = Width;
                    frmTemp.WindowState = FormWindowState.Normal;
                }

                frmTemp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageForm.Show(this, "SetMDIFormValues()...Exception: " + ex.Message, "Report Selection");
            }
        }

        public bool LoadReports(SplashScreen splashScreen)
        {
            if (mCenter == null)
            {
                //mCenter = new FrmReportCenter();
                mCenter = new FrmReportManager();   //US1622
            }

            if(!mCenter.GetReportsFromServer(splashScreen))
                return false;

            //load user defined type
            LoadUserDefinedReports(true);

            return true;
        }

        #endregion 

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion          

        private void standardReportsMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (!LoadTarget(mCenter))
                {
                    InitializeUserReport();
                    mCenter.LoadPredefinedReports();
                }
            }
            catch (Exception ex)
            {
                MessageForm.Show("standardReportsToolStripMenuItem_Click()..Exception: " + ex.Message);
            }
        }

        //US1622
        private void ReportManagerMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (!LoadTarget(mCenter))
                {
                    mCenter.LoadPredefinedReports();
                }
            }
            catch (Exception ex)
            {
                MessageForm.Show("ReportManagerMenu_Click()..Exception: " + ex.Message);
            }
        }


        private void customizedReportsMenu_Click(object sender, EventArgs e)
        {
        
                try
                {
                    if (mCustomizeReport == null || mCustomizeReport.Disposing ||
                                 mCustomizeReport.IsDisposed)
                    {
                        mCustomizeReport = new frmCustomizeReport(this);
                        mCustomizeReport.Closed += CustomizeUserReport_Closed;
                        mCustomizeReport.Dock = DockStyle.Fill;
                    }
                    LoadTarget(mCustomizeReport);

                }
                catch (Exception ex)
                {
                    MessageForm.Show(this, "customizedReportsToolStripMenuItem_Click()..Exception: " + ex.Message, Resources.report_center);
                }
        
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (mEditReport == null || mEditReport.Disposing ||
                             mEditReport.IsDisposed)
                {
                    mEditReport = new frmEditReport(this);
                    mEditReport.Closed += mEditReport_Closed;
                    mEditReport.Dock = DockStyle.Fill;
                }
                mEditReport.LoadDataIntoTheDataGrid();
     
                LoadTarget(mEditReport);
                mEditReport.HideReportMenu();
                //userReportMenu.Visible = false;
            }
            catch (Exception ex)
            {
                MessageForm.Show(this, "editToolStripMenuItem_Click()..Exception: " + ex.Message, Resources.report_center);
            }
        }

        private void michiganQuarterlyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
                try
                {
                    if (m_michiganQuarterlyReport == null || m_michiganQuarterlyReport.Disposing ||
                                 m_michiganQuarterlyReport.IsDisposed)
                    {
                        m_michiganQuarterlyReport = new MichiganQuarterlyReport { Location = Location, Size = Size };
                    }

                    m_michiganQuarterlyReport.Show();
                }
                catch (Exception ex)
                {
                    MessageForm.Show(this, "MichiganQuarterlyReport..Exception: " + ex.Message, Resources.report_center);
                }
           
        }

        private void cashAccountabilityReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_cashAccountability == null || m_cashAccountability.Disposing ||
                             m_cashAccountability.IsDisposed)
                {
                    m_cashAccountability = new CashAccountabilityForm {Location = Location, Size = Size};
                }

                m_cashAccountability.Show();
            }
            catch (Exception ex)
            {
                MessageForm.Show(this, "CashAccountabilityForm..Exception: " + ex.Message, Resources.report_center);
            }
        }      
    }
}
