using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 妃子showcase展示子窗口
    /// </summary>
    public class GGUIWndConsortShowCaseSubWnd : _ANPGGUIBasicSubWnd<GGUIMonoConsortShowCaseSubWnd>
    {
        private _IConsortShowInfo _m_consortShowInfo;//妃子的展示信息

        private NPGGUIWndCommonShowCase _m_tdShowCase;//

        private string _m_NowTdShowAniTag;//当前的展示动画tag
        private bool _m_bStartPlayAni = false;//是否开始播放动画
        private Action _m_aOnPlayAniComplete = null;//播放完成的回调
        private ALCommonEnableTaskController _m_tCheckAniPlayCompleteTask;
        
        public GGUIWndConsortShowCaseSubWnd(GGUIMonoConsortShowCaseSubWnd _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoShowcase != null)
            {
                _m_tdShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowcase);
            }
        }
        
        protected override void _onDiscard()
        {
            _m_tCheckAniPlayCompleteTask.setDisable();
            _m_aOnPlayAniComplete = null;
            _m_bStartPlayAni = false;
            _m_NowTdShowAniTag = string.Empty;
            
            _m_tdShowCase?.discard();
            _m_tdShowCase = null;
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SET_CONSORT_TD_SHOW_ANI, _winMsgSetTdShowAni);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SET_CONSORT_TD_SHOW_ANI, _winMsgSetTdShowAni);

            _m_tCheckAniPlayCompleteTask.setDisable();
            _m_aOnPlayAniComplete = null;
            _m_bStartPlayAni = false;
            _m_NowTdShowAniTag = string.Empty;

            _m_tdShowCase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_tCheckAniPlayCompleteTask.setDisable();
            _m_aOnPlayAniComplete = null;
            _m_bStartPlayAni = false;
            _m_NowTdShowAniTag = string.Empty;

            _m_tdShowCase?.resetWnd();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void setData(_IConsortShowInfo _consortShowInfo, bool _isPlayEnter = false)
        {
            _m_consortShowInfo = _consortShowInfo;

            _refreshWnd(_isPlayEnter);
        }

        private void _refreshWnd(bool _isPlayEnter = false)
        {
            if(wnd == null || _m_consortShowInfo == null || _m_tdShowCase == null)
                return;
            
            int maxShowCaseUnitCount = Math.Max(wnd.consortActorInShowCaseIndex, wnd.bgInShowCaseIndex) + 1;
            if (maxShowCaseUnitCount > 0)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[maxShowCaseUnitCount];
                
                if(wnd.consortActorInShowCaseIndex >= 0)
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_consortShowInfo.consortSkinShowInfo?.tdShow), wnd.consortActorInShowCaseIndex);
                
                if(wnd.bgInShowCaseIndex >= 0)
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_consortShowInfo.consortSkinShowInfo?.tdBgIndex), wnd.bgInShowCaseIndex);
                
                _m_tdShowCase.showWnd(showCaseUnitInfoObjList);
                _m_tdShowCase.regInitDoneDelegate(() =>
                {
                    if(_isPlayEnter)
                        _m_tdShowCase.playAnim(wnd.consortActorInShowCaseIndex, wnd.enterAniName);
                });
            }
            else
            {
                _m_tdShowCase.hideWnd();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_tdShowAniType"></param>
        public void setTdShowAni(EConsortTdShowAniType _tdShowAniType, bool _forcePlay, Action _onComplete)
        {
            if (wnd == null || wnd.consortActorInShowCaseIndex < 0 || _m_tdShowCase == null || _m_consortShowInfo == null || _m_consortShowInfo.consortRefObj == null)
            {
                _onComplete?.Invoke();
                return;
            }

            ConsortTdShowAniConfig tdShowAniConfig = _m_consortShowInfo.consortRefObj.getTdShowAniConfig(_tdShowAniType);
            if (tdShowAniConfig == null || string.IsNullOrEmpty(tdShowAniConfig.aniTag))
            {
                _onComplete?.Invoke();
                return;
            }
            
            _m_tdShowCase.regInitDoneDelegate(() =>
            {
                if (wnd == null || wnd.consortActorInShowCaseIndex < 0 || _m_tdShowCase == null)
                {
                    _onComplete?.Invoke();
                    return;
                }
                
                _m_tCheckAniPlayCompleteTask.setDisable();
                WinMsg.SendMsg(WinMsgType.CONSORT_TD_SHOW_ANI_PLAY_COMPLETE, _m_NowTdShowAniTag);
                _m_aOnPlayAniComplete?.Invoke();
                _m_aOnPlayAniComplete = _onComplete;
                _m_bStartPlayAni = false;
                _m_NowTdShowAniTag = tdShowAniConfig.aniTag;

                _m_tCheckAniPlayCompleteTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_checkAniPlayCompleteTask);
                
                //这边就是需要强切
                if(_forcePlay)
                    _m_tdShowCase.forceSetAni(wnd.consortActorInShowCaseIndex, tdShowAniConfig.aniTag);
                else
                    _m_tdShowCase.playAnim(wnd.consortActorInShowCaseIndex, tdShowAniConfig.aniTag);
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_aniTag"></param>
        public void setTdShowAni(string _aniTag, bool _forcePlay, Action _onComplete)
        {
            if (wnd == null || wnd.consortActorInShowCaseIndex < 0 || _m_tdShowCase == null || _m_consortShowInfo == null || _m_consortShowInfo.consortRefObj == null || string.IsNullOrEmpty(_aniTag))
            {
                _onComplete?.Invoke();
                return;
            }

            _m_tdShowCase.regInitDoneDelegate(() =>
            {
                if (wnd == null || wnd.consortActorInShowCaseIndex < 0 || _m_tdShowCase == null)
                {
                    _onComplete?.Invoke();
                    return;
                }
                
                _m_tCheckAniPlayCompleteTask.setDisable();
                WinMsg.SendMsg(WinMsgType.CONSORT_TD_SHOW_ANI_PLAY_COMPLETE, _m_NowTdShowAniTag);
                _m_aOnPlayAniComplete?.Invoke();
                _m_aOnPlayAniComplete = _onComplete;
                _m_bStartPlayAni = false;
                _m_NowTdShowAniTag = _aniTag;

                _m_tCheckAniPlayCompleteTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_checkAniPlayCompleteTask);
                
                //这边就是需要强切
                if(_forcePlay)
                    _m_tdShowCase.forceSetAni(wnd.consortActorInShowCaseIndex, _aniTag);
                else
                    _m_tdShowCase.playAnim(wnd.consortActorInShowCaseIndex, _aniTag);
            });
        }
        
        /// <summary>
        /// 设置td展示动画
        /// </summary>
        private void _winMsgSetTdShowAni(params object[] _objs)
        {
            if(_objs == null || _m_consortShowInfo == null || _objs.Length < 2 || !(_objs[0] is long _consortId) || !(_objs[1] is EConsortTdShowAniType _tdShowAniType) || _m_consortShowInfo.consortId != _consortId)
                return;

            setTdShowAni(_tdShowAniType, true, null);
        }

        private void _checkAniPlayCompleteTask()
        {
            if (wnd == null || _m_tdShowCase == null || string.IsNullOrEmpty(_m_NowTdShowAniTag) || wnd.consortActorInShowCaseIndex < 0)
            {
                _m_tCheckAniPlayCompleteTask.setDisable();
                WinMsg.SendMsg(WinMsgType.CONSORT_TD_SHOW_ANI_PLAY_COMPLETE, _m_NowTdShowAniTag);
                _m_aOnPlayAniComplete?.Invoke();
                _m_aOnPlayAniComplete = null;
                _m_bStartPlayAni = false;
                return;
            }

            if (!_m_bStartPlayAni && _m_tdShowCase.isPlayingAni(wnd.consortActorInShowCaseIndex, _m_NowTdShowAniTag))
            {
                _m_bStartPlayAni = true;
                return;
            }

            if (_m_bStartPlayAni && !_m_tdShowCase.isPlayingAni(wnd.consortActorInShowCaseIndex, _m_NowTdShowAniTag))
            {
                _m_tCheckAniPlayCompleteTask.setDisable();
                WinMsg.SendMsg(WinMsgType.CONSORT_TD_SHOW_ANI_PLAY_COMPLETE, _m_NowTdShowAniTag);
                _m_aOnPlayAniComplete?.Invoke();
                _m_aOnPlayAniComplete = null;
                _m_bStartPlayAni = false;
                _m_NowTdShowAniTag = string.Empty;
            }
        }
    }
}