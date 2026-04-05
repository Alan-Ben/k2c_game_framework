using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public abstract class _AGGUIWndOptionItem<_T_MONO, _T_SELF> : _ATALBasicUISubWnd<_T_MONO>
        where _T_MONO : _AGGUIMonoOptionItem
        where _T_SELF : _AGGUIWndOptionItem<_T_MONO, _T_SELF>
    {
        public _AGGUIWndOptionItem(_T_MONO _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<_T_SELF> onClick;

        protected sealed override void _onShowWnd()
        {
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd()
        {
            _onHideWndEx();
        }

        protected sealed override void _onReset()
        {
            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            if (wnd != null)
            {
                if(wnd.btnClick != null)
                    ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickBtn);
            }
            
            _onDiscardEx();
        }

        protected sealed override void _onWndInitDone()
        {
            if (wnd != null)
            {
                if(wnd.btnClick != null)
                    ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickBtn);
            }
            
            _onWndInitDoneEx();
        }

        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtn(GameObject _go)
        {
            onClick?.Invoke((_T_SELF)this);
        }
        
        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
    }
}