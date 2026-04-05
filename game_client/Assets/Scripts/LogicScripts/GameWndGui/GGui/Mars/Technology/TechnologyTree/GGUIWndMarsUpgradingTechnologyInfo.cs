using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技 - 升级中科技显示
    /// </summary>
    public class GGUIWndMarsUpgradingTechnologyInfo : _ANPGGUIBasicSubWnd<GGUIMonoMarsUpgradingTechnologyInfo>
    {
        // 升级中科技展示
        private GGUIWndMarsTechnologyInfo _m_wUpgradingTechnologyInfo;
        
        /// <summary>
        /// 倒计时进度条
        /// </summary>
        private NPGGUIWndProgress _m_wCountDownProgress;

        private MarsTechnologyInfo _m_iUpgradingTechnologyInfo;//升级中的科技信息
        
        public GGUIWndMarsUpgradingTechnologyInfo(GGUIMonoMarsUpgradingTechnologyInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            // 构建升级中科技信息窗口
            if (wnd.monoTechnologyInfo != null)
                _m_wUpgradingTechnologyInfo = new GGUIWndMarsTechnologyInfo(wnd.monoTechnologyInfo);
            
            if(wnd.countDownProgress != null)
                _m_wCountDownProgress = new NPGGUIWndProgress(wnd.countDownProgress);
            
            ALUGUICommon.combineBtnClick(wnd.btnSpeedUp, _onClickSpeedUp);
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onClickCancel);
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickSure);
            ALUGUICommon.combineBtnClick(wnd.btnAssist, _onClickAssist);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSpeedUp, _onClickSpeedUp);
                ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onClickCancel);
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickSure);
                ALUGUICommon.uncombineBtnClick(wnd.btnAssist, _onClickAssist);
            }
            
            _m_wUpgradingTechnologyInfo?.discard();
            _m_wUpgradingTechnologyInfo = null;
            
            _m_wCountDownProgress?.discard();
            _m_wCountDownProgress = null;
        }
        
        protected override void _onShowWnd()
        {
            refreshWnd();

            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_UPGRADING_TECHNOLOGY_CHG, refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _refreshWnd);
            NPPlayer.instance.marsComp.technologySubComponent.upgradingTechnologyCountDownChg += _onCountDownChg;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingStateChg;
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_UPGRADING_TECHNOLOGY_CHG, refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _refreshWnd);
            NPPlayer.instance.marsComp.technologySubComponent.upgradingTechnologyCountDownChg -= _onCountDownChg;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg -= _onBuildingStateChg;
            
            _m_wUpgradingTechnologyInfo?.hideWnd();
            _m_wCountDownProgress?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wUpgradingTechnologyInfo?.resetWnd();
            _m_wCountDownProgress?.resetWnd();
        }
        
        public void refreshWnd()
        {
            _m_iUpgradingTechnologyInfo = NPPlayer.instance.marsComp.technologySubComponent.nowdUpgradingOrUpgradedTechnologyInfo;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_iUpgradingTechnologyInfo == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasUpgradingTechnologyShow, false);
                ALUGUICommon.setGameObjEnable(wnd.noUpgradingTechnologyShow, true);
                
                _m_wUpgradingTechnologyInfo?.hideWnd();
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasUpgradingTechnologyShow, true);
                ALUGUICommon.setGameObjEnable(wnd.noUpgradingTechnologyShow, false);
                
                // 刷新升级中科技信息显示
                if (_m_wUpgradingTechnologyInfo != null)
                {
                    _m_wUpgradingTechnologyInfo.showWnd();
                    _m_wUpgradingTechnologyInfo.setData(_m_iUpgradingTechnologyInfo);
                }
                
                bool canAssist = NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding()
                                 && _m_wUpgradingTechnologyInfo != null && _m_wUpgradingTechnologyInfo.technologyInfo != null && _m_wUpgradingTechnologyInfo.technologyInfo.state == EMarsTechnologyState.UPGRADEING && _m_wUpgradingTechnologyInfo.technologyInfo.guildHelpId <= 0;

                ALUGUICommon.setGameObjEnable(wnd.canAssistShowGos, canAssist);
                ALUGUICommon.setGameObjEnable(wnd.canAssistHideGos, !canAssist);
                
                // 刷新倒计时进度条显示
                _refreshCountDown();
            }
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
                    refreshWnd();
            }
        }
        private void _refreshCountDown()
        {
            if(_m_iUpgradingTechnologyInfo == null)
                return;

            long totalTimeMs = _m_iUpgradingTechnologyInfo.beforeReductionTotalTimeMs;
            long remainingTimeMs = _m_iUpgradingTechnologyInfo.remainingUpgradeTimeMs;
            long passedTimeMs = totalTimeMs - remainingTimeMs;
            
            if (_m_wCountDownProgress != null)
            {
                _m_wCountDownProgress.showWnd();
                if(totalTimeMs == 0)
                    _m_wCountDownProgress.setProgress(1);
                else
                    _m_wCountDownProgress.setProgress(1f * passedTimeMs / totalTimeMs);
                
                _m_wCountDownProgress.setProgressTxt(TimeUtil.millisecondsToTime_dhms(remainingTimeMs), remainingTimeMs <= 0);
            }

            if(wnd != null)
               ALUGUICommon.setLabelTxt(wnd.tmpCountDown, TimeUtil.millisecondsToTime_dhms(remainingTimeMs));
        }
        
        private void _onCountDownChg()
        {
            _refreshCountDown();
        }
        
        /// <summary>
        /// 点击加速按钮
        /// </summary>
        private void _onClickSpeedUp(GameObject _go)
        {
            if(_m_iUpgradingTechnologyInfo == null)
                return;
            
            GGUIWndMarsTimeSpeedUp.addNode(_m_iUpgradingTechnologyInfo, _m_iUpgradingTechnologyInfo);
        }

        /// <summary>
        /// 点击取消按钮
        /// </summary>
        private void _onClickCancel(GameObject _go)
        {
            if(_m_iUpgradingTechnologyInfo == null || !_m_iUpgradingTechnologyInfo.isUpgrading)
                return;
            
            // 点击取消, 弹出提示弹窗
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.mars_technology_cancelUpgradeWarningTip_str, _m_iUpgradingTechnologyInfo?.technologyRefObj?.transName), 
                TextTranslate.instance.getLanguage(TransKeyConst.cancel), null, 
                TextTranslate.instance.getLanguage(TransKeyConst.confirm), 
                () =>
                {
                    // 确认取消升级
                    NPPlayer.instance.marsComp.technologySubComponent.reqCancelTechnologyUpgrade(_m_iUpgradingTechnologyInfo.technologyId);
                });
        }

        /// <summary>
        /// 点击确定按钮
        /// </summary>
        private void _onClickSure(GameObject _go)
        {
            if(_m_iUpgradingTechnologyInfo == null || !_m_iUpgradingTechnologyInfo.isUpgraded)
                return;
            
            NPPlayer.instance.marsComp.technologySubComponent.reqConfirmUpgradeTechnologyLvl(_m_iUpgradingTechnologyInfo.technologyId);
        }
        
        
        private void _onClickAssist(GameObject _obj)
        {
            bool canAssist = NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding()
                             && _m_wUpgradingTechnologyInfo != null && _m_wUpgradingTechnologyInfo.technologyInfo != null&& _m_wUpgradingTechnologyInfo.technologyInfo.guildHelpId <= 0;
            if(!canAssist)
                return;
            NPPlayer.instance.guildMarsHelpComp.reqSendMarsHelp(EGuildMarsHelpObjType.TECH_UP, _m_iUpgradingTechnologyInfo.technologyId,
                _suc =>
                {
                    _refreshWnd();
                });
        }
    }
}