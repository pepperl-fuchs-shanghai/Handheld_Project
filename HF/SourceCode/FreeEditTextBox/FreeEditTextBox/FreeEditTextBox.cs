using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FreeEditTextBox
{
    public partial class FreeEditTextBox : UserControl
    {
        private const byte NULL_TRANSFER_BYTE_VALUE = 0x87;
        private WarningForm mWarningForm = new WarningForm();

       
        public FreeEditTextBox()
        {
            InitializeComponent();
            mEditBytes = new byte[0];
            mRawBytes = new byte[0];
            CurrentDataFormat = DataFormat.HEX;
            ByteInterval = 4;
            BlockLength=4;

            mWarningForm.Location = new Point(20, 30);

        }
        public delegate void EventDelegate(object sender, EventArgs e);
        public event EventDelegate DataLengthChanged;

        private byte[] mRawBytes;
        private byte[] mEditBytes;
        private bool mDanishReadSuccess = true;
        public bool DanishReadSuccess
        {
            get { return mDanishReadSuccess; }
            set{mDanishReadSuccess=value;}
        }
        public Byte[] DataBytes
        {
            set
            {
                InitializeDataBytes(value);
                switch (CurrentDataFormat)
                {
                    case DataFormat.ASCII:
                        bool isContain87 = false;
                        for (int i = 0; i < mEditBytes.Length; i++)
                        {
                            if (mRawBytes[i] == NULL_TRANSFER_BYTE_VALUE)
                            {
                                isContain87 = true;
                                break;
                            }
                        }

                        if (!isContain87)
                        {
                            EditTextBox.Enabled = true;
                            EditTextBox.Text = ConvertBytesIntoAsciiString(mEditBytes);
                        }
                        else
                        {
                            EditTextBox.Enabled = false;
                            EditTextBox.Text = ConvertBytesIntoAsciiString(mEditBytes);
                            mWarningForm.Visible = true;

                        }
                        break;
                    case DataFormat.HEX:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoHexString(mEditBytes);
                        break;
                    case DataFormat.DEC:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoDecString(mEditBytes);
                        break;
                    case DataFormat.Ringsted:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoRingstedString(mEditBytes);
                        break;
                    case DataFormat.Blans:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoBlansString(mEditBytes);
                        break;
                    case DataFormat.Horsens:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoHorsensString(mEditBytes);
                        break;
                }
            }

            get
            {
                switch (CurrentDataFormat)
                {
                    case DataFormat.ASCII:
                        getEditBytesByAscii(EditTextBox.Text);
                        break;
                    case DataFormat.HEX:
                        getEditBytesByHexStr(EditTextBox.Text);
                        break;

                    case DataFormat.DEC:
                        getEditBytesByDecStr(EditTextBox.Text);
                        break;

                    default:
                        return new byte[0];
                }

                byte[] ret = new byte[mEditBytes.Length];
                for (int i = 0; i < ret.Length; i++)
                {
                    if (mEditBytes[i] == NULL_TRANSFER_BYTE_VALUE)
                    {
                        if (i < mRawBytes.Length && mRawBytes[i] != NULL_TRANSFER_BYTE_VALUE)
                            ret[i] = mRawBytes[i];
                        else
                            ret[i] = NULL_TRANSFER_BYTE_VALUE;
                    }
                    else
                        ret[i] = mEditBytes[i];
                }
                return ret;
            }
        }

        private string ConvertBytesIntoHorsensString(byte[] mEditBytes)
        {
            try
            {
                if (mEditBytes.Length < 5)
                    return "";
                for (int i = 0; i < mEditBytes.Length; i ++)
                {
                    if (mEditBytes[i] >= '0' && mEditBytes[i] <= '9' || mEditBytes[i] >= 'A' && mEditBytes[i] <= 'F' || mEditBytes[i] >= 'a' && mEditBytes[i] <= 'f')
                    { }
                    else
                    {
                        DanishReadSuccess = false;
                        MessageBox.Show("Fixcode value out of range!\r\n  HEX     ASC    DEC\r\n30-39h | 0-9 | 48-57");
                        return "";
                    }
                }
                string hexString = "";
                for (int i = 2; i < mEditBytes.Length - 1; i++)
                {
                    byte[] onebyte = new byte[1];
                    onebyte[0] = mEditBytes[i];
                    hexString += BitConverter.ToString(onebyte);
                }
                int len = hexString.Length;
                int result = 0;
                for (int i = 0; i < len; i++)
                {
                    string temp = hexString.Substring(i, 1);
                    int num = 0;
                    switch (temp)
                    {
                        case "A": num = 10; break;
                        case "B": num = 11; break;
                        case "C": num = 12; break;
                        case "D": num = 13; break;
                        case "E": num = 14; break;
                        case "F": num = 15; break;
                        case "a": num = 10; break;
                        case "b": num = 11; break;
                        case "c": num = 12; break;
                        case "d": num = 13; break;
                        case "e": num = 14; break;
                        case "f": num = 15; break;
                        default:
                            num = int.Parse(hexString.Substring(i, 1)); break;
                    }
                    result += num * (int)Math.Pow((double)16, (double)(len - 1 - i));
                }

                string endString = ((int)mEditBytes[0]).ToString("000") + " " + ((int)mEditBytes[1]).ToString("000") + " " + result.ToString() + " " + ((int)mEditBytes[4]).ToString("000");
                DanishReadSuccess = true;
                return endString;
            }
            catch (Exception ex)
            {
                DanishReadSuccess = false;
                return ""; }

        }

        private string ConvertBytesIntoBlansString(byte[] mEditBytes)
        {

            try
            {
                if (mEditBytes.Length < 5)
                    return "";
                for (int i = 0; i < mEditBytes.Length; i ++)
                {
                    if (mEditBytes[i] >= '0' && mEditBytes[i] <= '9' || mEditBytes[i] >= 'A' && mEditBytes[i] <= 'F' || mEditBytes[i] >= 'a' && mEditBytes[i] <= 'f')
                    { }
                    else
                    {
                        DanishReadSuccess = false;
                        MessageBox.Show("Fixcode value out of range!\r\n  HEX     ASC    DEC\r\n30-39h | 0-9 | 48-57");
                        return "";
                    }
                }
                string hexString = "";
                for (int i = 0; i < mEditBytes.Length - 1; i++)
                {
                    byte[] onebyte=new byte [1];
                    onebyte[0]=mEditBytes[i];
                    hexString += BitConverter.ToString(onebyte);
                }
                int len = hexString.Length;
                int result = 0;
                for (int i = 0; i < len; i++)
                {
                    string temp = hexString.Substring(i, 1);
                    int num = 0;
                    switch (temp)
                    {
                        case "A": num = 10; break;
                        case "B": num = 11; break;
                        case "C": num = 12; break;
                        case "D": num = 13; break;
                        case "E": num = 14; break;
                        case "F": num = 15; break;
                        case "a": num = 10; break;
                        case "b": num = 11; break;
                        case "c": num = 12; break;
                        case "d": num = 13; break;
                        case "e": num = 14; break;
                        case "f": num = 15; break;
                        default:
                            num = int.Parse(hexString.Substring(i, 1)); break;
                    }
                    result += num * (int)Math.Pow((double)16, (double)(len - 1 - i));
                }
                DanishReadSuccess = true;
                return result.ToString();
            }
            catch (Exception ex)
            {
                DanishReadSuccess = false;
                return ""; }
        }

        private string ConvertBytesIntoRingstedString(byte[] mEditBytes)
        {

            try
            {
                if (mEditBytes.Length < 5)
                    return "";
                for (int i = 0; i < mEditBytes.Length; i ++)
                {
                    if (mEditBytes[i] >= '0' && mEditBytes[i] <= '9' )
                    { }
                    else
                    {
                        DanishReadSuccess = false;
                        MessageBox.Show("Fixcode value out of range!\r\n  HEX     ASC    DEC\r\n30-39h | 0-9 | 48-57");
                        return "";
                    }
                }
                int len = mEditBytes.Length;
                string asciiString = Encoding.ASCII.GetString(mEditBytes,0,mEditBytes.Length);
                int result = 0;
                for (int i = 0; i < len; i++)
                {
                    string temp=asciiString.Substring(i, 1);
                    int num = 0;
                    switch (temp)
                    {
                        case "A": num = 10; break;
                        case "B": num = 11; break;
                        case "C": num = 12; break;
                        case "D": num = 13; break;
                        case "E": num = 14; break;
                        case "F": num = 15; break;
                        case "a": num = 10; break;
                        case "b": num = 11; break;
                        case "c": num = 12; break;
                        case "d": num = 13; break;
                        case "e": num = 14; break;
                        case "f": num = 15; break;
                        default:
                            num = int.Parse(asciiString.Substring(i, 1)); break;
                    }
                    result += num * (int)Math.Pow((double)16, (double)(len - 1 - i));
                }
                DanishReadSuccess = true;
                return result.ToString();
            }
            catch (Exception ex)
            {
                DanishReadSuccess = false;
                return ""; }
        }
        public int DataLength
        {
            get { return mEditBytes.Length; }
        }

        public enum DataFormat
        {
            ASCII,HEX,DEC,Ringsted,Blans,Horsens
        }
        private DataFormat mCurrentDataFormat;
        public DataFormat CurrentDataFormat
        {
            get { return mCurrentDataFormat; }
            set
            {

                mCurrentDataFormat = value;

                switch (CurrentDataFormat)
                {
                    case DataFormat.ASCII:
                        bool isContain87=false;
                        for (int i = 0; i < mEditBytes.Length; i++)
                        {
                            if (mRawBytes[i] == NULL_TRANSFER_BYTE_VALUE)
                            {
                                isContain87 = true;
                                break;
                            }
                        }

                        if (!isContain87)
                        {
                            EditTextBox.Enabled = true;
                            EditTextBox.Text = ConvertBytesIntoAsciiString(mEditBytes);
                        }
                        else
                        {
                            EditTextBox.Enabled = false;
                            EditTextBox.Text = ConvertBytesIntoAsciiString(mEditBytes);
                            mWarningForm.Visible = true;
                            
                        }
                        break;
                    case DataFormat.HEX:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoHexString(mEditBytes);
                        break;
                    case DataFormat.DEC:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoDecString(mEditBytes);
                        break;
                    case DataFormat.Ringsted:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoRingstedString(mEditBytes);
                        break;
                    case DataFormat.Blans:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoBlansString(mEditBytes);
                        break;
                    case DataFormat.Horsens:
                        EditTextBox.Enabled = true;
                        EditTextBox.Text = ConvertBytesIntoHorsensString(mEditBytes);
                        break;
                }

            }
        }
        public int ByteInterval;
        public int BlockLength;


        private string FormatHexOutput(ref byte[] bytes,ref int blockLength,ref int byteInterval)
        {
            string ret = "";
            try
            {
                if (bytes != null)
                {
                    int per_line_count = 0;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        if (per_line_count >= blockLength)
                        {
                            ret += "\r\n";
                            per_line_count = 0;
                        }

                        byte[] oneByte = new byte[1];
                        oneByte[0] = bytes[i];
                        ret += BitConverter.ToString(oneByte);
                        int j = 0;
                        while (j < byteInterval)
                        {
                            ret += " ";
                            j++;
                        }
                        per_line_count++;
                    }
                }
            }
            catch
            {
            }

            return ret;
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

                        if (ch[i] >= '0' && ch[i] <= '9' || ch[i] >= 'A' && ch[i] <= 'F' || ch[i] >= 'a' && ch[i] <= 'f')
                        {
                            ret += ch[i].ToString();
                            count++;

                            if (count % 2 == 0)
                            {
                                if (i < ch.Length - 1)
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

        private void getEditBytesByHexStr(string inHexStr)
        {
            List<string> ret = new List<string>();
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
                    if (i == inHexStr.Length - 1 && oneByte.Length > 0)
                        ret.Add(oneByte);
                }
            }
            byte[] ret_byte = new byte[ret.Count];
            for (int i = 0; i < ret.Count; i++)
            {
                ret_byte[i] = Convert.ToByte(ret[i], 16);
            }

            InitializeDataBytes(ret_byte);
        }

        private void getEditBytesByDecStr(string inHexStr)
        {

            try
            {
                List<byte> ByteList = new List<byte>();
                string[] HexBytes = inHexStr.Split(new char[] { ' ' });
                foreach(string mString in HexBytes)
                {
                    int iByte = -1;
                    try
                    {
                        iByte = int.Parse(mString);
                        if ((0 <= iByte) && (iByte <= 0xFF))
                            ByteList.Add((byte)iByte);
                    }
                    catch
                    {
                        continue;
                    }
                }
                byte[] ret_byte = ByteList.ToArray();
                InitializeDataBytes(ret_byte);
            }
            catch (Exception Error)
            {


            }
        }



        private void getEditBytesByAscii(string inAsciiStr)
        {
            try
            {
                byte[] ret = new byte[inAsciiStr.Length];
                for (int i = 0; i < inAsciiStr.Length; i++)
                {
                    ret[i] = (byte)inAsciiStr[i];
                    
                    if (i < mRawBytes.Length&&ret[i] == NULL_TRANSFER_BYTE_VALUE)
                    {
                        if (mRawBytes[i] == 0x00)
                            ret[i] = mRawBytes[i];
                    }
                }

                mEditBytes = ret;

            }
            catch
            {
            }
        }


        private void InitializeDataBytes(byte[] inBytes)
        {
            try
            {
                mRawBytes = new byte[inBytes.Length];
                mEditBytes = new byte[inBytes.Length];
                for (int i = 0; i < inBytes.Length; i++)
                {
                    mRawBytes[i] = inBytes[i];
                    if (inBytes[i] == 0x00)
                        mEditBytes[i] = NULL_TRANSFER_BYTE_VALUE;
                    else
                        mEditBytes[i] = inBytes[i];
                }
            }
            catch
            {
            }
        }

        private string ConvertBytesIntoAsciiString(byte[] inBytes)
        {
            try
            {
                string ret = "";
                for (int i = 0; i < inBytes.Length; i++)
                {
                    ret += (char)inBytes[i];
                }
                return ret;
            }
            catch
            {
                return "";
            }
        }

        private string ConvertBytesIntoHexString(byte[] inBytes)
        {
            try
            {
                string ret = "";
                for (int i = 0; i < inBytes.Length; i++)
                {
                    byte[] oneByte=new byte[1];
                    if (inBytes[i] != NULL_TRANSFER_BYTE_VALUE)
                    {
                        oneByte[0]=inBytes[i];
                    }
                    else
                    {
                        if (i < mRawBytes.Length && mRawBytes[i] != NULL_TRANSFER_BYTE_VALUE)
                            oneByte[0] = mRawBytes[i];
                        else
                            oneByte[0] = inBytes[i];
                    }
                    ret += BitConverter.ToString(oneByte);
                }
                return ret;
            }
            catch
            {
                return "";
            }
        }

        private string ConvertBytesIntoDecString(byte[] inBytes)
        {
            try
            {
                string ret = "";
                for (int i = 0; i < inBytes.Length; i++)
                {
                    byte[] oneByte = new byte[1];
                    if (inBytes[i] != NULL_TRANSFER_BYTE_VALUE)
                    {
                        oneByte[0] = inBytes[i];
                    }
                    else
                    {
                        if (i < mRawBytes.Length && mRawBytes[i] != NULL_TRANSFER_BYTE_VALUE)
                            oneByte[0] = mRawBytes[i];
                        else
                            oneByte[0] = inBytes[i];
                    }
                    ret += ((int)oneByte[0]).ToString()+" ";
                }
                return ret;
            }
            catch
            {
                return "";
            }
        }


        private int selectedIndex = 0;
        private bool isEditTextBoxModified = false;

        private void EditTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!isEditTextBoxModified)
                {
                    selectedIndex = EditTextBox.SelectionStart;

                    switch (CurrentDataFormat)
                    {
                        case DataFormat.HEX:
                            int relativeIndex = getRelativeSelectedStartForHexString(EditTextBox.Text, selectedIndex);
                            string ret = getEditTextBoxStandardHexString(EditTextBox.Text);
                            selectedIndex = getAbsoluteSelectedStartForHexString(ret, relativeIndex);
                            if (EditTextBox.Text != ret)
                            {
                                isEditTextBoxModified = true;
                                EditTextBox.Text = ret;
                                EditTextBox.SelectionStart = selectedIndex;
                            }
                            else
                            {
                                getEditBytesByHexStr(EditTextBox.Text);
                            }
                            break;
                        case DataFormat.ASCII:
                            getEditBytesByAscii(EditTextBox.Text);
                            break;
                        case DataFormat.DEC:
                            getEditBytesByDecStr(EditTextBox.Text);
                            break;
                    }
                }
                else
                {
                    isEditTextBoxModified = false;
                    switch (CurrentDataFormat)
                    {
                        case DataFormat.HEX:
                            getEditBytesByHexStr(EditTextBox.Text);
                            break;

                        case DataFormat.ASCII:
                            getEditBytesByAscii(EditTextBox.Text);
                            break;
                        case DataFormat.DEC:
                            getEditBytesByDecStr(EditTextBox.Text);
                            break;

                    }
                }
            }
            catch
            {
            }
        }
        private int lastDataLength = 0;
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (mEditBytes.Length != lastDataLength)
                {
                    if (DataLengthChanged != null)
                    {
                        DataLengthChanged.Invoke(this, new EventArgs());
                    }
                    lastDataLength = mEditBytes.Length;
                }
            }
            catch
            {
            }
        }

        public Color BoxBackColor
        {
            get { return EditTextBox.BackColor; }
            set { EditTextBox.BackColor = value; }
        }

        public Color BoxForeColor
        {
            get { return EditTextBox.ForeColor; }
            set { EditTextBox.ForeColor = value; }
        }

        private void EditTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //MessageBox.Show("KeyChar["+e.KeyChar.ToString()+"]");
                if (EditTextBox.Focused == true)
                {
                    if (e.KeyChar == 0x0d)
                    {

                        //EditTextBox.Text += " ";
                        int insertStart = EditTextBox.SelectionStart;
                        string TempText = EditTextBox.Text;
                        StringBuilder mstringBuild = new StringBuilder();
                        mstringBuild.Append(TempText);
                        mstringBuild.Insert(EditTextBox.SelectionStart, " ");
                        EditTextBox.Text = mstringBuild.ToString();
                        e.Handled = true;
                        EditTextBox.SelectionStart = insertStart+1;
                    }

                }

            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.ToString());
            }
        }


    }
}