package NPUSServer.CommonActivityMgr.Core.ActivityState;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ActivityEnum.EActivityState;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.UsActivityScheduleMgr.UsActivityReportPlayingToSsTask;

/**
 * 活动状态机状态 - 运行中
 * ---------------------
 * 在切换到结算状态时，排行榜和阶段奖励需要注销对应的事件监听
 */
public class ActivityState_Playing extends _AActivityState
{
    public ActivityState_Playing(_AActivityBase _activity)
    {
        super(_activity);
    }

    @Override
    public EActivityState getStateType()
    {
        return EActivityState.PLAYING;
    }

    @Override
    public boolean judgeCanToState(_AActivityState _state)
    {
        //活动状态 只能转到 冻结状态
        return EActivityState.SETTLING == _state.getStateType();
    }

    @Override
    public void enter(boolean _isTransByRestore)
    {
        if (!_isTransByRestore)
        {
            getActivity().onActivityStart();
        }

        //通知SS活动进入游玩状态
        ALSynTaskManager.getInstance().regTask(new UsActivityReportPlayingToSsTask(getActivity().getUSServer(), getActivity().getSchedule()));

        //通知活动管理器活动状态变更
        getActivity().getActivityMgr().onActivityStateChg(getActivity(), getStateType());

        getActivity().pushActivityStateChg(getStateType());
    }

    @Override
    public void quit()
    {

    }

    @Override
    public _AActivityState getCanTransToState(long _nowMs)
    {
        //如果当前时间小于活动冻结时间，则返回null
        if (_nowMs < getActivity().getEndTimeMs())
            return null;

        return new ActivityState_Settling(getActivity());
    }
}
