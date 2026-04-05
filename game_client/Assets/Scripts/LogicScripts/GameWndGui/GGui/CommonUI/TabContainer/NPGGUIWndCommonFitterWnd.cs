using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    //通用筛选界面
    public class NPGGUIWndCommonFitterWnd<T> : _ANPGGUIBasicSubWnd<NPGGUIMonoCommonFitterWnd>
    {
        private Action<T, bool> _m_onClickTab;
        private NPGGUIWndCommonFitterContainer<T> _m_tabComtainer;
        private Func<T,bool> _m_selectedList;
        private List<NPGGUICommonFitterMono<T>> _m_monoList;

        private List<T> _m_allTypes;
        private NPGGUIWndCommonToggleEx _m_toggleAllSelected;

        public NPGGUIWndCommonFitterWnd(NPGGUIMonoCommonFitterWnd _wnd,Action<T, bool> _onClickTab) : base(_wnd)
        {
            _m_onClickTab = _onClickTab;
            _m_allTypes = new List<T>();
            _m_monoList = new List<NPGGUICommonFitterMono<T>>();
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            _m_tabComtainer = new NPGGUIWndCommonFitterContainer<T>(wnd.tabContainer, _onClickTabItem);

            if (null != wnd.toggleAllSelected)
            {
                _m_toggleAllSelected = new NPGGUIWndCommonToggleEx(wnd.toggleAllSelected);
                _m_toggleAllSelected.clickDelegate += _clickAllSelected;
            }
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            if (null != _m_tabComtainer)
            {
                _m_tabComtainer.discard();
                _m_tabComtainer = null;
            }
            _m_monoList.Clear();
            _m_allTypes.Clear();
            
            if(null != _m_toggleAllSelected)
                _m_toggleAllSelected.discard();
            _m_toggleAllSelected = null;
        }
        
        protected virtual void _refreshWnd()
        {
            if (null != _m_tabComtainer)
            {
                _m_tabComtainer.setDefaultSelected(_m_selectedList);
                _m_monoList = _getMonoList();
                _m_tabComtainer.showList(_m_monoList);
            }

            _refreshAllTypes();

            _refreshBtn();
        }

        private void _refreshAllTypes()
        {
            
            if(null == _m_allTypes)
            {
                _m_allTypes = new List<T>();
            }
            _m_allTypes.Clear();
            
            for(int i=0;i<_m_monoList.Count;i++)
            {
                _m_allTypes.Add(_m_monoList[i].type);
            }

        }

        private void _refreshBtn()
        {
            bool isAllSelected = _getIsAllSelected();
            _m_toggleAllSelected?.setSelected(!isAllSelected);
        }
        
        /// <summary>
        /// 所有类型列表
        /// </summary>
        /// <returns></returns>
        protected List<T> _getAllTypes()
        {
            return _m_allTypes;
        }

        /// <summary>
        /// 是否全部选中
        /// </summary>
        /// <returns></returns>
        private bool _getIsAllSelected()
        {
            if (null == _m_selectedList)
                return false;

            if (null != _m_tabComtainer)
                return _m_tabComtainer.isAllSelected();
            
            return false;
        }


        private void _clickAllSelected(NPGGUIWndCommonToggleEx _obj)
        {
            _setAllSelected(_obj.isOn);
            _onClickAllSelected(_obj.isOn);
            _refreshBtn();
        }

        /// <summary>
        /// 点击全选或者全取消后，处理数据
        /// </summary>
        /// <param name="_isOn"></param>
        protected virtual void _onClickAllSelected(bool _isOn)
        {
            
        }

        protected void _setAllSelected(bool _isOn)
        {
            if (null != _m_tabComtainer)
                _m_tabComtainer.setAllSelected(_isOn);
        }

        /// <summary>
        /// 设置默认选中列表
        /// </summary>
        /// <param name="_defaultOnList"></param>
        public void setInfo(Func<T, bool> _defaultOnList)
        {
            _m_selectedList = _defaultOnList;
            _refreshWnd();
        }

        protected virtual List<NPGGUICommonFitterMono<T>> _getMonoList()
        {
            return null;
        }

        private void _onClickTabItem(NPGGUIWndCommonFitterTab<T> _itemWnd)
        {
            if(_itemWnd == null)
                return;
            
            _itemWnd.setSelected(!_itemWnd.isOn);
            
            if (null != _m_onClickTab)
            {
                _m_onClickTab(_itemWnd.info.type, _itemWnd.isOn);
            }   
            
            _refreshBtn();
        }
    }
}
