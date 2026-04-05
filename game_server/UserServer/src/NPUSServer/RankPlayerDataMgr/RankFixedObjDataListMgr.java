package NPUSServer.RankPlayerDataMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.RankFixedObjDataBO;

import java.util.HashMap;
import java.util.List;

/*****************
 * 玩家点赞数量管理器
 * @author mj
 *
 */
public class RankFixedObjDataListMgr
{
    private NPUserServer _m_usUSServer;
    //排行榜对象
    private HashMap<Long, RankFixedObjDataList> _m_hmRankObjMap;
    //锁对象
    private MutexAtom _m_mutex;

    public RankFixedObjDataListMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        _m_hmRankObjMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 初始化数据
     * @return
     */
    public boolean initFromDB()
    {
        //初始化加载-排行榜数据
        List<RankFixedObjDataBO> boList = getUSServer().getBM().getBM(RankFixedObjDataBO.class).s_findAll();
        if (null == boList)
        {
            USLog.error(_m_usUSServer, "RankFixedObjDataListMgr initFromDB load db fail!");
            return false;
        }

        for (int i = 0; i < boList.size(); i++)
        {
            RankFixedObjDataBO bo = boList.get(i);
            if (null == bo)
                continue;

            ensureObj(bo.getRankFixedId()).initPlayer(bo);
        }

        return true;
    }

    /**
     * 创建排行榜对象
     * @param _rankId
     * @return
     */
    public RankFixedObjDataList ensureObj(long _rankId)
    {
        _lock();
        try
        {
            RankFixedObjDataList obj = _m_hmRankObjMap.get(_rankId);
            if (null == obj)
            {
                obj = new RankFixedObjDataList(getUSServer(), _rankId);
                _m_hmRankObjMap.put(_rankId, obj);
            }

            return obj;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找排行榜对象
     * @param _rankId
     * @return
     */
    public RankFixedObjDataList lookupObj(long _rankId)
    {
        _lock();
        try
        {
            return _m_hmRankObjMap.get(_rankId);
        } finally
        {
            _unlock();
        }
    }
}
