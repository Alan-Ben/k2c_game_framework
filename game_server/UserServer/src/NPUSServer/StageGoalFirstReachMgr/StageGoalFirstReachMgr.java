package NPUSServer.StageGoalFirstReachMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.StageGoalObj.StageGoal_BigStepFirstReachInfo;
import Common.StageGoalObj.StageGoal_TopPlayerInfo;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Util.Delegate.HandlerTwo;
import NPGameRes.Refs.StageGoal.RefStageGoalBigStep;
import NPUSServer.NPUserServer;
import USDB.Bo.BigStageGoalFirstReachBO;
import USDB.Bo.StageGoalTopPlayerBO;

import java.util.ArrayList;
import java.util.List;

public class StageGoalFirstReachMgr
{
    private NPUserServer _m_server;
    private List<StageGoalFirstReachInfo> _m_StageGoalFirstReachInfoList;
    private MutexObject _m_mutex;

    private long _m_dbId; // 数据库ID
    private long _m_top1Cid; // TOP1玩家CID
    private long _m_top1Step; // TOP1玩家阶段
    private long _m_top1CacheEarnings; // TOP1玩家缓存收益

    public StageGoalFirstReachMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_StageGoalFirstReachInfoList = new ArrayList<>();
        _m_mutex = new MutexObject();
    }

    public void _lock()
    {
        _m_mutex.lock();
    }

    public void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getServer()
    {
        return _m_server;
    }

    /**
     * 从数据库初始化数据
     * @return
     */
    public boolean initFromDB()
    {
        List<StageGoalTopPlayerBO> topPlayerBoList = getServer().getBM().getBM(StageGoalTopPlayerBO.class).s_findAll();
        if (topPlayerBoList == null)
            return false;
        if (!topPlayerBoList.isEmpty())
        {
            StageGoalTopPlayerBO bo = topPlayerBoList.get(0);
            _m_dbId = bo.getId();
            _m_top1Cid = bo.getCid();
            _m_top1Step = bo.getStep();
            _m_top1CacheEarnings = 0;
        }

        List<BigStageGoalFirstReachBO> boList = getServer().getBM().getBM(BigStageGoalFirstReachBO.class).s_findAll();
        if (boList == null)
            return false;
        for (BigStageGoalFirstReachBO bo : boList)
        {
            StageGoalFirstReachInfo info = ensure(bo.getBigStageId());
            info.initAddRecord(bo);
        }

        for (StageGoalFirstReachInfo firstReachInfo : _m_StageGoalFirstReachInfoList)
        {
            firstReachInfo.sort();
        }

        return true;
    }

    /**
     * 查找大关卡目标首达成信息
     * @param _bigStageId
     * @return
     */
    public StageGoalFirstReachInfo lookup(long _bigStageId)
    {
        _lock();
        try
        {
            for (StageGoalFirstReachInfo info : _m_StageGoalFirstReachInfoList)
            {
                if (info.getBigStageId() == _bigStageId)
                {
                    return info;
                }
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保大关卡目标首达成信息存在
     * @param _bigStageId
     * @return
     */
    public StageGoalFirstReachInfo ensure(long _bigStageId)
    {
        _lock();
        try
        {
            StageGoalFirstReachInfo info = lookup(_bigStageId);
            if (info == null)
            {
                info = new StageGoalFirstReachInfo(this, _bigStageId);
                _m_StageGoalFirstReachInfoList.add(info);
            }

            return info;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 记录达成大阶段目标的玩家数据
     */
    public void recordFirstReach(long _cid, String _name, RefStageGoalBigStep _ref, long _reachTimeMs)
    {
        StageGoalFirstReachInfo info = ensure(_ref.big_step);
        info.addRecord(_cid, _name, _ref, _reachTimeMs);
    }

    /**
     * 记录TOP1玩家数据
     */
    public void recordTop1PlayerData(long _cid, long _earnings, long _step)
    {
        _lock();
        try
        {
            if (_m_dbId == 0)
            {
                StageGoalTopPlayerBO bo = new StageGoalTopPlayerBO();
                bo.setCid(_m_server.getBM(), _cid);
                bo.setStep(_m_server.getBM(), _step);
                bo.insert(_m_server.getBM());

                _m_dbId = bo.getId();
                _m_top1Cid = _cid;
                _m_top1Step = _step;
                _m_top1CacheEarnings = _earnings;
            } else
            {
                if (_step < _m_top1Step)
                    return;

                if (_step == _m_top1Step && _earnings <= _m_top1CacheEarnings)
                    return;

                _m_top1Cid = _cid;
                _m_top1Step = _step;
                _m_top1CacheEarnings = _earnings;

                ALMySqlUpdateValue update = new ALMySqlUpdateValue();
                update.addValueObj("cid", _m_top1Cid);
                update.addValueObj("step", _m_top1Step);
                getServer().getBM().getBM(StageGoalTopPlayerBO.class).update("id", _m_dbId, update);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 是否解锁
     * @param _bigStageId
     * @return
     */
    public boolean isUnlock(long _bigStageId)
    {
        StageGoalFirstReachInfo firstReachInfo = lookup(_bigStageId);
        return firstReachInfo != null && firstReachInfo.isReach();
    }

    public List<StageGoal_BigStepFirstReachInfo> makeDetailInfo(long _bigStepId)
    {
        StageGoalFirstReachInfo firstReachInfo = lookup(_bigStepId);
        if (firstReachInfo == null)
            return null;

        return firstReachInfo.makeDetailInfo();
    }

    /**
     * 构造大阶段目标首达成TOP1玩家基础信息
     * @return
     */
    public StageGoal_TopPlayerInfo makeTop1BaseInfo()
    {
        _lock();
        try
        {
            return new StageGoal_TopPlayerInfo(_m_top1Cid, _m_top1Step);
        } finally
        {
            _unlock();
        }
    }


    /**
     * 构造大阶段目标首达成基础信息列表
     * @return
     */
    public List<StageGoal_BigStepFirstReachInfo> makeBaseList()
    {
        List<StageGoal_BigStepFirstReachInfo> list = new ArrayList<>();
        _lock();
        try
        {
            for (StageGoalFirstReachInfo info : _m_StageGoalFirstReachInfoList)
            {
                if (info == null)
                    continue;

                StageGoal_BigStepFirstReachInfo proto = info.makeFirstBaseInfo();
                if (proto != null)
                    list.add(proto);
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 加载用户收益
     */
    public void loadUserEarnings()
    {
        long cid = _m_top1Cid;
        if (cid == 0)
            return;

        getServer().getPlayerCacheGetter().getDealer(PlayerInfo_IconShow.class).getInfo(cid, new HandlerTwo<Boolean, PlayerInfo_IconShow>()
        {
            @Override
            public void handle(Boolean _isSucc, PlayerInfo_IconShow _data)
            {
                if (_isSucc && _data != null)
                {
                    _lock();
                    try
                    {
                        if (cid != _m_top1Cid)
                            return;

                        _m_top1CacheEarnings = _data.getEarnings();
                    } finally
                    {
                        _unlock();
                    }
                }
            }
        });
    }

    /**
     * 获取可以领取的首达列表
     * @return
     */
    public List<Long> getCanDrawBigStepFirstReachList()
    {
        List<Long> list = new ArrayList<>();
        _lock();
        try
        {
            for (StageGoalFirstReachInfo info : _m_StageGoalFirstReachInfoList)
            {
                if (info == null)
                    continue;

                if (info.isReach())
                    list.add(info.getBigStageId());
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清空所有记录
     * 清除内存中的所有首达记录和TOP1玩家数据，并从数据库中删除相关记录
     */
    public void clearAllRecords()
    {
        _lock();
        try
        {
            // 清空内存中的首达记录列表
            _m_StageGoalFirstReachInfoList.clear();

            // 重置TOP1玩家数据
            _m_dbId = 0;
            _m_top1Cid = 0;
            _m_top1Step = 0;
            _m_top1CacheEarnings = 0;

            // 清空数据库中的首达记录
            getServer().getBM().getBM(BigStageGoalFirstReachBO.class).delAll();

            // 清空数据库中的TOP1玩家记录
            getServer().getBM().getBM(StageGoalTopPlayerBO.class).delAll();

        } finally
        {
            _unlock();
        }
    }
}
