package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

public class NPPlayerCondition_CS_BUF_LAYER extends _ANPBasicPlayerCondition
{
    private long _m_lBufId;
    private int _m_iMinValue = -1; // -1:不限制最小值
    private int _m_iMaxValue = -1; //-1:不限制最大值

    public long bufId()
    {
        return _m_lBufId;
    }

    public int minValue()
    {
        return _m_iMinValue;
    }

    public int maxValue()
    {
        return _m_iMaxValue;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_BUF_LAYER;
    }

    public static NPPlayerCondition_CS_BUF_LAYER readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_BUF_LAYER cond = new NPPlayerCondition_CS_BUF_LAYER();
        //逐个进行读取
        String bufIdS = _reader.readItem(':');
        String minVS = _reader.readItem(':');

        if (null == bufIdS || null == minVS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_BUF_LAYER[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lBufId = Long.parseLong(bufIdS);
        cond._m_iMinValue = Integer.parseInt(minVS);

        String maxVS = _reader.readItem(':');
        if (null != maxVS)
            cond._m_iMaxValue = Integer.parseInt(maxVS);

        return cond;
    }
}
