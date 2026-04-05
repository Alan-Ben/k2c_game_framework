using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 阶段目标大阶段表
    /// </summary>
    [Serializable]
    public class StageGoalBigStepRefObj : _IALBasicRefObj, IComparable<StageGoalBigStepRefObj>
    {
        public long _refId { get { return big_step; } }

        public int big_step; // 大阶段序号
        public int begins_from_small_step; // 从哪个小阶段开始（包含）
        public NPGTextureIndex icon;//图标
        public NPGTextureIndex peak_banner;//时代之巅大阶段banner图
        public long dialogue_id; // 大阶段完成的对话 id
        public string title;//标题
        public List<string> title_args;//标题参数
        public string desc;//描述
        public List<string> desc_args;//描述参数
        public List<NPCommonCostItem> first_reach_reward_item_list;//大阶段首达奖励列表
        public List<NPCommonCostItem> reward_item_list;//奖励列表
        public List<NPCommonCostItem> func_unlock_item_list;//任务完成后解锁的功能图片
        public NPGGoIndex building_index;//对应的建筑资源
        public long build_sfx_id;//建筑建造时的特效id
        public float build_sfx_delay;//特效播放多久后加载建造完成后的资源
        public bool done_need_draw_all_step_reward;//是否需要领取所有小阶段奖励才可以领奖

        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public int end_small_step; // 结束的小阶段序号（包含）
        
        
        public string getTitle { get => TextTranslate.instance.getLanguage(title, title_args); }
        public string getDesc { get => TextTranslate.instance.getLanguage(desc, desc_args); }


        public int CompareTo(StageGoalBigStepRefObj other)
        {
            if (other == null)
                return 1;
            
            return begins_from_small_step.CompareTo(other.begins_from_small_step);
        }
    }
    public class GSOStageGoalBigStepRefSet : _TALSOBasicRefSet<StageGoalBigStepRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/stage_refdata.unity3d"; } }
        public static string objName { get { return "stage_goal_big_step"; } }
    }
}