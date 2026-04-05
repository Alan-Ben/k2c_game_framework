using System;
using System.Collections.Generic;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 游戏模式
    /// </summary>
    public class TileMatchModeRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return (long) type; } }

        public TileMatchEnum.ETileMatch_ModeType type;//类型
        public _NPPlayerConditionSerializeInfo unlock_condition;//解锁条件
        public string unlock_condition_desc;//解锁条件描述
        public List<long> unlock_condition_desc_args;//解锁条件描述参数
        public int multiple;//分数翻倍倍数
        public long unlock_need_score;//解锁所需分数
        
        protected override void _parseFromString(string _line)
        {
            type = getEnum<TileMatchEnum.ETileMatch_ModeType>("type", false);
            unlock_condition = _NPPlayerConditionSerializeInfo.ReadFromString(getString("unlock_condition"));
            unlock_condition_desc = getString("unlock_condition_desc");
            unlock_condition_desc_args = getList<long>("unlock_condition_desc_args");
            multiple = getInt("multiple");
            unlock_need_score = getLong("unlock_need_score");
        }
        
        /// <summary>
        /// 游戏模式是否已解锁
        /// </summary>
        /// <returns></returns>
        public bool gameModeIsUnlock()
        {
            return (unlock_condition == null || !unlock_condition.hasCondition || unlock_condition.IsEnable(null)) 
                && unlock_need_score <= HotfixNPPlayer.instance.tileMatchComponent.activityTotalScore;
        }
        
        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "tilematch_mode"; } }
    }
}