package NPHttpServer.Http.Entity;

import NPCommon.CommonObj.NPCommonCostItem;

import java.util.ArrayList;
import java.util.List;

/**
 * 后台推送活动排期
 * @author mj
 *
 */
public class NPEntityPushAnnouncementChg
{
    private long _m_id;
	//过期时间
	private int _m_expiredTimeSec;
	//usId列表
    private List<Integer> _m_usIdList;
	//奖励列表
    private List<NPCommonCostItem> _m_rewardList;

    public NPEntityPushAnnouncementChg()
    {
        _m_expiredTimeSec = 0;
        _m_usIdList = new ArrayList<>();
        _m_rewardList = new ArrayList<>();
    }

    public long getId()
    {
        return _m_id;
    }

    public void setId(long _id)
    {
        _m_id = _id;
    }

    public long getExpiredTimeSec()
    {
        return _m_expiredTimeSec;
    }

    public void setExpiredTimeSec(int _expiredTimeSec)
    {
        _m_expiredTimeSec = _expiredTimeSec;
    }

    public List<Integer> getUsIdList()
    {
        return _m_usIdList;
    }

    public void setUsIdList(List<Integer> _usIdList)
    {
        _m_usIdList = _usIdList;
    }

    public void addUsIdList(int _usId)
    {
        _m_usIdList.add(_usId);
    }

    public List<NPCommonCostItem> getRewardList()
    {
        return _m_rewardList;
    }

    public void setRewardList(List<NPCommonCostItem> _rewardList)
    {
        _m_rewardList = _rewardList;
    }

    public void addRewardList(NPCommonCostItem _itemInfo)
    {
        _m_rewardList.add(_itemInfo);
    }
}
