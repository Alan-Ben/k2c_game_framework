package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle.WCGSingleConditionGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionRangeConditionS extends _AWCGBasicSingleCondition
{
    private int _m_iRange;//范围  厘米

    private int _m_eRelationType;

    private WCGSingleConditionGroupObj _m_lConditionList;

    public int Range()
    {
        return _m_iRange;
    }

    public int RelationType()
    {
        return _m_eRelationType;
    }

    public WCGSingleConditionGroupObj ConditionList()
    {
        return _m_lConditionList;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.RNG_S_COND;
    }

    public static WCGConditionRangeConditionS read(String _infoStr)
    {
        WCGConditionRangeConditionS obj = new WCGConditionRangeConditionS();

        String[] strs = CommonFunc.charSplit(_infoStr, ':', 3);

        try
        {
            obj._m_iRange = Integer.parseInt(strs[0].trim());
            obj._m_eRelationType = WCGResCommon.readRelationBitValue(strs[1]);
            if (strs.length > 2)
            {
                String errStr = "RNG_S_COND 内置条件配置错误: " + strs[2];
                obj._m_lConditionList = WCGSingleConditionGroupObj.readConditionGroupList(strs[2], errStr);
            }
            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - RNE_S_COND example: enum:range:enemy_type:conditionStr Error Str: " + _infoStr);
            return null;
        }
    }
}
