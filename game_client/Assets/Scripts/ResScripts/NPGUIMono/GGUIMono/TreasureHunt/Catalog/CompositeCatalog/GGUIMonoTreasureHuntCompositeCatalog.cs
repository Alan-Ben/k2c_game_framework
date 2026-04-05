using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合图鉴
    /// </summary>
    public class GGUIMonoTreasureHuntCompositeCatalog : _ANPBasicUIWndResBarMono
    {
        [ALHeader("普通图鉴收集进度")]
        public TextEx txtNormalCompositeCatalogCollectProgress;
        [ALHeader("普通图鉴收集进度Key, 两个参数(1.当前收集数量, 2.总数量)")]
        public string txtNormalCompositeCatalogCollectProgressKey;
        
        [ALHeader("高级图鉴收集进度")]
        public TextEx txtAdvancedCompositeCatalogCollectProgress;
        [ALHeader("高级图鉴收集进度Key, 两个参数(1.当前收集数量, 2.总数量)")]
        public string txtAdvancedCompositeCatalogCollectProgressKey;

        [ALHeader("过滤类型")]
        public GGUIMonoTreasureHuntCompositeCatalogFilter monoFilter;

        [ALHeader("组合图鉴Grid")]
        public GGUIMonoTreasureHuntCompositeCatalogItemGrid monoCompositeCatalogItemGrid;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6821); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6821); } }
    }
}