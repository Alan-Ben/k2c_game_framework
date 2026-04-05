package NPGameRes.Refs.AvatarGacha.Cond.Impl;

import Common.ClothesEnum.EAvatarGachaGuaranteeCondType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.AvatarGacha.Cond._IGachaCondition;

public class GachaCondition_Tag implements _IGachaCondition
{
    private String _m_tag;

    public String getTag()
    {
        return _m_tag;
    }

    @Override
    public EAvatarGachaGuaranteeCondType conditionType()
    {
        return EAvatarGachaGuaranteeCondType.TAG;
    }

    public static _IGachaCondition readCond(NPStringReader _reader)
    {
        GachaCondition_Tag cond = new GachaCondition_Tag();

        String tag = _reader.readItem();
        if (null == tag)
        {
            CommLog.error("Can not read str for EAvatarGachaGuaranteeCondType.TAG[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_tag = tag;
        return cond;
    }
}