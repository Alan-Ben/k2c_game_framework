using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// VIP等级预览
    /// </summary>
    public class GGUIMonoVIPLevelPreview : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("经验获取途径按钮")]
        public GameObject btnAccess;
        [ALHeader("领取奖励按钮")]
        public GameObject btnGetReward;
        [ALHeader("上一个按钮")]
        public GameObject btnPrevious;
        [ALHeader("下一个按钮")]
        public GameObject btnNext;
        [ALHeader("当前经验进度条")]
        public NPGGUIMonoProgress expProgress;
        [ALHeader("升级VIP等级描述")]
        public Text txtUpgradeDesc;
        [ALHeader("当前VIP等级")]
        public Text txtVIP;
        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer monoItemContainer;
        [ALHeader("无奖励时显示的GO列表")]
        public List<GameObject> goNoRewardShowList;
        [ALHeader("无奖励时隐藏的GO列表")]
        public List<GameObject> goNoRewardHideList;
        [ALHeader("属性列表")]
        public NPGGUIMonoCommonTextItemGrid monoPropertyGrid;
        [ALHeader("普通属性文本颜色")]
        public Color normalPropertyColor = Color.black;
        [ALHeader("新属性文本颜色")]
        public Color newPropertyColor = Color.green;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8101); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8101); } }
    }

}
