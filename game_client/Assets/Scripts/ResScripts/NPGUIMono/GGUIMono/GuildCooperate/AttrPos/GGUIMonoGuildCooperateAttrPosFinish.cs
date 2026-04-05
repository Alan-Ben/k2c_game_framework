using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 公会协作属性据点完成弹窗
    /// </summary>
    public class GGUIMonoGuildCooperateAttrPosFinish : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("奖励据点图标")]
        public RawImage imgPosIcon;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4936); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4936); } }
    }
}