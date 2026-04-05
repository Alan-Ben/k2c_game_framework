using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class RewardRefPatchDealer : _ACommonListHotRefPatchDealer
    {
        // 表名
        public override string getTableName
        {
            get { return NPSORewardRefSet.objName; }
        }

        // 补丁数据应用
        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;

            NPSORewardRefObj obj = GRefdataCoreMgr.instance.rewardMap.getRef(_refId);
            if (obj == null)
                return false;

            if (_valueDict.TryGetValue("reward_id", out string rewardIdStr))
                obj.reward_id = ALCommon.GetLong(rewardIdStr);

            if (_valueDict.TryGetValue("show_item_list", out string showItemListStr))
                obj.show_item_list = NPCommonCostItem.readList(showItemListStr);

            if (_valueDict.TryGetValue("show_pro_list", out string showProListStr))
                obj.show_pro_list = ALCommon.ParseIntList(showProListStr);

            if (_valueDict.TryGetValue("show_item_list_expand", out string expandStr))
                obj.show_item_list_expand = ALCommon.GetBool(expandStr);

            return true;
        }
    }
}