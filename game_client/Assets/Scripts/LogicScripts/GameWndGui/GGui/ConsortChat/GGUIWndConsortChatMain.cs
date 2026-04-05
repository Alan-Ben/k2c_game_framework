using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndConsortChatMain : _ATALBasicUIWnd<GGUIMonoConsortChatMain>
    {
        private static GGUIWndConsortChatMain _g_instance = new GGUIWndConsortChatMain();
    
        public static GGUIWndConsortChatMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndConsortChatMain();
                return _g_instance;
            }
        }
    
        private EConsortChatMainPage _m_curPage = EConsortChatMainPage.CONSORT_LIST; //当前页签
        private NPGGUIWndCommonTab _m_tabChat;
        private NPGGUIWndCommonTab _m_tabMoments;
        private NPGGUIWndCommonTab _m_tabConsortList;

        private GGUIWndConsortChatPageChat _m_wChatPage;
        private GGUIWndConsortChatPageMoments _m_wMomentsPage;
        private GGUIWndConsortMain _m_wConsortListPage;
        
        public bool isInviting { get { return _m_wConsortListPage != null && _m_wConsortListPage.isInviting; } }


        
        public GGUIWndConsortChatMain() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoConsortChatMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoConsortChatMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SWITCH_CONSORT_MAIN_TAB, _onSwitchTab);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_INVITE_SHOW_DONE, _refreshWnd);//妃子邀约完成后刷新界面，主要是为了刷新TabNodeTag. 触发引导
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SWITCH_CONSORT_MAIN_TAB, _onSwitchTab);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_INVITE_SHOW_DONE, _refreshWnd);//妃子邀约完成后刷新界面，主要是为了刷新TabNodeTag. 触发引导
            _m_wChatPage?.hideWnd();
            _m_wMomentsPage?.hideWnd();
            _m_wConsortListPage?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_wChatPage?.resetWnd();
            _m_wMomentsPage?.resetWnd();
            _m_wConsortListPage?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            if(_m_tabChat != null)
                _m_tabChat.discard();
            _m_tabChat = null;
            if(_m_tabMoments != null)
                _m_tabMoments.discard();
            _m_tabMoments = null;
            if(_m_tabConsortList != null)
                _m_tabConsortList.discard();
            _m_tabConsortList = null;
            
            _m_wChatPage?.discard();
            _m_wChatPage = null;
            _m_wMomentsPage?.discard();
            _m_wMomentsPage = null;
            _m_wConsortListPage?.discard();
            _m_wConsortListPage = null;
            
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (null != wnd.tabChat)
            {
                _m_tabChat = new NPGGUIWndCommonTab(wnd.tabChat);
                _m_tabChat.clickDelegate += _onClickTabChat;
            }
            if (null != wnd.tabMoments)
            {
                _m_tabMoments = new NPGGUIWndCommonTab(wnd.tabMoments);
                _m_tabMoments.clickDelegate += _onClickTabMoments;
            }
            if (null != wnd.tabConsortList)
            {
                _m_tabConsortList = new NPGGUIWndCommonTab(wnd.tabConsortList);
                _m_tabConsortList.clickDelegate += _onClickTabConsortList;
            }
        }

        /// <summary>
        /// 设置聊天页签的主页签和子页签
        /// </summary>
        /// <param name="_pageType"></param>
        /// <param name="_chatType"></param>
        public void setPageType(EConsortChatMainPage _pageType)
        {
            _m_curPage = _pageType;
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            _hideAllPage();
            switch (_m_curPage)
            {
                case EConsortChatMainPage.CHAT:
                    _m_tabChat?.setSelected(true);
                    _m_tabMoments?.setSelected(false);
                    _m_tabConsortList?.setSelected(false);
                    _showChatPage();
                    break;
                case EConsortChatMainPage.MOMENTS:
                    _m_tabChat?.setSelected(false);
                    _m_tabMoments?.setSelected(true);
                    _m_tabConsortList?.setSelected(false);
                    _showMomentsPage();
                    break;
                case EConsortChatMainPage.CONSORT_LIST:
                    _m_tabChat?.setSelected(false);
                    _m_tabMoments?.setSelected(false);
                    _m_tabConsortList?.setSelected(true);
                    _showConsortListPage();
                    break;
            }
        }
        //关闭所有页面
        private void _hideAllPage()
        {
            if (null != _m_wChatPage)
                _m_wChatPage.hideWnd();

            if (null != _m_wMomentsPage)
                _m_wMomentsPage.hideWnd();
            
            if (null != _m_wConsortListPage)
                _m_wConsortListPage.hideWnd();
        }
        
        //显示聊天页面
        private void _showChatPage()
        {
            if (wnd == null)
                return;

            QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_CONSORT_CHAT_MAIN_CHAT_PAGE);
            if (_m_wChatPage != null)
            {
                _m_wChatPage.showWnd();
            }
            else
            {
                _m_wChatPage = new GGUIWndConsortChatPageChat(wnd.pageParent);
                _m_wChatPage.load(_m_wChatPage.showWnd);
            }
            _m_wChatPage.setPage();
        }

        //显示朋友圈页面
        private void _showMomentsPage()
        {
            if (wnd == null)
                return;

            QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_CONSORT_CHAT_MAIN_MOMENTS_PAGE);
            if (_m_wMomentsPage != null)
            {
                _m_wMomentsPage.showWnd();
            }
            else
            {
                _m_wMomentsPage = new GGUIWndConsortChatPageMoments(wnd.pageParent);
                _m_wMomentsPage.load(_m_wMomentsPage.showWnd);
            }
        }
        //显示朋友圈页面
        private void _showConsortListPage()
        {
            if (wnd == null)
                return;

            QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_CONSORT_CHAT_MAIN_CONSORT_LIST_PAGE);
            if(_m_wConsortListPage != null)
            {
                _m_wConsortListPage.showWnd();
            }
            else
            {
                _m_wConsortListPage = new GGUIWndConsortMain(wnd.pageParent);
                _m_wConsortListPage.load(_m_wConsortListPage.showWnd);
            }
        }
        
        private void _onClickTabChat(bool obj)
        {
            if(_m_curPage == EConsortChatMainPage.CHAT)
                return; //如果已经是聊天页签了，则不需要处理
            _m_curPage = EConsortChatMainPage.CHAT;
            _refreshWnd();
        }
        private void _onClickTabMoments(bool obj)
        {
            if(_m_curPage == EConsortChatMainPage.MOMENTS)
                return; //如果已经是朋友圈页签了，则不需要处理
            _m_curPage = EConsortChatMainPage.MOMENTS;
            _refreshWnd();
        }
        private void _onClickTabConsortList(bool obj)
        {
            if(_m_curPage == EConsortChatMainPage.CONSORT_LIST)
                return; //如果已经是宗门列表页签了，则不需要处理
            _m_curPage = EConsortChatMainPage.CONSORT_LIST;
            _refreshWnd();
        }

        /// <summary>
        /// 切换页签事件
        /// </summary>
        /// <param name="args"></param>
        private void _onSwitchTab(params object[] args)
        {
            if (args == null || args.Length < 1 || !(args[0] is string))
                return;

            string tabName = (string)args[0];
            switch (tabName)
            {
                case "CHAT":
                    _onClickTabChat(true);
                    break;
                case "MOMENTS":
                    _onClickTabMoments(true);
                    break;
                case "CONSORT_LIST":
                    _onClickTabConsortList(true);
                    break;
                default:
                    Debug.LogError("Unknown tab name: " + tabName);
                    return;
            }
        }

        /// <summary>
        /// 根据目标类型获取对应妃子的 RectTransform
        /// </summary>
        public RectTransform getTargetConsortRectTransform(EConsortMainTargetConsortType _type, string _params)
        {
            if (_m_wConsortListPage == null)
                return null;

            return _m_wConsortListPage.getTargetConsortRectTransform(_type, _params);
        }
    }
}