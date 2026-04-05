using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using Common.MarsEnum;
using CommonEnum;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星探索队伍修复窗口
    /// 功能：队伍士兵修复、立即完成修复、取消修复、公会帮助
    /// </summary>
    public class GGUIWndMarsExploreTeamRepair : _ATALBasicUIWnd<GGUIMonoMarsExploreTeamRepair>, _IMarsCompleteNowObject
    {
        [NotNull] public static GGUIWndMarsExploreTeamRepair instance { get { return _g_instance ??= new GGUIWndMarsExploreTeamRepair(); } }
        private static GGUIWndMarsExploreTeamRepair _g_instance;

        // 队伍信息
        private MarsExploreTeamInfo _m_teamInfo;

        // 子窗口组件
        private NPGGUIWndCommonItem _m_repairCostItemWnd;           // 修复消耗资源显示
        private NPGGUIWndCommonItem _m_completeNowCostItemWnd;      // 立即完成消耗资源显示
        private GGUISubWndMarsExploreTeamHeroContainer _m_heroContainerWnd;  // 大臣容器
        private GGUIWndBagPopCounter _m_numSliderWnd;                 // 修复数量滑块

        // Tick 任务控制器
        private ALCommonEnableTaskController _m_tickTask;

        private new bool _m_bIsShow;


        public GGUIWndMarsExploreTeamRepair()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreTeamRepair.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreTeamRepair.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            // 显示所有子窗口
            _m_heroContainerWnd?.showWnd();
            _m_repairCostItemWnd?.showWnd();
            _m_completeNowCostItemWnd?.showWnd();
            _m_numSliderWnd?.showWnd();

            // 启动 Tick 更新任务
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);

            refreshWnd();
            
            _tryAddTeamEventListen();
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _onTeamUIStateChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _onTeamUIStateChg);
            _tryRemoveTeamEventListen();
            
            // 隐藏所有子窗口
            _m_heroContainerWnd?.hideWnd();
            _m_repairCostItemWnd?.hideWnd();
            _m_completeNowCostItemWnd?.hideWnd();
            _m_numSliderWnd?.hideWnd();

            // 停止 Tick 更新任务
            _m_tickTask.setDisable();

            _m_bIsShow = false;
        }

        protected override void _onReset()
        {
            // 重置所有子窗口
            _m_heroContainerWnd?.resetWnd();
            _m_repairCostItemWnd?.resetWnd();
            _m_completeNowCostItemWnd?.resetWnd();
            _m_numSliderWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            // 销毁所有子窗口
            _m_heroContainerWnd?.discard();
            _m_heroContainerWnd = null;
            _m_repairCostItemWnd?.discard();
            _m_repairCostItemWnd = null;
            _m_completeNowCostItemWnd?.discard();
            _m_completeNowCostItemWnd = null;
            _m_numSliderWnd?.discard();
            _m_numSliderWnd = null;
            

            // 解绑所有按钮事件
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnRepair, _onClickRepair);
            ALUGUICommon.uncombineBtnClick(wnd.btnCancelRepair, _onClickCancelRepair);
            ALUGUICommon.uncombineBtnClick(wnd.btnCompleteNow, _onClickCompleteNow);
            ALUGUICommon.uncombineBtnClick(wnd.btnGuildHelp, _onClickGuildHelp);
            ALUGUICommon.uncombineBtnClick(wnd.btnSpeedUp, _onClickSpeedUp);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化所有子窗口组件
            if (wnd.monoRepairCostItem != null)
                _m_repairCostItemWnd = new NPGGUIWndCommonItem(wnd.monoRepairCostItem);
            if (wnd.monoCompleteNowCostItem != null)
                _m_completeNowCostItemWnd = new NPGGUIWndCommonItem(wnd.monoCompleteNowCostItem);
            if (wnd.monoHeroContainer != null)
                _m_heroContainerWnd = new GGUISubWndMarsExploreTeamHeroContainer(wnd.monoHeroContainer, null);
            if (wnd.monoNumSlider != null)
            {
                _m_numSliderWnd = new GGUIWndBagPopCounter(wnd.monoNumSlider);
                _m_numSliderWnd.regCounterChangedEvent(_onSliderValueChg);
            }

            // 绑定所有按钮事件
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnRepair, _onClickRepair);
            ALUGUICommon.combineBtnClick(wnd.btnCancelRepair, _onClickCancelRepair);
            ALUGUICommon.combineBtnClick(wnd.btnCompleteNow, _onClickCompleteNow);
            ALUGUICommon.combineBtnClick(wnd.btnGuildHelp, _onClickGuildHelp);
            ALUGUICommon.combineBtnClick(wnd.btnSpeedUp, _onClickSpeedUp);
        }


        /// <summary>
        /// 刷新窗口（带队伍信息参数）
        /// </summary>
        public void refreshWnd(MarsExploreTeamInfo _teamInfo)
        {
            _tryRemoveTeamEventListen();
            _m_teamInfo = _teamInfo;
            _tryAddTeamEventListen();
            refreshWnd();
        }
        /// <summary>
        /// 刷新窗口
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            _refreshTeamBaseInfo();
            _onSliderValueChg(_m_numSliderWnd?.currentCount ?? 0);
            _onTeamUIStateChg();
        }
        
        
        /// <summary>
        /// 刷新队伍基础信息（队伍编号、名称、兵力、战力等）
        /// </summary>
        private void _refreshTeamBaseInfo()
        {
            if (wnd == null || _m_teamInfo == null)
                return;

            // 队伍编号和名称
            ALUGUICommon.setLabelTxt(wnd.txtNum, _m_teamInfo.teamId);
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_teamInfo.name);

            // 士兵数量
            ALUGUICommon.setLabelTxt(wnd.txtSoldierNum, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, 
                _m_teamInfo.soldierNum.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), 
                _m_teamInfo.soldierMax.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtSoldierLossNum, _m_teamInfo.soldierLoss.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            
            // 实力加成百分比
            ALUGUICommon.setLabelTxt(wnd.txtPowerAddPercent, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, _m_teamInfo.getPowerAddPer() / 100f));
            // 队伍战斗力
            ALUGUICommon.setLabelTxt(wnd.txtTeamPower, _m_teamInfo.getTeamPower().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            // 刷新英雄容器
            _m_heroContainerWnd?.refreshWnd(_m_teamInfo);
            
            // 默认修复全部损失士兵
            _m_numSliderWnd?.init(_m_teamInfo.soldierLoss, _m_teamInfo.soldierLoss, null, _m_teamInfo.soldierLoss);
        }
        private void _onSliderValueChg(long _sliderValue)
        {
            long originTimeCost = _sliderValue * GRefdataCoreMgr.instance.npGeneral.mars_explore_team_repair_unit_ms;
            if (originTimeCost < 0) originTimeCost = 0;
            ALUGUICommon.setLabelTxt(wnd.txtRepairNeedOriginTime, TimeUtil.millisecondsToTime_DayHourOrHMS(originTimeCost));
            long property = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_REPAIR_SPEED_PER);
            long realTimeCost = originTimeCost * 10000 / (10000 + property);
            if (realTimeCost < 0) realTimeCost = 0;
            ALUGUICommon.setLabelTxt(wnd.txtRepairNeedRealTime, TimeUtil.millisecondsToTime_DayHourOrHMS(realTimeCost));
            _m_repairCostItemWnd?.setItem(_calculateRepairCost(_sliderValue));
            NPCommonCostItem completeNowCostItem = new NPCommonCostItem(ENPItemType.CURRENCY, (int)ECurrency.GEM, 0);
            long gemCost = MarsUtil.calculateGemCostForSpeedUpMs(realTimeCost);
            completeNowCostItem.setCount(gemCost);
            _m_completeNowCostItemWnd?.setItem(completeNowCostItem);
        }
        /// <summary>
        /// Tick 更新方法
        /// </summary>
        private void _tick()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            // 只在修复中状态才需要更新
            if (_m_teamInfo.state != EMarsExploreTeamState.REPAIR || _m_teamInfo.exDataObj is not MarsTeamEx_Repaire repairDate)
                return;

            long timeNow = FpsAndPingMgr.instance.serverTimeTag;
            long totalTime = _m_teamInfo.stateEndTime - _m_teamInfo.stateStartTime;
            long elapsedTime = timeNow - _m_teamInfo.stateStartTime;
            float progress = totalTime > 0 ? (float)elapsedTime / totalTime : 1f;
            ALUGUICommon.setLabelTxt(wnd.txtRepairingTime, TimeUtil.millisecondsToTime_DayHourOrHMS(_m_teamInfo.stateRemainTimeMs));
            if (wnd.sldRepairingProgress != null)
            {
                wnd.sldRepairingProgress.minValue = 0;
                wnd.sldRepairingProgress.maxValue = 1;
                wnd.sldRepairingProgress.value = progress;
            }

            // 没有维修一半的说法，先注释掉
            // long repairedNum = (long)(progress * repairDate.repairNum);
            // long currentSoldierNum = _m_teamInfo.soldierNum + repairedNum;
            // long currentSoldierLossNum = _m_teamInfo.soldierLoss - repairedNum;
            // long currentTeamPower = MarsUtil.calculateMarsExploreTeamPower(currentSoldierNum, _m_teamInfo.getPowerAddPer());
            // ALUGUICommon.setLabelTxt(wnd.txtSoldierNum, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, 
            //     currentSoldierNum.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), 
            //     _m_teamInfo.soldierMax.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            // ALUGUICommon.setLabelTxt(wnd.txtSoldierLossNum, currentSoldierLossNum.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            // ALUGUICommon.setLabelTxt(wnd.txtTeamPower, currentTeamPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            _m_completeNowCostItemWnd?.setItem(_m_teamInfo.completeNowCostItem);
        }
        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_TEAM_REPAIR);
        }
        /// <summary>
        /// 点击修复按钮
        /// </summary>
        private void _onClickRepair(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;

            long repairNum = _m_numSliderWnd?.currentCount ?? 0;
            if (repairNum <= 0)
                return;

            // 检查修复消耗是否足够
            NPCommonCostItem repairCost = _calculateRepairCost(repairNum);
            if (!GCommon.isItemEnough(repairCost, true))
                return;

            NPPlayer.instance.marsComp.exploreSubComponent.reqTeamRepair(_m_teamInfo.teamId, repairNum, null);
        }
        /// <summary>
        /// 点击取消修复按钮
        /// </summary>
        private void _onClickCancelRepair(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;

            NPMesMgr.instance.showTwoBtnMes(
                TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreTeamRepairCancelConfirmDesc_none),
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () => NPGSClientListener.sendMsgByLog(new GC2GS_041_020_ReqCancelExploreTeamRepair(_m_teamInfo.teamId)), true,
                TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreTeamRepairCancelConfirmTitle_none));
        }
        /// <summary>
        /// 点击立即完成按钮
        /// </summary>
        private void _onClickCompleteNow(GameObject _go)
        {
            if (!checkCanCompleteNow(true, true))
                return;
            
            // 检查是否需要显示今日不再提示的确认窗口
            bool needConfirm = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_TIME_COMPLETE_NOW_CONFIRM);
            if (needConfirm)
                // 显示确认窗口
                GGUIWndMarsTimeCompleteNowConfim.addNode(this);
            else
                // 直接执行立即完成操作
                dealCompleteNowAction?.Invoke(null);
        }
        /// <summary>
        /// 点击公会帮助按钮
        /// </summary>
        private void _onClickGuildHelp(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;

            // 请求公会帮助
            NPPlayer.instance.guildMarsHelpComp.reqSendMarsHelp(EGuildMarsHelpObjType.TEAM_REPAIR, _m_teamInfo.teamId, null);
        }
        /// <summary>
        /// 计算修复消耗
        /// </summary>
        private NPCommonCostItem _calculateRepairCost(long _num)
        {
            long costProperty = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_REPAIR_COST_PER);
            List<NPCommonCostItem> costItemList = GRefdataCoreMgr.instance.npGeneral.mars_explore_team_repair_unit_cost_list;
            NPCommonCostItem costItem = costItemList.GetFirst();
            if (null == costItem)
                return null;

            //仅取出玩家单兵实力的绝对值基数，用于计算维修成本倍率
            //https://www.teambition.com/task/695bddb5dc51a877c25e5b42
            long pureSoldierPower = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER);

            long realNum = costItem.getCount() * _num * (10000 - costProperty) / 10000;
            //计算单兵实力
            if (GRefdataCoreMgr.instance.npGeneral.repair_power_basic > 0 && pureSoldierPower > 0)
            {
                //根据队伍维修实力计算
                realNum = realNum * pureSoldierPower / GRefdataCoreMgr.instance.npGeneral.repair_power_basic;
            }

            if (realNum < 1) 
                realNum = 1;

            NPCommonCostItem realCostItem = new NPCommonCostItem(costItem);
            realCostItem.setCount(realNum);
            return realCostItem;
        }
        private void _onClickSpeedUp(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;
            
            GGUIWndMarsTimeSpeedUp.addNode(_m_teamInfo, _m_teamInfo);
        }
        private void _tryAddTeamEventListen()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            _m_teamInfo.onUIStateChg += refreshWnd;
            _m_teamInfo.onSoldierNumChg += refreshWnd;
        }
        private void _tryRemoveTeamEventListen()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            _m_teamInfo.onUIStateChg -= refreshWnd;
            _m_teamInfo.onSoldierNumChg -= refreshWnd;
        }
        private void _onTeamUIStateChg()
        {
            if (wnd == null || _m_teamInfo == null)
                return;

            EMarsExploreTeamUIState state = _m_teamInfo.getUIState();
            switch (state)
            {
                case EMarsExploreTeamUIState.Exploring:
                case EMarsExploreTeamUIState.Back:
                case EMarsExploreTeamUIState.Collecting:
                case EMarsExploreTeamUIState.Idle:
                case EMarsExploreTeamUIState.Empty:
                case EMarsExploreTeamUIState.Lock:
                    _onClickClose(null);
                    break;
                case EMarsExploreTeamUIState.IdleSoldierLoss:
                    wnd.setRepairing(false);
                    wnd.setCanGuildHelp(false);
                    break;
                case EMarsExploreTeamUIState.Repairing:
                    wnd.setRepairing(true);
                    wnd.setCanGuildHelp(false);
                    break;
                case EMarsExploreTeamUIState.CanAskHelp:
                    wnd.setRepairing(true);
                    wnd.setCanGuildHelp(true);
                    break;
            }
        }
        public bool checkCanCompleteNow(bool _checkCompleteNowCostEnough, bool _showUnableTip)
        {
            if (_m_teamInfo == null)
                return false;
            
            if (_m_teamInfo.state == EMarsExploreTeamState.REPAIR)
                return ((_IMarsCompleteNowObject)_m_teamInfo).checkCanCompleteNow(_checkCompleteNowCostEnough, _showUnableTip);
            
            long repairNum = _m_numSliderWnd?.currentCount ?? 0;
            if (repairNum <= 0)
                return false;
            
            return !_checkCompleteNowCostEnough || 
                   (GCommon.isItemEnough(_calculateRepairCost(repairNum), _showUnableTip) && 
                   GCommon.isItemEnough(completeNowCostItem, _showUnableTip));
        }
        public bool hasRemainTime
        {
            get
            {
                if (_m_teamInfo == null)
                    return false;

                return _m_teamInfo.state == EMarsExploreTeamState.REPAIR;
            }
        }
        public long remainTimeMs
        {
            get
            {
                if (_m_teamInfo == null)
                    return 0;

                if (_m_teamInfo.state == EMarsExploreTeamState.REPAIR)
                    return _m_teamInfo.stateRemainTimeMs;
                
                long sliderValue = _m_numSliderWnd?.currentCount ?? 0;
                long originTimeCost = sliderValue * GRefdataCoreMgr.instance.npGeneral.mars_explore_team_repair_unit_ms;
                if (originTimeCost < 0) originTimeCost = 0;
                long property = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_REPAIR_SPEED_PER);
                long realTimeCost = originTimeCost * 10000 / (10000 + property);
                if (realTimeCost < 0) realTimeCost = 0;
                return realTimeCost;
            }
        }
        private NPCommonCostItem _m_completeNowCostItem;
        public NPCommonCostItem completeNowCostItem
        {
            get
            {
                if (_m_teamInfo == null)
                    return null;
                
                if (_m_teamInfo.state == EMarsExploreTeamState.REPAIR)
                    return _m_teamInfo.completeNowCostItem;
                
                _m_completeNowCostItem ??= new NPCommonCostItem(ENPItemType.CURRENCY, (int)ECurrency.GEM, 0);
                long gemCost = MarsUtil.calculateGemCostForSpeedUpMs(remainTimeMs);
                _m_completeNowCostItem.setCount(gemCost);
                return _m_completeNowCostItem;
            }
        }
        public Action<Action<bool>> dealCompleteNowAction
        {
            get
            {
                if (_m_teamInfo == null)
                    return null;
                
                if (_m_teamInfo.state == EMarsExploreTeamState.REPAIR)
                    return ((_IMarsCompleteNowObject)_m_teamInfo).dealCompleteNowAction;
                
                return _reqCompleteNow;
            }
        }
        private void _reqCompleteNow(Action<bool> _complete)
        {
            if (_m_teamInfo == null)
            {
                _complete?.Invoke(false);
                return;
            }
            
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPGSClientListener.sendRequestByLog(new GC2GS_041_021_ReqStartAndFinishRepair(_m_teamInfo.teamId, _m_numSliderWnd?.currentCount ?? 0), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_021_RetStartAndFinishRepair>((_isSuc, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                    _complete?.Invoke(_isSuc);
                }));
        }
    }
}
