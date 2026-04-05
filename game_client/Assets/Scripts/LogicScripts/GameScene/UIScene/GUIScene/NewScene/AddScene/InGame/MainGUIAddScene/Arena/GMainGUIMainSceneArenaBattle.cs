using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗主窗口
    /// </summary>
    public class GMainGUIMainSceneArenaBattle : _ANPGMainGUIAddSceneResBar<GGUIWndArenaBattle>
    {
        private static GMainGUIMainSceneArenaBattle _g_instance = new GMainGUIMainSceneArenaBattle();
        [NotNull] 
        public static GMainGUIMainSceneArenaBattle instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GMainGUIMainSceneArenaBattle();
                return _g_instance;
            }
        }

        protected override GGUIWndArenaBattle _m_wnd { get { return GGUIWndArenaBattle.instance; } }

        protected override void _onEnterScene()
        {
            base._onEnterScene();
        }

        protected override void _dealQuitSceneSub()
        {
            base._dealQuitSceneSub();
        }
    }
}
