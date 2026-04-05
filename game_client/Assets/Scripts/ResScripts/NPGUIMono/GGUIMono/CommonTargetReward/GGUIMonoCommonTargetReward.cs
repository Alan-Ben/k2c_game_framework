using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 国力目标窗口
    /// </summary>
    public class GGUIMonoCommonTargetReward : _AALBasicUIWndMono
    {
        [ALHeader("目标列表")]
        public List<GGUIMonoCommonTargetRewardItem> itemList;
        [ALHeader("目标列表的ScrollRect")]
        public ScrollRect targetRewardScrollRect;
        
        [ALHeader("不同状态展示的go列表")]
        public List<NPCommonEnumStatInfo<ECommonRewardType>> rewardStatList;
        
        [ALHeader("形象加载父节点")]
        public Transform modelParent;
        
        [ALHeader("对应文本描述")]
        public TextEx txtDesc;
        [ALHeader("进度达到目标值时描述的颜色")]
        public Color progressReachColor = Color.green;
        [ALHeader("进度未达到目标值时描述的颜色")]
        public Color progressUnReachColor = Color.red;
        
        [ALHeader("领取按钮")]
        public GameObject btnGet;
        [ALHeader("前往按钮")]
        public GameObject btnGo;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
    }
}