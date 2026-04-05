package NPUSServer.CollectLikesMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Util.Pair.WCGPairLong;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserServer;
import USDB.Bo.CollectLikeInfoBO;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class CollectLikeMgr
{
    private NPUserServer _m_server;
    private Map<Long, CollectLikeInfo> _m_map;
    private MutexAtom _m_mutex;

    public CollectLikeMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_map = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    public boolean s_init()
    {
        List<CollectLikeInfoBO> boList = _m_server.getBM().getBM(CollectLikeInfoBO.class).s_findAll();
        if (boList == null)
            return false;

        for (CollectLikeInfoBO bo : boList)
        {
            CollectLikeInfo info = new CollectLikeInfo(this, bo);
            _m_map.put(bo.getCid(), info);
        }
        return true;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getServer()
    {
        return _m_server;
    }

    /**
     * 查找点赞信息
     * @param _cid
     * @return
     */
    public CollectLikeInfo lookup(long _cid)
    {
        _lock();
        try{
            return _m_map.get(_cid);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取点赞信息
     * @param _cid
     * @return
     */
    public CollectLikeInfo ensure(long _cid)
    {
        _lock();
        try{
            CollectLikeInfo info = _m_map.get(_cid);
            if (null == info)
            {
                CollectLikeInfoBO bo = new CollectLikeInfoBO();
                bo.setCid(_m_server.getBM(), _cid);
                bo.insert(_m_server.getBM());

                info = new CollectLikeInfo(this,bo);
                _m_map.put(_cid, info);
            }
            return info;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 增加点赞次数
     */
    public void incLikeCount(long _cid, NPPlayerContext _context)
    {
        CollectLikeInfo info = ensure(_cid);
        info.incLikeCount(_context);
    }

    /**
     * 获取点赞次数
     */
    public long getLikeCount(long _cid)
    {
        CollectLikeInfo info = lookup(_cid);
        return info == null ? 0 : info.getLikeCount();
    }

    /**
     * 获取自己的点赞次数(上次,当前)
     * @param _cid
     * @return
     */
    public WCGPairLong selfGetCount(long _cid)
    {
        CollectLikeInfo info = lookup(_cid);
        return info == null ? new WCGPairLong() : info.selfGetCount();
    }
}
