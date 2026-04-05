using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoPushGiftPackList : _AALBasicUIWndMono
    {
        [ALHeader("推送礼包预制体加载父节点")]
        public Transform pushGiftPackPrefabParent;

        [ALHeader("推送礼包Banner容器")]
        public GGUIMonoPushGiftPackBannerItemContainer monoPushGiftPackBannerContainer;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8500); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8500);} }
    }
}