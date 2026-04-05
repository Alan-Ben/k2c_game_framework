
using System;
using System.Collections.Generic;
using ALPackage;
using Common;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用下拉选择框子窗口,多选
    /// </summary>
    public class NPGGUIWndCommonDropDownBoxMulti : _ATALBasicUISubWnd<NPGGUIMonoDropDownBox>
    {
        //当前选中tab
        private NPGGUIWndCommonTab _m_wSelectTab;
        //列表容器
        private NPGGUIWndCommonDropDownBoxContainer _m_commonDropDownBoxContainer;

        //当前选中id列表
        private HashSet<long> _m_curSelectItemIdList;
        //容器数据列表
        private List<_INPGGUICommonDropDownBoxInstance> _m_boxInstanceList;
        //点击item的回调，之前的选择集合，当前的选择集合
        private Action<HashSet<long>, HashSet<long>> _m_OnClickItem;

        public NPGGUIWndCommonDropDownBoxMulti(NPGGUIMonoDropDownBox _mono) : base(_mono)
        {
            initWnd();
        }

        public Action<HashSet<long>, HashSet<long>> onClickItem { get { return _m_OnClickItem; } set { _m_OnClickItem = value; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            _m_boxInstanceList = new List<_INPGGUICommonDropDownBoxInstance>();
            _m_curSelectItemIdList = new HashSet<long>();
            
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
            if(null == _m_curSelectItemIdList)
                _m_curSelectItemIdList = new HashSet<long>();
            _m_curSelectItemIdList.Clear();
            
            if(null != _m_wSelectTab)
                _m_wSelectTab.showWnd();
            
            if(null != _m_commonDropDownBoxContainer)
                _m_commonDropDownBoxContainer.showWnd();
            
            _refreshView();
        }

        protected override void _onHideWnd()
        {
            if(null == _m_curSelectItemIdList)
                _m_curSelectItemIdList = new HashSet<long>();
            _m_curSelectItemIdList.Clear();

            if(null != _m_wSelectTab)
                _m_wSelectTab.hideWnd();
            
            if(null != _m_commonDropDownBoxContainer)
                _m_commonDropDownBoxContainer.hideWnd();
        }

        protected override void _onReset() 
        {
            if(null == _m_curSelectItemIdList)
                _m_curSelectItemIdList = new HashSet<long>();
            _m_curSelectItemIdList.Clear();

            if(null != _m_wSelectTab)
               _m_wSelectTab.resetWnd();
           
           if(null != _m_commonDropDownBoxContainer)
               _m_commonDropDownBoxContainer.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(null == _m_curSelectItemIdList)
                _m_curSelectItemIdList = new HashSet<long>();
            _m_curSelectItemIdList.Clear();

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
            if(null == _boxInstances)
                return;
            
            if (null != _m_boxInstanceList)
            {
                _m_boxInstanceList.Clear();
                _m_boxInstanceList.AddRange(_boxInstances);
            }

            _refreshView();
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
            
            if(null == _m_curSelectItemIdList)
                return;

            //之前点击的数据集合
            HashSet<long> oldSelectList = new HashSet<long>(_m_curSelectItemIdList);
            
            //已经选中了取消选中
            if (_m_curSelectItemIdList.Contains(_instanceInfo.instanceId))
            {
                _m_curSelectItemIdList.Remove(_instanceInfo.instanceId);
                _m_commonDropDownBoxContainer.setItemIsSelect(_instanceInfo.instanceId, false);
            }
            //没有选中选中新的
            else
            {
                _m_curSelectItemIdList.Add(_instanceInfo.instanceId);
                _m_commonDropDownBoxContainer.setItemIsSelect(_instanceInfo.instanceId, true);
            }

            //触发回调
            if (null != _m_OnClickItem)
                _m_OnClickItem(oldSelectList, _m_curSelectItemIdList);
        }
        
        /// <summary>
        /// 清空选中
        /// </summary>
        public void clearSelect()
        {
            if(null == _m_curSelectItemIdList || null == _m_boxInstanceList || null == _m_commonDropDownBoxContainer)
                return;
            
            _m_curSelectItemIdList.Clear();
            
            foreach (_INPGGUICommonDropDownBoxInstance dropDownBoxInstance in _m_boxInstanceList)
            {
                _m_commonDropDownBoxContainer.setItemIsSelect(dropDownBoxInstance.instanceId, false);
            }
        }

        /// <summary>
        /// 强制选中
        /// </summary>
        public void forceSelect(long _instanceId, bool _isSelect)
        {
            if(null == _m_commonDropDownBoxContainer || null == _m_curSelectItemIdList)
                return;

            if (_isSelect)
            {
                _m_curSelectItemIdList.Add(_instanceId);
            }
            else
            {
                _m_curSelectItemIdList.Remove(_instanceId);
            }
            
            _m_commonDropDownBoxContainer.setItemIsSelect(_instanceId, _isSelect);
        }
    }
}