using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    //通用筛选界面item
    public class NPGGUIWndCommonFitterTab<T> : _ANPGGUIBasicSubWnd<NPGGUIMonoCommonFitterTab>
    {
        /// <summary>
        /// 排序点击回调 bool- 是否为正序
        /// </summary>
        private Action<NPGGUIWndCommonFitterTab<T>> _m_sortTypeDelegate;

        //筛选toggle
        private NPGGUIWndCommonToggleEx _m_toggleSelect;
        private NPGGUICommonFitterMono<T> _m_info;
        private bool _m_isOn = false;
        private NPGGuiWndTexture _m_sortTypeIcon;

        public NPGGUIWndCommonFitterTab(NPGGUIMonoCommonFitterTab _wnd, Action<NPGGUIWndCommonFitterTab<T>> _delegate) : base(_wnd)
        {
            _m_sortTypeDelegate = _delegate;
            initWnd();
        }
        
        public NPGGUICommonFitterMono<T> info { get { return _m_info; } }
        public bool isOn { get => _m_isOn;}

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.togSelect)
            {
                _m_toggleSelect = new NPGGUIWndCommonToggleEx(wnd.togSelect);
                _m_toggleSelect.setSelected(_m_isOn);
                _m_toggleSelect.clickDelegate += _onClickSelectToggle;
            }

            if (null != wnd.sortTypeIcon)
            {
                _m_sortTypeIcon = new NPGGuiWndTexture(wnd.sortTypeIcon);
            }
        }
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if (null != _m_toggleSelect)
                _m_toggleSelect.resetWnd();
            if(null != _m_sortTypeIcon)
            {
                _m_sortTypeIcon.discardTexture();
            }
        }

        protected override void _onDiscard()
        {
            _m_sortTypeDelegate = null;

            if (null != _m_toggleSelect)
                _m_toggleSelect.discard();
            _m_toggleSelect = null;

            if(null != _m_sortTypeIcon)
            {
                _m_sortTypeIcon.discard();
                _m_sortTypeIcon = null;
            }
        }

        public void setInfo(NPGGUICommonFitterMono<T> _info)
        {
            _m_info = _info;
            //设置显示文本
            foreach (TextEx txtEx in wnd.sortTypeTxt)
            {
                ALUGUICommon.setLabelTxt(txtEx, TextTranslate.instance.getLanguage(_m_info.keyStr));
            }
            
            _m_sortTypeIcon?.setTexture(_info.iconIndex);
            _m_sortTypeIcon?.showWnd();
        }

        //收缩和展开
        private void _onClickSelectToggle(NPGGUIWndCommonToggleEx _toggle)
        {
            if (null == _m_toggleSelect)
                return;
            if (null != _m_sortTypeDelegate)
            {
                _m_sortTypeDelegate(this);
            }
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setSelected(bool _isSelected)
        {
            if (_m_isOn == _isSelected)
                return;
            if (null == _m_toggleSelect)
                return;
            _m_isOn = _isSelected;
            _m_toggleSelect.setSelected(_m_isOn);
        }
    }
}
