namespace GTI.Modules.ReportCenter.UI
{
    partial class frmReportCenterMDIParent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReportCenterMDIParent));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.standardReportsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.customReportsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.michiganQuarterlyReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cashAccountabilityReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReportManagerMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(222)))), ((int)(((byte)(237)))));
            this.menuStrip.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.reportsMenu,
            this.helpMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip.Size = new System.Drawing.Size(1018, 32);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenu});
            this.fileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(49, 26);
            this.fileMenu.Text = "&File";
            // 
            // exitMenu
            // 
            this.exitMenu.Name = "exitMenu";
            this.exitMenu.Size = new System.Drawing.Size(108, 26);
            this.exitMenu.Text = "E&xit";
            this.exitMenu.Click += new System.EventHandler(this.ExitMenu_Click);
            // 
            // reportsMenu
            // 
            this.reportsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.standardReportsMenu,
            this.customReportsMenu,
            this.michiganQuarterlyReportToolStripMenuItem,
            this.cashAccountabilityReportToolStripMenuItem,
            this.ReportManagerMenu});
            this.reportsMenu.Name = "reportsMenu";
            this.reportsMenu.Size = new System.Drawing.Size(80, 26);
            this.reportsMenu.Text = "&Reports";
            // 
            // standardReportsMenu
            // 
            this.standardReportsMenu.Name = "standardReportsMenu";
            this.standardReportsMenu.Size = new System.Drawing.Size(285, 26);
            this.standardReportsMenu.Text = "&Standard Reports";
            this.standardReportsMenu.Click += new System.EventHandler(this.standardReportsMenu_Click);
            // 
            // customReportsMenu
            // 
            this.customReportsMenu.Name = "customReportsMenu";
            this.customReportsMenu.Size = new System.Drawing.Size(285, 26);
            this.customReportsMenu.Text = "&Custom Reports";
            this.customReportsMenu.Click += new System.EventHandler(this.customizedReportsMenu_Click);
            // 
            // michiganQuarterlyReportToolStripMenuItem
            // 
            this.michiganQuarterlyReportToolStripMenuItem.Name = "michiganQuarterlyReportToolStripMenuItem";
            this.michiganQuarterlyReportToolStripMenuItem.Size = new System.Drawing.Size(285, 26);
            this.michiganQuarterlyReportToolStripMenuItem.Text = "&Quarterly Report";
            this.michiganQuarterlyReportToolStripMenuItem.Click += new System.EventHandler(this.michiganQuarterlyReportToolStripMenuItem_Click);
            // 
            // cashAccountabilityReportToolStripMenuItem
            // 
            this.cashAccountabilityReportToolStripMenuItem.Name = "cashAccountabilityReportToolStripMenuItem";
            this.cashAccountabilityReportToolStripMenuItem.Size = new System.Drawing.Size(285, 26);
            this.cashAccountabilityReportToolStripMenuItem.Text = "Cash &Accountability Report";
            this.cashAccountabilityReportToolStripMenuItem.Visible = false;
            this.cashAccountabilityReportToolStripMenuItem.Click += new System.EventHandler(this.cashAccountabilityReportToolStripMenuItem_Click);
            // 
            // ReportManagerMenu
            // 
            this.ReportManagerMenu.Name = "ReportManagerMenu";
            this.ReportManagerMenu.Size = new System.Drawing.Size(285, 26);
            this.ReportManagerMenu.Text = "Report &Manager";
            this.ReportManagerMenu.Visible = false;
            this.ReportManagerMenu.Click += new System.EventHandler(this.ReportManagerMenu_Click);
            // 
            // helpMenu
            // 
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMenu});
            this.helpMenu.Name = "helpMenu";
            this.helpMenu.Size = new System.Drawing.Size(55, 26);
            this.helpMenu.Text = "&Help";
            // 
            // aboutMenu
            // 
            this.aboutMenu.Name = "aboutMenu";
            this.aboutMenu.Size = new System.Drawing.Size(148, 26);
            this.aboutMenu.Text = "&About ...";
            this.aboutMenu.Click += new System.EventHandler(this.aboutMenu_Click);
            // 
            // frmReportCenterMDIParent
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1018, 678);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "frmReportCenterMDIParent";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Center -- GameTech International Inc.";
            this.Load += new System.EventHandler(this.ReportCenterMDIParent_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;

        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem exitMenu;
        private System.Windows.Forms.ToolStripMenuItem helpMenu;
        private System.Windows.Forms.ToolStripMenuItem aboutMenu;

        private System.Windows.Forms.ToolStripMenuItem reportsMenu;
        private System.Windows.Forms.ToolStripMenuItem standardReportsMenu;
        private System.Windows.Forms.ToolStripMenuItem customReportsMenu;
        private System.Windows.Forms.ToolStripMenuItem michiganQuarterlyReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cashAccountabilityReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReportManagerMenu;
    }
}



