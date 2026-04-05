using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 区域详情Tab类型
    /// </summary>
    public enum ETreasureHuntAreaOreTreasureDetailTabType
    {
        ORE,        // 矿石
        TREASURE,   // 奇物
    }
    
    /// <summary>
    /// 太空区域产出的矿石、奇物详情信息
    /// </summary>
    public class GGUIMonoTreasureAreaOreTreasureDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("矿石tab")]
        public NPGGUIMonoCommonTab monoOreTab;
        [ALHeader("可获取矿石列表")]
        public GGUIMonoTreasureHuntOreItemContainer monoOreContainer;
        
        [ALHeader("奇物tab")]
        public NPGGUIMonoCommonTab monoTreasureTab;
        [ALHeader("可获取奇物列表")]
        public GGUIMonoTreasureHuntTreasureItemContainer monoTreasureContainer;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6810); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6810); } }
    }
}