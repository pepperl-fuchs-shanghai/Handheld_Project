using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FA_RFIDTextBox
{
    public class RFIDByte
    {
        /// <summary>
        /// 将单个字节类型数据转化成为十六进制字符串
        /// </summary>
        /// <param name="inByte"></param>
        /// <returns></returns>
        public static string GetHexStringOfOneByte(ref byte inByte)
        {
            string ret = "";
            try
            {
                byte[] oneByte = new byte[1];
                oneByte[0] = inByte;
                ret = BitConverter.ToString(oneByte);
            }
            catch
            {
            }
            return ret;
        }

        /// <summary>
        /// 将单个十六进制字符串转化成为单个字节类型数据，inStr的长度必须等于2，且任意单个字符数值只能取值于0~F之间
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static byte GetOneByteOfHexString(ref string inStr)
        {
            byte ret = 0x00;
            try
            {
                if (inStr.Length == 2)
                    ret = Convert.ToByte(inStr, 16);
            }
            catch
            {
            }
            return ret;
        }

        /// <summary>
        /// 将单个ASCII码字符转化成为字节数据，inStr的长度必须为1
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static byte GetOneByteOfAsciiChar(ref string inStr)
        {
            byte ret = 0x00;
            try
            {
                if (inStr.Length == 1)
                {
                    byte[] oneByte = RFIDTextBox.ConvertStringToBytes(inStr);
                    ret = oneByte[0];
                }
            }
            catch
            {
            }
            return ret;
        }

        /// <summary>
        /// 获取单个字节的ASCII字符串
        /// </summary>
        /// <param name="inByte"></param>
        /// <returns></returns>
        public static string GetAsciiStrOfOneByte(ref byte inByte)
        {
            string ret = "";
            try
            {
                byte[] oneByte=new byte[1];
                oneByte[0]=inByte;
                ret = RFIDTextBox.ConvertBytesToString(oneByte);
            }
            catch
            {
            }

            return ret;
        }

        public static string GetDECStrOfOneByte(ref byte inByte)
        {
            string h_ret = "";

            try
            {
                h_ret = inByte.ToString("d3");
            }
            catch
            {
            }

            return h_ret;
        }//2017/07/19 十进制

        public static byte GetOneByteOfDECString(ref string inStr)
        {
            byte h_ret = 0x00;
            try
            {
                h_ret = byte.Parse(inStr);
            }
            catch
            {
                
            }
            return h_ret;
        }//2017/07/19 十进制

        protected byte mByte;

        public byte ByteValue
        {
            get { return mByte; }
            set { mByte = value; }
        }

        public string StringHex
        {
            get { return GetHexStringOfOneByte(ref mByte); }
            set { mByte = GetOneByteOfHexString(ref value); }
        }//2017/07/19 十进制

        public string StringASCII
        {
            get { return GetAsciiStrOfOneByte(ref mByte); }
            set { GetOneByteOfAsciiChar(ref value); }
        }

        public string StringDEC
        {
            get { return GetDECStrOfOneByte(ref mByte);}
            set { GetOneByteOfDECString(ref value); }
        }

        public RFIDByte()
        {
            mByte = 0x00;
        }

        public RFIDByte(byte inByte)
        {
            mByte = inByte;
        }
    }
}
