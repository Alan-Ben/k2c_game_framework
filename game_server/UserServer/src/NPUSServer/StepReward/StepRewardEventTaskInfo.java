package NPUSServer.StepReward;

import Common.ActivityObj.Activity_StepRewardEventTaskInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ErrMain.StepRewardErr;
import NPCommon.Util.CallBack._ICallBackT;
import NPGameRes.Refs.StepReward.RefStepRewardSetEventTask;
import NPUSServer.NPUserServer;
import USDB.Bo.UsStepRewardEventTaskBO;

public class StepRewardEventTaskInfo 
{
    private StepRewardObj _m_stepRewardObj;
	
	private RefStepRewardSetEventTask _m_ref;
	
	private UsStepRewardEventTaskBO _m_bo;
	
	public StepRewardEventTaskInfo(StepRewardObj _stepRewardobj, UsStepRewardEventTaskBO _bo)
	{
		_m_stepRewardObj = _stepRewardobj;
		
		_m_bo = _bo;
		
		_m_ref = RefStepRewardSetEventTask.getMgr().get(_m_bo.getEventTaskId());
	}
	public StepRewardEventTaskInfo(StepRewardObj _stepRewardobj, RefStepRewardSetEventTask _ref, UsStepRewardEventTaskBO _bo)
	{
		_m_stepRewardObj = _stepRewardobj;
		
		_m_ref = _ref;
		
		_m_bo = _bo;
	}

	public StepRewardObj getStepRewardObj() {return _m_stepRewardObj;}
    public StepRewardList getStepRewardList() {return getStepRewardObj().getStepRewardList();}
    public NPUserServer getUSServer(){return getStepRewardList().getUSServer();}
	public BM getBM(){return getStepRewardList().getUSServer().getBM();}
	
	public RefStepRewardSetEventTask getRef() {return _m_ref;}
	
	public UsStepRewardEventTaskBO getBo() {return _m_bo;}
	public long getTaskId() {return getBo().getEventTaskId();}
	public long getScore() {return getBo().getScore();}
	
	public void saveScore(long _score) {getBo().saveScore(getBM(), _score);}
	
	/**
	 * 分数变更
	 * @param _cid
	 * @param _chgScore
	 * @param _callback
	 */
	public void onScoreChg(long _cid, long _chgScore, _ICallBackT<ResultOne<Long>> _callback)
    {
    	if(null == _m_ref)
    	{
            _callback.onRunOver(ResultOne.failed(CommErr.REF_NOT_FOUND));
            return;
    	}
    	
    	//检查最大分数
    	if(_m_ref.process_limit > 0 && getScore() > _m_ref.process_limit)
    	{
            _callback.onRunOver(ResultOne.failed(StepRewardErr.STEP_EVENT_SCORE_MAX));
            return;
    	}
    	
    	if(_m_ref.is_set) //设置分数
    	{
    		//不能超过最大值
    		if(_m_ref.process_limit > 0 && _chgScore > _m_ref.process_limit)
    		{
    			_chgScore = _m_ref.process_limit;
    		}
    		
    		//如果设置更大的分数，但是当前分数已经比设置的分数大，则不设置
            if (_m_ref.set_greater && getScore() > _chgScore)
            {
                _callback.onRunOver(ResultOne.failed(StepRewardErr.STEP_EVENT_SCORE_GT));
                return;
        	}
            
            getBo().saveScore(getBM(), _chgScore);
    	}
    	else //增加分数
    	{
    		long finalScore = getScore() + _chgScore;
        	if(_m_ref.process_limit > 0 && finalScore > _m_ref.process_limit)
        		finalScore = _m_ref.process_limit;
        	
        	getBo().saveScore(getBM(), finalScore);
    	}

		//发起重新计算所有任务分数
		_m_stepRewardObj.getEventTaskMgr().doLazyCalAllTaskScore();

        _callback.onRunOver(ResultOne.succ(getScore()));
    }

    /**
     * 增加分数
     * @param _chgScore 增加的分数
     */
    public void chgScore(long _chgScore)
    {
        long newScore = getScore() + _chgScore;

        getBo().saveScore(getBM(), newScore);
    }
    
    public Activity_StepRewardEventTaskInfo toProto()
    {
    	Activity_StepRewardEventTaskInfo proto = new Activity_StepRewardEventTaskInfo();
    	proto.setEventTaskId(getTaskId());
    	proto.setScore(getScore());
    	
    	return proto;
    }
}
