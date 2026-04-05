using GOE;
using System.Collections.Generic;

namespace Hotfix
{
    /// <summary>
    /// 万能活动附加排行榜界面
    /// </summary>
    public class GGUIWndRegularEventSubRank : _AHotfixBaseSubWnd<GGUIMonoRegularEventSubRank>
    {
        //活动id
        private long _m_lActivityId;
        //页签列表
        private List<GGuiWndRegularEventSubRankTab> _m_lTabWndList;
        //当前选中的页签
        private GGuiWndRegularEventSubRankTab _m_wSelectTabWnd;
        //本服排行榜页面
        private GGUIWndRegularEventSubRankSelfPage _m_wSelfPage;

        public GGUIWndRegularEventSubRank(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
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
                GGuiWndRegularEventSubRankTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    //重置状态
                    tempTabItem.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGuiWndRegularEventSubRankTab tempTabItem = null;
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
        }

        protected override void _onWndInitDoneHotfix()
        {
            //初始化页签列表
            _m_lTabWndList = new List<GGuiWndRegularEventSubRankTab>();
            GGuiWndRegularEventSubRankTab tempItem = null;

            if (hotfixWnd.monoTabList != null)
            {
                for (int i = 0; i < hotfixWnd.monoTabList.Count; i++)
                {
                    if (hotfixWnd.monoTabList[i] == null)
                        continue;
                    //创建页签对象
                    tempItem = new GGuiWndRegularEventSubRankTab(hotfixWnd.monoTabList[i].monoTab, hotfixWnd.monoTabList[i].tabTypeStr);
                    //初始化默认未选中
                    tempItem.setSelected(false);
                    //绑定点击事件
                    tempItem.onClickButton += _onTabSelect;
                    _m_lTabWndList.Add(tempItem);
                }
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(long _activityId)
        {
            _m_lActivityId = _activityId;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (hotfixWnd == null)
                return;

            //刷新默认页签
            if(_m_wSelectTabWnd == null)
                _refreshPageWnd(hotfixWnd.defaultTab);
            else
                _refreshPageWnd(_m_wSelectTabWnd.tabTag);
        }

        /// <summary>
        /// 刷新页签窗口
        /// </summary>
        private void _refreshPageWnd(string _selectTabType)
        {
            if (null == wnd)
                return;

            //设置选中页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGuiWndRegularEventSubRankTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabTag == _selectTabType)
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
                _refreshTabView(_m_wSelectTabWnd.tabTag);
            }
        }

        #region 页签页面处理

        //点击切换页签按钮
        private void _onTabSelect(GGuiWndRegularEventSubRankTab _tabItemWnd)
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
                _refreshTabView(_m_wSelectTabWnd.tabTag);
            }
        }

        //根据页签刷新列表内容
        private void _refreshTabView(string _tabView)
        {
            _hideAllPage();

            switch (_tabView?.ToUpperInvariant())
            {
                case RegularEventRankTabType.SELF_RANK://本服
                    _showSelfRankPage();
                    break;
                case RegularEventRankTabType.CROSS_RANK://跨服
                    _showCrossRankPage();
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wSelfPage?.hideWnd();
        }

        //显示本服页面
        private void _showSelfRankPage()
        {
            if (hotfixWnd == null)
                return;

            if (_m_wSelfPage != null)
            {
                _m_wSelfPage.showWnd();
                _m_wSelfPage.setInfo(_m_lActivityId);
            }
            else
            {
                _m_wSelfPage = new GGUIWndRegularEventSubRankSelfPage(_getPageAssetPathByType(RegularEventRankTabType.SELF_RANK), hotfixWnd.pageParent);
                _m_wSelfPage.load(() =>
                {
                    if (_m_wSelfPage == null)
                        return;

                    _m_wSelfPage.showWnd();
                    _m_wSelfPage.setInfo(_m_lActivityId);
                });
            }

        }

        //显示跨服页面
        private void _showCrossRankPage()
        {
        }


        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(string _type)
        {
            if (hotfixWnd == null || hotfixWnd.monoTabList == null)
                return null;

            foreach (GGUIMonoActivityCommonTab mono in hotfixWnd.monoTabList)
            {
                if (mono?.tabTypeStr == _type)
                    return NPCommonAssetPathInfo.readFromUiResId(mono.tabSubWndAssetId);
            }

            return null;
        }

        #endregion
    }
}