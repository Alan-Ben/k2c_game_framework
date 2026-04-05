using System;

namespace GOE
{
    /// <summary>
    /// 联盟派遣Scene
    /// </summary>
    public class GMainGUIAddSceneGuildDispatch : _ANPGMainGUIAddSceneResBar<GGUIWndGuildDispatchMain>
    {
        private static GMainGUIAddSceneGuildDispatch _g_instance;
        public static GMainGUIAddSceneGuildDispatch instance { get { return _g_instance ??= new GMainGUIAddSceneGuildDispatch(); } }

        protected override GGUIWndGuildDispatchMain _m_wnd { get { return GGUIWndGuildDispatchMain.instance; } }
    }
}