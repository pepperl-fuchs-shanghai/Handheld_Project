using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace FA_RFIDTextBox
{
    public partial class RFIDTextBox : UserControl
    {      
        public Size BoxSize
        {
            get { return this.Size; }
            set
            {
                this.Size = value;
            }
        }
        public int AddressColumnWidth = 30;
        public int ScrollBarWidth = 30;
        public Color AddressZoneBackColor = Color.Green;
        public Color AddressZoneForeColor = Color.Black;

        private WarningForm mWarningForm = new WarningForm();


        #region 可视化数据区域的属性设定
        //******************************************************************
        private VisualZone mVisualZone = new VisualZone();
        public Color DataZoneBackColor
        {
            get { return VisualPanel.BackColor; }
            set
            {
                VisualPanel.BackColor = value;
            }
        }
        public Point DataZoneLocation                                                                  //数据显示区域的位置，该位置是相对于VisualPanel的位置
        {
            get { return mVisualZone.ZonePosition; }
            set { mVisualZone.ZonePosition = value; }
        }
        public Size DataZoneSize                                                                       //数据显示区域的尺寸大小
        {
            get { return mVisualZone.ZoneSize; }
            set { mVisualZone.ZoneSize = value; }
        }
        public VisualZone.DataFormat VisualDataFormat
        {
            get { return mVisualZone.CurrentDataFormat; }
            set
            {
                mVisualZone.CurrentDataFormat = value;
            }
        }
        public bool DataZoneKeepEmpty = false;
        //******************************************************************
        #endregion

        #region 数据主控对象的属性设定
        //******************************************************************
        private DataMaster mDataMaster = new DataMaster();
        
        public static int RFIDBlockLength
        {
            get { return DataMaster.RFID_BLOCK_LENGTH; }
            set
            {
                DataMaster.RFID_BLOCK_LENGTH = value;
            }
        }
        public static Color RawForeColor
        {
            get { return DataMaster.RAW_FORECOLOR; }
            set
            {
                try
                {
                    DataMaster.RAW_FORECOLOR = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        public static Color EditForeColor
        {
            get { return DataMaster.EDIT_FORECOLOR; }
            set { EditForeColor = value; }
        }
        public static Color SpecialByteForeColor
        {
            get { return DataMaster.SPEICAL_BYTE_FORECOLOR; }
            set { DataMaster.SPEICAL_BYTE_FORECOLOR = value; }
        }
        public int DataCount
        {
            get
            {
                return mDataMaster.EditeBytesLength;
                
            }
        }
        public byte[] BaseEditData
        {
            get { return mDataMaster.GetBaseEditBytes(); }
        }
        public int NullByteIndex
        {
            get
            {
                int mIndex = -1;
                mDataMaster.IsNullByteExisted(out mIndex);

                return mIndex;
            }
        }
        //******************************************************************
        #endregion

        #region 可视化行的属性设定
        //******************************************************************
        
        public static int RowBytesMaxAmount
        {
            get { return VisualRow.ROW_MAX_BYTES_AMOUNT; }
            set { VisualRow.ROW_MAX_BYTES_AMOUNT = value; }
        }
        public static int RowBytesIntervalHorizon
        {
            get { return VisualRow.ROW_BYTES_INTERVAL_HORIZON; }
            set { VisualRow.ROW_BYTES_INTERVAL_HORIZON = value; }
        }
        public static Size RowSize
        {
            get { return VisualRow.ROW_SIZE; }
            set { VisualRow.ROW_SIZE = value; }
        }
        public static Font RowFont
        {
            get { return VisualRow.ROW_FONT; }
            set { VisualRow.ROW_FONT = value; }
        }
        //******************************************************************
         
        #endregion

        #region 可视化数据的编辑框
        //******************************************************************
        public bool IsEditTextBoxFocus
        {
            get { return EditeTextBox.Focused; }
        }

        /// <summary>
        /// 外界对编辑框的动作指令枚举
        /// </summary>
        public enum EditTextBoxAction
        {
            REPALCEMENT_UP,
            REPLACEMENT_DOWN,
        }

        private int SelectedRowNumber;

        public EditTextBoxAction EditBoxAction
        {
            set
            {
                switch (value)
                {
                    case EditTextBoxAction.REPALCEMENT_UP:
                        if (SelectedRowNumber > 0)
                        {
                            UpMoveEditText();
                            EditeTextBox.SelectionStart = EditeTextBox.Text.Length;
                            selectedIndex = EditeTextBox.SelectionStart;
                            RefreshVisualZoneData();
                        }
                        break;

                    case EditTextBoxAction.REPLACEMENT_DOWN:
                        break;
                }
            }
        }

        public int CurrentSelectedStart
        {
            get { return EditeTextBox.SelectionStart; }
        }

        private void UpMoveEditText()
        {
            try
            {
                if (SelectedRowNumber > mVisualZone.StartRowNumber)
                    SelectedRowNumber--;
                else
                {
                    if (mVisualZone.StartRowNumber > 0)
                    {
                        VisualScrollBar.Value--;
                        SelectedRowNumber--;
                    }
                }

                RefreshEditTextBoxLocation(SelectedRowNumber);
                getBaseDataForSelectedRow(SelectedRowNumber);//0909090990

            }
            catch
            {
            }
        }

        public bool EditedBoxVisiable
        {
            get { return EditeTextBox.Visible; }
            set
            {
                try
                {
                    if (EditeTextBox.Visible != value && !value)
                    {
                        ForceRefreshEditedData();
                    }
                    EditeTextBox.Visible = value;
                }
                catch
                {
                }
            }
        }

        //******************************************************************
        #endregion

        public RFIDTextBox()
        {
            InitializeComponent();
            mWarningForm.Location = new Point(20, 50);
            mWarningForm.OKButtonPressedEvent += new WarningForm.EventDelegate(mWarningForm_OKButtonPressedEvent);
        }

        void mWarningForm_OKButtonPressedEvent(object sender, EventArgs e)
        {
            try
            {
                RefreshVisualZoneData();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 初始化底层数据，并一同初始化了各布局相关参数
        /// </summary>
        /// <param name="inBytes"></param>
        public void InitializeRawBytes(byte[] inBytes)
        {
            try
            {
                RefreshVisualZoneLayout();
                mVisualZone.StartRowNumber = 0;
                mDataMaster.InitializeBaseBytes(inBytes);
                RefreshVisualZoneData();

            }
            catch
            {
            }
        }

        public void RefreshVisualZoneLayout()
        {
            try
            {
                VisualScrollBar.Value = 0;
                mVisualZone.ZoneSize = VisualPanel.Size;
                VisualRow.ROW_SIZE = new Size(mVisualZone.ZoneSize.Width, 9);
                DataMaster.CalculateBlockPerRowBytesAmountAllowed();
                mVisualZone.PrepareRowsLayout();
            }
            catch
            {
            }
        }

        public void RefreshVisualZoneData()
        {
            try
            {
                VisualPanel.Refresh();
                    if (!DataZoneKeepEmpty)
                    {
                        Graphics g = VisualPanel.CreateGraphics();
                        int[] bytes_indexes = DataMaster.GetBytesStartEndIndexes(mVisualZone.StartRowNumber, mVisualZone.StartRowNumber + mVisualZone.VisualRowsAmount - 1);
                        mVisualZone.DrawVisualZone(ref g, mDataMaster.PickOutVisualBytes(bytes_indexes));
                    }                 
                
                ReDrawBlockAddress();
                RefreshScrollBarValues();
                DataZoneKeepEmpty = false;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 定义控件各元素的位置、尺寸以及底色
        /// </summary>
        public void RefreshElementsLayout()
        {
            try
            {
                if (mVisualZone.CurrentDataFormat == VisualZone.DataFormat.HEX)
                {
                    AddressPanel.Location = new Point(0, 0);
                    AddressPanel.Size = new Size(AddressColumnWidth, BasePanel.Size.Height);
                    AddressPanel.BackColor = AddressZoneBackColor;

                    VisualPanel.Location = new Point(AddressColumnWidth, 0);
                    VisualPanel.Size = new Size(BasePanel.Size.Width - AddressColumnWidth - ScrollBarWidth, BasePanel.Height);
                    VisualPanel.BackColor = DataZoneBackColor;

                    ScrollPanel.Location = new Point(AddressColumnWidth + VisualPanel.Size.Width, 0);
                    ScrollPanel.Size = new Size(ScrollBarWidth, BasePanel.Size.Height);
                    ScrollPanel.BackColor = DataZoneBackColor;
                }
                else
                {
                    AddressPanel.Location = new Point(0, 0);
                    AddressPanel.Size = new Size(0,0);
                    AddressPanel.BackColor = AddressZoneBackColor;

                    VisualPanel.Location = new Point(0, 0);
                    VisualPanel.Size = new Size(BasePanel.Width, BasePanel.Height);
                    VisualPanel.BackColor = DataZoneBackColor;

                    ScrollPanel.Location = new Point(BasePanel.Width, 0);
                    ScrollPanel.Size = new Size(0, 0);
                    ScrollPanel.BackColor = DataZoneBackColor;
                }
            }
            catch
            {
            }
        }

        public int GetSelectedRowNumber(Point inPoint)
        {
            try
            {
                int full_rows_count = inPoint.Y / VisualRow.ROW_SIZE.Height;
                return full_rows_count + mVisualZone.StartRowNumber;
            }
            catch
            {
                return -1;
            }
        }

        private void VisualPanel_MouseDown(object sender, MouseEventArgs e)
        {
            Point mouse_point = new Point(e.X, e.Y);
            if (EditeTextBox.Visible)
            {
                ForceRefreshEditedData();
            }
            SelectedRowNumber = GetSelectedRowNumber(mouse_point);
            if (SelectedRowNumber <= mDataMaster.RowsCount)
            {
                RefreshEditTextBoxLocation(SelectedRowNumber);
                getBaseDataForSelectedRow(SelectedRowNumber);//0090909099009
            }


        }

        private void RefreshEditTextBoxLocation(int inSelectedRowNumber)
        {
            try
            {
                if (inSelectedRowNumber <=mDataMaster.RowsCount)
                    EditeTextBox.Location = mVisualZone.GetSelectedRowLocation(inSelectedRowNumber);
                else
                    EditeTextBox.Location = mVisualZone.GetSelectedRowLocation(mVisualZone.StartRowNumber);

                if (EditeTextBox.Location.Y >=0)
                {
                    EditeTextBox.Font = VisualRow.ROW_FONT;
                    EditeTextBox.Size = FA_RFIDTextBox.VisualRow.ROW_SIZE;
                    EditeTextBox.Visible = true;
                    EditeTextBox.Focus();
                    //getBaseDataForSelectedRow(SelectedRowNumber);
                    RefreshVisualZoneData();

                }
            }
            catch
            {
            }
        }

        private void VisualScrollBar_ValueChanged(object sender, EventArgs e)
        {
            EditeTextBox.Visible = false;
            mVisualZone.StartRowNumber = VisualScrollBar.Value;
            RefreshVisualZoneData();
        }

        private int selectedIndex = 0;
        private bool isEditTextBoxModified = false;

        private string CurrentEditTextBoxText = "";
        private bool IsInitializeEditText = false;
        /// <summary>
        /// 线程1： 实时完成对数据编辑框的版式排列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditeTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CurrentEditTextBoxText = EditeTextBox.Text;
                if (!isEditTextBoxModified)
                {
                    AllowCheckEditBytes = false;
                    if (!IsInitializeEditText)
                        selectedIndex = EditeTextBox.SelectionStart;
                    switch (mVisualZone.CurrentDataFormat)
                    {
                        case VisualZone.DataFormat.HEX:
                            int relativeIndex = getRelativeSelectedStartForHexString(EditeTextBox.Text, selectedIndex);
                            string ret = getEditTextBoxStandardHexString(EditeTextBox.Text);
                            selectedIndex = getAbsoluteSelectedStartForHexString(ret, relativeIndex);
                            if (EditeTextBox.Text != ret)
                            {
                                EditeTextBox.Text = ret;

                                EditeTextBox.SelectionStart = selectedIndex;
                            }
                            else
                            {
                                if (!IsInitializeEditText)
                                    AllowCheckEditBytes = true;
                                else
                                    IsInitializeEditText = false;
                            }
                            break;
                        case VisualZone.DataFormat.ASCII:
                            if (!IsInitializeEditText)
                                AllowCheckEditBytes = true;
                            else

                                IsInitializeEditText = false;

                            break;

                        case VisualZone.DataFormat.DEC://2017/07/20 十进制
                            relativeIndex = getRelativeSelectedStartForDecString(EditeTextBox.Text, selectedIndex);
                            ret = getEditTextBoxStandardDecString(EditeTextBox.Text);
                            selectedIndex = getAbsoluteSelectedStartForDecString(ret, relativeIndex);
                            if (EditeTextBox.Text != ret)
                            {
                                EditeTextBox.Text = ret;

                                EditeTextBox.SelectionStart = selectedIndex;
                            }
                            else
                            {
                                if (!IsInitializeEditText)
                                    AllowCheckEditBytes = true;
                                else
                                    IsInitializeEditText = false;
                            }
                            break;
                    }

                    isEditTextBoxModified = false;
                }
                else
                {
                    isEditTextBoxModified = false;
                    EditeTextBox.SelectionStart = selectedIndex;
                    if (!IsInitializeEditText)
                        AllowCheckEditBytes = true;
                    else
                        IsInitializeEditText = false;
                }
            }
            catch
            {
            }
        } 
                                                  
        private int LastEditBytesAmount;                                                           //存放上一次编辑框中的数据个数，以备与最新数据内容进行匹配
        private bool AllowCheckEditBytes = false;

        private int getAbsoluteSelectedStartForASCIIString(string rawText, int relativeStart)
        {
            try
            {
                if (rawText != null)
                {
                    int i = 0;
                    int count = 0;
                    while (i < rawText.Length)
                    {
                        if (count >= relativeStart)
                        {
                            return i;
                        }

                        if (rawText.Substring(i, 1) != " ")
                            count++;
                        i++;
                    }

                    return 0;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private int getRelativeSelectedStartForASCIIString(string rawText, int absolutStart)
        {
            try
            {
                int ret = 0;
                if (rawText != null)
                {

                    int i = 0;
                    while (i < rawText.Length)
                    {
                        if (i >= absolutStart)
                            return ret;
                        if (rawText.Substring(i, 1) != " ")
                            ret++;

                        i++;
                    }
                }
                return ret;
            }
            catch
            {
                return 0;
            }
        }

        private string getEditTextBoxStandardASCIIString(string str)
        {
            try
            {
                string ret = "";
                if (str != null)
                {
                    int i = 0;
                    while (i < str.Length)
                    {
                        if (str.Substring(i, 1) != " ")
                            ret += str.Substring(i++, 1) + "  ";
                        else
                            i++;
                    }
                }
                isEditTextBoxModified = true;
                return ret;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 将输入的数据从输入的行号的起始位置开始逐个写入数据
        /// 写入方式为：1. 当相对索引值小于当前行的容量时，采用覆盖的方式；
        ///             2. 当相对于索引值大于或等于当前行的容量时，采用嵌入的方式。
        /// </summary>
        /// <param name="inBytes"></param>
        /// <param name="inRowNumber"></param>
        private void RefreshBytesIntoBaseEditData(byte[] inBytes,int inRowNumber)
        {
            try
            {
                int row_capacity = mVisualZone.GetCurrentRowBytesAllowed(inRowNumber);
                int start_index = DataMaster.GetRowStartByteIndex(inRowNumber);
                for (int i = 0; i < inBytes.Length; i++)
                {
                    if (i < row_capacity)
                    {
                        /*
                        if (inBytes[i] == DataMaster.DEFAULT_NULL_BYTE_ANOTHER_BYTE)
                        {
                            if (i < mRawEditTextBoxBytes.Length && mRawEditTextBoxBytes[i] == 0x00)
                               continue;
                        }
                         * */

                        if (i + start_index < mDataMaster.EditeBytesLength)
                            mDataMaster.ReplaceByte(inBytes[i], start_index + i);
                        else
                            mDataMaster.AddByte(inBytes[i]);
                    }
                    else
                    {

                        mDataMaster.InsertByte(inBytes[i], start_index + i);
                    }
                }
                if (inBytes.Length > row_capacity&&selectedIndex>=EditeTextBox.Text.Length-2)
                {

                    switch (mVisualZone.CurrentDataFormat)
                    {
                        case VisualZone.DataFormat.HEX:
                            SelectedRowNumber++;
                            if (SelectedRowNumber >= (mVisualZone.StartRowNumber + mVisualZone.VisualRowsAmount - 1))
                                VisualScrollBar.Value += (SelectedRowNumber - mVisualZone.StartRowNumber - mVisualZone.VisualRowsAmount + 1);

                            RefreshEditTextBoxLocation(SelectedRowNumber);
                            selectedIndex = 2;
                            break;
                        case VisualZone.DataFormat.ASCII:
                            if (selectedIndex >= EditeTextBox.Text.Length)
                            {
                                SelectedRowNumber++;
                                if (SelectedRowNumber >= (mVisualZone.StartRowNumber + mVisualZone.VisualRowsAmount - 1))
                                    VisualScrollBar.Value += (SelectedRowNumber - mVisualZone.StartRowNumber - mVisualZone.VisualRowsAmount + 1);
                                selectedIndex = 1;
                                RefreshEditTextBoxLocation(SelectedRowNumber);
                            }
                            break;

                        case VisualZone.DataFormat.DEC://2017/07/20 十进制
                            SelectedRowNumber++;
                            if (SelectedRowNumber >= (mVisualZone.StartRowNumber + mVisualZone.VisualRowsAmount - 1))
                                VisualScrollBar.Value += (SelectedRowNumber - mVisualZone.StartRowNumber - mVisualZone.VisualRowsAmount + 1);

                            RefreshEditTextBoxLocation(SelectedRowNumber);
                            selectedIndex = 3;
                            break;
                    }
                }
                else if (inBytes.Length < row_capacity)
                {
                    int difference = row_capacity - inBytes.Length;
                    for (int i = 0; i < difference; i++)
                    {
                        if ((start_index + inBytes.Length) < mDataMaster.EditeBytesLength)
                        {
                            mDataMaster.RemoveByte(start_index + inBytes.Length);
                        }

                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 查验输入的字符串中所包含的十六进制字符是否为偶数个
        /// </summary>
        /// <param name="inHexStr"></param>
        /// <returns></returns>
        private bool IsHexStringEven(string inHexStr)
        {
            try
            {
                int count = 0;
                for (int i = 0; i < inHexStr.Length; i++)
                {
                    if (inHexStr[i] >= '0' && inHexStr[i] <= '9' || inHexStr[i] >= 'A' && inHexStr[i] <= 'F' || inHexStr[i] >= 'a' && inHexStr[i] <= 'f')
                        count++;
                }

                if (count % 2 == 0 && count > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        //2017/07/20 十进制
        private bool IsDecStringTreble(string inDecStr)
        {
            try
            {
                int count = 0;
                for (int i = 0; i < inDecStr.Length; i++)
                {
                    if (inDecStr[i] >= '0' && inDecStr[i] <= '9')
                        count++;
                }

                if (count % 3 == 0 && count > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 仅保留在字符串中的十六进制字符（'0'~'9'和'A'~'F'以及'a'~'f'）
        /// </summary>
        /// <param name="rawStr"></param>
        /// <returns></returns>
        private string ReserveOnlyHexString(string rawStr)
        {
            try
            {
                string ret = "";
                for (int i = 0; i < rawStr.Length; i++)
                {
                    if (rawStr[i] >= '0' && rawStr[i] <= '9' || rawStr[i] >= 'A' && rawStr[i] <= 'F'||rawStr[i]>='a'&&rawStr[i]<='f')
                    {
                        ret += rawStr[i].ToString();
                    }
                }
                return ret;
            }
            catch
            {
                return "";
            }
        }

        //2017/07/20 十进制
        private int getRelativeSelectedStartForDecString(string rawText, int absoluteStart)
        {
            try
            {
                int ret = 0;
                if (rawText != null)
                {
                    char[] ch = rawText.ToCharArray();
                    int i = 0;
                    while (i < ch.Length)
                    {
                        if (i >= absoluteStart)
                            break;

                        if((ch[i]>='0'&&ch[i]<='9'))
                        {
                            ret++;
                        }
                        i++;
                    }
                }

                return ret;
            }
            catch
            {
                return 0;
            }
        }

        private int getRelativeSelectedStartForHexString(string rawText, int absoluteStart)
        {
            try
            {
                int ret = 0;
                if (rawText != null)
                {
                    char[] ch = rawText.ToUpper().ToCharArray();
                    int i = 0;
                    while (i < ch.Length)
                    {
                        if (i >= absoluteStart)
                            break;
                        if ((ch[i] >= '0' && ch[i] <= '9') || (ch[i] >= 'A' && ch[i] <= 'F'))
                        {
                            ret++;
                        }
                        i++;
                    }
                }
                return ret;
            }
            catch
            {
                return 0;
            }
        }

        //2017/07/20 十进制
        private string getEditTextBoxStandardDecString(string str)
        {
            try
            {
                string ret = "";
                if (str != null)
                {
                    char[] ch = str.ToCharArray();
                    int i = 0;
                    int count = 0;
                    while (i < ch.Length)
                    {
                        if (ch[i] >= '0' && ch[i] <= '9')
                        {
                            ret += ch[i].ToString();
                            count++;

                            if (count % 3 == 0)
                            {
                                ret += " ";
                                count = 0;
                            }
                        }
                        i++;
                    }
                }

                isEditTextBoxModified = true;
                return ret;
            }
            catch
            {
                return "";
            }
        }

        //2017/07/20 十进制
        private int getAbsoluteSelectedStartForDecString(string rawText, int relativeStart)
        {
            try
            {
                if (rawText != null)
                {
                    int i = 0;
                    char[] ch = rawText.ToCharArray();
                    int count = 0;

                    while (i < ch.Length)
                    {
                        if (count >= relativeStart)
                        {
                            return i;
                        }

                        if (ch[i] >= '0' && ch[i] <= '9')
                            count++;

                        i++;
                    }
                    return ch.Length;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private int getAbsoluteSelectedStartForHexString(string rawText, int relativeStart)
        {
            try
            {
                if (rawText != null)
                {
                    int i = 0;
                    char[] ch = rawText.ToUpper().ToCharArray();
                    int count = 0;
                    while (i < ch.Length)
                    {

                        if (count >= relativeStart)
                        {
                            return i;
                        }

                        if (ch[i] >= '0' && ch[i] <= '9' || ch[i] >= 'A' && ch[i] <= 'F')
                            count++;

                        i++;
                    }

                    return ch.Length;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private string getEditTextBoxStandardHexString(string str)
        {
            try
            {
                string ret = "";
                if (str != null)
                {
                    char[] ch = str.ToCharArray();
                    int i = 0;
                    int count = 0;
                    while (i < ch.Length)
                    {

                        if (ch[i] >= '0' && ch[i] <= '9' || ch[i] >= 'A' && ch[i] <= 'F'||ch[i]>='a'&&ch[i]<='f')
                        {
                            ret += ch[i].ToString();
                            count++;

                            if (count % 2 == 0)
                            {
                                ret += "  ";
                                count = 0;
                            }
                        }
                        i++;
                    }
                }
                isEditTextBoxModified = true;
                return ret;
            }
            catch
            {
                return "";
            }
        }
        private byte[] mRawEditTextBoxBytes = new byte[0];
        private void getBaseDataForSelectedRow(int inRowNumber)
        {
            try
            {
                IsInitializeEditText = true;

                int[] indexes = DataMaster.GetBytesStartEndIndexes(inRowNumber, inRowNumber);
                VisualByte[] bytes = mDataMaster.PickOutVisualBytes(indexes);
                byte[] datas = new byte[bytes.Length];
                LastEditBytesAmount = datas.Length;
                bool isContain87 = false;
                for (int i = 0; i < bytes.Length; i++)
                {
                    datas[i] = bytes[i].ByteValue;

                    if (datas[i] == DataMaster.DEFAULT_NULL_BYTE_ANOTHER_BYTE)
                        isContain87 = true;
                }
                mRawEditTextBoxBytes = datas;
                switch (mVisualZone.CurrentDataFormat)
                {
                    case VisualZone.DataFormat.ASCII:
                        string text = ConvertBytesToString(datas);
                        if (!isContain87)
                        {
                            
                            if (EditeTextBox.Text != text)
                            {
                                EditeTextBox.Text = text;
                            }
                            else
                            {
                                IsInitializeEditText = false;
                            }
                            EditeTextBox.SelectionStart = selectedIndex;
                        }
                        else
                        {
                            EditeTextBox.Visible = false;
                            RefreshVisualZoneData();
                            mWarningForm.Visible = true;
                            
                        }
                        break;

                    case VisualZone.DataFormat.HEX:
                        text = "";
                        for (int i = 0; i < datas.Length; i++)
                        {
                            byte[] oneByte = new byte[1] { datas[i] };
                            if (i != datas.Length - 1)
                            {
                                text += BitConverter.ToString(oneByte) + "  ";
                            }
                            else
                                text += BitConverter.ToString(oneByte);
                        }
                        if (EditeTextBox.Text != text)
                            EditeTextBox.Text = text;
                        else
                            EditeTextBox.Text = text + " ";
                        break;

                    case VisualZone.DataFormat.DEC:
                        text = "";
                        for (int i = 0; i < datas.Length; i++)
                        {
                            if (i != datas.Length - 1)
                            {
                                text += datas[i].ToString("d3") + " ";
                            }
                            else
                            {
                                text += datas[i].ToString("d3");
                            }
                        }

                        if (EditeTextBox.Text != text)
                            EditeTextBox.Text = text;
                        else
                            EditeTextBox.Text = text + " ";
                        break;
                }
            }
            catch
            {

            }
        }

        //2017/07/20 十进制
        private byte[] getEditBytesByDecStrArray(string inDecStr)
        {
            List<string> ret = new List<string>();
            string oneByte = "";
            for (int i = 0; i < inDecStr.Length; i++)
            {
                if (inDecStr[i] >= '0' && inDecStr[i] <= '9')
                    oneByte += inDecStr.Substring(i, 1);

                if (oneByte.Length == 3)
                {
                    ret.Add(oneByte);
                    oneByte = "";
                }
                else
                {
                    if (i == inDecStr.Length - 1 && oneByte.Length > 0)
                        ret.Add(oneByte);
                }
            }

            byte[] ret_byte = new byte[ret.Count];
            for (int i = 0; i < ret.Count; i++)
            {
                    ret_byte[i] = byte.Parse(ret[i]);
            }

            return ret_byte;
        }

        private byte[] getEditBytesByHexStrArray(string inHexStr)
        {
            List<string> ret=new List<string>();
            string oneByte = "";
            for (int i = 0; i < inHexStr.Length; i++)
            {
                if (inHexStr[i] >= '0' && inHexStr[i] <= '9' || inHexStr[i] >= 'A' && inHexStr[i] <= 'F' || inHexStr[i] >= 'a' && inHexStr[i] <= 'f')
                    oneByte += inHexStr.Substring(i, 1);

                if (oneByte.Length == 2)
                {
                    ret.Add(oneByte);
                    oneByte = "";
                }
                else
                {
                    if (i == inHexStr.Length - 1&&oneByte.Length>0)
                        ret.Add(oneByte);
                }
            }
            byte[] ret_byte = new byte[ret.Count];
            for (int i = 0; i < ret.Count; i++)
            {
                    ret_byte[i] = Convert.ToByte(ret[i], 16);
            }

            return ret_byte;
        }

        private byte[] getEditBytesByAsciiStrArray(string inAsciiStr)
        {
            try
            {
                List<byte> ret_list=new List<byte>();
                for (int i = 0; i < inAsciiStr.Length; i++)
                {

                    byte[] abyte = ConvertStringToBytes(inAsciiStr.Substring(i, 1));
                    ret_list.Add(abyte[0]);
                }
                byte[] ret = new byte[ret_list.Count];
                for (int i = 0; i < ret_list.Count; i++)
                {
                    ret[i] = ret_list[i];
                }
                return ret;
            }
            catch
            {
                return new byte[0];
            }
        }

        private void CheckEditContentAndProcess()
        {
            try
            {
                if (AllowCheckEditBytes && EditeTextBox.Focused)
                {
                    switch (mVisualZone.CurrentDataFormat)
                    {

                        case VisualZone.DataFormat.DEC://2017/07/20 十进制
                            if (IsDecStringTreble(CurrentEditTextBoxText))
                            {
                                byte[] dec_bytes = getEditBytesByDecStrArray(CurrentEditTextBoxText);
                                if (dec_bytes.Length != LastEditBytesAmount)
                                {
                                    RefreshBytesIntoBaseEditData(dec_bytes, SelectedRowNumber);
                                    RefreshVisualZoneData();
                                    getBaseDataForSelectedRow(SelectedRowNumber);
                                }
                            }
                            else
                            {
                                if (CurrentEditTextBoxText.Length == 0)
                                {
                                    byte[] dec_bytes = new byte[0];
                                    RefreshBytesIntoBaseEditData(dec_bytes, SelectedRowNumber);
                                    RefreshVisualZoneData();
                                    getBaseDataForSelectedRow(SelectedRowNumber);
                                }
                            }
                            
                            break;

                        case VisualZone.DataFormat.HEX:
                            
                            if (IsHexStringEven(CurrentEditTextBoxText))
                            {
                                byte[] edit_bytes = getEditBytesByHexStrArray(CurrentEditTextBoxText);
                                if (edit_bytes.Length != LastEditBytesAmount)
                                {
                                    RefreshBytesIntoBaseEditData(edit_bytes, SelectedRowNumber);
                                    RefreshVisualZoneData();
                                    getBaseDataForSelectedRow(SelectedRowNumber);
                                }
                            }
                            else
                            {
                                if (CurrentEditTextBoxText.Length == 0)
                                {
                                    byte[] edit_bytes = new byte[0];
                                    RefreshBytesIntoBaseEditData(edit_bytes, SelectedRowNumber);
                                    RefreshVisualZoneData();
                                    getBaseDataForSelectedRow(SelectedRowNumber);
                                }
                            }
                            break;
                        case VisualZone.DataFormat.ASCII:
                            byte[] ascii_bytes = getEditBytesByAsciiStrArray(CurrentEditTextBoxText);
                            for (int i = 0; i < ascii_bytes.Length; i++)
                            {
                                if (ascii_bytes[i] == 0x87)
                                    ascii_bytes[i] = 0x00;
                            }
                            if (ascii_bytes.Length != LastEditBytesAmount)
                            {
                                RefreshBytesIntoBaseEditData(ascii_bytes, SelectedRowNumber);
                                RefreshVisualZoneData();
                                getBaseDataForSelectedRow(SelectedRowNumber);

                            }
                            break;

                    }
                    AllowCheckEditBytes = false;
                }
            }
            catch
            {
            }
        }

        public void ForceRefreshEditedData()
        {
            try
            {
                if (EditeTextBox.Visible)
                {
                    switch (mVisualZone.CurrentDataFormat)
                    {
                        case VisualZone.DataFormat.HEX:
                            byte[] edit_bytes = getEditBytesByHexStrArray(CurrentEditTextBoxText);

                            RefreshBytesIntoBaseEditData(edit_bytes, SelectedRowNumber);
                            RefreshVisualZoneData();
                            getBaseDataForSelectedRow(SelectedRowNumber);
                            break;

                        case VisualZone.DataFormat.ASCII:
                            byte[] ascii_bytes = getEditBytesByAsciiStrArray(CurrentEditTextBoxText);
                            for (int i = 0; i < ascii_bytes.Length; i++)
                            {
                                if (ascii_bytes[i] == 0x87)
                                    ascii_bytes[i] = 0x00;
                            }
                            RefreshBytesIntoBaseEditData(ascii_bytes, SelectedRowNumber);
                            RefreshVisualZoneData();
                            getBaseDataForSelectedRow(SelectedRowNumber);

                            break;

                        case VisualZone.DataFormat.DEC://2017/07/20 十进制
                            byte[] dec_bytes = getEditBytesByDecStrArray(CurrentEditTextBoxText);

                            RefreshBytesIntoBaseEditData(dec_bytes, SelectedRowNumber);
                            RefreshVisualZoneData();
                            getBaseDataForSelectedRow(SelectedRowNumber);
                            break;

                    }
                }

            }
            catch
            {
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            CheckEditContentAndProcess();
        }

        private void EditeTextBox_LostFocus(object sender, EventArgs e)
        {
            EditeTextBox.Visible = false;
        }

        private void ReDrawBlockAddress()
        {
            try
            {
                AddressPanel.Refresh();
                AddressPanel.Location = new Point(0, 0);
                AddressPanel.Size = new Size(AddressColumnWidth, BasePanel.Size.Height);

                AddressPanel.BackColor = AddressZoneBackColor;
                Graphics g = AddressPanel.CreateGraphics();
                mVisualZone.RefreshAddressData(ref g, mDataMaster.RowsCount);

            }
            catch
            {
            }
        }

        private void RefreshScrollBarValues()
        {
            try
            {
                VisualScrollBar.Maximum = mDataMaster.RowsCount;
                VisualScrollBar.Minimum = 0;
                if (VisualScrollBar.Maximum < mVisualZone.VisualRowsAmount)
                    VisualScrollBar.Visible = false;
                else
                    VisualScrollBar.Visible = true;
            }
            catch
            {
            }
        }


        /// <summary>
        /// 为避开不同转换格式之间差异而造成的异常，以强制字节-字符转换的方式将字节数组转换成为字符串信息
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns></returns>
        public static string ConvertBytesToString(byte[] inBytes)
        {
            try
            {
                string ret = "";
                for (int i = 0; i < inBytes.Length; i++)
                {
                    if (inBytes[i] != 0x00)
                        ret += (char)inBytes[i];
                    else
                        ret += (char)DataMaster.DEFAULT_NULL_BYTE_ANOTHER_BYTE;
                }

                return ret;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 为避开不同转换格式之间差异而造成的异常，以强制字节-字符转换的方式将字符串信息转换成为字节数组
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns></returns>
        public static byte[] ConvertStringToBytes(string inStr)
        {
            try
            {
                byte[] ret = new byte[inStr.Length];
                for (int i = 0; i < ret.Length; i++)
                {
                    ret[i] = (byte)inStr[i];
                }

                return ret;
            }
            catch
            {
                return new byte[0];
            }
        }

    }
}