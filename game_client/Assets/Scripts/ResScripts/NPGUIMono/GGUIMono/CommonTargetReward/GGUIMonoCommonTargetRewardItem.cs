using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoCommonTargetRewardItem : MonoBehaviour
    {
        [ALHeader("对应ID(common_target_reward)")]
        public long id;
        
        [ALHeader("对应加载形象展示的资源id（ui_res_path）")]
        public long uiResPathId;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        
        [ALHeader("选中状态展示的go列表")]
        public List<GameObject> selectShowGoList;
        [ALHeader("不同状态展示的go列表")]
        public List<NPCommonEnumStatInfo<ECommonRewardType>> rewardStatList;
    }
}