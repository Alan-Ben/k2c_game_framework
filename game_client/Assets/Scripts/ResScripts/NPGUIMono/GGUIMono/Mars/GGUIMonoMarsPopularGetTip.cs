using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星居民获取提示
    /// </summary>
    public class GGUIMonoMarsPopularGetTip : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7210); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7210); } }
    }
}
