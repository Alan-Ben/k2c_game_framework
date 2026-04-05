package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle._AWCGBasicBothCondition;
import WCGCommon.Enum.NPEnum.EWCGBothConditionType;

public class WCGConditionRelation extends _AWCGBasicBothCondition
{
    private int _m_iRelationShip;

    public int RelationShip()
    {
        return _m_iRelationShip;
    }

    @Override
    public EWCGBothConditionType conditionType()
    {
        return EWCGBothConditionType.RELATION;
    }

    public static WCGConditionRelation read(String _str)
    {
        WCGConditionRelation cond = new WCGConditionRelation();

        cond._m_iRelationShip = WCGResCommon.readRelationBitValue(_str);

        return cond;
    }
}