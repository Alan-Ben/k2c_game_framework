
using JetBrains.Annotations;

namespace GOE
{
    public class GMainGUIAddSceneChild : _ANPGMainGUIAddSceneResBar<GGUIWndChildMain>
    {
        [NotNull] public static GMainGUIAddSceneChild instance { get { return _g_instance ??= new GMainGUIAddSceneChild(); } }
        private static GMainGUIAddSceneChild _g_instance;

        protected override GGUIWndChildMain _m_wnd { get { return GGUIWndChildMain.instance; } }
    }
}