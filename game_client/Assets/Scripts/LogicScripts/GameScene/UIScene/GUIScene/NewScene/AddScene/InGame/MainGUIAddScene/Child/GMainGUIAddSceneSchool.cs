
using JetBrains.Annotations;

namespace GOE
{
    public class GMainGUIAddSceneSchool : _ANPGMainGUIAddSceneResBar<GGUIWndSchool>
    {
        [NotNull] public static GMainGUIAddSceneSchool instance { get { return _g_instance ??= new GMainGUIAddSceneSchool(); } }
        private static GMainGUIAddSceneSchool _g_instance;

        protected override GGUIWndSchool _m_wnd { get { return GGUIWndSchool.instance; } }
    }
}