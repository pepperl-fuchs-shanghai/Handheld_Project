using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FA_RFIDTextBox
{
    public class VisualZone
    {
        public const int ERROR_CODE_SUCCESS = 0;                                                   //错误代码：成功(正常)标志位=0
        public const int ERROR_CODE_EXCEPTION = 1;                                                 //错误代码：异常标志位=1 
        public const int ERROR_CODE_LAYOUT_OVERFLOW = 2;                                           //错误代码：布局溢出=2     
        public const int ERROR_CODE_DATA_OVERFLOW = 3;                                             //错误代码：数据溢出

        public Size  ZoneSize;                                                                     //显示区域的大小
        public Point ZonePosition;                                                                 //显示区域的位置

        private List<VisualRow> mRows;                                                             //显示区域所有的行
        private int mStartRowNumber;                                                               //显示区域的实时首行行数
        public int StartRowNumber
        {
            get { return mStartRowNumber; }
            set { mStartRowNumber = value; }
        }

        public enum DataFormat
        {
            ASCII,
            HEX,
            DEC//2018/07/19 十进制
        }                                                                  //数据显示的两种模式（十六进制字符串模式和ASCII码字符串模式）
        private DataFormat mDataFormat;
        public DataFormat CurrentDataFormat
        {
            get { return mDataFormat; }
            set { mDataFormat = value; }
        }

        public int VisualRowsAmount
        {
            get { return mRows.Count; }
        }
        /// <summary>
        /// 计算出当前显示区域所有行的空间布局
        /// </summary>
        /// <returns></returns>
        public int PrepareRowsLayout()
        {
            try
            {
                mRows.Clear();
                VisualRow.CalculateRowsBytesRelativeLayout();
                bool isLayoutOverflow = false;
                int row_counter = 0;
                while (!isLayoutOverflow)
                {
                    Point row_point = new Point(ZonePosition.X, row_counter * VisualRow.ROW_SIZE.Height + ZonePosition.Y);
                    if (CheckRowsLayoutOverflow(row_point) == ERROR_CODE_SUCCESS)
                    {
                        VisualRow aRow = new VisualRow();
                        aRow.CalculateRowBytesAbsoluteLayout(row_point);
                        mRows.Add(aRow);
                        row_counter++;
                    }
                    else
                    {
                        break;
                    }
                }

                return ERROR_CODE_SUCCESS;
            }
            catch
            {
                return ERROR_CODE_EXCEPTION;
            }
        }
        /// <summary>
        /// 检验目标行起始位置是否已超出当前区域
        /// </summary>
        /// <returns>int</returns>
        private int CheckRowsLayoutOverflow(Point inRowPoint)
        {
            try
            {
                if (inRowPoint.X >= ZonePosition.X && inRowPoint.X <= (ZonePosition.X + ZoneSize.Width)
                    && inRowPoint.Y >= ZonePosition.Y && inRowPoint.Y <= (ZonePosition.Y + ZoneSize.Height))
                {
                    return ERROR_CODE_SUCCESS;
                }
                else
                {
                    return ERROR_CODE_LAYOUT_OVERFLOW;
                }
            }
            catch
            {
                return ERROR_CODE_EXCEPTION;
            }
        }

        /// <summary>
        /// 用于在ASCII和十六进制两种模式下在数据可视化区域绘制出相应的数据
        /// </summary>
        /// <param name="g"></param>
        /// <param name="inBytes"></param>
        /// <returns></returns>
        public int DrawVisualZone(ref Graphics g,VisualByte[] inBytes)
        {
            try
            {
                int current_row_number = mStartRowNumber;

                int mIndex = 0;
                while (mIndex < inBytes.Length)
                {
                    int row_bytes_counter = 0;
                    int current_row_bytes_allowed = GetCurrentRowBytesAllowed(current_row_number);

                    for (int i = 0; i < current_row_bytes_allowed; i++)
                    {
                        SolidBrush strBrush = new SolidBrush(inBytes[mIndex+i].ForeColor);
                        switch (mDataFormat)
                        {
                            case DataFormat.ASCII:
                                g.DrawString(inBytes[mIndex+i].StringASCII,
                                    VisualRow.ROW_FONT,
                                    strBrush,
                                    mRows[current_row_number - mStartRowNumber].mBytesLocation[i].PointAscii.X,
                                    mRows[current_row_number - mStartRowNumber].mBytesLocation[i].PointAscii.Y
                                    );
                                break;
                            case DataFormat.HEX:
                                g.DrawString(inBytes[mIndex+i].StringHex,
                                    VisualRow.ROW_FONT,
                                    strBrush,
                                    mRows[current_row_number - mStartRowNumber].mBytesLocation[i].PointHex.X,
                                    mRows[current_row_number - mStartRowNumber].mBytesLocation[i].PointHex.Y
                                    );
                                break;
                            case DataFormat.DEC://2018/07/19 十进制
                                g.DrawString(inBytes[mIndex + i].StringDEC,
                                    VisualRow.ROW_FONT,
                                    strBrush,
                                    mRows[current_row_number - mStartRowNumber].mBytesLocation[i].PointDec.X,
                                    mRows[current_row_number - mStartRowNumber].mBytesLocation[i].PointDec.Y
                                    );//2018/07/19 十进制
                                break;
                        }
                        row_bytes_counter++;

                    }
                    mIndex += row_bytes_counter;
                    current_row_number++;
                    current_row_bytes_allowed = GetCurrentRowBytesAllowed(current_row_number);
                }

                return ERROR_CODE_SUCCESS;
            }
            catch
            {
                return ERROR_CODE_EXCEPTION;
            }
        }

        public int GetCurrentRowBytesAllowed(int inRowNumber)
        {
            int ret = 0;
            try
            {
                int relative_number = (inRowNumber + 1) % DataMaster.RFID_BLOCK_ROWS_AMOUNT;
                if (relative_number > 0)
                {
                    ret = DataMaster.RFID_BLOCK_PER_ROW_BYTES_AMOUNT[relative_number - 1];
                }
                else
                {
                    ret = DataMaster.RFID_BLOCK_PER_ROW_BYTES_AMOUNT[DataMaster.RFID_BLOCK_ROWS_AMOUNT - 1];
                }
            }
            catch
            {
            }
            return ret;
        }

        public Point GetSelectedRowLocation(int inRowNumber)
        {
            try
            {
                switch (mDataFormat)
                {
                    case DataFormat.ASCII:
                        return mRows[inRowNumber - mStartRowNumber].RowLocationAscii;
                    case DataFormat.HEX:
                        return mRows[inRowNumber - mStartRowNumber].RowLocationHex;
                    case DataFormat.DEC://2018/07/19 十进制
                        return mRows[inRowNumber - mStartRowNumber].RowLocationDEC;//2018/07/19 十进制
                    default:
                        return new Point(0, -1000);
                }
            }
            catch
            {
                return new Point(0,-1000);
            }
        }

        public void RefreshAddressData(ref Graphics g,int inTotalRows)
        {
            try
            {
                SolidBrush aBrush = new SolidBrush(DataMaster.RAW_FORECOLOR);
                for (int i = 0; i < mRows.Count; i++)
                {
                    if (i + mStartRowNumber < inTotalRows)
                    {
                        int blocks_count = (i + mStartRowNumber) / DataMaster.RFID_BLOCK_ROWS_AMOUNT;
                        int block_rest = (i + mStartRowNumber) % DataMaster.RFID_BLOCK_ROWS_AMOUNT;

                        if (block_rest == 0)
                        {
                            switch (mDataFormat)
                            {
                                case DataFormat.HEX:
                                    g.DrawString((DataMaster.RFID_BLOCK_START_ADDRESS + blocks_count).ToString(),
                                        VisualRow.ROW_FONT,
                                        aBrush,
                                        0,
                                        mRows[i].RowLocationHex.Y);
                                    break;
                                case DataFormat.ASCII:
                                    g.DrawString((DataMaster.RFID_BLOCK_START_ADDRESS + blocks_count).ToString(),
                                        VisualRow.ROW_FONT,
                                        aBrush,
                                        0,
                                        mRows[i].RowLocationAscii.Y);
                                    break;

                                case DataFormat.DEC://2018/07/19 十进制
                                    g.DrawString((DataMaster.RFID_BLOCK_START_ADDRESS+blocks_count).ToString(),
                                        VisualRow.ROW_FONT,
                                        aBrush,
                                        0,
                                        mRows[i].RowLocationDEC.Y);//2018/07/19 十进制
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public VisualZone()
        {
            mRows = new List<VisualRow>();
            mDataFormat = DataFormat.HEX;
        }
    }
}
