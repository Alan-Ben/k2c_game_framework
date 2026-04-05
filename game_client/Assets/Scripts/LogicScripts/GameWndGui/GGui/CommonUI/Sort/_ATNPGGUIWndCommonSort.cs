using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    //通用排序界面
    public class _ATNPGGUIWndCommonSort<T> : _ATNPGGUIWndCommonMutexFiiterWnd<T, _ATNPGGUIMonoCommonSort<T>>
    {
        //筛选toggle
        private NPGGUIWndCommonToggleEx _m_toggleSelect;

        public _ATNPGGUIWndCommonSort(_ATNPGGUIMonoCommonSort<T> _wnd, Action<NPGGUICommonFitterMono<T>> _delegate) : base(_wnd, _delegate)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if (null == wnd)
                return;

            if (null != wnd.toggleSelect)
            {
                _m_toggleSelect = new NPGGUIWndCommonToggleEx(wnd.toggleSelect);
                _m_toggleSelect.clickDelegate += _onClickSelectToggle;
            }

            //绑定点击事件
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();

            if (null != _m_toggleSelect)
            {
                _m_toggleSelect.discard();
                _m_toggleSelect.clickDelegate -= _onClickSelectToggle;
            }
            _m_toggleSelect = null;

            if (null == wnd)
                return;

            //解绑事件
            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
            
            if (null != _m_toggleSelect)
                _m_toggleSelect.showWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            if (null != _m_toggleSelect)
                _m_toggleSelect.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();

            if (null != _m_toggleSelect)
                _m_toggleSelect.resetWnd();
        }

        //收缩
        private void _closeBtnDidClick(GameObject _go)
        {
            _m_toggleSelect?.setSelected(false);
        }

        //收缩和展开
        private void _onClickSelectToggle(NPGGUIWndCommonToggleEx _toggleWnd)
        {
            if (null == _m_toggleSelect || null == _toggleWnd)
                return;

            _m_toggleSelect.setSelected(!_toggleWnd.isOn);
        }

        protected override void _onChgSelectTab(NPGGUICommonFitterMono<T> _tabMono)
        {
            base._onChgSelectTab(_tabMono);
            
            if (wnd != null && _tabMono != null)
                ALUGUICommon.setLabelTxt(wnd.sortTypeTxt, TextTranslate.instance.getLanguage(_tabMono.keyStr));
        }
    }
}
