namespace Hotfix
{
    public class TileMatchStepRewardRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return step; } }

        public int step;// 阶段
        public long goal;// 目标值
        public long jackpot_group_id;//奖池组id
        public NPCommonAssetPathInfo box_prefab_asset_path;//宝箱预制体资源路径
        
        protected override void _parseFromString(string _line)
        {
            step = getInt("step");
            goal = getLong("goal");
            jackpot_group_id = getLong("jackpot_group_id");
            box_prefab_asset_path = NPCommonAssetPathInfo.readFromStr(getString("box_prefab_asset_path"));
        }
        
        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "tilematch_step_reward"; } }
    }
}