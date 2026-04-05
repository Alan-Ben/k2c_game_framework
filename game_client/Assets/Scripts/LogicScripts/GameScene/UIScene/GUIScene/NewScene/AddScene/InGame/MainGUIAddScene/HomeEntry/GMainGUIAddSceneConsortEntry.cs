
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 妃子相关入口场景节点
    /// </summary>
    public class GMainGUIAddSceneConsortEntry : _ABaseHomeEntryContainerUIScene
    {
        private static GMainGUIAddSceneConsortEntry _g_instance = new GMainGUIAddSceneConsortEntry();
        public static GMainGUIAddSceneConsortEntry instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneConsortEntry();

                return _g_instance;
            }
        }

        // //显示资源栏的操作序列号
        // private int _m_iIconSerialize;
        // private int _m_iBarSerialize;

        protected GMainGUIAddSceneConsortEntry() : base(false)
        {
        }

        protected override void _onEnterSceneEx()
        {
            GGUIWndConsortEnter.instance.load(setSceneInited);

        }

        protected override void _dealQuitSceneEx()
        {
            GGUIWndConsortEnter.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        protected override void _dealShowSceneEx(Action _delegate)
        {
            GGUIWndConsortEnter.instance.showWnd();

            // //最后显示Bar
            // _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(NPGGUIWndMain.instance.getPlayerIconResId());
            // _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(NPGGUIWndMain.instance.getBarResId());

            if (null != _delegate)
                _delegate();
        }

        protected override void _dealHideSceneEx(Action _delegate)
        {
            if(!isEntered)
            {
                if(_delegate != null)
                    _delegate();
                return;
            }
            GGUIWndConsortEnter.instance.hideWnd();

            // NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            // NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);
            
            if(_delegate != null)
                _delegate();
        }
    }
}
