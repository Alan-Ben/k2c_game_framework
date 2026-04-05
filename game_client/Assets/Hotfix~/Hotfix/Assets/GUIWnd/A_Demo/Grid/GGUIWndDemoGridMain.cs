using System.Collections.Generic;
using ALPackage;

namespace Hotfix
{
    /// <summary>
    /// 范例包含grid的Wnd主窗口
    /// </summary>
    public class GGUIWndDemoGridMain : _AHotfixBaseWnd<GGUIMonoDemoGridMain>
    {
        private static GGUIWndDemoGridMain _g_instance;
        public static GGUIWndDemoGridMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndDemoGridMain();
                return _g_instance;
            }
        }

        //普通grid
        private GGUIWndDemoGrid _m_wGrid;
        //包含bar的grid
        private GGUIWndDemoGridWithBar _m_wGridWithBar;

        protected override string _monoAssetPath { get { return "gui/game_gui.unity3d";} }
        protected override string _monoObjName { get { return "win_hotfix_demo_Grid_wnd"; } }

        public GGUIWndDemoGridMain() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override void _onShowWnd()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridMain===_onShowWnd");

            List<int> numList = new List<int>()
            {
                1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20
            };

            if (_m_wGrid != null)
            {
                _m_wGrid.showWnd();
                _m_wGrid.showItemList(numList);
            }

            if (_m_wGridWithBar != null)
            {
                _m_wGridWithBar.showWnd();
                _m_wGridWithBar.showItemList(numList);
            }
        }

        protected override void _onHideWnd()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridMain===_onHideWnd");

            if(_m_wGrid != null)
                _m_wGrid.hideWnd();

            if(_m_wGridWithBar != null)
                _m_wGridWithBar.hideWnd();
        }

        protected override void _onReset()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridMain===_onReset");

            if (_m_wGrid != null)
                _m_wGrid.resetWnd();

            if (_m_wGridWithBar != null)
                _m_wGridWithBar.resetWnd();
        }

        protected override void _onDiscard()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridMain===_onDiscard");

            if (_m_wGrid != null)
                _m_wGrid.discard();
            _m_wGrid = null;

            if (_m_wGridWithBar != null)
                _m_wGridWithBar.discard();
            _m_wGridWithBar = null;
        }

        protected override void _onWndInitDoneHotfix()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridMain===_onWndInitDoneHotfix");

            if (hotfixWnd == null)
                return;

            if(hotfixWnd.gridMono != null)
                _m_wGrid = new GGUIWndDemoGrid(hotfixWnd.gridMono);

            if(hotfixWnd.gridWithBarMono != null)
                _m_wGridWithBar = new GGUIWndDemoGridWithBar(hotfixWnd.gridWithBarMono);
        }
    }
}