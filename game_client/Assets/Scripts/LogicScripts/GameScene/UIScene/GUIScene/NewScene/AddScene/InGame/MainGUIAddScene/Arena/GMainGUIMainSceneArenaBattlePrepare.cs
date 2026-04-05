using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗准备主窗口
    /// </summary>
    public class GMainGUIMainSceneArenaBattlePrepare : _ANPGMainGUIAddSceneResBar<GGUIWndArenaBattlePrepare>
    {
        private static GMainGUIMainSceneArenaBattlePrepare _g_instance = new GMainGUIMainSceneArenaBattlePrepare();
        [NotNull] 
        public static GMainGUIMainSceneArenaBattlePrepare instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GMainGUIMainSceneArenaBattlePrepare();
                return _g_instance;
            }
        }

        protected override GGUIWndArenaBattlePrepare _m_wnd { get { return GGUIWndArenaBattlePrepare.instance; } }

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
