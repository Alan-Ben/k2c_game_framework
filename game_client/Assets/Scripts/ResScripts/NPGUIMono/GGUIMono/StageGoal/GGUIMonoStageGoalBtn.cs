using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoStageGoalBtn : _AALBasicUIWndMono
    {
        [ALHeader("进入按钮")]
        public GameObject btnEnter;
        [ALHeader("阶段图和名字")]
        public RawImage imgStepIcon;
        public Text txtStepName;
        [ALHeader("阶段的进度")]
        public Slider sldProgress;
        public Text txtProgress;
    }
}