using ALPackage;

namespace GOE
{
    /// <summary>
    /// 处理事件时的背景窗口
    /// </summary>
    public class GGUIWndTravelDealEventBg : _ANPGGUIBasicWnd<GGUIMonoTravelDealEventBg>
    {
        private static GGUIWndTravelDealEventBg _g_instance;
        public static GGUIWndTravelDealEventBg instance { get { return _g_instance ??= new GGUIWndTravelDealEventBg(); } }

        private TravelPosRefObj _m_rTravelPosRefObj;

        private NPGGUIWndCommonShowCase _m_wBgShowCase;

        public GGUIWndTravelDealEventBg() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTravelDealEventBg.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTravelDealEventBg.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化背景ShowCase
            if (wnd.monoShowCaseBg != null)
                _m_wBgShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowCaseBg);
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wBgShowCase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBgShowCase?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wBgShowCase?.discard();
            _m_wBgShowCase = null;
        }


        /// <summary>
        /// 刷新窗口，传入游历地点配置
        /// </summary>
        /// <param name="_posRefObj">游历地点配置</param>
        public void refreshWnd(TravelPosRefObj _posRefObj)
        {
            _m_rTravelPosRefObj = _posRefObj;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (_m_rTravelPosRefObj == null || !isShow)
                return;

            // 加载对话背景场景资源到ShowCase
            if (_m_wBgShowCase != null && _m_rTravelPosRefObj.dialogue_bg_index != null)
                _m_wBgShowCase.showWnd(new ShowCaseCommonResUnitInfoObj(_m_rTravelPosRefObj.dialogue_bg_index));
        }
    }
}
