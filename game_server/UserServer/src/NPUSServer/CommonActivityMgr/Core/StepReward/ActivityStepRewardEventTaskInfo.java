package NPUSServer.CommonActivityMgr.Core.StepReward;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import GS2GC.p017_ActivityOp.GS2GC_017_063_OnActivityStepRewardEventTaskScoreChg;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.StepRewardErr;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.StepReward.RefStepRewardSetEventTask;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.RankingEvent.USRankingEventRecordCallbackMgr;
import NPUSServer.RankingEvent._IRankingEventHolder;
import NPUSServer.USLog;

public class ActivityStepRewardEventTaskInfo implements _IRankingEventHolder
{
	//对应主阶段奖励数据
	private ActivityStepRewardInfo _m_stepRewardInfo;
	
	//对应配置
	private RefStepRewardSetEventTask _m_ref;
	
    //事件注册序列号(本地生成)
    private long _m_lEventRegSerial;
    //事件监听注销序列号(目标事件管理器生成返回)
    private long _m_lEventUnRegSerial;
    
    public ActivityStepRewardEventTaskInfo(ActivityStepRewardInfo _info, RefStepRewardSetEventTask _ref)
    {
    	_m_stepRewardInfo = _info;
    	
    	_m_ref = _ref;
    }
	
	public ActivityStepRewardInfo getStepReward() {return _m_stepRewardInfo;}
    public _AActivityBase getActivity() {return getStepReward().getActivity();}
    public NPUserServer getUSServer() {return getActivity().getUSServer();}
    
    public long getStepRewardId() {return getStepReward().getStepRewardId();}
    public long getStepRewardInstanceId() {return getStepReward().getStepRewardInstanceId();}
    
    public RefStepRewardSetEventTask getRef() {return _m_ref;}
    public long getEventTaskId() {return _m_ref.id;}
    
	/**
     * 注册阶段奖励事件
     * @param _action 回调
     */
    public void regRankingEvent(_ICallBackBool _action)
    {
        //判断是否已经注册过事件
        if (_m_lEventRegSerial > 0)
        {
            _action.onRunOver(true);
            return;
        }

        //生成新的事件注册序列号，用于事件注册
        _m_lEventRegSerial = USRankingEventRecordCallbackMgr.getInstance().regRankInfoCallback(this);
        //调用注册接口发起注册
        getActivity().getUSServer().getRankingEventFunc().registerRankingEvent(_m_ref, _m_lEventRegSerial, (_result, _unRegSerial) ->
        {
            //区分注册回调是否成功
            if (_result.isSucc())
            {
                _m_lEventUnRegSerial = _unRegSerial;
                _action.onRunOver(true);
            } else
            {
                //重置事件注册序列号
                USRankingEventRecordCallbackMgr.getInstance().unregisterRankingEvent(_m_lEventRegSerial);
                _m_lEventRegSerial = 0;

                //如果注册失败则判断是否是配置未找到或者系统错误，如果是则不再重试
                if (_result.getCode() == CommErr.REF_NOT_FOUND.getCode() || _result.getCode() == CommErr.SYS_ERR.getCode())
                {
                    USLog.error(getUSServer(), "ActivityStepRewardInfo _makeSureInit _regRankingEvent fail. activityId:{} instanceId:{} stepRewardId:{} eventTaskId:{} errCode:{}",
                            getActivity().getActivityId(), getActivity().getInstanceId(), getStepRewardId(), getEventTaskId(), _result.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> regRankingEvent(_action), 3000);
            }
        });
    }

    /**
     * 注销阶段奖励事件
     * @param _action 回调
     */
    public void unRegRankingEvent(_ICallBackBool _action)
    {
        //判断是否已经注册过事件，如果注册序列号小于等于0则代表未注册
        if (_m_lEventRegSerial <= 0)
        {
            _action.onRunOver(true);
            return;
        }

        //调用注销接口发起注销
        getActivity().getUSServer().getRankingEventFunc().unregisterRankingEvent(_m_ref, _m_lEventUnRegSerial, _callbackResult ->
        {
            //判断是否注销成功
            if (_callbackResult.isSucc())
            {
                //注销排行处理
                USRankingEventRecordCallbackMgr.getInstance().unregisterRankingEvent(_m_lEventRegSerial);
                _m_lEventRegSerial = 0;
                _m_lEventUnRegSerial = 0;

                _action.onRunOver(true);
            } else
            {
                //如果注册失败则判断是否是配置未找到或者系统错误，如果是则不再重试
                if (_callbackResult.getCode() == CommErr.REF_NOT_FOUND.getCode() || _callbackResult.getCode() == CommErr.SYS_ERR.getCode())
                {
                    USLog.error(getUSServer(), "ActivityStepRewardInfo closeStepReward _unRegRankingEvent fail. activityId:{} stepRewardId:{} eventTaskId:{} errCode:{}",
                    		getActivity().getActivityId(), getStepRewardId(), getEventTaskId(), _callbackResult.getCode());

                    _action.onRunOver(false);
                    return;
                }

                //如果失败则创建一个3秒后的任务重试
                ALSynTaskManager.getInstance().regTask(() -> unRegRankingEvent(_action), 3000);
            }
        });
    }

	@Override
	public void onScoreChange(long _cid, long _scoreSourceId, long _chgScore) 
	{
        //调用阶段奖励实例分数变更接口
        getActivity().getUSServer().getStepRewardListMgr().onTaskScoreChg(getStepRewardInstanceId(), _cid, getEventTaskId(), _chgScore,
                _callback ->
        {
            if (!_callback.isSucc())
            {
            	//正常错误，不需要打印日志
            	if(StepRewardErr.STEP_EVENT_SCORE_GT.getCode() == _callback.getCode() 
            			|| StepRewardErr.STEP_EVENT_SCORE_MAX.getCode() == _callback.getCode())
            		return;
            	
                USLog.error(getActivity().getUSServer(), "ActivityStepRewardInfo onScoreChange fail. " +
                                "activityId:{} instanceId:{} stepRewardId:{} eventTask:{} errCode:{} cid:{} scoreSourceId:{} chgScore:{}",
                        getActivity().getActivityId(), getActivity().getInstanceId(), getStepRewardId(), getEventTaskId(), _callback.getCode(), _cid, _scoreSourceId, _chgScore);
                return;
            }

            //推送分数变化
            ALSynTaskManager.getInstance().regTask(()->{
                NPUSUserData userdata = getActivity().getUSServer().getUsUserMgr().lookupCacheUserData(_cid);
                if (userdata != null)
                {
                	userdata.safeCall(() -> 
                	{
                		//推送数据
                		userdata.sendMsgToGC(new GS2GC_017_063_OnActivityStepRewardEventTaskScoreChg(
                        		getActivity().getInstanceId(), getStepRewardId(), getEventTaskId(), _callback.getData()));
                	});
                }
            });
        });
    
	}
}
