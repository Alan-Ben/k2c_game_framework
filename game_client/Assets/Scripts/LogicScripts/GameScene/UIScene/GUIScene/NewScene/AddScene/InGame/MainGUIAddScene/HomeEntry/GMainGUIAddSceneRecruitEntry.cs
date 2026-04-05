
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 招募相关入口场景节点
    /// </summary>
    public class GMainGUIAddSceneRecruitEntry : _ABaseHomeEntryContainerUIScene
    {
        private static GMainGUIAddSceneRecruitEntry _g_instance = new GMainGUIAddSceneRecruitEntry();
        public static GMainGUIAddSceneRecruitEntry instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneRecruitEntry();

                return _g_instance;
            }
        }

        //显示资源栏的操作序列号
        private int _m_iIconSerialize;
        private int _m_iBarSerialize;

        protected GMainGUIAddSceneRecruitEntry() : base(false)
        {
        }

        protected override void _onEnterSceneEx()
        {
            GGUIWndRecruitMain.instance.load(setSceneInited);

        }

        protected override void _dealQuitSceneEx()
        {
            GGUIWndRecruitMain.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        protected override void _dealShowSceneEx(Action _delegate)
        {
            GGUIWndRecruitMain.instance.showWnd();

            //最后显示Bar
            _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(GGUIWndRecruitMain.instance.getPlayerIconResId());
            _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(GGUIWndRecruitMain.instance.getBarResId());

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
            GGUIWndRecruitMain.instance.hideWnd();

            NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);
            
            if(_delegate != null)
                _delegate();
        }
    }
}
