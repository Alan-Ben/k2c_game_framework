package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle.WCGBothConditionGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


/// <summary>
///  AI节点的条件:  根据与技能效果条件中相同格式的条件，对范围内对象进行判断
/// </summary>
public class WCGConditionRangeAllCondition extends _AWCGBasicSingleCondition
{

    private int _m_iRange;//范围  厘米

    private int _m_eRelationType;

    private WCGBothConditionGroupObj _m_lConditionList;

    public int Range()
    {
        return _m_iRange;
    }

    public int RelationType()
    {
        return _m_eRelationType;
    }

    public WCGBothConditionGroupObj ConditionList()
    {
        return _m_lConditionList;
    }

    @Override
    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.RALL_D_COND;
    }

    public static WCGConditionRangeAllCondition read(String _infoStr)
    {
        WCGConditionRangeAllCondition obj = new WCGConditionRangeAllCondition();

        String[] strs = CommonFunc.charSplit(_infoStr, ':', 3);
        try
        {
            obj._m_iRange = Integer.parseInt(strs[0].trim());
            obj._m_eRelationType = WCGResCommon.readRelationBitValue(strs[1]);
            if (strs.length > 2)
                obj._m_lConditionList = WCGBothConditionGroupObj.readConditionGroupList(strs[2]);
            return obj;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - RANGE_COND example: enum:range:enemy_type:conditionStr Error Str: " + _infoStr);
            return null;
        }
    }
}
