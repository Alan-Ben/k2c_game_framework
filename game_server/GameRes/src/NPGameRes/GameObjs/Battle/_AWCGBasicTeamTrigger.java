package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGTeamTriggerType;


//队伍触发效果
public abstract class _AWCGBasicTeamTrigger
{
    /******************
     * 获取类型
     */
    public abstract EWCGTeamTriggerType triggerType();


    /********************
     * 从节点中读取相关信息
     */
    public static _AWCGBasicTeamTrigger readAITrigger(EWCGTeamTriggerType _triggerType, String _infoStr)
    {
        if (_triggerType == EWCGTeamTriggerType.SUMMON)
            return WCGTeamTriggerSummon.read(_infoStr);
        if (_triggerType == EWCGTeamTriggerType.EFFECT_ID)
            return WCGTeamTriggerEffectID.read(_infoStr);
        if (_triggerType == EWCGTeamTriggerType.COMPLETE_BATTLE_TASK)
            return WCGTeamTriggerCompleteBattleTask.read(_infoStr);
        return null;
    }
}