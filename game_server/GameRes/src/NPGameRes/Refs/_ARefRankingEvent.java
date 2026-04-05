package NPGameRes.Refs;

import NPCommon.CommonObj.NPCountRate;
import NPCommon.RefData.Ref.RefBase;
import NPEnum.ERankingEventListenerType;
import NPEnum.ERankingEventServerType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPGameRes.Refs.Rank.RefRank;
import NPGameRes.Refs.StepReward.RefStepRewardSet;
import NPGameRes.Refs.StepReward.RefStepRewardSetEventTask;

/**
 * 排行榜事件配表基类
 */
public abstract class _ARefRankingEvent extends RefBase
{
    public static _ARefRankingEvent getRefRankingEvent(ERankingEventListenerType _listenerType, long _key)
    {
        _ARefRankingEvent rankingEventRef = null;

        if (_listenerType == ERankingEventListenerType.RANK)
        {
            rankingEventRef = RefRank.getMgr().get(_key);
        }
        else if(_listenerType == ERankingEventListenerType.STEP_REWARD)
        {
        	rankingEventRef = RefStepRewardSet.getMgr().get(_key);
        }
        else if(_listenerType == ERankingEventListenerType.STEP_REWARD_EVENT_TASK)
        {
        	rankingEventRef = RefStepRewardSetEventTask.getMgr().get(_key);
        }

        return rankingEventRef;
    }

    public abstract ERankingEventListenerType getListenerType();

    /**
     * 获取触发的服务器类型
     * @return 服务器类型
     */
    public abstract ERankingEventServerType getTriggerServerType();

    /**
     * 获取触发的逻辑事件ID
     * @return 逻辑事件ID
     */
    public abstract int getLogicEventId();

    /**
     * 分数来源(如：大臣id)
     * @return 分数来源
     */
    public abstract NPCountRate getScoreSource();

    /**
     * 计数器的高级公式(ENPPlayerVariableType)
     * @return 高级公式
     */
    public abstract NPPlayerVariableGroupObj getProcessCurCount();

    /**
     * 触发器count累计到进度值的倍率
     * @return 进度倍率
     */
    public abstract NPCountRate getTriggerCountRate();

    /**
     * 触发器-触发条件
     * @return 触发条件
     */
    public abstract NPPlayerConditionGroupObj getTriggerCondition();
}
