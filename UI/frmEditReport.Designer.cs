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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditReport));
            this.dgReportList = new System.Windows.Forms.DataGridView();
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
            this.dgReportList.MultiSelect = false;
            this.dgReportList.Name = "dgReportList";
            this.dgReportList.RowHeadersVisible = false;
            // 
            // frmEditReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
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

        public System.Windows.Forms.DataGridView dgReportList;


    }
}