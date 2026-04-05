using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 好友添加分组
    /// </summary>
    public class GGUIWndFriendAdd :_ANPGGUIBasicWnd<GGUIMonoFriendAdd>
    {
        private static GGUIWndFriendAdd _g_instance;

        public static GGUIWndFriendAdd instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndFriendAdd();
                return _g_instance;
            }
        }
        private NPGGUIWndCommonToggleEx _m_addFriendTab;//推荐好友 页签
        private NPGGUIWndCommonToggleEx _m_requestFriendTab;// 好友申请页签

        private GGUIWndFriendRequestPage _m_requestPage;//好友申请页面
        private GGUIWndFriendRecommendPage _m_recommendPage;//推荐好友页面

        public GGUIWndFriendAdd() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoFriendAdd.assetPath; }
        protected override string _monoObjName { get => GGUIMonoFriendAdd.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            _m_addFriendTab?.discard();
            _m_addFriendTab = null;
            
            _m_requestFriendTab?.discard();
            _m_requestFriendTab = null;
            
            _m_requestPage?.discard();
            _m_requestPage = null;
            
            _m_recommendPage?.discard();
            _m_recommendPage = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
            if (null != wnd.addFriendTog)
            {
                _m_addFriendTab = new NPGGUIWndCommonToggleEx(wnd.addFriendTog);
                _m_addFriendTab.clickDelegate += clickAddFriend;
            }
            if (null != wnd.requestFriendTog)
            {
                _m_requestFriendTab = new NPGGUIWndCommonToggleEx(wnd.requestFriendTog);
                _m_requestFriendTab.clickDelegate += clickRequestFriend;
            }
        }

        /// <summary>
        /// 添加好友tab
        /// </summary>
        /// <param name="obj"></param>
        private void clickAddFriend(NPGGUIWndCommonToggleEx obj)
        {
            _m_requestFriendTab?.setSelected(false);
            _m_addFriendTab?.setSelected(true);
            
            if (null == _m_recommendPage)
            {
                _m_recommendPage = new GGUIWndFriendRecommendPage(wnd.pageParent);
                _m_recommendPage.regLoadDoneDelegate(() =>
                {
                    _m_recommendPage.showWnd();
                });
                _m_recommendPage.load();
            }
            else
            {
                _m_recommendPage.showWnd();
            }
            
            _m_requestPage?.hideWnd();
        }

        /// <summary>
        /// 好友申请tab
        /// </summary>
        /// <param name="obj"></param>
        private void clickRequestFriend(NPGGUIWndCommonToggleEx obj)
        {
            _m_requestFriendTab?.setSelected(true);
            _m_addFriendTab?.setSelected(false);
            if (null == _m_requestPage)
            {
                _m_requestPage = new GGUIWndFriendRequestPage(wnd.pageParent);
                _m_requestPage.regLoadDoneDelegate(() =>
                {
                    _m_requestPage.showWnd();
                });
                _m_requestPage.load();
            }
            else
            {
                _m_requestPage.showWnd();
            }
            _m_recommendPage?.hideWnd();
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="obj"></param>
        private void _clickClose(GameObject obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            // 如果有新的好友申请，先显示i好友申请页面
            _ARedTipNode applyRed = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_FRIEND_APPLY_ENTER);
            if (null != applyRed && applyRed.needShow())
            {
                _m_requestFriendTab?.clickSelectButton();
            }
            else
            {
                _m_addFriendTab?.clickSelectButton();
            }
        }
    }
}