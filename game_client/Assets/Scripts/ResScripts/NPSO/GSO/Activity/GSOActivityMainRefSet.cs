using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 活动表数据
    /// </summary>
    [System.Serializable]
    public class GActivityMainRefObj : _IALBasicRefObj
    {
        public long _refId { get { return activity_id; } }
        public long activity_id;//活动id
        public string name;//活动名称
        public ECommonActivityType type_id;//活动类型id
        public List<long> rank_id_list;//排行榜id列表
        public List<long> step_reward_set_id_list;//阶段奖励id列表
        public long exchange_shop_id;//兑换商店id
        public long crystal_gift_pack_group_id;//钻石礼包组id
        public List<long> cash_gift_pack_group_id_list;//现金礼包组id列表
        public long step_reward_red_tip_id;//独立的阶段奖励红点id
    }

    public class GSOActivityMainRefSet : _TALSOBasicRefSet<GActivityMainRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
        public static string objName { get { return "activity_main"; } }
    }
}

