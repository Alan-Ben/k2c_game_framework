using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class CrystalGiftPackPatchDealer : _ACommonListHotRefPatchDealer
    {
        // 表名
        public override string getTableName
        {
            get { return GSOCrystalGiftPackRefSet.objName; }
        }

        // 补丁数据应用
        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;

            CrystalGiftPackRefObj obj = GRefdataCoreMgr.instance.crystalGiftPackRefCore.getRef(_refId);
            if (obj == null)
                return false;
            
            if (_valueDict.TryGetValue("crystal_gift_pack_group_id", out string groupIdStr))
                obj.crystal_gift_pack_group_id = ALCommon.GetLong(groupIdStr);

            if (_valueDict.TryGetValue("sort_id", out string sortIdStr))
                obj.sort_id = ALCommon.GetInt(sortIdStr);

            if (_valueDict.TryGetValue("item_list", out string itemListStr))
                obj.item_list = NPCommonCostItem.readList(itemListStr);

            if (_valueDict.TryGetValue("discount", out string discountStr))
                obj.discount = ALCommon.GetInt(discountStr);

            if (_valueDict.TryGetValue("buy_num", out string buyNumStr))
                obj.buy_num = ALCommon.GetInt(buyNumStr);

            if (_valueDict.TryGetValue("cost_item", out string costItemStr))
                obj.cost_item = NPCommonCostItem.readFromStr(costItemStr);

            // 如有更多字段，按需补充
            return true;
        }
    }
}