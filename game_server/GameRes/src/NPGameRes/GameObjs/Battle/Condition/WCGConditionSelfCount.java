package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.Battle.WCGSingleConditionGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGActorType;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

/**************
 * ai条件  判断我方所拥有的对应单位类型，标记的总数量，
 **/
public class WCGConditionSelfCount extends _AWCGBasicSingleCondition
{
    /* 范围，min~max*/
    private WCGIntRange _m_rCountRange;
    /* 单位类型*/
    private EWCGActorType _m_eActorType;
    /*条件列表*/
    private WCGSingleConditionGroupObj _m_lConditionList;


    public WCGIntRange CountRange()
    {
        return _m_rCountRange;
    }

    public EWCGActorType ActorType()
    {
        return _m_eActorType;
    }

    public WCGSingleConditionGroupObj ConditionList()
    {
        return _m_lConditionList;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.SELF_CNT;
    }


    /**************
     * a格式如下: 枚举:数量最小范围:数量最大范围:单位类型:单位条件(组)
     **/
    public static WCGConditionSelfCount readCond(String _infoStr)
    {
        WCGConditionSelfCount obj = new WCGConditionSelfCount();

        String[] strs = CommonFunc.charSplit(_infoStr, ':', 4);

        try
        {
            obj._m_rCountRange = new WCGIntRange(strs[0], strs[1]);
            obj._m_eActorType = EWCGActorType.valueOf(strs[2].toUpperCase().trim());
            if (strs.length > 3)
            {
                String errStr = "SELF_CNT 内置条件配置错误: " + strs[3];
                obj._m_lConditionList = WCGSingleConditionGroupObj.readConditionGroupList(strs[3], errStr);

            }
            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - SELF_CNT example: enum:range:actor_type:signleconditiongrouplist Error Str: " + _infoStr);
            return null;
        }
    }


}
