using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class CommonStepRewardPatchDealer : _ACommonListHotRefPatchDealer
    {
        // 表名
        public override string getTableName
        {
            get { return GSOActivityStepRewardRefSet.objName; }
        }

        // 补丁数据应用
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

            if (_valueDict.TryGetValue("name", out string nameStr))
                obj.name = nameStr;

            if (_valueDict.TryGetValue("icon", out string iconStr))
                obj.icon = NPGTextureIndex.readIndexInfo(iconStr);

            if (_valueDict.TryGetValue("complete_count", out string countStr))
                obj.complete_count = ALCommon.GetInt(countStr);

            if (_valueDict.TryGetValue("reward_item_list", out string rewardListStr))
                obj.reward_item_list = NPCommonCostItem.readList(rewardListStr);

            return true;
        }
    }
}