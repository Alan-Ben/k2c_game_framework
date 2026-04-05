namespace GOE
{
    /// <summary>
    /// 副本入口
    /// </summary>
    public class GMainGUIAddSceneDungeonEntrance : _ANPGMainGUIAddSceneResBar<GGUIWndDungeonEntrance>
    {
        private static GMainGUIAddSceneDungeonEntrance _g_instance;
        public static GMainGUIAddSceneDungeonEntrance instance { get { return _g_instance ??= new GMainGUIAddSceneDungeonEntrance(); } }
        
        protected override GGUIWndDungeonEntrance _m_wnd { get { return GGUIWndDungeonEntrance.instance; } }
    }
}