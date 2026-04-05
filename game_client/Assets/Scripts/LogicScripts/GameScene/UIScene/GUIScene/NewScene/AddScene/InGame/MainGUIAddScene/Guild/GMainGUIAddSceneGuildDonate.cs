using System;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠Scene
    /// </summary>
    public class GMainGUIAddSceneGuildDonate : _ANPGMainGUIAddSceneResBar<GGUIWndGuildDonate>
    {
        private static GMainGUIAddSceneGuildDonate _g_instance;
        public static GMainGUIAddSceneGuildDonate instance { get { return _g_instance ??= new GMainGUIAddSceneGuildDonate(); } }

        protected override GGUIWndGuildDonate _m_wnd { get { return GGUIWndGuildDonate.instance; } }
    }
}