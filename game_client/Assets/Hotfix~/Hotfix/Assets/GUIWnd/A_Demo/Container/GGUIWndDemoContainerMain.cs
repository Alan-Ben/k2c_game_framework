using System.Collections.Generic;
using ALPackage;

namespace Hotfix
{
    /// <summary>
    /// 范例包含container的Wnd主窗口
    /// </summary>
    public class GGUIWndDemoContainerMain : _AHotfixBaseWnd<GGUIMonoDemoContainerMain>
    {
        private static GGUIWndDemoContainerMain _g_instance;
        public static GGUIWndDemoContainerMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndDemoContainerMain();
                return _g_instance;
            }
        }

        private GGUIWndDemoContainer _m_container;

        protected override string _monoAssetPath { get { return "gui/game_gui.unity3d";} }
        protected override string _monoObjName { get { return "win_hotfix_demo_container_wnd"; } }

        public GGUIWndDemoContainerMain() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override void _onShowWnd()
        {
            Debug.LogError($"=====NPGGUIDemoContainerMain===_onShowWnd");

            if (_m_container != null)
            {
                List<int> numList = new List<int>()
                {
                    1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20
                };

                _m_container.showWnd();
                _m_container.showItemList(numList);
            }
        }

        protected override void _onHideWnd()
        {
            Debug.LogError($"=====NPGGUIDemoContainerMain===_onHideWnd");

            if(_m_container != null)
                _m_container.hideWnd();
        }

        protected override void _onReset()
        {
            Debug.LogError($"=====NPGGUIDemoContainerMain===_onReset");

            if (_m_container != null)
                _m_container.resetWnd();
        }

        protected override void _onDiscard()
        {
            Debug.LogError($"=====NPGGUIDemoContainerMain===_onDiscard");

            if (_m_container != null)
                _m_container.discard();
            _m_container = null;
        }

        protected override void _onWndInitDoneHotfix()
        {
            Debug.LogError($"=====NPGGUIDemoContainerMain===_onWndInitDoneHotfix");

            if (hotfixWnd == null)
                return;

            if(hotfixWnd.containerMono != null)
                _m_container = new GGUIWndDemoContainer(hotfixWnd.containerMono);
        }
    }
}