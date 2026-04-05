
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 宴会相关入口场景节点
    /// </summary>
    public class GMainGUIAddSceneDinnerEntry : _ABaseHomeEntryContainerUIScene
    {
        private static GMainGUIAddSceneDinnerEntry _g_instance = new GMainGUIAddSceneDinnerEntry();
        public static GMainGUIAddSceneDinnerEntry instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneDinnerEntry();

                return _g_instance;
            }
        }

        // //显示资源栏的操作序列号
        // private int _m_iIconSerialize;
        // private int _m_iBarSerialize;

        protected GMainGUIAddSceneDinnerEntry() : base(false)
        {
        }

        protected override void _onEnterSceneEx()
        {
            GGUIWndDinnerEnterMain.instance.load(setSceneInited);

        }

        protected override void _dealQuitSceneEx()
        {
            GGUIWndDinnerEnterMain.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        protected override void _dealShowSceneEx(Action _delegate)
        {
            GGUIWndDinnerEnterMain.instance.showWnd(_delegate);

            // //最后显示Bar
            // _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(NPGGUIWndMain.instance.getPlayerIconResId());
            // _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(NPGGUIWndMain.instance.getBarResId());
        }

        protected override void _dealHideSceneEx(Action _delegate)
        {
            if(!isEntered)
            {
                if(_delegate != null)
                    _delegate();
                return;
            }
            GGUIWndDinnerEnterMain.instance.hideWnd();

            // NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            // NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);
            
            if(_delegate != null)
                _delegate();
        }
    }
}
