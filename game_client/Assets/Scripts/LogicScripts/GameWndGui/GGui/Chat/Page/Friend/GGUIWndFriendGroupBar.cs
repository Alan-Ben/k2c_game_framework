using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 背包弹窗:类型bar
    public class GGUIWndFriendGroupBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoFriendGroupBar>
    {
        private Action _m_selectedAction;
        private NPGGUIWndCommonTab _m_tabSelected;
        private string _m_showTypeStr;//显示的文本
        private readonly long _m_uiPathId;
        public event Action onClickEdi;

        public GGUIWndFriendGroupBar(long _uiPathId, Transform _parent, Action _action)
            : base(_parent)
        {
            _m_uiPathId = _uiPathId;
            _m_selectedAction = _action;
        }
        
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }



        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnEdi, _clickEditor);
            if (null != wnd.tabSelected)
            {
                _m_tabSelected = new NPGGUIWndCommonTab(wnd.tabSelected);
                _m_tabSelected.clickDelegate = _onClickDelegate;
            }
        }

        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="obj"></param>
        private void _clickEditor(GameObject obj)
        {
            onClickEdi?.Invoke();
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
        protected override void _onDiscard()
        {
            if (null != _m_tabSelected)
            {
                _m_tabSelected.discard();
                _m_tabSelected = null;
            }
            _m_selectedAction = null;
        }
        
        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_typeStr">显示的文本</param>
        public void setInfo(string _typeStr)
        {
            _m_showTypeStr = _typeStr;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            ALUGUICommon.setLabelTxt(wnd.textTypeName, _m_showTypeStr);
        }

        /// <summary>
        /// 点击展开或者收起
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickDelegate(bool _obj)
        {
            if (null != _m_selectedAction)
            {
                _m_selectedAction();
            }
        }
        
        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setTabSelected(bool _isSelected)
        {
            if (null != _m_tabSelected)
            {
                _m_tabSelected.setSelected(_isSelected);
            }
        }
    }
}
