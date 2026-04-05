using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndDungeonRank : _ANPGGUIBasicResBarWnd<GGUIMonoDungeonRank>
    {
        private static GGUIWndDungeonRank _g_instance;
        public static GGUIWndDungeonRank instance { get { return _g_instance ??= new GGUIWndDungeonRank(); } }
        
        private EDungeonRankTab _m_eSelectTab; // 选中的页签
        private EEveningDungeonRankAndRewardDetailTabType _m_eSelectEveningDungeonTab; // 晚间副本选中的页签

        [NotNull] private Dictionary<EDungeonRankTab, GGUIWndDungeonRankTab> _m_dTabDic = new Dictionary<EDungeonRankTab, GGUIWndDungeonRankTab>();//页签字典

        private GGUIPrefabSubWndEveningDungeonRankAndRewardDetail _m_wEveningDungeonRankPage;//晚间活动排行页面
        private GGUIPrefabSubWndMiddayDungeonRank _m_wMiddayDungeonRankPage;//午间活动排行页面
        
        public GGUIWndDungeonRank() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoDungeonRank.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoDungeonRank.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_eSelectTab = wnd.defaultSelectTab;
            _m_eSelectEveningDungeonTab = wnd.defaultSelectEveningDungeonTab;

            _initAllTab();
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
            
            _discardAllTab();
        }
        
        protected override void _onShowWnd()
        {
            _dealAllTab((tabWnd) =>
            {
                tabWnd?.showWnd();
            });
            
            setSelectTab(_m_eSelectTab, true);
        }

        protected override void _onHideWnd()
        {
            _dealAllTab((tabWnd) =>
            {
                tabWnd?.hideWnd();
            });
            
            _hideAllTabPage();
        }

        protected override void _onReset()
        {
            _dealAllTab((tabWnd) =>
            {
                tabWnd?.resetWnd();
            });
            
            _resetAllTabPage();
        }

        /// <summary>
        /// 设置选中页签
        /// </summary>
        public void setSelectTab(EDungeonRankTab _tab, bool _forceRefresh)
        {
            if(_m_eSelectTab == _tab && !_forceRefresh)
                return;
            
            if (_m_dTabDic.TryGetValue(_m_eSelectTab, out GGUIWndDungeonRankTab _tabWnd))
            {
                _tabWnd?.setSelected(false);
            }

            _m_eSelectTab = _tab;
            if (_m_dTabDic.TryGetValue(_m_eSelectTab, out _tabWnd))
            {
                _tabWnd?.setSelected(true);
            }
            
            _showTabPage(_m_eSelectTab);
        }
        
        /// <summary>
        /// 设置选中晚间副本页签
        /// </summary>
        /// <param name="_tabType"></param>
        public void setSelectEveningDungeonTab(EEveningDungeonRankAndRewardDetailTabType _tabType)
        {
            _m_eSelectEveningDungeonTab = _tabType;

            setSelectTab(EDungeonRankTab.EVENING, true);// 强制切换到晚间副本页签
        }
        
        /// <summary>
        /// 点击返回按钮
        /// </summary>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DUNGEON_RANK);
        }

        #region 页签

        /// <summary>
        /// 销毁所有tab, tab的page也一起销毁
        /// </summary>
        private void _discardAllTab()
        {
            _dealAllTab((tabWnd) =>
            {
                if (tabWnd != null)
                {
                    tabWnd.onClickTab -= _onTabClick;
                    tabWnd.discard();
                }
            });
            _m_dTabDic.Clear();
            
            _discardAllTabPage();
        }

        /// <summary>
        /// 初始化所有tab
        /// </summary>
        private void _initAllTab()
        {
            _discardAllTab();
            if(wnd == null || wnd.tabList == null)
                return;

            GGUIWndDungeonRankTab tabWnd = null;
            foreach (var tabMono in wnd.tabList)
            {
                if(tabMono == null)
                    continue;

                if (_m_dTabDic.ContainsKey(tabMono.tabType))
                {
                    Debug.LogError($"[GGUIWndDungeonRank _initAllTab] 窗口tabList重复配置了页签类型：{tabMono.tabType}", wnd);
                    continue;
                }

                tabWnd = new GGUIWndDungeonRankTab(tabMono.monoTab, tabMono.tabType);
                tabWnd.onClickTab += _onTabClick;

                _m_dTabDic[tabMono.tabType] = tabWnd;
                
                // 初始化页签page
                _initTabPage(tabMono.tabType, tabMono.tabAssetPathInfo);
            }
        }
        
        private void _dealAllTab(Action<GGUIWndDungeonRankTab> _action)
        {
            if(_action == null)
                return;

            foreach (GGUIWndDungeonRankTab tab in _m_dTabDic.Values)
            {
                if (tab != null)
                    _action(tab);
            }
        }

        private void _onTabClick(GGUIWndDungeonRankTab _tabWnd)
        {
            if(_tabWnd == null)
                return;
            
            setSelectTab(_tabWnd.tabType, false);
        }
        
        private void _discardAllTabPage()
        {
            _m_wEveningDungeonRankPage?.discard();
            _m_wEveningDungeonRankPage = null;
            
            _m_wMiddayDungeonRankPage?.discard();
            _m_wMiddayDungeonRankPage = null;
        }

        private void _hideAllTabPage()
        {
            if (_m_wEveningDungeonRankPage != null)
            {
                _m_eSelectEveningDungeonTab = _m_wEveningDungeonRankPage.getCurSelectTabType();
                _m_wEveningDungeonRankPage.hideWnd();    
            }
            
            _m_wMiddayDungeonRankPage?.hideWnd();
        }
        
        private void _resetAllTabPage()
        {
            _m_wEveningDungeonRankPage?.resetWnd();
            _m_wMiddayDungeonRankPage?.resetWnd();
        }

        private void _initTabPage(EDungeonRankTab _tabType, NPCommonAssetPathInfo _pageAssetPathInfo)
        {
            switch (_tabType)
            {
                case EDungeonRankTab.MIDDAY:
                    _m_wMiddayDungeonRankPage?.discard();
                    _m_wMiddayDungeonRankPage = new GGUIPrefabSubWndMiddayDungeonRank(_pageAssetPathInfo, wnd == null ? rectTransform : wnd.tabPageParent);
                    _m_wMiddayDungeonRankPage.load();
                    break;
                
                case EDungeonRankTab.EVENING:
                    _m_wEveningDungeonRankPage?.discard();
                    _m_wEveningDungeonRankPage = new GGUIPrefabSubWndEveningDungeonRankAndRewardDetail(_pageAssetPathInfo, wnd == null ? rectTransform : wnd.tabPageParent);
                    _m_wEveningDungeonRankPage.load();
                    break;
            }
        }

        private void _showTabPage(EDungeonRankTab _tabType)
        {
            _hideAllTabPage();
            
            switch (_tabType)
            {
                case EDungeonRankTab.MIDDAY:
                    _m_wMiddayDungeonRankPage?.regLoadDoneDelegate(() =>
                    {
                        _m_wMiddayDungeonRankPage?.showWnd();
                    });
                    break;
                
                case EDungeonRankTab.EVENING:
                    _m_wEveningDungeonRankPage?.regLoadDoneDelegate(() =>
                    {
                        if (_m_wEveningDungeonRankPage != null)
                        {
                            _m_wEveningDungeonRankPage.showWnd();
                            _m_wEveningDungeonRankPage.setInfo(_m_eSelectEveningDungeonTab);
                        }
                    });
                    break;
            }
        }

        #endregion
    }
}