using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 游历地点解锁弹窗
    /// </summary>
    public class GGUIMonoTravelPosUnlock : _AALBasicUIWndMono
    {
        [ALHeader("地点icon")]
        public RawImage icon;
        [ALHeader("地点名")]
        public TextEx txtPosName;
        [ALHeader("地点描述")]
        public TextEx txtPosDesc;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("前往按钮")]
        public GameObject btnGoto;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3631); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3631); } }
    }
}