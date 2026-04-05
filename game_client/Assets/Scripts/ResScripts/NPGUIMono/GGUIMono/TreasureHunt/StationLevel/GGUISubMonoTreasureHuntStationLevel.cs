using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 太空舱等级子窗口
    /// </summary>
    public class GGUISubMonoTreasureHuntStationLevel : _AALBasicUIWndMono
    {
        [ALHeader("当前等级")]
        public TextEx txtCurLevel;
        [ALHeader("当前等级进度条")]
        public NPGGUIMonoProgress monoLevelProgress;

        [ALHeader("助跑距离")]
        public Text txtAutoDistance; // 助跑距离：{0}米
        [ALHeader("最大移动距离")]
        public Text txtMaxDistance; // 最远距离：{0}米
        [ALHeader("保护时间")]
        public Text txtProtectTime; // 保护次数：{0}
        [ALHeader("奖励数量")]
        public Text txtRewardItem; // 奖励数量：{0}

        [ALHeader("最高等级时显示")]
        public List<GameObject> maxLevelShow;
        [ALHeader("最高等级时隐藏")]
        public List<GameObject> maxLevelHide;
        
        [ALHeader("等级详情按钮")]
        public GameObject btnDetail;
    }
}