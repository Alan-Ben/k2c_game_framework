
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBuildingBuild : _AALBasicUIWndMono
    {
        [ALHeader("建筑名字")]
        public Text txtName;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        public GameObject btnClose2;
        [ALHeader("建筑预览样式")]
        public RawImage imgPreviewTex;
        [ALHeader("如果是经营建筑要展示的内容")]
        public List<GameObject> businessBuildingShow;
        [ALHeader("建筑收益文本")]
        public Text txtEmployeeEarnings;
        [ALHeader("未解锁和解锁了的状态的展示对象")]
        public List<GameObject> listLockedShow;
        public List<GameObject> listUnlockedShow;
        [ALHeader("建筑描述")]
        public Text txtDesc;
        [ALHeader("建造建筑的消耗")]
        public NPGGUIMonoCommonItem monoBuildCost;
        [ALHeader("建造按钮")]
        public GameObject btnBuild;
        [ALHeader("建造条件未达成时的提示")]
        public Text txtLockTip;
        [ALHeader("建造条件未达成时的跳转按钮")]
        public GameObject btnLockJump;
        [ALHeader("聚焦相关的配置")]
        public Vector2 focusViewportPos = new Vector2(0.5f, 0.7f);
        public float focusCameraSize = 10;
        public float focusMoveTime = 0.5f;
        
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1108); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1108); } }
    }
}