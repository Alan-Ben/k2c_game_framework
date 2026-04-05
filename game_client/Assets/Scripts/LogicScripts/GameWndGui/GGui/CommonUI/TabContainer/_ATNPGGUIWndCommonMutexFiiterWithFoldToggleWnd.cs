using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带有折叠开关的互斥过滤窗口
    /// </summary>
    /// <typeparam name="T">过滤类型</typeparam>
    public class _ATNPGGUIWndCommonMutexFiiterWithFoldToggleWnd<T, T_Mono> : _ATNPGGUIWndCommonMutexFiiterWnd<T, T_Mono> 
        where T_Mono : _ATNPGGUIMonoCommonMutexFiiterWithFoldToggleWnd<T>
    {
        //收起展开toggle
        private NPGGUIWndCommonToggleEx _m_wToggleFold;

        public _ATNPGGUIWndCommonMutexFiiterWithFoldToggleWnd(T_Mono _wnd, Action<NPGGUICommonFitterMono<T>> _delegate) : base(_wnd, _delegate)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if (null == wnd)
                return;

            if (null != wnd.toggleFold)
            {
                _m_wToggleFold = new NPGGUIWndCommonToggleEx(wnd.toggleFold);
                _m_wToggleFold.clickDelegate += _onClickFoldToggle;
            }

            //绑定点击事件
            ALUGUICommon.combineBtnClick(wnd.foldBtn, _foldBtnClose);
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();

            if (null != _m_wToggleFold)
            {
                _m_wToggleFold.discard();
                _m_wToggleFold.clickDelegate -= _onClickFoldToggle;
            }
            _m_wToggleFold = null;

            if (null == wnd)
                return;

            //解绑事件
            ALUGUICommon.uncombineBtnClick(wnd.foldBtn, _foldBtnClose);
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
            
            if (null != _m_wToggleFold)
                _m_wToggleFold.showWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            if (null != _m_wToggleFold)
                _m_wToggleFold.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();

            if (null != _m_wToggleFold)
                _m_wToggleFold.resetWnd();
        }

        /// <summary>
        /// 点击收起按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _foldBtnClose(GameObject _go)
        {
            _m_wToggleFold?.setSelected(false);
        }

        /// <summary>
        /// 点击收缩和展开toggle
        /// </summary>
        /// <param name="_toggleWnd"></param>
        private void _onClickFoldToggle(NPGGUIWndCommonToggleEx _toggleWnd)
        {
            if (null == _m_wToggleFold || null == _toggleWnd)
                return;

            _m_wToggleFold.setSelected(!_toggleWnd.isOn);
        }

        protected override void _onChgSelectTab(NPGGUICommonFitterMono<T> _tabMono)
        {
            base._onChgSelectTab(_tabMono);

            if (wnd != null && wnd.curTypeTxtList != null && _tabMono != null)
            {
                foreach (var text in wnd.curTypeTxtList)
                {
                    ALUGUICommon.setLabelTxt(text, TextTranslate.instance.getLanguage(_tabMono.keyStr));
                }
            }
        }
    }
}