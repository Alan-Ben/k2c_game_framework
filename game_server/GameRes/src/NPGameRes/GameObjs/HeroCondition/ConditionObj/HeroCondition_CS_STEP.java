package NPGameRes.GameObjs.HeroCondition.ConditionObj;

import Common.ConditionEnum.EHeroConditionType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.HeroCondition._ABasicHeroCondition;

public class HeroCondition_CS_STEP extends _ABasicHeroCondition
{
    private long _m_lMinValue = -1; //-1:不限制最小值
    private long _m_lMaxValue = -1; //-1:不限制最大值

    public long minValue()
    {
        return _m_lMinValue;
    }

    public long maxValue()
    {
        return _m_lMaxValue;
    }

    @Override
    public EHeroConditionType conditionType()
    {
        return EHeroConditionType.CS_STEP;
    }

    public static HeroCondition_CS_STEP readCond(NPStringReader _reader)
    {
        HeroCondition_CS_STEP cond = new HeroCondition_CS_STEP();

        String minVS = _reader.readItem();
        if (null == minVS)
        {
            CommLog.error("Can not read str for EHeroConditionType.CS_STAR[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lMinValue = Long.parseLong(minVS);

        String maxVS = _reader.readItem();
        if (null != maxVS)
            cond._m_lMaxValue = Long.parseLong(maxVS);

        return cond;
    }
}
