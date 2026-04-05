package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

public class NPPlayerCondition_CS_CONSORT_RES_COUNT extends _ANPBasicPlayerCondition
{
	//家人ID
    private long _m_lConsortId;
    //家人资源类型
    private EBagItemUse_ConsortDrawShowType _m_eConsortResType = EBagItemUse_ConsortDrawShowType.NONE;
    // -1:不限制最小值
    private long _m_lMinValue = -1; 
    //-1:不限制最大值
    private long _m_lMaxValue = -1; 

	//家人ID
    public long getConsortId() {return _m_lConsortId;}
    //家人资源类型
    public EBagItemUse_ConsortDrawShowType getConsortResType() {return _m_eConsortResType;}
    // -1:不限制最小值
    public long minValue() {return _m_lMinValue;}
    //-1:不限制最大值
    public long maxValue() {return _m_lMaxValue;}

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_CONSORT_RES_COUNT;
    }

    public static NPPlayerCondition_CS_CONSORT_RES_COUNT readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_CONSORT_RES_COUNT cond = new NPPlayerCondition_CS_CONSORT_RES_COUNT();

        String consortIdS = _reader.readItem();
        String resTypeS = _reader.readItem();
        if (null == consortIdS || null == resTypeS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_CONSORT_RES_COUNT[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lConsortId = Long.parseLong(consortIdS);
        cond._m_eConsortResType = EBagItemUse_ConsortDrawShowType.valueOf(resTypeS.toUpperCase());

        String minVS = _reader.readItem();
        if (null != minVS)
            cond._m_lMinValue = Long.parseLong(minVS);
        else
            cond._m_lMinValue = 1;

        String maxVS = _reader.readItem();
        if (null != maxVS)
            cond._m_lMaxValue = Long.parseLong(maxVS);

        return cond;
    }
}
