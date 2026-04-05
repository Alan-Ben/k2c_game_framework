package NPUSServer.StepReward;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.ActivityObj.Activity_StepRewardEventTaskInfo;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.Refs.StepReward.RefStepRewardSet;
import NPUSServer.NPUserServer;
import USDB.Bo.UsStepRewardBO;
import USDB.Bo.UsStepRewardEventTaskBO;
import USDB.Bo.UsStepRewardObjBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 阶段奖励列表
 * 即单个阶段奖励的所有玩家数据
 */
public class StepRewardList
{
    private NPUserServer _m_server;

    //阶段奖励实例ID
    private long _m_dbId;
    //阶段奖励配置
    private RefStepRewardSet _m_refStepRewardSet;
    //玩家数据
    private Map<Long, StepRewardObj> _m_stepRewardMap;
    //互斥锁
    private MutexAtom _m_mutex;

    public StepRewardList(NPUserServer _server, RefStepRewardSet _refStepRewardSet, UsStepRewardBO _usStepRewardBO)
    {
        _m_server = _server;

        _m_dbId = _usStepRewardBO.getId();
        _m_refStepRewardSet = _refStepRewardSet;
        _m_stepRewardMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }
    
    /**
     * 初始完成触发
     * 		需要计算一次当前阶段分数
     */
    public void _onInited()
    {
    	for(Map.Entry<Long, StepRewardObj> entry : _m_stepRewardMap.entrySet())
    	{
    		if(null == entry || null == entry.getValue())
    			continue;
    		
    		entry.getValue()._onInited();
    	}
    }

    public void _lock()
    {
        _m_mutex.lock();
    }
    public void _unlock()
    {
        _m_mutex.unlock();
    }
    
    public RefStepRewardSet getStepRewardRef()
    {
    	return _m_refStepRewardSet;
    }

    public long getStepRewardId()
    {
        return _m_refStepRewardSet.Id();
    }

    public NPUserServer getUSServer(){return _m_server;}
    
    public long getDbId()
    {
        return _m_dbId;
    }

    private StepRewardObj _lookup(long _cid)
    {
        return _m_stepRewardMap.get(_cid);
    }

    /**
     * 确保创建排行榜元素数据
     * @return _cid 主体id
     */
    public StepRewardObj ensure(long _cid)
    {
        _lock();
        try
        {
        	return _m_stepRewardMap.computeIfAbsent(_cid, k -> new StepRewardObj(this, _cid));
        } 
        finally
        {
            _unlock();
        }
    }

    /**
     * 分数变化处理
     * @param _cid      主体id
     * @param _chgScore 变化分数
     * @param _callback
     */
    public void onScoreChg(long _cid, long _chgScore, _ICallBackT<ResultOne<Long>> _callback)
    {
        onScoreChg(_cid, _chgScore, false, _callback);
    }

    /**
     * 分数变化处理，支持强制设值模式
     * @param _cid      主体id
     * @param _chgScore 变化分数
     * @param _forceSet 是否强制设值
     * @param _callback
     */
    public void onScoreChg(long _cid, long _chgScore, boolean _forceSet, _ICallBackT<ResultOne<Long>> _callback)
    {
        _lock();
        try
        {
        	ensure(_cid).onScoreChg(_chgScore, _forceSet, _callback);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 销毁所有数据
     */
    public void discard()
    {
        _lock();
        try
        {
            _m_stepRewardMap.clear();
            
            getUSServer().getBM().getBM(UsStepRewardBO.class).delAll("id", _m_dbId);
            getUSServer().getBM().getBM(UsStepRewardObjBO.class).delAll("step_reward_instance_id", _m_dbId);
            getUSServer().getBM().getBM(UsStepRewardEventTaskBO.class).delAll("step_reward_instance_id", _m_dbId);
        } 
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取指定玩家当前分数（全部分数）
     * @param _cid 主体id
     * @return 分数
     */
    public long getScore(long _cid)
    {
        StepRewardObj stepRewardObj = _lookup(_cid);
        if (stepRewardObj == null)
            return 0;

        return stepRewardObj.getScore();
    }
    
    /**
     * 获取玩家主体事件的分数
     * @param _cid
     * @return
     */
    public long getScoreObjScore(long _cid)
    {
        StepRewardObj stepRewardObj = _lookup(_cid);
        if (stepRewardObj == null)
            return 0;

        return stepRewardObj.getScoreObjScore();
    }

    /**
     * 获取所有玩家分数
     * @return 所有玩家分数
     */
    public List<WCGPairLong> getAllPlayerScore()
    {
        _lock();
        try
        {
            List<WCGPairLong> list = new ArrayList<>();
            for (StepRewardObj stepRewardObj : _m_stepRewardMap.values())
            {
                list.add(stepRewardObj.getScoreInfo());
            }
            return list;
        } finally
        {
            _unlock();
        }
    }
    
    /**
     * 事件任务数据
     * @param _cid
     * @param _list
     */
    public void makeEventTaskProto(long _cid, ArrayList<Activity_StepRewardEventTaskInfo> _list)
    {
    	StepRewardObj stepRewardObj = _lookup(_cid);
    	if(null == stepRewardObj)
    		return;
    	
    	stepRewardObj.getEventTaskMgr().makeEventTaskProto(_list);
    }
}
