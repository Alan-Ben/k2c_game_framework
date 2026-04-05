using System;
using System.Collections.Generic;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴相性筛选按钮子窗口
    /// </summary>
    public class GGUIWndHeroMainFilter : _ANPGGUIBasicSubWnd<GGUIMonoHeroMainFilter>
    {
        //点击tab事件
        private Action<ESpecAttrType> _m_aOnSortChanged;
        //页签列表
        private List<GGUIWndHeroMainFilterTab> _m_lSortTabWndList;
        //当前选中的页签
        private GGUIWndHeroMainFilterTab _m_wSelectSortTabWnd;

        /// <summary>
        /// 选中事件
        /// </summary>
        public Action<ESpecAttrType> onFilterChanged
        {
            get { return _m_aOnSortChanged; }
            set { _m_aOnSortChanged = value; }
        }

        public GGUIWndHeroMainFilter(GGUIMonoHeroMainFilter _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if (_m_lSortTabWndList != null)
            {
                GGUIWndHeroMainFilterTab tempTabItem = null;
                for (int i = 0; i < _m_lSortTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lSortTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            _m_aOnSortChanged = null;

            if (_m_lSortTabWndList != null)
            {
                GGUIWndHeroMainFilterTab tempTabItem = null;
                for (int i = 0; i < _m_lSortTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lSortTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.discard();
                }
                _m_lSortTabWndList.Clear();
                _m_lSortTabWndList = null;
            }
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            _m_lSortTabWndList = new List<GGUIWndHeroMainFilterTab>();
            if (null != wnd.monoFilterTabList)
            {
                GGUIHeroMainFilterTabMono tempTabMono = null;
                GGUIWndHeroMainFilterTab tempTabItem = null;
                for (int i = 0; i < wnd.monoFilterTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoFilterTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndHeroMainFilterTab(tempTabMono.monoTab, tempTabMono.tabType, tempTabMono.goTag);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickButton += _onSortTabSelect;
                    _m_lSortTabWndList.Add(tempTabItem);
                }
            }

            _m_aOnSortChanged = null;
        }

        /// <summary>
        /// 初始化状态，默认选中之前的
        /// </summary>
        public void initState(ESpecAttrType _defaultType)
        {
            if (_m_lSortTabWndList != null)
            {
                for (int i = 0; i < _m_lSortTabWndList.Count; i++)
                {
                    if (_m_lSortTabWndList[i] != null && _m_lSortTabWndList[i].tabType == _defaultType)
                    {
                        //先设置之前的为不选中
                        if (_m_wSelectSortTabWnd != null)
                            _m_wSelectSortTabWnd.setSelected(false);

                        _m_wSelectSortTabWnd = _m_lSortTabWndList[i];
                        _m_wSelectSortTabWnd.setSelected(true);
                    }
                }
            }
        }

        /// <summary>
        /// 设置显示标签
        /// </summary>
        /// <param name="_targetType"></param>
        public void setShowTag(ESpecAttrType _targetType)
        {
            if(_m_lSortTabWndList == null)
                return;

            for (int i = 0; i < _m_lSortTabWndList.Count; i++)
            {
                if(_m_lSortTabWndList[i] == null)
                    continue;

                if (_m_lSortTabWndList[i].tabType == _targetType)
                    _m_lSortTabWndList[i].setShowTag(true);
                else
                    _m_lSortTabWndList[i].setShowTag(false);
            }
        }

        /// <summary>
        /// 设置显示标签
        /// </summary>
        /// <param name="_targetTypeList"></param>
        public void setShowTagList(List<ESpecAttrType> _targetTypeList)
        {
            if (_m_lSortTabWndList == null || _targetTypeList == null)
                return;

            for (int i = 0; i < _m_lSortTabWndList.Count; i++)
            {
                if (_m_lSortTabWndList[i] == null)
                    continue;

                if (_targetTypeList.Contains(_m_lSortTabWndList[i].tabType))
                    _m_lSortTabWndList[i].setShowTag(true);
                else
                    _m_lSortTabWndList[i].setShowTag(false);
            }
        }

        //点击切换页签按钮
        private void _onSortTabSelect(GGUIWndHeroMainFilterTab _tabItemWnd)
        {
            if (null == _tabItemWnd || _m_wSelectSortTabWnd == _tabItemWnd)
                return;

            //取消原来的选择
            if (null != _m_wSelectSortTabWnd)
                _m_wSelectSortTabWnd.setSelected(false);

            //设置新对象
            _m_wSelectSortTabWnd = _tabItemWnd;

            if (null != _m_wSelectSortTabWnd)
            {
                _m_wSelectSortTabWnd.setSelected(true);
                if (_m_aOnSortChanged != null)
                    _m_aOnSortChanged(_m_wSelectSortTabWnd.tabType);
            }
        }
    }
}
