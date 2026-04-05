using System;
using System.Collections.Generic;

namespace Hotfix
{
    public class TileMatchStepRewardItemInfo
    {
        public NPCommonCostItem rewardItem;//奖励物品
        public long weight;//抽取权重
        
        public TileMatchStepRewardItemInfo(NPCommonCostItem _rewardItem, long _weight)
        {
            rewardItem = _rewardItem;
            weight = _weight;
        }
    }
    
    public class TileMatchJackpotGroupRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return id; } }

        public long id;//唯一id
        public long group_id;//奖池组id
        public string reward_quality_name;//奖励品质名称
        
        public List<NPCommonCostItem> reward_item_list;//奖励物品列表原始数据，用于编辑器显示
        public List<int> item_wei_list;//奖励物品权重列表原始数据，用于编辑器显示
        
        public List<TileMatchStepRewardItemInfo> reward_list;//奖励列表
        public int weight_total;//总权重
        
        protected override void _parseFromString(string _line)
        {
            id = getLong("id");
            group_id = getLong("group_id");
            reward_quality_name = getString("reward_quality_name");
            reward_item_list = NPCommonCostItem.readList(getString("reward_item_list"));
            item_wei_list = getList<int>("item_wei_list");

            _dealRewardList();
        }

        //2次处理奖励数据
        public void _dealRewardList()
        {
            int rewardItemCount = reward_item_list?.Count ?? 0;
            int itemWeightCount = item_wei_list?.Count ?? 0;
            if (rewardItemCount != itemWeightCount)
            {
                Debug.LogError($"[TileMatchJackpotGroupRefObj _parseFromString] 三消tilematch_jackpot_group表, id为{id}的行数据填写错误, reward_item_list的长度:{rewardItemCount}和item_wei_list:{itemWeightCount}的长度不一致");
            }
            
            reward_list = new List<TileMatchStepRewardItemInfo>();
            weight_total = 0;
            if (reward_item_list != null && item_wei_list != null)
            {
                // 需要遍历的数量
                int traversalCount = Math.Min(rewardItemCount, itemWeightCount);

                for (int i = 0; i < traversalCount; i++)
                {
                    NPCommonCostItem rewardItem = reward_item_list[i];
                    int itemWeight = item_wei_list[i];
                    
                    reward_list.Add(new TileMatchStepRewardItemInfo(rewardItem, itemWeight));
                    weight_total += itemWeight;
                }
            }
        }
        
        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "tilematch_jackpot_group"; } }
    }
}