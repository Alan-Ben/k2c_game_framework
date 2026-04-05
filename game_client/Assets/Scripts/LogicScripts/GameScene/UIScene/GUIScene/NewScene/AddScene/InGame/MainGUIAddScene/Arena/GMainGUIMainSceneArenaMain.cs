using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 竞技场主窗口
    /// </summary>
    public class GMainGUIMainSceneArenaMain : _ANPGMainGUIAddSceneResBar<GGUIWndArenaMain>
    {
        private static GMainGUIMainSceneArenaMain _g_instance = new GMainGUIMainSceneArenaMain();
        [NotNull] 
        public static GMainGUIMainSceneArenaMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GMainGUIMainSceneArenaMain();
                return _g_instance;
            }
        }

        protected override GGUIWndArenaMain _m_wnd { get { return GGUIWndArenaMain.instance; } }

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
