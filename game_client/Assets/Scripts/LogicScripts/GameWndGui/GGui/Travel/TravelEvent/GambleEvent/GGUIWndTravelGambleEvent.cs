using System;
using System.Collections.Generic;
using ALPackage;
using Common.TravelEnum;
using Common.TravelObj;
using JetBrains.Annotations;
using Spine.Unity;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTravelGambleOption
    {
        private GGUIMonoTravelGambleOption _m_mono;
        private Action<GGUIWndTravelGambleOption> _m_aOnOptionClick;

        public GGUIWndTravelGambleOption(GGUIMonoTravelGambleOption mono)
        {
            _m_mono = mono;
        }

        public GGUIMonoTravelGambleOption mono { get { return _m_mono; } }

        public void init(Action<GGUIWndTravelGambleOption> _onOptionClick)
        {
            _m_aOnOptionClick = _onOptionClick;
            if (_m_mono == null)
                return;

            ALUGUICommon.combineBtnClick(_m_mono.btnClick, _onBtnClick);
        }

        public void discard()
        {
            _m_aOnOptionClick = null;
            if (_m_mono != null)
                ALUGUICommon.uncombineBtnClick(_m_mono.btnClick, _onBtnClick);
        }

        public void showWnd()
        {
        }

        public void hideWnd()
        {
        }

        private void _onBtnClick(GameObject _go)
        {
            _m_aOnOptionClick?.Invoke(this);
        }
    }

    /// <summary>
    /// 游历博彩事件主展示窗口
    /// </summary>
    public class GGUIWndTravelGambleEvent : _ANPGGUIBasicWnd<GGUIMonoTravelGambleEvent>
    {
        private static GGUIWndTravelGambleEvent _g_instance;
        public static GGUIWndTravelGambleEvent instance { get { return _g_instance ??= new GGUIWndTravelGambleEvent(); } }

        private TravelGambleEventInfo _m_eventInfo;

        private TravelGambleEventResultInfo _m_gambleResult;

        [NotNull] private List<GGUIWndTravelGambleOption> _m_lOptionWndList = new List<GGUIWndTravelGambleOption>();

        private ETravelGambleEventWndState _m_eCurWndState = ETravelGambleEventWndState.NONE;
        private int _m_iShowResultSerialId;

        public GGUIWndTravelGambleEvent() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTravelGambleEvent.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTravelGambleEvent.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _buildOptionWnds();

            ALUGUICommon.combineBtnClick(wnd.btnAnte, _onAnteBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnSkip, _onSkipBtnClick);
        }
        
        private void _buildOptionWnds()
        {
            foreach (var opt in _m_lOptionWndList)
                opt?.discard();
            _m_lOptionWndList.Clear();

            if (wnd?.optionList == null)
                return;

            foreach (var optMono in wnd.optionList)
            {
                if (optMono == null)
                    continue;
                var optWnd = new GGUIWndTravelGambleOption(optMono);
                optWnd.init(_onOptionClick);
                _m_lOptionWndList.Add(optWnd);
            }
        }

        protected override void _onDiscard()
        {
            _m_eventInfo = null;
            _m_gambleResult = null;

            foreach (var opt in _m_lOptionWndList)
                opt?.discard();
            _m_lOptionWndList.Clear();

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnAnte, _onAnteBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnSkip, _onSkipBtnClick);
            }
        }

        protected override void _onShowWnd()
        {
            foreach (var optionWnd in _m_lOptionWndList)
            {
                optionWnd?.showWnd();
            }
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_iShowResultSerialId = ALSerializeOpMgr.next();
            
            foreach (var optionWnd in _m_lOptionWndList)
            {
                optionWnd?.hideWnd();
            }
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 传入博彩事件数据并刷新窗口
        /// </summary>
        public void setData(TravelGambleEventInfo _eventInfo)
        {
            _m_eventInfo = _eventInfo;
            _m_eCurWndState = ETravelGambleEventWndState.NONE;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            int anteNum = _m_eventInfo?.selectedAnteCount ?? 0;
            ALUGUICommon.setLabelTxt(wnd.txtAnteNum, anteNum.ToString());

            NPCommonEnumAniStatInfo<ETravelGambleEventWndState>.setStat(wnd.wndStateAniInfoList, _m_eCurWndState);
        }

        /// <summary>
        /// 打开下注窗口
        /// </summary>
        public void openAnteWnd()
        {
            if (_m_eventInfo == null || !(_m_eCurWndState is ETravelGambleEventWndState.NONE or ETravelGambleEventWndState.SELECTING_OPTION))
                return;

            _m_eCurWndState = ETravelGambleEventWndState.ANTEING;
            if (wnd != null)
            {
                NPCommonEnumAniStatInfo<ETravelGambleEventWndState>.setStat(wnd.wndStateAniInfoList, _m_eCurWndState);
            }
            
            GGUIWndTravelGambleAnte.instance.setData(_m_eventInfo);
            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(GGUIWndTravelGambleAnte.instance, () =>
            {
                // 当关闭下注窗口后, 但是处于还没下注状态或放弃状态, 直接退出窗口
                if (_m_eventInfo.anteState == ETravelGambleAnteState.WAITING_ANTE || _m_eventInfo.anteState == ETravelGambleAnteState.WAIVE)
                {
                    _doCloseWnd();
                    return;
                }

                _m_eCurWndState = ETravelGambleEventWndState.SELECTING_OPTION;
                _refreshWnd();
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_GAMBLE_ANTE_DEAL_WND, false, false));
        }
        
        private void _onAnteBtnClick(GameObject _go)
        {
            openAnteWnd();
        }

        /// <summary>
        /// 跳过按钮点击, 若事件已经处理完成, 直接显示结果窗口
        /// </summary>
        private void _onSkipBtnClick(GameObject _go)
        {
            if (!(_m_eCurWndState is ETravelGambleEventWndState.SHOWING_RESULT_PROCESS))
                return;

            _m_iShowResultSerialId = ALSerializeOpMgr.next();
            
            if (_m_eventInfo != null && _m_eventInfo.inDataLevelEventDone)
            {
                _showResultWnd();
            }
            else
            {
                _doCloseWnd();
            }
        }

        /// <summary>
        /// 展示结果窗口
        /// </summary>
        private void _showResultWnd()
        {
            if (_m_eventInfo == null || _m_gambleResult == null)
            {
                _doCloseWnd();
                return;
            }
            
            _m_eCurWndState = ETravelGambleEventWndState.SHOWING_RESULT_WND;
            if (wnd != null)
            {
                NPCommonEnumAniStatInfo<ETravelGambleEventWndState>.setStat(wnd.wndStateAniInfoList, _m_eCurWndState);
            }
            
            GGUIWndTravelGambleEventResult resultWnd = new GGUIWndTravelGambleEventResult(_m_gambleResult);
            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(resultWnd, () =>
            {
                // 结果窗口关闭后, 关闭当前窗口
                _doCloseWnd();
            }, UINodeTagConst.C_TRAVEL_GAMBLE_EVENT_RESULT));
        }

        /// <summary>
        /// 当选项被点击：若已下注则执行结果表现
        /// </summary>
        private void _onOptionClick(GGUIWndTravelGambleOption _optionWnd)
        {
            if (_m_eventInfo == null)
                return;

            if (_m_eventInfo.anteState != ETravelGambleAnteState.ANTED)
                return;

            // 记录选择的选项 tag
            _m_eventInfo.selectedOptionTag = _optionWnd.mono?.tag ?? string.Empty;
            long beforeDealEventEarnings = NPPlayer.instance.specialItemComp.goldData.earnings;

            int maskSerializeId = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.travelComp.reqDealGambleTravel(_m_eventInfo.instanceId, _m_eventInfo.selectedAnteCount, 
                _m_eventInfo.anteState == ETravelGambleAnteState.WAIVE,
                (_isSucc, _retMsg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(maskSerializeId);
                    
                    if(!_isSucc || _retMsg == null || _m_eventInfo == null)
                        return;

                    _m_gambleResult = _m_eventInfo.getEventResultInfo(_retMsg.getResult(), beforeDealEventEarnings);
                    _showResult(_optionWnd, () =>
                    {
                        _showResultWnd();
                    });
                });
        }

        /// <summary>
        /// 结果表现：
        /// WIN → 播放所选选项 showConfigList 中随机一个配置；
        /// LOSE → 从其余选项中随机选一个，播放其 showConfigList 中随机一个配置；
        /// JACKPOT → 从 jackpotShowConfigList 中随机一个配置；
        /// 播放期间显示跳过按钮
        /// </summary>
        private void _showResult(GGUIWndTravelGambleOption _chosenOption, Action _onPlayDone)
        {
            if (wnd == null || _m_gambleResult == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            ETravelGambleResult resultType = _m_gambleResult.travelGambleResult;
            List<TravelGambleResultShowConfig> configList = null;

            if (resultType == ETravelGambleResult.JACKPOT)
            {
                configList = wnd.jackpotShowConfigList;
            }
            else if (resultType == ETravelGambleResult.WIN)
            {
                configList = _chosenOption?.mono?.showConfigList;
            }
            else // LOSE / ABANDON / NONE
            {
                // 从其他选项中随机取一个
                if (wnd.optionList != null && wnd.optionList.Count > 1)
                {
                    GGUIMonoTravelGambleOption chosenMono = _chosenOption?.mono;
                    List<GGUIMonoTravelGambleOption> otherOptions = wnd.optionList.FindAll(_o => _o != chosenMono);
                    if (otherOptions.Count > 0)
                    {
                        int randIdx = UnityEngine.Random.Range(0, otherOptions.Count);
                        configList = otherOptions[randIdx]?.showConfigList;
                    }
                }
            }

            TravelGambleResultShowConfig config = _pickRandom(configList);
            if (config == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            long showResultSerialId = _m_iShowResultSerialId = ALSerializeOpMgr.next();
            ALProcess process = ALProcess.CreateProcess();
            
            process
                .addDelegateProcess((_processComplete) =>
                {
                    _m_eCurWndState = ETravelGambleEventWndState.SHOWING_RESULT_PROCESS;
                    if (wnd.wndStateAniInfoList != null)
                    {
                        NPCommonEnumAniStatInfo<ETravelGambleEventWndState>.setStat(wnd.wndStateAniInfoList, _m_eCurWndState, _processComplete);
                    }
                    else
                    {
                        _processComplete?.Invoke();
                    }
                })
                .addDelegateProcess((_processComplete) =>
                {
                    if(showResultSerialId != _m_iShowResultSerialId)
                        return;
                    
                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(2);
                    stepCounter.regAllDoneDelegate(_processComplete);
                    
                    _playSpineAnimation(config.spineAnimationName, config.trackIndex, config.splineAniTime, () =>
                    {
                        stepCounter.addDoneStepCount();
                    });

                    if (wnd != null)
                    {
                        _playAnimation(wnd.gambleResultProcessAnimation, config.processAniName, () =>
                        {
                            stepCounter.addDoneStepCount();
                        });
                    }
                    else
                    {
                        stepCounter.addDoneStepCount();
                    }
                })
                .addProcess(() =>
                {
                    if(showResultSerialId != _m_iShowResultSerialId)
                        return;
                    
                    _onPlayDone?.Invoke();
                })
                .deal();
        }
        
        /// <summary>
        /// 播放一条 Spine 动画，animationTime 秒后触发回调
        /// </summary>
        private void _playSpineAnimation(string _aniName, int _trackIndex, float _animationTime, Action _onPlayDone)
        {
            SkeletonGraphic skeleton = wnd?.skeletonGraphic;
            if (skeleton == null || skeleton.AnimationState == null || string.IsNullOrEmpty(_aniName))
            {
                _onPlayDone?.Invoke();
                return;
            }

            skeleton.AnimationState.SetAnimation(_trackIndex, _aniName, false);

            if (_animationTime <= 0)
            {
                _onPlayDone?.Invoke();
            }
            else
            {
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    _onPlayDone?.Invoke();
                }, _animationTime);
            }
        }
        
        private void _playAnimation(Animation _animation, string _aniName, Action _onPlayDone)
        {
            if (_animation == null || string.IsNullOrEmpty(_aniName))
            {
                _onPlayDone?.Invoke();
                return;
            }

            _animation.Play(_aniName, () => _onPlayDone?.Invoke());
        }

        /// <summary>
        /// 关闭主展示窗口
        /// </summary>
        private void _doCloseWnd()
        {
            _m_eCurWndState = ETravelGambleEventWndState.NONE;
            if (wnd != null)
            {
                NPCommonEnumAniStatInfo<ETravelGambleEventWndState>.setStat(wnd.wndStateAniInfoList, _m_eCurWndState);
            }
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_GAMBLE_EVENT_SHOW_WND);
        }

        private static TravelGambleResultShowConfig _pickRandom(List<TravelGambleResultShowConfig> _list)
        {
            if (_list == null || _list.Count == 0)
                return null;
            return _list[UnityEngine.Random.Range(0, _list.Count)];
        }
    }
}

