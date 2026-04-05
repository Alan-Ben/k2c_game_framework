using System.Collections.Generic;
using GOE;
using Hotfix.NumMergeEnum;

namespace Hotfix
{
    /// <summary>
    /// 2048 游戏模式表
    /// </summary>
    public class NumMergeModeRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return (long)type; } }

        public ENumMerge_ModeType type; // 游戏模式类型
        public int consume_cd; // 每动一步所需的体力
        public int unlock_need_total_score; // 解锁需要消耗体力数量
        public _NPPlayerConditionSerializeInfo unlock_condition; // 解锁条件
        public string unlock_condition_desc;//解锁条件描述
        public List<string> unlock_condition_desc_args;//解锁条件描述参数
        public int gen_block_level; // 初始生成棋子等级
        
        
        public bool isModeUnlocked(bool _showTip = false)
        {
            if (unlock_condition != null && !unlock_condition.IsEnable(null))
            {
                if (_showTip)
                {
                    if (unlock_condition_desc_args == null || unlock_condition_desc_args.Count == 0)
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(unlock_condition_desc));
                    else
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(unlock_condition_desc, unlock_condition_desc_args));
                }

                return false;
            }

            long totalScore = HotfixNPPlayer.instance.numMergeComponent.totalScore;
            if (totalScore < unlock_need_total_score)
            {
                if (_showTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_modeLockByTotalScore_count, unlock_need_total_score));
                return false;
            }

            return true;
        }
        

        protected override void _parseFromString(string _line)
        {
            type = getEnum<ENumMerge_ModeType>("type", false);
            consume_cd = getInt("consume_cd");
            unlock_need_total_score = getInt("unlock_need_total_score");
            unlock_condition = _NPPlayerConditionSerializeInfo.ReadFromString(getString("unlock_condition"));
            unlock_condition_desc = getString("unlock_condition_desc");
            unlock_condition_desc_args = getList<string>("unlock_condition_desc_args");
            gen_block_level = getInt("gen_block_level");
        }
        

        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "num_merge_mode"; } }
    }
}
