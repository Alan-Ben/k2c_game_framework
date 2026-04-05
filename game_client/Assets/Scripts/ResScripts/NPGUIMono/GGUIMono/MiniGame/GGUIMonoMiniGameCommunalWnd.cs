using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 小游戏中的公用游戏窗口
    /// </summary>
    public class GGUIMonoMiniGameCommunalWnd : _AALBasicUIWndMono
    {
        [ALHeader("跳过按钮")]
        public GameObject btnSkip;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(10000); } }
        public static string objName { get { return UIResPathAssistant.getObjName(10000);} }
    }
}