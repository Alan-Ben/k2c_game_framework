using System;
using System.Collections.Generic;

using System.Text;

namespace ALPackage
{
    /// <summary>
    /// 通用的配置字符串数据读取处理类
    /// 本类会对括号进行通用处理
    /// </summary>
    public class ALStringReader
    {
        //默认分割字符串
        private static char _g_defaultSplitChar = ' ';

        //读取的字符串
        private string _m_sStr;
        //读取的下标位置
        private int _m_iReadPos;

        //最后一个处理的分割字符串，方便解析的时候使用
        private char _m_cLastSplitChar;

        public ALStringReader(string _str)
        {
            _m_sStr = _str;
            _m_iReadPos = 0;

            _m_cLastSplitChar = _g_defaultSplitChar;
        }
        public ALStringReader(string _str, int _initReadPos)
        {
            _m_sStr = _str;
            _m_iReadPos = _initReadPos;

            _m_cLastSplitChar = _g_defaultSplitChar;
        }
        public ALStringReader(ALStringReader _reader)
        {
            _m_sStr = _reader._m_sStr;
            _m_iReadPos = _reader._m_iReadPos;

            _m_cLastSplitChar = _g_defaultSplitChar;
        }

        public string srcString { get { return _m_sStr; } }
        public char lastSplitChar { get { return _m_cLastSplitChar; } }

        //判断分割字符串是否有效
        public bool isSplitCharEnable(char _c)
        {
            if (_c == _g_defaultSplitChar)
                return false;

            return true;
        }

        /// <summary>
        /// 判断当前本数据读取对象是否已经是个空结果
        /// </summary>
        public bool isEmpty
        {
            get
            {
                if (null == _m_sStr)
                    return true;

                if (_m_iReadPos >= _m_sStr.Length)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// 使用分割符号分割，并且返回读取出来的第一段内容
        /// </summary>
        /// <returns></returns>
        public string readItem()
        {
            //默认使用：作为分隔符
            return readItem(':');
        }
        public string readItem(char _splitC)
        {
            if (null == _m_sStr)
            {
                _m_cLastSplitChar = _g_defaultSplitChar;
                return null;
            }

            if (_m_iReadPos >= _m_sStr.Length)
            {
                _m_cLastSplitChar = _g_defaultSplitChar;
                return null;
            }

            //从头开始处理
            StringBuilder builder = new StringBuilder();

            //括号层数
            int deep = 0;
            for (; _m_iReadPos < _m_sStr.Length;)
            {
                char tmpC = _m_sStr[_m_iReadPos];
                //读取标记前进
                _m_iReadPos++;

                //判断是否分隔符或者不在括号内
                if (tmpC == _splitC && deep <= 0)
                {
                    //记录最后一个分隔符
                    _m_cLastSplitChar = tmpC;

                    //如果头尾都是括号则删除头尾
                    if (builder.Length > 0 && builder[0] == '('
                            && builder[builder.Length - 1] == ')')
                    {
                        //先删除结尾的括号
                        builder.Remove(builder.Length - 1, 1);
                        builder.Remove(0, 1);
                    }

                    //无长度返回null
                    if (builder.Length <= 0)
                        return null;

                    return builder.ToString();
                }

                //添加到数据中
                builder.Append(tmpC);

                //判断是否括号，是则增加层数
                if (tmpC == '(')
                {
                    //增加深度
                    deep++;
                }
                else if (tmpC == ')')
                {
                    //减少深度
                    deep--;
                }
            }

            //如果括号未结束则报警告
            if (deep > 0)
            {
                ALLog.Warning($"String 【{_m_sStr}】 format error!");
            }

            //记录最后一个分隔符
            _m_cLastSplitChar = _g_defaultSplitChar;

            //如果头尾都是括号则删除头尾
            if (builder.Length > 0 && builder[0] == '('
                    && builder[builder.Length - 1] == ')')
            {
                //先删除结尾的括号
                builder.Remove(builder.Length - 1, 1);
                builder.Remove(0, 1);
            }

            //无长度返回null
            if (builder.Length <= 0)
                return null;

            //到结尾都没有则返回全部
            return builder.ToString();
        }
        public string readItem(char _splitC, char _splitC2)
        {
            if (null == _m_sStr)
            {
                _m_cLastSplitChar = _g_defaultSplitChar;
                return null;
            }

            if (_m_iReadPos >= _m_sStr.Length)
            {
                _m_cLastSplitChar = _g_defaultSplitChar;
                return null;
            }

            //从头开始处理
            StringBuilder builder = new StringBuilder();

            //括号层数
            int deep = 0;
            for (; _m_iReadPos < _m_sStr.Length;)
            {
                char tmpC = _m_sStr[_m_iReadPos];
                //读取标记前进
                _m_iReadPos++;

                //判断是否分隔符或者不在括号内
                if ((tmpC == _splitC || tmpC == _splitC2) && deep <= 0)
                {
                    //记录最后一个分隔符
                    _m_cLastSplitChar = tmpC;

                    //如果头尾都是括号则删除头尾
                    if (builder.Length > 0 && builder[0] == '('
                            && builder[builder.Length - 1] == ')')
                    {
                        //先删除结尾的括号
                        builder.Remove(builder.Length - 1, 1);
                        builder.Remove(0, 1);
                    }

                    //无长度返回null
                    if (builder.Length <= 0)
                        return null;

                    return builder.ToString();
                }

                //添加到数据中
                builder.Append(tmpC);

                //判断是否括号，是则增加层数
                if (tmpC == '(')
                {
                    //增加深度
                    deep++;
                }
                else if (tmpC == ')')
                {
                    //减少深度
                    deep--;
                }
            }

            //如果括号未结束则报警告
            if (deep > 0)
            {
                ALLog.Warning($"String 【{_m_sStr}】 format error!");
            }

            //记录最后一个分隔符
            _m_cLastSplitChar = _g_defaultSplitChar;

            //如果头尾都是括号则删除头尾
            if (builder.Length > 0 && builder[0] == '('
                    && builder[builder.Length - 1] == ')')
            {
                //先删除结尾的括号
                builder.Remove(builder.Length - 1, 1);
                builder.Remove(0, 1);
            }

            //无长度返回null
            if (builder.Length <= 0)
                return null;

            //到结尾都没有则返回全部
            return builder.ToString();
        }
        public String readItem(char[] _splitCArr)
        {
            if (null == _m_sStr)
            {
                _m_cLastSplitChar = _g_defaultSplitChar;
                return null;
            }

            if (_m_iReadPos >= _m_sStr.Length)
            {
                _m_cLastSplitChar = _g_defaultSplitChar;
                return null;
            }

            //从头开始处理
            StringBuilder builder = new StringBuilder();

            //括号层数
            int deep = 0;
            for (; _m_iReadPos < _m_sStr.Length;)
            {
                char tmpC = _m_sStr[_m_iReadPos];
                //读取标记前进
                _m_iReadPos++;

                //判断是否分隔符或者不在括号内
                if (_judgeCharInArr(tmpC, _splitCArr) && deep <= 0)
                {
                    //记录最后一个分隔符
                    _m_cLastSplitChar = tmpC;

                    //如果头尾都是括号则删除头尾
                    if (builder.Length > 0 && builder[0] == '('
                            && builder[builder.Length - 1] == ')')
                    {
                        //先删除结尾的括号
                        builder.Remove(builder.Length - 1, 1);
                        builder.Remove(0, 1);
                    }

                    //无长度返回null
                    if (builder.Length <= 0)
                        return null;

                    return builder.ToString();
                }

                //添加到数据中
                builder.Append(tmpC);

                //判断是否括号，是则增加层数
                if (tmpC == '(')
                {
                    //增加深度
                    deep++;
                }
                else if (tmpC == ')')
                {
                    //减少深度
                    deep--;
                }
            }

            //如果括号未结束则报警告
            if (deep > 0)
            {
                ALLog.Warning($"String 【{_m_sStr}】 format error!");
            }

            //记录最后一个分隔符
            _m_cLastSplitChar = _g_defaultSplitChar;

            //如果头尾都是括号则删除头尾
            if (builder.Length > 0 && builder[0] == '('
                    && builder[builder.Length - 1] == ')')
            {
                //先删除结尾的括号
                builder.Remove(builder.Length - 1, 1);
                builder.Remove(0, 1);
            }

            //无长度返回null
            if (builder.Length <= 0)
                return null;

            //到结尾都没有则返回全部
            return builder.ToString();
        }

        /// <summary>
        /// 判断在字符数组内是否有对应的数组
        /// </summary>
        /// <param name="_c"></param>
        /// <param name="_cArr"></param>
        /// <returns></returns>
        private static bool _judgeCharInArr(char _c, char[] _cArr)
        {
            for (int i = 0; i < _cArr.Length; i++)
            {
                if (_c == _cArr[i])
                    return true;
            }

            return false;
        }
    }
}

