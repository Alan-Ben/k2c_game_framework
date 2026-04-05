package NPUSServer.RankPlayerDataMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.DB.BM.BM;
import NPUSServer.NPUserServer;
import USDB.Bo.RankFixedObjDataBO;

import java.util.HashMap;

/***************
 * 每个排行榜的点赞数据管理器
 * @author mj
 *
 */
public class RankFixedObjDataList
{
    private NPUserServer _m_server;
    //排行榜ID
    private long _m_lRankId;
    //排行榜玩家数据对象
    private HashMap<Long, RankFixedObjDataInfo> _m_hmRankPlayerMap;
    //锁对象
    private MutexAtom _m_mutex;

    public RankFixedObjDataList(NPUserServer _server, long _rankId)
    {
        _m_server = _server;
        _m_lRankId = _rankId;
        _m_hmRankPlayerMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    public NPUserServer getUSServer(){return _m_server;}
    public long getRankId()
    {
        return _m_lRankId;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 初始化玩家数据
     * @param _bo
     */
    public void initPlayer(RankFixedObjDataBO _bo)
    {
        RankFixedObjDataInfo player = new RankFixedObjDataInfo(getUSServer(), _bo);
        _m_hmRankPlayerMap.put(player.getKey(), player);
    }

    /**
     * 查找玩家对象
     * @param _cid
     * @return
     */
    public RankFixedObjDataInfo lookupPlayer(long _cid)
    {
        _lock();

        try
        {
            return _m_hmRankPlayerMap.get(_cid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 创建玩家对象
     * @param _cid
     * @return
     */
    public RankFixedObjDataInfo ensurePlayer(long _cid)
    {
        _lock();

        try
        {
            BM bmObj = getUSServer().getBM();

            RankFixedObjDataInfo player = _m_hmRankPlayerMap.get(_cid);
            if (null == player)
            {
                RankFixedObjDataBO bo = new RankFixedObjDataBO();
                bo.setRankFixedId(bmObj, _m_lRankId);
                bo.setKey(bmObj, _cid);
                bo.insert(bmObj);

                player = new RankFixedObjDataInfo(getUSServer(), bo);
                _m_hmRankPlayerMap.put(_cid, player);
            }

            return player;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加玩家点赞数
     * @param _cid     玩家ID
     * @param _isCross 是否跨服排行榜
     */
    public long incrLike(long _cid, boolean _isCross)
    {
        _lock();

        try
        {
            RankFixedObjDataInfo player = ensurePlayer(_cid);
            return player._incrLike(_isCross);
        } finally
        {
            _unlock();
        }
    }
}
