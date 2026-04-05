using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingConstructing : _AALBasicUIWndMono
    {
        [ALHeader("建筑聚焦的设置")]
        public Vector2 focusViewportPos = new Vector2(0.5f, 0.7f);
        public float focusScale = 1.2f;
        public float focusTime = 0.5f;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        public GameObject btnCloseAdditional;
        [ALHeader("建筑名称")]
        public Text txtName;
        [ALHeader("建筑描述")]
        public Text txtDesc;
        [ALHeader("建造时间")]
        public Text txtBuildTime;
        [ALHeader("建造进度")]
        public Slider sldBuildProgress;
        [ALHeader("取消按钮")]
        public GameObject btnCancel;
        [ALHeader("加速按钮")]
        public GameObject btnSpeedUp;
        [ALHeader("立即完成")]
        public GameObject btnCompleteNow;
        [ALHeader("立即完成的消耗")]
        public NPGGUIMonoCommonItem monoCompleteNowCostItem;
        [ALHeader("求助按钮")]
        public GameObject btnHelp;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7111); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7111); } }
    }
}