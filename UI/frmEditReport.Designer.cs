﻿namespace GTI.Modules.ReportCenter.UI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditReport));
            this.dgReportList = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new GTI.Controls.ImageButton();
            this.btnSaveReportEdit = new GTI.Controls.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgReportList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgReportList
            // 
            this.dgReportList.AllowUserToAddRows = false;
            this.dgReportList.AllowUserToDeleteRows = false;
            this.dgReportList.AllowUserToResizeColumns = false;
            this.dgReportList.AllowUserToResizeRows = false;
            this.dgReportList.BackgroundColor = System.Drawing.Color.White;
            this.dgReportList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            resources.ApplyResources(this.dgReportList, "dgReportList");
            this.dgReportList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgReportList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.DisplayName,
            this.FileName});
            this.dgReportList.MultiSelect = false;
            this.dgReportList.Name = "dgReportList";
            this.dgReportList.RowHeadersVisible = false;
            this.dgReportList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgReportList_CellValueChanged);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            resources.ApplyResources(this.ID, "ID");
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // DisplayName
            // 
            this.DisplayName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DisplayName.DataPropertyName = "DisplayName";
            resources.ApplyResources(this.DisplayName, "DisplayName");
            this.DisplayName.Name = "DisplayName";
            // 
            // FileName
            // 
            this.FileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.FileName.DataPropertyName = "FileName";
            resources.ApplyResources(this.FileName, "FileName");
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FocusColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageNormal")));
            this.btnCancel.ImagePressed = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImagePressed")));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.SecondaryTextPadding = new System.Windows.Forms.Padding(5);
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSaveReportEdit
            // 
            this.btnSaveReportEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveReportEdit.FocusColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btnSaveReportEdit, "btnSaveReportEdit");
            this.btnSaveReportEdit.ImageNormal = ((System.Drawing.Image)(resources.GetObject("btnSaveReportEdit.ImageNormal")));
            this.btnSaveReportEdit.ImagePressed = ((System.Drawing.Image)(resources.GetObject("btnSaveReportEdit.ImagePressed")));
            this.btnSaveReportEdit.Name = "btnSaveReportEdit";
            this.btnSaveReportEdit.SecondaryTextPadding = new System.Windows.Forms.Padding(5);
            this.btnSaveReportEdit.UseVisualStyleBackColor = false;
            this.btnSaveReportEdit.Click += new System.EventHandler(this.btnSaveReportEdit_Click);
            // 
            // frmEditReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveReportEdit);
            this.Controls.Add(this.dgReportList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frmEditReport";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.frmEditReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgReportList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgReportList;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private Controls.ImageButton btnCancel;
        private Controls.ImageButton btnSaveReportEdit;

    }
}