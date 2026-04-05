using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 民意-选择求助-选项子项
    /// </summary>
    public class GGUIWndMarsPopularWillChoiceHelpOptionItem : _ANPGGUIBasicSubWnd<GGUIMonoMarsPopularWillChoiceHelpOptionItem>
    {
        private int _m_index;
        private string _m_desc;
        
        private bool _m_isSelected;

        public GGUIWndMarsPopularWillChoiceHelpOptionItem(GGUIMonoMarsPopularWillChoiceHelpOptionItem _mono) : base(_mono)
        {
            initWnd();
        }

        public int index { get { return _m_index; } }
        public bool isSelected { get { return _m_isSelected; } }

        public event Action<GGUIWndMarsPopularWillChoiceHelpOptionItem> onClick;
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 绑定点击
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickBtn);
        }

        protected override void _onDiscard()
        {
            _m_desc = null;
            onClick = null;
            
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickBtn);
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
        }

        /// <summary>
        /// 设置内容与回调
        /// </summary>
        public void setInfo(int _index, string _desc)
        {
            _m_index = _index;
            _m_desc = _desc;
            
            _refreshInfo();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshInfo()
        {
            if (wnd == null || !isShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_desc));
        }
        
        /// <summary>
        /// 设置选中状态
        /// </summary>
        public void setSelected(bool _selected)
        {
            _m_isSelected = _selected;
            
            if (wnd == null || !isShow)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.selectShowGoList, _m_isSelected);
            ALUGUICommon.setGameObjEnable(wnd.unSelectShowGoList, !_m_isSelected);
        }

        private void _onClickBtn(GameObject _)
        {
            onClick?.Invoke(this);
        }
    }
}
