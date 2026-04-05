using System;

namespace GOE
{
    public class GGUIWndTreasureHuntOreCatalogFilter : _ATNPGGUIWndCommonMutexFiiterWithFoldToggleWnd<ETreasureHuntOreCatalogFilterType, GGUIMonoTreasureHuntOreCatalogFilter>
    {
        public GGUIWndTreasureHuntOreCatalogFilter(GGUIMonoTreasureHuntOreCatalogFilter _wnd, Action<NPGGUICommonFitterMono<ETreasureHuntOreCatalogFilterType>> _delegate) : base(_wnd, _delegate)
        {
        }
    }
}