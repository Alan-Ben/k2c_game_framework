package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 火星指定科技等级判断 CS_MARS_TECH_LVL:科技id:min_value（:max_value）
 * @author mark
 */
public class NPPlayerCondition_CS_MARS_TECH_LVL extends _ANPBasicPlayerCondition
{
    private long _m_lId;
    private long _m_lMinValue; // -1:不限制最小值
    private long _m_lMaxValue = -1; //-1:不限制最大值

    public long id() {return _m_lId;}
    public long minValue() {return _m_lMinValue;}
    public long maxValue() {return _m_lMaxValue;}

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_MARS_TECH_LVL;
    }

    public static NPPlayerCondition_CS_MARS_TECH_LVL readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_MARS_TECH_LVL cond = new NPPlayerCondition_CS_MARS_TECH_LVL();

        String idS = _reader.readItem();
        String minVS = _reader.readItem();

        if (null == idS || null == minVS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_MARS_TECH_LVL[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lId = Long.parseLong(idS);
        cond._m_lMinValue = Long.parseLong(minVS);

        String maxVS = _reader.readItem();
        if (null != maxVS)
        {
        	cond._m_lMaxValue = Long.parseLong(maxVS);
        }

        return cond;
    }
}
