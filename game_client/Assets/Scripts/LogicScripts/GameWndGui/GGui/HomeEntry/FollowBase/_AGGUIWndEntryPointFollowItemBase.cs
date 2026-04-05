using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 主城入口随窗口信息
    /// </summary>
    public abstract class _AGGUIWndEntryPointFollowItemBase<T> : _ATALGGUIWndCommonFollowItem<T> where T : _AGGUIMonoEntryPointFollowItemBase
    {
        protected EntryPointRefObj _m_entryPointRefObj;

        private NPGGUIWndCommonRedTip _m_redTip;
        ///功能入口解锁提示附加窗口
        private GGUIWndSubEntryFuncUnlockTip _m_wSubEntryFuncUnlockTip;

        public _AGGUIWndEntryPointFollowItemBase(T _wnd) : base(_wnd)
        {
            initWnd();
        }

        public void showWnd(EntryPointRefObj _refObj)
        {
            if (null == _refObj)
                return;

            _m_entryPointRefObj = _refObj;
            showWnd();
            _refreshWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshRedTip);
            _onShowWndEx();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshRedTip);
            _m_wSubEntryFuncUnlockTip?.hideWnd();
            _onHideWndEx();
        }

        protected override void _onReset()
        {
            _m_wSubEntryFuncUnlockTip?.resetWnd();
            _onResetEx();
        }

        protected override void _onDiscard()
        {
            _m_redTip?.discard();
            _m_redTip = null;

            _m_wSubEntryFuncUnlockTip?.discard();
            _m_wSubEntryFuncUnlockTip = null;

            if (wnd != null) 
                ALUGUICommon.uncombineBtnClick(wnd.btnCLick, _onClickBtnEntryPoint);

            _onDiscardEx();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.monoRed)
                _m_redTip = new NPGGUIWndCommonRedTip(wnd.monoRed);

            if(null != wnd.monoSubEntryFuncUnlockTip)
                _m_wSubEntryFuncUnlockTip = new GGUIWndSubEntryFuncUnlockTip(wnd.monoSubEntryFuncUnlockTip);

            ALUGUICommon.combineBtnClick(wnd.btnCLick, _onClickBtnEntryPoint);
            
            _onWndInitDoneEx();
        }

        private void _onClickBtnEntryPoint(GameObject _go)
        {
            if (null == _m_entryPointRefObj)
                return;

            if (null == _m_entryPointRefObj.unlock_condition || _m_entryPointRefObj.unlock_condition.IsEnable(null))
            {
                _m_entryPointRefObj.click_effect?.dealEffect();
            }
        }
        
        /// <summary>
        /// 播放解锁动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playUnlockAni(Action _onPlayDone)
        {
            if (null == wnd || null == wnd.stateAniInfo)
            {
                if (_onPlayDone != null) 
                    _onPlayDone();
                return;
            }
            
            wnd.stateAniInfo.play(EEntryPointAniType.UNLOCKING_PROCESS,_onPlayDone);
        }

        /// <summary>
        /// 设置解锁动画状态
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_normalizeTime"></param>
        public void setUnlockAniSample(EEntryPointAniType _type, long _normalizeTime)
        {
            if (wnd == null || wnd.stateAniInfo == null)
                return;

            wnd.stateAniInfo.sample(_type, _normalizeTime);
        }

        /// <summary>
        /// 播放屏幕隐藏显示动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playScreenShowHideAni(bool _isShow, Action _onPlayDone)
        {
            if (null == wnd || null == wnd.stateAniInfo)
            {
                if (_onPlayDone != null) 
                    _onPlayDone();
                return;
            }

            if (_isShow)
            {
                wnd.stateAniInfo.play(EEntryPointAniType.SCREEN_CLICK_SHOW_PROCESS, _onPlayDone);
            }
            else
            {
                //有红点不隐藏
                if (_m_entryPointRefObj != null && _m_entryPointRefObj.red_id > 0)
                {
                    _ARedTipNode _redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(_m_entryPointRefObj.red_id);
                    if (null != _redTipNode && _redTipNode.getCount() > 0)
                        return;
                }
                
                wnd.stateAniInfo.play(EEntryPointAniType.SCREEN_CLICK_HIDE_PROCESS, _onPlayDone);
            }
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || null == _m_entryPointRefObj)
                return;

            string translatedName = TextTranslate.instance.getLanguage(_m_entryPointRefObj.name);
            ALUGUICommon.setLabelTxt(wnd.textName, translatedName);
            ALUGUICommon.setLabelTxt(wnd.textName2, translatedName);

            //判断是否已解锁 并且 解锁表现是否已完成
            if (null == _m_entryPointRefObj.unlock_condition || _m_entryPointRefObj.unlock_condition.IsEnable(null) && NPPlayer.instance.funcUnlockComp.isFuncUnlockTipDone(_m_entryPointRefObj.func_unlock_type))
            {
                //已解锁状态
                wnd?.stateAniInfo?.sample(EEntryPointAniType.UNLOCK, 1);
                //隐藏解锁提示
                _m_wSubEntryFuncUnlockTip?.hideWnd();
            }
            else
            {
                //未解锁状态
                wnd?.stateAniInfo?.sample(EEntryPointAniType.LOCK, 1);
                //显示解锁提示
                _m_wSubEntryFuncUnlockTip?.showWnd();
                _m_wSubEntryFuncUnlockTip?.setInfo(_m_entryPointRefObj.func_unlock_type);
            }

            _refreshRedTip();
            _refreshWndEx();
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            if (_m_entryPointRefObj == null)
                return;

            _ARedTipNode _redTipNode = null;
            if (_m_entryPointRefObj.red_id > 0)
                _redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(_m_entryPointRefObj.red_id);
            _m_redTip?.showWnd();
            _m_redTip?.showRedTipNum((_redTipNode != null) ? (int)_redTipNode.getCount() : 0);
        }

        protected abstract void _refreshWndEx();
        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
        
    }
}