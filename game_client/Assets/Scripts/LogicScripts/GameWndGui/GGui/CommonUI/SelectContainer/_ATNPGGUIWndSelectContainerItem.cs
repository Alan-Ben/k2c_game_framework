using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 选择滚动条ItemWnd基类
    /// </summary>
    public abstract class _ATNPGGUIWndSelectContainerItem<_T_ITEM_MONO> : _ATALBasicUISubWnd<_T_ITEM_MONO> where _T_ITEM_MONO : _ANPGGUIMonoSelectContainerItem
    {
        protected _ATNPGGUIWndSelectContainerItem(_T_ITEM_MONO _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        public event Action<int> onClick;//点击回调

        private int _m_iItemIndex;//item下标
        private float _m_fCenterPos;//居中时，container的位置

        /// <summary> item下标 </summary>
        public int itemIndex { get => _m_iItemIndex; }
        /// <summary> item居中时，父节点的位置 </summary>
        public float centerPos { get => _m_fCenterPos; }

        protected override void _onShowWnd()
        {
            _onShowWndEx();
        }

        protected override void _onHideWnd()
        {
            _onHideWndEx();
        }

        protected override void _onReset()
        {
            _onResetEx();
        }

        protected override void _onDiscard()
        {
            _onDiscardEx();

            onClick = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnItem, _onClickBtnItem);
        }

        protected override void _onWndInitDone()
        {
            _onWndInitDoneEx();

            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnItem, _onClickBtnItem);
        }

        #region 子类窗体相关
        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
        #endregion


        #region 点击事件

        /// <summary>
        /// 点击item
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnItem(GameObject _go)
        {
            onClick?.Invoke(_m_iItemIndex);
        }

        #endregion


        #region ScrollRect相关

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="_index">item在container中的下标</param>
        /// <param name="_centerPos">item居中时，父节点的位置</param>
        public void setScrollParam(int _index, float _centerPos)
        {
            _m_iItemIndex = _index;
            _m_fCenterPos = _centerPos;
        }

        /// <summary>
        /// 设置缩放
        /// </summary>
        /// <param name="_scale"></param>
        public void setScale(Vector3 _scale)
        {
            if (_m_rtRectTransform == null)
                return;

            _m_rtRectTransform.localScale = _scale;
        }

        /// <summary>
        /// 设置是否选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setSelected(bool _isSelected)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.showOnSelect, _isSelected);
            ALUGUICommon.setGameObjEnable(wnd.hideOnSelect, !_isSelected);
        }

        #endregion
    }
}
