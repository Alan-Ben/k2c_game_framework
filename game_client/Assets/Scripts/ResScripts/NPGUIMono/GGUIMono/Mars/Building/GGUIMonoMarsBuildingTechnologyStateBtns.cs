using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 科研所状态常驻hud按钮
    /// </summary>
    public class GGUIMonoMarsBuildingTechnologyStateBtns : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("研究按钮")]
        public GameObject btnResearch;
        
        [ALHeader("升级中的科技信息")]
        public GGUIMonoMarsUpgradingTechnologyInfo monoUpgradingTechnologyInfo;
        
        [ALHeader("科技图标")]
        public List<RawImage> monoIconList;
    }
}
