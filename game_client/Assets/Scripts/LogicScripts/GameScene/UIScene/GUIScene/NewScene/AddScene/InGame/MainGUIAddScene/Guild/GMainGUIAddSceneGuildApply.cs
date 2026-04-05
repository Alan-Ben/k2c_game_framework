using JetBrains.Annotations;

namespace GOE
{
    public class GMainGUIAddSceneGuildApply : _ANPGMainGUIAddSceneResBar<GGUIWndGuildApply>
    {
        [NotNull] public static GMainGUIAddSceneGuildApply instance { get { return _g_instance ??= new GMainGUIAddSceneGuildApply(); } }
        private static GMainGUIAddSceneGuildApply _g_instance;

        protected override GGUIWndGuildApply _m_wnd { get { return GGUIWndGuildApply.instance; } }
    }
}