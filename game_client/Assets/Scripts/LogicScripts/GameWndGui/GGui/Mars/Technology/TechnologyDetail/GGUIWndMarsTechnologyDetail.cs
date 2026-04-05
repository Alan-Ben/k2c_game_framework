using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技详情窗口
    /// </summary>
    public class GGUIWndMasrTechnologyDetail : _ANPGGUIBasicWnd<GGUIMonoMasrTechnologyDetail>
    {
        private static GGUIWndMasrTechnologyDetail _g_instance;
        public static GGUIWndMasrTechnologyDetail instance { get { return _g_instance ??= new GGUIWndMasrTechnologyDetail(); } }

        private long _m_lTechnologyId;//科技id
        private MarsTechnologyInfo _m_iTechnologyInfo;
        private EMarsTechnologyState _m_eTechnologyState;
        private List<MasrPropertyShowInfo> _m_lPropertyShowInfoList = new List<MasrPropertyShowInfo>();//属性显示信息列表
        private List<_IConditionDescShow> _m_lConditionDescShowList = new List<_IConditionDescShow>();//条件描述显示列表

        private GGUIWndMarsTechnologyInfo _m_wTechnologyInfo;
        private GGUIWndMarsPropertyShowItemContainer _m_wPropertyContainer;
        private GGUIWndMarsCostItemContainer _m_wCostItemContainer;
        private NPGGUIWndCommonItem _m_wCompleteNowCostItemWnd;//立即完成消耗物品窗口
        private GGUIWndConditionDescContainer _m_wConditionContainer;
        private GGUIWndSimpleCostButton _m_wUpgradeImmediatelyBtn;
        private NPGGUIWndProgress _m_wCountDownProgress;
        
        public GGUIWndMasrTechnologyDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMasrTechnologyDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMasrTechnologyDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建科技信息窗口
            if (wnd.monoTechnologyInfo != null)
                _m_wTechnologyInfo = new GGUIWndMarsTechnologyInfo(wnd.monoTechnologyInfo);

            // 构建属性列表容器
            if (wnd.monoPropertyContainer != null)
                _m_wPropertyContainer = new GGUIWndMarsPropertyShowItemContainer(wnd.monoPropertyContainer);

            // 构建消耗物品容器
            if (wnd.monoCostItemContainer != null)
                _m_wCostItemContainer = new GGUIWndMarsCostItemContainer(wnd.monoCostItemContainer);
            
            if(wnd.monoCompleteNowCostItem != null)
                _m_wCompleteNowCostItemWnd = new NPGGUIWndCommonItem(wnd.monoCompleteNowCostItem);

            // 构建条件描述容器
            if (wnd.monoConditionContainer != null)
                _m_wConditionContainer = new GGUIWndConditionDescContainer(wnd.monoConditionContainer);

            if (wnd.monoUpgradeCountDownProgress)
                _m_wCountDownProgress = new NPGGUIWndProgress(wnd.monoUpgradeCountDownProgress);
            
            // 绑定关闭按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            
            // 绑定属性总览按钮
            ALUGUICommon.combineBtnClick(wnd.btnPropertyOverview, _btnPropertyOverviewClick);

            // 绑定升级按钮
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onUpgradeBtnClick);

            // 绑定取消升级按钮
            ALUGUICommon.combineBtnClick(wnd.btnCancelUpgrade, _onCancelUpgradeBtnClick);

            // 绑定升级加速按钮
            ALUGUICommon.combineBtnClick(wnd.btnUpgradeSpeedUp, _onUpgradeSpeedUpBtnClick);
            
            // 升级完成确认按钮
            ALUGUICommon.combineBtnClick(wnd.btnUpgradeCompleteConfirm, _onUpgradeCompleteConfirm);
            
            // 立即完成按钮
            ALUGUICommon.combineBtnClick(wnd.btnCompleteNow, _onCompleteNowBtnClick);
            
            ALUGUICommon.combineBtnClick(wnd.btnAssist, _onClickAssist);
        }


        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                // 解绑按钮
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnPropertyOverview, _btnPropertyOverviewClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onUpgradeBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnCancelUpgrade, _onCancelUpgradeBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnUpgradeSpeedUp, _onUpgradeSpeedUpBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnUpgradeCompleteConfirm, _onUpgradeCompleteConfirm);
                ALUGUICommon.uncombineBtnClick(wnd.btnCompleteNow, _onCompleteNowBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnAssist, _onClickAssist);
            }

            // 销毁子窗口
            _m_wTechnologyInfo?.discard();
            _m_wTechnologyInfo = null;

            _m_wPropertyContainer?.discard();
            _m_wPropertyContainer = null;

            _m_wCostItemContainer?.discard();
            _m_wCostItemContainer = null;

            _m_wCompleteNowCostItemWnd?.discard();
            _m_wCompleteNowCostItemWnd = null;
            
            _m_wConditionContainer?.discard();
            _m_wConditionContainer = null;

            if (_m_wUpgradeImmediatelyBtn != null)
            {
                _m_wUpgradeImmediatelyBtn.onClickButton -= _onUpgradeImmediatelyBtnClick;
                _m_wUpgradeImmediatelyBtn.discard();
                _m_wUpgradeImmediatelyBtn = null;
            }
            
            _m_wCountDownProgress?.discard();
            _m_wCountDownProgress = null;

            _m_iTechnologyInfo = null;
            
            _m_lPropertyShowInfoList?.Clear();
            _m_lPropertyShowInfoList = null;
            
            _m_lConditionDescShowList?.Clear();
            _m_lConditionDescShowList = null;
        }


        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _onTechnologyChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_TECHNOLOGY_DETAIL_UPGRADE, _onSimulateClickUpgrade);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _refreshUpgradeInfo);
            NPPlayer.instance.marsComp.technologySubComponent.upgradingTechnologyCountDownChg += _upgradingCountDownChg;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingStateChg;
        }


        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _onTechnologyChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_TECHNOLOGY_DETAIL_UPGRADE, _onSimulateClickUpgrade);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _refreshUpgradeInfo);
            NPPlayer.instance.marsComp.technologySubComponent.upgradingTechnologyCountDownChg -= _upgradingCountDownChg;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg -= _onBuildingStateChg;
            
            // 隐藏子窗口
            _m_wTechnologyInfo?.hideWnd();
            _m_wPropertyContainer?.hideWnd();
            _m_wCostItemContainer?.hideWnd();
            _m_wCompleteNowCostItemWnd?.hideWnd();
            _m_wConditionContainer?.hideWnd();
            _m_wUpgradeImmediatelyBtn?.hideWnd();
            _m_wCountDownProgress?.hideWnd();
        }


        protected override void _onReset()
        {
            // 重置子窗口
            _m_wTechnologyInfo?.resetWnd();
            _m_wPropertyContainer?.resetWnd();
            _m_wCostItemContainer?.resetWnd();
            _m_wCompleteNowCostItemWnd?.resetWnd();
            _m_wConditionContainer?.resetWnd();
            _m_wUpgradeImmediatelyBtn?.resetWnd();
            _m_wCountDownProgress?.resetWnd();
        }

        public void setData(long _technologyId)
        {
            _m_lTechnologyId = _technologyId;
            _m_iTechnologyInfo = NPPlayer.instance?.marsComp?.technologySubComponent.getTechnologyInfoById(_m_lTechnologyId);
            if(_m_iTechnologyInfo == null)
                _m_iTechnologyInfo = new MarsTechnologyInfo(_m_lTechnologyId);

            _updateShowInfo();
            _refreshWnd();
        }
        
        public void setData(MarsTechnologyRefObj _technologyRefObj)
        {
            _m_lTechnologyId = _technologyRefObj?.id ?? 0;
            _m_iTechnologyInfo = NPPlayer.instance?.marsComp?.technologySubComponent.getTechnologyInfoById(_m_lTechnologyId);
            if(_m_iTechnologyInfo == null)
                _m_iTechnologyInfo = new MarsTechnologyInfo(_technologyRefObj);
            
            _updateShowInfo();
            _refreshWnd();
        }

        private void _updateShowInfo()
        {
            _updateState();
            _updatePropertyShowInfo();
            _updateConditionDescShowInfo();
        }
        
        /// <summary>
        /// 更新状态
        /// </summary>
        private void _updateState()
        {
            _m_eTechnologyState = _m_iTechnologyInfo?.state ?? EMarsTechnologyState.NONE;
        }

        /// <summary>
        /// 更新属性显示
        /// </summary>
        private void _updatePropertyShowInfo()
        {
            if (_m_lPropertyShowInfoList == null)
                _m_lPropertyShowInfoList = new List<MasrPropertyShowInfo>();
            _m_lPropertyShowInfoList.Clear();

            if (_m_iTechnologyInfo == null || _m_iTechnologyInfo.technologyRefObj == null || _m_iTechnologyInfo.technologyLvlRefObj == null)
                return;

            List<_IPropertyShow> allPropertyList = new List<_IPropertyShow>();
            
            Dictionary<_IPropertyShow, long> nowLevelPropertyValueDict = new Dictionary<_IPropertyShow, long>();
            _m_iTechnologyInfo.technologyLvlRefObj.addPropertyToCollections(nowLevelPropertyValueDict, allPropertyList);
            
            MarsTechnologyLevelRefObj nextLevelRefObj = GRefdataCoreMgr.instance.getMarsTechnologyLevelRefObj(_m_lTechnologyId, _m_iTechnologyInfo.lvl + 1);
            if (nextLevelRefObj == null)//最高等级时
            {
                foreach (var propertyShow in allPropertyList)
                {
                    if(propertyShow == null)
                        continue;
                    
                    nowLevelPropertyValueDict.TryGetValue(propertyShow, out long nowValue);
                    _m_lPropertyShowInfoList.Add(new MasrPropertyShowInfo()
                    {
                        propertyShow = propertyShow,
                        nowValue = nowValue,
                        nextValue = nowValue,
                    });
                }
                
                return;
            }
            
            Dictionary<_IPropertyShow, long> nextLevelPropertyValueDict = new Dictionary<_IPropertyShow, long>();
            nextLevelRefObj.addPropertyToCollections(nextLevelPropertyValueDict, allPropertyList);
            // 火星实力最后一个展示
            if (allPropertyList.Contains(MarsPowerPropertyShow.instance))
            {
                allPropertyList.Remove(MarsPowerPropertyShow.instance);
                allPropertyList.Add(MarsPowerPropertyShow.instance);
            }
            foreach (var propertyShow in allPropertyList)
            {
                if(propertyShow == null)
                    continue;
                    
                nowLevelPropertyValueDict.TryGetValue(propertyShow, out long nowValue);
                nextLevelPropertyValueDict.TryGetValue(propertyShow, out long nextValue);
                _m_lPropertyShowInfoList.Add(new MasrPropertyShowInfo()
                {
                    propertyShow = propertyShow,
                    nowValue = nowValue,
                    nextValue = nextValue,
                });
            }
        }

        /// <summary>
        /// 更新条件描述显示
        /// </summary>
        private void _updateConditionDescShowInfo()
        {
            if(_m_lConditionDescShowList == null)
                _m_lConditionDescShowList = new List<_IConditionDescShow>();
            _m_lConditionDescShowList.Clear();
            
            if (_m_iTechnologyInfo == null || _m_iTechnologyInfo.technologyRefObj == null || _m_iTechnologyInfo.technologyLvlRefObj == null)
                return;
            
            if (_m_iTechnologyInfo.technologyLvlRefObj.condition_id_list != null)
            {
                MarsBuildingConditionRefObj conditionRef = null;
                foreach (var conditionId in _m_iTechnologyInfo.technologyLvlRefObj.condition_id_list)
                {
                    conditionRef = GRefdataCoreMgr.instance.marsBuildingConditionRefCore.getRef(conditionId);
                    if (conditionRef != null)
                        _m_lConditionDescShowList.Add(conditionRef);
                }
            }

            foreach (var parentTechnologyRefObj in _m_iTechnologyInfo.technologyRefObj.parentTechnologyList)
            {
                _m_lConditionDescShowList.Add(new MarsTechnologyLvlCondition(parentTechnologyRefObj, 1));
            }
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            _refreshTechnologyInfo();
            _refreshPropertyList();
            _refreshUpgradeCostItemList();
            _refreshConditionList();
            _refreshUpgradeInfo();
            _refreshCountDownProgress();
            _refreshCompleteNow();
        }

        private void _refreshTechnologyInfo()
        {
            if(wnd == null || _m_iTechnologyInfo == null)
                return;
            
            if (_m_wTechnologyInfo != null)
            {
                _m_wTechnologyInfo.setData(_m_iTechnologyInfo);
                _m_wTechnologyInfo.showWnd();
            }
            
            // 当前科技等级配表数据
            if (_m_iTechnologyInfo.technologyLvlRefObj != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtNowLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_iTechnologyInfo.technologyLvlRefObj.level));
                MarsTechnologyLevelRefObj nextLevelRefObj = GRefdataCoreMgr.instance.getMarsTechnologyLevelRefObj(_m_lTechnologyId, _m_iTechnologyInfo.technologyLvlRefObj.level + 1);
                ALUGUICommon.setLabelTxt(wnd.txtNextLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, nextLevelRefObj?.level ?? 0));
            }
        }

        /// <summary>
        /// 刷新属性列表显示
        /// </summary>
        private void _refreshPropertyList()
        {
            if (_m_wPropertyContainer != null)
            {
                _m_wPropertyContainer.showWnd();
                _m_wPropertyContainer.setData(_m_lPropertyShowInfoList);
            }
        }

        /// <summary>
        /// 刷新升级消耗物品列表
        /// </summary>
        private void _refreshUpgradeCostItemList()
        {
            if(_m_wCostItemContainer == null || _m_iTechnologyInfo == null)
                return;
            
            _m_wCostItemContainer.showWnd();
            _m_wCostItemContainer.setData(_m_iTechnologyInfo.upgradeConsumeList);
        }

        /// <summary>
        /// 刷新升级条件描述列表显示
        /// </summary>
        private void _refreshConditionList()
        {
            if (_m_wConditionContainer != null)
            {
                _m_wConditionContainer.showWnd();
                _m_wConditionContainer.setShowData(_m_lConditionDescShowList);
            }
        }

        /// <summary>
        /// 刷新升级相关信息
        /// </summary>
        private void _refreshUpgradeInfo()
        {
            if(_m_iTechnologyInfo == null || _m_iTechnologyInfo.technologyRefObj == null || _m_iTechnologyInfo.technologyLvlRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtUpgradeOriTime,
                TextTranslate.instance.getLanguage(TransKeyConst.mars_technology_upgradeOriginalTime_str,
                    TimeUtil.millisecondsToTime_dhms(_m_iTechnologyInfo.technologyLvlRefObj.upgrade_time_sec * 1000)));
            ALUGUICommon.setLabelTxt(wnd.txtUpgradeRealTime, TextTranslate.instance.getLanguage(TimeUtil.millisecondsToTime_dhms(_m_iTechnologyInfo.technologyLvlRefObj.upgradeRealTimeMs)));
            
            bool canUpgrade = _m_iTechnologyInfo.checkCanUpgrade();
            ALUGUICommon.setGameObjEnable(wnd.canUpgradeShowGoList, canUpgrade);
            ALUGUICommon.setGameObjEnable(wnd.cannotUpgradeShowGoList, !canUpgrade);
            GGameCommonInfo.grayImage(wnd.cannotUpgradeGrayList, !canUpgrade);
            
            bool canAssist = NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding()
                             && _m_iTechnologyInfo != null  && _m_iTechnologyInfo.state == EMarsTechnologyState.UPGRADEING && _m_iTechnologyInfo.guildHelpId <= 0;

            ALUGUICommon.setGameObjEnable(wnd.canAssistShowGos, canAssist);
            ALUGUICommon.setGameObjEnable(wnd.canAssistHideGos, !canAssist);
        }
        
        /// <summary>
        /// 建筑状态变化事件处理
        /// </summary>
        private void _onBuildingStateChg(long _buildingId, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
        {
            var buildingRef = GRefdataCoreMgr.instance.marsBuildingRefCore.getRef(_buildingId);
            if (buildingRef != null && buildingRef.building_type == Common.MarsEnum.EMarsBuildingType.HELP)
            {
                if (_newState == MarsBuildingInfo.StateType.Normal)
                    _refreshUpgradeInfo();
            }
        }
        /// <summary>
        /// 刷新立即完成显示
        /// </summary>
        private void _refreshCompleteNow()
        {
            if(wnd == null || _m_iTechnologyInfo == null)
                return;

            bool canCompleteNow = _m_iTechnologyInfo.checkCanCompleteNow(true, false);
            ALUGUICommon.setUIObjScale(wnd.canCompleteNowShowGoList, canCompleteNow ? 1 : 0);
            ALUGUICommon.setUIObjScale(wnd.cannotCompleteNowShowGoList, canCompleteNow ? 0 : 1);
            GGameCommonInfo.grayImage(wnd.cannotCompleteNowGrayList, !canCompleteNow);

            if (_m_wCompleteNowCostItemWnd != null)
            {
                _m_wCompleteNowCostItemWnd.showWnd();
                _m_wCompleteNowCostItemWnd.setItem(_m_iTechnologyInfo.completeNowCostItem);
            }
        }

        /// <summary>
        /// 刷新倒计时进度条
        /// </summary>
        private void _refreshCountDownProgress()
        {
            if(_m_iTechnologyInfo == null)
                return;
            
            long totalTimeMs = _m_iTechnologyInfo.beforeReductionTotalTimeMs;
            long remainTimeMs = _m_iTechnologyInfo.remainingUpgradeTimeMs;
            if (_m_wCountDownProgress != null)
            {
                _m_wCountDownProgress.showWnd();
                if (totalTimeMs == 0)
                    _m_wCountDownProgress.setProgress(1);
                else
                    _m_wCountDownProgress.setProgress((totalTimeMs - remainTimeMs) / (float)totalTimeMs);
                _m_wCountDownProgress.setProgressTxt(TimeUtil.millisecondsToTime_dhms(remainTimeMs), remainTimeMs <= 0);
            }
        }
        
        #region 按钮事件


        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_TECHNOLOGY_DETAIL);
        }

        /// <summary>
        /// 属性概览按钮点击事件
        /// </summary>
        /// <param name="_go"></param>
        private void _btnPropertyOverviewClick(GameObject _go)
        {
            if(_m_iTechnologyInfo == null || _m_iTechnologyInfo.technologyRefObj == null)
                return;
            
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsTechnologyLvlProperty.instance, () =>
            {
                GGUIWndMarsTechnologyLvlProperty.instance.setData(_m_iTechnologyInfo.technologyRefObj, _m_iTechnologyInfo.lvl);
                GGUIWndMarsTechnologyLvlProperty.instance.showWnd();
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_TECHNOLOGY_LVL_PROPERTY, false, false);
        }

        /// <summary>
        /// 升级按钮点击事件
        /// </summary>
        private void _onUpgradeBtnClick(GameObject _go)
        {
            if(_m_iTechnologyInfo == null)
                return;

            if (!_m_iTechnologyInfo.checkCanUpgrade(true))
                return;
            
            NPPlayer.instance.marsComp.technologySubComponent.reqUpgradeTechnologyLvl(_m_lTechnologyId);
        }

        /// <summary>
        /// 模拟点击升级按钮
        /// </summary>
        private void _onSimulateClickUpgrade()
        {
            if (wnd == null) return;
            _onUpgradeBtnClick(wnd.btnUpgrade);
        }

        /// <summary>
        /// 立即升级按钮点击事件
        /// </summary>
        private void _onUpgradeImmediatelyBtnClick(GGUIWndSimpleCostButton _btnWnd)
        {
            if(_m_iTechnologyInfo == null)
                return;
            
            bool needShowWarningTip = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_TIME_COMPLETE_NOW_CONFIRM);
            if (needShowWarningTip)
            {
                GGUIWndMarsTimeCompleteNowConfim.addNode(_m_iTechnologyInfo);
            }
            else
            {
                _m_iTechnologyInfo.reqCompleteNow(null);
            }
        }

        /// <summary>
        /// 取消升级按钮点击事件
        /// </summary>
        private void _onCancelUpgradeBtnClick(GameObject _go)
        {
            if(_m_iTechnologyInfo == null || !_m_iTechnologyInfo.isUpgrading)
                return;
            
            // 点击取消, 弹出提示弹窗
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.mars_technology_cancelUpgradeWarningTip_str, _m_iTechnologyInfo?.technologyRefObj?.transName), 
                TextTranslate.instance.getLanguage(TransKeyConst.cancel), null, 
                TextTranslate.instance.getLanguage(TransKeyConst.confirm), 
                () =>
                {
                    // 确认取消升级
                    NPPlayer.instance.marsComp.technologySubComponent.reqCancelTechnologyUpgrade(_m_iTechnologyInfo.technologyId);
                });
        }

        /// <summary>
        /// 升级加速按钮点击事件
        /// </summary>
        private void _onUpgradeSpeedUpBtnClick(GameObject _go)
        {
            GGUIWndMarsTimeSpeedUp.addNode(_m_iTechnologyInfo, _m_iTechnologyInfo);
        }

        /// <summary>
        /// 升级完成确认按钮点击事件
        /// </summary>
        /// <param name="_go"></param>
        private void _onUpgradeCompleteConfirm(GameObject _go)
        {
            if(_m_iTechnologyInfo == null || !_m_iTechnologyInfo.isUpgraded)
                return;
            
            NPPlayer.instance.marsComp.technologySubComponent.reqConfirmUpgradeTechnologyLvl(_m_iTechnologyInfo.technologyId);
        }

        /// <summary>
        /// 点击立即完成按钮
        /// </summary>
        private void _onCompleteNowBtnClick(GameObject _go)
        {
            if(_m_iTechnologyInfo == null || !_m_iTechnologyInfo.checkCanCompleteNow(false, true))
                return;
            
            bool needShowWarningTip = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_TIME_COMPLETE_NOW_CONFIRM);
            if (needShowWarningTip)
            {
                GGUIWndMarsTimeCompleteNowConfim.addNode(_m_iTechnologyInfo);
            }
            else
            {
                _m_iTechnologyInfo.reqCompleteNow(null);
            }
        }
            
        private void _onClickAssist(GameObject _obj)
        {
            bool canAssist = NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding()
                             && _m_iTechnologyInfo != null && _m_iTechnologyInfo.state == EMarsTechnologyState.UPGRADEING&&  _m_iTechnologyInfo.guildHelpId <= 0;
            if(!canAssist)
                return;
            NPPlayer.instance.guildMarsHelpComp.reqSendMarsHelp(EGuildMarsHelpObjType.TECH_UP, _m_iTechnologyInfo.technologyId,
                _suc =>
                {
                    _refreshWnd();
                });
        }
        
        #endregion

        #region 消息监听

        /// <summary>
        /// 科技变化消息回调
        /// </summary>
        /// <param name="_objs">参数：_objs[0] 为 MarsTechnologyInfo</param>
        private void _onTechnologyChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is MarsTechnologyInfo technologyInfo))
                return;

            // 若id相同的情况, 直接更新数据(不能判断id不相同就不更新, 因为其他科技变化可能会影响当前科技的升级条件达成状态)
            if (technologyInfo.technologyId == _m_lTechnologyId)
                _m_iTechnologyInfo = technologyInfo;
         
            _updateShowInfo();
            _refreshWnd();
        }
        
        /// <summary>
        /// 升级倒计时变化
        /// </summary>
        private void _upgradingCountDownChg()
        {
            _refreshCountDownProgress();//刷新倒计时进度条
            _refreshCompleteNow();//刷新立即完成显示
        }

        /// <summary>
        /// 通用道具数量变化回调
        /// </summary>
        private void _onCommonItemCountChg()
        {
            // 可能影响升级消耗物品数量显示
            _refreshUpgradeCostItemList();
            _refreshUpgradeInfo();// 可能影响升级按钮可用状态显示
            
            // 可能影响立即完成消耗物品数量显示
            _refreshCompleteNow();
        }
        
        #endregion
    }
}
