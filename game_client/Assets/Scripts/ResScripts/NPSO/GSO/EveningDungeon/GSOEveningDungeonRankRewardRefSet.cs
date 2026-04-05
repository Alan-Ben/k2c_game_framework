using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 晚间副本排行榜奖励表
    /// </summary>
    [Serializable]
    public class EveningDungeonRankRewardRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        
        public long id;//唯一id
        public int rank_begin;//开始名次
        public int rank_end;//结束名次
        public List<NPCommonCostItem> reward_item_list;//奖励
    }
    
    public class GSOEveningDungeonRankRewardRefSet : _TALSOBasicRefSet<EveningDungeonRankRewardRefObj> 
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/evening_dungeon_refdata.unity3d"; } }
        public static string objName { get { return "evening_dungeon_rank_reward"; } }
    }
}