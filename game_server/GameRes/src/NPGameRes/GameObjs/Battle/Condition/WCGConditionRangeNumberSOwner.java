package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.Battle.WCGSingleConditionGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


public class WCGConditionRangeNumberSOwner extends _AWCGBasicSingleCondition
{
    private int _m_iRange;

    private int _m_eRelationType;

    private WCGIntRange _m_rCountRange;

    private WCGSingleConditionGroupObj _m_lConditionList;

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

    public WCGSingleConditionGroupObj ConditionList()
    {
        return _m_lConditionList;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.RNG_NUM_S_O_COND;
    }

    public static WCGConditionRangeNumberSOwner read(String _infoStr)
    {
        WCGConditionRangeNumberSOwner obj = new WCGConditionRangeNumberSOwner();

        String[] strs = CommonFunc.charSplit(_infoStr, ':', 5);

        try
        {
            obj._m_iRange = Integer.parseInt(strs[0].trim());
            obj._m_eRelationType = WCGResCommon.readRelationBitValue(strs[1]);
            obj._m_rCountRange = new WCGIntRange(strs[2], strs[3]);
            if (strs.length > 4)
            {
                String errStr = "RNG_NUM_S_O_COND 内置条件配置错误: " + strs[4];
                obj._m_lConditionList = WCGSingleConditionGroupObj.readConditionGroupList(strs[4], errStr);
            }
            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - RNG_NUM_S_COND example: enum:range:enemy_type Error Str: " + _infoStr, e);
            return null;
        }
    }
}