
namespace GOE
{
    /// <summary>
    /// 前往火星scene
    /// </summary>
    public class GMainGUIAddSceneMarsGoToStartConfirm : _ANPGMainGUIAddSceneResBar<GGUIWndMarsGoToStartConfirm>
    {
        private static GMainGUIAddSceneMarsGoToStartConfirm _g_instance = new GMainGUIAddSceneMarsGoToStartConfirm();
        public static GMainGUIAddSceneMarsGoToStartConfirm instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneMarsGoToStartConfirm();

                return _g_instance;
            }
        }

        public override bool needDiscardOnSwitch { get { return false; } }
        protected override GGUIWndMarsGoToStartConfirm _m_wnd { get { return GGUIWndMarsGoToStartConfirm.instance; } }
    }
}
