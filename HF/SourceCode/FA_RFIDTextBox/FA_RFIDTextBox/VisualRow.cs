using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FA_RFIDTextBox
{
    public class VisualRow
    {
        public static int           ROW_MAX_BYTES_AMOUNT = 8;                                                     //单行最大的字节数量
        public static int           ROW_BYTES_INTERVAL_HORIZON = 4;                                               //单行字节间的布局间隔
        public static Size          ROW_SIZE = new Size(100, 12);                                                 //单行的几何尺寸
        public static VisualPoint[] ROW_BYTES_LOCATIONS;                                                          //单行各字节相对于行起始位置的位移
        public static Font          ROW_FONT = new Font("Arial", 7.0f, FontStyle.Regular);                        //单行字体
        private const string        HEX_STRING_SAMPLE = "FF";                                                     //用于计算布局的十六进制字符串样本
        private const string        ASCII_STRING_SAMPLE = "A";                                                    //用于计算布局的ASCII字符串样本
        private const string        DEC_STRING_SAMPLE = "255";                                                    //用于计算布局的十进制字符串样本

        /// <summary>
        /// 计算出单行各字符相对于行起始位置的位移量（包含ASCII和HEX两种模式下的位移）
        /// 2017年07月19日，我们要添加十进制DEC模式下的位移
        /// </summary>
        public static void CalculateRowsBytesRelativeLayout()
        {
            try
            {
                ROW_BYTES_LOCATIONS = new VisualPoint[ROW_MAX_BYTES_AMOUNT];
                System.Windows.Forms.Form aForm = new System.Windows.Forms.Form();
                Graphics g = aForm.CreateGraphics();
                SizeF hex_sizeF = g.MeasureString(HEX_STRING_SAMPLE, ROW_FONT);
                SizeF ascii_sizeF = g.MeasureString(ASCII_STRING_SAMPLE, ROW_FONT);

                SizeF dec_sizeF = g.MeasureString(DEC_STRING_SAMPLE, ROW_FONT);//2017/07/19  十进制
                Size dec_size = new Size((int)dec_sizeF.Width, (int)dec_sizeF.Height);//2017/07/19 十进制

                Size hex_size = new Size((int)hex_sizeF.Width, (int)hex_sizeF.Height);
                Size ascii_size = new Size((int)ascii_sizeF.Width, (int)ascii_sizeF.Height);
                


                for (int i = 0; i < ROW_MAX_BYTES_AMOUNT; i++)
                {
                    VisualPoint byte_point = new VisualPoint();
                    Point ascii_point = new Point(0, 0);
                    Point hex_point = new Point(0, 0);
                    Point dec_point = new Point(0, 0);//2017/07/19 十进制

                    ascii_point.X = i * (ascii_size.Width + ROW_BYTES_INTERVAL_HORIZON);
                    hex_point.X = i * (hex_size.Width + ROW_BYTES_INTERVAL_HORIZON);
                    dec_point.X = i * (dec_size.Width + ROW_BYTES_INTERVAL_HORIZON);//2017/07/19 十进制

                    byte_point.PointAscii = ascii_point;
                    byte_point.PointHex = hex_point;
                    byte_point.PointDec = dec_point;//2017/07/19 十进制

                    ROW_BYTES_LOCATIONS[i] = byte_point;
                }
            }
            catch
            {
            }
        }

        public VisualPoint[] mBytesLocation;                                                                      //单行所有字节的布局位置

        private Point mRowLocation_Hex;
        public Point RowLocationHex
        {
            get { return mRowLocation_Hex; }
        }
        private Point mRowLocation_Ascii;
        public Point RowLocationAscii
        {
            get { return mRowLocation_Ascii; }
        }

        private Point mRowLocation_Dec;//2017/07/19 十进制
        public Point RowLocationDEC
        {
            get { return mRowLocation_Dec; }
        }//2017/07/19 十进制
        /// <summary>
        /// 计算出当前行满字节数时各字节的绝对布局位置
        /// </summary>
        /// <param name="inRowLocation"></param>
        public void CalculateRowBytesAbsoluteLayout(Point inRowLocation)
        {
            try
            {
                mBytesLocation = new VisualPoint[ROW_MAX_BYTES_AMOUNT];
                for (int i = 0; i < ROW_MAX_BYTES_AMOUNT; i++)
                {
                    VisualPoint byte_point = new VisualPoint();
                    Point ascii_point = new Point(inRowLocation.X + ROW_BYTES_LOCATIONS[i].PointAscii.X,
                        inRowLocation.Y + ROW_BYTES_LOCATIONS[i].PointAscii.Y);
                    Point hex_point = new Point(inRowLocation.X + ROW_BYTES_LOCATIONS[i].PointHex.X,
                        inRowLocation.Y + ROW_BYTES_LOCATIONS[i].PointHex.Y);
                    Point dec_point = new Point(inRowLocation.X + ROW_BYTES_LOCATIONS[i].PointDec.X,
                        inRowLocation.Y + ROW_BYTES_LOCATIONS[i].PointDec.Y);//2017/07/19 十进制
                    byte_point.PointAscii = ascii_point;
                    byte_point.PointHex = hex_point;
                    byte_point.PointDec = dec_point;//2017/07/19 十进制

                    mBytesLocation[i] = byte_point;

                    if (i == 0)
                    {
                        mRowLocation_Ascii = byte_point.PointAscii;
                        mRowLocation_Hex = byte_point.PointHex;
                        mRowLocation_Dec = byte_point.PointDec;//2017/07/19 十进制
                    }
                }
            }
            catch
            {
            }
        }

        public VisualRow()
        {
            mBytesLocation = new VisualPoint[ROW_MAX_BYTES_AMOUNT];
        }
    }
}
