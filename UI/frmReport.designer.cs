

namespace GTI.Modules.ReportCenter.UI
{
	partial class frmReport
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

        private CrystalDecisions.Windows.Forms.CrystalReportViewer reportViewer;
        private GTI.Controls.ImageButton closeImageButton;
        private GTI.Controls.ImageButton printImageButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;

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
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReport));
                this.reportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
                this.closeImageButton = new GTI.Controls.ImageButton();
                this.printImageButton = new GTI.Controls.ImageButton();
                this.menuStrip1 = new System.Windows.Forms.MenuStrip();
                this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.drillDownDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.menuStrip1.SuspendLayout();
                this.SuspendLayout();
                // 
                // reportViewer
                // 
                this.reportViewer.ActiveViewIndex = -1;
                resources.ApplyResources(this.reportViewer, "reportViewer");
                this.reportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.reportViewer.Cursor = System.Windows.Forms.Cursors.Default;
                this.reportViewer.Name = "reportViewer";
                this.reportViewer.SelectionFormula = "";
                this.reportViewer.ViewTimeSelectionFormula = "";
                // 
                // closeImageButton
                // 
                this.closeImageButton.BackColor = System.Drawing.Color.Transparent;
                this.closeImageButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.closeImageButton.FocusColor = System.Drawing.Color.Black;
                resources.ApplyResources(this.closeImageButton, "closeImageButton");
                this.closeImageButton.ImageNormal = ((System.Drawing.Image)(resources.GetObject("closeImageButton.ImageNormal")));
                this.closeImageButton.ImagePressed = ((System.Drawing.Image)(resources.GetObject("closeImageButton.ImagePressed")));
                this.closeImageButton.MinimumSize = new System.Drawing.Size(30, 30);
                this.closeImageButton.Name = "closeImageButton";
                this.closeImageButton.UseVisualStyleBackColor = false;
                this.closeImageButton.Click += new System.EventHandler(this.closeImageButton_Click);
                // 
                // printImageButton
                // 
                this.printImageButton.BackColor = System.Drawing.Color.Transparent;
                this.printImageButton.FocusColor = System.Drawing.Color.Black;
                resources.ApplyResources(this.printImageButton, "printImageButton");
                this.printImageButton.ImageNormal = ((System.Drawing.Image)(resources.GetObject("printImageButton.ImageNormal")));
                this.printImageButton.ImagePressed = ((System.Drawing.Image)(resources.GetObject("printImageButton.ImagePressed")));
                this.printImageButton.MinimumSize = new System.Drawing.Size(30, 30);
                this.printImageButton.Name = "printImageButton";
                this.printImageButton.UseVisualStyleBackColor = false;
                // 
                // menuStrip1
                // 
                this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(222)))), ((int)(((byte)(237)))));
                resources.ApplyResources(this.menuStrip1, "menuStrip1");
                this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.closeToolStripMenuItem});
                this.menuStrip1.Name = "menuStrip1";
                // 
                // printToolStripMenuItem
                // 
                this.printToolStripMenuItem.Name = "printToolStripMenuItem";
                resources.ApplyResources(this.printToolStripMenuItem, "printToolStripMenuItem");
                this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
                // 
                // viewToolStripMenuItem
                // 
                this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drillDownDataToolStripMenuItem});
                this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
                resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
                // 
                // drillDownDataToolStripMenuItem
                // 
                this.drillDownDataToolStripMenuItem.Name = "drillDownDataToolStripMenuItem";
                resources.ApplyResources(this.drillDownDataToolStripMenuItem, "drillDownDataToolStripMenuItem");
                this.drillDownDataToolStripMenuItem.Click += new System.EventHandler(this.drillDownDataToolStripMenuItem_Click);
                // 
                // closeToolStripMenuItem
                // 
                this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
                resources.ApplyResources(this.closeToolStripMenuItem, "closeToolStripMenuItem");
                this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeImageButton_Click);
                // 
                // frmReport
                // 
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
                resources.ApplyResources(this, "$this");
                this.CancelButton = this.closeImageButton;
                this.Controls.Add(this.printImageButton);
                this.Controls.Add(this.closeImageButton);
                this.Controls.Add(this.reportViewer);
                this.Controls.Add(this.menuStrip1);
                this.DoubleBuffered = true;
                this.MainMenuStrip = this.menuStrip1;
                this.Name = "frmReport";
                this.ShowIcon = false;
                this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
                this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReport_FormClosing);
                this.menuStrip1.ResumeLayout(false);
                this.menuStrip1.PerformLayout();
                this.ResumeLayout(false);
                this.PerformLayout();

			}

		#endregion

        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drillDownDataToolStripMenuItem;

       
		}
	}