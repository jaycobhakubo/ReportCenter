namespace GTI.Modules.ReportCenter.UI
{
    partial class frmCustomizeReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TreeView predefinedReportTreeView;
        private System.Windows.Forms.Label predefinedReportlabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView customReportTreeView;
        private GTI.Controls.ImageButton newTypeButton;
        private GTI.Controls.ImageButton newGroupButton;
        private GTI.Controls.ImageButton saveButton;
        private GTI.Controls.ImageButton addReportButton;
        private GTI.Controls.ImageButton deleteButton;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomizeReport));
            this.predefinedReportTreeView = new System.Windows.Forms.TreeView();
            this.predefinedReportlabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.customReportTreeView = new System.Windows.Forms.TreeView();
            this.newTypeButton = new GTI.Controls.ImageButton();
            this.newGroupButton = new GTI.Controls.ImageButton();
            this.saveButton = new GTI.Controls.ImageButton();
            this.addReportButton = new GTI.Controls.ImageButton();
            this.deleteButton = new GTI.Controls.ImageButton();
            this.SuspendLayout();
            // 
            // predefinedReportTreeView
            // 
            this.predefinedReportTreeView.CheckBoxes = true;
            resources.ApplyResources(this.predefinedReportTreeView, "predefinedReportTreeView");
            this.predefinedReportTreeView.Name = "predefinedReportTreeView";
            this.predefinedReportTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.predefinedReportTreeView_AfterCheck);
            // 
            // predefinedReportlabel
            // 
            resources.ApplyResources(this.predefinedReportlabel, "predefinedReportlabel");
            this.predefinedReportlabel.BackColor = System.Drawing.Color.Transparent;
            this.predefinedReportlabel.Name = "predefinedReportlabel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // customReportTreeView
            // 
            this.customReportTreeView.CheckBoxes = true;
            resources.ApplyResources(this.customReportTreeView, "customReportTreeView");
            this.customReportTreeView.HideSelection = false;
            this.customReportTreeView.LabelEdit = true;
            this.customReportTreeView.Name = "customReportTreeView";
            this.customReportTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.customReportTreeView_AfterCheck);
            this.customReportTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.customReportLabelEdit);
            this.customReportTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.customReportSelect);
            this.customReportTreeView.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.customReportTreeView_BeforeLabelEdit);
            // 
            // newTypeButton
            // 
            this.newTypeButton.BackColor = System.Drawing.Color.Transparent;
            this.newTypeButton.FocusColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.newTypeButton, "newTypeButton");
            this.newTypeButton.ImageNormal = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonUp;
            this.newTypeButton.ImagePressed = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonDown;
            this.newTypeButton.MinimumSize = new System.Drawing.Size(30, 30);
            this.newTypeButton.Name = "newTypeButton";
            this.newTypeButton.UseVisualStyleBackColor = false;
            this.newTypeButton.Click += new System.EventHandler(this.newTypeClick);
            // 
            // newGroupButton
            // 
            this.newGroupButton.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.newGroupButton, "newGroupButton");
            this.newGroupButton.FocusColor = System.Drawing.Color.Black;
            this.newGroupButton.ImageNormal = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonUp;
            this.newGroupButton.ImagePressed = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonDown;
            this.newGroupButton.MinimumSize = new System.Drawing.Size(30, 30);
            this.newGroupButton.Name = "newGroupButton";
            this.newGroupButton.UseVisualStyleBackColor = false;
            this.newGroupButton.Click += new System.EventHandler(this.newGroupClick);
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.Transparent;
            this.saveButton.FocusColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.ImageNormal = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonUp;
            this.saveButton.ImagePressed = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonDown;
            this.saveButton.MinimumSize = new System.Drawing.Size(30, 30);
            this.saveButton.Name = "saveButton";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveReportsClick);
            // 
            // addReportButton
            // 
            this.addReportButton.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.addReportButton, "addReportButton");
            this.addReportButton.FocusColor = System.Drawing.Color.Black;
            this.addReportButton.ImageNormal = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonUp;
            this.addReportButton.ImagePressed = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonDown;
            this.addReportButton.MinimumSize = new System.Drawing.Size(30, 30);
            this.addReportButton.Name = "addReportButton";
            this.addReportButton.UseVisualStyleBackColor = false;
            this.addReportButton.Click += new System.EventHandler(this.addCheckedReportClick);
            // 
            // deleteButton
            // 
            this.deleteButton.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.deleteButton, "deleteButton");
            this.deleteButton.FocusColor = System.Drawing.Color.Black;
            this.deleteButton.ImageNormal = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonUp;
            this.deleteButton.ImagePressed = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonDown;
            this.deleteButton.MinimumSize = new System.Drawing.Size(30, 30);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.DeleteClick);
            // 
            // frmCustomizeReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addReportButton);
            this.Controls.Add(this.newTypeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.newGroupButton);
            this.Controls.Add(this.customReportTreeView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.predefinedReportlabel);
            this.Controls.Add(this.predefinedReportTreeView);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frmCustomizeReport";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.CustomizeReport_Load);
            this.Enter += new System.EventHandler(this.frmCustomizeReport_Enter);
            this.Leave += new System.EventHandler(this.frmCustomizeReport_Leave);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCustomizeReport_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
    }
}