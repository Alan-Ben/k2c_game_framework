using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 科研所升级中状态操作按钮
    /// </summary>
    public class GGUIMonoMarsBuildingTechUpgradingOpBtns : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("详情按钮")]
        public GameObject btnDetail;
        
        [ALHeader("加速按钮")]
        public GameObject btnSpeedUp;

        [ALHeader("研究按钮")]
        public GameObject btnResearch;
    }
}
