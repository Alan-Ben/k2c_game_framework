package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_S_RND_FROM_SCOPE extends _ANPBasicPlayerVariableObj
{
    //下限值
    private int _m_iMinValue;
    //上限值
    private int _m_iMaxValue;

    public int minValue()
    {
        return _m_iMinValue;
    }

    public int maxValue()
    {
        return _m_iMaxValue;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.S_RND_FROM_SCOPE;
    }

    public static NPPlayerVariable_S_RND_FROM_SCOPE readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_S_RND_FROM_SCOPE variableObj = new NPPlayerVariable_S_RND_FROM_SCOPE();

        //解析字符串
        String minValue = _reader.readItem('@');
        String maxValue = _reader.readItem('@');
        //逐个判断
        if (null == minValue || null == maxValue)
        {
            CommLog.error("高级公式配置错误 - S_RND_FROM_SCOPE S_RND_FROM_SCOPE:minValue:maxValue Error Str: " + _reader.getSrcString());
            return null;
        }

        try
        {
            variableObj._m_iMinValue = Integer.parseInt(minValue);
            variableObj._m_iMaxValue = Integer.parseInt(maxValue);

            if (variableObj._m_iMinValue > variableObj._m_iMaxValue)
            {
                CommLog.error("高级公式配置错误 - S_RND_FROM_SCOPE S_RND_FROM_SCOPE:minValue:maxValue Error reason: minValue > maxValue Str:{}");
            }

            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - S_RND_FROM_SCOPE S_RND_FROM_SCOPE:minValue:maxValue Error Str:{} Exception:{}", _reader.getSrcString(), e);
            return null;
        }
    }
}
