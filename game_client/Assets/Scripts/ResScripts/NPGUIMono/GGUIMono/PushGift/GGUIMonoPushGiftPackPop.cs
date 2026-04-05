using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 推送礼包弹出界面
    /// </summary>
    public class GGUIMonoPushGiftPackPop : _AALBasicUIWndMono
    {
        [ALHeader("推送礼包预制体加载父节点")]
        public Transform pushGiftPackPrefabParent;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8501); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8501);} }
    }
}