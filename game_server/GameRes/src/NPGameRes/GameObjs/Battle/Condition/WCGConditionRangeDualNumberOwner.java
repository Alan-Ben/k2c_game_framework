package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle.WCGBothConditionGroupObj;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


public class WCGConditionRangeDualNumberOwner extends _AWCGBasicSingleCondition
{
    private int _m_iRange;

    private int _m_eRelationType;

    private WCGIntRange _m_rCountRange;

    private WCGBothConditionGroupObj _m_lConditionList;

    public int Range()
    {
        return _m_iRange;
    }

    public int RelationType()
    {
        return _m_eRelationType;
    }

    public WCGIntRange CountRange()
    {
        return _m_rCountRange;
    }

    public WCGBothConditionGroupObj CondtionList()
    {
        return _m_lConditionList;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.RNG_NUM_D_O_COND;
    }

    public static WCGConditionRangeDualNumberOwner read(String _infoStr)
    {
        WCGConditionRangeDualNumberOwner obj = new WCGConditionRangeDualNumberOwner();

        String[] strs = CommonFunc.charSplit(_infoStr, ':', 5);
        try
        {
            obj._m_iRange = Integer.parseInt(strs[0].trim());
            obj._m_eRelationType = WCGResCommon.readRelationBitValue(strs[1].trim());
            obj._m_rCountRange = new WCGIntRange(strs[2], strs[3]);
            if (strs.length > 4)
                obj._m_lConditionList = WCGBothConditionGroupObj.readConditionGroupList(strs[4]);
            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - RNG_NUM_D_O_COND example: enum:range:enemy_type Error Str: " + _infoStr, e);
            return null;
        }
    }
}