using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConsortEnum;
using Common.TravelEnum;
using Common.TravelObj;
using CommonEnum;
using GS2GC.p021_PlayerInfo;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历主界面
    /// </summary>
    public class GGUIWndTravelMain: _ANPGGUIBasicWnd<GGUIMonoTravelMain>
    {
        private static GGUIWndTravelMain _g_instance = new GGUIWndTravelMain();
        public static GGUIWndTravelMain instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new  GGUIWndTravelMain();
                return _g_instance;
            }
        }
        
        private _ITravelController _m_travelController;
        
        private NPGGUIWndCommonToggleEx _m_togAllTravel;
        private bool _m_isAllTravel = false;
        private NPGGUIWndCommonCountDown _m_timeDown;
        private int _m_inputMaskSerialize = -1;
        private GGUIWndCommonLazyCDCountResume _m_energyCount;

        private int _m_iWndShowSerialize = -1;
        
        private GGUIWndSimpleVideo _m_wVideoPlayer;

        public GGUIWndTravelMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTravelMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTravelMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAVEL, _onSimulateClickTravel);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAVEL_ONE_KEY_TOGGLE, _onSimulateClickOneKeyToggle);
        }

        protected override void _onHideWnd()
        {
            _m_iWndShowSerialize = ALSerializeOpMgr.next();
            
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAVEL, _onSimulateClickTravel);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAVEL_ONE_KEY_TOGGLE, _onSimulateClickOneKeyToggle);
            
            _m_energyCount?.hideWnd();
            _m_wVideoPlayer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wVideoPlayer?.reset();
        }

        protected override void _onDiscard()
        {
            _m_togAllTravel?.discard();
            
            _m_timeDown?.discard();
            _m_timeDown = null;
            _m_isAllTravel = false;

            _m_inputMaskSerialize = -1;
            
            _m_energyCount?.discard();
            _m_energyCount = null;

            _m_wVideoPlayer?.discard();
            _m_wVideoPlayer = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
            ALUGUICommon.combineBtnClick(wnd.btnTravel, _clickTravel);
            ALUGUICommon.combineBtnClick(wnd.btnConsort, _clickConsort);
            _m_isAllTravel = AccountSettingMgr.instance.accountSetting.isAllTravel;
            if (null != wnd.togAllTravel)
            {
                _m_togAllTravel = new NPGGUIWndCommonToggleEx(wnd.togAllTravel);
                _m_togAllTravel.setSelected(_m_isAllTravel);
                _m_togAllTravel.clickDelegate += _clickAllTravel;
            }

            if (null != wnd.energyCount)
            {
                _m_energyCount = new GGUIWndCommonLazyCDCountResume(wnd.energyCount);
            }

            if(wnd.monoVideoPlayer != null)
                _m_wVideoPlayer = new GGUIWndSimpleVideo(wnd.monoVideoPlayer);
        }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        public void setTravelController(_ITravelController _controller)
        {
            _m_travelController = _controller;
        }
        
        private void _clickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MAIN_TRAVEL);
        }

        /// <summary>
        /// 显示妃子列表
        /// </summary>
        /// <param name="obj"></param>
        private void _clickConsort(GameObject obj)
        {
            // QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTarvelConsortList.instance, UINodeTagConst.C_MAIN_TRAVEL_CONSORT_LIST);
            QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndTarvelConsortList.instance,
                EUIQueueStageType.MAIN, UINodeTagConst.C_MAIN_TRAVEL_CONSORT_LIST, null, () =>
                {
                    GGUIWndTarvelConsortList.instance.showWnd();
                }, true, false);
        }

        /// <summary>
        /// 显示事件CenterTip
        /// </summary>
        public void showEventCenterTip(_ATravelEventInfo _travelEventInfo, Action _showDone)
        {
            if (wnd == null || !isShow)
            {
                _showDone?.Invoke();
                return;
            }
            
            long showSerialize = _m_iWndShowSerialize;
            Action afterDelay = () =>
            {
                if (_travelEventInfo != null && _travelEventInfo.travelEventTypeRefObj != null)
                {
                    NPGUIAddSceneCenterTip.instance.showTextTip(_travelEventInfo.getEventCenterTipText(), _travelEventInfo.travelEventTypeRefObj.event_center_tip_id);
                    NPCenterTipsRefObj centerTipsRefObj = GRefdataCoreMgr.instance.tipMap.getRef(_travelEventInfo.travelEventTypeRefObj.event_center_tip_id);
                    if (centerTipsRefObj != null)
                    {
                        CommonTaskController.CommonActionAddMonoTask(() =>
                        {
                            if (showSerialize != _m_iWndShowSerialize)
                                return;

                            _showDone?.Invoke();
                        }, centerTipsRefObj.disable_time);
                    }
                    else
                    {
                        _showDone?.Invoke();
                    }
                }
                else
                {
                    _showDone?.Invoke();
                }
            };
            
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                if (showSerialize != _m_iWndShowSerialize)
                    return;

                afterDelay();
            }, wnd.showEventCenterTipDelayTimeS);
        }

        #region 首次进入表现

        /// <summary>
        /// 播放首次进入视频
        /// </summary>
        public void playFirstEnterVideo(Action _onPlayDone)
        {
            if(wnd == null || !isShow)
            {
                _onPlayDone?.Invoke();
                return;
            }
            
            playVideo(wnd.firstEnterVideoClipIndex, false, _onPlayDone);
        }

        /// <summary>
        /// 显示首次进入对话
        /// </summary>
        public void showFirstEnterDialog(Action _showDone)
        {
            if(wnd == null || !isShow || wnd.firstEnterDialogId <= 0)
            {
                _showDone?.Invoke();
                return;
            }
            
            GCommon.enterDialogueNode(wnd.firstEnterDialogId, _showDone);
        }

        #endregion
        
        #region 视频播放

        public void playVideo(GVideoClipIndex _videoClipIndex, bool _canSkip, Action _onVideoEnd)
        {
            if(_m_wVideoPlayer == null || _videoClipIndex == null || !_videoClipIndex.isValid() || !isShow)
            {
                _onVideoEnd?.Invoke();
                return;
            }
            
            _m_wVideoPlayer.showWnd();
            _m_wVideoPlayer.setVideoClip(_videoClipIndex);
            _m_wVideoPlayer.playVideo();
            _m_wVideoPlayer.monitorPlay(() =>
            {
                _m_wVideoPlayer?.hideWnd();
                _onVideoEnd?.Invoke();
            });

            // 若可以跳过, 展示跳过窗口
            if (_canSkip)
            {
                // 展示跳过按钮窗口
                GGUIWndTravelProcessSkip.instance.setInfo(() =>
                {
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_RANDOM_TRAVEL_PROCESS);
                    GGUIWndTravelProcessSkip.instance.hideWnd();
                    
                    // 直接隐藏视频窗口, 不需要等视频播放结束
                    _m_wVideoPlayer?.hideWnd();
                });
                GUISceneMain.instance.showAddWnd(GGUIWndTravelProcessSkip.instance, GGUIWndTravelProcessSkip.instance.showWnd);
            }
        }

        /// <summary>
        /// 播放开始游历视频
        /// </summary>
        /// <param name="_videoClipIndex"></param>
        /// <param name="_onVideoEnd"></param>
        public void playStartTravelVideo(GVideoClipIndex _videoClipIndex, Action _onVideoEnd)
        {
            if(_m_wVideoPlayer == null || _videoClipIndex == null || !_videoClipIndex.isValid() || wnd == null || !isShow)
            {
                _onVideoEnd?.Invoke();
                return;
            }
            
            long wndShowSerialize = _m_iWndShowSerialize;
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(wndShowSerialize != _m_iWndShowSerialize)
                    return;
                
                playVideo(_videoClipIndex, true, _onVideoEnd);
            }, wnd.startTravelVideoDelayTimeS);
        }

        /// <summary>
        /// 播放降落视频
        /// </summary>
        public void playLandingVideo(GVideoClipIndex _videoClipIndex, Action _onVideoEnd)
        {
            if(_m_wVideoPlayer == null || _videoClipIndex == null || !_videoClipIndex.isValid() || wnd == null || !isShow)
            {
                _onVideoEnd?.Invoke();
                return;
            }
         
            playVideo(_videoClipIndex, true, _onVideoEnd);
        }
        
        #endregion
        
        #region 动画播放

        /// <summary>
        /// 播放游历的动画
        /// </summary>
        /// <param name="_aniName"></param>
        /// <param name="_playDone"></param>
        private void _playTravelAni(string _aniName, Action _playDone = null)
        {
            if (null == wnd)
            {
                _playDone?.Invoke();
                return;
            }
            //播放UI收起的动画
            if (null != wnd.travelAnimation)
            {
                wnd.travelAnimation.Play(_aniName, _playDone);
            }
            else
            {
                _playDone?.Invoke();
            }
        }

        private void _sampleTravelAni(string _aniName, float _normalizedTime)
        {
            if(wnd == null || wnd.travelAnimation == null)
                return;
            
            wnd.travelAnimation.Sample(_aniName, _normalizedTime);
        }

        #endregion
        
        /// <summary>
        /// 点击游历
        /// </summary>
        /// <param name="obj"></param>
        private void _clickTravel(GameObject obj)
        {
            if (NPPlayer.instance.travelComp.isTraveling)
                return;

            if(_m_travelController != null && _m_travelController.tryDealAlreadyExistedEvent())
                return;
            
            //判断是随机游历还是指定游历
            if (_m_isAllTravel)
            {
                _reqAkeyTravel();
            }
            else
            {
                _reqRandomTravel();
            }
        }

        private void _reqRandomTravel()
        {
            _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.travelComp.isTraveling = true;

            //随机游历
            NPPlayer.instance.travelComp.reqStartTravel((_info) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
                NPPlayer.instance.travelComp.isTraveling = false;
                
                _m_travelController?.randomTravelShowProcess(_info, null);
            }, () =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
                NPPlayer.instance.travelComp.isTraveling = false;
            });
        }

        private void _reqAkeyTravel()
        {
            _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.travelComp.isTraveling = true;
            
            long oldExp = GCommon.getItemCount(ENPItemType.CURRENCY, (long) ECurrency.P_EXP);
            //一键游历
            NPPlayer.instance.travelComp.reqAkeyTravel((_info) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
                NPPlayer.instance.travelComp.isTraveling = false;
                
                long nowExp = GCommon.getItemCount(ENPItemType.CURRENCY, (long) ECurrency.P_EXP);
                _m_travelController?.akeyTravelShowProcess(_info, nowExp - oldExp, null);
            }, () =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
                NPPlayer.instance.travelComp.isTraveling = false;
            });
        }

        // /// <summary>
        // /// 进入游历状态表现
        // /// </summary>
        // public void enterTravelStateShow(bool _needShowAnima, Action _enterComplete)
        // {
        //     if (wnd == null)
        //     {
        //         _enterComplete?.Invoke();
        //         return;
        //     }
        //     
        //     if (_needShowAnima)
        //     {
        //         _sampleTravelAni(wnd.enterTravelAniName, 0f);
        //         //播放UI收起的动画
        //         _playTravelAni(wnd.enterTravelAniName, () =>
        //         {
        //             _enterComplete?.Invoke();
        //         });
        //     }
        //     else
        //     {
        //         _sampleTravelAni(wnd.enterTravelAniName, 1f);
        //         _enterComplete?.Invoke();
        //     }
        // }
        
        // /// <summary>
        // /// 退出游历状态表现
        // /// </summary>
        // /// <param name="_needShowAnima"></param>
        // /// <param name="_exitComplete"></param>
        // public void exitTravelEventStateShow(bool _needShowAnima, Action _exitComplete)
        // {
        //     if (wnd == null)
        //     {
        //         _exitComplete?.Invoke();
        //         return;
        //     }
        //     
        //     if (_needShowAnima)
        //     {
        //         _sampleTravelAni(wnd.exitTravelAniName, 0f);
        //         //播放UI收起的动画
        //         _playTravelAni(wnd.exitTravelAniName, () =>
        //         {
        //             _exitComplete?.Invoke();
        //         });
        //     }
        //     else
        //     {
        //         _sampleTravelAni(wnd.exitTravelAniName, 1f);
        //         _exitComplete?.Invoke();
        //     }   
        // }
        
        /// <summary>
        /// 一键游历tog
        /// </summary>
        /// <param name="obj"></param>
        private void _clickAllTravel(NPGGUIWndCommonToggleEx obj)
        {
            //判断是否解锁？
            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.travel_one_key_simple_unlock_id,true))
                return;
            
            _m_isAllTravel = !_m_isAllTravel;
            _m_togAllTravel?.setSelected(_m_isAllTravel);
            AccountSettingMgr.instance.accountSetting.setIsAllTravel(_m_isAllTravel);
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            
            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.travel_one_key_simple_unlock_id,false))
            {
                NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.statInfosAllTravel, EGameCommonUnlockType.LOCK);
            }
            else
            {
                NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.statInfosAllTravel, EGameCommonUnlockType.UNLOCK);
            }
            _refreshTimeDown();
            
            _m_wVideoPlayer?.hideWnd();
        }

        /// <summary>
        /// 刷新倒计时显示
        /// </summary>
        private void _refreshTimeDown()
        {
            _m_energyCount?.showWnd();
            _m_energyCount?.setInfo(GRefdataCoreMgr.instance.npGeneral.travel_cost_lazycd_id);
        }

        //模拟点击游历
        private void _onSimulateClickTravel()
        {
            _clickTravel(null);
        }

        //模拟点击一键游历开关
        private void _onSimulateClickOneKeyToggle()
        {
            _clickAllTravel(_m_togAllTravel);
        }
    }
}