package NPServerProtocolWriter.NP2US.Request;

import NP2US_R.p009_CrossRankOp.ToUS_R_009_001_PushCrossRankScoreChg;

/**
 * CrossRankServer到UserServer的跨服排行榜相关协议Writer类
 *
 * 提供便利方法构造跨服排行榜推送协议
 */
public class ToUS_R_Writer_009_CrossRankOp
{
    /**
     * 创建跨服排行榜分数变更推送协议
     *
     * @param _cid 玩家CID
     * @param _crossInstanceId 跨服活动实例ID
     * @param _rankId 排行榜ID
     * @param _oriRank 原排名
     * @param _curRank 当前排名
     * @param _score 当前分数
     * @return 推送协议对象
     */
    public static ToUS_R_009_001_PushCrossRankScoreChg make_009_001_PushCrossRankScoreChg(
        long _cid, long _crossInstanceId, long _rankId, int _oriRank, int _curRank, long _score)
    {
        ToUS_R_009_001_PushCrossRankScoreChg proto = new ToUS_R_009_001_PushCrossRankScoreChg();
        proto.setCid(_cid);
        proto.setCrossInstanceId(_crossInstanceId);
        proto.setRankId(_rankId);
        proto.setOriRank(_oriRank);
        proto.setCurRank(_curRank);
        proto.setScore(_score);
        return proto;
    }
}
