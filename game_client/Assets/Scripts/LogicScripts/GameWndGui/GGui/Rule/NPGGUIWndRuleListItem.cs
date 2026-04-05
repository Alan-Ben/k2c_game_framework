using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 规则列表item
    /// </summary>
    public class NPGGUIWndRuleListItem : _ATALUGUIBasicGridItemWnd<NPGGUIMonoRuleListItem>
    {
        private NPRuleRefObj _m_ruleRefObj;
        private NPGGUIWndCommonToggleEx _m_btnToggle;
        private Action<NPGGUIWndRuleListItem,bool,int> _m_clickAction;
        private int _m_index;
        private NPGGuiWndTexture _m_iconWnd;//图标

        public NPGGUIWndRuleListItem(NPGGUIMonoRuleListItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public NPRuleRefObj ruleRefObj { get => _m_ruleRefObj; }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
        
        }

        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_btnToggle?.discard();
            _m_btnToggle = null;
            
            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnGoto,_clickGoto);

            if (null != wnd.btnToggle)
            {
                _m_btnToggle = new NPGGUIWndCommonToggleEx(wnd.btnToggle);
                _m_btnToggle.clickDelegate += _clickTogle;
            }
            
            if(null != wnd.texIcon)
                _m_iconWnd = new NPGGuiWndTexture(wnd.texIcon);
        }
        

        protected override void _resetGridItem()
        {
        }
        
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_ruleRefObj"></param>
        /// <param name="_index"></param>
        /// <param name="_isSelect"></param>
        /// <param name="_clickAction"></param>
        public void setInfo(NPRuleRefObj _ruleRefObj, int _index, bool _isSelect, Action<NPGGUIWndRuleListItem, bool, int> _clickAction)
        {
            _m_clickAction = _clickAction;
            _m_ruleRefObj = _ruleRefObj;
            _m_index = _index;
            _setSelect(_isSelect);
            _refreshWnd();
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelect"></param>
        private void _setSelect(bool _isSelect)
        {
            _m_btnToggle?.setState(_isSelect);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null == _m_ruleRefObj)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_ruleRefObj.name));
            _m_iconWnd?.setTexture(_m_ruleRefObj.icon);
            _m_iconWnd?.showWnd();
        }

        /// <summary>
        /// 点击展开或者收起
        /// </summary>
        /// <param name="_toggleWnd"></param>
        private void _clickTogle(NPGGUIWndCommonToggleEx _toggleWnd)
        {            
            _m_clickAction?.Invoke(this, !_toggleWnd.isOn, _m_index);
            _m_btnToggle?.setSelected(!_toggleWnd.isOn);
        }

        /// <summary>
        /// 点击跳转
        /// </summary>
        /// <param name="_obj"></param>
        private void _clickGoto(GameObject _obj)
        {
            if (null == _m_ruleRefObj && null == _m_ruleRefObj.client_effect)
                return;
            _m_ruleRefObj.client_effect.dealEffect();
        }
    }
}
