using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GiftPackPatchDealer : _ACommonListHotRefPatchDealer
    {
        // 表名
        public override string getTableName
        {
            get { return GSOGiftPackRefSet.objName; }
        }

        // 补丁数据应用
        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;

            GiftPackRefObj obj = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_refId);
            if (obj == null)
                return false;
            
            if (_valueDict.TryGetValue("buy_limit_count", out string buyLimitCountStr))
                obj.buy_limit_count = ALCommon.GetInt(buyLimitCountStr);

            if (_valueDict.TryGetValue("buy_limit_refresh_time", out string buyLimitRefreshTimeStr))
                obj.buy_limit_refresh_time = NPTimeRefreshInfo.readFromStr(buyLimitRefreshTimeStr);

            if (_valueDict.TryGetValue("item_list", out string itemListStr))
                obj.item_list = NPCommonCostItem.readList(itemListStr);

            if (_valueDict.TryGetValue("cost_list", out string costListStr))
                obj.cost_list = NPCommonCostItem.readList(costListStr);

            if (_valueDict.TryGetValue("profit_per", out string profitPerStr))
                obj.profit_per = ALCommon.GetLong(profitPerStr);

            // 如有更多字段，按需补充
            return true;
        }
    }
}