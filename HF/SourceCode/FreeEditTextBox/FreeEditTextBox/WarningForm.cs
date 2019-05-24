using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FreeEditTextBox
{
    public partial class WarningForm : Form
    {
        public delegate void EventDelegate(object sender, EventArgs e);
        public event EventDelegate OKButtonPressedEvent;

        public WarningForm()
        {
            InitializeComponent();
        }

        private void OKPictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
            }
            catch
            {
            }
        }

        private void WarningForm_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                this.Visible = false;
                e.Cancel = true;
            }
            catch
            {
            }
        }
    }
}