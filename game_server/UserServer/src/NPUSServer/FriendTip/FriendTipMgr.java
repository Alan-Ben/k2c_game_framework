package NPUSServer.FriendTip;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerBO;

import java.util.*;

/**
 * 好友推荐管理器
 * 管理玩家的国力和等级数据，用于好友推荐
 * 需要实现玩家登录时的数据更新，和玩家离线时的数据更新并写入数据库
 * 需要提供方法用于获取推荐好友列表，传入玩家的国力和等级，返回推荐好友列表
 */
public class FriendTipMgr
{
    private NPUserServer _m_usUSServer;
    //玩家id和好友推荐信息的映射，仅存储在线玩家数据
    private Set<Long> _m_onlineCidSet = new HashSet<>();
    private Set<Long> _m_recentOfflineCidSet = new LinkedHashSet<>();
    private MutexObject _m_mapLock = new MutexObject();

    public FriendTipMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    /**
     * 锁和解锁
     */
    private void _lock()
    {
        _m_mapLock.lock();
    }

    private void _unlock()
    {
        _m_mapLock.unlock();
    }

    /**
     * 初始化最近离线玩家数据
     * @return
     */
    public boolean initFromDb()
    {
        List<PlayerBO> playerList = getUSServer().getBM().getBM(PlayerBO.class).s_findAllBySort("latest_login_time_ms", false, 100);
        if (playerList == null)
            return false;

        for (PlayerBO PlayerBO : playerList)
        {
            if (PlayerBO == null)
                continue;

            _m_recentOfflineCidSet.add(PlayerBO.getCid());
        }

        return true;
    }


    /**
     * 获取在线玩家列表
     * @return
     */
    private List<Long> _getOnlinePlayerList()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_onlineCidSet);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取最近离线玩家列表
     * @return
     */
    private List<Long> _getRecentOfflinePlayerList()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_recentOfflineCidSet);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 玩家登录
     * @param _cid 玩家id
     */
    public void onlineUpdateInfo(long _cid)
    {
        _lock();
        try
        {
            //尝试从离线列表中删除
            _m_recentOfflineCidSet.remove(_cid);

            //加入到在线列表中
            _m_onlineCidSet.add(_cid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 下线更新玩家数据
     * @param _cid 玩家id
     */
    public void offlineUpdateInfo(long _cid)
    {
        _lock();
        try
        {
            //尝试从在线列表中删除
            _m_onlineCidSet.remove(_cid);
            
            //加入到离线列表中
            _m_recentOfflineCidSet.add(_cid);

            //如果超过100个元素,删除最早加入的元素
            if (_m_recentOfflineCidSet.size() > 100) {
                Iterator<Long> iterator = _m_recentOfflineCidSet.iterator();
                iterator.next();
                iterator.remove();
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取推荐好友列表
     * @param _cid           玩家id
     * @param _friendCidList 玩家当前好友列表
     */
    public List<Long> getRecommendList(long _cid, List<Long> _friendCidList)
    {
        //1.首先获取在线玩家列表
        List<Long> onlinePlayerList = _getOnlinePlayerList();
        Collections.shuffle(onlinePlayerList);
        //2.从最近离线的玩家里取
        List<Long> offlinePlayerList = _getRecentOfflinePlayerList();
        Collections.shuffle(offlinePlayerList);

        List<Long> recommendList = new ArrayList<>();

        //3.先从在线玩家里取
        for (Long onlineCid : onlinePlayerList)
        {
            if (recommendList.size() >= RefGeneral.Ref().friend_recommend_num)
                break;

            //跳过自己和已拥有好友
            if (onlineCid == _cid || _friendCidList.contains(onlineCid))
                continue;

            recommendList.add(onlineCid);
        }

        //4.再从离线玩家里取
        for (int i = offlinePlayerList.size() - 1; i >= 0; i--)
        {
            Long offlineCid = offlinePlayerList.get(i);

            if (recommendList.size() >= RefGeneral.Ref().friend_recommend_num)
                break;

            //跳过自己和已拥有好友
            if (offlineCid == _cid || _friendCidList.contains(offlineCid))
                continue;

            recommendList.add(offlineCid);
        }

        return recommendList;
    }
}
