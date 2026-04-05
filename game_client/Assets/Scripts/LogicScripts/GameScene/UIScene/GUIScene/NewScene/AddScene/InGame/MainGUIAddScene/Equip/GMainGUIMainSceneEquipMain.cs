using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 藏品主窗口
    /// </summary>
    public class GMainGUIMainSceneEquipMain : _ANPGMainGUIAddSceneResBar<GGUIWndEquipMain>
    {
        private static GMainGUIMainSceneEquipMain _g_instance = new GMainGUIMainSceneEquipMain();
        [NotNull] 
        public static GMainGUIMainSceneEquipMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GMainGUIMainSceneEquipMain();
                return _g_instance;
            }
        }

        protected override GGUIWndEquipMain _m_wnd { get { return GGUIWndEquipMain.instance; } }

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
