using JetBrains.Annotations;

namespace GOE
{
    public class GMainGUIAddSceneAdultMain : _ANPGMainGUIAddSceneResBar<GGUIWndAdultMain>
    {
        [NotNull] public static GMainGUIAddSceneAdultMain instance { get { return _g_instance ??= new GMainGUIAddSceneAdultMain(); } }
        private static GMainGUIAddSceneAdultMain _g_instance;

        protected override GGUIWndAdultMain _m_wnd { get { return GGUIWndAdultMain.instance; } }
    }
}