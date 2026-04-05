namespace GOE
{
    /// <summary>
    /// 奇物图鉴
    /// </summary>
    public class GGUIMonoTreasureHuntTreasureCatalog : GGUIMonoTreasureHuntCatalogTabWnd
    {
        [ALHeader("图鉴收集进度")]
        public TextEx txtTreasureCollectProgress;
        [ALHeader("图鉴收集进度Key, 两个参数(1.当前收集数量, 2.总数量)")]
        public string txtTreasureCollectProgressKey;
        
        [ALHeader("奇物图鉴过滤窗口")]
        public GGUIMonoTreasureHuntTreasureCatalogFilter filterWnd;

        [ALHeader("奇物列表")]
        public GGUIMonoTreasureHuntTreasureItemGrid treasureGrid;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6818); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6818); } }
    }
}