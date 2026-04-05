using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 公会协作奖励据点
    /// </summary>
    public class GGUIMonoGuildCooperatePosPrefab : _AALBasicUIWndMono
    {
        [ALHeader("点击详情按钮")]
        public GameObject btnClickDetail;
        [ALHeader("奖励据点名称")]
        public Text txtName;
        [ALHeader("奖励据点图标")]
        public RawImage imgIcon;

        [ALHeader("属性据点列表")]
        public List<GGUIMonoGuildCooperateAttrPosItem> attrPosItemList;
        [ALHeader("有权限设置推荐据点时显示的列表")]
        public List<GameObject> goCanSetRecommendShowList;
        [ALHeader("推荐开关")]
        public NPGGUIMonoCommonToggleEx monoRecommendToggle;
        [ALHeader("是推荐据点时显示的GO列表")]
        public List<GameObject> goRecommendShowList;

        [ALInfo("下面状态列表只显示一种")]
        [ALHeader("未解锁时显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("已解锁不可领奖时显示的GO列表")]
        public List<GameObject> goUnlockShowList;
        [ALHeader("已解锁可领取奖励时显示的GO列表")]
        public List<GameObject> goCanGetRewardShowList;
        [ALHeader("已解锁已领取奖励时显示的GO列表")]
        public List<GameObject> goAlreadyGetRewardShowList;
    }
}

