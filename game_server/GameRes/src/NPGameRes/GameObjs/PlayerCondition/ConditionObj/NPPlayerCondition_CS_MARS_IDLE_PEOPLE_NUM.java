package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 火星空闲人口数量判断:min(:max) CS_MARS_IDLE_PEOPLE_NUM
 * @author claude
 */
public class NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM extends _ANPBasicPlayerCondition
{
    private long _m_lMinValue; // -1:不限制最小值
    private long _m_lMaxValue = -1; //-1:不限制最大值

    public long minValue() {return _m_lMinValue;}
    public long maxValue() {return _m_lMaxValue;}

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_MARS_IDLE_PEOPLE_NUM;
    }

    public static NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM cond = new NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM();

        String minVS = _reader.readItem();

        if (null == minVS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_MARS_IDLE_PEOPLE_NUM[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lMinValue = Long.parseLong(minVS);

        String maxVS = _reader.readItem();
        if (null != maxVS)
        {
        	cond._m_lMaxValue = Long.parseLong(maxVS);
        }

        return cond;
    }
}
