using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class CommonRankRewardPatchDealer : _ACommonListHotRefPatchDealer
    {
        //表名
        public override string getTableName
        {
            get { return GSOActivityRankRewardRefSet.objName; }
        }
        
        //补丁数据
        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (null == _valueDict)
                return false;
            
            GActivityRankRewardRefObj activityRankRewardRefObj = GRefdataCoreMgr.instance.activityRankRewardRefCore.getRef(_refId);
            if (null == activityRankRewardRefObj)
                return false;

            if (_valueDict.TryGetValue("rank_id", out string rankIdStr))
                activityRankRewardRefObj.rank_id = ALCommon.GetLong(rankIdStr);
            
            if (_valueDict.TryGetValue("rank_begin", out string rankBeginStr))
                activityRankRewardRefObj.rank_begin = ALCommon.GetInt(rankBeginStr);
            
            if (_valueDict.TryGetValue("rank_end", out string rankEndStr))
                activityRankRewardRefObj.rank_end = ALCommon.GetInt(rankEndStr);
            
            if (_valueDict.TryGetValue("title_reward", out string titleRewardStr))
                activityRankRewardRefObj.title_reward = ActivityTitleReward.readFromStr(titleRewardStr);
            
            if (_valueDict.TryGetValue("reward_item_list", out string rewardItemListStr))
                activityRankRewardRefObj.reward_item_list = NPCommonCostItem.readList(rewardItemListStr);
            
            if (_valueDict.TryGetValue("member_reward_item_list", out string memberRewardItemListStr))
                activityRankRewardRefObj.member_reward_item_list = NPCommonCostItem.readList(memberRewardItemListStr);
            
            return true;
        }
    }
}