package NPGameRes.GameObjs.Battle.Condition;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicBothCondition;
import WCGCommon.Enum.NPEnum.EWCGBothConditionType;
import WCGCommon.Enum.NPEnum.EWCGCompareResult;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGValueType;

public class WCGConditionValueCompare extends _AWCGBasicBothCondition
{

    /* 比较对象 */
    private EWCGEffectTargetType _m_eCompareType;

    /* 比较目标对象 */
    private EWCGEffectTargetType _m_eCompareTargetType;

    /* 比较状态值类型 */
    private EWCGValueType _m_eCompareValueType;

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

    public EWCGValueType CompareValueType()
    {
        return _m_eCompareValueType;
    }

    public EWCGCompareResult CompareResult()
    {
        return _m_eCompareResult;
    }

    @Override
    public WCGCommon.Enum.NPEnum.EWCGBothConditionType conditionType()
    {
        return EWCGBothConditionType.V_COMP;
    }

    public static WCGConditionValueCompare readCond(String _str)
    {
        WCGConditionValueCompare cond = new WCGConditionValueCompare();

        // 解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        // 逐个判断
        if (strs.length < 4)
        {
            ALServerLog
                    .Error("Error Format for Condition - Pro_Compare example: enum:target_a:target_b:valueType:compare_result Error Str: "
                            + _str);
            return null;
        }

        try
        {
            cond._m_eCompareType = EWCGEffectTargetType.valueOf(strs[0].toUpperCase().trim());
            cond._m_eCompareTargetType = EWCGEffectTargetType.valueOf(strs[1].toUpperCase().trim());
            cond._m_eCompareValueType = EWCGValueType.valueOf(strs[2].toUpperCase().trim());
            cond._m_eCompareResult = EWCGCompareResult.valueOf(strs[3].toUpperCase().trim());

            return cond;
        } catch (Exception e)
        {
            ALServerLog
                    .Error("Error Format for Condition - Pro_Compare example: enum:target_a:target_b:valueType:compare_result Error Str: "
                            + _str);
            return null;
        }
    }

}