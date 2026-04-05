using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 单选列表item基类
    /// </summary>
    public abstract class _ATNPGGUIWndSingleChoiceItem<_T_MONO,_T_SELF> : _ATALBasicUISubWnd<_T_MONO> 
        where _T_MONO : _ANPGGUIMonoSingleChoiceItem
        where _T_SELF : _ATNPGGUIWndSingleChoiceItem<_T_MONO, _T_SELF>
    {
        protected _ATNPGGUIWndSingleChoiceItem(_T_MONO _mono) : base(_mono)
        {
            
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
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onClickBtnSelect);
            _onDiscardEx();
        }

        protected sealed override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            setSelectShow(false);
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onClickBtnSelect);
            _onWndInitDoneEx();
        }

        /// <summary>
        /// 点击选中
        /// </summary>
        /// <param name="_go"></param>
        protected virtual void _onClickBtnSelect(GameObject _go)
        {
            onClick?.Invoke((_T_SELF)this);
        }

        /// <summary>
        /// 设置选中相关显示
        /// </summary>
        /// <param name="_isSelect"></param>
        public virtual void setSelectShow(bool _isSelect)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goListShowOnSelect, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goListHideOnSelect, !_isSelect);
        }

        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
    }
}
