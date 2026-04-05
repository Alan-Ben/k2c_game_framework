
namespace GOE
{
    /// <summary>
    /// 前往火星scene
    /// </summary>
    public class GMainGUIAddSceneMarsGoTo : _ANPGMainGUIAddSceneResBar<GGUIWndMarsGoTo>
    {
        private static GMainGUIAddSceneMarsGoTo _g_instance = new GMainGUIAddSceneMarsGoTo();
        public static GMainGUIAddSceneMarsGoTo instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneMarsGoTo();

                return _g_instance;
            }
        }

        public override bool needDiscardOnSwitch { get { return false; } }
        protected override GGUIWndMarsGoTo _m_wnd { get { return GGUIWndMarsGoTo.instance; } }
    }
}
