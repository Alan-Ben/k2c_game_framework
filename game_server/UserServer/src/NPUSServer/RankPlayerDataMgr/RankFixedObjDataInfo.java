package NPUSServer.RankPlayerDataMgr;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUserServer;
import USDB.Bo.RankFixedObjDataBO;

/*************
 * 每个玩家点赞数据管理对象
 * @author mj
 *
 */
public class RankFixedObjDataInfo
{
    private NPUserServer _m_server;
    private long _m_dbId;
    private long _m_key;
    private long _m_likeScore;
    private long _m_crossLikeScore;

    public RankFixedObjDataInfo(NPUserServer _server, RankFixedObjDataBO _bo)
    {
        _m_server = _server;
        _m_dbId = _bo.getId();
        _m_key = _bo.getKey();
        _m_likeScore = _bo.getLikeScore();
        _m_crossLikeScore = _bo.getCrossLikeScore();
    }

    public long getKey()
    {
        return _m_key;
    }
    public NPUserServer getUSServer(){return _m_server;}

    /**
     * 获取点赞数量
     * @param _isCross
     * @return
     */
    public long getLikeScore(boolean _isCross)
    {
        return _isCross ? _m_crossLikeScore : _m_likeScore;
    }

    /**
     * 点赞
     */
    protected long _incrLike(boolean _isCross)
    {
        //区分跨服和非跨服
        if (!_isCross)
        {
            return _localIncrLike();
        } else
        {
            return _crossIncrLike();
        }
    }

    /**
     * 本服点赞
     * @return
     */
    private long _localIncrLike()
    {
        _m_likeScore += RefGeneral.Ref().receive_like_score;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("like_score", _m_likeScore);
        getUSServer().getBM().getBM(RankFixedObjDataBO.class).update("id", _m_dbId, updateValue);

        return _m_likeScore;
    }

    /**
     * 跨服点赞
     * @return
     */
    private long _crossIncrLike()
    {
        _m_crossLikeScore += RefGeneral.Ref().receive_like_score;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("cross_like_score", _m_crossLikeScore);
        getUSServer().getBM().getBM(RankFixedObjDataBO.class).update("id", _m_dbId, updateValue);

        return _m_crossLikeScore;
    }
}
