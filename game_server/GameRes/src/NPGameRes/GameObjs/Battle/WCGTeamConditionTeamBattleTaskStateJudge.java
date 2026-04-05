package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGBattleTaskState;
import WCGCommon.Enum.NPEnum.EWCGTeamConditionType;

public class WCGTeamConditionTeamBattleTaskStateJudge extends _AWCGBasicTeamCondition
{
    private long _m_lBattleTaskID;//任务ID
    private EWCGBattleTaskState _m_eTaskState;   //任务状态

    public long BattleTaskID()
    {
        return _m_lBattleTaskID;
    }

    public EWCGBattleTaskState TaskState()
    {
        return _m_eTaskState;
    }

    public EWCGTeamConditionType conditionType()
    {
        return EWCGTeamConditionType.TEAM_BATTLE_TASK;
    }

    public static WCGTeamConditionTeamBattleTaskStateJudge readCond(String _str)
    {
        WCGTeamConditionTeamBattleTaskStateJudge cond = new WCGTeamConditionTeamBattleTaskStateJudge();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        //逐个判断
        if (strs.length < 2)
        {
            CommLog.error("Team条件配置错误 - TEAM_BATTLE_TASK example: enum:task_id:task_state Error Str: " + _str);
            return null;
        }

        try
        {
            cond._m_lBattleTaskID = Long.parseLong(strs[0]);
            cond._m_eTaskState = EWCGBattleTaskState.valueOf(strs[1].toUpperCase().trim());

            return cond;
        } catch (Exception e)
        {
            CommLog.error("Team条件配置错误 - TEAM_BATTLE_TASK example: enum:task_id:task_state Error Str: " + _str);
            return null;
        }
    }
}