using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class ActivityFundStepPatchDealer : _ACommonListHotRefPatchDealer
    {
        public override string getTableName
        {
            get { return GSOActivityFundStepRefSet.objName; }
        }

        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;

            ActivityFundStepRefObj obj = GRefdataCoreMgr.instance.activityFundStepRefCore.getRef(_refId);
            if (obj == null)
                return false;
            
            if (_valueDict.TryGetValue("step", out string setIdStr))
                obj.step = ALCommon.GetLong(setIdStr);
            
            if (_valueDict.TryGetValue("need_count", out string need_count))
                obj.need_count = ALCommon.GetLong(need_count);
            
            if (_valueDict.TryGetValue("free_reward_item_list", out string free_reward_item_list))
                obj.free_reward_item_list = NPCommonCostItem.readList(free_reward_item_list);
            
            if (_valueDict.TryGetValue("pay_reward_item_list", out string pay_reward_item_list))
                obj.pay_reward_item_list = NPCommonCostItem.readList(pay_reward_item_list);
            
            if (_valueDict.TryGetValue("is_special_step", out string is_special_step))
                obj.is_special_step = ALCommon.GetBool(is_special_step);

            return true;
        }
    }
}