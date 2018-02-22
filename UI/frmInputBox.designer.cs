

namespace GTI.Modules.ReportCenter.UI
{
    partial class frmInputBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label promptLabel;
        private System.Windows.Forms.TextBox inputTextBox;
        private GTI.Controls.ImageButton cancelImageButton;
        private GTI.Controls.ImageButton okImageButton;

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
            this.promptLabel = new System.Windows.Forms.Label();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.cancelImageButton = new GTI.Controls.ImageButton();
            this.okImageButton = new GTI.Controls.ImageButton();
            this.labelMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // promptLabel
            // 
            this.promptLabel.BackColor = System.Drawing.Color.Transparent;
            this.promptLabel.Location = new System.Drawing.Point(55, 9);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(343, 53);
            this.promptLabel.TabIndex = 3;
            this.promptLabel.Text = "Prompt";
            this.promptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inputTextBox
            // 
            this.inputTextBox.AcceptsReturn = true;
            this.inputTextBox.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputTextBox.Location = new System.Drawing.Point(59, 76);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(339, 26);
            this.inputTextBox.TabIndex = 0;
            this.inputTextBox.TextChanged += new System.EventHandler(this.inputTextBox_TextChanged);
            this.inputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputTextBox_KeyDown);
            // 
            // cancelImageButton
            // 
            this.cancelImageButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelImageButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelImageButton.FocusColor = System.Drawing.Color.Black;
            this.cancelImageButton.Font = new System.Drawing.Font("Trebuchet MS", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelImageButton.ImageNormal = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonUp;
            this.cancelImageButton.ImagePressed = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonDown;
            this.cancelImageButton.Location = new System.Drawing.Point(304, 121);
            this.cancelImageButton.MinimumSize = new System.Drawing.Size(30, 30);
            this.cancelImageButton.Name = "cancelImageButton";
            this.cancelImageButton.Size = new System.Drawing.Size(94, 30);
            this.cancelImageButton.TabIndex = 2;
            this.cancelImageButton.Text = "&Cancel";
            this.cancelImageButton.UseVisualStyleBackColor = false;
            // 
            // okImageButton
            // 
            this.okImageButton.BackColor = System.Drawing.Color.Transparent;
            this.okImageButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okImageButton.FocusColor = System.Drawing.Color.Black;
            this.okImageButton.Font = new System.Drawing.Font("Trebuchet MS", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okImageButton.ImageNormal = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonUp;
            this.okImageButton.ImagePressed = global::GTI.Modules.ReportCenter.Properties.Resources.BlueButtonDown;
            this.okImageButton.Location = new System.Drawing.Point(59, 121);
            this.okImageButton.MinimumSize = new System.Drawing.Size(30, 30);
            this.okImageButton.Name = "okImageButton";
            this.okImageButton.Size = new System.Drawing.Size(94, 30);
            this.okImageButton.TabIndex = 1;
            this.okImageButton.Text = "&OK";
            this.okImageButton.UseVisualStyleBackColor = false;
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.Color.Transparent;
            this.labelMessage.Location = new System.Drawing.Point(59, 166);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(339, 27);
            this.labelMessage.TabIndex = 4;
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 202);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.okImageButton);
            this.Controls.Add(this.cancelImageButton);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.promptLabel);
            this.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmInputBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmInputBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMessage;

       
    }
}