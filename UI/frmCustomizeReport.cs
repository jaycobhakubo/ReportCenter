#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion
// FIX : DE3252 Customized reports not working (entire source file updated)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using GTI.Modules.ReportCenter.Data;
using GTI.Modules.Shared;
using GTI.Modules.ReportCenter.Business;
using GTI.Modules.ReportCenter.Properties;

namespace GTI.Modules.ReportCenter.UI
{
    public partial class frmCustomizeReport : GradientForm
    {
        #region Declarations

        private Dictionary<int, ReportInfo> m_reportsDictionary;
        private Dictionary<int, UserReportType> m_userReportDictionary;
        private int m_customReportCounter;
        #endregion

        public frmReportCenterMDIParent MyParent { get; private set; }

        #region Customize Report methods
        public frmCustomizeReport(frmReportCenterMDIParent myParent)
        {
            MyParent = myParent;
            InitializeComponent();
        }

        private void CustomizeReport_Load(object sender, EventArgs e)
        {
            // FIX:RALLY DE 3336 Crash when database has no custom reports
            if (MyParent.userReportMenu != null)
                MyParent.userReportMenu.Visible = false;
            // END:RALLY DE 3336 Crash when database has no custom reports
            LoadPredefinedReports();
            LoadCustomReports();
            saveButton.Enabled = false;
        }

        private void frmCustomizeReport_Enter(object sender, EventArgs e)
        {
            Debug.WriteLine("Entering Customize Reports");
            // FIX: RALLY DE 3336 Crash when database has no custom reports
            if (MyParent.userReportMenu != null)
                MyParent.userReportMenu.Visible = false;
            // END: RALLY DE 3336 Crash when database has no custom reports
            SetDeleteEnable();
        }

        private void frmCustomizeReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine("FormClosing Customize Reports");
            if (saveButton.Enabled)
            {
                if (MessageForm.Show(Resources.CustomReportsNotSaved, Resources.CustomReports, MessageFormTypes.YesNo) == DialogResult.Yes)
                {
                    saveReportsClick(null, null);
                }
            }
        }

        private bool m_alreadyLeft;
        private void frmCustomizeReport_Leave(object sender, EventArgs e)
        {
            
            if (m_alreadyLeft)
            {
                m_alreadyLeft = false;
                return;
            }
            Debug.WriteLine("Leaving Customize Reports");
            if (saveButton.Enabled)
            {
                if (MessageForm.Show(Resources.CustomReportsNotSaved, Resources.CustomReports , MessageFormTypes.YesNo) == DialogResult.Yes)
                {
                    saveReportsClick(null, null);
                }
            }
            m_alreadyLeft = true;
            MyParent.LoadUserDefinedReports(true);
        }
        
        //RALLY DE 4505 START
        const int BingoCardSalesDetailReport = 16;
        const int BingoCardSummaryReport = 24;
        //RALLY DE 4505 END

        private static ReportSetTreeNode CreateReportSetTreeNode(ReportTypes reportTypeID)
        {
            ReportSetTreeNode setTreeNode;
            switch (reportTypeID)
            {
                case ReportTypes.Bingo:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Bingo, ReportTypes.Bingo);
                    break;
                case ReportTypes.Electronics:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Electronics, ReportTypes.Electronics);
                    break;
                case ReportTypes.Exceptions:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Exceptions, ReportTypes.Exceptions);
                    break;
                case ReportTypes.Misc:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Misc, ReportTypes.Misc);
                    break;
                case ReportTypes.Player:
                    setTreeNode = Configuration.mMachineAccounts
                                      ? new ReportSetTreeNode(Resources.reportTypePlayersMachine, ReportTypes.Player)
                                      : new ReportSetTreeNode(Resources.reportType_Players, ReportTypes.Player);
                    break;
                case ReportTypes.Sales:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Sales, ReportTypes.Sales);
                    break;
                case ReportTypes.Staff:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Staff, ReportTypes.Staff);
                    break;
                // Rally US1492
                case ReportTypes.Paper:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Paper, ReportTypes.Paper);
                    break;
                case ReportTypes.Gaming:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Gaming, ReportTypes.Gaming);
                    break;
                case ReportTypes.Inventory:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Inventory, ReportTypes.Inventory);
                    break;
                // END: US1492

                // US1831, US1844
                case ReportTypes.Accruals:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Accruals, ReportTypes.Accruals);
                    break;
                case ReportTypes.Payouts:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Payouts, ReportTypes.Payouts);
                    break;
                // END US1831, 1844

                //DE10904
                case ReportTypes.Texas:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Texas, ReportTypes.Texas);
                    break;
                case ReportTypes.Coupon:
                    setTreeNode = new ReportSetTreeNode(Resources.reportType_Coupon, ReportTypes.Coupon);
                    break;
                
                default:
                    throw new NullReferenceException("Report Type not found");
            }
            return setTreeNode;
        }

        #endregion

        #region Pre-Defined Reports
        private void LoadPredefinedReports()
        {
            predefinedReportTreeView.Nodes.Clear();

            m_reportsDictionary = MyParent.ReportsDictionary;
            ReportSetTreeNode salesTreeNode = CreateReportSetTreeNode(ReportTypes.Sales);
            ReportSetTreeNode playerTreeNode = CreateReportSetTreeNode(ReportTypes.Player);
            ReportSetTreeNode miscTreeNode = CreateReportSetTreeNode(ReportTypes.Misc);
            ReportSetTreeNode staffTreeNode = CreateReportSetTreeNode(ReportTypes.Staff);
            ReportSetTreeNode bingoTreeNode = CreateReportSetTreeNode(ReportTypes.Bingo);
            ReportSetTreeNode electronicTreeNode = CreateReportSetTreeNode(ReportTypes.Electronics);
            ReportSetTreeNode exceptionsTreeNode = CreateReportSetTreeNode(ReportTypes.Exceptions);
            // Rally US1492
            ReportSetTreeNode paperTreeNode = CreateReportSetTreeNode(ReportTypes.Paper);
            ReportSetTreeNode inventoryTreeNode = CreateReportSetTreeNode(ReportTypes.Inventory);
            ReportSetTreeNode gamingTreeNode = CreateReportSetTreeNode(ReportTypes.Gaming);

            // US1831, US1844, US1845
            ReportSetTreeNode accrualsTreeNode = CreateReportSetTreeNode(ReportTypes.Accruals);
            ReportSetTreeNode payoutsTreeNode = CreateReportSetTreeNode(ReportTypes.Payouts);

            ReportSetTreeNode texasTreeNode = CreateReportSetTreeNode(ReportTypes.Texas);
            ReportSetTreeNode couponTreeNode = CreateReportSetTreeNode(ReportTypes.Coupon);

            
            predefinedReportTreeView.Nodes.Add(bingoTreeNode);
            predefinedReportTreeView.Nodes.Add(electronicTreeNode);
            predefinedReportTreeView.Nodes.Add(exceptionsTreeNode);
            predefinedReportTreeView.Nodes.Add(miscTreeNode);
            predefinedReportTreeView.Nodes.Add(playerTreeNode);
            predefinedReportTreeView.Nodes.Add(salesTreeNode);
            predefinedReportTreeView.Nodes.Add(staffTreeNode);
            predefinedReportTreeView.Nodes.Add(gamingTreeNode);
            predefinedReportTreeView.Nodes.Add(paperTreeNode);
            predefinedReportTreeView.Nodes.Add(inventoryTreeNode);

            // END: US1492

            predefinedReportTreeView.Nodes.Add(accrualsTreeNode);   // US1831
            predefinedReportTreeView.Nodes.Add(payoutsTreeNode);    // US1844
            predefinedReportTreeView.Nodes.Add(couponTreeNode);

            if (Configuration.m_txPayoutsEnabled) // DE10904 only add the Texas node if TX payouts are enabled
            {
                predefinedReportTreeView.Nodes.Add(texasTreeNode);
            }

            foreach (KeyValuePair<int, ReportInfo> report in m_reportsDictionary)
            {
                switch ((ReportTypes)report.Value.TypeID)
                {
                    case ReportTypes.Bingo:
                        bingoTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    case ReportTypes.Electronics:
                        electronicTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    case ReportTypes.Exceptions:
                        exceptionsTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    case ReportTypes.Misc:
                        miscTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    case ReportTypes.Player:
                        playerTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    case ReportTypes.Sales:
                        // START RALLY DE4505 - Do NOT show bingoCard reports if PWP is on.
                        if ((report.Value.ID == BingoCardSalesDetailReport || 
                             report.Value.ID == BingoCardSummaryReport)
                            && Configuration.m_playWithPaper)
                            continue;
                        salesTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                        //END RALLY DE 4505
                    case ReportTypes.Staff:
                        staffTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    // Rally US1492
                    case ReportTypes.Paper:
                        paperTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    case ReportTypes.Gaming:
                        gamingTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    case ReportTypes.Inventory:
                        inventoryTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    // END: US1492

                    // US1831, 1844
                    case ReportTypes.Accruals:
                        accrualsTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                    case ReportTypes.Payouts:
                        payoutsTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;

                    case ReportTypes.Texas:
                        if (Configuration.m_txPayoutsEnabled) // only add the Texas reports 
                        {
                            texasTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        }
                        break;
                    case ReportTypes.Coupon:
                        couponTreeNode.Nodes.Add(new ReportTreeNode(report.Value, false));
                        break;
                }
            }
        }

        private static bool m_busyCheckSetting;
        private void predefinedReportTreeView_AfterCheck(object sender, TreeViewEventArgs args)
        {
            if (m_busyCheckSetting)
                return;
            m_busyCheckSetting = true;
            // check or uncheck all children
            foreach (TreeNode subNode in args.Node.Nodes)
            {
                if (subNode.Checked != args.Node.Checked)
                    subNode.Checked = args.Node.Checked;
            }
            TreeNode parent = args.Node.Parent;
            if (parent != null)
            {
                bool parentFlag = true;

                foreach (TreeNode sibling in parent.Nodes)
                {
                    if (sibling.Checked == false)
                    {
                        parentFlag = false;
                        break;
                    }
                }
                if (parent.Checked != parentFlag)
                    parent.Checked = parentFlag;
            }
            m_busyCheckSetting = false;
        }

        /// <summary>
        /// Adds the checked report(s) to the Customize Report list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addCheckedReportClick(object sender, EventArgs e)
        {
            //START RALLY DE 6919 error when deleting a group
            if (customReportTreeView.Nodes.Count == 0 || customReportTreeView.SelectedNode == null)
            {
                return;
            }
            //END RALLY DE 6919

            foreach (TreeNode node in predefinedReportTreeView.Nodes)
            {
                foreach (ReportTreeNode report in node.Nodes)
                {
                    if (report.Checked)
                    {
                        ReportTypes reportType = (ReportTypes)report.ReportInfo.TypeID;
                        bool setFound = false;
                        bool reportFound = false;
                        // Starting with the selected Group Node, see if the ReportSet Node is available
                        foreach (ReportSetTreeNode setNode in customReportTreeView.SelectedNode.Nodes)
                        {
                            if (setNode.ReportType == reportType)
                            {
                                // If present, we need to add the Report node if it is not in the ReportSet node.
                                setFound = true;
                                foreach (ReportTreeNode reportNode in setNode.Nodes)
                                {
                                    if (reportNode.ReportInfo.ID == report.ReportInfo.ID)
                                    {
                                        reportFound = true;
                                        break;
                                    }
                                }

                                if (!reportFound)
                                {
                                    saveButton.Enabled = true;
                                    ReportTreeNode tempReport = new ReportTreeNode(report.ReportInfo, true);
                                    setNode.Nodes.Add(tempReport);
                                    setNode.Expand();
                                }
                                break;
                            }
                        }

                        // If ReportSet is not in group, we need to create and add it to the group node 
                        //  then create and add the Report Node to the ReportSet.
                        if (!setFound)
                        {
                            saveButton.Enabled = true;
                            ReportSetTreeNode setTreeNode = CreateReportSetTreeNode(reportType);
                            setTreeNode.Nodes.Add(new ReportTreeNode(report.ReportInfo, true));
                            customReportTreeView.SelectedNode.Nodes.Add(setTreeNode);
                            setTreeNode.Expand();
                        }
                    }
                }
                customReportTreeView.SelectedNode.Expand();
            }
            // Uncheck all boxes
            foreach (TreeNode node in predefinedReportTreeView.Nodes)
            {
                if (node.Checked) 
                    node.Checked = false;
                else
                {
                    foreach (TreeNode treeNode in node.Nodes)
                    {
                        if (treeNode.Checked)
                            treeNode.Checked = false;
                    }
                }
            }
        }
        #endregion

        #region Custom Reports
        private void LoadCustomReports()
        {
            customReportTreeView.Nodes.Clear();
            MyParent.LoadUserDefinedReports(false);//RALLY DE 6239
            m_userReportDictionary = MyParent.UserReportTypesDictionary;
            if (m_userReportDictionary == null || m_userReportDictionary.Count == 0)
            {
                m_customReportCounter = 0;
                return;
            }
            m_customReportCounter = m_userReportDictionary.Count; //we will limit 10 types only
            UserReportGroupTreeNode tempGroup;
            UserReportTypeTreeNode tempType;

            foreach (KeyValuePair<int, UserReportType> type in m_userReportDictionary)
            {
                tempType = new UserReportTypeTreeNode(type.Value, false);
                foreach (KeyValuePair<int, UserReportGroup> group in type.Value.UserReportGroups)
                {
                    tempGroup = new UserReportGroupTreeNode(group.Value, false);
                    for (int iReport = 0; iReport < group.Value.ReportsArray.Count; iReport++)
                    {
                        // determine report set to use
                        ReportInfo info = (ReportInfo)group.Value.ReportsArray[iReport];
                        ReportTypes reportTypeID = (ReportTypes)info.TypeID;
                        // START RALLY DE4505 - Do NOT show bingoCard reports if PWP is on.
                        if ((info.ID == BingoCardSalesDetailReport ||
                             info.ID == BingoCardSummaryReport)
                            && Configuration.m_playWithPaper && reportTypeID == ReportTypes.Sales)
                            continue;
                        //END RALLY DE4505
                        ReportSetTreeNode setTreeNode = null;
                        foreach (ReportSetTreeNode setNode in tempGroup.Nodes)
                        {
                            if (setNode.ReportType == reportTypeID)
                            {
                                setTreeNode = setNode;
                                break;
                            }
                        }
                        if (setTreeNode == null)
                        {
                            setTreeNode = CreateReportSetTreeNode(reportTypeID);
                            tempGroup.Nodes.Add(setTreeNode);
                        }
                        setTreeNode.Nodes.Add(new ReportTreeNode((ReportInfo)group.Value.ReportsArray[iReport], false));
                    }
                    tempType.Nodes.Add(tempGroup);
                }
                customReportTreeView.Nodes.Add(tempType);
            }
        }

        private void DisableCustomizeTreeRelatedButtons()
        {
            addReportButton.Enabled = false;
            newGroupButton.Enabled = false;
            deleteButton.Enabled = false;
        }

        private void customReportTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            TreeNode tn = customReportTreeView.SelectedNode;
            if (tn is ReportSetTreeNode || tn is ReportTreeNode) e.CancelEdit = true;
        }

        public static bool IsValidAlphaNumeric(string inputStr)
        {
            //make sure the user provided us with data to check
            //if not then return false
            if (string.IsNullOrEmpty(inputStr))
                return false;

            //now we need to loop through the string, examining each character
            for (int i = 0; i < inputStr.Length; i++)
            {
                //if this character isn't a letter and it isn't a number then return false
                //because it means this isn't a valid alpha numeric string
                if (!(char.IsLetter(inputStr[i])) && (!(char.IsNumber(inputStr[i]))))
                    return false;
            }
            //we made it this far so return true
            return true;
        }

        private void customReportLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (IsValidAlphaNumeric(e.Label))
                    {
                        // Stop editing without canceling the label change.
                        e.Node.EndEdit(false);
                        TreeNode tn = customReportTreeView.SelectedNode;
                        tn.Text = e.Label;
                        UserReportGroupTreeNode groupTreeNode = tn as UserReportGroupTreeNode;
                        if (groupTreeNode != null)
                        {
                            if (groupTreeNode.ReportGroup != null)
                            {
                                groupTreeNode.ReportGroup.UserGroupName = tn.Text;
                                saveButton.Enabled = true;
                            }
                        }
                        UserReportTypeTreeNode typeTreeNode = tn as UserReportTypeTreeNode;
                        if (typeTreeNode != null)
                        {
                            if (typeTreeNode.ReportType != null)
                            {
                                typeTreeNode.ReportType.UserReportTypeName = tn.Text;
                                saveButton.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        /* Cancel the label edit action, inform the user, and 
                           place the node in edit mode again. */
                        e.CancelEdit = true;
                        MessageForm.Show(this, Resources.infoNameInvalid, Resources.report_center);
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    /* Cancel the label edit action, inform the user, and 
                       place the node in edit mode again. */
                    e.CancelEdit = true;
                    MessageForm.Show(this, Resources.infoNameNotNull, Resources.report_center);
                    e.Node.BeginEdit();
                }
            }
        }

        private void customReportSelect(object sender, TreeViewEventArgs e)
        {
            DisableCustomizeTreeRelatedButtons();
            if (e.Node is UserReportTypeTreeNode)
            {
                newGroupButton.Enabled = true;
            }
            if (e.Node is UserReportGroupTreeNode)
            {
                addReportButton.Enabled = true;
            }
            SetDeleteEnable();
        }

        private void SetDeleteEnable()
        {
            deleteButton.Enabled = false;
            foreach (UserReportTypeTreeNode typeNode in customReportTreeView.Nodes)
            {
                foreach (UserReportGroupTreeNode groupNode in typeNode.Nodes)
                {
                    foreach (ReportSetTreeNode setNode in groupNode.Nodes)
                    {
                        foreach (ReportTreeNode reportNode in setNode.Nodes)
                        {
                            if (reportNode.Checked)
                            {
                                deleteButton.Enabled = true;
                                return;
                            }
                        }
                    }
                    if (groupNode.Checked)
                    {
                        deleteButton.Enabled = true;
                        return;
                    }
                }

                if (typeNode.Checked)
                {
                    deleteButton.Enabled = true;
                    return;
                }
            }
        }

        private void newTypeClick(object sender, EventArgs e)
        {
            if (m_customReportCounter >= Configuration.PreDefinedCustomizeTypeCount)
            {
                MessageForm.Show(this,
                    string.Format(Resources.infoCustomerTypeLimit, Configuration.PreDefinedCustomizeTypeCount),
                    Resources.report_center);
                return;
            }
            frmInputBox newType = new frmInputBox(Resources.infoNewTypePrompt, Resources.infoNewTypeTitle, Resources.infoNewTypeDefaultValue);
            newType.ShowDialog();
            if (newType.DialogResult == DialogResult.OK)
            {
                string typeName = newType.InputValue;
                if (typeName.Length > Configuration.PreDefinedCustomizeTypeNameLength)
                {  //we truncate the name
                    typeName = typeName.Substring(0,Configuration.PreDefinedCustomizeTypeNameLength);
                }
                UserReportTypeTreeNode addedNode = new UserReportTypeTreeNode(typeName, true);
                customReportTreeView.Nodes.Add(addedNode);
                customReportTreeView.SelectedNode = addedNode;
                saveButton.Enabled = true;
            }
        }

        private void newGroupClick(object sender, EventArgs e)
        {
            frmInputBox newGroup = new frmInputBox(Resources.infoNewGroupPrompt, Resources.infoNewGroupTitle, Resources.infoNewGroupDefaultValue);
            newGroup.ShowDialog();
            if (newGroup.DialogResult == DialogResult.OK)
            {
                UserReportGroupTreeNode addedNode = new UserReportGroupTreeNode(newGroup.InputValue, true);
                customReportTreeView.SelectedNode.Nodes.Add(addedNode);
                customReportTreeView.SelectedNode.Expand();
                customReportTreeView.SelectedNode = addedNode;
                saveButton.Enabled = true;
            }
        }

        private static bool m_busyCheckCustom;
        private void customReportTreeView_AfterCheck(object sender, TreeViewEventArgs args)
        {
            if (m_busyCheckCustom)
                return;
            m_busyCheckCustom = true;
            SetChildrenCheckFlag(args.Node);
            SetParentCheckFlag(args.Node.Parent);
            args.Node.Expand();
            m_busyCheckCustom = false;
            SetDeleteEnable();
        }
        /// <summary>
        /// Sets the Check flag for all children of the specified TreeNode.
        /// </summary>
        /// <param name="node"></param>
        private static void SetChildrenCheckFlag(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Checked != node.Checked)
                {
                    child.Checked = node.Checked;
                    SetChildrenCheckFlag(child);
                }
            }
            node.Expand();
        }
        /// <summary>
        /// Sets the Check flag for all parents of the specified TreeNode.
        /// </summary>
        /// <param name="parent"></param>
        private static void SetParentCheckFlag(TreeNode parent)
        {
            if (parent != null)
            {
                bool parentFlag = true;

                foreach (TreeNode sibling in parent.Nodes)
                {
                    if (sibling.Checked == false)
                    {
                        parentFlag = false;
                        break;
                    }
                }
                if (parent.Checked != parentFlag)
                    parent.Checked = parentFlag;
                SetParentCheckFlag(parent.Parent);
            }
        }
        /// <summary>
        /// Delete all check items in the customReportTreeView
        /// </summary>
        /// <param name="sender">delete button</param>
        /// <param name="e">event args</param>
        private void DeleteClick(object sender, EventArgs e)
        {
            //try
            //{

            //    Debug.WriteLine("Custom Delete");
            //    foreach (UserReportTypeTreeNode typeNode in customReportTreeView.Nodes)
            //    {
            //        int typeId = typeNode.ReportType.UserReportTypeID;
            //        UserReportType reportType;
            //        m_userReportDictionary.TryGetValue(typeId, out reportType);
            //        Debug.WriteLine("Type : " + typeNode.ReportType.UserReportTypeName + " ID:" + typeNode.ReportType.UserReportTypeID + " Checked : " + typeNode.Checked);
            //        foreach (UserReportGroupTreeNode groupNode in typeNode.Nodes)
            //        {
            //            int groupId = groupNode.ReportGroup.UserGroupID;
            //            UserReportGroup reportGroup = null;
            //            if (reportType != null)
            //                reportType.UserReportGroups.TryGetValue(groupId, out reportGroup);

            //            Debug.WriteLine("  Group : " + groupNode.ReportGroup.UserGroupName + " ID:" + groupNode.ReportGroup.UserGroupID + " Checked : " + groupNode.Checked);
            //            foreach (ReportSetTreeNode setNode in groupNode.Nodes)
            //            {
            //                Debug.WriteLine("    Set : " + setNode.Text + " Checked : " + setNode.Checked);
            //                foreach (ReportTreeNode reportNode in setNode.Nodes)
            //                {
            //                    Debug.WriteLine("      Report : " + reportNode.ReportInfo.DisplayName + " ID:" + reportNode.ReportInfo.ID + " Checked : " + reportNode.Checked);
            //                    if (reportNode.Checked)
            //                    {
            //                        if (reportGroup != null && reportNode.IsNewNode == false)
            //                        {
            //                            for (int iReport = 0; iReport < reportGroup.ReportsArray.Count; iReport++)
            //                            {
            //                                ReportInfo reportInfo = (ReportInfo)reportGroup.ReportsArray[iReport];
            //                                if (reportInfo.ID == reportNode.ReportInfo.ID)
            //                                {
            //                                    reportInfo.RemoveType = 1;
            //                                    saveButton.Enabled = true;
            //                                    break;
            //                                }
            //                            }
            //                        }
            //                        // DE6919
            //                        if(reportNode != null)
            //                            customReportTreeView.Nodes.Remove(reportNode);
            //                    }
            //                }

            //                if (setNode.Checked)
            //                {
            //                    customReportTreeView.Nodes.Remove(setNode); 
            //                }
            //            }
            //            if (groupNode.Checked)
            //            {
            //                if (reportGroup != null && groupNode.IsNewNode == false)
            //                {
            //                    reportGroup.RemoveType = 1;
            //                    saveButton.Enabled = true;
            //                }
            //                customReportTreeView.Nodes.Remove(groupNode);
            //            }
            //        }

            //        if (typeNode.Checked)
            //        {
            //            if (reportType != null && typeNode.IsNewNode == false)
            //            {
            //                reportType.RemoveType = 1;
            //                saveButton.Enabled = true;
            //            }
            //            customReportTreeView.Nodes.Remove(typeNode);
            //        }
            //    }


            // DE6919: deletion must be recursive.
            try
            {
                // Start at top and delete children first!
                if ( customReportTreeView == null || customReportTreeView.Nodes == null) return;

                for (int i = 0; i < customReportTreeView.Nodes.Count; i++)
                {
                    DeleteNode(customReportTreeView.Nodes[i]);
                }

                ShowUserReportDictionary();
                saveReportsClick(null, null); //RALLY DE 6239
                SetDeleteEnable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to delete custom reports: " + ex.Message, Application.ProductName);
            }
        }

        /// <summary>
        /// DE6919: deleting nodes MUST begin at leafs then move up to root...
        /// WARNING: Recursive method.  Stops when at top of tree.
        /// </summary>
        /// <param name="tn"></param>
        private void DeleteNode(TreeNode node)
        {
            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                DeleteNode(node.Nodes[i]);
            }

            // Only remove if requested!
            if (node.Checked)
            {
                int typeId = 0, groupId = 0;
                UserReportType reportType;
                UserReportGroup reportGroup;
                ReportTreeNode reportNode;

                switch (node.Level)
                {
                    case 0: // Top level Types
                        typeId = (node as UserReportTypeTreeNode).ReportType.UserReportTypeID;
                        m_userReportDictionary.TryGetValue(typeId, out reportType);
                        if (reportType == null) break;  // de6919

                        reportType.RemoveType = 1;
                        break;

                    case 1:  // Groups
                        typeId = (node.Parent as UserReportTypeTreeNode).ReportType.UserReportTypeID;
                        m_userReportDictionary.TryGetValue(typeId, out reportType);
                        if (reportType == null) break;  // de6919

                        groupId = (node as UserReportGroupTreeNode).ReportGroup.UserGroupID;
                        reportType.UserReportGroups.TryGetValue(groupId, out reportGroup);
                        if (reportGroup == null) break;  // de6919

                        reportGroup.RemoveType = 1;
                        break;

                    case 3:  // Leaf/children, the reports themselves
                        typeId = (node.Parent.Parent.Parent as UserReportTypeTreeNode).ReportType.UserReportTypeID;
                        m_userReportDictionary.TryGetValue(typeId, out reportType);
                        if (reportType == null) break;  // de6919

                        groupId = (node.Parent.Parent as UserReportGroupTreeNode).ReportGroup.UserGroupID;
                        reportType.UserReportGroups.TryGetValue(groupId, out reportGroup);
                        if (reportGroup == null) break;  // de6919

                        reportNode = (node as ReportTreeNode);
                        foreach (ReportInfo ri in reportGroup.ReportsArray)
                        {
                            if (ri.ID == reportNode.ReportInfo.ID)
                            {
                                ri.RemoveType = 1;
                                saveButton.Enabled = true;
                                break;
                            }
                        }
                        break;

                    default:
                        break;
                }
                customReportTreeView.Nodes.Remove(node);
            }
        }

        private void DeleteNode2(TreeNode node)
        {
            TreeNode parent = node.Parent;
            //if (parent == null) return;

            int nodeCount = node.Nodes.Count;
            TreeNode[] nodes = new TreeNode[nodeCount];

            if (node.Nodes.Count != 0)
            {
                for (int i = 0; i < nodeCount; i++)
                {
                    nodes[i] = node.Nodes[i];
                }
            }
            for (int i = 0; i < nodeCount; i++)
            {
                DeleteNode(nodes[i]);
            }

            // Only remove if requested!
            if (node.Checked)
                node.Remove();
        }


        int m_tmpTypeId = -1;
        int m_tmpGroupId = -1;
        private void SaveTree()
        {
            foreach (UserReportTypeTreeNode typeNode in customReportTreeView.Nodes)
            {
                int typeId = typeNode.ReportType.UserReportTypeID;
                UserReportType reportType;
                m_userReportDictionary.TryGetValue(typeId, out reportType);
                if (reportType == null)
                {
                    reportType = new UserReportType(m_tmpTypeId, typeNode.ReportType.UserReportTypeName);
                    m_userReportDictionary.Add(m_tmpTypeId, reportType);
                    m_tmpTypeId--;
                }

                foreach (UserReportGroupTreeNode groupNode in typeNode.Nodes)
                {
                    int groupId = groupNode.ReportGroup.UserGroupID;
                    UserReportGroup group;
                    reportType.UserReportGroups.TryGetValue(groupId, out group);
                    if (group == null)
                    {
                        group = new UserReportGroup(m_tmpGroupId, groupNode.ReportGroup.UserGroupName);
                        reportType.UserReportGroups.Add(m_tmpGroupId, group);
                        m_tmpGroupId--;
                    }

                    foreach (ReportSetTreeNode setNode in groupNode.Nodes)
                    {
                        foreach (ReportTreeNode reportNode in setNode.Nodes)
                        {
                            if (reportNode.IsNewNode)
                            {
                                group.ReportsArray.Add(reportNode.ReportInfo);
                            }
                        }
                    }
                }
            }
        }

        private void saveReportsClick(object sender, EventArgs e)
        {
            saveButton.Enabled = false;
            SaveTree();

            foreach (KeyValuePair<int, UserReportType> reportType in m_userReportDictionary)
            {
                // reset the id's for new entries
                if (reportType.Value.UserReportTypeID < 0)
                {
                    reportType.Value.UserReportTypeID = 0;
                }
                foreach (KeyValuePair<int, UserReportGroup> reportGroup in reportType.Value.UserReportGroups)
                {
                    if (reportGroup.Value.UserGroupID < 0)
                    {
                        reportGroup.Value.UserGroupID = 0;
                    }
                }
            }

            Debug.WriteLine("Save Custom Reports");
            ShowUserReportDictionary();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SetUserReportList saveReport = new SetUserReportList();
                string locale = "en-US";
                if (Configuration.mForceEnglish == false)
                {
                    locale = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                }
                saveReport.Locale = locale;
                saveReport.OperatorID = Configuration.operatorID;
                saveReport.StaffID = Configuration.LoginStaffID;

                foreach (KeyValuePair<int, UserReportType> type in m_userReportDictionary)
                {
                    saveReport.ReportType = type.Value;
                    saveReport.Send();
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageForm.Show(this, Resources.errorFailedToSaveToServer + "..Save Reports " + ex.Message, Resources.report_center);
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                SetDeleteEnable();
                Debug.WriteLine("Save Custom Reports completed");
                LoadPredefinedReports();//RALLY DE 6239
                LoadCustomReports();//RALLY DE 6239
                saveButton.Enabled = false;
            }
        }

        private void ShowUserReportDictionary()
        {
            Debug.WriteLine("================ User Report Dictionary =====================");
            foreach (KeyValuePair<int, UserReportType> reportType in m_userReportDictionary)
            {
                Debug.Write("  TYPE : " + reportType.Value.UserReportTypeName + " ID:" + reportType.Value.UserReportTypeID);
                Debug.WriteLine(reportType.Value.RemoveType==1?" Remove": "");
                foreach (KeyValuePair<int, UserReportGroup> group in reportType.Value.UserReportGroups)
                {
                    Debug.Write("    GROUP : " + group.Value.UserGroupName + " ID:" + group.Value.UserGroupID);
                    Debug.WriteLine(group.Value.RemoveType == 1 ? " Remove" : "");
                    for (int iReport = 0; iReport < group.Value.ReportsArray.Count; iReport++)
                    {
                        ReportInfo report = (ReportInfo)group.Value.ReportsArray[iReport];
                        Debug.Write("      REPORT : " + report.DisplayName + " ID:" + report.ID);
                        Debug.WriteLine(report.RemoveType == 1 ? " Remove" : "");
                    }
                }
            }
        }
        #endregion
    }
    // END : DE3252 Customized reports not working (entire source file updated)
}