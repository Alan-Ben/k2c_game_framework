package NPGameRes.GameObjs.Battle.Condition;

import ALServerLog.ALServerLog;
import NPCommon.Enum.NPCommonEnum.ENPPropertyType;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicBothCondition;
import WCGCommon.Enum.NPEnum.EWCGBothConditionType;
import WCGCommon.Enum.NPEnum.EWCGCompareResult;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;

public class WCGConditionProCompare extends _AWCGBasicBothCondition
{

    /* 比较对象 */
    private EWCGEffectTargetType _m_eCompareType;

    /* 比较目标对象 */
    private EWCGEffectTargetType _m_eCompareTargetType;

    /* 比较属性类型 */
    private ENPPropertyType _m_ePropertyType;

    /* 比较结果类型 */
    private EWCGCompareResult _m_eCompareResult;

    public EWCGEffectTargetType CompareType()
    {
        return _m_eCompareType;
    }

    public EWCGEffectTargetType CompareTargetType()
    {
        return _m_eCompareTargetType;
    }

    public ENPPropertyType PropertyType()
    {
        return _m_ePropertyType;
    }

    public EWCGCompareResult CompareResult()
    {
        return _m_eCompareResult;
    }

    @Override
    public WCGCommon.Enum.NPEnum.EWCGBothConditionType conditionType()
    {
        return EWCGBothConditionType.P_COMP;
    }

    public static WCGConditionProCompare readCond(String _str)
    {
        WCGConditionProCompare cond = new WCGConditionProCompare();

        // 解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        // 逐个判断
        if (strs.length < 4)
        {
            ALServerLog
                    .Error("Error Format for Condition - Pro_Compare example: enum:target_a:target_b:property:compare_result Error Str: "
                            + _str);
            return null;
        }

        try
        {
            cond._m_eCompareType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            cond._m_eCompareTargetType = EWCGEffectTargetType.valueOf(strs[1].toUpperCase().trim());
            cond._m_ePropertyType = ENPPropertyType.valueOf(strs[2].toUpperCase().trim());
            cond._m_eCompareResult = EWCGCompareResult.valueOf(strs[3].toUpperCase().trim());
            return cond;
        } catch (Exception e)
        {
            ALServerLog
                    .Error("Error Format for Condition - Pro_Compare example: enum:target_a:target_b:property:compare_result Error Str: "
                            + _str);
            return null;
        }
    }

}