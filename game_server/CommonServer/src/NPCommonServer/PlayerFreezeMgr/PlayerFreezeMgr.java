package NPCommonServer.PlayerFreezeMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import CSDB.Bo.PlayerFreezeInfoBO;
import NPCommon.Util.CommonFunc;
import NPCommonServer.NPCommonServer;

import java.util.HashMap;
import java.util.List;

/**
 * @description: 用户冻结管理器
 * @author: ricci
 * @date: 2022-06-29 16:38:06
 */
public class PlayerFreezeMgr
{
    /**
     * 冻结角色集合 uid：冻结信息
     */
    private HashMap<String, PlayerFreezeInfo> _m_PlayerFreezeMap;

    /**
     * _m_PlayerFreezeMap 锁对象
     */
    private MutexAtom _m_mutex;

    //////单例的//////
    private static final PlayerFreezeMgr _s_instance = new PlayerFreezeMgr();


    public static PlayerFreezeMgr getInstance()
    {
        return _s_instance;
    }

    private PlayerFreezeMgr()
    {
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

    /**
     * 从数据库初始化
     * @return boolean
     */
    public boolean initFromDB()
    {
        List<PlayerFreezeInfoBO> boList = NPCommonServer.getInstance().getBM().getBM(PlayerFreezeInfoBO.class).s_findAll();
        for (PlayerFreezeInfoBO bo : boList)
        {
            if (bo == null)
            {
                continue;
            }
            _m_PlayerFreezeMap.put(bo.getUid(), new PlayerFreezeInfo(this, bo));
        }
        return true;
    }

    /**
     * 查询玩家冻结信息
     * @param _uid 玩家uid
     * @return PlayerFreezeInfo
     */
    public PlayerFreezeInfo lookup(String _uid)
    {
        _lock();
        try
        {
            return _m_PlayerFreezeMap.get(_uid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查玩家是否被冻结
     * @param _uid 玩家uid
     * @return boolean
     */
    public boolean checkPlayerIsFreeze(String _uid)
    {
        _lock();
        try
        {
            PlayerFreezeInfo freezeInfo = lookup(_uid);
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
     * 冻结角色
     * @param _uid          玩家uid
     * @param _freezeTimeMs 冻结时长
     * @return PlayerFreezeInfo
     */
    public PlayerFreezeInfo freeze(String _uid, long _freezeTimeMs)
    {
        _lock();
        try
        {
            PlayerFreezeInfo freezeInfo = lookup(_uid);
            if (freezeInfo == null)
            {
                PlayerFreezeInfoBO bo = new PlayerFreezeInfoBO();
                bo.setUid(NPCommonServer.getInstance().getBM(), _uid);
                bo.insert(NPCommonServer.getInstance().getBM());

                freezeInfo = new PlayerFreezeInfo(this, bo);
                _m_PlayerFreezeMap.put(_uid, freezeInfo);
            }
            freezeInfo.freeze(_freezeTimeMs + CommonFunc.getNowTimeMS());
            return freezeInfo;
        } finally
        {
            _unlock();
        }
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
