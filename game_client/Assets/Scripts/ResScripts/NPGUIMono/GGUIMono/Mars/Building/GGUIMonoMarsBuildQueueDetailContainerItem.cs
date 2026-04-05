using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildQueueDetailContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("建筑图标")]
        public RawImage imgBuildingIcon;
        [ALHeader("建筑名称")]
        public Text txtBuildingName;
        [ALHeader("建造进度条")]
        public Slider sldBuildProgress;
        [ALHeader("建造剩余时间")]
        public Text txtBuildRemainTime;
        [ALHeader("等级变化展示")]
        public CommonUpgradePropertyShow<Text> txtLevelChg;
        [ALHeader("加速按钮")]
        public GameObject btnSpeedUp;
        [ALHeader("建造展示对象列表")]
        public List<GameObject> listConstructionShow;
        public List<GameObject> listUpgradeShow;
    }
}