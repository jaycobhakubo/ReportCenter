#region Copyright
// This is an unpublished work protected under the copyright laws of the
// United States and other countries.  All rights reserved.  Should
// publication occur the following will apply:  © 2008 GameTech
// International, Inc.
#endregion

using System;
using System.Linq;
using System.Windows.Forms;
using GTI.Modules.Shared;

namespace GTI.Modules.ReportCenter.UI
{
    public partial class frmInputBox : GradientForm
    {
        public frmInputBox(string prompt, string title, string defaultValue)
        {
            InitializeComponent();
            promptLabel.Text = prompt;
            this.Text = title;
            inputTextBox.Text = defaultValue;
        }

        public static bool IsValidAlphaNumeric(string inputStr)
        {
            //make sure the user provided us with data to check
            //if not then return false
            if (string.IsNullOrEmpty(inputStr))
                return false;

            // STARTRALLY DE3822
            // Allow only alpha numerics spaces and underscores and dashes
            return !inputStr.Where((t, i) => (!char.IsLetterOrDigit(inputStr, i) && t != ' ') && t != '-' && t != '_').Any();
            // END :RALLY  DE3822
        }

        public string InputValue
        { 
           get { return inputTextBox.Text;}
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (IsValidAlphaNumeric(inputTextBox.Text))
            {
                okImageButton.Enabled = true;
                labelMessage.Text = "";
            }
            else
            {
                okImageButton.Enabled = false;
                labelMessage.Text = "Invalid character detected.";
            }
        }

        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) 
            {
                DialogResult = DialogResult.OK;
                e.Handled = true;
            }
        }


    }// End frmInputBox
}