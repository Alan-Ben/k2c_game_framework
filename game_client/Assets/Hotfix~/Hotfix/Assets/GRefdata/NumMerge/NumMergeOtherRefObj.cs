using ALPackage;

namespace Hotfix
{
    /// <summary>
    /// 2048 游戏的 general 配置表
    /// </summary>
    public class NumMergeOtherRefObj : _AHotfixGeneralRefCore
    {
        public int num_merge_start_gen_block_num;
        public int num_merge_each_gen_block_num;
        public NPCommonItem num_merge_ticket_item;
        public NPCommonItem num_merge_organize_item;
        public NPCommonItem num_merge_eliminate_item;
        public int num_merge_first_buff_stamina_cost;
        public int num_merge_buff_stamina_cost;
        public int num_merge_max_buff_count;
        public WCGIntRange num_merge_buff_duration_round;
        public long num_merge_lazy_cd_id;
        public int num_merge_lazy_cd_red_tip_show_per;
        public long num_merge_buff_bonus_rate;
        public long highest_score_rank_id;
        public long total_score_rank_id;


        public NumMergeOtherRefObj(_AALResourceCore _resCore) : base(_resCore, assetPath, objName)
        {
        }

        protected override void _parseFromString()
        {
            num_merge_start_gen_block_num = getInt("num_merge_start_gen_block_num");
            num_merge_each_gen_block_num = getInt("num_merge_each_gen_block_num");
            num_merge_ticket_item = NPCommonItem.readFromStr(getString("num_merge_ticket_item"));
            num_merge_organize_item = NPCommonItem.readFromStr(getString("num_merge_organize_item"));
            num_merge_eliminate_item = NPCommonItem.readFromStr(getString("num_merge_eliminate_item"));
            num_merge_first_buff_stamina_cost = getInt("num_merge_first_buff_stamina_cost");
            num_merge_buff_stamina_cost = getInt("num_merge_buff_stamina_cost");
            num_merge_max_buff_count = getInt("num_merge_max_buff_count");
            num_merge_buff_duration_round = WCGIntRange.readFromStr(getString("num_merge_buff_duration_round"));
            num_merge_lazy_cd_id = getLong("num_merge_lazy_cd_id");
            num_merge_lazy_cd_red_tip_show_per = getInt("num_merge_lazy_cd_red_tip_show_per");
            num_merge_buff_bonus_rate = getLong("num_merge_buff_bonus_rate");
            highest_score_rank_id = getLong("highest_score_rank_id");
            total_score_rank_id = getLong("total_score_rank_id");
        }

        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "num_merge_other"; } }
    }
}
