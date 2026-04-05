package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import Common.LevyEnum.ELevy_Type;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 征收累计数量 CS_LEVY_SUM:ELevy_Type:min:max
 * @author mark
 */
public class NPPlayerCondition_CS_LEVY_SUM extends _ANPBasicPlayerCondition
{
    private ELevy_Type _m_eType = ELevy_Type.NONE;
    private long _m_lMinValue = -1; // -1:不限制最小值
    private long _m_lMaxValue = -1; //-1:不限制最大值

    public ELevy_Type levyType()
    {
        return _m_eType;
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
        return ENPPlayerConditionType.CS_LEVY_SUM;
    }

    public static NPPlayerCondition_CS_LEVY_SUM readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_LEVY_SUM cond = new NPPlayerCondition_CS_LEVY_SUM();

        String typeS = _reader.readItem();
        String minVS = _reader.readItem();

        if (null == typeS || null == minVS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_LEVY_SUM[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_eType = ELevy_Type.valueOf(typeS.toUpperCase());
        cond._m_lMinValue = Long.parseLong(minVS);

        String maxVS = _reader.readItem();
        if (null != maxVS)
            cond._m_lMaxValue = Long.parseLong(maxVS);

        return cond;
    }
}
