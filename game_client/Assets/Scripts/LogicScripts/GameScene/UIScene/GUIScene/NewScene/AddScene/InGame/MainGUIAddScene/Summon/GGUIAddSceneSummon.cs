namespace GOE
{
    /// <summary>
    /// 召唤系统Scene
    /// </summary>
    public class GGUIAddSceneSummon : _ANPGMainGUIAddSceneResBar<GGUIWndSummonMain>
    {
        private static GGUIAddSceneSummon _g_instance;
        public static GGUIAddSceneSummon instance { get { return _g_instance ??= new GGUIAddSceneSummon(); } }
        
        protected override GGUIWndSummonMain _m_wnd { get { return GGUIWndSummonMain.instance; } }
    }
}