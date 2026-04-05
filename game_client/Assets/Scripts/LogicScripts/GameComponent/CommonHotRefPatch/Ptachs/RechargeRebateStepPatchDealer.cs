using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class RechargeRebateStepPatchDealer : _ACommonListHotRefPatchDealer
    {
        public override string getTableName
        {
            get { return GSORechargeRebateStepRefSet.objName; }
        }

        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;

            RechargeRebateStepRefObj obj = GRefdataCoreMgr.instance.rechargeRebateStepRefCore.getRef(_refId);
            if (obj == null)
                return false;
            
            if (_valueDict.TryGetValue("group_id", out string groupIdStr))
                obj.group_id = ALCommon.GetLong(groupIdStr);

            if (_valueDict.TryGetValue("step", out string stepStr))
                obj.step = ALCommon.GetLong(stepStr);

            if (_valueDict.TryGetValue("target_count", out string targetCountStr))
                obj.target_count = ALCommon.GetLong(targetCountStr);

            if (_valueDict.TryGetValue("reward_list", out string rewardListStr))
                obj.reward_list = NPCommonCostItem.readList(rewardListStr);

            return true;
        }
    }
}