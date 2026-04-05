package ActivitiesV01.Activities.TileMatchActivity;

import ALBasicServer.ALBasicMutex.MutexObject;
import ActivitiesV01.Bo.TileMatchPlayerGameInfoBO;
import ActivitiesV01.Bo.TileMatchPlayerInfoBO;
import NPUSServer.USLog;

import java.util.HashMap;
import java.util.Map;

public class TileMatchPlayerMgr
{
    //活动信息
    private TileMatchActivity _m_activity;
    //玩家信息
    private Map<Long, TileMatchPlayerInfo> _m_mapPlayerInfo;
    //锁对象
    private MutexObject _m_mutex;

    public TileMatchPlayerMgr(TileMatchActivity _activity)
    {
        _m_activity = _activity;
        _m_mutex = new MutexObject();
        _m_mapPlayerInfo = new HashMap<>();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public TileMatchActivity getActivity()
    {
        return _m_activity;
    }

    /**
     * 初始化玩家数据
     * @param _bo
     */
    public void initFromDB(TileMatchPlayerInfoBO _bo)
    {
        _m_mapPlayerInfo.put(_bo.getCid(), new TileMatchPlayerInfo(this, _bo));
    }

    /**
     * 初始化游戏数据
     * @param _bo
     */
    public void initGameFromDB(TileMatchPlayerGameInfoBO _bo)
    {
        TileMatchPlayerInfo playerInfo = lookup(_bo.getCid());
        if (playerInfo == null)
        {
            USLog.error(getClass().getName(), "TileMatchPlayerMgr initGameFromDB, playerInfo is null, cid:{}", _bo.getCid());
            return;
        }

        playerInfo.initGameFromDB(_bo);
    }

    /**
     * 按key查询接口
     * @param _cid 玩家id
     * @return
     */
    public TileMatchPlayerInfo lookup(long _cid)
    {
        _lock();
        try
        {
            return _m_mapPlayerInfo.get(_cid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保游戏数据生成
     * @param _cid 玩家id
     * @return
     */
    public TileMatchPlayerInfo ensure(long _cid)
    {
        _lock();
        try
        {
            TileMatchPlayerInfo info = lookup(_cid);
            if (info == null)
            {
                TileMatchPlayerInfoBO bo = new TileMatchPlayerInfoBO();
                bo.setActivityInstanceId(_m_activity.getUSServer().getBM(), _m_activity.getInstanceId());
                bo.setCid(_m_activity.getUSServer().getBM(), _cid);
                bo.setStepRewardStep(_m_activity.getUSServer().getBM(), 1);
                bo.insert(_m_activity.getUSServer().getBM());
                info = new TileMatchPlayerInfo(this, bo);
                _m_mapPlayerInfo.put(bo.getCid(), info);
            }
            return info;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 发放所有未领取的阶梯奖励
     */
    public void dispatchUnclaimedStepRewards()
    {
        _lock();
        try{
            for (TileMatchPlayerInfo playerInfo : _m_mapPlayerInfo.values())
            {
                playerInfo.dispatchUnclaimedStepRewards();
            }
        }finally
        {
            _unlock();
        }
    }
}
