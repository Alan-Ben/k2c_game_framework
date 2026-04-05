using UnityEngine;

namespace GOE
{
    public enum ETreasureHuntOreCatalogFilterType
    {
        [InspectorName("全部")]
        ALL,
        [InspectorName("已拥有")]
        HAD,
        [InspectorName("未拥有")]
        NOT_HAD,
        [InspectorName("高级矿石")]
        ADVANCE_ORE,
        [InspectorName("普通矿石")]
        NORMAL_ORE,
    }
    
    public class GGUIMonoTreasureHuntOreCatalogFilter : _ATNPGGUIMonoCommonMutexFiiterWithFoldToggleWnd<ETreasureHuntOreCatalogFilterType>
    {
        
    }
}