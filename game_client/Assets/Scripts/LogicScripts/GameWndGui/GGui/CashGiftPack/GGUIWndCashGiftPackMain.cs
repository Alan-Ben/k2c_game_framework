using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 现金礼包主界面
    /// </summary>
    public class GGUIWndCashGiftPackMain : _ANPGGUIBasicResBarWnd<GGUIMonoCashGiftPackMain>
    {
        private static GGUIWndCashGiftPackMain _g_instance;
        public static GGUIWndCashGiftPackMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndCashGiftPackMain();
                return _g_instance;
            }
        }

        //当前选中的页签类型
        private ECashGiftPackMainTabType _m_eCurSelectTabType;
        //特殊处理类型
        private ECashGiftPackSpecialDealType _m_eSpecialDealType;
        //页签列表
        private List<GGUIWndCashGiftPackMainTab> _m_lTabWndList;
        //钻石商店页面
        private GGUIWndCashGiftPackGemMainPage _m_wGemPage;
        //活动礼包页面
        private GGUIWndCashGiftPackMainPage _m_wActivityPage;
        //常驻礼包页面
        private GGUIWndCashGiftPackMainPage _m_wPermanentPage;
        //充值返利页面
        private GGUIWndRechargeRebatePage _m_wRechargeRebatePage;
        //权益卡页面
        private GGUIWndPrivilegeCardPage _m_wPrivilegeCardPage;
        //基金页面
        private GGUIWndFundPage _m_wFundPage;
        //指定的 tab id （只生效一次）
        private long _m_lFundTabId;

        public GGUIWndCashGiftPackMain() 
            : base(EALUIWndLayer.NORMAL)
        {
            _m_lFundTabId = -1;
        }

        protected override string _monoAssetPath { get { return GGUIMonoCashGiftPackMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoCashGiftPackMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_RECHARGE_REBATE_ADD, _onAddRemoveRechargeRebate);
            WinMsg.RegisterMsg(WinMsgType.ON_RECHARGE_REBATE_REMOVE, _onAddRemoveRechargeRebate);
            wnd?.tabScrollRect?.onValueChanged?.AddListener(_onScrollRectValueChg);

            //刷新界面
            _refreshWnd();
            _moveTabWithinRange();

            //先重置一下左右红点tip显示
            wnd?.sideRedTipInfo?.hideRedTipGo();
            ALCommonActionMonoTask.addNextFrameTask(() => { _onScrollRectValueChg(Vector2.one); });
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_RECHARGE_REBATE_ADD, _onAddRemoveRechargeRebate);
            WinMsg.UnregisterMsg(WinMsgType.ON_RECHARGE_REBATE_REMOVE, _onAddRemoveRechargeRebate);
            wnd?.tabScrollRect?.onValueChanged?.RemoveAllListeners();
            _hideAllPage();
        }

        protected override void _onReset()
        {
            _m_wGemPage?.resetWnd();
            _m_wActivityPage?.resetWnd();
            _m_wPermanentPage?.resetWnd();
            _m_wRechargeRebatePage?.resetWnd();
            _m_wPrivilegeCardPage?.resetWnd();
            _m_wFundPage?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndCashGiftPackMainTab tempTabItem = null;
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

            _m_wGemPage?.discard();
            _m_wGemPage = null;
            _m_wActivityPage?.discard();
            _m_wActivityPage = null;
            _m_wPermanentPage?.discard();
            _m_wPermanentPage = null;
            _m_wRechargeRebatePage?.discard();
            _m_wRechargeRebatePage = null;
            _m_wPrivilegeCardPage?.discard();
            _m_wPrivilegeCardPage = null;
            _m_wFundPage?.discard();
            _m_wFundPage = null;

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

            _m_lTabWndList = new List<GGUIWndCashGiftPackMainTab>();
            if (null != wnd.monoTabList)
            {
                GGUICashGiftPackMainTabMono tempTabMono = null;
                GGUIWndCashGiftPackMainTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null)
                        continue;
                    tempTabItem = new GGUIWndCashGiftPackMainTab(tempTabMono.monoTab, tempTabMono.tabType);
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
        /// 设置信息
        /// </summary>
        /// <param name="_type"></param>
        public void setInfo(ECashGiftPackMainTabType _type)
        {
            _m_eCurSelectTabType = _type;
        }

        /// <summary>
        /// 设置展示的特殊处理
        /// </summary>
        /// <param name="_type"></param>
        public void setSpecialDealType(ECashGiftPackSpecialDealType _type)
        {
            _m_eSpecialDealType = _type;
        }
        
        public void setFundPageId(long _fundTabId)
        {
            _m_lFundTabId = _fundTabId;
            if (_m_wFundPage != null)
            {
                _m_wFundPage.setFundPageId(_m_lFundTabId);
                _m_lFundTabId = -1;
            }
        }

        /// <summary>
        /// 设置选中页签
        /// </summary>
        /// <param name="_type"></param>
        public void setSelectTab(ECashGiftPackMainTabType _type)
        {
            if (wnd == null || _m_lTabWndList == null)
                return;

            foreach (GGUIWndCashGiftPackMainTab itemTab in _m_lTabWndList)
            {
                if (itemTab == null)
                    continue;

                if (itemTab.tabType == _type)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }
        }

        //刷新界面
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            _refreshRechargeRebateTabShow();
            _refreshFundTabShow();
            _refreshTabSelectState();
            _refreshTabView(_m_eCurSelectTabType);
        }

        //刷新充值返利页签显示隐藏及选择状态
        private void _refreshRechargeRebateTabShow()
        {
            //是否有检查充值返利
            bool haveRechargeRebate = _checkRechargeRebateTabShow();

            //如果选中充值返利但没有充值返利，重置选择
            if (!haveRechargeRebate && _m_eCurSelectTabType == ECashGiftPackMainTabType.RECHARGE_REBATE)
                _m_eCurSelectTabType = ECashGiftPackMainTabType.GEM;
        }

        //刷新基金页签显示隐藏及选择状态
        private void _refreshFundTabShow()
        {
            bool haveFund = _checkFundTabShow();
            
            //如果选中基金但没有基金，重置选择
            if (!haveFund && _m_eCurSelectTabType == ECashGiftPackMainTabType.FUND)
                _m_eCurSelectTabType = ECashGiftPackMainTabType.GEM;
        }

        //刷新页签选择状态
        private void _refreshTabSelectState()
        {
            if (_m_lTabWndList == null)
                return;

            foreach (GGUIWndCashGiftPackMainTab itemTab in _m_lTabWndList)
            {
                if (itemTab == null)
                    continue;

                if (itemTab.tabType == _m_eCurSelectTabType)
                    itemTab.setSelected(true);
                else
                    itemTab.setSelected(false);
            }
        }

        //检查并设置充值返利页签显示隐藏
        private bool _checkRechargeRebateTabShow()
        {
            if (_m_lTabWndList == null)
                return false;

            List<RechargeRebateInfo> infoList = NPPlayer.instance.rechargeRebateComp.getRechargeRebateInfoList();
            bool haveRechargeRebate = infoList != null && infoList.Count > 0;
            foreach (GGUIWndCashGiftPackMainTab itemTab in _m_lTabWndList)
            {
                if (itemTab is not { tabType: ECashGiftPackMainTabType.RECHARGE_REBATE })
                    continue;
                
                //如果没有充值返利则隐藏充值返利页签
                if (!haveRechargeRebate)
                    itemTab.hideWnd();
                else
                    itemTab.showWnd();
            }
            return haveRechargeRebate;
        }

        private bool _checkFundTabShow()
        {
            if (_m_lTabWndList == null)
                return false;

            bool haveFund = GCommon.isFuncUnlock(ENPFunctionType.ACTIVITY_FUND);
            foreach (GGUIWndCashGiftPackMainTab itemTab in _m_lTabWndList)
            {
                if (itemTab is not { tabType: ECashGiftPackMainTabType.FUND })
                    continue;
                
                //如果没有基金功能解锁则隐藏基金页签
                if (!haveFund)
                    itemTab.hideWnd();
                else
                    itemTab.showWnd();
            }
            return haveFund;
        }

        //根据页签刷新列表内容
        private void _refreshTabView(ECashGiftPackMainTabType _tabView)
        {
            _hideAllPage();
            switch (_tabView)
            {
                case ECashGiftPackMainTabType.GEM:
                    _showGemPage();
                    break;
                case ECashGiftPackMainTabType.ACTIVITY:
                    _showActivityPage();
                    break;
                case ECashGiftPackMainTabType.PERMANENT:
                    _showPermanentPage();
                    break;
                case ECashGiftPackMainTabType.RECHARGE_REBATE:
                    _showRechargeRebatePage();
                    break;
                case ECashGiftPackMainTabType.PRIVILEGE_CARD:
                    _showPrivilegeCardPage();
                    break;
                case ECashGiftPackMainTabType.FUND:
                    _showFundPage();
                    break;
            }

            //处理需要特殊处理的逻辑
            _dealSpecialDeal();
        }

        //移动选中页签到可视范围内
        private void _moveTabWithinRange()
        {
            if (_m_lTabWndList == null)
                return;

            GGUIWndCashGiftPackMainTab targetTab = null;
            foreach (GGUIWndCashGiftPackMainTab itemTab in _m_lTabWndList)
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

        //处理需要特殊处理的逻辑
        private void _dealSpecialDeal()
        {
            ECashGiftPackSpecialDealType curType = _m_eSpecialDealType;
            _m_eSpecialDealType = ECashGiftPackSpecialDealType.NONE;

            switch (curType)
            {
                case ECashGiftPackSpecialDealType.SHOW_MONTH_CARD_GUIDE_HAND:
                case ECashGiftPackSpecialDealType.SHOW_YEAR_CARD_GUIDE_HAND:
                    _m_wPrivilegeCardPage?.regLoadDoneDelegate(() =>
                    {
                        _m_wPrivilegeCardPage.showGuideHand(curType);
                    });
                    break;
            }
        }

        #region 页面展示

        //显示钻石商店页面
        private void _showGemPage()
        {
            if (wnd == null)
                return;

            if (_m_wGemPage != null)
                _m_wGemPage.showWnd();
            else
            {
                _m_wGemPage = new GGUIWndCashGiftPackGemMainPage(_getPageAssetPathIdByType(ECashGiftPackMainTabType.GEM), wnd.pageParent);
                _m_wGemPage.load(() =>
                {
                    if (_m_wGemPage == null)
                        return;
                    _m_wGemPage.showWnd();
                });
            }
        }

        //显示活动礼包页面
        private void _showActivityPage()
        {
            if (wnd == null)
                return;

            if (_m_wActivityPage != null)
            {
                _m_wActivityPage.showWnd();
                _m_wActivityPage.setInfo(_getGiftPackGroupListByType(NPPlayer.instance.giftPackComp.activityPageGroupTypeList));
            }
            else
            {
                _m_wActivityPage = new GGUIWndCashGiftPackMainPage(_getPageAssetPathIdByType(ECashGiftPackMainTabType.ACTIVITY), wnd.pageParent);
                _m_wActivityPage.load(() =>
                {
                    if (_m_wActivityPage == null)
                        return;
                    _m_wActivityPage.showWnd();
                    _m_wActivityPage.setInfo(_getGiftPackGroupListByType(NPPlayer.instance.giftPackComp.activityPageGroupTypeList));
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
                _m_wPermanentPage.setInfo(_getGiftPackGroupListByType(NPPlayer.instance.giftPackComp.permanentPageGroupTypeList));
            }
            else
            {
                _m_wPermanentPage = new GGUIWndCashGiftPackMainPage(_getPageAssetPathIdByType(ECashGiftPackMainTabType.PERMANENT), wnd.pageParent);
                _m_wPermanentPage.load(() =>
                {
                    if (_m_wPermanentPage == null)
                        return;
                    _m_wPermanentPage.showWnd();
                    _m_wPermanentPage.setInfo(_getGiftPackGroupListByType(NPPlayer.instance.giftPackComp.permanentPageGroupTypeList));
                });
            }
        }

        //显示充值返利页面
        private void _showRechargeRebatePage()
        {
            if (wnd == null)
                return;

            if (_m_wRechargeRebatePage != null)
            {
                _m_wRechargeRebatePage.showWnd();
            }
            else
            {
                _m_wRechargeRebatePage = new GGUIWndRechargeRebatePage(_getPageAssetPathIdByType(ECashGiftPackMainTabType.RECHARGE_REBATE), wnd.pageParent);
                _m_wRechargeRebatePage.load(() =>
                {
                    if (_m_wRechargeRebatePage == null)
                        return;
                    _m_wRechargeRebatePage.showWnd();
                });
            }
        }

        //显示充值返利页面
        private void _showPrivilegeCardPage()
        {
            if (wnd == null)
                return;

            if (_m_wPrivilegeCardPage != null)
            {
                _m_wPrivilegeCardPage.showWnd();
            }
            else
            {
                _m_wPrivilegeCardPage = new GGUIWndPrivilegeCardPage(_getPageAssetPathIdByType(ECashGiftPackMainTabType.PRIVILEGE_CARD), wnd.pageParent);
                _m_wPrivilegeCardPage.load(() =>
                {
                    if (_m_wPrivilegeCardPage == null)
                        return;
                    _m_wPrivilegeCardPage.showWnd();
                });
            }
        }

        //显示基金页面
        private void _showFundPage()
        {
            if (wnd == null)
                return;
            
            long fundTabId = _m_lFundTabId;
            _m_lFundTabId = -1;
            if (_m_wFundPage != null)
            {
                _m_wFundPage.showWnd();
                _m_wFundPage.setFundPageId(fundTabId);
            }
            else
            {
                _m_wFundPage = new GGUIWndFundPage(_getPageAssetPathIdByType(ECashGiftPackMainTabType.FUND), wnd.pageParent);
                _m_wFundPage.load(() =>
                {
                    if (_m_wFundPage == null)
                        return;
                    
                    _m_wFundPage.showWnd();
                    _m_wFundPage.setFundPageId(fundTabId);
                });
            }
        }

        //隐藏所有页面
        private void _hideAllPage()
        {
            _m_wGemPage?.hideWnd();
            _m_wActivityPage?.hideWnd();
            _m_wPermanentPage?.hideWnd();
            _m_wRechargeRebatePage?.hideWnd();
            _m_wPrivilegeCardPage?.hideWnd();
            _m_wFundPage?.hideWnd();
        }

        //根据页签类型获取对应的子页面的加载路径
        private long _getPageAssetPathIdByType(ECashGiftPackMainTabType _type)
        {
            if (wnd == null || wnd.monoTabList == null)
                return 0;

            foreach (GGUICashGiftPackMainTabMono mono in wnd.monoTabList)
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

        #endregion

        #region 点击事件

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndCashGiftPackMainTab _tabItemWnd)
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

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CASH_GIFT_PACK_MAIN);
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

        #region 消息事件

        //列表滚动事件
        private void _onScrollRectValueChg(Vector2 _arg)
        {
            //刷新列表左右两边红点提示
            wnd?.sideRedTipInfo?.refreshContainerSideRedTip(_m_lTabWndList, (RectTransform)wnd?.tabScrollRect?.transform);
        }

        //新增移除充值返利
        private void _onAddRemoveRechargeRebate(params object[] _objects)
        {
            bool haveRechargeRebate = _checkRechargeRebateTabShow();

            //如果当前没有充值返利了并且选中的页签是充值返利页签，则切换到钻石页签
            if(!haveRechargeRebate && _m_eCurSelectTabType == ECashGiftPackMainTabType.RECHARGE_REBATE)
                setSelectTab(ECashGiftPackMainTabType.GEM);
        }

        #endregion
    }
}