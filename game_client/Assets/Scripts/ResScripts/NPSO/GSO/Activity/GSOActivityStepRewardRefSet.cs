using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励数据
    /// </summary>
    [System.Serializable]
    public class GActivityStepRewardRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id;//唯一id
        public long step_reward_set_id;//阶段奖励设定id
        public int step;//步骤id
        public string name;//阶段名称
        public NPGTextureIndex icon;//图片
        public int complete_count;//目标计数
        public List<NPCommonCostItem> reward_item_list;//奖励
    }

    public class GSOActivityStepRewardRefSet : _TALSOBasicRefSet<GActivityStepRewardRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/activity_refdata.unity3d"; } }
        public static string objName { get { return "activity_step_reward"; } }
    }
}

