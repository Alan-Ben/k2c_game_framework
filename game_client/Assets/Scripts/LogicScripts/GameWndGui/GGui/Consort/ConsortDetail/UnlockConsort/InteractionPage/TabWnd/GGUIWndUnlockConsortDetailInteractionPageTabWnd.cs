using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndUnlockConsortDetailInteractionPageTab : _ATNPGGUIWndCommonTab<
        EUnlockConsortDetailWndInteractionPageTabType, GGUIWndUnlockConsortDetailInteractionPageTab>
    {
        public GGUIWndUnlockConsortDetailInteractionPageTab(NPGGUIMonoCommonTab _wnd, EUnlockConsortDetailWndInteractionPageTabType _bagItemType) : base(_wnd, _bagItemType)
        {
        }
    }
    
    public class GGUIWndUnlockConsortDetailInteractionPageTabWnd : _ANPGGUIBasicSubWnd<GGUIMonoUnlockConsortDetailInteractionPageTabWnd>
    {
        private GGottenConsortInfo _m_iConsortInfo;//已获取的妃子信息
        private _IGGUIWndUnlockConsortDetailInteractionPageParam _m_iPageParam;//页面参数
        
        private List<GGUIWndUnlockConsortDetailInteractionPageTab> _m_lTabList;
        
        private GGUIWndUnLockConsortDetailInteractionSendGiftPage _m_wSendGiftPage;//赠送礼物页
        private GGUIWndUnLockConsortDetailInteractionStoryPage _m_wStoryPage;//故事页 
        private GGUIWndUnLockConsortDetailInteractionTravelPage _m_wTravelPage;//旅行页
        
        public GGUIWndUnlockConsortDetailInteractionPageTabWnd(_IGGUIWndUnlockConsortDetailInteractionPageParam _pageParam, GGUIMonoUnlockConsortDetailInteractionPageTabWnd _wnd) : base(_wnd)
        {
            _m_iPageParam = _pageParam;
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (_m_lTabList == null)
                _m_lTabList = new List<GGUIWndUnlockConsortDetailInteractionPageTab>();
            if (wnd.tabTypeSettingList != null)
            {
                foreach (var tabSetting in wnd.tabTypeSettingList)
                {
                    if(tabSetting == null || tabSetting.tabType == EUnlockConsortDetailWndInteractionPageTabType.NONE)
                        continue;

                    GGUIWndUnlockConsortDetailInteractionPageTab tabWnd = _getTabWnd(tabSetting.tabType);
                    if (tabWnd != null)
                    {
                        Debug.LogError($"GGUIWndUnlockConsortDetailInteractionPageTabWnd._onWndInitDone: tabType:{tabSetting.tabType} 重复配置", wnd);
                        continue;
                    }
                    
                    tabWnd = new GGUIWndUnlockConsortDetailInteractionPageTab(tabSetting.tabMono, tabSetting.tabType);
                    tabWnd.onClickTab += _onTabClick;
                    _m_lTabList.Add(tabWnd);
                    
                    _initTabPage(tabSetting);
                }
            }
        }
        
        protected override void _onDiscard()
        {
            if (_m_lTabList != null)
            {
                foreach (var tabWnd in _m_lTabList)
                {
                    if(tabWnd == null)
                        continue;
                    
                    tabWnd.onClickTab -= _onTabClick;
                    tabWnd.discard();
                }
                _m_lTabList?.Clear();
            }
            _m_lTabList = null;

            _discardAllTabPage();
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CONSORT_TRAVEL, _onSimulateClickOpenConsortTravel);

            if (_m_lTabList != null)
            {
                foreach (var tabWnd in _m_lTabList)
                {
                    tabWnd?.showWnd();
                }
            }
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CONSORT_TRAVEL, _onSimulateClickOpenConsortTravel);

            if (_m_lTabList != null)
            {
                foreach (var tabWnd in _m_lTabList)
                {
                    tabWnd?.hideWnd();
                }
            }

            _hideAllTabPage();
        }

        protected override void _onReset()
        {
            if (_m_lTabList != null)
            {
                foreach (var tabWnd in _m_lTabList)
                {
                    tabWnd?.resetWnd();
                }
            }
            
            _m_wSendGiftPage?.resetWnd();
            _m_wStoryPage?.resetWnd();
            _m_wTravelPage?.resetWnd();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void setData(GGottenConsortInfo _consortInfo)
        {
            _m_iConsortInfo = _consortInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            selectTab(_m_iPageParam?.interactionPageTabType ?? EUnlockConsortDetailWndInteractionPageTabType.NONE, true);
        }
        
        public void onlyShowActor(bool _show)
        {
            if(_m_wSendGiftPage != null && _m_wSendGiftPage.isShow)
                _m_wSendGiftPage.onlyShowActor(_show);
            
            if(_m_wStoryPage != null && _m_wStoryPage.isShow)
                _m_wStoryPage.onlyShowActor(_show);
            
            if(_m_wTravelPage != null && _m_wTravelPage.isShow)
                _m_wTravelPage.onlyShowActor(_show);
        }
        
        private GGUIWndUnlockConsortDetailInteractionPageTab _getTabWnd(EUnlockConsortDetailWndInteractionPageTabType _tabType)
        {
            if (_m_lTabList == null)
                return null;

            foreach (GGUIWndUnlockConsortDetailInteractionPageTab tabWnd in _m_lTabList)
            {
                if (tabWnd != null && tabWnd.tabType == _tabType)
                    return tabWnd;
            }

            return null;
        }
        
        /// <summary>
        /// 当页签被点击时
        /// </summary>
        /// <param name="_tabWnd"></param>
        private void _onTabClick(GGUIWndUnlockConsortDetailInteractionPageTab _tabWnd)
        {
            if(_tabWnd == null)
                return;
            
            selectTab(_tabWnd.tabType, false);
        }

        /// <summary>
        /// 模拟点击打开妃子出游弹窗
        /// </summary>
        private void _onSimulateClickOpenConsortTravel()
        {
            if (_m_iConsortInfo == null)
                return;

            selectTab(EUnlockConsortDetailWndInteractionPageTabType.TRAVEL, false);
        }
        
        /// <summary>
        /// 选中页签
        /// </summary>
        /// <param name="_tabType"></param>
        public void selectTab(EUnlockConsortDetailWndInteractionPageTabType _tabType, bool _forceSelect)
        {
            if (_m_lTabList == null)
                return;

            if(_m_iPageParam != null && _m_iPageParam.interactionPageTabType == _tabType && !_forceSelect)
                return;
            
            if(_m_iPageParam != null)
                _m_iPageParam.interactionPageTabType = _tabType;
            
            foreach (GGUIWndUnlockConsortDetailInteractionPageTab tabWnd in _m_lTabList)
            {
                if (tabWnd != null)
                    tabWnd.setSelected(tabWnd.tabType == _tabType);
            }
            
            if(wnd != null)
                wnd.selectTab(_tabType);
            
            switch (_tabType)
            {
                case EUnlockConsortDetailWndInteractionPageTabType.STORY:
                    _showStoryPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_INTERACTION_PAGE_STORY);
                    break;
                case EUnlockConsortDetailWndInteractionPageTabType.TRAVEL:
                    _showTravelPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_INTERACTION_PAGE_TRAVEL);
                    break;
                case EUnlockConsortDetailWndInteractionPageTabType.SEND_GIFT:
                    _showSendGiftPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL_INTERACTION_PAGE_SEND_GIFT);
                    break;
                case EUnlockConsortDetailWndInteractionPageTabType.NONE:
                    _hideAllTabPage();
                    //默认算作打开detail
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_UNLOCK_CONSORT_DETAIL);
                    break;
            }
        }

        #region tabPage

        private void _initTabPage(UnlockConsortDetailWndInteractionPageTabTypeSetting _tabSetting)
        {
            if(_tabSetting == null || wnd == null)
                return;
            
            NPCommonAssetPathInfo tabPageAssetPathInfo = UIResPathAssistant.getAssetInfo(_tabSetting.ui_path_id);
            switch (_tabSetting.tabType)
            {
                case EUnlockConsortDetailWndInteractionPageTabType.SEND_GIFT:
                    if (_m_wSendGiftPage != null)
                    {
                        _m_wSendGiftPage.onCloseTabPageBtnClick -= _onTabPageCloseBtnClick;
                        _m_wSendGiftPage.discard();
                    }
                    _m_wSendGiftPage = new GGUIWndUnLockConsortDetailInteractionSendGiftPage(_m_iPageParam, tabPageAssetPathInfo, wnd.tabPageParent);
                    _m_wSendGiftPage.onCloseTabPageBtnClick += _onTabPageCloseBtnClick;
                    break;
                
                case EUnlockConsortDetailWndInteractionPageTabType.STORY:
                    if (_m_wStoryPage != null)
                    {
                        _m_wStoryPage.onCloseTabPageBtnClick -= _onTabPageCloseBtnClick;
                        _m_wStoryPage.discard();
                    }
                    _m_wStoryPage = new GGUIWndUnLockConsortDetailInteractionStoryPage(_m_iPageParam, tabPageAssetPathInfo, wnd.tabPageParent);
                    _m_wStoryPage.onCloseTabPageBtnClick += _onTabPageCloseBtnClick;
                    break;
                
                case EUnlockConsortDetailWndInteractionPageTabType.TRAVEL:
                    if (_m_wTravelPage != null)
                    {
                        _m_wTravelPage.onCloseTabPageBtnClick -= _onTabPageCloseBtnClick;
                        _m_wTravelPage.discard();
                    }
                    _m_wTravelPage = new GGUIWndUnLockConsortDetailInteractionTravelPage(_m_iPageParam, tabPageAssetPathInfo, wnd.tabPageParent);
                    _m_wTravelPage.onCloseTabPageBtnClick += _onTabPageCloseBtnClick;
                    break;
            }
        }
        
        private void _discardAllTabPage()
        {
            if (_m_wSendGiftPage != null)
            {
                _m_wSendGiftPage.discard();
                _m_wSendGiftPage.onCloseTabPageBtnClick -= _onTabPageCloseBtnClick;
            }

            if (_m_wStoryPage != null)
            {
                _m_wStoryPage.discard();
                _m_wStoryPage.onCloseTabPageBtnClick -= _onTabPageCloseBtnClick;
            }
            
            if (_m_wTravelPage != null)
            {
                _m_wTravelPage.discard();
                _m_wTravelPage.onCloseTabPageBtnClick -= _onTabPageCloseBtnClick;
            }
        }

        /// <summary>
        /// tabPage页面点击关闭
        /// </summary>
        private void _onTabPageCloseBtnClick(EUnlockConsortDetailWndInteractionPageTabType _tabType)
        {
            selectTab(EUnlockConsortDetailWndInteractionPageTabType.NONE, false);
        }
        
        /// <summary>
        /// 隐藏所有tabPage页面
        /// </summary>
        private void _hideAllTabPage()
        {
            _m_wStoryPage?.hideWnd();
            _m_wTravelPage?.hideWnd();
            _m_wSendGiftPage?.hideWnd();
        }
        
        /// <summary>
        /// 显示赠送礼物页
        /// </summary>
        private void _showSendGiftPage()
        {
            _hideAllTabPage();
            
            if (_m_wSendGiftPage == null)
            {
                Debug.LogError($"GGUIWndUnlockConsortDetailInteractionPageTabWnd._showSendGiftPage: _m_wSendGiftPage == null, 请检查是否没在init中进行初始化, 或者窗口上没有进行相应配置", wnd);
                return;
            }
         
            //如未加载则保持一次加载
            if(!_m_wSendGiftPage.isLoaded)
                _m_wSendGiftPage.load();
            
            _m_wSendGiftPage.regLoadDoneDelegate(() =>
            {
                if(!isShow || _m_wSendGiftPage == null)
                    return;
                
                _m_wSendGiftPage.showWnd();
                _m_wSendGiftPage.setData(_m_iConsortInfo);
            });
        }
        
        /// <summary>
        /// 显示故事页
        /// </summary>
        private void _showTravelPage()
        {
            _hideAllTabPage();
            
            if (_m_wTravelPage == null)
            {
                Debug.LogError($"GGUIWndUnlockConsortDetailInteractionPageTabWnd._showTravelPage: _m_wTravelPage == null, 请检查是否没在init中进行初始化, 或者窗口上没有进行相应配置", wnd);
                return;
            }

            //如未加载则保持一次加载
            if (!_m_wTravelPage.isLoaded)
                _m_wTravelPage.load();
            
            _m_wTravelPage.regLoadDoneDelegate(() =>
            {
                if(!isShow || _m_wTravelPage == null)
                    return;
                
                _m_wTravelPage.showWnd();
                _m_wTravelPage.setData(_m_iConsortInfo);
            });
        }
        
        /// <summary>
        /// 显示故事页
        /// </summary>
        private void _showStoryPage()
        {
            _hideAllTabPage();
            
            if (_m_wStoryPage == null)
            {
                Debug.LogError($"GGUIWndUnlockConsortDetailInteractionPageTabWnd._showStoryPage: _m_wStoryPage == null, 请检查是否没在init中进行初始化, 或者窗口上没有进行相应配置", wnd);
                return;
            }

            //如未加载则保持一次加载
            if (!_m_wStoryPage.isLoaded)
                _m_wStoryPage.load();
            
            _m_wStoryPage.regLoadDoneDelegate(() =>
            {
                if(!isShow || _m_wStoryPage == null)
                    return;
                
                _m_wStoryPage.showWnd();
                _m_wStoryPage.setData(_m_iConsortInfo);
            });
        }

        #endregion
    }
}