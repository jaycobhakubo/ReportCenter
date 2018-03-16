namespace GTI.Modules.ReportCenter.UI
{
    partial class frmEditReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditReport));
            this.dgReportList = new System.Windows.Forms.DataGridView();
            this.ReportId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ReportDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReportType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReportFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new GTI.Controls.ImageButton();
            this.btnSaveReportEdit = new GTI.Controls.ImageButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.dgReportList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgReportList
            // 
            this.dgReportList.AllowUserToAddRows = false;
            this.dgReportList.AllowUserToDeleteRows = false;
            this.dgReportList.AllowUserToOrderColumns = true;
            this.dgReportList.AllowUserToResizeColumns = false;
            this.dgReportList.AllowUserToResizeRows = false;
            this.dgReportList.BackgroundColor = System.Drawing.Color.White;
            this.dgReportList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgReportList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dgReportList, "dgReportList");
            this.dgReportList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgReportList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReportId,
            this.IsActive,
            this.ReportDisplayName,
            this.ReportType,
            this.ReportFileName});
            this.dgReportList.MultiSelect = false;
            this.dgReportList.Name = "dgReportList";
            this.dgReportList.RowHeadersVisible = false;
            this.dgReportList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgReportList_CellClick);
            this.dgReportList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgReportList_CellValidating);
            this.dgReportList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgReportList_CellValueChanged);
            this.dgReportList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgReportList_ColumnHeaderMouseClick);
            // 
            // ReportId
            // 
            this.ReportId.DataPropertyName = "ReportId";
            resources.ApplyResources(this.ReportId, "ReportId");
            this.ReportId.Name = "ReportId";
            this.ReportId.ReadOnly = true;
            this.ReportId.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ReportId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            resources.ApplyResources(this.IsActive, "IsActive");
            this.IsActive.Name = "IsActive";
            this.IsActive.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ReportDisplayName
            // 
            this.ReportDisplayName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ReportDisplayName.DataPropertyName = "ReportDisplayName";
            resources.ApplyResources(this.ReportDisplayName, "ReportDisplayName");
            this.ReportDisplayName.Name = "ReportDisplayName";
            this.ReportDisplayName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ReportType
            // 
            this.ReportType.DataPropertyName = "ReportType";
            resources.ApplyResources(this.ReportType, "ReportType");
            this.ReportType.Name = "ReportType";
            this.ReportType.ReadOnly = true;
            this.ReportType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ReportFileName
            // 
            this.ReportFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ReportFileName.DataPropertyName = "ReportFileName";
            resources.ApplyResources(this.ReportFileName, "ReportFileName");
            this.ReportFileName.Name = "ReportFileName";
            this.ReportFileName.ReadOnly = true;
            this.ReportFileName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ReportFileName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.FocusColor = System.Drawing.Color.Black;
            this.btnCancel.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageNormal")));
            this.btnCancel.ImagePressed = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImagePressed")));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.SecondaryTextPadding = new System.Windows.Forms.Padding(5);
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSaveReportEdit
            // 
            this.btnSaveReportEdit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnSaveReportEdit, "btnSaveReportEdit");
            this.btnSaveReportEdit.FocusColor = System.Drawing.Color.Black;
            this.btnSaveReportEdit.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnSaveReportEdit.ImageNormal")));
            this.btnSaveReportEdit.ImagePressed = ((System.Drawing.Image)(resources.GetObject("btnSaveReportEdit.ImagePressed")));
            this.btnSaveReportEdit.Name = "btnSaveReportEdit";
            this.btnSaveReportEdit.SecondaryTextPadding = new System.Windows.Forms.Padding(5);
            this.btnSaveReportEdit.UseVisualStyleBackColor = false;
            this.btnSaveReportEdit.Click += new System.EventHandler(this.btnSaveReportEdit_Click);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgReportList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnSaveReportEdit);
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            // 
            // frmEditReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frmEditReport";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.dgReportList)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgReportList;
        private Controls.ImageButton btnCancel;
        private Controls.ImageButton btnSaveReportEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportDisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportFileName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;

    }
}