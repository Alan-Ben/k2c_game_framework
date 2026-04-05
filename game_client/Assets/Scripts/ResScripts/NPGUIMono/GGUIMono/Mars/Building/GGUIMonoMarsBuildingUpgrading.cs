using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingUpgrading : _AALBasicUIWndMono
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
        [ALHeader("建筑等级")]
        public Text txtLevel;
        [ALHeader("返回按钮")]
        public GameObject btnBack;
        [ALHeader("战力变化")]
        public CommonUpgradePropertyShow<Text> txtPower;
        [ALHeader("升级时间")]
        public Text txtUpgradeTime;
        [ALHeader("升级进度")]
        public Slider sldUpgradeProgress;
        [ALHeader("取消按钮")]
        public GameObject btnCancel;
        [ALHeader("加速按钮")]
        public GameObject btnSpeedUp;
        [ALHeader("立即完成")]
        public GameObject btnCompleteNow;
        [ALHeader("立即完成的消耗")]
        public NPGGUIMonoCommonItem monoCompleteNowCostItem;
        [ALHeader("联盟求助按钮")]
        public GameObject btnAssist;
        [ALHeader("可联盟求助需要显隐藏的物体列表")]
        public List<GameObject> canAssistShowGos;
        public List<GameObject> canAssistHideGos;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7112); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7112); } }
    }
}