using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 科研所普通状态操作按钮
    /// </summary>
    public class GGUIMonoMarsBuildingTechNormalOpBtns : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("详情按钮")]
        public GameObject btnDetail;

        [ALHeader("升级建筑按钮")]
        public GameObject btnUpgradeBuilding;
        
        [ALHeader("有科技在升级中时显示物体列表")]
        public List<GameObject> hasTechOnUpgradingShowGoList;
        [ALHeader("有科技升级完成时显示物体列表")]
        public List<GameObject> hasTechOnUpgradedShowGoList;
        [ALHeader("没有科技在升级中隐藏物体列表")]
        public List<GameObject> noTechOnUpgradingHideGoList;
        [ALHeader("科技升级加速按钮")]
        public GameObject btnTechUpgradeSpeedUp;
        [ALHeader("科技升级倒计时")]
        public TextEx txtTechUpgradingCountDown;

        [ALHeader("研究按钮")]
        public GameObject btnResearch;
    }
}
