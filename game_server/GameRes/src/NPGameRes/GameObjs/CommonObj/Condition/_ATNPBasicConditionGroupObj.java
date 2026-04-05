package NPGameRes.GameObjs.CommonObj.Condition;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPBasicCondition;

import java.util.ArrayList;

/*************
 * 通用的条件集合数据对象
 * 对条件的读取进行抽象
 * @author mj
 *
 */
public abstract class _ATNPBasicConditionGroupObj<E extends Enum<E>, C extends _ITNPBasicCondition<E>, G extends _ATNPBasicConditionGroupObj<E, C, G>>
{
    public enum ENPJudgeChar
    {
        NONE,
        AND,
        OR,
    }

    //原始字符串，用于打印信息
    protected String _m_sOriStr;

    /**
     * 本对象的条件类型
     */
    protected boolean _m_bTrueEnable;  //是否正向正确判断
    protected C _m_cConditionObj;
    protected ArrayList<G> _m_lChildConditionList;
    protected ENPJudgeChar _m_eJudgeType = ENPJudgeChar.NONE;

    public _ATNPBasicConditionGroupObj()
    {
        _m_bTrueEnable = true;
        _m_cConditionObj = null;
        _m_lChildConditionList = new ArrayList<G>();
    }

    public boolean hasCondition()
    {
        return (_m_cConditionObj != null)
                || (_m_lChildConditionList != null && _m_lChildConditionList.size() > 0);
    }

    /**************************************
     * 可能的高级条件读取方式函数，日后可能需要用到，有兴趣可参看
     **/
    public static char _g_or = '|';
    public static char _g_and = '#';
    private static char _g_not = '!';
    public static char[] _g_arrTotalSplitArr = {_g_or, _g_and, _g_not};


    //将字符串转化为本对象
    public void readConditionGroupList(String _str, String _err)
    {
        _readConditionGroupList(_str, 0, _err);
    }

    protected void _readConditionGroupList(String _str, int _depth, String _err)
    {
        //存储原始字符串
        _m_sOriStr = _str;
        //构造读取对象
        NPStringReader stringReader = new NPStringReader(_str);

        if (_depth > 5)
        {
            ALServerLog.Error(_err);
            ALServerLog.Error(" Notice!!! Single Condition depth more than 5!!! conditionStr: " + _str + " depth: " +
                    _depth);
        }

        if (stringReader.isEmpty())
            return;

        char tmpChar;
        boolean isTrueEnable = true;

        //读取第一个分隔位置信息
        do
        {
            //读取字符串
            String splitStr = stringReader.readItem(_g_arrTotalSplitArr);
            //获取最后一个分隔符
            tmpChar = stringReader.getLastSpliteChar();

            //如果读取的数据和原数据长度一致，可以看做是一个独立的条件
            if (null != splitStr && splitStr.length() == _str.length())
            {
                //UnityEngine.ALServerLog.Error(index + " - " + startIdx + " single conddi: " + infoStr.ToString());
                _m_cConditionObj = _readConditionStr(splitStr);
                //设置正向还是反向判断
                _m_bTrueEnable = isTrueEnable;
                //重置正向
                isTrueEnable = true;
            } else
            {
                //此时表明有进行了字符拆分，先判断后续字符信息
                if (tmpChar == _g_or)
                {
                    //UnityEngine.ALServerLog.Error("set or");
                    //取出字符串进行处理
                    if (_m_eJudgeType == ENPJudgeChar.AND)
                    {
                        ALServerLog.Error(_err);
                        ALServerLog.Error("同一个括号内使用不同的关系符号, 只能使用一个关系符号  \'|\' 或者 \'~\'");
                    }
                    _m_eJudgeType = ENPJudgeChar.OR;
                } else if (tmpChar == _g_and)
                {
                    //UnityEngine.ALServerLog.Error("set and");
                    //取出字符串进行处理
                    if (_m_eJudgeType == ENPJudgeChar.OR)
                    {
                        ALServerLog.Error(_err);
                        ALServerLog.Error("同一个括号内使用不同的关系符号, 只能使用一个关系符号  \'|\' 或者 \'~\'");
                    }
                    _m_eJudgeType = ENPJudgeChar.AND;
                } else if (tmpChar == _g_not)
                {
                    //UnityEngine.ALServerLog.Error("set not");
                    //设置下一个判断条件反向
                    isTrueEnable = !isTrueEnable;
                }//判断此符号是否(，是则开始一个新集合的读取

                //继续进行条件组合读取
                if (null != splitStr)
                {
                    //读取内容对象
                    //UnityEngine.ALServerLog.Error(" sub conddi11: " + infoStr.ToString());
                    G newObj = _createGroupObj();
                    newObj._readConditionGroupList(splitStr, _depth + 1, _err);
                    if (null != newObj)
                    {
                        //设置正向还是反向判断
                        newObj._m_bTrueEnable = isTrueEnable;
                        //重置正向
                        isTrueEnable = true;
                        if (null != newObj)
                            _m_lChildConditionList.add(newObj);
                    }
                }
            }
        } while (!stringReader.isEmpty());
    }

    /*******************
     * 创建一个Group对象
     * @return
     */
    protected abstract G _createGroupObj();

    /***************
     * 从字符串读取出对应的条件
     * @param _str
     * @return
     */
    protected abstract C _readConditionStr(String _str);

    @Override
    public String toString()
    {
        return _m_sOriStr;
    }
}
