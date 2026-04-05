using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 藏品分解选择页签子窗口
    /// </summary>
    public class GGUIWndEquipRecycleBatchSelect : _ANPGGUIBasicSubWnd<GGUIMonoEquipRecycleBatchSelect>
    {
        //点击tab事件
        private Action<EEquipRecycleBatchSelectTabType> _m_aOnSortChanged;
        //页签列表
        private List<GGUIWndEquipRecycleBatchSelectTab> _m_lSortTabWndList;
        //当前选中的页签
        private GGUIWndEquipRecycleBatchSelectTab _m_wSelectSortTabWnd;
        //选中隐藏切换控件
        private NPGGUIWndCommonToggleEx _m_wToggle;

        /// <summary>
        /// 选中事件
        /// </summary>
        public Action<EEquipRecycleBatchSelectTabType> onSortChanged
        {
            get { return _m_aOnSortChanged; }
            set { _m_aOnSortChanged = value; }
        }

        /// <summary>
        /// 选择类型
        /// </summary>
        public EEquipRecycleBatchSelectTabType curSelectType
        {
            get
            {
                return _m_wSelectSortTabWnd != null
                    ? _m_wSelectSortTabWnd.tabType
                    : EEquipRecycleBatchSelectTabType.NONE;
            }
        }

        public GGUIWndEquipRecycleBatchSelect(GGUIMonoEquipRecycleBatchSelect _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            if (wnd == null)
                return;

            _m_wToggle?.showWnd();
            _m_wToggle?.setSelected(false,true,false);

            _refreshButtonName();
        }

        protected override void _onHideWnd()
        {
            _m_wToggle?.hideWnd();
            _m_wSelectSortTabWnd?.setSelected(false);
            _m_wSelectSortTabWnd = null;
        }

        protected override void _onReset()
        {
            if (_m_lSortTabWndList != null)
            {
                GGUIWndEquipRecycleBatchSelectTab tempTabItem = null;
                for (int i = 0; i < _m_lSortTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lSortTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.resetWnd();
                }
            }

            _m_wToggle?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_aOnSortChanged = null;

            if (_m_lSortTabWndList != null)
            {
                GGUIWndEquipRecycleBatchSelectTab tempTabItem = null;
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

            _m_wToggle?.discard();
            _m_wToggle = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            _m_lSortTabWndList = new List<GGUIWndEquipRecycleBatchSelectTab>();
            if (null != wnd.monoSortTabList)
            {
                GGUIMonoEquipRecycleBatchSelectTabMono tempTabMono = null;
                GGUIWndEquipRecycleBatchSelectTab tempTabItem = null;
                for (int i = 0; i < wnd.monoSortTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoSortTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndEquipRecycleBatchSelectTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickButton += _onSortTabSelect;
                    _m_lSortTabWndList.Add(tempTabItem);
                }
            }

            _m_aOnSortChanged = null;

            if (wnd.monoToggle != null)
            {
                _m_wToggle = new NPGGUIWndCommonToggleEx(wnd.monoToggle);
                _m_wToggle.clickDelegate += _onClickSelectButton;
            }
        }

        /// <summary>
        /// 初始化状态，默认选中之前的
        /// </summary>
        public void initState(EEquipRecycleBatchSelectTabType _defaultType)
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
                        _refreshButtonName();//刷新按钮名字
                    }
                }
            }
        }

        //设置选中状态
        public void setSelected(bool _isSelect, bool _force = false)
        {
            if (null == wnd)
                return;

            _m_wToggle?.setSelected(_isSelect, _force);
        }

        //刷新按钮名字
        private void _refreshButtonName()
        {
            if (wnd == null || _m_wSelectSortTabWnd == null)
                return;

            string btnName = null;
            switch (_m_wSelectSortTabWnd.tabType)
            {
                case EEquipRecycleBatchSelectTabType.GREEN_AND_BELOW://绿色及以下
                    btnName = TextTranslate.instance.getLanguage(TransKeyConst.equip_greedAndBelow_none);
                    break;
                case EEquipRecycleBatchSelectTabType.BLUE_AND_BELOW://蓝色及以下
                    btnName = TextTranslate.instance.getLanguage(TransKeyConst.equip_blueAndBelow_none);
                    break;
                case EEquipRecycleBatchSelectTabType.PURPLE_AND_BELOW://紫色及以下
                    btnName = TextTranslate.instance.getLanguage(TransKeyConst.equip_purpleAndBelow_none);
                    break;
                case EEquipRecycleBatchSelectTabType.ORANGE_AND_BELOW://橙色及以下
                    btnName = TextTranslate.instance.getLanguage(TransKeyConst.equip_orangeAndBelow_none);
                    break;
            }

            ALUGUICommon.setLabelTxt(wnd.txtButtonName, btnName);
        }

        //默认收缩状态，点击展开，再次点击收缩
        private void _onClickSelectButton(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_toggle == null)
                return;

            setSelected(!_toggle.isOn);
        }

        //点击切换页签按钮
        private void _onSortTabSelect(GGUIWndEquipRecycleBatchSelectTab _tabItemWnd)
        {
            if (null == _tabItemWnd || _m_wSelectSortTabWnd == _tabItemWnd)
            {
                //关闭页签
                setSelected(false);
                return;
            }

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
                //刷新按钮名字
                _refreshButtonName();
                //关闭页签
                setSelected(false);
            }
        }
    }
}
