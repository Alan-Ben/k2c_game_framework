using System;

namespace GOE
{
    /// <summary>
    /// 联盟scene
    /// </summary>
    public class GMainGUIAddSceneGuildMain : _ABaseHomeEntryContainerUIWithResBarScene
    {
        private static GMainGUIAddSceneGuildMain _g_instance = new GMainGUIAddSceneGuildMain();
        public static GMainGUIAddSceneGuildMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneGuildMain();

                return _g_instance;
            }
        }

        protected GMainGUIAddSceneGuildMain() : base(false)
        {
        }

        protected override void _onEnterSceneEx()
        {
            GGUIWndGuildMain.instance.load(setSceneInited);
        }

        protected override void _dealQuitSceneEx()
        {
            GGUIWndGuildMain.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        protected override void _dealShowSceneEx(Action _delegate)
        {
            showMainWnd(GGUIWndGuildMain.instance, _delegate);
        }

        protected override void _dealHideSceneEx(Action _delegate)
        {
            if (!isEntered)
            {
                if(_delegate != null)
                    _delegate();

                return;
            }

            GGUIWndGuildMain.instance.hideWnd(_delegate);
        }
    }
}
