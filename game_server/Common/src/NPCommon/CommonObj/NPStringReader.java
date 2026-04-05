package NPCommon.CommonObj;

import ALServerLog.ALServerLog;

/**************
 * 字符串在进行一些复杂配置的时候的读取行为对象
 * 主要是不进行统一分割，而是逐个参数读取
 * 分隔符以:为判断对象进行处理
 * 如果遇到嵌套括号，可以根据嵌套进行完整读取
 *
 * @author mj
 *
 */
public class NPStringReader
{
    //默认的分隔符值
    private static char _g_defaultSplitChar = ' ';

    //源字符串
    private String _m_sString;
    //当前读取的文字下标位置
    private int _m_iReadPos;

    //记录最后一个拆分出来的字符，可以用于外部判断
    private char _m_cLastSplitChar;

    public NPStringReader(String _str)
    {
        _m_sString = _str;
        _m_iReadPos = 0;

        _m_cLastSplitChar = _g_defaultSplitChar;
    }

    public NPStringReader(String _str, int _initPos)
    {
        _m_sString = _str;
        _m_iReadPos = _initPos;

        _m_cLastSplitChar = _g_defaultSplitChar;
    }

    public NPStringReader(NPStringReader _reader)
    {
        _m_sString = _reader._m_sString;
        _m_iReadPos = _reader._m_iReadPos;

        _m_cLastSplitChar = _g_defaultSplitChar;
    }

    public String getSrcString()
    {
        return _m_sString;
    }

    public char getLastSpliteChar()
    {
        return _m_cLastSplitChar;
    }

    public boolean isSpliteCharEnable(char _c)
    {
        return _c != _g_defaultSplitChar;
    }

    public boolean isEmpty()
    {
        if (null == _m_sString)
            return true;

        if (_m_iReadPos >= _m_sString.length())
            return true;

        return _m_sString.isEmpty();
    }

    /************
     * 根据拆分字符读取下一个关键字
     * @return
     */
    public String readItem()
    {
        //默认使用:作为分隔
        return readItem(':');
    }

    public String readItem(char _splitC)
    {
        if (null == _m_sString)
        {
            _m_cLastSplitChar = _g_defaultSplitChar;
            return null;
        }

        if (_m_iReadPos >= _m_sString.length())
        {
            _m_cLastSplitChar = _g_defaultSplitChar;
            return null;
        }

        //从头开始处理
        StringBuilder builder = new StringBuilder();

        //括号层数
        int deep = 0;
        for (; _m_iReadPos < _m_sString.length(); )
        {
            char tmpC = _m_sString.charAt(_m_iReadPos);
            //读取标记前进
            _m_iReadPos++;

            //判断是否分隔符或者不在括号内
            if (tmpC == _splitC && deep <= 0)
            {
                //记录最后一个分隔符
                _m_cLastSplitChar = tmpC;

                //如果头尾都是括号则删除头尾
                if (builder.length() > 0 && builder.charAt(0) == '('
                        && builder.charAt(builder.length() - 1) == ')')
                {
                    //先删除结尾的括号
                    builder.deleteCharAt(builder.length() - 1);
                    builder.deleteCharAt(0);
                }

                //无长度返回null
                if (builder.length() <= 0)
                    return null;

                return builder.toString();
            }

            //添加到数据中
            builder.append(tmpC);

            //判断是否括号，是则增加层数
            if (tmpC == '(')
            {
                //增加深度
                deep++;
            } else if (tmpC == ')')
            {
                //减少深度
                deep--;
            }
        }

        //如果括号未结束则报警告
        if (deep > 0)
        {
            ALServerLog.Warning("String 【" + _m_sString + "】 format error!");
        }

        //记录最后一个分隔符
        _m_cLastSplitChar = _g_defaultSplitChar;

        //如果头尾都是括号则删除头尾
        if (builder.length() > 0 && builder.charAt(0) == '('
                && builder.charAt(builder.length() - 1) == ')')
        {
            //先删除结尾的括号
            builder.deleteCharAt(builder.length() - 1);
            builder.deleteCharAt(0);
        }

        //无长度返回null
        if (builder.length() <= 0)
            return null;

        //到结尾都没有则返回全部
        return builder.toString();
    }

    public String readItem(char _splitC, char _splitC2)
    {
        if (null == _m_sString)
        {
            _m_cLastSplitChar = _g_defaultSplitChar;
            return null;
        }

        if (_m_iReadPos >= _m_sString.length())
        {
            _m_cLastSplitChar = _g_defaultSplitChar;
            return null;
        }

        //从头开始处理
        StringBuilder builder = new StringBuilder();

        //括号层数
        int deep = 0;
        for (; _m_iReadPos < _m_sString.length(); )
        {
            char tmpC = _m_sString.charAt(_m_iReadPos);
            //读取标记前进
            _m_iReadPos++;

            //判断是否分隔符或者不在括号内
            if ((tmpC == _splitC || tmpC == _splitC2) && deep <= 0)
            {
                //记录最后一个分隔符
                _m_cLastSplitChar = tmpC;

                //如果头尾都是括号则删除头尾
                if (builder.length() > 0 && builder.charAt(0) == '('
                        && builder.charAt(builder.length() - 1) == ')')
                {
                    //先删除结尾的括号
                    builder.deleteCharAt(builder.length() - 1);
                    builder.deleteCharAt(0);
                }

                //无长度返回null
                if (builder.length() <= 0)
                    return null;

                return builder.toString();
            }

            //添加到数据中
            builder.append(tmpC);

            //判断是否括号，是则增加层数
            if (tmpC == '(')
            {
                //增加深度
                deep++;
            } else if (tmpC == ')')
            {
                //减少深度
                deep--;
            }
        }

        //如果括号未结束则报警告
        if (deep > 0)
        {
            ALServerLog.Warning("String 【" + _m_sString + "】 format error!");
        }

        //记录最后一个分隔符
        _m_cLastSplitChar = _g_defaultSplitChar;

        //如果头尾都是括号则删除头尾
        if (builder.length() > 0 && builder.charAt(0) == '('
                && builder.charAt(builder.length() - 1) == ')')
        {
            //先删除结尾的括号
            builder.deleteCharAt(builder.length() - 1);
            builder.deleteCharAt(0);
        }

        //无长度返回null
        if (builder.length() <= 0)
            return null;

        //到结尾都没有则返回全部
        return builder.toString();
    }

    public String readItem(char[] _splitCArr)
    {
        if (null == _m_sString)
        {
            _m_cLastSplitChar = _g_defaultSplitChar;
            return null;
        }

        if (_m_iReadPos >= _m_sString.length())
        {
            _m_cLastSplitChar = _g_defaultSplitChar;
            return null;
        }

        //从头开始处理
        StringBuilder builder = new StringBuilder();

        //括号层数
        int deep = 0;
        for (; _m_iReadPos < _m_sString.length(); )
        {
            char tmpC = _m_sString.charAt(_m_iReadPos);
            //读取标记前进
            _m_iReadPos++;

            //判断是否分隔符或者不在括号内
            if (_judgeCharInArr(tmpC, _splitCArr) && deep <= 0)
            {
                //记录最后一个分隔符
                _m_cLastSplitChar = tmpC;

                //如果头尾都是括号则删除头尾
                if (builder.length() > 0 && builder.charAt(0) == '('
                        && builder.charAt(builder.length() - 1) == ')')
                {
                    //先删除结尾的括号
                    builder.deleteCharAt(builder.length() - 1);
                    builder.deleteCharAt(0);
                }

                //无长度返回null
                if (builder.length() <= 0)
                    return null;

                return builder.toString();
            }

            //添加到数据中
            builder.append(tmpC);

            //判断是否括号，是则增加层数
            if (tmpC == '(')
            {
                //增加深度
                deep++;
            } else if (tmpC == ')')
            {
                //减少深度
                deep--;
            }
        }

        //如果括号未结束则报警告
        if (deep > 0)
        {
            ALServerLog.Warning("String 【" + _m_sString + "】 format error!");
        }

        //记录最后一个分隔符
        _m_cLastSplitChar = _g_defaultSplitChar;

        //如果头尾都是括号则删除头尾
        if (builder.length() > 0 && builder.charAt(0) == '('
                && builder.charAt(builder.length() - 1) == ')')
        {
            //先删除结尾的括号
            builder.deleteCharAt(builder.length() - 1);
            builder.deleteCharAt(0);
        }

        //无长度返回null
        if (builder.length() <= 0)
            return null;

        //到结尾都没有则返回全部
        return builder.toString();
    }

    /***********
     * 判断字符是否在数组内
     * @param _c
     * @param _cArr
     */
    private static boolean _judgeCharInArr(char _c, char[] _cArr)
    {
        for (int i = 0; i < _cArr.length; i++)
        {
            if (_c == _cArr[i])
                return true;
        }

        return false;
    }
}
