package NPUSServer.Guild.MarsMineShare;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.GuildObj.Guild_MineShareInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.NPUserServer;
import USDB.Bo.GuildMarsMineShareBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 联盟分享矿管理器
 */
public class GuildMarsMineShareMgr
{
    // 联盟数据对象
    private GuildInfo _m_guildInfo;

    // 分享矿列表
    private ArrayList<GuildMarsMineShareInfo> _m_shareList;
    //最大ID，用于客户端红点展示
    private long _m_lMaxId;

    // 分享矿列表锁
    private MutexAtom _m_shareMutex;

    public GuildMarsMineShareMgr(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;
        _m_shareList = new ArrayList<>();
        _m_shareMutex = new MutexAtom();
    }

    public GuildInfo getGuildInfo()
    {
        return _m_guildInfo;
    }

    public BM getBM()
    {
        return _m_guildInfo.getGuildMgr().getServer().getBM();
    }

    public NPUserServer getUSServer()
    {
        return _m_guildInfo.getGuildMgr().getServer();
    }

    protected void _lock()
    {
        _m_shareMutex.lock();
    }

    protected void _unlock()
    {
        _m_shareMutex.unlock();
    }
    
    /**
     * 获取最大ID
     * @return
     */
    public long getMaxId() {return _m_lMaxId;}

    /**
     * 从BO对象初始化分享矿数据
     */
    public void _initFromBO(GuildMarsMineShareBO _bo)
    {
        _m_shareList.add(new GuildMarsMineShareInfo(this, _bo));
        
        if(_bo.getId() > _m_lMaxId)
        {
        	_m_lMaxId = _bo.getId();
        }
    }

    /**
     * 查询
     * @param _dbId
     */
    public GuildMarsMineShareInfo lookup(long _dbId)
    {
        _lock();
        try{
            for (GuildMarsMineShareInfo shareInfo : _m_shareList)
            {
                if (shareInfo.getDbId() == _dbId)
                {
                    return shareInfo;
                }
            }
            return null;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 通过矿实例ID查询
     * @param _mineInstanceId 矿实例ID
     * @return 找不到返回null
     */
    public GuildMarsMineShareInfo lookupByInstanceId(long _mineInstanceId)
    {
        _lock();
        try{
            for (GuildMarsMineShareInfo shareInfo : _m_shareList)
            {
                if (shareInfo.getMineInstanceId() == _mineInstanceId)
                {
                    return shareInfo;
                }
            }
            return null;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 添加分享矿
     */
    public boolean addMineShare(long _finderCid, long _mineInstanceId, long _posId, long _mineEndShowMs)
    {
        _lock();

        try
        {
            // 检查移除过期分享信息
            checkRemoveExpiredItem();

            // 去重检查
            if (mineHadExist(_mineInstanceId))
                return false;

            // 超过上限则移除最旧记录
            checkRemoveOverLimitItem();

            // 插入数据库
            GuildMarsMineShareBO bo = new GuildMarsMineShareBO();
            bo.setGuildId(getBM(), getGuildInfo().getGuildId());
            bo.setFinderCid(getBM(), _finderCid);
            bo.setMineInstanceId(getBM(), _mineInstanceId);
            bo.setMineEndShowMs(getBM(), _mineEndShowMs);
            bo.setPosId(getBM(), _posId);
            bo.insert(getBM());

            // 添加到内存列表
            GuildMarsMineShareInfo newInfo = new GuildMarsMineShareInfo(this, bo);
            _m_shareList.add(newInfo);
            
            _m_lMaxId = bo.getId();
            
            //推送联盟全部玩家
            ALSynTaskManager.getInstance().regTask(() -> 
            {
            	getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_041_MarsExploreOp.make_063_OnGuildMarsMineShareAdd(_m_guildInfo));
            });

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查移除超过上限分享信息
     */
    private void checkRemoveOverLimitItem()
    {
        _lock();
        try{
            int maxCount = RefGeneral.Ref().mars_mine_guild_share_limit;
            if (_m_shareList.size() >= maxCount)
            {
                GuildMarsMineShareInfo oldestInfo = _m_shareList.remove(0);
                oldestInfo.deleteFromDB();
            }
        }finally
        {
            _unlock();
        }
    }

    /**
     * 矿实例是否存在
     * @param _mineInstanceId
     * @return
     */
    private boolean mineHadExist(long _mineInstanceId)
    {
        _lock();
        try{
            for (GuildMarsMineShareInfo info : _m_shareList)
            {
                if (info.getMineInstanceId() == _mineInstanceId)
                {
                    return true;
                }
            }
            return false;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 清理过期矿
     */
    public void checkRemoveExpiredItem()
    {
        _lock();
        try
        {
            long currentTimeMs = CommonFunc.getNowTimeMS();
            List<Long> expiredDbIds = new ArrayList<>();

            for (int i = _m_shareList.size() - 1; i >= 0; i--)
            {
                GuildMarsMineShareInfo info = _m_shareList.get(i);
                if (currentTimeMs >= info.getMineEndShowMs())
                {
                    _m_shareList.remove(i);
                    expiredDbIds.add(info.getDbId());
                }
            }

            getBM().getBM(GuildMarsMineShareBO.class).delAllInList("id", expiredDbIds);
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
            _m_shareList.clear();
            getBM().getBM(GuildMarsMineShareBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 根据实例ID移除分享矿
     * @param _instanceId
     */
    public void removeByInstanceId(long _instanceId)
    {
        _lock();
        try
        {
            for (int i = _m_shareList.size() - 1; i >= 0; i--)
            {
                GuildMarsMineShareInfo info = _m_shareList.get(i);
                if (info.getDbId() == _instanceId)
                {
                    _m_shareList.remove(i);
                    info.deleteFromDB();
                    return;
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造分享矿列表协议
     */
    public List<Guild_MineShareInfo> makeShareListProto()
    {
        _lock();
        try
        {
            List<Guild_MineShareInfo> protoList = new ArrayList<>();
            for (GuildMarsMineShareInfo info : _m_shareList)
            {
                protoList.add(info.toProto());
            }
            return protoList;
        } finally
        {
            _unlock();
        }
    }
}
