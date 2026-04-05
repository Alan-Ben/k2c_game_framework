using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 兵工厂操作按钮
    /// </summary>
    public class GGUIMonoMarsBuildingArmoryOperationBtns : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
        [ALHeader("详情窗口的资源id")]
        public long detailWndUIAssetPathId;

        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级窗口资源id")]
        public long upgradeWndAssetPathId;
        
        [ALHeader("在升级中显示物体列表")]
        public List<GameObject> onUpgradingShowGoList;
        [ALHeader("在升级中隐藏物体列表")]
        public List<GameObject> onUpgradingHideGoList;
        [ALHeader("加速按钮")]
        public GameObject btnSpeedUp;
        [ALHeader("升级倒计时")]
        public TextEx txtUpgradingCountDown;
    }
}
