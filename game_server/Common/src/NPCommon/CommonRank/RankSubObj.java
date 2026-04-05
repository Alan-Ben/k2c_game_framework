package NPCommon.CommonRank;

import Common.RankObj.Rank_BaseSubItem;
import Common.ServerObj.ServerObj_RankSubObjInfo;

/**
 * @description: 排行榜数据的子集存储对象，一般用于联盟排行或者组队排行的二级信息存储
 * @author: ricci
 * @date: 2022-06-17 14:15:17
 */
public class RankSubObj
{
    //数据Id，当作为二级数据模式的时候，数据Id无效
    private long _m_lDbId;

    //数据对应Id，根据不同排行使用不同Id，具体什么Id取决于外部调用方式
    protected long _m_lSubObjId;
    //分数来源id
    protected long _m_lScoreSourceId;
    //对应分数
    protected long _m_lScore;
    //对应变更时间戳
    protected long _m_lUpdatedMs;

    public RankSubObj(long _dbId, long _subObjId, long _scoreSourceId, long _score, long _updatedMs)
    {
        _m_lDbId = _dbId;
        _m_lSubObjId = _subObjId;
        _m_lScoreSourceId = _scoreSourceId;
        _m_lScore = _score;
        _m_lUpdatedMs = _updatedMs;
    }

    //region get&&set
    public long getDBId()
    {
        return _m_lDbId;
    }

    public long getSubObjId()
    {
        return _m_lSubObjId;
    }

    public long getScoreSourceId()
    {
        return _m_lScoreSourceId;
    }

    public long getScore()
    {
        return _m_lScore;
    }

    public long getUpdatedMs()
    {
        return _m_lUpdatedMs;
    }

    /**********
     * 设置和更改分数操作
     * @param _scoreSourceId
     * @param _score
     * @param _updateTimeMs
     */
    protected void _setScore(long _scoreSourceId, long _score, long _updateTimeMs)
    {
        _m_lScoreSourceId = _scoreSourceId;
        _m_lScore = _score;
        //更新本对象的时间戳
        _m_lUpdatedMs = _updateTimeMs;
    }

    protected void _chgScore(long _scoreSourceId, long _chgValue, long _updateTimeMs)
    {
        _m_lScoreSourceId = _scoreSourceId;
        _m_lScore += _chgValue;
        //更新本对象的时间戳
        _m_lUpdatedMs = _updateTimeMs;
    }

    /**
     * 构造排行榜子对象数据协议
     * @return 排行榜子对象数据
     */
    public ServerObj_RankSubObjInfo toRankSubObjInfo()
    {
        return new ServerObj_RankSubObjInfo(_m_lSubObjId, _m_lScoreSourceId, _m_lScore, _m_lUpdatedMs);
    }
    
    public Rank_BaseSubItem toRankBaseSubItem()
    {
    	Rank_BaseSubItem proto = new Rank_BaseSubItem();
    	proto.setKey(_m_lSubObjId);
    	proto.setSourceId(_m_lScoreSourceId);
    	proto.setScore(_m_lScore);
    	
    	return proto;
    }

    //endregion
}
