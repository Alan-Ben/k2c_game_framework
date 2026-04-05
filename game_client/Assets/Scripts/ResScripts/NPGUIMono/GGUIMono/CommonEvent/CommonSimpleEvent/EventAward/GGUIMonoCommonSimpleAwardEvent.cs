using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 关卡奖励事件页面
    /// </summary>
    public class GGUIMonoCommonSimpleAwardEvent : _AGGUIMonoCommonSimpleEvent
    {
        [ALHeader("事件图片")]
        public RawImage monoEventImg;
        
        [ALHeader("奖励ContainerMono")]
        public NPGGUIMonoCommonRewardContainer monoReward;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
    }
}