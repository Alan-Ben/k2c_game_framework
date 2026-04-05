package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.WCGTeamConditionGroupObj;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGConditionSpeTeamCond extends _AWCGBasicSingleCondition
{
    private int _m_iTeamGroupId;

    private WCGTeamConditionGroupObj _m_aTeamConidtionGroupObj;

    public WCGTeamConditionGroupObj TeamConidtionGroupObj()
    {
        return _m_aTeamConidtionGroupObj;
    }

    public int TeamGroupId()
    {
        return _m_iTeamGroupId;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.SPE_TEAM_COND;
    }

    public static WCGConditionSpeTeamCond readCond(String _str)
    {

        String[] strs = CommonFunc.charSplit(_str, ':', 2);
        WCGConditionSpeTeamCond cond = new WCGConditionSpeTeamCond();

        try
        {
            cond._m_iTeamGroupId = Integer.parseInt(strs[0].trim());

            String errStr = "SPE_TEAM_COND 内置条件配置错误: " + strs[1];
            cond._m_aTeamConidtionGroupObj = WCGTeamConditionGroupObj.readConditionGroupList(strs[1], errStr);

            return cond;
        } catch (Exception e)
        {
            CommLog.error("条件配置错误 - SPE_TEAM_COND example: enum:SPE_TEAM_COND(:condition) Error Str: " + _str);
            return null;
        }

    }
}