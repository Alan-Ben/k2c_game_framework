package NPUSServer.CommonActivityMgr.Core.Rank.RewardRecord;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ActivityObj.Activity_RankSettleInfo;
import NPUSServer.NPUserServer;
import USDB.Bo.ActivityRankRewardInfoBO;

/**
 * 活动排行榜的奖励信息，用于存储玩家的奖励领取状态
 */
public class ActivityRankRewardInfo
{
    private ActivityRankRewardMgr _m_mgr;
    private long _m_dbId;
    //玩家cid
    private long _m_cid;
    //归属的团体id（仅在团体排行榜中有效）
    private long _m_groupId;
    //排名
    private int _m_rank;
    //是否已领取
    private boolean _m_hadDraw;
    //分数
    private long _m_score;

    public ActivityRankRewardInfo(ActivityRankRewardMgr _mgr, ActivityRankRewardInfoBO _bo)
    {
        _m_mgr = _mgr;
        _m_dbId = _bo.getId();
        _m_cid = _bo.getCid();
        _m_groupId = _bo.getGroupId();
        _m_rank = _bo.getRank();
        _m_hadDraw = _bo.getHadDraw();
        _m_score = _bo.getScore();
    }

    public NPUserServer getUSServer()
    {
        return _m_mgr.getUSServer();
    }

    public long getCid()
    {
        return _m_cid;
    }
    public long getGroupId()
    {
    	return _m_groupId;
    }
    public boolean hadDraw()
    {
        return _m_hadDraw;
    }
    public int getRank()
    {
        return _m_rank;
    }

    public void setHadDraw(boolean _hadDraw)
    {
        _m_hadDraw = _hadDraw;

        //保存到数据库
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("had_draw", 1);
        getUSServer().getBM().getBM(ActivityRankRewardInfoBO.class).update("id", _m_dbId, updateValue);
    }
    
    /**
     * 只设置内存标志位（新流程改造）
     * @param _hadDraw
     */
    public void setHadDrawFlag(boolean _hadDraw)
    {
    	_m_hadDraw = _hadDraw;
    }

    /**
     * 构造结算信息
     * @return 结算信息
     */
    public Activity_RankSettleInfo makeSettleInfo()
    {
        Activity_RankSettleInfo settleInfo = new Activity_RankSettleInfo();
        settleInfo.setRankId(_m_mgr.getRank().getRankId());
        settleInfo.setRank(_m_rank);
        settleInfo.setHadDraw(_m_hadDraw);
        settleInfo.setScore(_m_score);
        return settleInfo;
    }
}
