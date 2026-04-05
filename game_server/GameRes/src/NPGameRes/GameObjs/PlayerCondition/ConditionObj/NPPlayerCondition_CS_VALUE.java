package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPEnum.ENPPlayerValueType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

public class NPPlayerCondition_CS_VALUE extends _ANPBasicPlayerCondition
{
    private ENPPlayerValueType _m_ePlayerValueType;
    private long _m_lMinValue = -1; // -1:不限制最小值
    private long _m_lMaxValue = -1; //-1:不限制最大值

    public ENPPlayerValueType conditionValueType()
    {
        return _m_ePlayerValueType;
    }

    public long minValue()
    {
        return _m_lMinValue;
    }

    public long maxValue()
    {
        return _m_lMaxValue;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_VALUE;
    }

    public static NPPlayerCondition_CS_VALUE readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_VALUE cond = new NPPlayerCondition_CS_VALUE();

        String playerValueS = _reader.readItem();
        String minVS = _reader.readItem();

        if (null == playerValueS || null == minVS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_VALUE[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_ePlayerValueType = ENPPlayerValueType.valueOf(playerValueS.toUpperCase());
        cond._m_lMinValue = Long.parseLong(minVS);

        String maxVS = _reader.readItem();
        if (null != maxVS)
            cond._m_lMaxValue = Long.parseLong(maxVS);

        return cond;
    }
}
