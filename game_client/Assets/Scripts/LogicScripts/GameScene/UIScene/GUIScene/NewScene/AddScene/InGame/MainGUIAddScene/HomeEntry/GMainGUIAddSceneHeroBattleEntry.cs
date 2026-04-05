
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 骑士挑战相关入口场景节点
    /// </summary>
    public class GMainGUIAddSceneHeroBattleEntry : _ABaseHomeEntryContainerUIScene
    {
        private static GMainGUIAddSceneHeroBattleEntry _g_instance = new GMainGUIAddSceneHeroBattleEntry();
        public static GMainGUIAddSceneHeroBattleEntry instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneHeroBattleEntry();

                return _g_instance;
            }
        }

        // //显示资源栏的操作序列号
        // private int _m_iIconSerialize;
        // private int _m_iBarSerialize;

        protected GMainGUIAddSceneHeroBattleEntry() : base(false)
        {
        }

        protected override void _onEnterSceneEx()
        {
            GGUIWndHeroBattleEntry.instance.load(setSceneInited);

        }

        protected override void _dealQuitSceneEx()
        {
            GGUIWndHeroBattleEntry.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        protected override void _dealShowSceneEx(Action _delegate)
        {
            GGUIWndHeroBattleEntry.instance.showWnd();

            //最后显示Bar
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
            GGUIWndHeroBattleEntry.instance.hideWnd();

            // NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            // NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);
            
            if(_delegate != null)
                _delegate();
        }
    }
}
