using System;

namespace GOE
{
    /// <summary>
    /// 联盟杂物委托Scene
    /// </summary>
    public class GMainGUIAddSceneGuildEntrust : _ANPGMainGUIAddSceneResBar<GGUIWndGuildEntrust>
    {
        private static GMainGUIAddSceneGuildEntrust _g_instance;
        public static GMainGUIAddSceneGuildEntrust instance { get { return _g_instance ??= new GMainGUIAddSceneGuildEntrust(); } }

        protected override GGUIWndGuildEntrust _m_wnd { get { return GGUIWndGuildEntrust.instance; } }
    }
}