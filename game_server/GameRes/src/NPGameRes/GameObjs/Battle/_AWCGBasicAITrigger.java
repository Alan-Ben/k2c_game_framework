package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.Battle.AITrigger.*;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

public abstract class _AWCGBasicAITrigger
{
    /******************
     * 获取条件类型
     *
     * @author alzq.z
     * @time Nov 12, 2013 10:56:58 PM
     */
    public abstract EWCGAITriggerType triggerType();

    /********************
     * 从节点中读取相关信息
     *
     * @author alzq.z
     * @time Jun 27, 2013 12:29:59 AM
     */
    public static _AWCGBasicAITrigger readAITrigger(EWCGAITriggerType _triggerType, String _infoStr)
    {

        switch (_triggerType)
        {
            case SET_AI_V:
                return WCGAITriggerSetIndexValue.read(_infoStr);
            case CHG_AI_V:
                return WCGAITriggerChgIndexValue.read(_infoStr);
            case SELF_TRIGGER:
                return WCGAITriggerSelfTrigger.read(_infoStr);
            case START_TIMER:
                return WCGAITriggeStartTimer.read(_infoStr);
            case START_OP:
                return WCGAITriggerStartOperation.read(_infoStr);
            case SPECIAL:
                return WCGAITriggerSpecial.read(_infoStr);
            case SELF_EFFECT:
                return WCGAITriggerSelfEffect.read(_infoStr);
            case PRINT:
                return WCGAITriggerPrint.read(_infoStr);
            case SHOW_NOTICE:
                return WCGAITriggerShowNotice.read(_infoStr);
            case SHOW_ADD_NOTICE:
                return WCGAITriggerShowAddNotice.read(_infoStr);
            case SHOW_SINGLE_NOTICE:
                return WCGAITriggerShowSingleNotice.read(_infoStr);
            case SOUND_PLAY:
                return WCGAITriggerSoundPlay.read(_infoStr);
            case TEAM_WIN:
                return WCGAITriggerTeamWin.read(_infoStr);
            case TEAM_LOSE:
                return WCGAITriggerTeamLose.read(_infoStr);
            case CAMP_WIN:
                return WCGAITriggerCampWin.read(_infoStr);
            case CAMP_LOSE:
                return WCGAITriggerCampLose.read(_infoStr);
            case SUM_T_POINT:
                return WCGAITriggerSumActorInPoint.read(_infoStr);
            case SUM_T_POS:
                return WCGAITriggerSumActorInPos.read(_infoStr);
            case SET_TUTORIAL_DONE:
                return WCGAITriggerSetTutorialDone.read(_infoStr);
            case SHOW_TXT_NOTICE:
                return WCGAITriggerShowTxtNotice.read(_infoStr);
            case TEAM_TRIGGER:
                return WCGAITriggerTeamTrigger.read(_infoStr);
            case SHOW_TIP:
                return WCGAITriggerShowTip.read(_infoStr);


            case SET_ACTOR_V:
                return WCGAITriggerSetActorValue.read(_infoStr);
            case SET_SP_ACTOR_V:
                return WCGAITriggerSetSpActorValue.read(_infoStr);
            case SET_BATTLE_V:
                return WCGAITriggerSetBattleValue.read(_infoStr);

            case CHG_ACTOR_V:
                return WCGAITriggerChgActorValue.read(_infoStr);
            case CHG_SP_ACTOR_V:
                return WCGAITriggerChgSpActorValue.read(_infoStr);
            case CHG_BATTLE_V:
                return WCGAITriggerChgBattleValue.read(_infoStr);

            case ADD_CAMP_RES:
                return WCGAITriggerAddCampRes.read(_infoStr);
            case DEL_CAMP_RES:
                return WCGAITriggerDelCampRes.read(_infoStr);
//            case DO_TUTORIAL:
//                return WCGAITriggerDoTutorialEffect.read(_infoStr);

            case RMV_CONTROL:
                return WCGAITriggerRmvControl.read(_infoStr);
            default:
                return null;
        }

    }
}