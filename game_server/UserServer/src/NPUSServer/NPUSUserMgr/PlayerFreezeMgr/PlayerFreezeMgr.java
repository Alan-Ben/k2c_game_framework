package NPUSServer.NPUSUserMgr.PlayerFreezeMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Util.CommonFunc;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerFreezeBO;

import java.util.HashMap;
import java.util.List;

/**
 * @description: 角色冻结管理器
 * @author: ricci
 * @date: 2022-06-29 16:38:06
 */
public class PlayerFreezeMgr
{
    private NPUserServer _m_usUSServer;
    /**
     * 冻结角色集合 cid：冻结信息
     */
    private HashMap<Long, PlayerFreezeInfo> _m_PlayerFreezeMap;

    /**
     * _m_PlayerFreezeMap 锁对象
     */
    private MutexAtom _m_mutex;

    public PlayerFreezeMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        _m_PlayerFreezeMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    /**
     * 从数据库初始化
     * @return boolean
     */
    public boolean initFromDB()
    {
        List<PlayerFreezeBO> boList = getUSServer().getBM().getBM(PlayerFreezeBO.class).s_findAll();
        for (PlayerFreezeBO bo : boList)
        {
            if (bo == null)
            {
                continue;
            }
            _m_PlayerFreezeMap.put(bo.getCid(), new PlayerFreezeInfo(this, bo));
        }
        return true;
    }

    /**
     * 查询玩家冻结信息
     * @param _cid 玩家cid
     * @return PlayerFreezeInfo
     */
    public PlayerFreezeInfo lookup(Long _cid)
    {
        _lock();
        try
        {
            return _m_PlayerFreezeMap.get(_cid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查玩家是否被冻结
     * @param _cid 玩家cid
     * @return boolean
     */
    public boolean checkPlayerIsFreeze(long _cid)
    {
        _lock();
        try
        {
            PlayerFreezeInfo freezeInfo = lookup(_cid);
            //信息不存在则未被封禁
            if (freezeInfo == null)
            {
                return false;
            }
            return freezeInfo.getFreezeTime() > CommonFunc.getNowTimeMS();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取封禁时长
     * @param _cid 角色id
     * @return 封禁时长
     */
    public long lookupFreezeTime(long _cid)
    {
        _lock();
        try
        {
            PlayerFreezeInfo freezeInfo = lookup(_cid);
            if (freezeInfo == null)
            {
                return 0;
            }
            return freezeInfo.getFreezeTime() - CommonFunc.getNowTimeMS();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 冻结角色
     * @param _cid          玩家cid
     * @param _freezeTimeMs 冻结时长
     * @return PlayerFreezeInfo
     */
    public PlayerFreezeInfo freeze(long _cid, long _freezeTimeMs)
    {
        PlayerFreezeInfo freezeInfo;
        _lock();
        try
        {
            freezeInfo = lookup(_cid);
            if (freezeInfo == null)
            {
                PlayerFreezeBO bo = new PlayerFreezeBO();
                bo.setCid(getUSServer().getBM(), _cid);
                bo.insert(getUSServer().getBM());

                freezeInfo = new PlayerFreezeInfo(this, bo);
                _m_PlayerFreezeMap.put(_cid, freezeInfo);
            }
            freezeInfo.freeze(_freezeTimeMs + CommonFunc.getNowTimeMS());

        } finally
        {
            _unlock();
        }
        //更新cache中的封禁时间
        PlayerCacheFunc.updateFreezeTime(getUSServer(), _cid, _freezeTimeMs + CommonFunc.getNowTimeMS());
        return freezeInfo;
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (PlayerFreezeInfo info : _m_PlayerFreezeMap.values())
        {
            if (info == null)
            {
                continue;
            }
            sb.append("\n").append(info).append("\n");
        }
        return "PlayerFreezeMgr{" +
                "_m_PlayerFreezeMap=" + sb +
                '}';
    }
}
