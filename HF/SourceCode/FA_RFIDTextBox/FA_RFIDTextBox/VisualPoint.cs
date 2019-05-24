using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FA_RFIDTextBox
{
    public class VisualPoint
    {
        private Point mAscii;                                //ASCII字符的位置
        public Point PointAscii
        {
            get { return mAscii; }
            set { mAscii = value; }
        }

        private Point mHex;                                  //HEX字符串的位置
        public Point PointHex
        {
            get { return mHex; }
            set { mHex = value; }
        }

        private Point mDec;                                  //纸巾纸字符串的位置
        public Point PointDec
        {
            get { return mDec; }
            set { mDec = value; }
        }

        public VisualPoint()
        {
            mAscii = new Point();
            mHex = new Point();
        }

        public VisualPoint(Point inAsciiPoint, Point inHexPoint)
        {
            mHex = inHexPoint;
            mAscii = inAsciiPoint;
        }
    }
}
