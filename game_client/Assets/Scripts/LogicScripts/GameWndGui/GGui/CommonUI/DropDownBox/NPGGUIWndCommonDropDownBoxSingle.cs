
using System;
using System.Collections.Generic;
using ALPackage;
using Common;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用下拉选择框子窗口,单选
    /// </summary>
    public class NPGGUIWndCommonDropDownBoxSingle : _ATALBasicUISubWnd<NPGGUIMonoDropDownBox>
    {
        //当前选中tab
        private NPGGUIWndCommonTab _m_wSelectTab;
        //列表容器
        private NPGGUIWndCommonDropDownBoxContainer _m_commonDropDownBoxContainer;

        //当前选中id
        private int _m_curSelectItemId;
        //容器数据列表
        private List<_INPGGUICommonDropDownBoxInstance> _m_boxInstanceList;
        //点击item的回调
        private Action<int> _m_OnClickItem;

        public NPGGUIWndCommonDropDownBoxSingle(NPGGUIMonoDropDownBox _mono) : base(_mono)
        {
            initWnd();
        }

        public Action<int> onClickItem { get { return _m_OnClickItem; } set { _m_OnClickItem = value; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            _m_boxInstanceList = new List<_INPGGUICommonDropDownBoxInstance>();
            _m_curSelectItemId = -1;
            
            if (wnd.monoCommonTab != null)
            {
                _m_wSelectTab = new NPGGUIWndCommonTab(wnd.monoCommonTab);
                _m_wSelectTab.clickDelegate += _onClickSelectTab;
            }

            if (null != wnd.monoDropDownBoxContainer)
            {
                _m_commonDropDownBoxContainer = new NPGGUIWndCommonDropDownBoxContainer(wnd.monoDropDownBoxContainer);
                _m_commonDropDownBoxContainer.onClickItem += _onClickBox;
            }
        }

        protected override void _onShowWnd()
        {
            if(null == wnd)
                return;
            
            _m_curSelectItemId = -1;
            
            if(null != _m_wSelectTab)
                _m_wSelectTab.showWnd();
            
            _refreshView();
            
            //设置初始是否选择
            _onClickSelectTab(wnd.defaultIsSelect);

        }

        protected override void _onHideWnd()
        {
            _m_curSelectItemId = -1;

            if(null != _m_wSelectTab)
                _m_wSelectTab.hideWnd();
            
            if(null != _m_commonDropDownBoxContainer)
                _m_commonDropDownBoxContainer.hideWnd();
        }

        protected override void _onReset() 
        {
            _m_curSelectItemId = -1;

            if(null != _m_wSelectTab)
               _m_wSelectTab.resetWnd();
           
           if(null != _m_commonDropDownBoxContainer)
               _m_commonDropDownBoxContainer.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_curSelectItemId = -1;

            if(null != _m_wSelectTab)
               _m_wSelectTab.discard();
           _m_wSelectTab = null;
           
           if(null != _m_commonDropDownBoxContainer)
               _m_commonDropDownBoxContainer.discard();
           _m_commonDropDownBoxContainer = null;
           
           if(null != _m_boxInstanceList)
               _m_boxInstanceList.Clear();
           _m_boxInstanceList = null;

           _m_OnClickItem = null;
        }

        //设置数据
        public void setInfo(List<_INPGGUICommonDropDownBoxInstance> _boxInstances)
        {
            if(null == _boxInstances || _boxInstances.Count == 0)
                return;
            
            if (null != _m_boxInstanceList)
            {
                _m_boxInstanceList.Clear();
                _m_boxInstanceList.AddRange(_boxInstances);
            }

            _refreshView();
            
        }

        /// <summary>
        /// 强制设置当前选中第几个
        /// </summary>
        public void setSelect(int _index = 0)
        {
            if(null == _m_boxInstanceList || _m_boxInstanceList.Count == 0)
                return;
            
            _onClickBox(_m_boxInstanceList[_index]);
        }

        //刷新数据
        private void _refreshView()
        {
            if(null == _m_boxInstanceList)
                return;

            if (null != _m_commonDropDownBoxContainer)
            {
                _m_commonDropDownBoxContainer.setItemList(_m_boxInstanceList);
            }
        }
        
        //点击选择tab
        private void _onClickSelectTab(bool _isOn)
        {
            _setSelectTab(_isOn);
        }
        
        //设置选择的tab是否选中
        private void _setSelectTab(bool _isSelect)
        {
            if (null == wnd || _m_wSelectTab == null)
                return;

            _m_wSelectTab.setSelected(_isSelect);

            if (_m_wSelectTab.isOn)
            {
                if(null != _m_commonDropDownBoxContainer)
                    _m_commonDropDownBoxContainer.showWnd();
            }
            else
            {
                if(null != _m_commonDropDownBoxContainer)
                    _m_commonDropDownBoxContainer.hideWnd();
            }
        }

        /// <summary>
        /// 点击子item
        /// </summary>
        /// <param name="_instanceInfo"></param>
        private void _onClickBox(_INPGGUICommonDropDownBoxInstance _instanceInfo)
        {
            if(null == _instanceInfo || null == _m_commonDropDownBoxContainer)
                return;
            
            if(_m_curSelectItemId == _instanceInfo.instanceId)
                return;
            
            //取消选中
            _m_commonDropDownBoxContainer.setItemIsSelect(_m_curSelectItemId, false);

            //选中新的
            _m_curSelectItemId = _instanceInfo.instanceId;
            _m_commonDropDownBoxContainer.setItemIsSelect(_m_curSelectItemId, true);

            //设置选中文本
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_instanceInfo.content));
            
            //触发回调
            if (null != _m_OnClickItem)
                _m_OnClickItem(_m_curSelectItemId);

            //单选框选中完就关掉
            _setSelectTab(false);
        }
    }
}