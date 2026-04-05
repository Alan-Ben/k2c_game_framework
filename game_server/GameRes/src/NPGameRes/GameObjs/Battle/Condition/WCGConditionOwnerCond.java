package NPGameRes.GameObjs.Battle.Condition;

import NPGameRes.GameObjs.Battle.WCGSingleConditionGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionOwnerCond extends _AWCGBasicSingleCondition
{
    private WCGSingleConditionGroupObj _m_aConidtionGroupObj;

    public WCGSingleConditionGroupObj conditionGroupObj()
    {
        return _m_aConidtionGroupObj;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.OWNER_S_COND;
    }

    public static WCGConditionOwnerCond readCond(String _str)
    {
        WCGConditionOwnerCond cond = new WCGConditionOwnerCond();

        cond._m_aConidtionGroupObj = WCGSingleConditionGroupObj.readConditionGroupList(_str, "");

        return cond;
    }
}