using System.Collections.Generic;
using GOE;
using NPEnum;

namespace Hotfix
{
    /// <summary>
    /// 品质概率信息
    /// </summary>
    public struct QualityProbabilityInfo
    {
        public EQuality quality; // 品质
        public int probability; // 总概率（万分比）
        public List<NPCommonCostItem> itemList; // 该品质下的物品列表
        public List<int> probabilityList; // 对应的物品概率列表

        public QualityProbabilityInfo(EQuality _quality, int _probability, List<NPCommonCostItem> _itemList, List<int> _probabilityList)
        {
            quality = _quality;
            probability = _probability;
            itemList = _itemList;
            probabilityList = _probabilityList;
        }

        public override string ToString()
        {
            return $"Quality: {quality}, Probability: {probability}, ItemCount: {itemList.ToStringList()}, ProbabilityCount: {probabilityList.ToStringList()}";
        }
    }

    /// <summary>
    /// 2048 游戏宝箱表
    /// </summary>
    /// <remarks>
    /// 根据积分阶段获取宝箱奖励
    /// </remarks>
    public class NumMergeBoxRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return step; } }

        public int step; // 宝箱阶段
        public long upgrade_need_score; // 升到下一阶段所需的积分
        public long reward_id; // 奖励id
        public NPGGoIndex box_go_index;

        public NPSORewardRefObj reward_ref;


        protected override void _parseFromString(string _line)
        {
            step = getInt("step");
            upgrade_need_score = getLong("upgrade_need_score");
            reward_id = getLong("reward_id");
            box_go_index = new NPGGoIndex();
            box_go_index.readIndex(getString("box_go_index"));
        }


        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "num_merge_box"; } }


        /// <summary>
        /// 获取品质概率汇总列表
        /// </summary>
        /// <returns>按品质汇总的概率信息列表（从低到高排序）</returns>
        public List<QualityProbabilityInfo> getQualityProbabilityList()
        {
            List<QualityProbabilityInfo> result = new List<QualityProbabilityInfo>();

            if (reward_ref == null || reward_ref.show_item_list == null || reward_ref.show_pro_list == null)
                return result;

            // 用字典来汇总各品质的数据
            Dictionary<EQuality, int> qualityProbDict = new Dictionary<EQuality, int>();
            Dictionary<EQuality, List<NPCommonCostItem>> qualityItemDict = new Dictionary<EQuality, List<NPCommonCostItem>>();
            Dictionary<EQuality, List<int>> qualityProbListDict = new Dictionary<EQuality, List<int>>();

            // 遍历所有物品
            for (int i = 0; i < reward_ref.show_item_list.Count && i < reward_ref.show_pro_list.Count; i++)
            {
                NPCommonCostItem item = reward_ref.show_item_list[i];
                int probability = reward_ref.show_pro_list[i];

                // 获取物品品质
                EQuality quality = GCommon.getItemQuality(item.getItemType(), item.subId);
                
                // 初始化字典
                if (!qualityProbDict.ContainsKey(quality))
                {
                    qualityProbDict[quality] = 0;
                    qualityItemDict[quality] = new List<NPCommonCostItem>();
                    qualityProbListDict[quality] = new List<int>();
                }

                // 累加该品质的总概率
                qualityProbDict[quality] += probability;
                // 添加物品到列表
                qualityItemDict[quality].Add(item);
                // 添加概率到列表
                qualityProbListDict[quality].Add(probability);
            }

            // 转换为列表并按品质从低到高排序
            foreach (KeyValuePair<EQuality, int> pair in qualityProbDict)
            {
                result.Add(new QualityProbabilityInfo(
                    pair.Key,
                    pair.Value,
                    qualityItemDict[pair.Key],
                    qualityProbListDict[pair.Key]));
            }

            result.Sort((a, b) => b.quality.CompareTo(a.quality));
            
            return result;
        }
    }
}