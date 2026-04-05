package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGSingleConditionGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicBothCondition;
import WCGCommon.Enum.NPEnum.EWCGBothConditionType;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;

public class WCGConditionActorCondition extends _AWCGBasicBothCondition
{
    private EWCGEffectTargetType _m_eTargetType;

    private WCGSingleConditionGroupObj _m_lConditionList;

    public EWCGEffectTargetType TargetType()
    {
        return _m_eTargetType;
    }

    public WCGSingleConditionGroupObj ConditionList()
    {
        return _m_lConditionList;
    }

    @Override
    public EWCGBothConditionType conditionType()
    {
        return EWCGBothConditionType.A_COND;
    }

    public static WCGConditionActorCondition read(String _infoStr)
    {
        WCGConditionActorCondition obj = new WCGConditionActorCondition();

        String[] strs = CommonFunc.charSplit(_infoStr, ':', 2);
        try
        {
            obj._m_eTargetType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            if (strs.length > 1)
            {
                String errStr = "A_COND 内置条件配置错误: " + strs[1];
                obj._m_lConditionList = WCGSingleConditionGroupObj.readConditionGroupList(strs[1], errStr);
            }
            return obj;
        } catch (Exception e)
        {
            CommLog.error("Error Format for Both Condition - ACT_COND example: enum:target:single_cond_type:condition_str Error Str: " + _infoStr);
            return null;
        }
    }
}