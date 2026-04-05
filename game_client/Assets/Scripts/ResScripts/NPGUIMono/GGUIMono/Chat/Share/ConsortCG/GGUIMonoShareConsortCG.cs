using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 家人CG分享弹窗
    /// </summary>
    public class GGUIMonoShareConsortCG : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("分享按钮")]
        public GameObject btnShare;
        [ALHeader("CG列表")]
        public GGUIMonoShareConsortCGGrid monoCGGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1370); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1370);} }
    }
}