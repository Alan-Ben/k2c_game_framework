package NPGameRes.Refs.AvatarGacha.Cond.Impl;

import Common.ClothesEnum.EAvatarGachaGuaranteeCondType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.AvatarGacha.Cond._IGachaCondition;

public class GachaCondition_Quality implements _IGachaCondition
{
    private int _m_minQuality = -1;
    private int _m_maxQuality = -1;

    public int getMinQuality()
    {
        return _m_minQuality;
    }

    public int getMaxQuality()
    {
        return _m_maxQuality;
    }

    @Override
    public EAvatarGachaGuaranteeCondType conditionType()
    {
        return EAvatarGachaGuaranteeCondType.QUALITY;
    }

    public static _IGachaCondition readCond(NPStringReader _reader)
    {
        GachaCondition_Quality cond = new GachaCondition_Quality();

        String minQualityStr = _reader.readItem();
        if (null == minQualityStr)
        {
            CommLog.error("Can not read str for EAvatarGachaGuaranteeCondType.QUALITY[" + _reader.getSrcString() + "]");
            return null;
        }
        cond._m_minQuality = Integer.parseInt(minQualityStr);

        String maxQualityStr = _reader.readItem();
        if (null != maxQualityStr)
            cond._m_maxQuality = Integer.parseInt(maxQualityStr);

        return cond;
    }
}