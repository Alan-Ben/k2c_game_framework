namespace Hotfix
{
    /// <summary>
    /// 热更工程红点常量
    /// </summary>
    public class HotfixRedTipConst
    {
        /*
         * 1、普通活动的红点直接定义对应id即可
         * 2、换皮活动的红点id需要拼接活动id，在这里只需要定义红点序号
         *    id格式为：(1 * 10 ^ 活动id长度 + 活动id) * 100 + 序号，在调用红点时，使用replaceActivity方法传入活动id拼接出完整红点id
         *    例：
         *      定义红点常量
         *      public const long ACTIVITY_TEST = 2;
         *      调用获取完整红点id
         *      HotfixRedTipConst.ACTIVITY_TEST.replaceActivity(_m_lActivityId));
         *      如果活动id是401，拼接出的红点id是140102
         */

        public const long REGULAR_EVENT_WAREHOUSE = 2;//万能活动仓库红点  (140102，401是活动id)
        public const long REGULAR_EVENT_FREE_BUY = 3;//万能活动消耗商店可免费购买 (140103，401是活动id)

        public const long TILEMATCH_FREE_BUY = 37003;//三消礼包商店有配置免费礼包可领取红点
        public const long TILEMATCH_LAYZ_CD = 37004;//三消lazy_cd红点(当前体力值达到体力上限的{TileMatchOtherRefObj.tilematch_lazy_cd_red_tip_show_per}%时显示)
        public const long TILEMATCH_STEP_REWARD_CAN_DRAW = 37005;//三消可领取阶段奖励红点

        public const long NUMMERGE_LAZY_CD = 42001;//2048体力红点(当前体力值达到体力上限的{NumMergeOtherRefObj.num_merge_lazy_cd_red_tip_show_per}%时显示)
        public const long NUMMERGE_BOX_CAN_CLAIM = 42002;//2048宝箱可领取红点
    }
}