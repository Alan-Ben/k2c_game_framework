using System.Collections.Generic;
using System.Linq;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 现金礼包主界面
    /// </summary>
    public class GGUIWndMarsCashGiftPackMain : _ANPGGUIBasicResBarWnd<GGUIMonoMarsCashGiftPackMain>
    {
        private static GGUIWndMarsCashGiftPackMain _g_instance;
        public static GGUIWndMarsCashGiftPackMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndMarsCashGiftPackMain();
                return _g_instance;
            }
        }

        private EMarsCashGiftPackMainTabType _m_eCurSelectTabType;
        //页签列表
        private List<GGUIWndMarsCashGiftPackMainTab> _m_lTabWndList;
        //建造队列页面
        private GGUIWndSingleCashGiftPackPage _m_wConstructionQueuePage;
        //行军队列页面
        private GGUIWndSingleCashGiftPackPage _m_wArmyQueuePage;
        //常驻礼包页面
        private GGUIWndCashGiftPackMainPage _m_wPermanentPage;

        public GGUIWndMarsCashGiftPackMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsCashGiftPackMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsCashGiftPackMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            wnd?.tabScrollRect?.onValueChanged?.AddListener(_onScrollRectValueChg);
            
            if (_m_eCurSelectTabType == EMarsCashGiftPackMainTabType.NONE)
            {
                if (wnd != null && wnd.monoTabList != null)
                    foreach (var tabMono in wnd.monoTabList)
                    {
                        if (tabMono == null || tabMono.redTip == null || tabMono.redTip.redTipList == null) continue;
                        foreach (var redTip in tabMono.redTip.redTipList)
                        {
                            if (redTip == null)
                                continue;

                            _ARedTipNode nodeItem = RedTipMgr.instance.getNodeByRefRedTipId(redTip.redTipId);
                            if (nodeItem == null)
                            {
                                continue;
                            }
                            bool needShow = nodeItem.needShow();
                            if(_m_eCurSelectTabType == EMarsCashGiftPackMainTabType.NONE && needShow)
                                _m_eCurSelectTabType = tabMono.tabType;
                        }
                    }
                if(_m_eCurSelectTabType == EMarsCashGiftPackMainTabType.NONE)
                    _m_eCurSelectTabType = EMarsCashGiftPackMainTabType.PERMANENT;
            }
            _refreshWnd();
            _moveTabWithinRange();

            //先重置一下左右红点tip显示
            wnd?.sideRedTipInfo?.hideRedTipGo();
            ALCommonActionMonoTask.addNextFrameTask(() => { _onScrollRectValueChg(Vector2.one); });
        }

        protected override void _onHideWnd()
        {
            wnd?.tabScrollRect?.onValueChanged?.RemoveAllListeners();
            _hideAllPage();
        }

        protected override void _onReset()
        {
            _m_wConstructionQueuePage?.resetWnd();
            _m_wArmyQueuePage?.resetWnd();
            _m_wPermanentPage?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndMarsCashGiftPackMainTab tempTabItem = null;
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
            
            _m_wArmyQueuePage?.discard();
            _m_wArmyQueuePage = null;

            _m_wConstructionQueuePage?.discard();
            _m_wConstructionQueuePage = null;

            _m_wPermanentPage?.discard();
            _m_wPermanentPage = null;

            GCashGiftPackGroupTitleCacheMgr.instance.discard();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.sideRedTipInfo?.btnLeftRedTip, _onClickLeftRedTipBtn);
            ALUGUICommon.uncombineBtnClick(wnd.sideRedTipInfo?.btnRightRedTip, _onClickRightRedTipBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndMarsCashGiftPackMainTab>();
            if (null != wnd.monoTabList)
            {
                GGUIMarsCashGiftPackMainTabMono tempTabMono = null;
                GGUIWndMarsCashGiftPackMainTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null)
                        continue;
                    tempTabItem = new GGUIWndMarsCashGiftPackMainTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.sideRedTipInfo?.btnLeftRedTip, _onClickLeftRedTipBtn);
            ALUGUICommon.combineBtnClick(wnd.sideRedTipInfo?.btnRightRedTip, _onClickRightRedTipBtn);
        }

        /// <summary>
        /// 设置选中页签
        /// </summary>
        /// <param name="_type"></param>
        public void setInfo(EMarsCashGiftPackMainTabType _type)
        {
            _m_eCurSelectTabType = _type;
        }
        
        private void _refreshInValidTabShow()
        { 
            if(wnd == null)
                return;
            NPPlayer.instance.giftPackComp.isGiftPackSellOut(wnd.constructionQueueGiftPackId);

            bool constructionQueueGiftPackSellOut = NPPlayer.instance.giftPackComp.isGiftPackSellOut(wnd.constructionQueueGiftPackId);
            bool armyQueueGiftPackSellOut = NPPlayer.instance.giftPackComp.isGiftPackSellOut(wnd.armyQueueGiftPackId);
            foreach (GGUIWndMarsCashGiftPackMainTab itemTab in _m_lTabWndList)
            {
                if (itemTab == null) continue;
                //如果队列礼包已买则隐藏tab
                if ((itemTab.tabType == EMarsCashGiftPackMainTabType.CONSTRUCTION_QUEUE && constructionQueueGiftPackSellOut) 
                    || itemTab.tabType == EMarsCashGiftPackMainTabType.ARMY_QUEUE && armyQueueGiftPackSellOut)
                    itemTab.hideWnd();
                else
                    itemTab.showWnd();
            }
            //如果选中建造队列礼包但没有建造队列礼包了，重置选择
            if (constructionQueueGiftPackSellOut && armyQueueGiftPackSellOut)
            {
                _m_eCurSelectTabType = EMarsCashGiftPackMainTabType.PERMANENT;
            }
            else if (_m_eCurSelectTabType == EMarsCashGiftPackMainTabType.CONSTRUCTION_QUEUE && constructionQueueGiftPackSellOut)
            {
                _m_eCurSelectTabType = EMarsCashGiftPackMainTabType.ARMY_QUEUE;
            }
            else if (_m_eCurSelectTabType == EMarsCashGiftPackMainTabType.ARMY_QUEUE && armyQueueGiftPackSellOut)
            {
                _m_eCurSelectTabType = EMarsCashGiftPackMainTabType.CONSTRUCTION_QUEUE;
            }
        }

        //刷新当前页签
        private void _refreshWnd()
        {
            _refreshInValidTabShow();
            _refreshTabSelectState();
            _refreshTabView(_m_eCurSelectTabType);
        }
        

        //刷新页签选择状态
        private void _refreshTabSelectState()
        {
            if (_m_lTabWndList == null)
                return;

            foreach (var itemTab in _m_lTabWndList)
            {
                if (itemTab == null)
                    continue;

                if (itemTab.tabType == _m_eCurSelectTabType)
                    itemTab.setSelected(true);
                else
                    itemTab.setSelected(false);
            }
        }

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndMarsCashGiftPackMainTab _tabItemWnd)
        {
            if (null == _tabItemWnd || null == wnd)
                return;

            //选择新页签
            if (_m_eCurSelectTabType != _tabItemWnd.tabType)
            {
                _m_eCurSelectTabType = _tabItemWnd.tabType;
                _refreshWnd();
            }

            //移动选中页签到可视范围内
            _moveTabWithinRange();
        }

        //根据页签刷新列表内容
        private void _refreshTabView(EMarsCashGiftPackMainTabType _tabView)
        {
            _hideAllPage();
            switch (_tabView)
            {
                case EMarsCashGiftPackMainTabType.NONE:
                    break;
                case EMarsCashGiftPackMainTabType.CONSTRUCTION_QUEUE:
                    _showConstructionQueuePage();
                    break;
                case EMarsCashGiftPackMainTabType.ARMY_QUEUE:
                    _showArmyQueuePage();
                    break;
                case EMarsCashGiftPackMainTabType.PERMANENT:
                    _showPermanentPage();
                    break;
            }

            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.emptyShowGoList, _tabView == EMarsCashGiftPackMainTabType.NONE);
        }

        //移动选中页签到可视范围内
        private void _moveTabWithinRange()
        {
            if (_m_lTabWndList == null)
                return;

            GGUIWndMarsCashGiftPackMainTab targetTab = null;
            foreach (GGUIWndMarsCashGiftPackMainTab itemTab in _m_lTabWndList)
            {
                if (itemTab != null && itemTab.tabType == _m_eCurSelectTabType)
                {
                    targetTab = itemTab;
                    break;
                }
            }
            if (targetTab != null)
                GCommon.setContainerMoveItemWithinRangeInHorizontal(targetTab?.rectTransform, (RectTransform)wnd?.tabScrollRect?.transform, wnd?.tabContentTransform, wnd?.moveItemAdditionDistanceParam);
        }

        //建造队列页面
        private void _showConstructionQueuePage()
        {
            if (wnd == null)
                return;

            if (_m_wConstructionQueuePage != null)
            {
                _m_wConstructionQueuePage.setInfo(wnd.constructionQueueGiftPackId);
                _m_wConstructionQueuePage.showWnd();
            }
            else
            {
                _m_wConstructionQueuePage = new GGUIWndSingleCashGiftPackPage(_getPageAssetPathIdByType(EMarsCashGiftPackMainTabType.CONSTRUCTION_QUEUE), wnd.pageParent);
                _m_wConstructionQueuePage.setInfo(wnd.constructionQueueGiftPackId);
                _m_wConstructionQueuePage.load(() =>
                {
                    if (_m_wConstructionQueuePage == null)
                        return;
                    _m_wConstructionQueuePage.showWnd();
                });
            }
        }

        //行军队列页面
        private void _showArmyQueuePage()
        {
            if (wnd == null)
                return;
            if (_m_wArmyQueuePage != null)
            {
                _m_wArmyQueuePage.setInfo(wnd.armyQueueGiftPackId);
                _m_wArmyQueuePage.showWnd();
            }
            else
            {
                _m_wArmyQueuePage = new GGUIWndSingleCashGiftPackPage(_getPageAssetPathIdByType(EMarsCashGiftPackMainTabType.ARMY_QUEUE), wnd.pageParent);
                _m_wArmyQueuePage.setInfo(wnd.armyQueueGiftPackId);
                _m_wArmyQueuePage.load(() =>
                {
                    if (_m_wArmyQueuePage == null)
                        return;
                    _m_wArmyQueuePage.showWnd();
                });
            }
        }
        
        //显示常驻礼包页面
        private void _showPermanentPage()
        {
            if (wnd == null)
                return;

            if (_m_wPermanentPage != null)
            {
                _m_wPermanentPage.showWnd();
                _m_wPermanentPage.setInfo(_getGiftPackGroupListByType(NPPlayer.instance.giftPackComp.marsPermanentPageGroupTypeList));
            }
            else
            {
                _m_wPermanentPage = new GGUIWndCashGiftPackMainPage(_getPageAssetPathIdByType(EMarsCashGiftPackMainTabType.PERMANENT), wnd.pageParent);
                _m_wPermanentPage.load(() =>
                {
                    if (_m_wPermanentPage == null)
                        return;
                    _m_wPermanentPage.showWnd();
                    _m_wPermanentPage.setInfo(_getGiftPackGroupListByType(NPPlayer.instance.giftPackComp.marsPermanentPageGroupTypeList));
                });
            }
        }

        //隐藏所有页面
        private void _hideAllPage()
        {
            _m_wConstructionQueuePage?.hideWnd();
            _m_wArmyQueuePage?.hideWnd();
            _m_wPermanentPage?.hideWnd();
        }

        //根据页签类型获取对应的子页面的加载路径
        private long _getPageAssetPathIdByType(EMarsCashGiftPackMainTabType _type)
        {
            if (wnd == null || wnd.monoTabList == null)
                return 0;

            foreach (GGUIMarsCashGiftPackMainTabMono mono in wnd.monoTabList)
            {
                if (mono.tabType == _type)
                    return mono.resId;
            }

            return 0;
        }

        /// <summary>
        /// 根据类型获取对应的礼包组列表
        /// </summary>
        /// <param name="_typeList"></param>
        /// <returns></returns>
        private List<GiftPackGroupRefObj> _getGiftPackGroupListByType(params EGiftPackGroupShowType[] _typeList)
        {
            List<GiftPackGroupRefObj> targetList = new List<GiftPackGroupRefObj>();
            GRefdataCoreMgr.instance.giftPackGroupRefCore.dealAllRef(_groupRef =>
            {
                if(_groupRef != null && _typeList.Contains(_groupRef.show_type))
                    targetList.Add(_groupRef);
            });

            return targetList;
        }

        #region 点击事件

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_CASH_GIFT_PACK_MAIN);
        } 
        
        /// <summary>
        /// 点击左侧跳转红点按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickLeftRedTipBtn(GameObject _go)
        {
            if (wnd == null || wnd.sideRedTipInfo == null || _m_lTabWndList == null)
                return;

            int targetIndex = wnd.sideRedTipInfo.findFirstRedTipItemIndexOutOfContaienr(_m_lTabWndList, (RectTransform)wnd?.tabScrollRect?.transform, true);

            if (targetIndex > -1 && _m_lTabWndList.Count > targetIndex)
                _onTabSelect(_m_lTabWndList[targetIndex]);
        }

        /// <summary>
        /// 点击右侧跳转红点按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickRightRedTipBtn(GameObject _go)
        {
            if (wnd == null || wnd.sideRedTipInfo == null || _m_lTabWndList == null)
                return;

            int targetIndex = wnd.sideRedTipInfo.findFirstRedTipItemIndexOutOfContaienr(_m_lTabWndList, (RectTransform)wnd?.tabScrollRect?.transform, false);

            if (targetIndex > -1 && _m_lTabWndList.Count > targetIndex)
                _onTabSelect(_m_lTabWndList[targetIndex]);
        }

        #endregion

        //列表滚动事件
        private void _onScrollRectValueChg(Vector2 _arg)
        {
            //刷新列表左右两边红点提示
            wnd?.sideRedTipInfo?.refreshContainerSideRedTip(_m_lTabWndList, (RectTransform)wnd?.tabScrollRect?.transform);
        }
    }
}