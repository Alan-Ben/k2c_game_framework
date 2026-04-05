package NPUSServer.StepReward;

import Common.ActivityObj.Activity_StepRewardEventTaskInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.CallBack._ICallBackT;
import NPGameRes.Refs.StepReward.RefStepRewardSetEventTask;
import NPUSServer.NPUserServer;
import USDB.Bo.UsStepRewardEventTaskBO;

import java.util.ArrayList;

/**
 * 阶段奖励事件任务数据管理，step_reward_set_event_task 配表的数据
 * @author mj
 *
 */
public class StepRewardEventTaskMgr 
{
	//阶段奖励列表
    private StepRewardObj _m_stepRewardObj;
	
	//阶段事件列表
	private ArrayList<StepRewardEventTaskInfo> _m_alEventTaskList;
	
	//所有任务分数
	private long _m_lScore;
    //火星全部建筑属性延迟计算
    private LazyTaskDealer _m_ldCalAllTaskScoreDealer;
	
	public StepRewardEventTaskMgr(StepRewardObj _stepRewardobj)
	{
		_m_stepRewardObj = _stepRewardobj;
		
		_m_alEventTaskList = new ArrayList<>();
		
		_m_ldCalAllTaskScoreDealer = new LazyTaskDealer(() -> calScore(), 200);
	}
	
	public StepRewardObj getStepRewardObj() {return _m_stepRewardObj;}
    public StepRewardList getStepRewardList() {return getStepRewardObj().getStepRewardList();}
    public NPUserServer getUSServer(){return getStepRewardList().getUSServer();}
	public BM getBM(){return getStepRewardList().getUSServer().getBM();}
	
	public long getStepRewardInstanceId() {return getStepRewardList().getDbId();}
	
	public long getScore() {return _m_lScore;}
	public void doLazyCalAllTaskScore() {_m_ldCalAllTaskScoreDealer.setNeedDeal();}
	
	private void _lock() {getStepRewardList()._lock();}
	private void _unlock() {getStepRewardList()._unlock();}

    protected void _initFromBoList(UsStepRewardEventTaskBO _bo)
    {
    	StepRewardEventTaskInfo info = new StepRewardEventTaskInfo(_m_stepRewardObj, _bo);
    	_m_alEventTaskList.add(info);
    	
    	_m_lScore += info.getScore();
    }
	
    public void calScore()
    {
    	_lock();
    	
    	try
    	{
    		long score = 0;
    		for(int i = 0; i < _m_alEventTaskList.size(); i++)
    		{
    			StepRewardEventTaskInfo info = _m_alEventTaskList.get(i);
    			if(null == info)
    				continue;
    			
    			score += info.getScore();
    		}
    		
    		if(score == _m_lScore)
    			return;
    		
    		_m_lScore = score;
    		
    		//发起重新计算总分数
    		_m_stepRewardObj.doLazyCalAllScore();
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
	/**
	 * 查找指定任务数据
	 * @param _taskId
	 * @return
	 */
	public StepRewardEventTaskInfo lookup(long _taskId)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alEventTaskList.size(); i++)
			{
				StepRewardEventTaskInfo info = _m_alEventTaskList.get(i);
				if(null == info)
					continue;
				
				if(info.getTaskId() == _taskId)
					return info;
			}
			
			return null;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 创建任务数据
	 * @param _cid
	 * @param _taskId
	 * @param _score
	 * @return
	 */
	public StepRewardEventTaskInfo addTask(long _cid, RefStepRewardSetEventTask _taskRef, long _score)
	{
		_lock();
		
		try
		{
			UsStepRewardEventTaskBO bo = new UsStepRewardEventTaskBO();
			bo.setStepRewardInstanceId(getBM(), getStepRewardInstanceId());
			bo.setCid(getBM(), _cid);
			bo.setEventTaskId(getBM(), _taskRef.id);
			bo.setScore(getBM(), _score);
			bo.insert(getBM());
			
			StepRewardEventTaskInfo info = new StepRewardEventTaskInfo(_m_stepRewardObj, _taskRef, bo);
			_m_alEventTaskList.add(info);
			
			return info;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 分数变更
	 * @param _cid
	 * @param _taskId
	 * @param _chgScore
	 * @param _callback
	 */
	public void onScoreChg(long _cid, long _taskId, long _chgScore, _ICallBackT<ResultOne<Long>> _callback)
	{
		_lock();
		
		try
		{
			StepRewardEventTaskInfo info = lookup(_taskId);
			
			//不存在则新建数据
			if(null == info)
			{
				RefStepRewardSetEventTask ref = RefStepRewardSetEventTask.getMgr().get(_taskId);
				if(null == ref)
				{
	                _callback.onRunOver(ResultOne.failed(CommErr.REF_NOT_FOUND));
	                return;
	            }
				
				//最大值检查
				if(ref.process_limit > 0 && _chgScore > ref.process_limit)
				{
					_chgScore = ref.process_limit;
				}
				
				info = addTask(_cid, ref, _chgScore);
				//发起重新计算所有任务分数
				doLazyCalAllTaskScore();
				
				_callback.onRunOver(ResultOne.succ(info.getScore()));
				
				return;
			}
			
			//更新分数变化
			info.onScoreChg(_cid, _chgScore, _callback);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 事件任务数据
	 * @param _list
	 */
	public void makeEventTaskProto(ArrayList<Activity_StepRewardEventTaskInfo> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alEventTaskList.size(); i++)
			{
				StepRewardEventTaskInfo info = _m_alEventTaskList.get(i);
				if(null == info)
					continue;
			
				_list.add(info.toProto());
			}
		}
		finally
		{
			_unlock();
		}
	}
}
