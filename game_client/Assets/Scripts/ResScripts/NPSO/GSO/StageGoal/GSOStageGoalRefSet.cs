using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 阶段目标数据
    /// </summary>
    [Serializable]
    public class StageGoalRefObj : _IALBasicRefObj
    {
        public long _refId { get { return step; } }
        public long step;//唯一识别ID
        public int next_step_need_server_start_day; //进入下一阶段要求的服务器天数
        public long next_step_simple_unlock_id;//进入下一阶段解锁条件id
        public List<long> task_list;
        public NPGTextureIndex icon;//图标
        public string title;//标题
        public List<string> title_args;//标题参数
        public long dialogue_id;//任务结束时的对话 id
        public List<NPCommonCostItem> show_item_list;//任务的关键物品图片展示
        public List<NPCommonCostItem> reward_item_list;//奖励列表
        public long task_ui_res_id;//小阶段的任务面板 ui 资源 ID
        public NPGTextureIndex task_bg; //任务的背景图

        public string getTitle { get => TextTranslate.instance.getLanguage(title, title_args); }
    }

    public class GSOStageGoalRefSet : _TALSOBasicRefSet<StageGoalRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/stage_refdata.unity3d"; } }
        public static string objName { get { return "stage_goal"; } }
    }
}