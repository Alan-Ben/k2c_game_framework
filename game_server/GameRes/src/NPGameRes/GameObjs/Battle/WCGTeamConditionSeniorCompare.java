package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGCompareResult;
import WCGCommon.Enum.NPEnum.EWCGTeamConditionType;

/// <summary>
/// P_COMP - 属性比较   格式如下：枚举:高级公式1:高级公式2:比较类型
/// </summary>
public class WCGTeamConditionSeniorCompare extends _AWCGBasicTeamCondition
{
    /*目标对象高级公式*/
    private WCGTeamVariableGroupObj _m_eCompareDealer;

    /*比较目标对象高级公式*/
    private WCGTeamVariableGroupObj _m_eCompareTargetDealer;

    /*比较结果类型*/
    private EWCGCompareResult _m_eCompareResult;

    public WCGTeamVariableGroupObj CompareDealer()
    {
        return _m_eCompareDealer;
    }

    public WCGTeamVariableGroupObj CompareTargetDealer()
    {
        return _m_eCompareTargetDealer;
    }

    public EWCGCompareResult CompareResult()
    {
        return _m_eCompareResult;
    }

    public EWCGTeamConditionType conditionType()
    {
        return EWCGTeamConditionType.TEAM_S_COMP;
    }

    public static WCGTeamConditionSeniorCompare readCond(String _str)
    {
        WCGTeamConditionSeniorCompare cond = new WCGTeamConditionSeniorCompare();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        //逐个判断
        if (strs.length < 3)
        {
            CommLog.error("Team条件配置错误 - Pro_Compare example: enum:target_a:target_b:compare_result Error Str: " + _str);
            return null;
        }

        try
        {
            cond._m_eCompareDealer = WCGTeamVariableGroupObj.readVariableGroup(strs[0], "Team获取队伍资源数效果高级公式错误： ");
            cond._m_eCompareTargetDealer = WCGTeamVariableGroupObj.readVariableGroup(strs[1], "Team获取队伍资源数效果高级公式错误： ");

            cond._m_eCompareResult = EWCGCompareResult.valueOf(strs[2].toUpperCase().trim());
            return cond;
        } catch (Exception e)
        {
            CommLog.error("Team条件配置错误 - Pro_Compare example: enum:target_a:target_b:compare_result Error Str: " + _str);
            return null;
        }
    }
}