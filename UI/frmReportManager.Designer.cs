namespace GTI.Modules.ReportCenter.UI
{
    partial class FrmReportManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReportManager));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.reportTreeView = new System.Windows.Forms.TreeView();
            this.groupBoxParams = new System.Windows.Forms.GroupBox();
            this.tablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnPreview = new GTI.Controls.ImageButton();
            this.btnPrint = new GTI.Controls.ImageButton();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxParams.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxParams);
            this.splitContainer1.Panel2.Controls.Add(this.panelButtons);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.reportTreeView);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // reportTreeView
            // 
            resources.ApplyResources(this.reportTreeView, "reportTreeView");
            this.reportTreeView.Name = "reportTreeView";
            this.reportTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.ReportTreeViewAfterCheck);
            this.reportTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.reportTreeView_BeforeSelect);
            this.reportTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.reportTreeView_NodeMouseClick);
            // 
            // groupBoxParams
            // 
            resources.ApplyResources(this.groupBoxParams, "groupBoxParams");
            this.groupBoxParams.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxParams.Controls.Add(this.tablePanel);
            this.groupBoxParams.Name = "groupBoxParams";
            this.groupBoxParams.TabStop = false;
            // 
            // tablePanel
            // 
            resources.ApplyResources(this.tablePanel, "tablePanel");
            this.tablePanel.Name = "tablePanel";
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnPreview);
            this.panelButtons.Controls.Add(this.btnPrint);
            resources.ApplyResources(this.panelButtons, "panelButtons");
            this.panelButtons.Name = "panelButtons";
            // 
            // btnPreview
            // 
            resources.ApplyResources(this.btnPreview, "btnPreview");
            this.btnPreview.BackColor = System.Drawing.Color.Transparent;
            this.btnPreview.FocusColor = System.Drawing.Color.Black;
            this.btnPreview.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnPreview.ImageNormal")));
            this.btnPreview.ImagePressed = ((System.Drawing.Image)(resources.GetObject("btnPreview.ImagePressed")));
            this.btnPreview.MinimumSize = new System.Drawing.Size(30, 30);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.UseVisualStyleBackColor = false;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FocusColor = System.Drawing.Color.Black;
            this.btnPrint.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnPrint.ImageNormal")));
            this.btnPrint.ImagePressed = ((System.Drawing.Image)(resources.GetObject("btnPrint.ImagePressed")));
            this.btnPrint.MinimumSize = new System.Drawing.Size(30, 30);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // FrmReportManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(179)))), ((int)(((byte)(213)))));
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmReportManager";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.ReportManager_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxParams.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView reportTreeView;
        private System.Windows.Forms.GroupBox groupBoxParams;
        private Controls.ImageButton btnPrint;
        private Controls.ImageButton btnPreview;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.TableLayoutPanel tablePanel;
        private System.Windows.Forms.Panel panel1;
    }
}