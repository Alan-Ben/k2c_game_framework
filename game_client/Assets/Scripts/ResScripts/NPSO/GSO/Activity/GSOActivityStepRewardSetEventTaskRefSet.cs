using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    [System.Serializable]
    public class ActivityStepRewardSetEventTaskRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一id

        public long step_reward_set_id;//阶段奖励设定id
        public string task_name;//触发任务名称
        public List<string> task_name_args;//任务名称参数
        public int trigger_gain_points;//触发时获取点数(客户端展示)
        public NPGTextureIndex icon;//图标
        public _NPPlayerEffectSerializeInfo go_to;//前往跳转效果
		public EValueFormatType process_num_format;//分数显示逻辑，客户端用(EValueFormatType)
        public long process_limit;//最大值（不填表示无限制, 即<=0时）
        public long center_tip_id;//完成任务时提示id（center_tips表id）
    }
    
    public class GSOActivityStepRewardSetEventTaskRefSet : _TALSOBasicRefSet<ActivityStepRewardSetEventTaskRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
        public static string objName { get { return "step_reward_set_event_task"; } }
    }
}