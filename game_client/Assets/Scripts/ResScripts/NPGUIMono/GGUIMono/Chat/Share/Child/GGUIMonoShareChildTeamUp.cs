using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 子嗣分享组队弹窗
    /// </summary>
    public class GGUIMonoShareChildTeamUp : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("目标子嗣头像")]
        public RawImage imgTargetIcon;
        [ALHeader("目标子嗣名称")]
        public Text txtTargetName;
        [ALHeader("目标子嗣名称KEY")]
        public string targetNameKey;
        [ALHeader("目标子嗣赚速")]
        public Text txtTargetEarnings;
        [ALHeader("目标子嗣赚速KEY")]
        public string targetEarningsKey;
        [ALHeader("目标子嗣是卷王需要展示的GO列表")]
        public List<GameObject> listTargetSuperShow;
        [ALHeader("自己子嗣列表")]
        public GGUIMonoAdultUnmarriedGrid monoAdultGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1374); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1374);} }
    }
}