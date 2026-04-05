package NPUSServer.USRank.TreasureHuntOreRank;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerTreasureHuntOreBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class TreasureHuntRankMgr
{
    private NPUserServer _m_server;
    private Map<Long, TreasureHuntRankList> _m_rankMap;
    private MutexAtom _m_mutex;

    public TreasureHuntRankMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_rankMap = new HashMap<>();
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

    /**
     * 从数据库初始化塔楼数据
     * @return
     */
    public boolean initFromDB()
    {
        List<PlayerTreasureHuntOreBO> allOreBoList = _m_server.getBM().getBM(PlayerTreasureHuntOreBO.class).s_findAll();
        if (allOreBoList == null)
            return false;

        Map<Long, List<PlayerTreasureHuntOreBO>> oreBoMap = new HashMap<>();
        // 将所有矿石数据按矿石ID分组
        allOreBoList.forEach(oreBo -> oreBoMap.computeIfAbsent(oreBo.getOreId(), k -> new ArrayList<>()).add(oreBo));
        // 遍历初始化每个排行榜
        oreBoMap.forEach((oreId, _oreBoList) -> ensureRank(oreId).initFromDB(_oreBoList));

        return true;
    }

    /**
     * 获取排行榜实例
     * @param _oreId
     * @return
     */
    public TreasureHuntRankList lookupRank(long _oreId)
    {
        _lock();
        try
        {
            return _m_rankMap.get(_oreId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取排行榜实例
     * @param _oreId
     * @return
     */
    public TreasureHuntRankList ensureRank(long _oreId)
    {
        _lock();
        try
        {
            return _m_rankMap.computeIfAbsent(_oreId, k -> new TreasureHuntRankList(_oreId));
        } finally
        {
            _unlock();
        }
    }
}
