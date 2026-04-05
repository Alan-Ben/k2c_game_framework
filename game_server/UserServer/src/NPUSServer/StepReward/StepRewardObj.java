package NPUSServer.StepReward;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.StepReward.RefStepRewardSet;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_STEP_REWARD_SCORE_CHG;
import NPUSServer.NPEvent.EventMgr.EventObj.NPGlobalUserEventObj;
import NPUSServer.NPUserServer;
import USDB.Bo.UsStepRewardObjBO;

public class StepRewardObj 
{
	//阶段任务数据
	private StepRewardList _m_stepRewardList;
	
	//玩家CID
	private long _m_lCid;
	
	//阶段任务分数对象
	private StepRewardScoreObj _m_soScoreObj;
    //玩家事件任务数据
    private StepRewardEventTaskMgr _m_mgrEventTaskMgr;
    
    //玩家分数 = 主分数 + 事件任务分数
    private long _m_lScore;
    //火星全部建筑属性延迟计算
    private LazyTaskDealer _m_ldCalAllScoreDealer;
    
    public StepRewardObj(StepRewardList _stepRewardList, long _cid)
    {
    	_m_stepRewardList = _stepRewardList;
    	
    	_m_lCid = _cid;
    	
        _m_mgrEventTaskMgr = new StepRewardEventTaskMgr(this);
        
        _m_ldCalAllScoreDealer = new LazyTaskDealer(() -> calScore(), 100);
    }

	public StepRewardList getStepRewardList() {return _m_stepRewardList;}
    public NPUserServer getUSServer(){return getStepRewardList().getUSServer();}
    
    public long getCid() {return _m_lCid;}

    public StepRewardScoreObj getScoreObj() {return _m_soScoreObj;}
    public StepRewardEventTaskMgr getEventTaskMgr() {return _m_mgrEventTaskMgr;}
    public long getScoreObjScore()
    {
    	return null != _m_soScoreObj ? _m_soScoreObj.getScore() : 0;
    }

	public long getStepRewardInstanceId() {return getStepRewardList().getDbId();}
	
	public long getScore() {return _m_lScore;}
	public void doLazyCalAllScore() {_m_ldCalAllScoreDealer.setNeedDeal();}

	private void _lock() {_m_stepRewardList._lock();}
	private void _unlock() {_m_stepRewardList._unlock();}
	
    /**
     * 初始化阶段奖励子数据
     * @param _usStepRewardObjBO 阶段奖励子数据
     */
    protected void _initStepRewardInfo(UsStepRewardObjBO _usStepRewardObjBO)
    {
    	_m_soScoreObj = new StepRewardScoreObj(this, _usStepRewardObjBO);
    }
    
    /**
     * 初始化完成触发
     * 		计算所有分数
     */
    protected void _onInited() 
    {
    	_m_lScore = 0;
    	
    	if(null != _m_soScoreObj)
		{
    		_m_lScore += _m_soScoreObj.getScore();
		}
    	
    	_m_lScore += _m_mgrEventTaskMgr.getScore();
	}
    
    /**
     * 计算所有事件对应的分数
     */
    public void calScore()
    {
    	long newScore = -1;
    	
    	_lock();
    	
    	try
    	{
    		long score = 0;
    		
    		//主分数
    		if(null != _m_soScoreObj)
    		{
    			score += _m_soScoreObj.getScore();
    		}
    		
    		//所有任务分数
    		score += _m_mgrEventTaskMgr.getScore();
    		
    		if(_m_lScore == score)
    			return;
    		
    		_m_lScore = score;
    		
    		newScore = _m_lScore;
    	}
    	finally
    	{
    		_unlock();
    	}

        //触发阶段奖励分数变动事件 
    	if(newScore != -1)
    	{
    		final long rankScore = newScore;
    		ALSynTaskManager.getInstance().regTask(() -> 
    		{
    			Event_P_STEP_REWARD_SCORE_CHG event = new Event_P_STEP_REWARD_SCORE_CHG(
                        NPPlayerContext.createNew(ENPGameEvent.STEP_REWARD_SCORE_CHG), getStepRewardList().getStepRewardId(), rankScore);
                getUSServer().getGlobalEventHandlerMgr().handle(event, new NPGlobalUserEventObj(_m_lCid));
    		});
    	}
    }
    
    /**
     * 监听主分数变化，但是需要返回全部的分数（主分数 + 所有任务的分数）
     * @param _chgScore
     * @param _callback
     */
    public void onScoreChg(long _chgScore, _ICallBackT<ResultOne<Long>> _callback)
    {
    	onScoreChg(_chgScore, false, _callback);
    }

    /**
     * 监听主分数变化，支持强制设值模式
     * @param _chgScore 变化分数
     * @param _forceSet 是否强制设值（true时忽略ref.is_set配置，直接以设值模式处理）
     * @param _callback
     */
    public void onScoreChg(long _chgScore, boolean _forceSet, _ICallBackT<ResultOne<Long>> _callback)
    {
    	_lock();

    	try
    	{
    		RefStepRewardSet ref = getStepRewardList().getStepRewardRef();
			if(null == ref)
			{
				_callback.onRunOver(ResultOne.failed(CommErr.REF_NOT_FOUND));
				return;
			}

    		if(null == _m_soScoreObj) //不存在主分数，需要新建分数
    		{
    			UsStepRewardObjBO bo = new UsStepRewardObjBO();
                bo.setStepRewardInstanceId(getUSServer().getBM(), getStepRewardInstanceId());
                bo.setCid(getUSServer().getBM(), getCid());
                bo.setScore(getUSServer().getBM(), _chgScore);
                bo.insert(getUSServer().getBM());

                _m_soScoreObj = new StepRewardScoreObj(this, bo);
    		}
    		else //存在主分数
    		{
    			if (_forceSet || ref.is_set)
                {
    				_m_soScoreObj.setScore(_chgScore, ref.set_greater);
                }
    			else
                {
                	_m_soScoreObj.chgScore(_chgScore);
                }
    		}

    		//重新发起计算所有总分，这里不使用异步处理，避免后续展示出现跳动
    		calScore();

            //回调返回当前数据，这里返回的是主任务的分数
            _callback.onRunOver(ResultOne.succ(_m_soScoreObj.getScore()));
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 构造玩家得分信息
     * @return
     */
    public WCGPairLong getScoreInfo()
    {
        return new WCGPairLong(getCid(), getScore());
    }
}
