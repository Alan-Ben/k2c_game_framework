using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class ActivityRankRewardPatchDealer : _ACommonListHotRefPatchDealer
    {
        public override string getTableName
        {
            get { return GSOActivityRankRewardRefSet.objName; }
        }

        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;

            GActivityRankRewardRefObj obj = GRefdataCoreMgr.instance.activityRankRewardRefCore.getRef(_refId);
            if (obj == null)
                return false;

            if (_valueDict.TryGetValue("member_reward_item_list", out string member_reward_item_list))
                obj.member_reward_item_list = NPCommonCostItem.readList(member_reward_item_list);

            return true;
        }
    }
}