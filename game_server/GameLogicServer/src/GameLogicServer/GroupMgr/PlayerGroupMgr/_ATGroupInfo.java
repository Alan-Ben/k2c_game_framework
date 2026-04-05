package GameLogicServer.GroupMgr.PlayerGroupMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import GameLogicServer.GameLogicServer;

import java.util.ArrayList;

/**
 * 玩家归属主体数据，管理玩家基础数据
 */
public abstract class _ATGroupInfo<P extends _APlayerGroupPlayerInfo>
{
    //所属活动对象
    private _ATGroupMgr<P, ?> _m_gmGroupMgr;
    //群组ID
    private long _m_lGroupId;

    //团队成员cid列表
    private ArrayList<Long> _m_alMemberCidList;
    //玩家群组内的玩家数据列表，有请求的玩家才有数据
    private ArrayList<P> _m_alPlayerList;
    //锁对象，只对自身类的list进行保护，继承类根据需要增加锁保护其他数据
    private MutexAtom _m_groupMutex;

    public _ATGroupInfo(_ATGroupMgr<P, ?> _groupMgr, long _groupId)
    {
        _m_gmGroupMgr = _groupMgr;

        _m_lGroupId = _groupId;

        _m_alMemberCidList = new ArrayList<>();

        _m_alPlayerList = new ArrayList<>();

        _m_groupMutex =  new MutexAtom();
    }

    public long getGroupId()
    {
        return _m_lGroupId;
    }

    public _ATGroupMgr<P, ?> getGroupMgr() {return _m_gmGroupMgr;}

    protected void _groupLock() {_m_groupMutex.lock();}
    protected void _groupUnlock() {_m_groupMutex.unlock();}

    /**
     * 初始化玩家数据
     * @param _player
     */
    protected void _initPlayer(P _player)
    {
        _m_alMemberCidList.add(_player.getCid());
        _m_alPlayerList.add(_player);
    }

    /**
     * 获取成员数据列表
     * @return
     */
    public ArrayList<Long> getMemberCidList()
    {
        _groupLock();

        try
        {
            return new ArrayList<>(_m_alMemberCidList);
        }
        finally
        {
            _groupUnlock();
        }
    }

    /**
     * 判断玩家是否在成员列表里
     * @param _cid
     * @return
     */
    public boolean isInGroup(long _cid)
    {
        _groupLock();

        try
        {
            return _m_alMemberCidList.contains(_cid);
        }
        finally
        {
            _groupUnlock();
        }
    }

    /**
     * 更新成员玩家列表，对已经不再成员列表的玩家数据进行移除
     * @param _memberCidList
     */
    public void setMemberCidList(ArrayList<Long> _memberCidList)
    {
        _groupLock();

        try
        {
            _m_alMemberCidList.clear();
            _m_alMemberCidList.addAll(_memberCidList);

            // 逆序遍历，移除已不在成员列表中的玩家缓存数据
            for (int i = _m_alPlayerList.size() - 1; i >= 0; i--)
            {
                P player = _m_alPlayerList.get(i);
                if (null == player)
                    continue;

                if (!_m_alMemberCidList.contains(player.getCid()))
                    _m_alPlayerList.remove(i);
            }
        }
        finally
        {
            _groupUnlock();
        }
    }

    /**
     * 增加玩家数据
     * @param _player
     */
    protected void _addPlayer(P _player)
    {
        _groupLock();

        try
        {
            // 不在成员列表的，不增加
            if (!_m_alMemberCidList.contains(_player.getCid()))
                return;

            // 已存在玩家数据的，不重复增加
            for (int i = 0; i < _m_alPlayerList.size(); i++)
            {
                if (_m_alPlayerList.get(i).getCid() == _player.getCid())
                    return;
            }

            _m_alPlayerList.add(_player);
        }
        finally
        {
            _groupUnlock();
        }
    }

    /**
     * 根据玩家CID查找玩家数据
     * @param _cid
     * @return
     */
    public P lookup(long _cid)
    {
        _groupLock();

        try
        {
            for (int i = 0; i < _m_alPlayerList.size(); i++)
            {
                P player = _m_alPlayerList.get(i);
                if(null == player)
                    continue;

                if (player.getCid() == _cid)
                    return player;
            }

            return null;
        }
        finally
        {
            _groupUnlock();
        }
    }

    /**
     * 对成员数据进行协议广播
     * 按 US 分组批量发送，委托 GLS 的 broadMsg2GC 统一下发到 US
     * @param _proto 待广播的协议对象
     */
    public void broadcast(_IALProtocolStructure _proto)
    {
        // 加锁取成员列表快照，避免长时间持锁
        ArrayList<Long> memberCids;
        _groupLock();
        try
        {
            if (_m_alMemberCidList.isEmpty())
                return;
            memberCids = new ArrayList<>(_m_alMemberCidList);
        }
        finally
        {
            _groupUnlock();
        }

        if(null == memberCids)
            return;

        GameLogicServer.getInstance().broadMsg2GC(memberCids, _proto);
    }
}
