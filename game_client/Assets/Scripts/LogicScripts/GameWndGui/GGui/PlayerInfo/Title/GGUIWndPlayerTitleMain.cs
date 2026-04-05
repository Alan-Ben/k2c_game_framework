using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家称号详情界面
    /// </summary>
    public class GGUIWndPlayerTitleMain : _ANPGGUIBasicWnd<GGUIMonoPlayerTitleMain>
    {
        private static GGUIWndPlayerTitleMain _g_instance;
        public static GGUIWndPlayerTitleMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndPlayerTitleMain();
                return _g_instance;
            }
        }

        //页签列表
        private List<GGUIWndPlayerTitleMainTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndPlayerTitleMainTab _m_wSelectTabWnd;
        //组合称号详情页面
        private GGUIWndPlayerTitleComboPage _m_wComboPage;
        //固定称号详情页面
        private GGUIWndPlayerTitleFixedPage _m_wFixedPage;
        //限时称号详情页面
        private GGUIWndPlayerTitleLimitedPage _m_wLimitedPage;

        public GGUIWndPlayerTitleMain() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPlayerTitleMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerTitleMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            //默认选中组合页面
            _refreshPageWnd(EPlayerTitleTabType.COMBO);
        }

        protected override void _onHideWnd()
        {
            _hideAllPage();
        }

        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndPlayerTitleMainTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    //重置状态
                    tempTabItem.resetWnd();
                }
            }

            _m_wSelectTabWnd = null;

            _m_wComboPage?.resetWnd();
            _m_wFixedPage?.resetWnd();
            _m_wLimitedPage?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndPlayerTitleMainTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.discard();
                }
                _m_lTabWndList.Clear();
                _m_lTabWndList = null;
            }

            _m_wSelectTabWnd = null;

            _m_wComboPage?.discard();
            _m_wComboPage = null;

            _m_wFixedPage?.discard();
            _m_wFixedPage = null;

            _m_wLimitedPage?.discard();
            _m_wLimitedPage = null;

            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndPlayerTitleMainTab>();
            if (null != wnd.monoTabList)
            {
                GGUIPlayerTitleMainTabMono tempTabMono = null;
                GGUIWndPlayerTitleMainTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndPlayerTitleMainTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 刷新页签窗口
        /// </summary>
        private void _refreshPageWnd(EPlayerTitleTabType _selectTabType)
        {
            if (null == wnd)
                return;

            //设置选中页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGUIWndPlayerTitleMainTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabType == _selectTabType)
                    {
                        _onTabSelect(itemTab);
                        break;
                    }
                }
            }
            else
            {
                _m_wSelectTabWnd.setSelected(true);
                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        #region 页签页面处理

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndPlayerTitleMainTab _tabItemWnd)
        {
            if (null == _tabItemWnd || _m_wSelectTabWnd == _tabItemWnd)
                return;

            //取消原来的选择
            if (null != _m_wSelectTabWnd)
                _m_wSelectTabWnd.setSelected(false);

            //设置新对象
            _m_wSelectTabWnd = _tabItemWnd;

            if (null != _m_wSelectTabWnd)
            {
                _m_wSelectTabWnd.setSelected(true);

                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        //根据页签刷新列表内容
        private void _refreshTabView(EPlayerTitleTabType _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case EPlayerTitleTabType.COMBO://组合
                    _showComboPage();
                    break;
                case EPlayerTitleTabType.FIXED://固定
                    _showFixedPage();
                    break;
                case EPlayerTitleTabType.LIMITED://限时
                    _showLimitedPage();
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wComboPage?.hideWnd();
            _m_wFixedPage?.hideWnd();
            _m_wLimitedPage?.hideWnd();
        }

        //显示组合称号页面
        private void _showComboPage()
        {
            if (wnd == null)
                return;
            
            if (_m_wComboPage != null)
            {
                _m_wComboPage.showWnd();
            }
            else
            {
                _m_wComboPage = new GGUIWndPlayerTitleComboPage(_getPageAssetPathByType(EPlayerTitleTabType.COMBO), wnd.pageParent);
                _m_wComboPage.load(() =>
                {
                    if (_m_wComboPage == null)
                        return;
                    _m_wComboPage.showWnd();
                });
            }
        }

        //显示固定称号页面
        private void _showFixedPage()
        {
            if (wnd == null)
                return;
            
            if (_m_wFixedPage != null)
            {
                _m_wFixedPage.showWnd();
            }
            else
            {
                _m_wFixedPage = new GGUIWndPlayerTitleFixedPage(_getPageAssetPathByType(EPlayerTitleTabType.FIXED), wnd.pageParent);
                _m_wFixedPage.load(() =>
                {
                    if (_m_wFixedPage == null)
                        return;
                    _m_wFixedPage.showWnd();
                });
            }
        }

        //显示限时称号页面
        private void _showLimitedPage()
        {
            if (wnd == null)
                return;

            if (_m_wLimitedPage != null)
            {
                _m_wLimitedPage.showWnd();
            }
            else
            {
                _m_wLimitedPage = new GGUIWndPlayerTitleLimitedPage(_getPageAssetPathByType(EPlayerTitleTabType.LIMITED), wnd.pageParent);
                _m_wLimitedPage.load(() =>
                {
                    if (_m_wLimitedPage == null)
                        return;
                    _m_wLimitedPage.showWnd();
                });
            }
        }

        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(EPlayerTitleTabType _type)
        {
            if (wnd == null || wnd.monoTabList == null)
                return null;

            foreach (var mono in wnd.monoTabList)
            {
                if (mono.tabType == _type)
                    return NPCommonAssetPathInfo.readFromUiResId(mono.tabSubWndAssetId);
            }

            return null;
        }

        #endregion

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_TITLE_NODE);
        }
    }
}