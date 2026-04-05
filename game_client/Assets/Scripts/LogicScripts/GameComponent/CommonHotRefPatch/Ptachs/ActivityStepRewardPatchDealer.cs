using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class ActivityStepRewardPatchDealer : _ACommonListHotRefPatchDealer
    {
        public override string getTableName
        {
            get { return GSOActivityStepRewardRefSet.objName; }
        }

        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;

            GActivityStepRewardRefObj obj = GRefdataCoreMgr.instance.activityStepRewardRefCore.getRef(_refId);
            if (obj == null)
                return false;

            if (_valueDict.TryGetValue("step_reward_set_id", out string setIdStr))
                obj.step_reward_set_id = ALCommon.GetLong(setIdStr);

            if (_valueDict.TryGetValue("step", out string stepStr))
                obj.step = ALCommon.GetInt(stepStr);

            if (_valueDict.TryGetValue("complete_count", out string completeCountStr))
                obj.complete_count = ALCommon.GetInt(completeCountStr);

            if (_valueDict.TryGetValue("reward_item_list", out string rewardListStr))
                obj.reward_item_list = NPCommonCostItem.readList(rewardListStr);

            return true;
        }
    }
}