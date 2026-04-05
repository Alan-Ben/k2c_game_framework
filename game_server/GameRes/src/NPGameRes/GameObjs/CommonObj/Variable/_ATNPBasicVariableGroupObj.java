package NPGameRes.GameObjs.CommonObj.Variable;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Enum.NPCommonEnum.ENPVariableCalType;
import NPGameRes.GameObjs.CommonObj.Variable.InterfaceObj._ITNPBasicVariable;

import java.util.ArrayList;

public abstract class _ATNPBasicVariableGroupObj<E extends Enum<E>, V extends _ITNPBasicVariable<E>, G extends _ATNPBasicVariableGroupObj<E, V, G>>
{
    /**
     * 本对象的条件类型
     */
    protected V _m_vVariableObj;
    protected ArrayList<G> _m_lChildGroupList;
    protected ENPVariableCalType _m_eCalType = ENPVariableCalType.NONE;

    protected _ATNPBasicVariableGroupObj()
    {
        _m_vVariableObj = null;
        _m_lChildGroupList = new ArrayList<G>();
    }

    public boolean hasVariable()
    {
        return (null != _m_vVariableObj)
                || (_m_lChildGroupList != null && _m_lChildGroupList.size() > 0);
    }

    /**************************************
     * 可能的高级条件读取方式函数，日后可能需要用到，有兴趣可参看
     **/
    private static char[] _g_arrTotalSplitArr = {'+', '-', '*', '|', '<', '>'};


    //将字符串转化为本对象
    public void readVariableGroup(String _str, String _err)
    {
        _readVariableGroup(_str, 0, _err);
    }

    protected void _readVariableGroup(String _str, int _depth, String _err)
    {
        //构造读取对象
        NPStringReader stringReader = new NPStringReader(_str);

        if (_depth > 5)
        {
            ALServerLog.Error(_err);
            ALServerLog.Error(" Notice!!! Single Variable depth more than 5!!! variableStr: " + _str + " depth: " + _depth);
        }

        if (stringReader.isEmpty())
            return;

        char tmpChar;
        //最后一个运算符，一般用于当前读取的对象设置
        ENPVariableCalType calType = ENPVariableCalType.NONE;

        //读取第一个分隔位置信息
        do
        {
            //读取字符串
            String splitStr = stringReader.readItem(_g_arrTotalSplitArr);
            //获取最后一个分隔符
            tmpChar = stringReader.getLastSpliteChar();

            //如果读取的数据和原数据长度一致，可以看做是一个独立的元数据
            if (null != splitStr && splitStr.length() == _str.length())
            {
                //取完字符串，使用截取的字符串读取对应条件
                _m_vVariableObj = _readVariableStr(splitStr);
                //设置计算方式
                _m_eCalType = calType;
                //重置计算方式
                calType = ENPVariableCalType.NONE;
            } else
            {
                //先读取前面的计算对象
                G newObj = _createGroupObj();
                newObj._readVariableGroup(splitStr, _depth + 1, _err);
                if (null != newObj)
                {
                    //设置计算方式为最后一个计算方式
                    newObj._m_eCalType = calType;
                    //重置计算方式
                    calType = ENPVariableCalType.NONE;

                    //如果队列不为空，而计算方式为none需要报错
                    if (!_m_lChildGroupList.isEmpty() && newObj._m_eCalType == ENPVariableCalType.NONE)
                    {
                        ALServerLog.Error("Variable Cal type is none: " + _str);
                    }

                    //添加数据到集合
                    _m_lChildGroupList.add(newObj);
                }

                //根据结束字符，设置下一个计算公式的计算方式
                //判断此符号是否(，是则开始一个新集合的读取
                if (tmpChar == '+')
                {
                    calType = ENPVariableCalType.ADD;
                } else if (tmpChar == '-')
                {
                    calType = ENPVariableCalType.SUB;
                } else if (tmpChar == '*')
                {
                    calType = ENPVariableCalType.MUL;
                } else if (tmpChar == '|')
                {
                    calType = ENPVariableCalType.DIV;
                } else if (tmpChar == '>')
                {
                    calType = ENPVariableCalType.MAX;
                } else if (tmpChar == '<')
                {
                    calType = ENPVariableCalType.MIN;
                }
            }
        } while (!stringReader.isEmpty());
    }

    /***************
     * 创建对应的条件集合对象
     * @param _str
     * @return
     */
    protected abstract G _createGroupObj();

    /***************
     * 从字符串读取出对应的条件
     * @param _str
     * @return
     */
    protected abstract V _readVariableStr(String _str);
}
