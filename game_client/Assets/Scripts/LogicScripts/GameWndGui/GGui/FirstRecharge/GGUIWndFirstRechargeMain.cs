using System;
using ALPackage;
using NPEnum;
using System.Collections.Generic;
using GC2GS.p021_PlayerInfo;
using GS2GC.p021_PlayerInfo;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 首充礼包主界面
    /// </summary>
    public class GGUIWndFirstRechargeMain : _ANPGGUIBasicWnd<GGUIMonoFirstRechargeMain>
    {
        private static GGUIWndFirstRechargeMain _g_instance = new GGUIWndFirstRechargeMain();

        public static GGUIWndFirstRechargeMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndFirstRechargeMain();

                return _g_instance;
            }
        }

        public GGUIWndFirstRechargeMain() : base(EALUIWndLayer.ADDITION)
        {
        }

        //首充礼包配置
        private GiftPackRefObj _m_giftPackRef;
        //当前选中的天数配置
        private FirstRechargeDayRefObj _m_curSelectDayRef;
        //奖励列表
        private GGUIWndCommonRewardContainer _m_wItemContainer;
        //特殊奖励形象列表
        private List<GGUIWndSubFirstRechargeSpecialRewardActor> _m_subSpecialRewardActorWndList;
        //通用购买按钮
        private GGUIWndCommonBuyButton _m_wBuyButton;
        //页签列表
        private List<GGUIWndFirstRechargeTab> _m_lTabList;
        //顾问卡牌item
        private GGUIWndHeroCommonCardItem _m_wHeroCardItem;
        //情人卡牌item
        private GGUIWndConsortCardItem _m_wConsortCardItem;
        //道具特殊奖励item
        private GGUIWndCommonRewardContainerItem _m_wSpecialItem;
        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;

        protected override string _monoAssetPath { get { return GGUIMonoFirstRechargeMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFirstRechargeMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_ADD, _onGiftPackChg);
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff += _onBuffChg;
            NPPlayer.instance.playerBuffComp.onRemovePlayerBuff += _onRemoveBuff;

            _m_giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.first_recharge_gift_pack_id);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_ADD, _onGiftPackChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff -= _onBuffChg;
            NPPlayer.instance.playerBuffComp.onRemovePlayerBuff -= _onRemoveBuff;
            if (_m_subSpecialRewardActorWndList != null)
            {
                for (int i = 0; i < _m_subSpecialRewardActorWndList.Count; i++)
                {
                    _m_subSpecialRewardActorWndList[i]?.hideWnd();
                }
            }
            _m_iTickTask.setDisable();
            _m_wItemContainer?.hideWnd();
            _m_wBuyButton?.hideWnd();
            _m_wHeroCardItem?.hideWnd();
            _m_wConsortCardItem?.hideWnd();
            _m_wSpecialItem?.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_subSpecialRewardActorWndList != null)
            {
                for (int i = 0; i < _m_subSpecialRewardActorWndList.Count; i++)
                {
                    _m_subSpecialRewardActorWndList[i]?.resetWnd();
                }
            }
            _m_wItemContainer?.resetWnd();
            _m_wBuyButton?.resetWnd();
            _m_wHeroCardItem?.resetWnd();
            _m_wConsortCardItem?.resetWnd();
            _m_wSpecialItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_subSpecialRewardActorWndList != null)
            {
                for (int i = 0; i < _m_subSpecialRewardActorWndList.Count; i++)
                {
                    _m_subSpecialRewardActorWndList[i]?.discard();
                }
            }
            _m_subSpecialRewardActorWndList?.Clear();
            _m_subSpecialRewardActorWndList = null;

            if (_m_lTabList != null)
            {
                for (int i = 0; i < _m_lTabList.Count; i++)
                {
                    _m_lTabList[i]?.discard();
                }
            }
            _m_lTabList?.Clear();
            _m_lTabList = null;

            _m_wItemContainer?.discard();
            _m_wItemContainer = null;

            _m_wBuyButton?.discard();
            _m_wBuyButton = null;

            _m_wHeroCardItem?.discard();
            _m_wHeroCardItem = null;

            _m_wConsortCardItem?.discard();
            _m_wConsortCardItem = null;

            _m_wSpecialItem?.discard();
            _m_wSpecialItem = null;

            _m_curSelectDayRef = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnSpecItemDetail, _onClickSpecItemDetail);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new GGUIWndCommonRewardContainer(wnd.monoItemContainer);

            _m_subSpecialRewardActorWndList = new List<GGUIWndSubFirstRechargeSpecialRewardActor>();
            if (wnd.monoSpecialRewardActorList != null)
            {
                for (int i = 0; i < wnd.monoSpecialRewardActorList.Count; i++)
                {
                    GGUIWndSubFirstRechargeSpecialRewardActor actorWnd = new GGUIWndSubFirstRechargeSpecialRewardActor(wnd.monoSpecialRewardActorList[i]);
                    _m_subSpecialRewardActorWndList.Add(actorWnd);
                }
            }

            if (wnd.monoBuyButton != null)
            {
                _m_wBuyButton = new GGUIWndCommonBuyButton(wnd.monoBuyButton);
                _m_wBuyButton.onClickButton += _onClickBuyButton;
            }

            if (wnd.monoTabList != null)
            {
                _m_lTabList = new List<GGUIWndFirstRechargeTab>();
                for (int i = 0; i < wnd.monoTabList.Count; i++)
                {
                    GGUIWndFirstRechargeTab tab = new GGUIWndFirstRechargeTab(wnd.monoTabList[i], i + 1);
                    tab.onClickTab += _onClickTab;
                    _m_lTabList.Add(tab);
                }
            }

            if (wnd.monoHeroCard != null)
            {
                _m_wHeroCardItem = new GGUIWndHeroCommonCardItem(wnd.monoHeroCard);
                _m_wHeroCardItem.ClickAction += _onClickHeroSpecialItem;
            }

            if (wnd.monoConsortCard != null)
                _m_wConsortCardItem = new GGUIWndConsortCardItem(wnd.monoConsortCard, _onClickConsortSpecialItem);

            if (wnd.monoSpecialItem != null)
                _m_wSpecialItem = new GGUIWndCommonRewardContainerItem(wnd.monoSpecialItem);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnSpecItemDetail, _onClickSpecItemDetail);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            _refreshActorList();
            _refreshRechargeInfo();
            _refreshTab();
            _refreshTabRedTip();
        }

        /// <summary>
        /// 刷新形象列表
        /// </summary>
        private void _refreshActorList()
        {
            if (_m_subSpecialRewardActorWndList == null)
                return;

            List<FirstRechargeDayRefObj> firstRechargeDayList = GRefdataCoreMgr.instance.firstRechargeDayRefCore.refList;
            if (firstRechargeDayList == null)
                return;

            int specialRewardCount = 0;
            for (int i = 0; i < firstRechargeDayList.Count; i++)
            {
                if (firstRechargeDayList[i] != null && firstRechargeDayList[i].special_item != null && 
                    (firstRechargeDayList[i].special_item.getItemType() == ENPItemType.HERO || firstRechargeDayList[i].special_item.getItemType() == ENPItemType.CONSORT))
                {
                    if (_m_subSpecialRewardActorWndList.Count > specialRewardCount)
                    {
                        _m_subSpecialRewardActorWndList[specialRewardCount]?.showWnd();
                        _m_subSpecialRewardActorWndList[specialRewardCount]?.setInfo(firstRechargeDayList[i]);
                        specialRewardCount++;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新充值信息
        /// </summary>
        private void _refreshRechargeInfo()
        {
            if (wnd == null || _m_giftPackRef == null)
                return;

            long buyCount = NPPlayer.instance.giftPackComp.getGiftPackHadBuyCount(_m_giftPackRef.id);

            //购买按钮
            _m_wBuyButton?.showWnd();
            _m_wBuyButton?.setInfoList(_m_giftPackRef.cost_list, false);

            //已购买显隐
            ALUGUICommon.setGameObjEnable(wnd.goHadRechargeHideList, buyCount <= 0);
            ALUGUICommon.setGameObjEnable(wnd.goHadRechargeShowList, buyCount > 0);
        }

        /// <summary>
        /// 刷新页签列表
        /// </summary>
        private void _refreshTab()
        {
            if (_m_lTabList == null)
                return;

            List<FirstRechargeDayRefObj> firstRechargeDayList = GRefdataCoreMgr.instance.firstRechargeDayRefCore.refList;
            if (firstRechargeDayList == null)
                return;

            //设置默认选中页签
            long targetSelectDay = -1;

            if (_m_curSelectDayRef != null)
            {
                //已经有选中页签，继续选中
                targetSelectDay = _m_curSelectDayRef.day;
                _refreshPage();
            }
            else
            {
                //没有选中页签，默认选中第一个可领取奖励的页签
                long buyCount = NPPlayer.instance.giftPackComp.getGiftPackHadBuyCount(_m_giftPackRef.id); //已购买，选中第一个可领取奖励的页签
                if (buyCount > 0)
                {
                    long firstCanNotGetRewardDay = 1;
                    for (int i = 0; i < firstRechargeDayList.Count; i++)
                    {
                        FirstRechargeDayRefObj dayRef = firstRechargeDayList[i];
                        if (dayRef == null)
                            continue;

                        ECommonRewardType rewardType = _getRewardType(dayRef);
                        if (rewardType == ECommonRewardType.CAN_GET_REWARD && targetSelectDay == -1)
                        {
                            targetSelectDay = dayRef.day;
                            break;
                        }
                        else if (rewardType == ECommonRewardType.NOT_GET_REWARD && firstCanNotGetRewardDay == 1)
                            firstCanNotGetRewardDay = dayRef.day;
                    }

                    //如果没有可领取的页签，选中第一个不可领取页签页签
                    if (targetSelectDay == -1)
                        targetSelectDay = firstCanNotGetRewardDay;
                }
                else
                {
                    targetSelectDay = 1;
                }
            }

            //设置选中页签
            for (int i = 0; i < _m_lTabList.Count; i++)
            {
                if (_m_lTabList[i] == null)
                    continue;

                if (_m_lTabList[i].day == targetSelectDay)
                    _m_lTabList[i].setClickTab();
                else
                    _m_lTabList[i].setSelected(false);
            }
        }

        /// <summary>
        /// 刷新页签红点
        /// </summary>
        private void _refreshTabRedTip()
        {
            for (int i = 0; i < _m_lTabList.Count; i++)
            {
                if (_m_lTabList[i] == null)
                    continue;

                FirstRechargeDayRefObj dayRef = GRefdataCoreMgr.instance.firstRechargeDayRefCore.getRef(_m_lTabList[i].day);
                if(dayRef == null)
                    continue;

                NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(dayRef.buff_id);
                bool canGetReward = buffInfo != null && buffInfo.layer > 0 && (dayRef.condition == null || dayRef.condition.isEmpty || dayRef.condition.IsEnable(null));
                _m_lTabList[i]?.showRedTipNum(canGetReward ? 1 : 0);
            }
        }

        /// <summary>
        /// 刷新页面
        /// </summary>
        private void _refreshPage()
        {
            _refreshRewardList();
            _refreshTabRedTip();
            _checkShowCDTask();
        }

        /// <summary>
        /// 刷新奖励列表
        /// </summary>
        private void _refreshRewardList()
        {
            if (wnd == null || _m_curSelectDayRef == null)
                return;

            GiftPackRefObj giftGiftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.first_recharge_gift_pack_id);
            NPCommonCostItem specialItem = _m_curSelectDayRef.special_item;
            bool haveSpecialItem = specialItem != null && specialItem.getItemType() != ENPItemType.NONE;
            ECommonRewardType rewardType = _getRewardType(_m_curSelectDayRef);

            //礼包价值
            ALUGUICommon.setLabelTxt(wnd.txtDiscount, TextTranslate.instance.getLanguage(TransKeyConst.firstRecharge_giftPackProfit_num, giftGiftPackRef != null ? giftGiftPackRef.profit_per / 100 : 0));

            //奖励列表
            _m_wItemContainer?.showWnd();
            _m_wItemContainer?.setRewardList(_m_curSelectDayRef.item_list, rewardType);

            //特殊奖励
            _m_wHeroCardItem?.hideWnd();
            _m_wConsortCardItem?.hideWnd();
            _m_wSpecialItem?.hideWnd();
            if (specialItem != null)
            {
                switch (specialItem.getItemType())
                {
                    case ENPItemType.HERO:
                        HeroCardShowInfo heroShowInfo = new HeroCardShowInfo(null, GRefdataCoreMgr.instance.heroRefCore.getRef(specialItem.subId));
                        _m_wHeroCardItem?.showWnd();
                        _m_wHeroCardItem?.setInfo(heroShowInfo);
                        break;
                    case ENPItemType.CONSORT:
                        ConsortRefShowInfo consortShowInfo = new ConsortRefShowInfo(specialItem.subId);
                        _m_wConsortCardItem?.showWnd();
                        _m_wConsortCardItem?.setInfo(consortShowInfo);
                        break;
                    default:
                        _m_wSpecialItem?.showWnd();
                        _m_wSpecialItem?.setItem(specialItem, rewardType);
                        break;
                }
            }
            ALUGUICommon.setGameObjEnable(wnd.goSpecialRewardGetShowList, rewardType == ECommonRewardType.HAS_GET_REWARD);
            ALUGUICommon.setGameObjEnable(wnd.goNoSpecialItemHideList, haveSpecialItem);
            ALUGUICommon.setGameObjEnable(wnd.goNoSpecialItemShowList, !haveSpecialItem);

            //奖励标题
            ALUGUICommon.setLabelTxt(wnd.txtRewardTitle, TextTranslate.instance.getLanguage(_m_curSelectDayRef.day_reward_desc, _m_curSelectDayRef.day_reward_desc_args));

            //奖励状态
            NPCommonEnumStatInfo<ECommonRewardType>.setStat(wnd.rewardStateList, rewardType);
        }

        /// <summary>
        /// 获取领奖状态
        /// </summary>
        /// <returns></returns>
        private ECommonRewardType _getRewardType(FirstRechargeDayRefObj _dayRef)
        {
            ECommonRewardType rewardType = ECommonRewardType.NONE;
            if(_dayRef == null || _m_giftPackRef == null)
                return rewardType;

            long buyCount = NPPlayer.instance.giftPackComp.getGiftPackHadBuyCount(_m_giftPackRef.id);
            NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(_dayRef.buff_id);
            bool canGetReward = buffInfo != null && buffInfo.layer > 0 && (_dayRef.condition == null || _dayRef.condition.isEmpty || _dayRef.condition.IsEnable(null));
            if (buyCount > 0 && canGetReward)
                rewardType = ECommonRewardType.CAN_GET_REWARD;//已购买且可领取
            else if(buyCount > 0 && buffInfo == null)
                rewardType = ECommonRewardType.HAS_GET_REWARD;//已购买且已领取
            else
                rewardType = ECommonRewardType.NOT_GET_REWARD;//不可领取
            return rewardType;
        }

        #region 可领奖倒计时

        /// <summary>
        /// 检查是否需要开启定时任务
        /// </summary>
        private void _checkShowCDTask()
        {
            _m_iTickTask.setDisable();
            if (_m_giftPackRef == null)
                return;

            long buyCount = NPPlayer.instance.giftPackComp.getGiftPackHadBuyCount(_m_giftPackRef.id);
            if (buyCount <= 0 || _m_curSelectDayRef == null || _m_giftPackRef == null || _getRewardType(_m_curSelectDayRef) != ECommonRewardType.NOT_GET_REWARD)
                return;

            _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCD, 0.2f);
        }

        /// <summary>
        /// 倒计时任务
        /// </summary>
        private void _tickCD()
        {
            if (wnd == null || _m_curSelectDayRef == null)
                return;

            NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(_m_curSelectDayRef.buff_id);
            if (buffInfo == null || buffInfo.hadActiveDay >= _m_curSelectDayRef.day)
            {
                _refreshPage();
                return;
            }

            //间隔的天数
            long dayInterval = _m_curSelectDayRef.day - buffInfo.hadActiveDay;
            if (dayInterval <= 0)
            {
                _refreshPage();
                return;
            }

            //距离24点的毫秒数
            long leftTimeMs = TimeUtil.getNextAssignTimeRemainMs(FpsAndPingMgr.instance.serverTimeTag, 24, 0);
            if (dayInterval > 1)
                leftTimeMs = leftTimeMs + (dayInterval - 1) * 24 * 60 * 60 * 1000;

            if (leftTimeMs < 0)
            {
                _m_iTickTask.setDisable();
                _refreshPage();
            }
            else
            {
                //刷新倒计时显示
                ALUGUICommon.setLabelTxt(wnd.txtCD, TimeUtil.millisecondsToTime_hms(leftTimeMs, TransKeyConst.firstRecharge_getRewardCD_num_num_num));
            }
        }

        #endregion

        #region 点击事件

        //点击购买按钮
        private void _onClickBuyButton(NPCommonCostItem _costItem, bool _isSellOut)
        {
            if (_m_giftPackRef == null || _isSellOut)
                return;

            if (_costItem == null || _costItem.getItemType() != ENPItemType.PAY)
            {
                //免费购买或道具购买
                NPPlayer.instance.giftPackComp.reqBuyGiftPack(_m_giftPackRef.id);
            }
            else
            {
                //支付流程
                GCommon.reqPay(_m_giftPackRef, _refreshRechargeInfo, null);
            }
        }

        //点击页签
        private void _onClickTab(GGUIWndFirstRechargeTab _item)
        {
            if(_item == null || _m_lTabList == null)
                return;

            if (_m_curSelectDayRef != null && _m_curSelectDayRef.day == _item.day)
                return;

            //设置选中页签
            for (int i = 0; i < _m_lTabList.Count; i++)
            {
                if (_m_lTabList[i] == _item)
                    _m_lTabList[i].setSelected(true);
                else
                    _m_lTabList[i].setSelected(false);
            }

            _m_curSelectDayRef = GRefdataCoreMgr.instance.firstRechargeDayRefCore.getRef(_item.day);
            _refreshPage();
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_FIRST_RECHARGE_MAIN);
        }

        //点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            if (_m_curSelectDayRef == null)
                return;

            ECommonRewardType rewardType = _getRewardType(_m_curSelectDayRef);
            switch (rewardType )
            {
                case ECommonRewardType.CAN_GET_REWARD:
                    NPGSClientListener.sendRequestByLog(new GC2GS_021_044_ReqDrawFirstRechargeReward((int)_m_curSelectDayRef.day),
                        new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_021_044_RetDrawFirstRechargeReward>((_isSuc, _msg) =>
                        {
                            if (!_isSuc || _msg == null)
                                return;

                            //刷新界面
                            _refreshPage();
                            GCommon.reloadCustomLoadPrefab();

                            //添加奖励列表到notice
                            GCommon.dealGainItem(_msg.getItemList());
                        }));
                    break;
                case ECommonRewardType.HAS_GET_REWARD:
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.firstRecharge_hadGetReward_none);
                    break;
                case ECommonRewardType.NOT_GET_REWARD:
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.firstRecharge_canGetRewardLeftDay_num, _m_curSelectDayRef.day));
                    break;
            }
        }

        //点击道具特殊奖励详情按钮
        private void _onClickSpecItemDetail(GameObject _go)
        {
            if (_m_curSelectDayRef == null || _m_curSelectDayRef.special_item == null)
                return;

            switch (_m_curSelectDayRef.special_item.getItemType())
            {
                //藏品需要特殊处理，打开藏品预览界面
                case ENPItemType.EQUIP:
                    EquipRefObj curEquipRef = GRefdataCoreMgr.instance.equipRefCore.getRef(_m_curSelectDayRef.special_item.subId);
                    List<EquipRefObj> equipRefList = new List<EquipRefObj>();
                    equipRefList.Add(curEquipRef);
                    QueueMgr.instance.AddNode(new GMainQueueEquipPreviewNode(equipRefList, curEquipRef));
                    break;
                default:
                    GCommon.clickShowItemToolTip(_m_curSelectDayRef.special_item, 0, _go.GetComponent<RectTransform>());
                    break;
            }
        }

        //点击情人特殊奖励item
        private void _onClickConsortSpecialItem(_IConsortShowInfo _info)
        {
            if (_info == null)
                return;

            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_info.consortId);
            if(consortInfo == null)
                QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(_info.consortId)));
            else
                GNodeUnLockConsortDetail.addConsortNode(new List<GGottenConsortInfo>(){ consortInfo }, consortInfo.consortId);
        }

        //点击顾问特殊奖励item
        private void _onClickHeroSpecialItem(GGUIWndHeroCommonCardItem _item)
        {
            if (_item == null || _item.heroCardShow == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_item.heroCardShow.id);
            HeroCardShowInfo showInfo = new HeroCardShowInfo(heroInfo, GRefdataCoreMgr.instance.heroRefCore.getRef(_item.heroCardShow.id));
            if(heroInfo == null)
                QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(showInfo, null));
            else
                QueueMgr.instance.AddNode(new GMainQueueHeroInfoNode(showInfo, null));
        }

        #endregion

        #region 消息事件

        //礼包变化消息
        private void _onGiftPackChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long giftPackId = (long)_objects[0];
            if (giftPackId == GRefdataCoreMgr.instance.npGeneral.first_recharge_gift_pack_id)
            {
                _refreshRechargeInfo();
                _refreshPage();
            }
        }

        //buff变化消息
        private void _onBuffChg(NPPlayerBuffInfo _info, int _layer, long _curLeftTimeMS)
        {
            if (_info != null && _m_curSelectDayRef != null && _info.buffId == _m_curSelectDayRef.buff_id)
            {
                _refreshPage();
            }
        }

        //buff移除消息
        private void _onRemoveBuff(NPPlayerBuffInfo _info)
        {
            if (_info != null && _m_curSelectDayRef != null && _info.buffId == _m_curSelectDayRef.buff_id)
            {
                _refreshPage();
            }
        }

        #endregion
    }
}
