using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public abstract class _AGGUIWndTreasureHuntCatalogTabWnd<T> : _ANPGGUIBasicResBarWnd<T> where T : GGUIMonoTreasureHuntCatalogTabWnd
    {
        private NPGGuiWndTexture _m_wSelectedTabBanner;//选择tab的banner
        private GGUIWndTreasureHuntCatalogTabContainer _m_wTabContainer;//页签容器
        
        protected int _m_iSelectedTabIndex;//当前选择的tab索引
        
        protected _AGGUIWndTreasureHuntCatalogTabWnd() : base(EALUIWndLayer.NORMAL)
        {
        }

        public override bool showResBarBySelf { get { return true; } }

        /// <summary>
        /// 页签配表数据
        /// </summary>
        [NotNull] protected abstract List<TreasureHuntCatalogTabRefObj> tabRefObjList { get; }

        /// <summary>
        /// 选中的页签配表数据
        /// </summary>
        protected TreasureHuntCatalogTabRefObj selectedTabRefObj { get { return tabRefObjList.SafeGet(_m_iSelectedTabIndex); } }

        protected override void _onWndInitDone()
        {
            if (wnd != null)
            {
                if (wnd.selectedTabImgBanner != null)
                    _m_wSelectedTabBanner = new NPGGuiWndTexture(wnd.selectedTabImgBanner);

                if (wnd.tabContainer != null)
                {
                    _m_wTabContainer = new GGUIWndTreasureHuntCatalogTabContainer(wnd.tabContainer);
                    _m_wTabContainer.onTabClick += _onCatalogTabClick;
                }
            
                ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }

            _onWndInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
            
            _m_wSelectedTabBanner?.discard();
            _m_wSelectedTabBanner = null;
            
            if (_m_wTabContainer != null)
            {
                _m_wTabContainer.onTabClick -= _onCatalogTabClick;
                _m_wTabContainer.discard();
                _m_wTabContainer = null;
            }
            
            _onDiscardSub();
        }
        
        protected override void _onShowWnd()
        {
            _onShowWndSub();
            
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wSelectedTabBanner?.hideWnd();
            _m_wTabContainer?.hideWnd();

            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _m_wSelectedTabBanner?.discardTexture();
            _m_wTabContainer?.resetWnd();
            
            _onResetSub();
        }

        private void _refreshWnd()
        {
            if (_m_wTabContainer != null)
            {
                _m_wTabContainer.showWnd();
                _m_wTabContainer.setData(tabRefObjList, _m_iSelectedTabIndex);
            }

            _refreshSelectedTabInfo();

            _refreshWndSub();
        }
        
        /// <summary>
        /// 刷新选中的tab信息
        /// </summary>
        private void _refreshSelectedTabInfo()
        {
            if (wnd == null)
                return;
            
            TreasureHuntCatalogTabRefObj tabRefObj = this.selectedTabRefObj;
            if (tabRefObj == null)
            {
                _m_wSelectedTabBanner?.hideWnd();
                ALUGUICommon.setLabelTxt(wnd.txtSelectedTabName, string.Empty);
                return;
            }
            
            if (_m_wSelectedTabBanner != null)
            {
                _m_wSelectedTabBanner.showWnd();
                _m_wSelectedTabBanner.setTexture(tabRefObj.tab_banner);
            }

            ALUGUICommon.setLabelTxt(wnd.txtSelectedTabName, TextTranslate.instance.getLanguage(tabRefObj.tab_name));
        }
        
        public void setSelectTab(int _tabIndex)
        {
            _m_iSelectedTabIndex = _tabIndex;
            List<TreasureHuntCatalogTabRefObj> tabList = tabRefObjList;
            // 若下标不合法, 则设置为0(默认选中第一个)
            if(_m_iSelectedTabIndex < 0 || _m_iSelectedTabIndex >= tabList.Count)
                _m_iSelectedTabIndex = 0;
            
            TreasureHuntCatalogTabRefObj tabRefObj = this.selectedTabRefObj;
            
            _m_wTabContainer?.setSelectIndex(_m_iSelectedTabIndex);
            _refreshSelectedTabInfo();

            _onSelectTab(tabRefObj);
        }

        public void setSelectTab(long _tabRefObjId)
        {
            _m_iSelectedTabIndex = _getTabRefObjIndex(_tabRefObjId);
            setSelectTab(_m_iSelectedTabIndex);
        }
        
        /// <summary>
        /// 获取页签配表下标
        /// </summary>
        /// <param name="_tabRefObjId">页签配表id</param>
        /// <returns>返回在tabRefObjList中的下标, 若找不到返回0</returns>
        protected int _getTabRefObjIndex(long _tabRefObjId)
        {
            List<TreasureHuntCatalogTabRefObj> tabList = tabRefObjList;
            if (tabList.Count <= 0)
                return 0;

            TreasureHuntCatalogTabRefObj item = null;
            for (int i = 0; i < tabList.Count; i++)
            {
                item = tabList[i];
                if (item != null && item.tab_id == _tabRefObjId)
                    return i;
            }

            return 0;
        }
        
        /// <summary>
        /// 当图鉴页签被点击时
        /// </summary>
        private void _onCatalogTabClick(GGUIWndTreasureHuntCatalogTab _catalogTab)
        {
            if (_catalogTab == null || _catalogTab.index == _m_iSelectedTabIndex)
                return;
            
            setSelectTab(_catalogTab.index);
        }

        protected abstract void _onWndInitDoneSub();
        protected abstract void _onDiscardSub();
        protected abstract void _onShowWndSub();
        protected abstract void _onHideWndSub();
        protected abstract void _onResetSub();
        protected abstract void _refreshWndSub();

        /// <summary>
        /// 当选择页签时调用方法()
        /// </summary>
        /// <param name="_refObj">为null时, 为选中所有</param>
        protected abstract void _onSelectTab(TreasureHuntCatalogTabRefObj _refObj);

        /// <summary>
        /// 返回按钮点击事件
        /// </summary>
        /// <param name="_go"></param>
        protected abstract void _onReturnBtnClick(GameObject _go);
    }
}