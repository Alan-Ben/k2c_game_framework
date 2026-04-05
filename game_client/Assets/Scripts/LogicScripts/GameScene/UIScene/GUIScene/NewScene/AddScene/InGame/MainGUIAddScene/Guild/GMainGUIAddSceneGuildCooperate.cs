using System;

namespace GOE
{
    /// <summary>
    /// 联盟杂物委托Scene
    /// </summary>
    public class GMainGUIAddSceneGuildCooperate : _ANPGMainGUIAddSceneResBar<GGUIWndGuildCooperateMain>
    {
        private static GMainGUIAddSceneGuildCooperate _g_instance;
        public static GMainGUIAddSceneGuildCooperate instance { get { return _g_instance ??= new GMainGUIAddSceneGuildCooperate(); } }

        protected override GGUIWndGuildCooperateMain _m_wnd { get { return GGUIWndGuildCooperateMain.instance; } }
    }
}