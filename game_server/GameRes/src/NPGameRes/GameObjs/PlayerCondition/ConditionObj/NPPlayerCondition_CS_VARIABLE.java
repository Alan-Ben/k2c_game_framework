package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

/**
 * NPPlayerCondition_CS_VARIABLE 获取某高级公式的值，判断值是否在范围之内
 * @author mark
 */
public class NPPlayerCondition_CS_VARIABLE extends _ANPBasicPlayerCondition
{
    private long _m_minVal;
    private long _m_maxVal;
    private NPPlayerVariableGroupObj _m_variable;

    public long min()
    {
        return _m_minVal;
    }

    public long max()
    {
        return _m_maxVal;
    }

    public NPPlayerVariableGroupObj variable()
    {
        return _m_variable;
    }


    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_VARIABLE;
    }

    public static NPPlayerCondition_CS_VARIABLE readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_VARIABLE cond = new NPPlayerCondition_CS_VARIABLE();

        String minStr = _reader.readItem();
        String maxStr = _reader.readItem();
        String varStr = _reader.readItem();

        if (null == minStr)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.NPPlayerCondition_CS_VARIABLE[" + _reader.getSrcString() + "]");
            return null;
        }
        if (null == maxStr)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.NPPlayerCondition_CS_VARIABLE[" + _reader.getSrcString() + "]");
            return null;
        }
        if (null == varStr)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.NPPlayerCondition_CS_VARIABLE[" + _reader.getSrcString() + "]");
            return null;
        }
        cond._m_minVal = Long.parseLong(minStr);
        cond._m_maxVal = Long.parseLong(maxStr);
        cond._m_variable = new NPPlayerVariableGroupObj();
        cond._m_variable.parseFromString(varStr);

        return cond;
    }
}
