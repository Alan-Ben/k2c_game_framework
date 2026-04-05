using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class CommonShopItemPatchDealer : _ACommonListHotRefPatchDealer
    {
        // 表名
        public override string getTableName
        {
            get { return GSOActivityShopItemRefSet.objName; }
        }

        // 补丁数据应用
        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;

            ActivityShopItemRefObj obj = GRefdataCoreMgr.instance.activityShopItemRefCore.getRef(_refId);
            if (obj == null)
                return false;

            if (_valueDict.TryGetValue("activity_shop_id", out string shopIdStr))
                obj.activity_shop_id = ALCommon.GetLong(shopIdStr);

            if (_valueDict.TryGetValue("sort_id", out string sortIdStr))
                obj.sort_id = ALCommon.GetLong(sortIdStr);

            if (_valueDict.TryGetValue("item", out string itemStr))
                obj.item = NPCommonCostItem.readFromStr(itemStr);

            if (_valueDict.TryGetValue("discount", out string discountStr))
                obj.discount = ALCommon.GetLong(discountStr);

            if (_valueDict.TryGetValue("buy_num", out string buyNumStr))
                obj.buy_num = ALCommon.GetLong(buyNumStr);

            if (_valueDict.TryGetValue("cost_item", out string costItemStr))
                obj.cost_item = NPCommonCostItem.readFromStr(costItemStr);

            if (_valueDict.TryGetValue("is_recommend", out string isRecommendStr))
                obj.is_recommend = ALCommon.GetBool(isRecommendStr);

            return true;
        }
    }
}