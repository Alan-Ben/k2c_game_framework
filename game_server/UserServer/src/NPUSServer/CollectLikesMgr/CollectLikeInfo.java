package NPUSServer.CollectLikesMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.Util.Pair.WCGPairLong;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_COLLECT_LIKE_COUNT_CHG;
import NPUSServer.NPEvent.EventMgr.EventObj.NPGlobalUserEventObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.CollectLikeInfoBO;

public class CollectLikeInfo
{
    private CollectLikeMgr _m_mgr;
    private long _m_dbId;
    private long _m_cid;
    private long _m_likeCount;
    private long _m_lastReadLikeCount;
    private MutexAtom _m_mutex;

    public CollectLikeInfo(CollectLikeMgr _mgr, CollectLikeInfoBO _bo)
    {
        _m_mgr = _mgr;
        _m_dbId = _bo.getId();
        _m_cid = _bo.getCid();
        _m_likeCount = _bo.getLikeCount();
        _m_lastReadLikeCount = _bo.getLastReadLikeCount();
        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public long getLikeCount()
    {
        return _m_likeCount;
    }

    public void incLikeCount(NPPlayerContext _context)
    {
        _lock();
        try{
            _m_likeCount++;

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("like_count", _m_likeCount);
            _m_mgr.getServer().getBM().getBM(CollectLikeInfoBO.class).update("id", _m_dbId, updateValue);
        }finally
        {
            _unlock();
        }

        //触发事件
        Event_P_COLLECT_LIKE_COUNT_CHG event = new Event_P_COLLECT_LIKE_COUNT_CHG(_context, _m_likeCount);
        NPUSUserData userData = _m_mgr.getServer().getUsUserMgr().lookupCacheUserData(_m_cid);
        if (userData == null)
        {
            _m_mgr.getServer().getGlobalEventHandlerMgr().handle(event, new NPGlobalUserEventObj(_m_cid));
        }else
        {
            userData.onLogicEvent(event);
        }
    }

    /**
     * 获取自己的点赞次数(上次,当前)
     * @return
     */
    public WCGPairLong selfGetCount()
    {
        long lastReadLikeCount = _m_lastReadLikeCount;
        _m_lastReadLikeCount = _m_likeCount;

        //更新数据库
        if (lastReadLikeCount != _m_lastReadLikeCount)
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("last_read_like_count", _m_lastReadLikeCount);
            _m_mgr.getServer().getBM().getBM(CollectLikeInfoBO.class).update("id", _m_dbId, updateValue);
        }

        return new WCGPairLong(lastReadLikeCount, _m_lastReadLikeCount);
    }
}
