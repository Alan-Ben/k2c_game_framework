package NPUSServer.USRank.TreasureHuntOreRank;

import Common.TreasureHuntObj.TreasureHunt_OreRankItem;

public class TreasureHuntRankItem
{
    private final long _m_cid;
    private int _m_maxRecord;
    private int _m_rank;
    private long _m_reachTimeMs;

    public TreasureHuntRankItem(long _cid, int _score, long _timeMs)
    {
        _m_cid = _cid;
        _m_maxRecord = _score;
        _m_reachTimeMs = _timeMs;
    }

    public long getCid()
    {
        return _m_cid;
    }

    /**
     * 设置排名
     * @param _rank
     */
    public void setRank(int _rank)
    {
        _m_rank = _rank;
    }

    /**
     * 获取排名
     * @return
     */
    public int getRank()
    {
        return _m_rank;
    }

    /**
     * 获取最大记录
     * @return
     */
    public int getMaxRecord()
    {
        return _m_maxRecord;
    }

    /**
     * 获取达成时间
     * @return
     */
    public long getReachTimeMs()
    {
        return _m_reachTimeMs;
    }

    /**
     * 更新玩家的最高记录
     * @param _maxRecord
     * @param _timeMs
     */
    public void updateMaxRecord(int _maxRecord, long _timeMs)
    {
        if (_m_maxRecord > _maxRecord)
            return;

        _m_maxRecord = _maxRecord;
        _m_reachTimeMs = _timeMs;
    }

    /**
     * 构造协议
     * @return
     */
    public TreasureHunt_OreRankItem makeProto()
    {
        return new TreasureHunt_OreRankItem(_m_rank, _m_cid, _m_maxRecord);
    }
}
