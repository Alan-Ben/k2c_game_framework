using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 本地推送设置窗口
    /// </summary>
    public class GGUIMonoGameSettingLocalPush : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("总开关")]
        public NPGGUIMonoCommonTab monoMainSwitch;
        [ALHeader("开关列表")] 
        public GGUIMonoGameSettingLocalPushContainer monoGameSettingLocalPushContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1750); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1750); } }
    }
}
