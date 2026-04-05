using System;
using System.Collections.Generic;
using System.Text;

namespace ALBasicProtocolPack
{
    public class ALProtocolCommon
    {
        /*****************
         * 将字节转化为int对象
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 11:30:30 AM
         */
        public static int byte2int(byte value) { return (int)value & 0xFF; }
        /*****************
         * 以下是协议内容读取对象的操作函数
         **/
        public static byte get(byte[] _buf, ref int _idx)
        {
            if (_idx >= _buf.Length)
                return 0;

            byte res = _buf[_idx];
            _idx++;

            return res;
        }

        private static byte[] _shortArr = new byte[2];
        public static short getShort(byte[] _buf, ref int _idx)
        {
            if (_idx + 1 >= _buf.Length)
                return 0;

            lock (_shortArr)
            {
                _shortArr[0] = _buf[_idx + 1];
                _shortArr[1] = _buf[_idx];
                short res = BitConverter.ToInt16(_shortArr, 0);
                _idx += 2;

                return res;
            }
        }

        private static byte[] _intArr = new byte[4];
        public static int getInt(byte[] _buf, ref int _idx)
        {
            if (_idx + 3 >= _buf.Length)
                return 0;

            lock (_intArr)
            {
                _intArr[0] = _buf[_idx + 3];
                _intArr[1] = _buf[_idx + 2];
                _intArr[2] = _buf[_idx + 1];
                _intArr[3] = _buf[_idx];
                int res = BitConverter.ToInt32(_intArr, 0);
                _idx += 4;

                return res;
            }
        }

        private static byte[] _floatArr = new byte[4];
        public static float getFloat(byte[] _buf, ref int _idx)
        {
            if (_idx + 3 >= _buf.Length)
                return 0;

            lock (_floatArr)
            {
                _floatArr[0] = _buf[_idx + 3];
                _floatArr[1] = _buf[_idx + 2];
                _floatArr[2] = _buf[_idx + 1];
                _floatArr[3] = _buf[_idx];
                float res = BitConverter.ToSingle(_floatArr, 0);
                _idx += 4;

                return res;
            }
        }

        private static byte[] _longArr = new byte[8];
        public static long getLong(byte[] _buf, ref int _idx)
        {
            if (_idx + 7 >= _buf.Length)
                return 0;

            lock (_longArr)
            {
                _longArr[0] = _buf[_idx + 7];
                _longArr[1] = _buf[_idx + 6];
                _longArr[2] = _buf[_idx + 5];
                _longArr[3] = _buf[_idx + 4];
                _longArr[4] = _buf[_idx + 3];
                _longArr[5] = _buf[_idx + 2];
                _longArr[6] = _buf[_idx + 1];
                _longArr[7] = _buf[_idx];
                long res = BitConverter.ToInt64(_longArr, 0);
                _idx += 8;

                return res;
            }
        }

        private static byte[] _doubleArr = new byte[8];
        public static double getDouble(byte[] _buf, ref int _idx)
        {
            if (_idx + 7 >= _buf.Length)
                return 0;

            lock (_doubleArr)
            {
                _doubleArr[0] = _buf[_idx + 7];
                _doubleArr[1] = _buf[_idx + 6];
                _doubleArr[2] = _buf[_idx + 5];
                _doubleArr[3] = _buf[_idx + 4];
                _doubleArr[4] = _buf[_idx + 3];
                _doubleArr[5] = _buf[_idx + 2];
                _doubleArr[6] = _buf[_idx + 1];
                _doubleArr[7] = _buf[_idx];
                double res = BitConverter.ToDouble(_doubleArr, 0);
                _idx += 8;

                return res;
            }
        }

        public static string getString(byte[] _buf, ref int _idx)
        {
            //获取字符串长度
            short strLen = getShort(_buf, ref _idx);

            if (0 >= strLen)
            {
                return string.Empty;
            }
            else
            {
                //读取字符串
                string str = Encoding.UTF8.GetString(_buf, _idx, strLen);
                _idx += strLen;

                return str;
            }
        }

        /// <summary>
        /// 长字符串支持
        /// </summary>
        /// <param name="_buf"></param>
        /// <param name="_idx"></param>
        /// <returns></returns>
        public static string getLString(byte[] _buf, ref int _idx)
        {
            //获取字符串长度
            int strLen = getInt(_buf, ref _idx);

            if (0 >= strLen)
            {
                return string.Empty;
            }
            else
            {
                //读取字符串
                string str = Encoding.UTF8.GetString(_buf, _idx, strLen);
                _idx += strLen;

                return str;
            }
        }

        /****************
         * 获取Int型数据在压缩数据后的数据大小
         * 需要多一字节存放长度
         * 
         * @author alzq.z
         * @time   Jan 23, 2013 11:25:38 PM
         */
        public static int GetIntZipSize(int _num)
        {
            if (_num > byte.MinValue && _num < byte.MaxValue)
                return 2;
            else if (_num > short.MinValue && _num < short.MaxValue)
                return 3;

            return 5;
        }

        /****************
         * 获取Long型数据在压缩数据后的数据大小
         * 需要多一字节存放长度
         * 
         * @author alzq.z
         * @time   Jan 23, 2013 11:25:38 PM
         */
        public static int GetLongZipSize(long _num)
        {
            if (_num > byte.MinValue && _num < byte.MaxValue)
                return 2;
            else if (_num > short.MinValue && _num < short.MaxValue)
                return 3;
            else if (_num > int.MinValue && _num < int.MaxValue)
                return 5;

            return 9;
        }

        /****************
         * 将Int数据通过压缩方式放入内存块中
         * 
         * @author alzq.z
         * @time   Jan 23, 2013 11:25:38 PM
         */
        public static void ZipPutIntIntoBuf(ALProtocolBuf _buff, int _num)
        {
            if (_num > byte.MinValue && _num < byte.MaxValue)
            {
                //put the length
                _buff.put((byte)1);
                //put the value
                _buff.put((byte)_num);
                return;
            }
            else if (_num > short.MinValue && _num < short.MaxValue)
            {
                //put the length
                _buff.put((byte)2);
                //put the value
                _buff.putShort((short)_num);
                return;
            }

            //put the length
            _buff.put((byte)4);
            //put the value
            _buff.putInt(_num);
        }

        /****************
         * 获取Long型数据在压缩数据后的数据大小
         * 需要多一字节存放长度
         * 
         * @author alzq.z
         * @time   Jan 23, 2013 11:25:38 PM
         */
        public static void ZipPutLongIntoBuf(ALProtocolBuf _buff, long _num)
        {
            if (_num > byte.MinValue && _num < byte.MaxValue)
            {
                //put the length
                _buff.put((byte)1);
                //put the value
                _buff.put((byte)_num);
                return;
            }
            else if (_num > short.MinValue && _num < short.MaxValue)
            {
                //put the length
                _buff.put((byte)2);
                //put the value
                _buff.putShort((short)_num);
                return;
            }
            else if (_num > int.MinValue && _num < int.MaxValue)
            {
                //put the length
                _buff.put((byte)4);
                //put the value
                _buff.putInt((int)_num);
                return;
            }


            //put the length
            _buff.put((byte)8);
            //put the value
            _buff.putLong(_num);
        }

        /****************
         * 将Int数据通过压缩方式从内存方式
         * 
         * @author alzq.z
         * @time   Jan 23, 2013 11:25:38 PM
         */
        public static int ZipGetIntFromBuf(ALProtocolBuf _buff)
        {
            byte size = _buff.get();

            if (size == 1)
                return _buff.get();
            else if (size == 2)
                return _buff.getShort();

            return _buff.getInt();
        }

        /****************
         * 将Long数据通过压缩方式从内存方式
         * 
         * @author alzq.z
         * @time   Jan 23, 2013 11:25:38 PM
         */
        public static long ZipGetLongFromBuf(ALProtocolBuf _buff)
        {
            byte size = _buff.get();

            if (size == 1)
                return _buff.get();
            else if (size == 2)
                return _buff.getShort();
            else if (size == 4)
                return _buff.getInt();

            return _buff.getLong();
        }

        /***********
         * 获取发送字符串所需要的包长度
         * @param str
         * @return
         */
        public static int GetStringBufSize(String str)
        {
            if (null == str || str.Length <= 0)
            {
                return 2;
            }

            return ALProtocolBuf.g_EncodingObj.GetByteCount(str) + 2;
        }

        /***********
         * 增加长字符串支持
         * @param str
         * @return
         */
        public static int GetLStringBufSize(String str)
        {
            if (null == str || str.Length <= 0)
            {
                return 4;
            }

            return ALProtocolBuf.g_EncodingObj.GetByteCount(str) + 4;
        }
    }
}
