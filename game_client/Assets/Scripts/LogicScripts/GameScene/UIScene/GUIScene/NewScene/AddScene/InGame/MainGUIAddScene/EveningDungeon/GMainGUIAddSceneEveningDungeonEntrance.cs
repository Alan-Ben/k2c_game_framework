namespace GOE
{
    /// <summary>
    /// 晚间副本入口
    /// </summary>
    public class GMainGUIAddSceneEveningDungeonEntrance : _ANPGMainGUIAddSceneResBar<GGUIWndEveningDungeonEntrance>
    {
        private static GMainGUIAddSceneEveningDungeonEntrance _g_instance;
        public static GMainGUIAddSceneEveningDungeonEntrance instance { get { return _g_instance ??= new GMainGUIAddSceneEveningDungeonEntrance(); } }
        
        protected override GGUIWndEveningDungeonEntrance _m_wnd { get { return GGUIWndEveningDungeonEntrance.instance; } }
    }
}