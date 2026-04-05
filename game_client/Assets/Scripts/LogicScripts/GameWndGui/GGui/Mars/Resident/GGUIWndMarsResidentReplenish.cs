using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 居民补充窗口
    /// </summary>
    public class GGUIWndMarsResidentReplenish : _ANPGGUIBasicWnd<GGUIMonoMarsResidentReplenish>
    {
        private static GGUIWndMarsResidentReplenish _g_instance;
        [NotNull] public static GGUIWndMarsResidentReplenish instance { get { return _g_instance ??= new GGUIWndMarsResidentReplenish(); } }

        private int _m_iTodayImmigrantCount = 0;//今日已补充次数
        private MarsImmigrationRefObj _m_ThisTimeImmigrationRefObj = null;//本次补充配置数据
        
        private GGUIWndCommonRewardContainer _m_wRewardContainer;
        private NPGGUIWndCommonItem _m_wConsumeItem;

        public GGUIWndMarsResidentReplenish() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsResidentReplenish.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsResidentReplenish.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建子窗口
            if (wnd.monoRewardContainer != null)
            {
                _m_wRewardContainer = new GGUIWndCommonRewardContainer(wnd.monoRewardContainer);
            }

            if (wnd.itemConsume != null)
            {
                _m_wConsumeItem = new NPGGUIWndCommonItem(wnd.itemConsume);
            }

            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnReplenish, _onClickReplenish);
            ALUGUICommon.combineBtnClick(wnd.btnExpandResidentLimit, _onClickExpandResidentLimit);
        }

        protected override void _onDiscard()
        {
            // 销毁子窗口
            if (_m_wRewardContainer != null)
            {
                _m_wRewardContainer.discard();
                _m_wRewardContainer = null;
            }

            if (_m_wConsumeItem != null)
            {
                _m_wConsumeItem.discard();
                _m_wConsumeItem = null;
            }

            // 解绑按钮点击事件
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnReplenish, _onClickReplenish);
                ALUGUICommon.uncombineBtnClick(wnd.btnExpandResidentLimit, _onClickExpandResidentLimit);
            }
        }

        protected override void _onShowWnd()
        {
            // 显示子窗口
            if (_m_wRewardContainer != null)
                _m_wRewardContainer.showWnd();

            if (_m_wConsumeItem != null)
                _m_wConsumeItem.showWnd();

            // 注册消息
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_COUNT_CHG, _onImmigrantCountChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH, _onSimulateClickReplenish);

            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏子窗口
            if (_m_wRewardContainer != null)
                _m_wRewardContainer.hideWnd();

            if (_m_wConsumeItem != null)
                _m_wConsumeItem.hideWnd();

            // 解除消息注册
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_COUNT_CHG, _onImmigrantCountChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH, _onSimulateClickReplenish);
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            // 获取今日已补充次数
            _m_iTodayImmigrantCount = NPPlayer.instance.marsComp.peopleSubComponent.todayImmigrantCount;
            
            // 获取本次补充配置数据(本次补充对应的配置数据是根据今日已补充次数+1来获取的)
            _m_ThisTimeImmigrationRefObj = GRefdataCoreMgr.instance.getMarsImmigrationRef(_m_iTodayImmigrantCount + 1);

            long peopleNumLimit = NPPlayer.instance.marsComp.peopleNumLimit;
            long totalPeopleNum = NPPlayer.instance.marsComp.totalPeopleNum;
            if (peopleNumLimit > totalPeopleNum)
            {
                ALUGUICommon.setGameObjEnable(wnd.residentNumReachLimitShow, false);
                ALUGUICommon.setGameObjEnable(wnd.residentNumReachLimitHide, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.residentNumReachLimitShow, true);
                ALUGUICommon.setGameObjEnable(wnd.residentNumReachLimitHide, false);
            }

            if (string.IsNullOrEmpty(wnd.txtCanGetResidentCountDescStr))
            {
                ALUGUICommon.setLabelTxt(wnd.txtCanGetResidentCountDesc, peopleNumLimit - totalPeopleNum);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtCanGetResidentCountDesc, TextTranslate.instance.getLanguage(wnd.txtCanGetResidentCountDescStr, peopleNumLimit - totalPeopleNum));
            }
            
            EMarsResidentReplenishState residentReplenishState = MarsUtil.getMarsResidentReplenishState();
            NPCommonEnumStatMutexShowInfo<EMarsResidentReplenishState>.setStat(wnd.replenishStateShowList, residentReplenishState);
            
            // 刷新奖励列表
            _refreshReward();

            // 刷新消耗道具
            _refreshConsume();

            // 刷新剩余次数文本
            _refreshLeftCount();
        }

        /// <summary>
        /// 刷新奖励列表
        /// </summary>
        private void _refreshReward()
        {
            if (_m_wRewardContainer == null)
                return;

            // 根据奖励id获取奖励列表
            List<NPCommonCostItem> rewardItemList = GCommon.getShowListByReawrdId(_m_ThisTimeImmigrationRefObj?.reward_id ?? 0);
            _m_wRewardContainer.showWnd();
            _m_wRewardContainer.setRewardList(rewardItemList);
        }

        /// <summary>
        /// 刷新消耗道具
        /// </summary>
        private void _refreshConsume()
        {
            if (_m_wConsumeItem == null)
                return;

            _m_wConsumeItem.setItem(_m_ThisTimeImmigrationRefObj?.cost_item);
            _m_wConsumeItem.showWnd();
        }

        /// <summary>
        /// 刷新剩余次数文本
        /// </summary>
        private void _refreshLeftCount()
        {
            if (wnd == null || wnd.txtLeftCount == null)
                return;

            // 获取每日最大次数
            int maxCount = GRefdataCoreMgr.instance.npGeneral.mars_immigration_daily_max_num;
            int leftCount = maxCount - _m_iTodayImmigrantCount;

            // 设置文本（使用通用格式：今日剩余请求补给次数: {0}）
            ALUGUICommon.setLabelTxt(wnd.txtLeftCount, 
                TextTranslate.instance.getLanguage(TransKeyConst.mars_resident_replenishLeftCount_num, leftCount));
        }

        /// <summary>
        /// 当commonItem变化回调
        /// </summary>
        /// <param name="_objs"></param>
        private void _onCommonItemChg(params object[] _objs)
        {
            if (_m_ThisTimeImmigrationRefObj == null || _m_ThisTimeImmigrationRefObj.cost_item == null || 
                _objs == null || _objs.Length < 2 || !(_objs[0] is ENPItemType itemType) || !(_objs[1] is long subId)
                || _m_ThisTimeImmigrationRefObj.cost_item.getItemType() != itemType || _m_ThisTimeImmigrationRefObj.cost_item.subId != subId)
                return;

                // 刷新消耗道具显示
                _refreshConsume();
        }
        
        /// <summary>
        /// 移民次数变化回调
        /// </summary>
        private void _onImmigrantCountChg()
        {
            refreshWnd();
        }

        /// <summary>
        /// 模拟点击补充按钮
        /// </summary>
        private void _onSimulateClickReplenish()
        {
            if (wnd == null) return;
            _onClickReplenish(wnd.btnReplenish);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_RESIDENT_REPLENISH);
        }

        /// <summary>
        /// 点击补充按钮
        /// </summary>
        private void _onClickReplenish(GameObject _go)
        {
            if (_m_ThisTimeImmigrationRefObj == null)
                return;
            
            EMarsResidentReplenishState residentReplenishState = MarsUtil.getMarsResidentReplenishState();
            switch (residentReplenishState)
            {
                case EMarsResidentReplenishState.Replenishing:
                    Common.MarsObj.Mars_PeopleImmigrant immigrantInfo = NPPlayer.instance.marsComp.peopleSubComponent.getImmigrantInfo();
                    if (immigrantInfo != null)
                    {
                        long countDownMs = immigrantInfo.getEndMs() - FpsAndPingMgr.instance.serverTimeTag;//获取移民倒计时
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_resident_replenishingTip_str, TimeUtil.millisecondsToTime_hms(countDownMs)));
                    }
                    return;
                        
                case EMarsResidentReplenishState.ReplenishComplete:
                    // 打开结果窗口
                    GGUIWndMarsResidentReplenishResult.addWndNode((_isSucc) =>
                    {
                        refreshWnd();
                    });
                    return;
                case EMarsResidentReplenishState.ReplenishCountLimit:
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_resident_replenishCountLimitTip_none));
                    return;
            }
            
            // 需要的道具不足，直接返回
            if(_m_ThisTimeImmigrationRefObj.cost_item != null && !GCommon.isItemEnough(_m_ThisTimeImmigrationRefObj.cost_item, true))
                return;

            // 检查是否人口已达上限且需要提示
            long peopleNumLimit = NPPlayer.instance.marsComp.peopleNumLimit;
            long totalPeopleNum = NPPlayer.instance.marsComp.totalPeopleNum;
            bool isPeopleLimitReached = totalPeopleNum >= peopleNumLimit;
            bool needShowPeopleLimitTip = isPeopleLimitReached && AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_RESIDENT_REPLENISH_PEOPLE_LIMIT);

            if (needShowPeopleLimitTip)
            {
                // 弹出人口上限二次确认弹窗（带今日不再提示勾选框）
                NPMesMgr.instance.showTwoBtnTogMes(
                    (_toggle) =>
                    {
                        // 点击确定按钮：保存今日不再提示设置，然后继续正常流程
                        if (_toggle)
                            AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.MARS_RESIDENT_REPLENISH_PEOPLE_LIMIT);
                        
                        // 继续正常的补充流程
                        _checkCostTipAndRequest();
                    },
                    null, // 点击取消按钮：不进行任何操作
                    TransKeyConst.sys_tip_title_none,
                    TextTranslate.instance.getLanguage(TransKeyConst.mars_resident_replenishPeopleLimitTip_none));
            }
            else
            {
                // 正常流程：检查是否需要消耗提示
                _checkCostTipAndRequest();
            }
        }

        /// <summary>
        /// 检查消耗提示并请求
        /// </summary>
        private void _checkCostTipAndRequest()
        {
            if (_m_ThisTimeImmigrationRefObj == null)
                return;

            // 获取玩家赚速
            long earnings = NPPlayer.instance.getValue(ENPPlayerValueType.EARNINGS) * GRefdataCoreMgr.instance.npGeneral.mars_immigration_show_silver_cost_tip_earning_multiples;
            if (_m_ThisTimeImmigrationRefObj.cost_item != null && _m_ThisTimeImmigrationRefObj.cost_item.getItemType() == ENPItemType.CURRENCY
                && _m_ThisTimeImmigrationRefObj.cost_item.subId == 2 && _m_ThisTimeImmigrationRefObj.cost_item.count >=  earnings)
            {
                NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(_m_ThisTimeImmigrationRefObj.cost_tip), 
                    TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                    null,
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        // 发送补充居民请求
                        _reqPeopleImmigrant();
                    });
            }
            else
            {
                // 发送补充居民请求
                _reqPeopleImmigrant();
            }
        }

        /// <summary>
        /// 点击扩展居民上限按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickExpandResidentLimit(GameObject _go)
        {
            MarsBuildingInfo targetLivingBuilding = MarsUtil.getCanBuildOrUpdateLivingBuilding();//聚焦的居住舱
            if(targetLivingBuilding == null)
                return;
            
            // 关闭窗口
            _onClickClose(null);
            
            MarsUtil.jumpToBuildingUpgrade(targetLivingBuilding);
        }
        
        /// <summary>
        /// 请求移民
        /// </summary>
        private void _reqPeopleImmigrant()
        {
            NPPlayer.instance.marsComp.peopleSubComponent.reqPeopleImmigrant((_isSucc, _msg) =>
            {
                if (!_isSucc)
                    return;

                // 请求补充成功，判断是否移民完成
                EMarsResidentReplenishState replenishState = MarsUtil.getMarsResidentReplenishState();
                // 若完成, 弹出结果窗口
                if (replenishState == EMarsResidentReplenishState.ReplenishComplete)
                {
                    GGUIWndMarsResidentReplenishResult.addWndNode((_isSucc) =>
                    {
                        refreshWnd();
                    });
                }
                // 若没有完成, 直接退出窗口
                else
                {
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_RESIDENT_REPLENISH);
                }
            });
        }
    }
}
