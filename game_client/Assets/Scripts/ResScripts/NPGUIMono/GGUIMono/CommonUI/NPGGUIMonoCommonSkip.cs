using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用的跳过按钮
    /// </summary>
    public class NPGGUIMonoCommonSkip : _AALBasicUIWndMono
    {
        [ALHeader("跳过按钮")]
        public GameObject btnSkip;
                
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_SKIP); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_SKIP); } }
    }
}