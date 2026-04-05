using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 成就主窗口
    /// </summary>
    public class GGUIWndDailyMain : _ANPGGUIBasicWnd<GGUIMonoDailyMain>
    {
        private static GGUIWndDailyMain _g_instance = new GGUIWndDailyMain();
        public static GGUIWndDailyMain instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GGUIWndDailyMain();
                return _g_instance;
            }
        }

        //页签列表
        private List<GGUIWndDailyMainTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndDailyMainTab _m_wSelectTabWnd;
        //成就页面
        private GGUIWndAchievePage _m_wAchievePage;
        //每日任务页面
        private GGUIWndDailyQuestPage _m_wDailyQuestPage;

        public GGUIWndDailyMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoDailyMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoDailyMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _hideAllPage();
        }

        /// <summary>
        /// 选中指定页签
        /// </summary>
        /// <param name="_tab"></param>
        public void setSelectTab(EDailyTab _tab)
        {
            if (wnd == null || _m_lTabWndList == null)
                return;

            for (int i = 0; i < _m_lTabWndList.Count; i++)
            {
                if (_m_lTabWndList[i] == null)
                    continue;

                if (_m_lTabWndList[i].tabType == _tab)
                {
                    _onTabSelect(_m_lTabWndList[i]);
                    break;
                }
            }
            
        }
        
        protected override void _onReset()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndDailyMainTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.resetWnd();
                }
            }

            if (_m_wAchievePage != null)
                _m_wAchievePage.resetWnd();

            if (_m_wDailyQuestPage != null)
                _m_wDailyQuestPage.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_lTabWndList != null)
            {
                GGUIWndDailyMainTab tempTabItem = null;
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

            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;

            if (_m_wAchievePage != null)
                _m_wAchievePage.discard();
            _m_wAchievePage = null;

            if (_m_wDailyQuestPage != null)
                _m_wDailyQuestPage.discard();
            _m_wDailyQuestPage = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndDailyMainTab>();
            if (null != wnd.monoTabList)
            {
                GGUIDailyTabMono tempTabMono = null;
                GGUIWndDailyMainTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndDailyMainTab(tempTabMono.monoTab, tempTabMono.tabType, tempTabMono.tabSubWndAssetId);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            _m_wSelectTabWnd = null;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }


        public void resetSelect()
        {
            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_lTabWndList == null || _m_lTabWndList.Count <= 0)
                return;

            if (_m_wSelectTabWnd == null)
            {
                //按照优先级排序页签，取第一个展示
                GGUIWndDailyMainTab targetTab = null;
                _m_lTabWndList.Sort(_sortTabList);
                targetTab = _m_lTabWndList[0];

                if (targetTab != null)
                {
                    targetTab.setSelected(true);
                    _m_wSelectTabWnd = targetTab;
                    _refreshTabView(targetTab.tabType);
                }
            }
            else
            {
                _m_wSelectTabWnd.setSelected(true);
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndDailyMainTab _tabItemWnd)
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
        private void _refreshTabView(EDailyTab _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case EDailyTab.ACHIEVE:
                    _showAchievePage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_MAIN_DAILY_ACHIEVE);
                    break;
                case EDailyTab.DAILY_QUEST:
                    _showDailyQuestPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_MAIN_DAILY_QUEST);
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            if (null != _m_wAchievePage)
                _m_wAchievePage.hideWnd();

            if (null != _m_wDailyQuestPage)
                _m_wDailyQuestPage.hideWnd();
        }

        //显示成就详情页面
        private void _showAchievePage()
        {
            if (wnd == null || _m_wSelectTabWnd == null)
                return;

            if (_m_wAchievePage != null)
            {
                _m_wAchievePage.showWnd();
            }
            else
            {
                _m_wAchievePage = new GGUIWndAchievePage(NPCommonAssetPathInfo.readFromUiResId(_m_wSelectTabWnd.subWndAssetId), wnd.pageParent);
                _m_wAchievePage.load(_m_wAchievePage.showWnd);
            }
        }

        //显示每日任务详情页面
        private void _showDailyQuestPage()
        {
            if (wnd == null || _m_wSelectTabWnd == null)
                return;

            if (_m_wDailyQuestPage != null)
            {
                _m_wDailyQuestPage.showWnd();
            }
            else
            {
                _m_wDailyQuestPage = new GGUIWndDailyQuestPage(NPCommonAssetPathInfo.readFromUiResId(_m_wSelectTabWnd.subWndAssetId), wnd.pageParent);
                _m_wDailyQuestPage.load(_m_wDailyQuestPage.showWnd);
            }
        }

        //排序，是否解锁-->是否有奖励-->排序id
        private int _sortTabList(GGUIWndDailyMainTab _a, GGUIWndDailyMainTab _b)
        {
            int isUnlock = _a.isUnlock.CompareTo(_b.isUnlock);
            if (isUnlock != 0)
                return -isUnlock;

            int haveReward = _a.haveReward.CompareTo(_b.haveReward);
            if (haveReward != 0)
                return -haveReward;

            return _a.sortId.CompareTo(_b.sortId);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickCloseBtn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MAIN_DAILY);
        }
    }
}
