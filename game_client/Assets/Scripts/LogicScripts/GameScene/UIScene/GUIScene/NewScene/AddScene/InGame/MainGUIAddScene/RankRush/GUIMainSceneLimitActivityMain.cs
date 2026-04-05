using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 限时活动主界面
    /// </summary>
    public class GUIMainSceneLimitActivityMain : _ANPGMainGUIAddSceneResBar<GGUIWndLimitActivityMain>
    {
        private static GUIMainSceneLimitActivityMain _g_instance = new GUIMainSceneLimitActivityMain();
        [NotNull] 
        public static GUIMainSceneLimitActivityMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GUIMainSceneLimitActivityMain();
                return _g_instance;
            }
        }

        protected override GGUIWndLimitActivityMain _m_wnd { get { return GGUIWndLimitActivityMain.instance; } }

        protected override void _onEnterScene()
        {
            base._onEnterScene();
        }

        protected override void _dealQuitSceneSub()
        {
            base._dealQuitSceneSub();
        }

        public void setInfo(ELimitActivityTabType _tabType)
        {
            GGUIWndLimitActivityMain.instance.setInfo(_tabType);
        }
    }
}
