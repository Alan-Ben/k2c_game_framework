using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合图鉴过滤类型
    /// </summary>
    public enum ETreasureHuntCompositeCatalogFilterType
    {
        [InspectorName("全部")]
        ALL,
        [InspectorName("已拥有")]
        HAD,
        [InspectorName("未拥有")]
        NOT_HAD,
        [InspectorName("高级图鉴")]
        ADVANCE_CATALOG,
        [InspectorName("普通图鉴")]
        NORMAL_CATALOG,
    }
    
    public class GGUIMonoTreasureHuntCompositeCatalogFilter : _ATNPGGUIMonoCommonMutexFiiterWithFoldToggleWnd<ETreasureHuntCompositeCatalogFilterType>
    {
        
    }
}