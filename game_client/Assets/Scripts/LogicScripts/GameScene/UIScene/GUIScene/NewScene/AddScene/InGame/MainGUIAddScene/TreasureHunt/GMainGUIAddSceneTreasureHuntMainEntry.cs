using System;

namespace GOE
{
    public class GMainGUIAddSceneTreasureHuntMainEntry : _ABaseHomeEntryContainerUIScene
    {
        private static GMainGUIAddSceneTreasureHuntMainEntry _g_instance = new GMainGUIAddSceneTreasureHuntMainEntry();
        public static GMainGUIAddSceneTreasureHuntMainEntry instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneTreasureHuntMainEntry();

                return _g_instance;
            }
        }

        protected GMainGUIAddSceneTreasureHuntMainEntry() : base(true)
        {
        }

        protected override void _onEnterSceneEx()
        {
            GGUIWndTreasureHuntMain.instance.load(setSceneInited);

        }

        protected override void _dealQuitSceneEx()
        {
            GGUIWndTreasureHuntMain.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        protected override void _dealShowSceneEx(Action _delegate)
        {
            GGUIWndTreasureHuntMain.instance.showWnd(_delegate);
        }

        protected override void _dealHideSceneEx(Action _delegate)
        {
            if(!isEntered)
            {
                if(_delegate != null)
                    _delegate();
                return;
            }
            GGUIWndTreasureHuntMain.instance.hideWnd(_delegate);
        }
    }
}