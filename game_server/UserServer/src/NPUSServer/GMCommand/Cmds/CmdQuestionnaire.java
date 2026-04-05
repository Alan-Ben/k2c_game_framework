package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * @description: 活动作弊命令
 */
@ACommander(comment = "问卷命令", name = "questionnaire")
public class CmdQuestionnaire extends UsCmdBase
{
    @ACommand(comment = "获取全部问卷列表")
    public String all()
    {
        return getUserServer().getQuestionnaireMgr().toString();
    }

    @ACommand(comment = "开启问卷[链接,问卷编码,奖励列表,持续时间]")
    public String openQuestionnaire(String _urlLink, String _questionnaireCode, String _rewardList, long _durationTimeSec)
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();
        return openQuestionnaireA(_urlLink, _questionnaireCode, _rewardList, nowTimeMS, nowTimeMS + _durationTimeSec * 1000);
    }

    @ACommand(comment = "开启问卷[链接,问卷编码,奖励列表,开始时间,结束时间]")
    public String openQuestionnaireA(String _urlLink, String _questionnaireCode, String _rewardList, long _startTimeMs, long _closeTimeMs)
    {
        return getUserServer().getQuestionnaireMgr().startQuestionnaire(_urlLink, _questionnaireCode, _rewardList, _startTimeMs, _closeTimeMs).toString();
    }

    @ACommand(comment = "修改问卷[id,链接,问卷编码,奖励列表,开始时间,结束时间]")
    public String chgQuestionnaire(long _questionnaireId, String _urlLink, String _questionnaireCode, String _rewardList, long _startTimeMs, long _closeTimeMs)
    {
        return getUserServer().getQuestionnaireMgr().chgQuestionnaire(_questionnaireId, _urlLink, _questionnaireCode, _rewardList, _startTimeMs, _closeTimeMs).toString();
    }

    @ACommand(comment = "关闭问卷[id]")
    public String closeQuestionnaire(long _questionnaireId)
    {
        return getUserServer().getQuestionnaireMgr().closeQuestionnaire(_questionnaireId).toString();
    }
}
