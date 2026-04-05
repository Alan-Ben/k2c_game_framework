package NPUSServer.CommonActivityMgr.Core.StepReward;

import NPGameRes.Refs.StepReward.RefStepRewardSet;
import NPGameRes.Refs.StepReward.RefStepRewardSetEventTask;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;

import java.util.ArrayList;

/**
 * 管理阶段事件任务监听，数据源配表：step_reward_set_event_task
 * @author mj
 *
 */
public class ActivityStepRewardEventTaskMgr 
{
	//对应主阶段奖励数据
	private ActivityStepRewardInfo _m_stepRewardInfo;
	
	//事件任务数据列表
	private ArrayList<ActivityStepRewardEventTaskInfo> _m_alEventTaskList;
	
	public ActivityStepRewardEventTaskMgr(ActivityStepRewardInfo _info)
	{
		_m_stepRewardInfo = _info;
		
		_m_alEventTaskList = new ArrayList<>();
		
		_initAllFromRef();
	}
	
	public ActivityStepRewardInfo getStepReward() {return _m_stepRewardInfo;}
	public _AActivityBase getActivity() {return getStepReward().getActivity();}
	public long getStepRewardInstanceId() {return getStepReward().getStepRewardInstanceId();}
	
	public ArrayList<ActivityStepRewardEventTaskInfo> getEventTaskList() {return _m_alEventTaskList;}
	
	/**
	 * 加载关联所有事件任务配置
	 */
	private void _initAllFromRef()
	{
		RefStepRewardSet setRef = _m_stepRewardInfo.getStepRewardRef();
		if(null == setRef)
			return;
		
		ArrayList<RefStepRewardSetEventTask> eventTaskRefList = setRef.eventTaskRefList;
		for(int i = 0; i < eventTaskRefList.size(); i++)
		{
			RefStepRewardSetEventTask eventTaskRef = eventTaskRefList.get(i);
			if(null == eventTaskRef)
				continue;
			
			ActivityStepRewardEventTaskInfo eventTaskInfo = new ActivityStepRewardEventTaskInfo(_m_stepRewardInfo, eventTaskRef);
			_m_alEventTaskList.add(eventTaskInfo);
		}
	}
}
