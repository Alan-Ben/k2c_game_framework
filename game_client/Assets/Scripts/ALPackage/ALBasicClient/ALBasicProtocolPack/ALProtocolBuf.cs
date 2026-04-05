﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ALBasicProtocolPack
{
    public class ALProtocolBuf
    {
        public static System.Text.UTF8Encoding g_EncodingObj = new System.Text.UTF8Encoding();

        private byte[] _m_bBuf;
        private int _m_iReadPos;

        public static ALProtocolBuf allocate(int _bufSize)
        {
            if (_bufSize <= 0)
                return null;

            byte[] buf = new byte[_bufSize];
            ALProtocolBuf bufObj = new ALProtocolBuf(buf);

            return bufObj;
        }

        /*****************
         * 将需要读取的数据带入，并设置读取下标为0
         **/
        public ALProtocolBuf(byte[] _buf)
        {
            _m_bBuf = _buf;
            _m_iReadPos = 0;
        }

        public int getCurPos() { return _m_iReadPos; }
        public byte[] getBuf() { return _m_bBuf; }

        public void discard()
        {
            _m_bBuf = null;
            _m_iReadPos = 0;
        }

        /*****************
         * 重置读取写入的下标位置
         **/
        public void resetPosition()
        {
            _m_iReadPos = 0;
        }

        /*****************
         * 设置读取写入的下标位置
         **/
        public void setPosition(int _pos)
        {
            _m_iReadPos = _pos;
        }

        /*****************
         * 以下是协议内容读取对象的操作函数
         **/
        public byte get()
        {
            if(_m_iReadPos >= _m_bBuf.Length)
                return 0;

            return ALProtocolCommon.get(_m_bBuf, ref _m_iReadPos);
        }

        public short getShort()
        {
            if(_m_iReadPos >= _m_bBuf.Length)
                return 0;

            return ALProtocolCommon.getShort(_m_bBuf, ref _m_iReadPos);
        }

        public int getInt()
        {
            if(_m_iReadPos >= _m_bBuf.Length)
                return 0;

            return ALProtocolCommon.getInt(_m_bBuf, ref _m_iReadPos);
        }

        public float getFloat()
        {
            if(_m_iReadPos >= _m_bBuf.Length)
                return 0;

            return ALProtocolCommon.getFloat(_m_bBuf, ref _m_iReadPos);
        }

        public long getLong()
        {
            if(_m_iReadPos >= _m_bBuf.Length)
                return 0;

            return ALProtocolCommon.getLong(_m_bBuf, ref _m_iReadPos);
        }

        public double getDouble()
        {
            if(_m_iReadPos >= _m_bBuf.Length)
                return 0;

            return ALProtocolCommon.getDouble(_m_bBuf, ref _m_iReadPos);
        }

        public string getString()
        {
            if(_m_iReadPos >= _m_bBuf.Length)
                return string.Empty;

            return ALProtocolCommon.getString(_m_bBuf, ref _m_iReadPos);
        }

        /// <summary>
        /// 长字符串支持
        /// </summary>
        /// <returns></returns>
        public string getLString()
        {
            if (_m_iReadPos >= _m_bBuf.Length)
                return string.Empty;

            return ALProtocolCommon.getLString(_m_bBuf, ref _m_iReadPos);
        }

        public byte[] getByteBuffer()
        {
            if(_m_iReadPos >= _m_bBuf.Length)
                return null;

            //获取长度
            int len = getInt();

            if (0 >= len)
            {
                return null;
            }
            else
            {
                //从数组中将内容拷贝出来
                byte[] res = new byte[len];
                Array.Copy(_m_bBuf, _m_iReadPos, res, 0, len);

                _m_iReadPos += len;

                return res;
            }
        }

        /*****************
         * 以下是协议内容写入对象的操作函数
         **/
        public void put(byte _num)
        {
            _m_bBuf[_m_iReadPos] = _num;
            _m_iReadPos++;
        }

        private static object __shortMutex = new object();
        private static byte[] __shortBytes = new byte[2];
        public void putShort(short _num)
        {
            lock (__shortMutex)
            {
                for (int i = 0; i < 2; i++)
                {
                    __shortBytes[i] = (byte)((_num >> ((1 - i) * 8)) & 0xff);
                }

                //将数据拷贝到数组中
                __shortBytes.CopyTo(_m_bBuf, _m_iReadPos);
                _m_iReadPos += 2;
            }
        }

        private static object __intMutex = new object();
        private static byte[] __intBytes = new byte[4];
        public void putInt(int _num)
        {
            lock (__intMutex)
            {
                for (int i = 0; i < 4; i++)
                {
                    __intBytes[i] = (byte)((_num >> ((3 - i) * 8)) & 0xff);
                }

                //将数据拷贝到数组中
                __intBytes.CopyTo(_m_bBuf, _m_iReadPos);
                _m_iReadPos += 4;
            }
        }

        public void putFloat(float _num)
        {
            byte[] numBuf = BitConverter.GetBytes(_num);
            __reverse(numBuf);

            //将数据拷贝到数组中
            numBuf.CopyTo(_m_bBuf, _m_iReadPos);
            _m_iReadPos += 4;

            numBuf = null;
        }

        private static object __longMutex = new object();
        private static byte[] __longBytes = new byte[8];
        public void putLong(long _num)
        {
            lock (__longMutex)
            {
                for (int i = 0; i < 8; i++)
                {
                    __longBytes[i] = (byte)((_num >> ((7 - i) * 8)) & 0xff);
                }

                //将数据拷贝到数组中
                __longBytes.CopyTo(_m_bBuf, _m_iReadPos);
                _m_iReadPos += 8;
            }
        }

        public void putDouble(double _num)
        {
            byte[] numBuf = BitConverter.GetBytes(_num);
            __reverse(numBuf);

            //将数据拷贝到数组中
            numBuf.CopyTo(_m_bBuf, _m_iReadPos);
            _m_iReadPos += 8;

            numBuf = null;
        }

        public void putString(string _str)
        {
            if (null == _str || _str.Length <= 0)
            {
                //写入长度
                putShort((short)0);
            }
            else
            {
                byte[] strBuf = g_EncodingObj.GetBytes(_str);

                //写入长度
                putShort((short)strBuf.Length);

                //将数据拷贝到数组中
                strBuf.CopyTo(_m_bBuf, _m_iReadPos);
                _m_iReadPos += strBuf.Length;

                strBuf = null;
            }
        }

        /// <summary>
        /// 长字符串支持
        /// </summary>
        /// <param name="_str"></param>
        public void putLString(string _str)
        {
            if (null == _str || _str.Length <= 0)
            {
                //写入长度
                putInt(0);
            }
            else
            {
                byte[] strBuf = g_EncodingObj.GetBytes(_str);

                //写入长度
                putInt(strBuf.Length);

                //将数据拷贝到数组中
                strBuf.CopyTo(_m_bBuf, _m_iReadPos);
                _m_iReadPos += strBuf.Length;

                strBuf = null;
            }
        }

        public void putByteBuffer(byte[] _buf)
        {
            if (null == _buf || _buf.Length <= 0)
            {
                //写入长度
                putInt(0);
            }
            else
            {
                //写入长度
                putInt(_buf.Length);

                //将数据拷贝到数组中
                _buf.CopyTo(_m_bBuf, _m_iReadPos);
                _m_iReadPos += _buf.Length;
            }
        }

        public void cpyByteBuffer(byte[] _buf)
        {
            //将数据拷贝到数组中
            _buf.CopyTo(_m_bBuf, _m_iReadPos);
            _m_iReadPos += _buf.Length;
        }

        /**************
         * 反转数组
         **/
        private void __reverse(byte[] _buf)
        {
            byte tmp;
            int half = _buf.Length / 2;

            for (int i = 0; i < half; i++)
            {
                int lastIdx = _buf.Length - i - 1;
                tmp = _buf[lastIdx];
                _buf[lastIdx] = _buf[i];
                _buf[i] = tmp;
            }
        }

        /** 释放资源 */
        public void dispose()
        {
            _m_bBuf = null;
            _m_iReadPos = 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _m_bBuf.Length; i++)
            {
                sb.Append($"{_m_bBuf[i]},");
            }
            return $"{nameof(_m_bBuf)}: {sb}, {nameof(_m_iReadPos)}: {_m_iReadPos}";
        }
    }
}
