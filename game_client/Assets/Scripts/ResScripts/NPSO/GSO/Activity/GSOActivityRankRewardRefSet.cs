using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 活动排行奖励数据
    /// </summary>
    [System.Serializable]
    public class GActivityRankRewardRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id;//唯一id
        public long rank_id;//排行榜id
        public int rank_begin;//开始名次
        public int rank_end;//结束名次
        public ActivityTitleReward title_reward; //称号奖
        public List<NPCommonCostItem> reward_item_list;//奖励（盟主奖励）
        public List<NPCommonCostItem> member_reward_item_list;//盟友奖励
    }

    public class GSOActivityRankRewardRefSet : _TALSOBasicRefSet<GActivityRankRewardRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
        public static string objName { get { return "activity_rank_reward"; } }
    }
}

