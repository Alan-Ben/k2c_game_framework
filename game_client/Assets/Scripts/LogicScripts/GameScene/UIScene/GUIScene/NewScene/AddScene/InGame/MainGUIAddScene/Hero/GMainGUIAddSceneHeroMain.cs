
using System;
using ALPackage;
using ChatPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 宠物主窗口
    /// </summary>
    public class GMainGUIAddSceneHeroMain : _ANPGMainGUIAddSceneResBar<GGUIWndHeroMain>
    {
        private static GMainGUIAddSceneHeroMain _g_instance = new GMainGUIAddSceneHeroMain();
        [NotNull] 
        public static GMainGUIAddSceneHeroMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GMainGUIAddSceneHeroMain();
                return _g_instance;
            }
        }

        public override bool needDiscardOnSwitch { get { return false; } }
        protected override GGUIWndHeroMain _m_wnd { get { return GGUIWndHeroMain.instance; } }

        protected override void _onEnterScene()
        {
            base._onEnterScene();

            // GUIAddSceneHeroTotalAttrTips.instance.enterScene();
        }

        protected override void _dealQuitSceneSub()
        {
            base._dealQuitSceneSub();

            // GUIAddSceneHeroTotalAttrTips.instance.quitScene();
        }
    }
}
