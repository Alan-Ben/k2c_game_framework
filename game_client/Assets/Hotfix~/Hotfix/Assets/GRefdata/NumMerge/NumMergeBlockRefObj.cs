using System.Collections.Generic;

namespace Hotfix
{
    /// <summary>
    /// 2048 游戏棋子表
    /// </summary>
    /// <remarks>
    /// 同 level 相碰合成下一 level
    /// </remarks>
    public class NumMergeBlockRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return level; } }

        public int level;
        public string name;
        public string desc;
        public NPGTextureIndex icon;
        public long merge_gain_score;
        public long delete_gain_score;
        public long merge_gain_ticket;
        public long buff_gen_weight;
        public string merge_anim_name;
        public long merge_sfx_id;
        public string buff_merge_anim_name;
        public long buff_merge_sfx_id;
        public string merge_tip_key;
        public List<string> merge_tip_params;
        public long merge_tip_id;
        

        protected override void _parseFromString(string _line)
        {
            level = getInt("level");
            name = getString("name");
            desc = getString("desc");
            icon = NPGTextureIndex.readIndexInfo(getString("icon"));
            merge_gain_score = getLong("merge_gain_score");
            delete_gain_score = getLong("delete_gain_score");
            merge_gain_ticket = getLong("merge_gain_ticket");
            buff_gen_weight = getLong("buff_gen_weight");
            merge_anim_name = getString("merge_anim_name");
            merge_sfx_id = getLong("merge_sfx_id");
            buff_merge_anim_name = getString("buff_merge_anim_name");
            buff_merge_sfx_id = getLong("buff_merge_sfx_id");
            merge_tip_key = getString("merge_tip_key");
            merge_tip_params = getList<string>("merge_tip_params");
            merge_tip_id = getLong("merge_tip_id");
        }


        public bool canMergeWith(NumMergeBlockRefObj _refObj)
        {
            if (_refObj == null)
                return false;
            if (HotfixRefdataCoreMgr.instance.isNumMergeBlockLevelMax(level))
                return false;
            if (HotfixRefdataCoreMgr.instance.isNumMergeBlockLevelMax(_refObj.level))
                return false;

            return level == _refObj.level;
        }
        

        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "num_merge_block"; } }
    }
}
