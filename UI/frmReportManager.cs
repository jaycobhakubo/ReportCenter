#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2012 GameTech
// International, Inc.
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

using GTI.Modules.ReportCenter.Business;
using GTI.Modules.ReportCenter.Data;
using GTI.Modules.ReportCenter.Properties;
using GTI.Modules.Shared;
using System.Windows.Documents;
using GTI.Modules.Shared.Business;
using GTI.Modules.Shared.Data;
using System.Drawing.Printing;

//UserStories fixed - US2244 OAS 6/20/2012
//using System.Web.UI.WebControls;

//US1831: redesign this form to accomodate reporting parameters for Accruals Module
//Numerous changes made including dynamically placing widgets at runtime.

//TA11340: Adding support for displaying the ProductItem list

// US2715 Adding support for displaying the Charity list

namespace GTI.Modules.ReportCenter.UI
{

    //public delegate void TreeLostFocusEventHandler(object sender, EventArgs e);

    /// <summary>
    /// This enum ids is from database ReportParameters,
    /// we have to keep syn between both when there is no parameters.
    /// Programmers use this info to set the value of its parameter.
    /// </summary> 
    internal enum ReportParamIDs
    {
        OperatorID = 1,
        ContentLocale = 2,
        StartDate = 3,
        EndDate = 4,
        Session = 5,
        StaffID = 6,
        PlayerID = 7,
        AccrualAccountID = 8,
        ProductItemID = 9,
        ProductGroupID = 10,
        CompID = 11,
        WorkStationID = 12,
        CompRaffleID = 13,
        CustomFieldID = 14,
        DeviceID = 15,
        DiscountID = 16,
        EventID = 17,
        GameCategoryID = 18,
        GameTypeID = 19,
        GiftCardID = 20,
        MachineID = 21,
        MerchandiseGroupID = 22,
        MerchandiseID = 23,
        PaperTemplateID = 24,
        PlayerListID = 25,
        PlayerRaffleID = 26,
        PrizeCheckID = 27,
        ProgramID = 28,
        ProductTypeID = 29,
        PulltabID = 30,
        SlotGamesID = 31,
        ReceiptID = 32,
        GamingDate = 33,
        POSMenu = 35,
        Month = 36,
        Year = 37,

        // US1831: offer all params in ReportParameters table
        SerialNumber = 38,
        LastIssueDate = 39,
        InvoiceNumber = 40,
        RetireDate = 41,
        FormNumber = 42,
        Up = 43,
        CardCutID = 44,
        RetiredOnly = 45,
        InPlayOnly = 46,
        AccrualsName = 47,       //US1622
        VendorID = 48,
        TabName = 49,
        ManufacturerID = 50,
        InventoryItemID = 51,
        AccrualsType = 52,       //US1622
        AccrualsStatus = 53,     //US1622
        // END US1831

        IncludeConcessions = 54,
        IncludeMerchandise = 55,
        IncludePullTabs = 56,
        InvLocationID = 57,
        PlayerTaxID = 58,
        PositionID = 59,
        SerialNbrDevice = 60,        // US1839
        AuditLogType = 61,
        ByPackage = 62,             //US1808
        CharityId = 63,             // US2715
        RaffleOrDrawingSetting = -10,  //Note not in EDGE.Daily.ReportParameter
        IsActive = 64,
    }

    internal enum AccrualTypes
    {
        Session = 1,
        Daily = 2,
        Instant = 3,
        Manual = 4
    }

    public partial class FrmReportManager : GradientForm
    {
        // For flicker free refreshes!
        [DllImport("user32.dll", EntryPoint = "LockWindowUpdate", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        #region Declarations
        private static readonly int PARAMCOUNT = 54;   // Walk thru all params

        private int _lastRow = 0;
        private readonly Dictionary<int, object> m_reportsToRun = new Dictionary<int, object>();
        private DateTime m_enddate;
        private frmReportCenterMDIParent m_frmMyParent;
        private WaitForm m_frmWaiting;
        private GetReportListExMessage m_gotReports;
        private FindStaffOrPlayerMessage m_objStaff;
        private int m_playerID;
        private Dictionary<int, ReportInfo> m_reportsDictionary;
        private int m_session;
        private int m_staffID;
        private DateTime m_startDate;
        private Business.ReportCenter m_objRptCtr;
        private int m_quarter = 1; // FIX : DE3323 set default for quarter
        private int m_year;
        private int m_machineId; //RALLY DE6958
        private GetAccuralMessage m_AccrualsMessage;
        private string m_AccrualsName;
        private string m_AccrualsType;
        private int m_activeStatus;
        private int m_PlayerTaxID;
        private int m_PositionID;
        // private int m_SerialNo;
        private string m_SerialNo = "0";
        private int m_ProductItemID;
        private int m_ProductTypeId;
        private int m_LocationID;
        private int m_auditTypeID;
        private int m_ProductGroupID = 0;       // US1902
        private string m_SerialNbrDevice;
        private int m_ByPackage; //US1808
        private int m_CharityId; // US2715
        private int m_ProgramId; // US2744
        private int m_PosMenuId; // US2744
        private int m_CompId;

        #endregion

        #region Properties
        internal Dictionary<int, ReportInfo> ReportsDictionary { get { return m_gotReports.Reports; } }//knc4
        public Form MyParent { get { return m_frmMyParent; } set { m_frmMyParent = (frmReportCenterMDIParent)value; } }
        public string ErrorMessage { get; set; }
        #endregion

        #region Controls
        // Machine 
        ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmReportManager));
        //FlowLayoutPanel MachinePanel = new FlowLayoutPanel();
        TableLayoutPanel MachinePanel = new TableLayoutPanel();
        Panel panelMachinePlayer = new Panel();
        Label fieldPerson = new Label();
        Label lblPerson = new Label();
        GTI.Controls.ImageButton btnFindPlayer = new GTI.Controls.ImageButton();
        Panel panelMachineStaff = new Panel();
        Label lblStaff = new Label();
        ComboBox cboStaff = new ComboBox();
        Panel panelMachineSession = new Panel();
        ComboBox cboSession = new ComboBox();
        Label lblSession = new Label();
        Panel panelMachineMach = new Panel();
        ComboBox cboMachine = new ComboBox();
        Label lblMachine = new Label();

        //Accruals
        //FlowLayoutPanel AccrualsPanel = new FlowLayoutPanel();
        TableLayoutPanel AccrualsPanel = new TableLayoutPanel();
        Panel typePanel = new Panel();
        ComboBox cboAccType = new ComboBox();
        Label labelAccType = new Label();
        //Panel sessionPanel = new Panel();
        //ComboBox cboAccSession = new ComboBox();
        //Label labelAccSession = new Label();
        Panel namePanel = new Panel();
        ComboBox cboAccName = new ComboBox();
        Label labelAccName = new Label();

        // Dates
        Panel DatesPanel = new Panel();
        DateTimePicker endDatePicker = new DateTimePicker();
        Label lblEndDate = new Label();
        DateTimePicker startDatePicker = new DateTimePicker();
        Label lblStartDate = new Label();

        // Times
        Panel TimesPanel = new Panel();
        DateTimePicker endTimePicker = new DateTimePicker();
        Label lblEndTime = new Label();
        DateTimePicker startTimePicker = new DateTimePicker();
        Label lblStartTime = new Label();
        CheckBox cbTime = new CheckBox();

        // Quarters
        Panel QuartersPanel = new Panel();
        GroupBox groupQuarter = new GroupBox();
        RadioButton rbQ4 = new RadioButton();
        RadioButton rbQ3 = new RadioButton();
        RadioButton rbQ2 = new RadioButton();
        RadioButton rbQ1 = new RadioButton();
        Label lblYear = new Label();
        NumericUpDown upYear = new NumericUpDown();

        //AuditTypePanel
        Panel AuditTypePanel = new Panel();
        GroupBox groupAuditType = new GroupBox();
        ComboBox comboAuditType = new ComboBox();
        Label lblAuditType = new Label();

        // Global stuff
        Label lblReportType = new Label();
        Label lblErrorMessage = new Label();

        // Positions 
        Panel PositionsPanel = new Panel();
        Label lblPositions = new Label();
        ComboBox cboPositions = new ComboBox();

        // Product Items
        Panel ProdItemsPanel = new Panel();
        Label lblProdItems = new Label();
        ComboBox cboProdItems = new ComboBox();

        // Product Types
        Panel ProdTypesPanel = new Panel();
        Label lblProdTypes = new Label();
        ComboBox cboProdTypes = new ComboBox();

        // Locations
        Panel LocationsPanel = new Panel();
        Label lblLocations = new Label();
        ComboBox cboLocations = new ComboBox();

        // Serial Numbers
        Panel SerialNumbersPanel = new Panel();
        Label lblSerialNumbers = new Label();
        ComboBox cboSerialNumbers = new ComboBox();

        //Charities US2715
        ComboBox cboCharities = new ComboBox();

        // US2744
        ComboBox cboPrograms = new ComboBox();
        ComboBox cboPosMenus = new ComboBox();

        // US1902: Product Groups
        ComboBox cboProductGroups = new ComboBox();

        // US1839: Device, not inventory item, serial number
        TextBox txtSerialNbrDevice = new TextBox();

        //US1808
        CheckBox cbByPackage = new CheckBox();

        ComboBox cboComp = new ComboBox();

        // US5486 Is Active
        Panel statusPanel = new Panel();
        ComboBox cboStatus = new ComboBox();
        Label labelStatus = new Label();

        #endregion Controls

        #region Constructors and Form Events

        public FrmReportManager()
        {
            InitializeComponent();
            lblStaff.Text = "Staff: ";
            fieldPerson.Text = "All";
        }

        private void ReportManager_Load(object sender, EventArgs e)
        {
            lblPerson.Text = !Configuration.mMachineAccounts ? Resources.PlayerText : Resources.MachineText;

            if (m_gotReports == null || m_gotReports.Reports.Count == 0)
            {
                GetReportsFromServer(null);
            }

            lblReportType.Text = "Report Type";

            LoadStaffCombo();

            LoadMachineCombo();     //DE6958
            LoadAccrualsCombos();
            LoadPositionsCombo();   // US1850
            LoadProdItemsCombo();   // US1754
            LoadCharityCombo();     // US2715
            LoadProdTypesCombo();
            LoadLocationsCombo();   // us1754
            LoadSerialNumbersCombo();   // us1747
            LoadProdGroupsCombo();   // US1902
            LoadAuditTypeCombo();

            LoadProgramCombo(); //US2744
            LoadPosMenuCombo(); //US2744

            LoadCompCombo();


            startDatePicker.Value = Configuration.GamingDate;
            endDatePicker.Value = Configuration.GamingDate;
            LoadSessionCombo();
            m_playerID = 0;
            m_staffID = 0;
            m_session = 0;

            fieldPerson.Text = "All";
            //START RALLY DE 6001
            if (cboStaff.Items.Count > 0)
            {
                cboStaff.SelectedIndex = 0;
            }
            if (cboSession.Items.Count > 0)
            {
                cboSession.SelectedIndex = 0;
            }
            //END RALLY DE 6001

            m_ByPackage = Configuration.m_byPackage;
            cbByPackage.Checked = (m_ByPackage == 1) ? true : false;
            ShowParameters();
        }

        #endregion Constructors and Form Events

        #region Loaders
        //START RALLY DE6958
        private void LoadMachineCombo()
        {
            cboMachine.Items.Clear();

            cboMachine.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
            GetMachineDataMessage getMachineDataMessage = new GetMachineDataMessage(0);
            getMachineDataMessage.Send();

            foreach (var machineData in getMachineDataMessage.MachineDataList)
            {
                // DE11331 Added support for only displaying these device types in the machine list.
                // 5 - POS
                // 7 - Portable POS
                // 10 - User Defined
                // 13 - POS / Management
                if (machineData.Id == 5 || machineData.Id == 7 || machineData.Id == 10 || machineData.Id == 13)
                {
                    string machineName = string.Format("{0} - {1}", machineData.Id.ToString(), machineData.Description);
                    cboMachine.Items.Add(new System.Web.UI.WebControls.ListItem(machineName, machineData.Id.ToString()));
                }
            }

            if (cboMachine.Items.Count > 0)
            {
                cboMachine.SelectedIndex = 0;
            }
        }
        //END RALLY DE6958

        private void LoadStaffCombo()
        {
            m_objStaff = new FindStaffOrPlayerMessage(false, 0, "", "", "");
            try
            {
                m_objStaff.Send();
            }
            catch (ServerException ex)
            {
                MessageForm.Show(this, Resources.errorLoadData + "," + ex.Message, Resources.report_center);
            }

            if (m_objStaff.FoundPersons == null || m_objStaff.FoundPersons.Count == 0)
            {
                cboStaff.Items.Clear();//RALLY DE 6001
                cboStaff.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString())); //RALLY DE 6001
                return;
            }

            List<Staff> staffList = new List<Staff>();
            foreach (var person in m_objStaff.FoundPersons)
            {
                staffList.Add(new Staff { Name = person.Value, Id = person.Key.ToString() });
            }
            staffList.Sort((x, y) => x.Name.CompareTo(y.Name));

            cboStaff.Items.Clear();

            cboStaff.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
            foreach (var person in staffList)
            {
                cboStaff.Items.Add(new System.Web.UI.WebControls.ListItem(person.Name, person.Id));
            }
            //START RALLY DE 6001
            if (cboStaff.Items.Count > 0)
            {
                cboStaff.SelectedIndex = 0;
            }
            //END RALLY DE 6001
        }

        private void LoadSessionCombo()
        {
            cboSession.Items.Clear();

            if (startDatePicker.Value <= endDatePicker.Value)
            {
                cboSession.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
                foreach (var sess in GetReportSessionList.GetList(startDatePicker.Value, endDatePicker.Value)) // RALLY TA7438
                {
                    cboSession.Items.Add(new System.Web.UI.WebControls.ListItem(sess.ToString(), sess.ToString()));
                }
            }
            else
            {
                m_session = 0;
                cboSession.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
            }
            //START RALLY DE 6001
            if (cboSession.Items.Count > 0)
            {
                cboSession.SelectedIndex = 0;
            }
            //END RALLY DE 6001
        }

        // US1850: allow new cash activity report to show positions
        private void LoadPositionsCombo()
        {
            cboPositions.Items.Clear();
            cboPositions.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));

            foreach (PositionType position in GetPositionList.GetList(Configuration.operatorID))
            {
                cboPositions.Items.Add(new System.Web.UI.WebControls.ListItem(position.PositionName, position.PositionID.ToString()));
            }

            if (cboPositions.Items.Count > 0)
                cboPositions.SelectedIndex = 0;
        }

        // US1754: Inv Phys Count requires product types param
        private void LoadProdItemsCombo()
        {
            cboProdItems.Items.Clear();
            cboProdItems.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
            foreach (ProductItemData prodItem in GetProductItemList.GetList())    // DE8678
            {
                cboProdItems.Items.Add(new System.Web.UI.WebControls.ListItem(prodItem.ProductItemName, prodItem.ProductItemID.ToString()));
            }

            if (cboProdItems.Items.Count > 0)
                cboProdItems.SelectedIndex = 0;
        }

        // US2715 Adding support for loading charities
        private void LoadCharityCombo()
        {
            cboCharities.Items.Clear();
            cboCharities.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
            foreach (Charity charity in GetCharityDataMessage.GetList())
            {
                cboCharities.Items.Add(new System.Web.UI.WebControls.ListItem(charity.Name, charity.CharityId.ToString()));
            }

            if (cboCharities.Items.Count > 0)
                cboCharities.SelectedIndex = 0;
        }

        //US2744
        private void LoadProgramCombo()
        {
            cboPrograms.Items.Clear();
            foreach (Data.ProgramType program in GetProgramList.GetList())
            {
                cboPrograms.Items.Add(new System.Web.UI.WebControls.ListItem(program.ProgramName, program.ProgramId.ToString()));
            }

            if (cboPrograms.Items.Count > 0)
                cboPrograms.SelectedIndex = 0;
        }

        //US2744
        private void LoadPosMenuCombo()
        {
            cboPosMenus.Items.Clear();
            foreach (PosMenuType menu in GetMenuList.GetList())
            {
                cboPosMenus.Items.Add(new System.Web.UI.WebControls.ListItem(menu.MenuName, menu.MenuId.ToString()));
            }

            if (cboPosMenus.Items.Count > 0)
                cboPosMenus.SelectedIndex = 0;
        }

        private void LoadProdTypesCombo()
        {
            cboProdTypes.Items.Clear();
            cboProdTypes.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
            foreach (ProductTypes prodType in GetProductTypesList.GetList(Configuration.operatorID))
            {
                cboProdTypes.Items.Add(new System.Web.UI.WebControls.ListItem(prodType.ProdTypeName, prodType.ProdTypeID.ToString()));
            }

            if (cboProdTypes.Items.Count > 0)
                cboProdTypes.SelectedIndex = 0;
        }

        // US1902: Door Sales Reports now offer product group param
        private void LoadProdGroupsCombo()
        {
            cboProductGroups.Items.Clear();
            cboProductGroups.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
            foreach (ProductGroupData prodGroup in GetProductGroupsList.GetList(Configuration.operatorID))
            {
                if (prodGroup.IsActive == true)
                    cboProductGroups.Items.Add(new System.Web.UI.WebControls.ListItem(prodGroup.ProductGroupName, prodGroup.ProductGroupID.ToString()));
            }

            if (cboProductGroups.Items.Count > 0)
                cboProductGroups.SelectedIndex = 0;
        }

        private void LoadAuditTypeCombo()
        {
            comboAuditType.Items.Clear();
            comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
            List<AuditType> auditTypeList = GetAuditTypes.GetList();
            if (auditTypeList != null)
            {
                foreach (AuditType auditType in auditTypeList)
                {
                    comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem(auditType.Description, auditType.Type.ToString()));
                }
            }
            //comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem("Accrual changes", 1.ToString()));
            //comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem("Inventory changes", 2.ToString()));
            //comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem("Payouts", 3.ToString()));
            //comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem("Staff changes", 4.ToString()));
            //comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem("Staff login changes", 5.ToString()));
            //comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem("Accrual setting changes", 6.ToString()));
            //comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem("System setting changes", 7.ToString()));
            //comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem("Points structure changes", 8.ToString()));
            //comboAuditType.Items.Add(new System.Web.UI.WebControls.ListItem("Accounts and permission changes", 9.ToString()));

            if (comboAuditType.Items.Count > 0)
            {
                comboAuditType.SelectedIndex = 0;
            }
        }

        private void LoadLocationsCombo()
        {
            cboLocations.Items.Clear();
            cboLocations.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));

            // DE8678 - Message should gets it by location type not operator.
            foreach (LocationType type in GetLocationList.GetList((int)LocationTypes.Physical))
            {
                cboLocations.Items.Add(new System.Web.UI.WebControls.ListItem(type.LocationName, type.LocationID.ToString()));
            }

            if (cboLocations.Items.Count > 0)
                cboLocations.SelectedIndex = 0;
        }

        // US1747: Inv Skips allow serial number queries
        private void LoadSerialNumbersCombo()
        {
            cboSerialNumbers.Items.Clear();
            cboSerialNumbers.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));

            foreach (SerialNumbersType nbrs in GetSerialNumbers.GetList(Configuration.operatorID))
            {
                cboSerialNumbers.Items.Add(new System.Web.UI.WebControls.ListItem(nbrs.SerialNumber));
            }

            if (cboSerialNumbers.Items.Count > 0)
                cboSerialNumbers.SelectedIndex = 0;
        }

        private void LoadCompCombo()
        {
            cboComp.Items.Clear();
            cboComp.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
            GetCompList gcl = new GetCompList();

            List<int> comparer = new List<int>();
            if (m_playerID == 0)//Load all compID
            {
                foreach (CompList cl in gcl.getList())
                {
                    if (!comparer.Contains(cl.CompId))
                    {
                        cboComp.Items.Add(new System.Web.UI.WebControls.ListItem(cl.CompName, cl.CompId.ToString()));
                        comparer.Add(cl.CompId);
                    }
                }
            }//Load compID per player
            else
            {
                foreach (CompList cl in gcl.getList().Where(p => p.PlayerID == m_playerID))
                {
                    if (!comparer.Contains(cl.CompId))
                    {
                        cboComp.Items.Add(new System.Web.UI.WebControls.ListItem(cl.CompName, cl.CompId.ToString()));
                        comparer.Add(cl.CompId);
                    }
                }
            }

            comparer.Clear();

            if (cboComp.Items.Count > 0)
            {
                cboComp.SelectedIndex = 0;
            }

        }


        internal void LoadUserReportToTreeView(UserReportType reportType)
        {
            // US1622 ?
            lblReportType.Text = "";
            groupBox1.Text = "";

            reportTreeView.Nodes.Clear();
            if (reportType == null || reportType.UserReportGroups == null || reportType.UserReportGroups.Count == 0)
            {
                return;
            }
            //load user Report to tree view
            UserReportGroupTreeNode tempGroup;

            foreach (KeyValuePair<int, UserReportGroup> group in reportType.UserReportGroups)
            {
                tempGroup = new UserReportGroupTreeNode(group.Value.UserGroupName, false);
                foreach (ReportInfo rpt in group.Value.ReportsArray)
                {
                    ReportTreeNode tempReport = new ReportTreeNode(rpt, false);
                    tempGroup.Nodes.Add(tempReport);
                }
                reportTreeView.Nodes.Add(tempGroup);
            }

            reportTreeView.ExpandAll();

            //US1622?
            lblReportType.Text = reportType.UserReportTypeName;
            groupBox1.Text = reportType.UserReportTypeName;

            if (reportTreeView.Nodes.Count > 0)
            {
                reportTreeView.TopNode = reportTreeView.Nodes[0];
            }
        }

        internal void LoadReportToTreeView(ReportTypes typeID)
        {
            reportTreeView.Nodes.Clear();
            if (m_gotReports == null)
            {
                return;
            }
            foreach (KeyValuePair<int, ReportInfo> report in m_gotReports.Reports)
            {
                if (report.Value.TypeID == (int)typeID)
                {
                    ReportTreeNode tempReportNode = new ReportTreeNode(report.Value, false);
                    reportTreeView.Nodes.Add(tempReportNode);
                }
            }

            if (reportTreeView.Nodes.Count > 0)
            {
                reportTreeView.TopNode = reportTreeView.Nodes[0];
            }
        }

        internal void LoadPredefinedReports()
        {
            Dictionary<int, string> rpts = new Dictionary<int, string>();
            reportTreeView.Nodes.Clear(); //clear up 

            m_reportsDictionary = ((frmReportCenterMDIParent)MdiParent).ReportsDictionary;

            //US1831: allow ALL parameters to be seen
            //rpts.Add(0, "All Reports");

            foreach (ReportInfo rpt in m_reportsDictionary.Values)
            {
                if (!rpts.ContainsKey(rpt.TypeID))
                {
                    string strRptGrp;
                    switch (rpt.TypeID)
                    {
                        case 1:
                            strRptGrp = "Sales";
                            break;
                        case 2:
                            strRptGrp = "Paper";
                            break;
                        case 3:
                            strRptGrp = Configuration.mMachineAccounts ?
                                Resources.reportTypePlayersMachine :
                                Resources.reportTypePlayers;
                            break;
                        case 4:
                            strRptGrp = "Misc";
                            break;
                        case 5:
                            strRptGrp = "Staff";
                            break;
                        case 6:
                            strRptGrp = "POS";
                            break;
                        case 7:
                            strRptGrp = "Bingo";
                            break;
                        case 8:
                            strRptGrp = "Electronics";
                            break;
                        case 9:
                            strRptGrp = "Exceptions";
                            break;
                        case 10:
                            strRptGrp = "Custom";
                            break;
                        case 11:
                            strRptGrp = "Tax Forms";
                            break;
                        case 12:
                            strRptGrp = "Gaming";
                            break;
                        // Rally US1492
                        case 13:
                            strRptGrp = "Inventory";
                            break;
                        // US1622: new Accrual Reports
                        case 14:
                            strRptGrp = "Progressives";
                            break;
                        case 15:
                            strRptGrp = "Payouts";
                            break;
                        case 16:
                            strRptGrp = "Texas";
                            break;
                        case 17:
                            strRptGrp = "Coupon";
                            break;
                        default:
                            strRptGrp = "";
                            break;
                    }
                    if (strRptGrp != "")
                    {
                        rpts.Add(rpt.TypeID, strRptGrp);
                    }
                }
            }

            int ctr = 0;
            //RALLY DE 4505
            const int bingoCardSalesDetailReport = 16;
            const int bingoCardSummaryReport = 24;
            //END RALLY DE 4505
            foreach (int c in rpts.Keys)
            {
                UserReportGroupTreeNode urgNode = new UserReportGroupTreeNode(rpts[c], false);

                reportTreeView.Nodes.Add(urgNode);//knc1
                ctr++;

                foreach (ReportInfo rpt in m_reportsDictionary.Values)
                {
                    if (rpt.TypeID == c)
                    {
                        // DE4505 - Do NOT show bingoCard reports if PWP is on.
                        if ((rpt.ID == bingoCardSalesDetailReport || rpt.ID == bingoCardSummaryReport)
                            && Configuration.m_playWithPaper && rpt.TypeID == (int)ReportTypes.Sales)
                            continue;

                        // Do NOT load any of the Texas reports if TX Payouts are not enabled
                        if (rpt.TypeID == (int)ReportTypes.Texas && !Configuration.m_txPayoutsEnabled)
                            continue;

                        ReportTreeNode rtn = new ReportTreeNode(rpt, false);
                        reportTreeView.Nodes[ctr - 1].Nodes.Add(rtn);
                    }
                }
            }

            lblReportType.Text = "Standard Reports";
            groupBox1.Text = "Standard Reports";
            reportTreeView.ExpandAll();

            if (reportTreeView.Nodes.Count > 0)
            {
                reportTreeView.TopNode = reportTreeView.Nodes[0];
            }
        }

        private void LoadAccrualsCombos()
        {
            if (Configuration.m_accrualsEnabled)
            {
                m_AccrualsMessage = new GetAccuralMessage(0, true, true);
                try
                {
                    m_AccrualsMessage.Send();
                }
                catch (ServerException ex)
                {
                    MessageForm.Show(this, Resources.errorLoadData + "," + ex.Message, Resources.report_center);
                }
            }

            // Use enum since we have no msg
            // Order established by data on gtiserverlatin
            cboAccType.Items.Clear();
            cboAccType.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, "%"));
            cboAccType.Items.Add(new System.Web.UI.WebControls.ListItem("Session", AccrualTypes.Session.ToString()));
            cboAccType.Items.Add(new System.Web.UI.WebControls.ListItem("Daily", AccrualTypes.Session.ToString()));
            // DE8635: DO NOT SHOW THIS ITEM! cboAccType.Items.Add(new System.Web.UI.WebControls.ListItem("Instant", AccrualTypes.Session.ToString()));
            cboAccType.Items.Add(new System.Web.UI.WebControls.ListItem("Manual", AccrualTypes.Session.ToString()));

            cboStatus.Items.Clear();
            cboStatus.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, "-1"));    // Wild card set by stored procedure
            cboStatus.Items.Add(new System.Web.UI.WebControls.ListItem("Active", 1.ToString()));
            cboStatus.Items.Add(new System.Web.UI.WebControls.ListItem("InActive", 0.ToString()));


            if (!Configuration.m_accrualsEnabled || m_AccrualsMessage.AccuralList.Count <= 0)
            {
                cboAccName.Items.Clear();
                cboAccName.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, 0.ToString()));
                return;
            }

            List<Accrual> accrualList = new List<Accrual>();
            foreach (var accrual in m_AccrualsMessage.AccuralList)
            {
                //staffList.Add(new Staff { Name = person.Value, Id = person.Key.ToString() });
                accrualList.Add(new Accrual { Id = accrual.Id, Name = accrual.Name, Type = accrual.Type });
            }
            accrualList.Sort((x, y) => x.Name.CompareTo(y.Name));
            cboAccName.Items.Clear();
            cboAccName.Items.Add(new System.Web.UI.WebControls.ListItem(Resources.infoAll, "%"));
            foreach (var acc in accrualList)
            {
                cboAccName.Items.Add(new System.Web.UI.WebControls.ListItem(acc.Name, acc.Id.ToString()));
            }

            if (cboAccName.Items.Count > 0)
            {
                cboAccName.SelectedIndex = 0;
            }
            if (cboAccType.Items.Count > 0)
            {
                cboAccType.SelectedIndex = 0;
            }
            if (cboStatus.Items.Count > 0)
            {
                cboStatus.SelectedIndex = 0;
            }
        }

        #endregion Loaders

        #region Events
        // TreeView Events
        private void reportTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                e.Node.Checked = !e.Node.Checked;
            }
            else
            {
                bool hold = !e.Node.Checked;
                ResetTreeViewFont();
                e.Node.Checked = hold;
            }
            ShowParameters();
            ValidateDates();
        }
        private void reportTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }
        private void ReportTreeViewAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node is UserReportGroupTreeNode)
            {
                foreach (ReportTreeNode node in e.Node.Nodes)
                    node.Checked = e.Node.Checked;
            }
            e.Node.ForeColor = e.Node.Checked ? Color.Red : Color.Black;
        }

        // ComboBox Events
        private void cboStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selPerson = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selPerson.Value, out m_staffID)) m_staffID = 0;
        }

        //START RALLY DE 6958
        private void cboMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = sender as ComboBox;
            System.Web.UI.WebControls.ListItem selectedMachine = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedMachine.Value, out m_machineId)) m_machineId = 0;
        }
        //END RALLY DE 6958
        private void cboSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selSession = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selSession.Value, out m_session)) m_session = 0;
        }
        private void cboSession_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(cboSession.Text, out m_session)) m_session = 0;
        }
        private void cboSession_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!int.TryParse(cboSession.Text, out m_session)) m_session = 0;
        }

        // US1622: the reporting sp's use strings not int params here.  Pass "%" as wildcard
        private void cboAccName_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_AccrualsName = cboAccName.Text == "All" ? "%" : cboAccName.Text;
        }
        private void cboAccType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_AccrualsType = cboAccType.Text == "All" ? "%" : cboAccType.Text;
        }

        // This one uses -1=all, 0=inactive, 1=active
        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedStatus = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedStatus.Value, out m_activeStatus)) m_activeStatus = 0;
        }

        private void cboPositions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out m_PositionID)) m_PositionID = 0;
        }

        private void cbocomp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out  m_CompId)) m_CompId = 0;
        }

        private void cboProdItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out m_ProductItemID)) m_ProductItemID = 0;
        }

        private void cboCharity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out m_CharityId)) m_CharityId = 0;
        }

        private void cboProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out m_ProgramId)) m_ProgramId = 0;
        }

        private void cboPosMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out m_PosMenuId)) m_PosMenuId = 0;
        }

        private void cboProdTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out m_ProductTypeId)) m_ProductTypeId = 0;
        }

        void cboProductGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out m_ProductGroupID)) m_ProductGroupID = 0;
        }

        private void cboLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out m_LocationID)) m_LocationID = 0;
        }

        private void cboAuditType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            if (!int.TryParse(selectedItem.Value, out m_auditTypeID)) m_auditTypeID = 0;
        }

        private void cboSerialNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            System.Web.UI.WebControls.ListItem selectedItem = (System.Web.UI.WebControls.ListItem)c.SelectedItem;
            //DE12064 
            //if (!int.TryParse(selectedItem.Value, out m_SerialNo)) m_SerialNo = 0;
            if (string.IsNullOrEmpty(c.Text) || c.Text == "all")
            {
                m_SerialNo = "0";
            }
            else
            {
                m_SerialNo = selectedItem.Value;
            }
        }
        void txtSerialNbrDevice_TextChanged(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (string.IsNullOrEmpty(box.Text) || box.Text.ToLower() == "all")
                m_SerialNbrDevice = "%";
            else
                m_SerialNbrDevice = box.Text;
        }


        // Button Events
        private void btnPreview_Click(object sender, EventArgs e)
        {
            DoReporting(false);
            Parent.BringToFront();
            BringToFront();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            DoReporting(true);
        }
        private void btnFindPlayer_Click(object sender, EventArgs e)
        {
            // PDTS 1064 - Portable POS Card Swipe.
            // Create a mag. card reader for the player form only.
            MagneticCardReader magCardReader = new MagneticCardReader(Configuration.mMagCardReaderSettings);
            // Rally DE130 - Mag card number needs to be typed into the system with a colon.
            magCardReader.BeginReading();

            // This event ONLY fires a Player search now.
            PlayerSearchForm search = new PlayerSearchForm(false, Configuration.operatorID, magCardReader,
                                              Configuration.mMachineAccounts, Configuration.mForceEnglish);

            search.ShowDialog();

            magCardReader.EndReading();

            if (search.DialogResult == DialogResult.OK)
            {
                fieldPerson.Text = search.SelectedPlayer.ToString(!Configuration.mMachineAccounts);
                m_playerID = search.SelectedPlayer.Id;
            }
            else
            {
                fieldPerson.Text = "All";
                m_playerID = 0;

                if (search.DialogResult == DialogResult.Abort)
                {
                    MessageForm.Show(this, Resources.errorLoadData, Resources.report_center);
                }
            }

            if (showComp)
            {
                LoadCompCombo();
            }
        }
        void cbByPackage_CheckedChanged(object sender, EventArgs e)
        {
            m_ByPackage = (cbByPackage.Checked == true) ? 1 : 0;
        }
        #endregion Events

        #region Workers
        private void ResetTreeViewFont()
        {
            foreach (TreeNode node in reportTreeView.Nodes)
            {
                node.Checked = false;
                foreach (TreeNode subNode in node.Nodes)
                {
                    subNode.Checked = false;
                }
            }
        }
        private bool AreThereCheckedNodes()
        {
            foreach (TreeNode node in reportTreeView.Nodes)
            {
                foreach (TreeNode subNode in node.Nodes)
                {
                    if (subNode.Checked)
                        return true;
                }
            }
            return false;
        }
        private bool ValidateDates()
        {
            DateTime dtStartDateTime = startDatePicker.Value;
            DateTime dtEndDateTime = endDatePicker.Value;

            if (cbTime.Checked)
            {
                dtStartDateTime = DateTime.Parse(startDatePicker.Text + " " + startTimePicker.Text);
                dtEndDateTime = DateTime.Parse(endDatePicker.Text + " " + endTimePicker.Text);
            }

            bool isOk = !DatesPanel.Contains(startDatePicker) || !DatesPanel.Contains(endDatePicker) || dtStartDateTime <= dtEndDateTime;

            lblErrorMessage.Text = isOk ? String.Empty : "Start Date/Time must be earlier than End Date/Time.";

            btnPrint.Enabled = btnPreview.Enabled = isOk && AreThereCheckedNodes();
            return isOk;
        }


        private SplashScreen msplashScreen = new SplashScreen();

        internal bool GetReportsFromServer(SplashScreen splashScreen)//knc5
        {
            m_gotReports = Configuration.mForceEnglish ?
                new GetReportListExMessage(ReportTypes.All, "en-US") :
                new GetReportListExMessage(ReportTypes.All, Thread.CurrentThread.CurrentCulture.Name);


            msplashScreen = splashScreen;
            try
            {
                m_gotReports.m_raffledisplaytextsetting = RaffleSettingDisplayText.valuerf;
                m_gotReports.Send();

                // FIX : TA5952 Replace "Server Error 1" exception with msgbox indicating missing reports.
                // see if server actually has the report
                List<int> keys = (from report in m_gotReports.Reports
                                  where string.IsNullOrEmpty(report.Value.FileName)
                                  select report.Key).ToList();
                foreach (int key in keys)
                {
                    ReportInfo info = m_gotReports.Reports[key];
                    if (string.IsNullOrEmpty(info.FileName))
                    {
                        string msg = string.Format(Resources.reportIsMissing, info.DisplayName);
                        MessageForm.Show(this, msg, Resources.report_center);
                    }
                    m_gotReports.Reports.Remove(key);
                }
                // END : TA5952
                // Try to cache the reports to the local computer.
                Business.ReportCenter.CacheReports(splashScreen, m_gotReports.Reports);
            }
            catch (ServerException ex)
            {
                MessageForm.Show(this, ex.Message, Resources.report_center);
                return false;
            }

            return true;
        }


        public bool RefreshReport()
        {
            m_gotReports = Configuration.mForceEnglish ?
                new GetReportListExMessage(ReportTypes.All, "en-US") :
                new GetReportListExMessage(ReportTypes.All, Thread.CurrentThread.CurrentCulture.Name);


            try
            {
                m_gotReports.m_raffledisplaytextsetting = RaffleSettingDisplayText.valuerf;
                m_gotReports.Send();

                // FIX : TA5952 Replace "Server Error 1" exception with msgbox indicating missing reports.
                // see if server actually has the report
                List<int> keys = (from report in m_gotReports.Reports
                                  where string.IsNullOrEmpty(report.Value.FileName)
                                  select report.Key).ToList();
                foreach (int key in keys)
                {
                    ReportInfo info = m_gotReports.Reports[key];
                    if (string.IsNullOrEmpty(info.FileName))
                    {
                        string msg = string.Format(Resources.reportIsMissing, info.DisplayName);
                        MessageForm.Show(this, msg, Resources.report_center);
                    }
                    m_gotReports.Reports.Remove(key);
                }
                // END : TA5952
                // Try to cache the reports to the local computer.
                Business.ReportCenter.CacheReports(msplashScreen, m_gotReports.Reports);
            }
            catch (ServerException ex)
            {
                MessageForm.Show(this, ex.Message, Resources.report_center);
                return false;
            }

            return true;
        }


        private string GetPersonName(int intID)
        {
            FindStaffOrPlayerMessage foundPersons = new FindStaffOrPlayerMessage(true, 0, "", "", "");

            try
            {
                foundPersons.Send();
            }
            catch (Exception ex)
            {
                Utilities.Log("GetPersonName() finding person error....Exception:" + ex.Message, LoggerLevel.Severe);
                Utilities.Log("exception stack:" + ex.StackTrace, LoggerLevel.Severe);
                MessageForm.Show(this, Resources.errorLoadData + "," + ex.Message, Resources.report_center);
            }

            if (foundPersons.FoundPersons.Count > 0)
            {
                foreach (var person in foundPersons.FoundPersons)
                {
                    if (person.Key == intID)
                    {
                        return person.Value;
                    }
                }
            }
            return "All";
        }
        #endregion Workers

        #region Dynamic Control Loading

        bool showStartDate = false;
        bool showEndDate = false;
        bool showSession = false;
        bool showStaff = false;
        bool showProductItems = false;
        bool showCharities = false; // US2715
        bool showPlayer = false;
        bool showGamingDate = false;
        bool showQuarter = false;
        bool showYear = false;
        bool showMachine = false; //RALLY DE 6958
        // US1831
        bool showAccrualAccount = false;
        bool showComp = false;
        bool showCompRaffle = false;
        bool showCustomField = false;
        bool showDevice = false;
        bool showDiscount = false;
        bool showEvent = false;
        bool showGameCategory = false;
        bool showGameType = false;
        bool showGiftCard = false;
        bool showMerchandiseGroup = false;
        bool showMerchandise = false;
        bool showPaperTemplate = false;
        bool showPlayerList = false;
        bool showPlayerRaffle = false;
        bool showPrizeCheck = false;
        bool showProgram = false;
        bool showProductType = false;
        bool showPulltab = false;
        bool showSlotGames = false;
        bool showReceipt = false;
        bool showPOSMenu = false;
        bool showSerialNumber = false;
        bool showInvoiceNumber = false;
        bool showFormNumber = false;
        bool showUp = false;
        bool showCardCut = false;
        bool showRetiredOnly = false;
        bool showInPlayOnly = false;
        bool showAccrualName = false;
        bool showVendor = false;
        bool showTabName = false;
        bool showManufacturer = false;
        bool showInventoryItem = false;
        bool showAccrualStatus = false;
        bool showAccrualType = false;
        // END US1831
        bool showPositions = false;         // us1830
        bool showLocations = false;         // us1754
        bool showProductGroup = false;      // us1902
        bool showSerialNbrDevice = false;   // us1839
        bool showAuditType = false;
        bool showByPackage = false; //US1808
        bool showIsActive = false;
        List<bool> _showParamList = null;

        private void InitParamList()
        {
            _showParamList = new List<bool>();
            for (int i = 0; i < PARAMCOUNT; i++)
            {
                _showParamList.Add(false);
            }

            showStartDate = false;
            showEndDate = false;
            showSession = false;
            showStaff = false;
            showProductItems = false;
            showCharities = false; // US2715
            showPlayer = false;
            showGamingDate = false;
            showQuarter = false;
            showYear = false;
            showMachine = false;

            showAccrualAccount = false;
            showComp = false;
            showCompRaffle = false;
            showCustomField = false;
            showDevice = false;
            showDiscount = false;
            showEvent = false;
            showGameCategory = false;
            showGameType = false;
            showGiftCard = false;
            showMerchandiseGroup = false;
            showMerchandise = false;
            showPaperTemplate = false;
            showPlayerList = false;
            showPlayerRaffle = false;
            showPrizeCheck = false;
            showProgram = false;
            showProductType = false;
            showPulltab = false;
            showSlotGames = false;
            showReceipt = false;
            showPOSMenu = false;
            showSerialNumber = false;
            showInvoiceNumber = false;
            showFormNumber = false;
            showUp = false;
            showCardCut = false;
            showRetiredOnly = false;
            showInPlayOnly = false;
            showAccrualName = false;
            showVendor = false;
            showTabName = false;
            showManufacturer = false;
            showInventoryItem = false;
            showAccrualStatus = false;
            showAccrualType = false;
            showPositions = false;
            showLocations = false;
            showProductGroup = false;
            showSerialNbrDevice = false;
            showAuditType = false;
            showByPackage = false;
            showIsActive = false;
            // Unwire the event handlers
            try
            {
                btnFindPlayer.Click -= new System.EventHandler(btnFindPlayer_Click);
            }
            catch (Exception ex)
            {
                MessageBox.Show("InitParamList FAILED to unwire event handlers: " + ex.Message);
                //throw;
            }
        }

        private void ShowParameters()
        {
            // Avoid screen flashing
            LockWindowUpdate(this.Handle);

            // When user deselects a node, remove all controls from screen...
            InitParamList();

            // cleanup first and get rid of scroll bars if they exist
            tablePanel.Controls.Clear();
            tablePanel.AutoSize = false;
            tablePanel.AutoScroll = false;

            tablePanel.BackColor = Color.Transparent;
            tablePanel.AutoScroll = true;
            tablePanel.AutoSize = false;
            tablePanel.ColumnCount = 1;
            tablePanel.RowCount = PARAMCOUNT;
            _lastRow = 0;

            tablePanel.Size = new System.Drawing.Size(542, 526);
            tablePanel.Dock = DockStyle.Fill;

            foreach (TreeNode node in reportTreeView.Nodes)
            {
                if (node.Checked && node.Text == "All Reports")
                {
                    showStartDate = true;
                    showEndDate = true;
                    showSession = true;
                    showStaff = true;
                    showProductItems = true;
                    showCharities = true; //US2715
                    showPlayer = true;
                    showGamingDate = true;
                    showQuarter = true;
                    showYear = true;
                    showMachine = true; //RALLY DE 6958
                    // US1831
                    showAccrualAccount = true;
                    showComp = true;
                    showCompRaffle = true;
                    showCustomField = true;
                    showDevice = true;
                    showDiscount = true;
                    showEvent = true;
                    showGameCategory = true;
                    showGameType = true;
                    showGiftCard = true;
                    showMerchandiseGroup = true;
                    showMerchandise = true;
                    showPaperTemplate = true;
                    showPlayerList = true;
                    showPlayerRaffle = true;
                    showPrizeCheck = true;
                    showProgram = true;
                    showProductType = true;
                    showPulltab = true;
                    showSlotGames = true;
                    showReceipt = true;
                    showPOSMenu = true;
                    showSerialNumber = true;
                    showInvoiceNumber = true;
                    showFormNumber = true;
                    showUp = true;
                    showCardCut = true;
                    showRetiredOnly = true;
                    showInPlayOnly = true;
                    showAccrualName = true;
                    showVendor = true;
                    showTabName = true;
                    showManufacturer = true;
                    showInventoryItem = true;
                    showAccrualStatus = true;
                    showAccrualType = true;
                    showPositions = true;
                    showLocations = true;
                    showProductGroup = true;
                    showSerialNbrDevice = true;
                    showAuditType = true;
                    showByPackage = true;
                    showIsActive = true;
                    break;
                }


                foreach (ReportTreeNode subNode in node.Nodes)
                {
                    if (subNode.Checked)
                    {
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.IsActive))
                            showIsActive = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.StartDate))
                            showStartDate = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.EndDate))
                            showEndDate = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.Session))
                            showSession = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.StaffID))
                            showStaff = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.ProductItemID))
                            showProductItems = true;
                        //start US2715
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.CharityId))
                            showCharities = true;
                        //end US2715
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.PlayerID))
                            showPlayer = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.GamingDate))
                            showGamingDate = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.Month))
                            showQuarter = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.Year))
                            showYear = true;
                        //START RALLY DE 6958   
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.MachineID))
                            showMachine = true;
                        //END RALLY DE 6958

                        // US1831: SHOW ALL PARAMS
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.AccrualAccountID))
                            showAccrualAccount = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.CompID))
                            showComp = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.CompRaffleID))
                            showCompRaffle = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.CustomFieldID))
                            showCustomField = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.DeviceID))
                            showDevice = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.DiscountID))
                            showDiscount = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.EventID))
                            showEvent = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.GameCategoryID))
                            showGameCategory = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.GameTypeID))
                            showGameType = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.GiftCardID))
                            showGiftCard = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.MerchandiseGroupID))
                            showMerchandiseGroup = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.MerchandiseID))
                            showMerchandise = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.PaperTemplateID))
                            showPaperTemplate = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.PlayerListID))
                            showPlayerList = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.PlayerRaffleID))
                            showPlayerRaffle = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.PrizeCheckID))
                            showPrizeCheck = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.ProgramID))
                            showProgram = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.ProductTypeID))
                            showProductType = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.PulltabID))
                            showPulltab = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.SlotGamesID))
                            showSlotGames = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.ReceiptID))
                            showReceipt = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.POSMenu))
                            showPOSMenu = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.SerialNumber))
                            showSerialNumber = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.InvoiceNumber))
                            showInvoiceNumber = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.FormNumber))
                            showFormNumber = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.Up))
                            showUp = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.CardCutID))
                            showCardCut = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.RetiredOnly))
                            showRetiredOnly = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.InPlayOnly))
                            showInPlayOnly = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.AccrualsName))
                            showAccrualName = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.VendorID))
                            showVendor = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.TabName))
                            showTabName = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.ManufacturerID))
                            showManufacturer = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.InventoryItemID))
                            showInventoryItem = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.AccrualsType))
                            showAccrualType = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.AccrualsStatus))
                            showAccrualStatus = true;
                        // END US1831

                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.PositionID)) // US1830
                            showPositions = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.InvLocationID)) // US1754
                            showLocations = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.ProductGroupID)) // US1902
                            showProductGroup = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.SerialNbrDevice)) // US1839
                            showSerialNbrDevice = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.AuditLogType))
                            showAuditType = true;
                        if (subNode.ReportInfo.Parameters.ContainsKey((int)ReportParamIDs.ByPackage))
                            showByPackage = true;
                    }
                }
            }

            // Now add new widgets 
            if (showStartDate || showEndDate)
            {
                ShowDatesPanel(showStartDate, showEndDate);
            }

            if (showGamingDate)
            {
                ShowTimesPanel(showGamingDate);
            }

            if (showMachine || showSession || showStaff || showPlayer)
            {
                ShowMachinePanel(showMachine, showSession, showStaff, showPlayer);
            }

            if (showQuarter || showYear)
            {
                ShowQuartersPanel(showQuarter, showYear);
            }

            if (showAccrualAccount || showAccrualStatus || showAccrualType || (showAccrualName && showSession))
            {
                ShowAccrualsPanel(showAccrualAccount, showAccrualName, showAccrualStatus, showAccrualType, showSession);
            }

            // Handle displaying new parameters more dynamically!
            DisplayParamsManager();

            // Finally, allow newly added controls to be seen!
            this.Refresh();                     //Forces a synchronous redraw of all controls
            LockWindowUpdate(IntPtr.Zero);
        }

        void DisplayParamsManager()
        {
            if (showCardCut) DisplayParams("Card Cut:");
            //if (showComp) DisplayParams("Comp:");
            if (showCompRaffle) DisplayParams("Comp Raffle:");
            if (showCustomField) DisplayParams("Custom:");
            if (showDevice) DisplayParams("Device:");
            if (showDiscount) DisplayParams("Discount:");
            if (showEvent) DisplayParams("Event:");
            if (showGameCategory) DisplayParams("Game Cat:");
            if (showGameType) DisplayParams("Game Type:");

            if (showGiftCard) DisplayParams("Gift Card:");
            if (showMerchandiseGroup) DisplayParams("Merch Grp:");
            if (showMerchandise) DisplayParams("Merchandise:");

            if (showPaperTemplate) DisplayParams("Paper Template:");
            if (showPlayerRaffle) DisplayParams("Player Raffle:");
            if (showPrizeCheck) DisplayParams("Prize Check:");
            //if (showProgram) DisplayParams("Program:");
            if (showPulltab) DisplayParams("Pulltab:");
            if (showSlotGames) DisplayParams("Slot Game:");
            if (showReceipt) DisplayParams("Receipt:");
            //if (showPOSMenu) DisplayParams("POS Menu:");           
            if (showInvoiceNumber) DisplayParams("Invoice No:");
            if (showFormNumber) DisplayParams("Form No:");
            if (showUp) DisplayParams("Up:");
            if (showRetiredOnly) DisplayParams("Retired:");
            if (showInPlayOnly) DisplayParams("In Play:");
            if (showVendor) DisplayParams("Vendor:");
            if (showTabName) DisplayParams("Tab Name:");
            if (showManufacturer) DisplayParams("Manuf:");
            if (showInventoryItem) DisplayParams("Inv Item:");
            //if (showByPackage) DisplayParams("ByPackage:");

            if (showComp)
            {
                DisplayParams("Comp:", cboComp);
                cboComp.SelectedIndexChanged += cbocomp_SelectedIndexChanged;
            }

            if (showPositions)
            {
                DisplayParams("Positions:", cboPositions);
                cboPositions.SelectedIndexChanged += cboPositions_SelectedIndexChanged;
            }

            if (showProductItems)
            {
                DisplayParams("Product:", cboProdItems);
                cboProdItems.SelectedIndexChanged += cboProdItems_SelectedIndexChanged;
            }

            //US2715
            if (showCharities)
            {
                DisplayParams("Charities:", cboCharities);
                cboCharities.SelectedIndexChanged += cboCharity_SelectedIndexChanged;
            }

            //US2744
            if (showProgram)
            {
                DisplayParams("Program:", cboPrograms);
                cboPrograms.SelectedIndexChanged += cboProgram_SelectedIndexChanged;
            }

            //US2744
            if (showPOSMenu)
            {
                DisplayParams("POS Menu:", cboPosMenus);
                cboPosMenus.SelectedIndexChanged += cboPosMenu_SelectedIndexChanged;
            }

            if (showProductType)
            {
                DisplayParams("Product type:", cboProdTypes);
                cboProdTypes.SelectedIndexChanged += cboProdTypes_SelectedIndexChanged;
            }

            if (showLocations)
            {
                DisplayParams("Location:", cboLocations);
                cboLocations.SelectedIndexChanged += cboLocations_SelectedIndexChanged;
            }

            if (showSerialNumber)
            {
                DisplayParams("Serial No:", cboSerialNumbers);
                cboSerialNumbers.SelectedIndexChanged += cboSerialNumbers_SelectedIndexChanged;
            }

            if (showAuditType)
            {
                DisplayParams("Audit type:", comboAuditType);
                comboAuditType.SelectedIndexChanged += cboAuditType_SelectedIndexChanged;
            }

            if (showProductGroup)
            {
                DisplayParams("Prod Group:", cboProductGroups);  // terse literal to match other labels
                cboProductGroups.SelectedIndexChanged += new EventHandler(cboProductGroups_SelectedIndexChanged);
            }

            if (showSerialNbrDevice)
            {
                m_SerialNbrDevice = "%";
                DisplayParamBox("Device No:", txtSerialNbrDevice, "All");  // terse literal to match other labels
                txtSerialNbrDevice.TextChanged += new EventHandler(txtSerialNbrDevice_TextChanged);
            }
            if (showByPackage)
            {
                DisplayParamsByPackage("ByPackage:", cbByPackage);
            }

            if (showIsActive)
            {
                DisplayParamsIsActive();
            }
        }


        // Attempt to make adding these controls more dynamic
        void DisplayParams(string labelText)
        {
            DisplayParams(labelText, null);
        }

        /// <summary>
        /// Display a text box for user input.  This allows user entry for params which may contain too many options for a combo box.
        /// </summary>
        /// <param name="labelText">Label for this dynamic control.</param>
        /// <param name="box">Control created in ctor.</param>
        /// <param name="defValue">ex: 0 or % or ALL</param>
        void DisplayParamBox(string labelText, TextBox box, string defValue)
        {
            Panel pnl = new Panel();
            pnl.Size = new System.Drawing.Size(474, 39);
            pnl.BackColor = Color.Transparent;

            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Text = labelText;
            lbl.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            lbl.Location = new Point(7, 8);
            lbl.Margin = new Padding(4, 0, 4, 0);
            lbl.Size = new System.Drawing.Size(55, 22);

            pnl.Controls.Add(lbl);
            if (box == null)
            {
                box = new TextBox();
            }
            box.Text = defValue;
            box.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            //box.FormattingEnabled = true;
            box.Location = new System.Drawing.Point(99, 5);
            box.Margin = new Padding(4, 5, 4, 5);
            box.Size = new System.Drawing.Size(339, 30);
            pnl.Controls.Add(box);

            tablePanel.Controls.Add(pnl);
            tablePanel.SetRow(pnl, _lastRow++);   // Place this panel in the last empty row
        }

        void DisplayParams(string labelText, ComboBox cbo)
        {
            Panel pnl = new Panel();
            pnl.Size = new System.Drawing.Size(474, 39);
            pnl.BackColor = Color.Transparent;

            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Text = labelText;
            lbl.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            lbl.Location = new Point(7, 8);
            lbl.Margin = new Padding(4, 0, 4, 0);
            //lbl.Size = new System.Drawing.Size(55, 22);
            lbl.Size = new System.Drawing.Size(85, 22);
            pnl.Controls.Add(lbl);
            if (cbo == null)
            {
                cbo = new ComboBox();
            }
            cbo.AutoCompleteMode = AutoCompleteMode.Append;
            cbo.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbo.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            cbo.FormattingEnabled = true;
            //cbo.Location = new System.Drawing.Point(99, 5);
            cbo.Location = new System.Drawing.Point(129, 5);
            cbo.Margin = new Padding(4, 5, 4, 5);
            //cbo.Size = new System.Drawing.Size(339, 30);
            cbo.Size = new System.Drawing.Size(309, 30);
            if (cbo.Items.Count > 0)
            {
                cbo.SelectedIndex = 0;
            }
            pnl.Controls.Add(cbo);

            tablePanel.Controls.Add(pnl);
            tablePanel.SetRow(pnl, _lastRow++);   // Place this panel in the last empty row
        }
        void DisplayParamsByPackage(string labelText, CheckBox cb)
        {
            Panel pnl = new Panel();
            pnl.Size = new System.Drawing.Size(474, 39);
            pnl.BackColor = Color.Transparent;

            resources.ApplyResources(this.cbByPackage, "cbPackageName");
            cbByPackage.AutoSize = true;
            cbByPackage.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            cbByPackage.ImeMode = ImeMode.NoControl;
            cbByPackage.Location = new System.Drawing.Point(11, 8);
            cbByPackage.Margin = new Padding(4, 5, 4, 5);
            cbByPackage.CheckedChanged += new EventHandler(cbByPackage_CheckedChanged);
            cbByPackage.Name = "cbByPackage";
            cbByPackage.Size = new System.Drawing.Size(120, 26);
            //cbByPackage.TabIndex = 23;
            cbByPackage.Text = "By Package";
            cbByPackage.UseVisualStyleBackColor = true;

            cbByPackage.Visible = true;
            pnl.Controls.Add(cbByPackage);
            tablePanel.Controls.Add(pnl);
            tablePanel.SetRow(pnl, _lastRow++);   // Place this panel in the last empty row
        }

        /// US5486 Changed to a drop-down
        /// <summary>
        /// Add checkbox for Is Active.
        /// </summary>
        /// <param name="cb"></param>
        void DisplayParamsIsActive()
        {
            statusPanel.SuspendLayout();
            statusPanel.Controls.Add(cboStatus);
            statusPanel.Controls.Add(labelStatus);
            statusPanel.Location = new System.Drawing.Point(3, 99);
            statusPanel.Name = "statusPanel";
            statusPanel.Size = new System.Drawing.Size(485, 42);
            statusPanel.BackColor = Color.Transparent;

            cboStatus.AutoCompleteMode = AutoCompleteMode.Append;
            cboStatus.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            cboStatus.FormattingEnabled = true;
            cboStatus.Location = new System.Drawing.Point(100, 5);
            cboStatus.Margin = new Padding(4, 5, 4, 5);
            cboStatus.Name = "cboAccStatus";
            cboStatus.Size = new System.Drawing.Size(339, 30);
            cboStatus.SelectedIndexChanged -= cboStatus_SelectedIndexChanged; // keep one subscription
            cboStatus.SelectedIndexChanged += new System.EventHandler(cboStatus_SelectedIndexChanged);
            if (cboStatus.Items.Count > 0)
            {
                if (cboStatus.Items.Count > 1)
                    cboStatus.SelectedIndex = 1; // US5146 items are added manually. This corrisponds to "Active." Can search for "Active" if this changes, but looks like it'll be static for a long time
                else
                    cboStatus.SelectedIndex = 0;
            }

            labelStatus.AutoSize = true;
            labelStatus.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labelStatus.Location = new System.Drawing.Point(9, 8);
            labelStatus.Name = "labelAccStatus";
            labelStatus.Size = new System.Drawing.Size(130, 22);
            labelStatus.TabIndex = 32;
            labelStatus.Text = "Status:";
            tablePanel.Controls.Add(statusPanel);
            tablePanel.SetRow(statusPanel, _lastRow++);   // Place this panel in the last empty row
            statusPanel.ResumeLayout(false);
            statusPanel.PerformLayout();
        }


        void ShowAccrualsPanel(bool showAccrualAccountID, bool showAccrualName, bool showAccrualStatus, bool showAccrualType, bool showSession)
        {
            AccrualsPanel.SuspendLayout();
            namePanel.SuspendLayout();
            typePanel.SuspendLayout();
            this.SuspendLayout();

            AccrualsPanel.Controls.Clear();
            AccrualsPanel.Name = "Progressive Panel";
            AccrualsPanel.Size = new System.Drawing.Size(490, 193);
            AccrualsPanel.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            AccrualsPanel.BackColor = Color.Transparent;

            if (showAccrualName)
            {
                namePanel.Controls.Add(cboAccName);
                namePanel.Controls.Add(labelAccName);
                namePanel.Location = new System.Drawing.Point(3, 3);
                namePanel.Name = "namePanel";
                namePanel.Size = new System.Drawing.Size(485, 42);
                namePanel.BackColor = Color.Transparent;

                cboAccName.AutoCompleteMode = AutoCompleteMode.Append;
                cboAccName.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboAccName.DropDownStyle = ComboBoxStyle.DropDownList;
                cboAccName.Font = new System.Drawing.Font("Trebuchet MS", 12F);
                cboAccName.FormattingEnabled = true;
                cboAccName.Location = new System.Drawing.Point(158, 5);
                cboAccName.Margin = new Padding(4, 5, 4, 5);
                cboAccName.Name = "cboAccName";
                cboAccName.Size = new System.Drawing.Size(302, 30);
                cboAccName.SelectedIndexChanged += new System.EventHandler(cboAccName_SelectedIndexChanged);
                if (cboAccName.Items.Count > 0)
                {
                    cboAccName.SelectedIndex = 0;
                }

                labelAccName.AutoSize = true;
                labelAccName.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labelAccName.Location = new System.Drawing.Point(9, 8);
                labelAccName.Name = "labelAccName";
                labelAccName.Size = new System.Drawing.Size(130, 22);
                labelAccName.TabIndex = 32;
                labelAccName.Text = "Progressive Name:";
                AccrualsPanel.Controls.Add(namePanel);
            }
            if (showAccrualStatus)
            {
                DisplayParamsIsActive();
            }

            if (showAccrualType)
            {
                typePanel.Controls.Add(cboAccType);
                typePanel.Controls.Add(labelAccType);
                typePanel.Location = new System.Drawing.Point(3, 51);
                typePanel.Name = "typePanel";
                typePanel.Size = new System.Drawing.Size(485, 42);
                typePanel.BackColor = Color.Transparent;

                cboAccType.AutoCompleteMode = AutoCompleteMode.Append;
                cboAccType.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboAccType.DropDownStyle = ComboBoxStyle.DropDownList;
                cboAccType.Font = new System.Drawing.Font("Trebuchet MS", 12F);
                cboAccType.FormattingEnabled = true;
                cboAccType.Location = new System.Drawing.Point(158, 5);
                cboAccType.Margin = new Padding(4, 5, 4, 5);
                cboAccType.Name = "cboAccType";
                cboAccType.Size = new System.Drawing.Size(302, 30);
                cboAccType.SelectedIndexChanged += new System.EventHandler(cboAccType_SelectedIndexChanged);
                if (cboAccType.Items.Count > 0)
                {
                    cboAccType.SelectedIndex = 0;
                }

                labelAccType.AutoSize = true;
                labelAccType.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                labelAccType.Location = new System.Drawing.Point(9, 8);
                labelAccType.Name = "labelAccType";
                labelAccType.Size = new System.Drawing.Size(130, 22);
                labelAccType.Text = "Progressive Type:";

                AccrualsPanel.Controls.Add(typePanel);
            }

            tablePanel.Controls.Add(AccrualsPanel);
            tablePanel.SetRow(AccrualsPanel, _lastRow++);   // Place this panel in the last empty row

            AccrualsPanel.ResumeLayout(false);
            AccrualsPanel.PerformLayout();
            namePanel.ResumeLayout(false);
            namePanel.PerformLayout();
            typePanel.ResumeLayout(false);
            typePanel.PerformLayout();
            this.ResumeLayout(false);
        }

        void ShowDatesPanel(bool showStartDate, bool showEndDate)
        {
            DatesPanel.Controls.Clear();

            // Display the panel containing start and end date
            DatesPanel.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            DatesPanel.Margin = new Padding(4, 5, 4, 5);
            DatesPanel.Name = "DatesPanel";
            DatesPanel.Size = new System.Drawing.Size(490, 48);

            if (showStartDate)
            {
                DatesPanel.Controls.Add(startDatePicker);
                DatesPanel.Controls.Add(lblStartDate);

                lblStartDate.AutoSize = true;
                lblStartDate.BackColor = System.Drawing.Color.Transparent;
                lblStartDate.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblStartDate.ImeMode = ImeMode.NoControl;
                lblStartDate.Location = new System.Drawing.Point(8, 13);
                lblStartDate.Margin = new Padding(4, 0, 4, 0);
                lblStartDate.Name = "lblStartDate";
                lblStartDate.Size = new System.Drawing.Size(88, 22);
                lblStartDate.TabIndex = 2;
                lblStartDate.Text = "Start Date:";

                startDatePicker.Font = new System.Drawing.Font("Trebuchet MS", 12F);
                startDatePicker.Format = DateTimePickerFormat.Short;
                startDatePicker.Location = new System.Drawing.Point(102, 9);
                startDatePicker.Margin = new Padding(4, 5, 4, 5);
                startDatePicker.Name = "startDatePicker";
                startDatePicker.Size = new System.Drawing.Size(124, 26);
                startDatePicker.TabIndex = 3;
                startDatePicker.ValueChanged += new System.EventHandler(DatePicker_ValueChanged);
            }
            if (showEndDate)
            {
                DatesPanel.Controls.Add(endDatePicker);
                DatesPanel.Controls.Add(lblEndDate);

                lblEndDate.AutoSize = true;
                lblEndDate.BackColor = System.Drawing.Color.Transparent;
                lblEndDate.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblEndDate.ImeMode = ImeMode.NoControl;
                lblEndDate.Location = new System.Drawing.Point(234, 13);
                lblEndDate.Margin = new Padding(4, 0, 4, 0);
                lblEndDate.Name = "lblEndDate";
                lblEndDate.Size = new System.Drawing.Size(80, 22);
                lblEndDate.TabIndex = 5;
                lblEndDate.Text = "End Date:";

                endDatePicker.Font = new System.Drawing.Font("Trebuchet MS", 12F);
                endDatePicker.Format = DateTimePickerFormat.Short;
                endDatePicker.Location = new System.Drawing.Point(316, 9);
                endDatePicker.Margin = new Padding(4, 5, 4, 5);
                endDatePicker.Name = "endDatePicker";
                endDatePicker.Size = new System.Drawing.Size(125, 26);
                endDatePicker.TabIndex = 6;
                endDatePicker.ValueChanged += new System.EventHandler(DatePicker_ValueChanged);
            }
            DatesPanel.BackColor = Color.Transparent;
            tablePanel.Controls.Add(DatesPanel);
            tablePanel.SetRow(DatesPanel, _lastRow++);   // Place this panel in the last empty row
        }

        void ShowTimesPanel(bool showGamingDate)
        {
            TimesPanel.SuspendLayout();
            this.SuspendLayout();

            TimesPanel.Controls.Clear();

            TimesPanel.Controls.Add(endTimePicker);
            TimesPanel.Controls.Add(lblEndTime);
            TimesPanel.Controls.Add(startTimePicker);
            TimesPanel.Controls.Add(lblStartTime);
            TimesPanel.Controls.Add(cbTime);
            TimesPanel.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            TimesPanel.Margin = new Padding(4, 5, 4, 5);
            TimesPanel.Name = "TimesPanel";
            TimesPanel.Size = new System.Drawing.Size(490, 48);

            endTimePicker.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            endTimePicker.Format = DateTimePickerFormat.Time;
            endTimePicker.Location = new System.Drawing.Point(349, 5);
            endTimePicker.Margin = new Padding(4, 5, 4, 5);
            endTimePicker.Name = "endTimePicker";
            endTimePicker.ShowUpDown = true;
            endTimePicker.Size = new System.Drawing.Size(91, 26);
            endTimePicker.TabIndex = 27;
            endTimePicker.ValueChanged += new System.EventHandler(TimePicker_ValueChanged);

            lblEndTime.AutoSize = true;
            lblEndTime.BackColor = System.Drawing.Color.Transparent;
            lblEndTime.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblEndTime.ImeMode = ImeMode.NoControl;
            lblEndTime.Location = new System.Drawing.Point(302, 9);
            lblEndTime.Margin = new Padding(4, 0, 4, 0);
            lblEndTime.Name = "lblEndTime";
            lblEndTime.Size = new System.Drawing.Size(42, 22);
            lblEndTime.TabIndex = 26;
            lblEndTime.Text = "End:";

            startTimePicker.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            startTimePicker.Format = DateTimePickerFormat.Time;
            startTimePicker.Location = new System.Drawing.Point(191, 5);
            startTimePicker.Margin = new Padding(4, 5, 4, 5);
            startTimePicker.Name = "startTimePicker";
            startTimePicker.ShowUpDown = true;
            startTimePicker.Size = new System.Drawing.Size(91, 26);
            startTimePicker.TabIndex = 25;
            startTimePicker.ValueChanged += new System.EventHandler(TimePicker_ValueChanged);

            lblStartTime.AutoSize = true;
            lblStartTime.BackColor = System.Drawing.Color.Transparent;
            lblStartTime.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblStartTime.ImeMode = ImeMode.NoControl;
            lblStartTime.Location = new System.Drawing.Point(133, 9);
            lblStartTime.Margin = new Padding(4, 0, 4, 0);
            lblStartTime.Name = "lblStartTime";
            lblStartTime.Size = new System.Drawing.Size(50, 22);
            lblStartTime.TabIndex = 24;
            lblStartTime.Text = "Start:";

            resources.ApplyResources(this.cbTime, "cbTime");
            cbTime.AutoSize = true;
            cbTime.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            cbTime.ImeMode = ImeMode.NoControl;
            cbTime.Location = new System.Drawing.Point(11, 8);
            cbTime.Margin = new Padding(4, 5, 4, 5);
            cbTime.CheckedChanged += new System.EventHandler(cbTime_CheckedChanged);
            cbTime.Name = "cbTime";
            cbTime.Size = new System.Drawing.Size(120, 26);
            cbTime.TabIndex = 23;
            cbTime.Text = "Include Time";
            cbTime.UseVisualStyleBackColor = true;

            cbTime.Visible = showGamingDate;
            lblStartTime.Visible = showGamingDate && cbTime.Checked;
            startTimePicker.Visible = showGamingDate && cbTime.Checked;
            lblEndTime.Visible = showGamingDate && cbTime.Checked;
            endTimePicker.Visible = showGamingDate && cbTime.Checked;

            TimesPanel.BackColor = Color.Transparent;
            tablePanel.Controls.Add(TimesPanel);
            tablePanel.SetRow(TimesPanel, _lastRow++);   // Place this panel in the last empty row

            TimesPanel.ResumeLayout(false);
            TimesPanel.PerformLayout();
            this.ResumeLayout();
        }


        void ShowMachinePanel(bool showMachine, bool showSession, bool showStaff, bool showPlayer)
        {
            MachinePanel.SuspendLayout();
            panelMachinePlayer.SuspendLayout();
            panelMachineStaff.SuspendLayout();
            panelMachineSession.SuspendLayout();
            panelMachineMach.SuspendLayout();
            this.SuspendLayout();

            MachinePanel.Controls.Clear();
            MachinePanel.AutoSize = true;
            MachinePanel.ColumnCount = 1;
            MachinePanel.RowCount = 4;
            MachinePanel.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            MachinePanel.Name = "MachinePanel";
            MachinePanel.Margin = new Padding(4, 5, 4, 5);
            MachinePanel.Size = new System.Drawing.Size(490, 48);

            if (showPlayer)
            {
                BuildMachinePlayerPanel();
            }

            if (showSession)
            {
                BuildMachineSessionPanel();
            }

            if (showStaff)
            {
                BuildMachineStaffPanel();
            }

            if (showMachine)
            {
                BuildMachineMachPanel();
            }

            MachinePanel.BackColor = Color.Transparent;
            MachinePanel.AutoSize = true;
            tablePanel.Controls.Add(MachinePanel);
            tablePanel.SetRow(MachinePanel, _lastRow++);   // Place this panel in the last empty row

            MachinePanel.ResumeLayout(false);
            MachinePanel.PerformLayout();
            panelMachinePlayer.ResumeLayout(false);
            panelMachinePlayer.PerformLayout();
            panelMachineStaff.ResumeLayout(false);
            panelMachineStaff.PerformLayout();
            panelMachineSession.ResumeLayout(false);
            panelMachineSession.PerformLayout();
            panelMachineMach.ResumeLayout(false);
            panelMachineMach.PerformLayout();
            this.ResumeLayout(false);
        }

        void BuildMachinePlayerPanel()
        {
            panelMachinePlayer.Controls.Clear();
            panelMachinePlayer.Controls.Add(fieldPerson);
            panelMachinePlayer.Controls.Add(lblPerson);
            panelMachinePlayer.Controls.Add(btnFindPlayer);
            panelMachinePlayer.BackColor = Color.Transparent;

            panelMachinePlayer.Name = "panelMachineButton";
            panelMachinePlayer.Size = new System.Drawing.Size(474, 40);
            panelMachinePlayer.TabIndex = 0;

            fieldPerson.AutoSize = true;
            fieldPerson.BackColor = System.Drawing.Color.Transparent;
            fieldPerson.ForeColor = System.Drawing.SystemColors.Desktop;
            fieldPerson.ImeMode = ImeMode.NoControl;
            fieldPerson.Location = new System.Drawing.Point(177, 13);
            fieldPerson.Margin = new Padding(4, 0, 4, 0);
            fieldPerson.Name = "fieldPerson";
            fieldPerson.Size = new System.Drawing.Size(18, 13);
            fieldPerson.TabIndex = 16;

            if (m_playerID > 0)
            {
                fieldPerson.Text = GetPersonName(m_playerID);
            }
            else
            {
                fieldPerson.Text = "All";
            }
            lblPerson.AutoSize = true;
            lblPerson.BackColor = System.Drawing.Color.Transparent;
            lblPerson.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblPerson.ImeMode = ImeMode.NoControl;
            lblPerson.Location = new System.Drawing.Point(95, 13);
            lblPerson.Margin = new Padding(4, 0, 4, 0);
            lblPerson.Name = "lblPerson";
            lblPerson.Size = new System.Drawing.Size(74, 22);
            lblPerson.TabIndex = 15;
            lblPerson.Text = "Player:";

            btnFindPlayer.BackColor = System.Drawing.Color.Transparent;
            btnFindPlayer.FocusColor = System.Drawing.Color.Black;
            // Use the preview button since it is a static control
            btnFindPlayer.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnPreview.ImageNormal")));
            btnFindPlayer.ImagePressed = ((System.Drawing.Image)(resources.GetObject("btnPreview.ImagePressed")));
            btnFindPlayer.MinimumSize = new System.Drawing.Size(30, 30);
            btnFindPlayer.Name = "btnFindPlayer";
            btnFindPlayer.UseVisualStyleBackColor = false;
            btnFindPlayer.Click += new System.EventHandler(btnFindPlayer_Click);
            btnFindPlayer.ImeMode = ImeMode.NoControl;
            btnFindPlayer.Location = new System.Drawing.Point(9, 5);
            btnFindPlayer.Margin = new Padding(4, 5, 4, 5);
            btnFindPlayer.MinimumSize = new System.Drawing.Size(30, 30);
            btnFindPlayer.Size = new System.Drawing.Size(54, 30);
            btnFindPlayer.Text = "&Find";

            resources.ApplyResources(btnFindPlayer, "btnFindPlayer");
            MachinePanel.Controls.Add(panelMachinePlayer);
        }
        void BuildMachineStaffPanel()
        {
            panelMachineStaff.Controls.Clear();
            panelMachineStaff.Controls.Add(lblStaff);
            panelMachineStaff.Controls.Add(cboStaff);
            panelMachineStaff.Name = "panelMachineStaff";
            panelMachineStaff.Size = new System.Drawing.Size(474, 39);
            panelMachineStaff.BackColor = Color.Transparent;

            lblStaff.AutoSize = true;
            lblStaff.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblStaff.ImeMode = ImeMode.NoControl;
            lblStaff.Location = new System.Drawing.Point(7, 8);
            lblStaff.Margin = new Padding(4, 0, 4, 0);
            lblStaff.Name = "lblStaff";
            lblStaff.Size = new System.Drawing.Size(55, 22);
            lblStaff.Text = "Staff: ";

            cboStaff.AutoCompleteMode = AutoCompleteMode.Append;
            cboStaff.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboStaff.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStaff.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            cboStaff.FormattingEnabled = true;
            cboStaff.Location = new System.Drawing.Point(99, 5);
            cboStaff.Margin = new Padding(4, 5, 4, 5);
            cboStaff.Name = "cboStaff";
            cboStaff.Size = new System.Drawing.Size(339, 30);
            cboStaff.SelectedIndexChanged += new EventHandler(cboStaff_SelectedIndexChanged);

            MachinePanel.Controls.Add(panelMachineStaff);
        }
        void BuildMachineSessionPanel()
        {
            panelMachineSession.Controls.Clear();
            panelMachineSession.Controls.Add(lblSession);
            panelMachineSession.Controls.Add(cboSession);
            panelMachineSession.Name = "panelMachineSession";
            panelMachineSession.Size = new System.Drawing.Size(474, 39);
            panelMachineSession.BackColor = Color.Transparent;

            lblSession.AutoSize = true;
            lblSession.BackColor = System.Drawing.Color.Transparent;
            lblSession.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblSession.ImeMode = ImeMode.NoControl;
            lblSession.Location = new System.Drawing.Point(7, 8);
            lblSession.Margin = new Padding(4, 0, 4, 0);
            lblSession.Name = "lblSession";
            lblSession.Size = new System.Drawing.Size(55, 22);
            lblSession.Text = "Session: ";
            lblSession.Visible = true;

            cboSession.AutoCompleteMode = AutoCompleteMode.Append;
            cboSession.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboSession.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSession.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            cboSession.FormattingEnabled = true;
            cboSession.Location = new System.Drawing.Point(99, 5);
            cboSession.Margin = new Padding(4, 5, 4, 5);
            cboSession.Name = "cboSession";
            cboSession.Size = new System.Drawing.Size(339, 30);
            cboSession.Visible = true;
            cboSession.SelectedIndexChanged += new EventHandler(cboSession_SelectedIndexChanged);
            cboSession.Validating += new System.ComponentModel.CancelEventHandler(cboSession_Validating);
            cboSession.TextChanged += new System.EventHandler(cboSession_TextChanged);

            MachinePanel.Controls.Add(panelMachineSession);
        }
        void BuildMachineMachPanel()
        {
            panelMachineMach.Controls.Clear();
            panelMachineMach.Controls.Add(cboMachine);
            panelMachineMach.Controls.Add(lblMachine);
            panelMachineMach.Name = "panelMachineMach";
            panelMachineMach.Size = new System.Drawing.Size(474, 40);
            panelMachineMach.BackColor = Color.Transparent;

            cboMachine.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMachine.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            cboMachine.FormattingEnabled = true;
            cboMachine.Location = new System.Drawing.Point(99, 5);
            cboMachine.Margin = new Padding(4, 5, 4, 5);
            cboMachine.Name = "cboMachine";
            cboMachine.Size = new System.Drawing.Size(339, 30);
            cboMachine.SelectedIndexChanged += new EventHandler(cboMachine_SelectedIndexChanged);

            lblMachine.AutoSize = true;
            lblMachine.BackColor = System.Drawing.Color.Transparent;
            lblMachine.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblMachine.ImeMode = ImeMode.NoControl;
            lblMachine.Location = new System.Drawing.Point(5, 8);
            lblMachine.Margin = new Padding(4, 0, 4, 0);
            lblMachine.Name = "lblMachine";
            lblMachine.Size = new System.Drawing.Size(74, 22);
            lblMachine.TabIndex = 36;
            lblMachine.Text = "Machine:";
            MachinePanel.Controls.Add(panelMachineMach);
        }

        void ShowQuartersPanel(bool showQuarter, bool showYear)
        {
            QuartersPanel.SuspendLayout();
            this.SuspendLayout();

            QuartersPanel.Controls.Clear();
            QuartersPanel.Location = new System.Drawing.Point(4, 5);
            QuartersPanel.Margin = new Padding(4, 5, 4, 5);
            QuartersPanel.Name = "QuartersPanel";
            QuartersPanel.Size = new System.Drawing.Size(488, 107);
            QuartersPanel.TabIndex = 32;

            QuartersPanel.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));

            if (showQuarter)
            {
                groupQuarter.Controls.Add(rbQ4);
                groupQuarter.Controls.Add(rbQ3);
                groupQuarter.Controls.Add(rbQ2);
                groupQuarter.Controls.Add(rbQ1);
                groupQuarter.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                groupQuarter.Location = new System.Drawing.Point(4, 5);
                groupQuarter.Margin = new Padding(4, 5, 4, 5);
                groupQuarter.Name = "groupQuarter";
                groupQuarter.Padding = new Padding(4, 5, 4, 5);
                groupQuarter.Size = new System.Drawing.Size(324, 97);
                groupQuarter.TabIndex = 28;
                groupQuarter.TabStop = false;
                groupQuarter.Text = "Quarter";

                rbQ4.AutoSize = true;
                rbQ4.Font = new System.Drawing.Font("Trebuchet MS", 9.75F);
                rbQ4.ImeMode = ImeMode.NoControl;
                rbQ4.Location = new System.Drawing.Point(170, 58);
                rbQ4.Margin = new Padding(4, 5, 4, 5);
                rbQ4.Name = "rbQ4";
                rbQ4.Size = new System.Drawing.Size(144, 22);
                rbQ4.TabIndex = 3;
                rbQ4.Text = "Q4 : Oct - Nov - Dec";
                rbQ4.UseVisualStyleBackColor = true;
                rbQ4.CheckedChanged += new System.EventHandler(rbQuarterChanged);

                rbQ3.AutoSize = true;
                rbQ3.Font = new System.Drawing.Font("Trebuchet MS", 9.75F);
                rbQ3.ImeMode = ImeMode.NoControl;
                rbQ3.Location = new System.Drawing.Point(170, 25);
                rbQ3.Margin = new Padding(4, 5, 4, 5);
                rbQ3.Name = "rbQ3";
                rbQ3.Size = new System.Drawing.Size(140, 22);
                rbQ3.TabIndex = 2;
                rbQ3.Text = "Q3 : Jul - Aug - Sep";
                rbQ3.UseVisualStyleBackColor = true;
                rbQ3.CheckedChanged += new System.EventHandler(rbQuarterChanged);

                rbQ2.AutoSize = true;
                rbQ2.Font = new System.Drawing.Font("Trebuchet MS", 9.75F);
                rbQ2.ImeMode = ImeMode.NoControl;
                rbQ2.Location = new System.Drawing.Point(8, 58);
                rbQ2.Margin = new Padding(4, 5, 4, 5);
                rbQ2.Name = "rbQ2";
                rbQ2.Size = new System.Drawing.Size(144, 22);
                rbQ2.TabIndex = 1;
                rbQ2.Text = "Q2 : Apr - May - Jun";
                rbQ2.UseVisualStyleBackColor = true;
                rbQ2.CheckedChanged += new System.EventHandler(rbQuarterChanged);

                rbQ1.AutoSize = true;
                rbQ1.Checked = true;
                rbQ1.Font = new System.Drawing.Font("Trebuchet MS", 9.75F);
                rbQ1.ImeMode = ImeMode.NoControl;
                rbQ1.Location = new System.Drawing.Point(8, 26);
                rbQ1.Margin = new Padding(4, 5, 4, 5);
                rbQ1.Name = "rbQ1";
                rbQ1.Size = new System.Drawing.Size(144, 22);
                rbQ1.TabIndex = 0;
                rbQ1.TabStop = true;
                rbQ1.Text = "Q1 : Jan - Feb - Mar";
                rbQ1.UseVisualStyleBackColor = true;
                rbQ1.CheckedChanged += new System.EventHandler(rbQuarterChanged);

                QuartersPanel.Controls.Add(groupQuarter);
            }

            if (showYear)
            {
                lblYear.Text = "Year";
                lblYear.Location = new System.Drawing.Point(345, 30);
                lblYear.Size = new System.Drawing.Size(42, 22);

                upYear.Location = new System.Drawing.Point(349, 60);
                upYear.Maximum = new decimal(new int[] {
                2100,
                0,
                0,
                0});
                upYear.Minimum = new decimal(new int[] {
                2000,
                0,
                0,
                0});
                upYear.Name = "upYear";
                upYear.Size = new System.Drawing.Size(92, 26);
                upYear.TabIndex = 34;
                m_year = DateTime.Now.Year;
                upYear.Value = m_year;

                QuartersPanel.Controls.Add(lblYear);
                QuartersPanel.Controls.Add(upYear);
            }

            QuartersPanel.BackColor = Color.Transparent;
            tablePanel.Controls.Add(QuartersPanel);
            tablePanel.SetRow(QuartersPanel, _lastRow++);   // Place this panel in the last empty row

            QuartersPanel.ResumeLayout(false);
            QuartersPanel.PerformLayout();
            this.ResumeLayout();
        }

        #endregion Dynamic Control Loading

        #region Report Methods
        public void KillWaitForm()
        {
            if (m_frmWaiting != null)
            {
                m_frmWaiting.CloseForm();
                m_frmWaiting.Dispose();
                m_frmWaiting = null;
            }
        }

        private void ObjRptCtrRunReportsError(object sender, ReportEventArgs e)
        {
            string strMsg = e.ReportName + "(" + e.ReportID + ") " + " " + e.ErrorMsg;
            KillWaitForm();
            MessageForm.Show(this, strMsg, "Report Center Error");
        }

        private void DoReporting(bool bPrint)
        {
            if (!ValidateDates()) return;

            lock (this)
            {
                if (GetReportsToRun(bPrint))
                {
                    m_frmWaiting = new WaitForm
                    {
                        CancelButtonClosesForm = true,
                        CancelButtonVisible = true,
                        Message = "Retrieving all reports...Please wait.",
                        WaitImage = Resources.PleaseWait,
                        Cursor = Cursors.WaitCursor,
                        ProgressBarVisible = true
                    };

                    m_objRptCtr = new Business.ReportCenter();

                    try
                    {
                        m_objRptCtr.RunReportsError += ObjRptCtrRunReportsError;

                        // This creates and fires the Worker Thread
                        m_objRptCtr.FireReportThread(m_frmWaiting, m_reportsToRun);
                        m_frmWaiting.ShowDialog(this);
                        Application.DoEvents();

                        KillWaitForm();
                        if (m_objRptCtr.ReportObjects != null && !m_objRptCtr.IsCanceled)
                        {
                            ShowReports(m_objRptCtr.ReportObjects, bPrint);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageForm.Show(this, "DoReporting : " + ex.Message, Resources.report_center);
                    }
                    finally
                    {
                        m_objRptCtr.RunReportsError -= ObjRptCtrRunReportsError;
                    }
                }
            }
        }

        private void LoadLocalData()
        {
            //if (fieldPerson.Visible && fieldPerson.Text.Equals(Resources.infoAll))
            //{
            //    m_playerID = 0;
            //    m_staffID = 0;
            //}

            m_startDate = startDatePicker.Value;
            m_enddate = endDatePicker.Value;
            //upYear.Value = DateTime.Now.Year;
            if (m_year == 0) m_year = DateTime.Now.Year;

            //US1622
            m_AccrualsName = cboAccName.Text == "All" ? "%" : cboAccName.Text;
            m_AccrualsType = cboAccType.Text == "All" ? "%" : cboAccType.Text;
            System.Web.UI.WebControls.ListItem selectedStatus = (System.Web.UI.WebControls.ListItem)cboStatus.SelectedItem;

            if (selectedStatus == null)
            {
                m_activeStatus = 0;
            }
            else
            {
                if (!int.TryParse(selectedStatus.Value, out m_activeStatus)) m_activeStatus = 0;
            }
        }

        private bool GetReportsToRun(bool bPrint)
        {
            m_reportsToRun.Clear();
            LoadLocalData();
            int reportNumber = 0;

            foreach (TreeNode node in reportTreeView.Nodes)
            {
                foreach (ReportTreeNode reportNode in node.Nodes)
                {
                    if (reportNode.Checked)
                    {
                        clsReport rpt = new clsReport(reportNode.ReportInfo)
                        {
                            StartDate = m_startDate,
                            EndDate = m_enddate,
                            PlayerID = m_playerID,
                            PrintReport = bPrint,
                            Session = m_session,
                            StaffID = m_staffID,
                            Quarter = m_quarter,
                            Year = m_year,
                            MachineID = m_machineId //RALLY DE6958
                            ,
                            AccrualsName = m_AccrualsName
                            ,
                            AccrualsStatus = m_activeStatus
                            ,
                            AccrualsType = m_AccrualsType
                            ,
                            PlayerTaxID = m_PlayerTaxID
                            ,
                            PositionID = m_PositionID
                            ,
                            SerialNo = m_SerialNo //DE12064                          
                            ,
                            ProductItemID = m_ProductItemID
                            ,
                            CharityId = m_CharityId
                            ,
                            ProgramId = m_ProgramId
                            ,
                            POSMenuId = m_PosMenuId
                            ,
                            ProductTypeId = m_ProductTypeId
                            ,
                            LocationID = m_LocationID
                            ,
                            ProductGroupID = m_ProductGroupID
                            ,
                            SerialNbrDevice = m_SerialNbrDevice
                            ,
                            AuditLogType = m_auditTypeID
                            ,
                            ByPackage = m_ByPackage
                            ,
                            CompID = m_CompId
                            ,
                            isActive = m_activeStatus

                        };

                        if (cbTime.Checked)
                        {
                            rpt.StartDate =
                                DateTime.Parse(startDatePicker.Text + " " + startTimePicker.Text);
                            rpt.EndDate = DateTime.Parse(endDatePicker.Text + " " + endTimePicker.Text);
                            rpt.GamingDate = 1;
                        }

                        m_reportsToRun.Add(reportNumber++, rpt);
                    }
                }
            }
            return m_reportsToRun.Count > 0;
        }

        private void ShowReports(ICollection<clsReport> arReports, bool bPrint)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (arReports.Count > 0)
                {
                    double totalReports = arReports.Count;
                    double curReport = 0;
                    string baseMsg = bPrint ? "Sending Reports to printer....." : "Building Print Preview.....";

                    m_frmWaiting = new WaitForm
                    {
                        CancelButtonClosesForm = false,
                        CancelButtonVisible = false,
                        Message = baseMsg,
                        WaitImage = Resources.Waiting,
                        ProgressBarVisible = true,
                        TopMost = true,
                        StartPosition = FormStartPosition.Manual,
                        Location = new Point(300, 100)
                    };

                    // This creates and fires the Worker Thread
                    foreach (clsReport report in arReports)
                    {
                        m_frmWaiting.Message = baseMsg + report.ReportInfo.DisplayName;

                        // US1831: NEW RUNTIME HAS ITS OWN WAIT INDICATOR
                        // Create this wait form but do not display it to retain existing code!
                        //m_frmWaiting.Show();

                        if (!report.PrintReport)
                        {
                            // show print preview form
                            frmReport frm = new frmReport(report.CrystalRptDoc)
                            {
                                Cursor = Cursors.Default,
                                WindowState = FormWindowState.Maximized,
                                StartPosition = FormStartPosition.CenterParent,
                                TopLevel = true,
                                ShowInTaskbar = false
                            };
                            frm.Show();
                            frm.BringToFront();
                        }
                        else
                        {
                            // print to printer 
                            string ourPrinter = Configuration.mGlobalPrinterName;
                            int copies = 1;

                            if (string.IsNullOrEmpty(ourPrinter)) //we don't have a printer, get one from the user
                            {
                                var printer = new PrintDialog();
                                
                                printer.AllowCurrentPage = false;
                                printer.AllowPrintToFile = false;
                                printer.AllowSelection = false;
                                printer.AllowSomePages = false;
                                printer.ShowHelp = false;

                                DialogResult result = printer.ShowDialog();

                                if (result == DialogResult.OK)
                                {
                                    ourPrinter = printer.PrinterSettings.PrinterName;
                                    copies = printer.PrinterSettings.Copies;
                                }

                                printer.Dispose();
                            }

                            if (!string.IsNullOrEmpty(ourPrinter)) //we have a printer, print the report
                            {
                                bool receiptPrinter = false;
                                bool correctlyDefined = false;
                                PrinterSettings ps = new PrinterSettings();
                                
                                ps.PrinterName = ourPrinter;

                                if (ps.IsValid)
                                {
                                    foreach (System.Drawing.Printing.PaperSize psz in ps.PaperSizes)
                                    {
                                        if (psz.PaperName.ToUpper().Contains("RECEIPT") || psz.PaperName.ToUpper().Contains("ROLL PAPER") || (psz.Height / psz.Width > 10)) //looks like receipt paper, assume this is a receipt printer
                                        {
                                            receiptPrinter = true;

                                            if (ps.DefaultPageSettings.PaperSize.RawKind == psz.RawKind)
                                            {
                                                correctlyDefined = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                report.CrystalRptDoc.PrintOptions.PrinterName = ourPrinter;

                                if (receiptPrinter)
                                {
                                    if (correctlyDefined)
                                        report.CrystalRptDoc.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize; //a bug with some drivers sets this to LETTER, so we will fix it
                                    else
                                        report.CrystalRptDoc.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true; //do our best to get the report to fill the page
                                }

                                report.CrystalRptDoc.PrintToPrinter(copies, copies > 1, 0, 0);
                            }
                        }

                        ++curReport;
                        int percent = (int)((curReport / totalReports) * 100.0);
                        m_frmWaiting.ReportProgress(null, new ProgressChangedEventArgs(percent, null));
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception e)
            {
                MessageForm.Show(this, "ShowReports : " + e.Message, Resources.report_center);
            }
            finally
            {
                Cursor = Cursors.Default;
                KillWaitForm();
            }
        }
        #endregion

        #region Report Parameters Controls

        // Start Date Picker ..............
        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            LoadSessionCombo();
            ValidateDates();
        }

        private void cbTime_CheckedChanged(object sender, EventArgs e)
        {
            startTimePicker.Visible = cbTime.Checked;
            endTimePicker.Visible = cbTime.Checked;
            lblStartTime.Visible = cbTime.Checked;
            lblEndTime.Visible = cbTime.Checked;
        }

        private void TimePicker_ValueChanged(object sender, EventArgs e)
        {
            ValidateDates();
        }

        private void rbQuarterChanged(object sender, EventArgs e)
        {
            if (rbQ1.Checked) m_quarter = 1;
            else if (rbQ2.Checked) m_quarter = 2;
            else if (rbQ3.Checked) m_quarter = 3;
            else if (rbQ4.Checked) m_quarter = 4;
        }

        #endregion Report Parameters Controls


    } // end FrmReportManager

    public class Staff
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

}
