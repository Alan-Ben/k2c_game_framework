
using System;

namespace GOE
{
    /// <summary>
    /// 到达火星着陆选择界面scene
    /// </summary>
    public class GMainGUIAddSceneMarsLandingSelect : _ANPGMainGUIAddSceneResBar<GGUIWndMarsLandingSelect>
    {
        private static GMainGUIAddSceneMarsLandingSelect _g_instance = new GMainGUIAddSceneMarsLandingSelect();
        public static GMainGUIAddSceneMarsLandingSelect instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneMarsLandingSelect();

                return _g_instance;
            }
        }

        public override bool needDiscardOnSwitch { get { return false; } }
        protected override GGUIWndMarsLandingSelect _m_wnd { get { return GGUIWndMarsLandingSelect.instance; } }

        public void setInfo(Action _onSelectLanding)
        {
            GGUIWndMarsLandingSelect.instance.setInfo(_onSelectLanding);
        }
    }
}
