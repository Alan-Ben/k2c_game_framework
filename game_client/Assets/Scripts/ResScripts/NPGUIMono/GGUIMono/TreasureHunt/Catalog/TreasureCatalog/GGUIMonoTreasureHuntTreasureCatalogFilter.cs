using UnityEngine;

namespace GOE
{
    public enum ETreasureHuntTreasureCatalogFilterType
    {
        [InspectorName("全部")]
        ALL,
        [InspectorName("已拥有")]
        HAD,
        [InspectorName("未拥有")]
        NOT_HAD,
    }
    
    public class GGUIMonoTreasureHuntTreasureCatalogFilter : _ATNPGGUIMonoCommonMutexFiiterWithFoldToggleWnd<ETreasureHuntTreasureCatalogFilterType>
    {
        
    }
}