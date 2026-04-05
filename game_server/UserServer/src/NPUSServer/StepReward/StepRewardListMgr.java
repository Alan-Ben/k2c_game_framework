package NPUSServer.StepReward;

import ALBasicServer.ALBasicMutex.MutexObject;
import Common.ActivityObj.Activity_StepRewardEventTaskInfo;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ErrMain.StepRewardErr;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.Refs.StepReward.RefStepRewardSet;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsStepRewardBO;
import USDB.Bo.UsStepRewardEventTaskBO;
import USDB.Bo.UsStepRewardObjBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 阶段奖励管理器
 * 用于管理阶段奖励数据
 * 开放创建和销毁阶段奖励的接口，通过阶段奖励实例ID进行管理
 */
public class StepRewardListMgr
{
    private NPUserServer _m_usUSServer;
    private Map<Long, StepRewardList> _m_stepRewardListMap = new HashMap<>();
    private MutexObject _m_mutex = new MutexObject();

    public StepRewardListMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;
    }

    public void _lock()
    {
        _m_mutex.lock();
    }

    public void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    public boolean initFromDB()
    {
        //1.加载所有的阶段奖励列表数据
        List<UsStepRewardBO> stepRewardBoList = getUSServer().getBM().getBM(UsStepRewardBO.class).s_findAll();
        if (null == stepRewardBoList)
            return false;

        for (UsStepRewardBO usStepRewardBO : stepRewardBoList)
        {
            if (usStepRewardBO == null)
                continue;

            RefStepRewardSet refStepRewardSet = RefStepRewardSet.getMgr().get(usStepRewardBO.getStepRewardId());
            if (refStepRewardSet == null)
            {
                USLog.error(_m_usUSServer, "StepRewardListMgr initFromDB refStepReward is null, stepRewardId = " + usStepRewardBO.getStepRewardId());
                continue;
            }

            StepRewardList stepRewardList = new StepRewardList(getUSServer(), refStepRewardSet, usStepRewardBO);
            _m_stepRewardListMap.put(usStepRewardBO.getId(), stepRewardList);
        }

        //2.加载所有的阶段奖励数据
        List<UsStepRewardObjBO> stepRewardObjBoList = getUSServer().getBM().getBM(UsStepRewardObjBO.class).s_findAll();
        if (null == stepRewardObjBoList)
            return false;

        for (UsStepRewardObjBO usStepRewardObjBO : stepRewardObjBoList)
        {
            if (usStepRewardObjBO == null)
                continue;

            StepRewardList stepRewardList = lookupStepRewardList(usStepRewardObjBO.getStepRewardInstanceId());
            if (stepRewardList == null)
            {
                USLog.error(_m_usUSServer, "StepRewardListMgr initFromDB stepRewardList is null, stepRewardInstanceId = " + usStepRewardObjBO.getStepRewardInstanceId());
                continue;
            }

            stepRewardList.ensure(usStepRewardObjBO.getCid())._initStepRewardInfo(usStepRewardObjBO);
        }
        
        //3.加载所有阶段奖励事件任务数据
        List<UsStepRewardEventTaskBO> stepRewardEventTaskBoList = getUSServer().getBM().getBM(UsStepRewardEventTaskBO.class).s_findAll();
        if (null == stepRewardEventTaskBoList)
            return false;

        for (UsStepRewardEventTaskBO eventTaskBo : stepRewardEventTaskBoList)
        {
            if (eventTaskBo == null)
                continue;

            StepRewardList stepRewardList = lookupStepRewardList(eventTaskBo.getStepRewardInstanceId());
            if (stepRewardList == null)
            {
                USLog.error(_m_usUSServer, "StepRewardListMgr initFromDB stepRewardList is null, stepRewardInstanceId = " + eventTaskBo.getStepRewardInstanceId());
                continue;
            }

            stepRewardList.ensure(eventTaskBo.getCid()).getEventTaskMgr()._initFromBoList(eventTaskBo);
        }
        
        //计算所有阶段奖励的分数
        for(Map.Entry<Long, StepRewardList> entry : _m_stepRewardListMap.entrySet())
        {
        	if(null == entry || null == entry.getValue())
        		continue;
        	
        	entry.getValue()._onInited();
        }

        return true;
    }

    /**
     * 查找阶段奖励列表
     * @param _stepRewardInstanceId 阶段奖励实例ID
     * @return 阶段奖励列表
     */
    public StepRewardList lookupStepRewardList(long _stepRewardInstanceId)
    {
        _lock();
        try
        {
            return _m_stepRewardListMap.get(_stepRewardInstanceId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 创建阶段奖励列表
     * @param _stepRewardId 阶段奖励ID
     * @return 阶段奖励实例id
     */
    public long createStepRewardList(long _stepRewardId)
    {
        _lock();
        try
        {
            RefStepRewardSet refStepRewardSet = RefStepRewardSet.getMgr().get(_stepRewardId);
            if (refStepRewardSet == null)
            {
                USLog.error(_m_usUSServer, "StepRewardListMgr createStepRewardList refStepReward is null, stepRewardId = " + _stepRewardId);
                return 0;
            }

            UsStepRewardBO usStepRewardBO = new UsStepRewardBO();
            usStepRewardBO.setStepRewardId(getUSServer().getBM(), _stepRewardId);
            usStepRewardBO.insert(getUSServer().getBM());

            StepRewardList stepRewardList = new StepRewardList(getUSServer(), refStepRewardSet, usStepRewardBO);
            _m_stepRewardListMap.put(usStepRewardBO.getId(), stepRewardList);

            return stepRewardList.getDbId();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 销毁阶段奖励列表
     * @param _stepRewardInstanceId 阶段奖励实例ID
     */
    public void discardStepRewardList(long _stepRewardInstanceId)
    {
        _lock();
        try
        {
            StepRewardList stepRewardList = _m_stepRewardListMap.remove(_stepRewardInstanceId);
            if (stepRewardList == null)
                return;

            stepRewardList.discard();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 分数变化处理
     * @param _stepRewardInstanceId 阶段奖励实例ID
     * @param _cid                  主体id
     * @param _chgScore             变化分数
     */
    public void onScoreChg(long _stepRewardInstanceId, long _cid, long _chgScore, _ICallBackT<ResultOne<Long>> _callback)
    {
        onScoreChg(_stepRewardInstanceId, _cid, _chgScore, false, _callback);
    }

    /**
     * 分数变化处理，支持强制设值模式
     * @param _stepRewardInstanceId 阶段奖励实例ID
     * @param _cid                  主体id
     * @param _chgScore             变化分数
     * @param _forceSet             是否强制设值
     */
    public void onScoreChg(long _stepRewardInstanceId, long _cid, long _chgScore, boolean _forceSet, _ICallBackT<ResultOne<Long>> _callback)
    {
        _lock();
        try
        {
            StepRewardList stepRewardList = lookupStepRewardList(_stepRewardInstanceId);
            if (stepRewardList == null)
            {
                _callback.onRunOver(ResultOne.failed(StepRewardErr.STEP_REWARD_NO_EXIST));
                return;
            }

            stepRewardList.onScoreChg(_cid, _chgScore, _forceSet, _callback);
        } finally
        {
            _unlock();
        }
    }
    
    /**
     * 事件任务数据分数变更
     * @param _stepRewardInstanceId
     * @param _cid
     * @param _taskId
     * @param _chgScore
     * @param _callback
     */
    public void onTaskScoreChg(long _stepRewardInstanceId, long _cid, long _taskId, long _chgScore, _ICallBackT<ResultOne<Long>> _callback)
    {
    	_lock();
    	
    	try
    	{
    		StepRewardList stepRewardList = lookupStepRewardList(_stepRewardInstanceId);
    		if(null == stepRewardList)
    		{
                _callback.onRunOver(ResultOne.failed(StepRewardErr.STEP_REWARD_NO_EXIST));
                return;
    		}
    		
    		stepRewardList.ensure(_cid).getEventTaskMgr().onScoreChg(_cid, _taskId, _chgScore, _callback);
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    /**
     * 获取玩家当前分数
     * @param _instanceId 阶段奖励实例ID
     * @param _cid        主体id
     * @return 分数
     */
    public long getScore(long _instanceId, long _cid)
    {
        StepRewardList stepRewardList = lookupStepRewardList(_instanceId);
        if (stepRewardList == null)
            return 0;

        return stepRewardList.getScore(_cid);
    }
    
    public long getScoreObjScore(long _instanceId, long _cid)
    {
        StepRewardList stepRewardList = lookupStepRewardList(_instanceId);
        if (stepRewardList == null)
            return 0;

        return stepRewardList.getScoreObjScore(_cid);
    }

    /**
     * 获取所有玩家的分数列表
     * @param _stepRewardInstanceId 阶段奖励实例ID
     * @return
     */
    public List<WCGPairLong> getAllPlayerScore(long _stepRewardInstanceId)
    {
        StepRewardList stepRewardList = lookupStepRewardList(_stepRewardInstanceId);
        if (stepRewardList == null)
            return null;

        return stepRewardList.getAllPlayerScore();
    }
    
    /**
     * 事件任务数据
     * @param _stepRewardInstanceId
     * @param _cid
     * @param _list
     */
    public void makeEventTaskProto(long _stepRewardInstanceId, long _cid, ArrayList<Activity_StepRewardEventTaskInfo> _list)
    {
    	StepRewardList stepRewardList = lookupStepRewardList(_stepRewardInstanceId);
        if (stepRewardList == null)
            return;
    	
        stepRewardList.makeEventTaskProto(_cid, _list);
    }
}
