using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 限时活动主界面
    /// </summary>
    public class GGUIWndLimitActivityMain : _ANPGGUIBasicResBarWnd<GGUIMonoLimitActivityMain>
    {
        private static GGUIWndLimitActivityMain _g_instance;
        public static GGUIWndLimitActivityMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndLimitActivityMain();
                return _g_instance;
            }
        }

        //页签列表
        private List<GGUIWndLimitActivityMainTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndLimitActivityMainTab _m_wSelectTabWnd;
        //本服冲榜页面
        private GGUIWndRankRushPage _m_wRankRushPage;
        //阶段奖励页面
        private GGUIWndStepRewardPage _m_wStepRewardPage;

        public GGUIWndLimitActivityMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoLimitActivityMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoLimitActivityMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
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
                GGUIWndLimitActivityMainTab tempTabItem = null;
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

            _m_wRankRushPage?.resetWnd();
            _m_wStepRewardPage?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndLimitActivityMainTab tempTabItem = null;
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

            _m_wRankRushPage?.discard();
            _m_wRankRushPage = null;
            
            _m_wStepRewardPage?.discard();
            _m_wStepRewardPage = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndLimitActivityMainTab>();
            if (null != wnd.monoTabList)
            {
                GGUILimitActivityTabMono tempTabMono = null;
                GGUIWndLimitActivityMainTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndLimitActivityMainTab(tempTabMono.monoTab, tempTabMono.tabType);
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
        /// 设置信息
        /// </summary>
        /// <param name="_selectTabType"></param>
        public void setInfo(ELimitActivityTabType _selectTabType)
        {
            //重置选择
            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;

            foreach (GGUIWndLimitActivityMainTab itemTab in _m_lTabWndList)
            {
                if (itemTab?.tabType == _selectTabType)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取当前选中的页签类型
        /// </summary>
        /// <returns></returns>
        public ELimitActivityTabType getCurSelectTabType()
        {
            if (_m_wSelectTabWnd != null)
                return _m_wSelectTabWnd.tabType;

            return ELimitActivityTabType.RANK_RUSH;
        }

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndLimitActivityMainTab _tabItemWnd)
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
        private void _refreshTabView(ELimitActivityTabType _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case ELimitActivityTabType.CROSS_RANK_RUSH://跨服冲榜
                    break;
                case ELimitActivityTabType.RANK_RUSH://本服冲榜
                    _showRankRushPage();
                    break;
                case ELimitActivityTabType.STEP_REWARD://限时任务
                    _showStepRewardPage();
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wRankRushPage?.hideWnd();
            _m_wStepRewardPage?.hideWnd();
        }

        //显示本服冲榜页面
        private void _showRankRushPage()
        {
            if (wnd == null)
                return;
            
            if (_m_wRankRushPage != null)
            {
                _m_wRankRushPage.showWnd();
            }
            else
            {
                _m_wRankRushPage = new GGUIWndRankRushPage(_getPageAssetPathByType(ELimitActivityTabType.RANK_RUSH), wnd.pageParent);
                _m_wRankRushPage.load(() =>
                {
                    if (_m_wRankRushPage == null)
                        return;

                    _m_wRankRushPage.showWnd();
                });
            }
        }

        //显示本服冲榜页面
        private void _showStepRewardPage()
        {
            if (wnd == null)
                return;
            
            if (_m_wStepRewardPage != null)
            {
                _m_wStepRewardPage.showWnd();
            }
            else
            {
                _m_wStepRewardPage = new GGUIWndStepRewardPage(_getPageAssetPathByType(ELimitActivityTabType.STEP_REWARD), wnd.pageParent);
                _m_wStepRewardPage.load(() =>
                {
                    if (_m_wStepRewardPage == null)
                        return;

                    _m_wStepRewardPage.showWnd();
                });
            }
        }

        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(ELimitActivityTabType _type)
        {
            if (wnd == null || wnd.monoTabList == null)
                return null;

            foreach (GGUILimitActivityTabMono mono in wnd.monoTabList)
            {
                if (mono?.tabType == _type)
                    return NPCommonAssetPathInfo.readFromUiResId(mono.tabSubWndAssetId);
            }

            return null;
        }

        //点击关闭按钮
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_LIMIT_ACTIVITY_MAIN);
        }
    }
}
