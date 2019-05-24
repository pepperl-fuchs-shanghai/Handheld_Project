using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FA_RFIDTextBox
{
    public partial class WarningForm : Form
    {
        public delegate void EventDelegate(object sender, EventArgs e);
        public event EventDelegate OKButtonPressedEvent;

        public WarningForm()
        {
            InitializeComponent();
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

        private void OKPictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
                if (OKButtonPressedEvent != null)
                {
                    OKButtonPressedEvent.Invoke(sender, e);
                }
            }
            catch
            {
            }
        }
    }
}