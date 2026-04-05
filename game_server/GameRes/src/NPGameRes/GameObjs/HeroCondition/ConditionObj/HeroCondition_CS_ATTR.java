package NPGameRes.GameObjs.HeroCondition.ConditionObj;

import Common.ConditionEnum.EHeroConditionType;
import CommonEnum.ESpecAttrType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.HeroCondition._ABasicHeroCondition;

public class HeroCondition_CS_ATTR extends _ABasicHeroCondition
{
    private ESpecAttrType _m_attr = ESpecAttrType.NONE;

    public ESpecAttrType attrType()
    {
        return _m_attr;
    }

    @Override
    public EHeroConditionType conditionType()
    {
        return EHeroConditionType.CS_ATTR;
    }

    public static HeroCondition_CS_ATTR readCond(NPStringReader _reader)
    {
        HeroCondition_CS_ATTR cond = new HeroCondition_CS_ATTR();

        String attr = _reader.readItem();
        if (null == attr)
        {
            CommLog.error("Can not read str for EHeroConditionType.CS_ATTR[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_attr = ESpecAttrType.valueOf(attr.toUpperCase());

        return cond;
    }
}
