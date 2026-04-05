using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndMarsExploreEventQualityContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsExploreEventQualityContainerItem>
    {
        [CanBeNull] private CommonQualityWeight _m_qualityWeight;
        private NPGGuiWndTexture _m_qualityIconWnd;


        public GGUISubWndMarsExploreEventQualityContainerItem(GGUIMonoMarsExploreEventQualityContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_qualityIconWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_qualityIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_qualityIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_qualityIconWnd?.discard();
            _m_qualityIconWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgQualityIcon != null)
                _m_qualityIconWnd = new NPGGuiWndTexture(wnd.imgQualityIcon);
        }


        public void refreshWnd([CanBeNull] CommonQualityWeight _qualityWeight)
        {
            _m_qualityWeight = _qualityWeight;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_qualityWeight == null)
                return;

            NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long)_m_qualityWeight.quality);
            _m_qualityIconWnd?.setTexture(qualityExtRefObj?.mars_explore_event_quality_icon);
        }
    }
}
