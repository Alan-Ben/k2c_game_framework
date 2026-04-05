namespace GOE
{
    /// <summary>
    /// 矿石图鉴
    /// </summary>
    public class GGUIMonoTreasureHuntOreCatalog : GGUIMonoTreasureHuntCatalogTabWnd
    {
        [ALHeader("普通矿石收集进度")]
        public TextEx txtNormalOreCollectProgress;
        [ALHeader("普通矿石收集进度Key, 两个参数(1.当前收集数量, 2.总数量)")]
        public string txtNormalOreCollectProgressKey;
        
        [ALHeader("高级矿石收集进度")]
        public TextEx txtAdvancedOreCollectProgress;
        [ALHeader("高级矿石收集进度Key, 两个参数(1.当前收集数量, 2.总数量)")]
        public string txtAdvancedOreCollectProgressKey;
        
        [ALHeader("矿石图鉴过滤窗口")]
        public GGUIMonoTreasureHuntOreCatalogFilter filterWnd;

        [ALHeader("矿石列表")]
        public GGUIMonoTreasureHuntOreItemGrid oreGrid;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6817); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6817); } }
    }
}