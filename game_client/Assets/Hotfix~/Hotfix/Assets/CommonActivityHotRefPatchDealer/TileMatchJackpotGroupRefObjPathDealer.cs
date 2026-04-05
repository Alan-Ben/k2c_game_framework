using System.Collections.Generic;
using System.Linq;
using ALPackage;
using GOE;

namespace Hotfix
{
    public class TileMatchJackpotGroupRefObjPathDealer : _ACommonListHotRefPatchDealer
    {
        // 表名
        public override string getTableName
        {
            get { return TileMatchJackpotGroupRefObj.objName; }
        }
        
        protected override bool _applyPatch(long _refId, Dictionary<string, string> _valueDict)
        {
            if (_valueDict == null)
                return false;
            
            TileMatchJackpotGroupRefObj obj = HotfixRefdataCoreMgr.instance.tileMatchJackpotGroupRefCore.getRef(_refId);
            if (obj == null)
                return false;
            
            if (_valueDict.TryGetValue("group_id", out string groupIdStr))
                obj.group_id = long.Parse(groupIdStr);

            if (_valueDict.TryGetValue("reward_quality_name", out string qualityNameStr))
                obj.reward_quality_name = qualityNameStr;

            if (_valueDict.TryGetValue("reward_item_list", out string rewardItemListStr))
                obj.reward_item_list = NPCommonCostItem.readList(rewardItemListStr);

            if (_valueDict.TryGetValue("item_wei_list", out string itemWeiListStr))
                obj.item_wei_list = ALCommon.ParseIntList(itemWeiListStr);

            // 重新处理奖励列表
            obj._dealRewardList();

            return true;
        }
    }
}