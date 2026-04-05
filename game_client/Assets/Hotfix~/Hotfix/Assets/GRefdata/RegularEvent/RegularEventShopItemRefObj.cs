namespace Hotfix
{
    /// <summary>
    /// 万能活动商店商品表
    /// </summary>
    public class RegularEventShopItemRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return id; } }

        public long id;//唯一id
        public long activity_id;//活动id
        public bool can_direct_buy;//是否可以直接购买
        public long buy_num;//可购买次数
        public NPCommonCostItem item;//商品
        public NPCommonCostItem buy_cost;//购买消耗的道具
        public long use_gain_activity_currency_count;//获得兑换券数量

        protected override void _parseFromString(string _line)
        {
            id = getLong("id");
            activity_id = getLong("activity_id");
            can_direct_buy = getBool("can_direct_buy");
            buy_num = getLong("buy_num");
            item = NPCommonCostItem.readFromStr(getString("item"));
            //免费的情况可为空
            string buyCostStr = getString("buy_cost");
            buy_cost = string.IsNullOrEmpty(buyCostStr) ? null : NPCommonCostItem.readFromStr(buyCostStr);
            use_gain_activity_currency_count = getLong("use_gain_activity_currency_count");
        }
        
        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "regular_event_shop_item"; } }
    }
}