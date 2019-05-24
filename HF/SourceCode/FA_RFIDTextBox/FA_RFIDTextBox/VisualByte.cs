using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FA_RFIDTextBox
{
    public class VisualByte:RFIDByte
    {
        private Color mForeColor;
        public Color ForeColor
        {
            get { return mForeColor; }
            set { mForeColor = value; }
        }

        public VisualByte()
        {
            mForeColor = Color.Black;
        }
    }
}
