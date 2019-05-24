using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FA_RFIDTextBox
{
    public class DataMaster
    {
        public const int ERROR_CODE_SUCCESS = 0;                                                         //错误代码：成功(正常)=0
        public const int ERROR_CODE_EXCEPTION = 1;                                                       //错误代码：异常=1

        public static int           RFID_BLOCK_LENGTH = 4;                                               //RFID数据块的长度
        public static int           RFID_BLOCK_ROWS_AMOUNT = 1;                                          //单个数据块所包含行数
        public static List<int>     RFID_BLOCK_PER_ROW_BYTES_AMOUNT = new List<int>();                   //单个数据块各行所允许包含的字节数
        public static Color         RAW_FORECOLOR = Color.Black;                                         //未被编辑过的字节的显示颜色
        public static Color         EDIT_FORECOLOR = Color.Red;                                          //被编辑过的字节的显示颜色
        public static Color         SPEICAL_BYTE_FORECOLOR = Color.Blue;                                  //0x87字节指示字符颜色
        public static int           RFID_BLOCK_START_ADDRESS = 0;                                        //RFID数据块的起始地址
        public static byte          DEFAULT_NULL_BYTE_ANOTHER_BYTE = 0x87;
        /// <summary>
        /// 计算单个RFID数据块所包含的行数以及各行所允许包含的字节数据数量
        /// </summary>
        public static void CalculateBlockPerRowBytesAmountAllowed()
        {
            try
            {
                RFID_BLOCK_PER_ROW_BYTES_AMOUNT = new List<int>();
                if (RFID_BLOCK_LENGTH <= VisualRow.ROW_MAX_BYTES_AMOUNT)
                    RFID_BLOCK_PER_ROW_BYTES_AMOUNT.Add(RFID_BLOCK_LENGTH);
                else
                {
                    int full_rows_amount = RFID_BLOCK_LENGTH / VisualRow.ROW_MAX_BYTES_AMOUNT;
                    int last_row_bytes_amount = RFID_BLOCK_LENGTH % VisualRow.ROW_MAX_BYTES_AMOUNT;
                    for (int i = 0; i < full_rows_amount; i++)
                    {
                        RFID_BLOCK_PER_ROW_BYTES_AMOUNT.Add(VisualRow.ROW_MAX_BYTES_AMOUNT);
                    }
                    if (last_row_bytes_amount > 0)
                        RFID_BLOCK_PER_ROW_BYTES_AMOUNT.Add(last_row_bytes_amount);
                }

                RFID_BLOCK_ROWS_AMOUNT = RFID_BLOCK_PER_ROW_BYTES_AMOUNT.Count;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取指定范围行的起始字节和终止字节在底层编辑数据中的索引
        /// </summary>
        /// <param name="inStartRow"></param>
        /// <param name="inEndRow"></param>
        /// <returns></returns>
        public static int[] GetBytesStartEndIndexes(int inStartRow, int inEndRow)
        {
            int[] ret=new int[0];
            try
            {
                int byte_start_index = GetRowStartByteIndex(inStartRow);
                int byte_end_index = GetRowEndByteIndex(inEndRow);
                ret = new int[2];
                ret[0]=byte_start_index;
                ret[1]=byte_end_index;

                return ret;
            }
            catch
            {
                return ret;
            }
        }
        /// <summary>
        /// 计算指定行号的第一个字节在底层编辑数据中的索引值
        /// </summary>
        /// <param name="inRowNumber"></param>
        /// <returns></returns>
        public static int GetRowStartByteIndex(int inRowNumber)
        {
            int ret = -1;
            try
            {
                int complet_block_amount = (inRowNumber) / RFID_BLOCK_ROWS_AMOUNT;
                int rest_rows_amount = (inRowNumber) % RFID_BLOCK_ROWS_AMOUNT;
                ret = RFID_BLOCK_LENGTH * complet_block_amount + rest_rows_amount * VisualRow.ROW_MAX_BYTES_AMOUNT;
            }
            catch
            {
            }
            return ret;
        }
        /// <summary>
        /// 计算指定行号的最后一个字节在底层编辑数据的中的索引值
        /// </summary>
        /// <param name="inRowNumber"></param>
        /// <returns></returns>
        public static int GetRowEndByteIndex(int inRowNumber)
        {
            int ret = -1;
            try
            {
                int complet_block_amount = (inRowNumber+1) / RFID_BLOCK_ROWS_AMOUNT;
                int rest_rows_amount = (inRowNumber+1) % RFID_BLOCK_ROWS_AMOUNT;
                ret = RFID_BLOCK_LENGTH * complet_block_amount + rest_rows_amount * VisualRow.ROW_MAX_BYTES_AMOUNT - 1;
            }
            catch
            {
            }

            return ret;
        }



        private List<RFIDByte> mRawBytes;                                                         //用于存储底层所有数据
        private List<RFIDByte> mEditBytes;                                                        //用于编辑的底层所有数据

        private int mRowsCount;                                                                   //总的行数
        public int RowsCount
        {
            get
            {
                CalculateTotalRowsCount();
                return mRowsCount;
            }
        }

        /// <summary>
        /// 查找空值元素
        /// </summary>
        private Predicate<RFIDByte> FindNullByte = delegate(RFIDByte inByte)
        {
            return inByte.ByteValue.Equals(0x00);
        };

        public void IsNullByteExisted(out int nullIndex)
        {
            nullIndex=-1;
            try
            {
                if (mEditBytes.Count > 0)
                {
                    nullIndex = mEditBytes.FindIndex(0, mEditBytes.Count - 1, FindNullByte);
                }
            }
            catch
            {
            }
        }

        public byte[] EditBytesArray
        {
            get
            {
                try
                {
                    byte[] ret = new byte[mEditBytes.Count];
                    for (int i = 0; i < mEditBytes.Count; i++)
                    {
                        ret[i] = mEditBytes[i].ByteValue;
                    }
                    return ret;
                }
                catch
                {
                    return new byte[0];
                }
            }
        }

        public void RefreshEditBytes(byte[] inBytes)
        {
            try
            {
                mEditBytes = new List<RFIDByte>();
                for (int i = 0; i < inBytes.Length; i++)
                {
                    RFIDByte aBaseByte = new RFIDByte(inBytes[i]);
                    mEditBytes.Add(aBaseByte);
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 计算底层数据所涵盖的行数
        /// </summary>
        private void CalculateTotalRowsCount()
        {
            try
            {
                int blocks_count = mEditBytes.Count / RFID_BLOCK_LENGTH;
                int rest = mEditBytes.Count % RFID_BLOCK_LENGTH;

                int in_block_rows_count = rest / VisualRow.ROW_MAX_BYTES_AMOUNT;
                int rest_in_block = rest % VisualRow.ROW_MAX_BYTES_AMOUNT;

                mRowsCount = blocks_count * RFID_BLOCK_ROWS_AMOUNT + in_block_rows_count;
                if (rest_in_block > 0)
                    mRowsCount++;

                if (mRowsCount == 0)
                    mRowsCount++;
            }
            catch
            {
            }
        }

        public int RawBytesLength
        {
            get { return mRawBytes.Count; }
        }

        public int EditeBytesLength
        {
            get { return mEditBytes.Count; }
        }
        /// <summary>
        /// 初始话底层字节数据
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns></returns>
        public int InitializeBaseBytes(byte[] inBytes)
        {
            try
            {
                mEditBytes = new List<RFIDByte>();
                mRawBytes = new List<RFIDByte>();
                for (int i = 0; i < inBytes.Length; i++)
                {
                    RFIDByte aBaseByte = new RFIDByte(inBytes[i]);
                    mRawBytes.Add(aBaseByte);
                    mEditBytes.Add(aBaseByte);
                }

                return ERROR_CODE_SUCCESS;
            }
            catch
            {
                return ERROR_CODE_EXCEPTION;
            }
        }
        /// <summary>
        /// 向底层数据列表中的指定位置添加指定字节数据,添加的原则为末尾添加
        /// </summary>
        /// <param name="inByte"></param>
        /// <param name="mIndex"></param>
        /// <returns></returns>
        public int AddByte(byte inByte)
        {
            try
            {
                RFIDByte aBaseByte=new RFIDByte(inByte);
                mEditBytes.Add(aBaseByte);

                return ERROR_CODE_SUCCESS;
            }
            catch
            {
                return ERROR_CODE_EXCEPTION;
            }
        }
        /// <summary>
        /// 删除底层数据列表中指定位置的数据,删除的原则为末尾删除
        /// </summary>
        /// <param name="mIndex"></param>
        /// <returns></returns>
        public int RemoveByte(int inIndex)
        {
            try
            {
                mEditBytes.RemoveAt(inIndex);
                return ERROR_CODE_SUCCESS;
            }
            catch
            {
                return ERROR_CODE_EXCEPTION;
            }
        }
        /// <summary>
        /// 以指定的字节数据值替换掉底层数据列表中指定位置的数据
        /// </summary>
        /// <param name="inByte"></param>
        /// <param name="mIndex"></param>
        /// <returns></returns>
        public int ReplaceByte(byte inByte, int mIndex)
        {
            try
            {
                RFIDByte aRFIDByte=new RFIDByte(inByte);
                mEditBytes.RemoveAt(mIndex);
                mEditBytes.Insert(mIndex, aRFIDByte);

                return ERROR_CODE_SUCCESS;
            }
            catch
            {
                return ERROR_CODE_EXCEPTION;
            }
        }
        public int InsertByte(byte inByte, int mIndex)
        {
            try
            {
                RFIDByte aBaseByte = new RFIDByte(inByte);
                mEditBytes.Insert(mIndex, aBaseByte);

                return ERROR_CODE_SUCCESS;
            }
            catch
            {
                return ERROR_CODE_EXCEPTION;
            }
        }
        /// <summary>
        /// 检验指定索引位置的字节值是否已经发生改变（对比编辑数据和初始数据）,超出RawBytes的部分全部视为已被编辑
        /// </summary>
        /// <param name="mIndex"></param>
        /// <param name="isEdited"></param>
        /// <returns></returns>
        private int CheckIsEdited(int mIndex,ref bool isEdited)
        {
            try
            {
                if (mIndex < mRawBytes.Count)
                {
                    if (mEditBytes[mIndex].ByteValue == mRawBytes[mIndex].ByteValue)
                        isEdited = false;
                    else
                        isEdited = true;
                }
                else
                {
                    isEdited = true;
                }

                return ERROR_CODE_SUCCESS;
            }
            catch
            {
                return ERROR_CODE_EXCEPTION;
            }
        }
        /// <summary>
        /// 根据前后索引值提取出对应的字节数据，并设定字节的显示颜色
        /// </summary>
        /// <param name="mStartEndIndexes"></param>
        /// <returns></returns>
        public VisualByte[] PickOutVisualBytes(int[] mStartEndIndexes)
        {
            try
            {
                if (mStartEndIndexes.Length == 2)
                {
                    VisualByte[] ret=new VisualByte[0];
                    if ((mStartEndIndexes[1] - mStartEndIndexes[0] + 1) <= (mEditBytes.Count - mStartEndIndexes[0]))
                        ret = new VisualByte[mStartEndIndexes[1] - mStartEndIndexes[0] + 1];
                    else
                        ret = new VisualByte[mEditBytes.Count - mStartEndIndexes[0]];
                    for (int i = 0; i < ret.Length; i++)
                    {
                        ret[i] = new VisualByte();
                        ret[i].ByteValue = mEditBytes[i + mStartEndIndexes[0]].ByteValue;
                        bool is_edited = false;

                        if (ret[i].ByteValue == DEFAULT_NULL_BYTE_ANOTHER_BYTE)
                            ret[i].ForeColor = SPEICAL_BYTE_FORECOLOR;
                        else if (CheckIsEdited(i+mStartEndIndexes[0], ref is_edited) == ERROR_CODE_SUCCESS)
                        {
                            if (is_edited)
                                ret[i].ForeColor = EDIT_FORECOLOR;
                            else
                                ret[i].ForeColor = RAW_FORECOLOR;
                        }

                    }
                    return ret;
                }
                return new VisualByte[0];
            }
            catch
            {
                return new VisualByte[0];
            }
        }

        public byte[] GetBaseEditBytes()
        {
            try
            {
                byte[] ret = new byte[mEditBytes.Count];
                for (int i = 0; i < mEditBytes.Count; i++)
                {
                    ret[i] = mEditBytes[i].ByteValue;
                }

                return ret;
            }
            catch
            {
                return new byte[0];
            }
        }

        public DataMaster()
        {
            mRawBytes = new List<RFIDByte>();
            mEditBytes = new List<RFIDByte>();
        }
        public DataMaster(byte[] inBytes)
        {
            InitializeBaseBytes(inBytes);
        }
    }
}
