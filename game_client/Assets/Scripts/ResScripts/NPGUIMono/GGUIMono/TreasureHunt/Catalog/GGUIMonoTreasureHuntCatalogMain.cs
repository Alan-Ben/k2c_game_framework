using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 图鉴主页面
    /// </summary>
    public class GGUIMonoTreasureHuntCatalogMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("矿石图鉴按钮")]
        public GameObject btnOreCatalog;
        [ALHeader("矿石图鉴进度")]
        public NPGGUIMonoProgress monoOreCatalogProgress;
        
        [ALHeader("奇物图鉴按钮")]
        public GameObject btnTreasureCatalog;
        [ALHeader("奇物图鉴进度")]
        public NPGGUIMonoProgress monoTreasureCatalogProgress;
        
        [ALHeader("组合图鉴按钮")]
        public GameObject btnCompositeCatalog;
        [ALHeader("组合图鉴进度")]
        public NPGGUIMonoProgress monoCompositeCatalogProgress;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6816); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6816); } }
    }
}