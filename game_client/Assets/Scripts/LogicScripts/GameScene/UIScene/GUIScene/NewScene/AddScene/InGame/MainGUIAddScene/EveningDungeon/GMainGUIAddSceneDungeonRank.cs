namespace GOE
{
    public class GMainGUIAddSceneDungeonRank : _ANPGMainGUIAddSceneResBar<GGUIWndDungeonRank>
    {
        private static GMainGUIAddSceneDungeonRank _g_instance;
        public static GMainGUIAddSceneDungeonRank instance { get { return _g_instance ??= new GMainGUIAddSceneDungeonRank(); } }

        protected override GGUIWndDungeonRank _m_wnd { get { return GGUIWndDungeonRank.instance; } }
    }
}