package NPGameRes.GameObjs.Reward;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.Reward.RefReward;
import NPGameRes.Refs.Reward.RefRewardSub;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class RewardMgr
{
    private static final RewardMgr _instance = new RewardMgr();

    public static RewardMgr getInstance()
    {
        return _instance;
    }

    private final Map<Long, RewardObj> _m_mRewardMap = new ConcurrentHashMap<>(); //奖励列表

    public RewardObj lookupReward(long _rewardId)
    {
        return _m_mRewardMap.get(_rewardId);
    }

    /*******
     * 从配表中初始化管理器
     *
     * 构建奖励对象，关联主奖励配置和子掉落配置
     * 注意：RewardObj 直接持有 RefRewardSub 引用，配表热更后自动生效，无需重建
     */
    public void init()
    {
        _m_mRewardMap.clear();
        for (RefReward refReward : RefReward.getMgr().getList())
        {
            RewardObj obj = new RewardObj(refReward);
            _m_mRewardMap.put(obj.getRef().Id(), obj);
        }
        for (RefRewardSub refRewardSub : RefRewardSub.getMgr().getList())
        {
            RewardObj obj = lookupReward(refRewardSub.reward_id);
            if (null == obj)
            {
                CommLog.error("can not find RefReward  for rewardId:{} subId:{}", refRewardSub.reward_id, refRewardSub.reward_sub_id);
            } else
            {
                obj.addSubDrop(refRewardSub);
            }
        }
    }

}
