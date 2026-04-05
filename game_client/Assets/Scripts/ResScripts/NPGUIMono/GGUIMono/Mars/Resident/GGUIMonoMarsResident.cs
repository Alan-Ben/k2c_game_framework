using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民
    /// </summary>
    public class GGUIMonoMarsResident : _AALBasicUIWndMono
    {
        [ALHeader("tab列表")]
        public GGUIMonoMarsResidentTabList tabList;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        public GameObject btnCloseAdditional;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7200); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7200);} }
    }
}