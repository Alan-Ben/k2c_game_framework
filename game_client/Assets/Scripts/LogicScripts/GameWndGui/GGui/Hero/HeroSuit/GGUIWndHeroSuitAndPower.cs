using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴套系及战力详情界面
    /// </summary>
    public class GGUIWndHeroSuitAndPower : _ANPGGUIBasicWnd<GGUIMonoHeroSuitAndPower>
    {
        private static GGUIWndHeroSuitAndPower _g_instance;
        public static GGUIWndHeroSuitAndPower instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroSuitAndPower();
                return _g_instance;
            }
        }

        //伙伴信息
        private HeroInfo _m_heroInfo;
        //页签列表
        private List<GGUIWndHeroSuitAndPowerTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndHeroSuitAndPowerTab _m_wSelectTabWnd;
        //套系详情页面
        private GGUIWndHeroSuitDetailPage _m_wSuitDetailPage;
        //战力详情页面
        private GGUIWndHeroPowerDetailPage _m_wPowerDetailPage;

        public GGUIWndHeroSuitAndPower() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroSuitAndPower.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroSuitAndPower.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _hideAllPage();
        }

        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndHeroSuitAndPowerTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    //重置状态
                    tempTabItem.resetWnd();
                }
            }

            _m_wSelectTabWnd = null;

            _m_wSuitDetailPage?.resetWnd();
            _m_wPowerDetailPage?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndHeroSuitAndPowerTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.discard();
                }
                _m_lTabWndList.Clear();
                _m_lTabWndList = null;
            }

            _m_wSelectTabWnd = null;

            _m_wSuitDetailPage?.discard();
            _m_wSuitDetailPage = null;

            _m_wPowerDetailPage?.discard();
            _m_wPowerDetailPage = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndHeroSuitAndPowerTab>();
            if (null != wnd.monoTabList)
            {
                GGUIHeroSuitAndPowerTabMono tempTabMono = null;
                GGUIWndHeroSuitAndPowerTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndHeroSuitAndPowerTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_tabType"></param>
        public void setInfo(HeroInfo _heroInfo, EHeroSuitAndPowerTabType _selectTabType)
        {
            if (wnd == null || _heroInfo == null)
                return;

            _m_heroInfo = _heroInfo;

            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.HERO, _heroInfo.id));

            _refreshPageWnd(_selectTabType);
        }

        /// <summary>
        /// 刷新页签窗口
        /// </summary>
        private void _refreshPageWnd(EHeroSuitAndPowerTabType _selectTabType)
        {
            if (null == wnd || null == _m_heroInfo || null == _m_heroInfo.heroRefObj)
                return;

            bool haveSuit = _m_heroInfo.heroRefObj.suit_id > 0;
            GGUIWndHeroSuitAndPowerTab tempTabItem = null;
            for (int i = 0; i < _m_lTabWndList.Count; ++i)
            {
                tempTabItem = _m_lTabWndList[i];
                if (tempTabItem == null)
                    continue;
                tempTabItem.showWnd();

                //检查是否有套系，否则隐藏页签
                if (tempTabItem.tabType == EHeroSuitAndPowerTabType.SUIT && !haveSuit)
                    tempTabItem.hideWnd();
            }

            //没有套系时需要显隐设置
            ALUGUICommon.setGameObjEnable(wnd.goNoSuitShowList, !haveSuit);
            ALUGUICommon.setGameObjEnable(wnd.goNoSuitHideList, haveSuit);

            //如果没有套系却选择了套系，则修改为战力页签
            if (!haveSuit && _selectTabType == EHeroSuitAndPowerTabType.SUIT)
                _selectTabType = EHeroSuitAndPowerTabType.POWER;

            //设置选中页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGUIWndHeroSuitAndPowerTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabType == _selectTabType)
                    {
                        _onTabSelect(itemTab);
                        break;
                    }
                }
            }
            else
            {
                _m_wSelectTabWnd.setSelected(true);
                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        #region 页签页面处理

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndHeroSuitAndPowerTab _tabItemWnd)
        {
            if (null == _tabItemWnd || _m_wSelectTabWnd == _tabItemWnd)
                return;

            //取消原来的选择
            if (null != _m_wSelectTabWnd)
                _m_wSelectTabWnd.setSelected(false);

            //设置新对象
            _m_wSelectTabWnd = _tabItemWnd;

            if (null != _m_wSelectTabWnd)
            {
                _m_wSelectTabWnd.setSelected(true);

                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        //根据页签刷新列表内容
        private void _refreshTabView(EHeroSuitAndPowerTabType _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case EHeroSuitAndPowerTabType.SUIT://套系详情
                    _showSuitPage();
                    break;
                case EHeroSuitAndPowerTabType.POWER://战力详情
                    _showPowerPage();
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wSuitDetailPage?.hideWnd();
            _m_wPowerDetailPage?.hideWnd();
        }

        //显示套系页面
        private void _showSuitPage()
        {
            if (wnd == null || _m_heroInfo == null)
                return;
            
            if (_m_wSuitDetailPage != null)
            {
                _m_wSuitDetailPage.showWnd();
                _m_wSuitDetailPage.setInfo(_m_heroInfo);
            }
            else
            {
                _m_wSuitDetailPage = new GGUIWndHeroSuitDetailPage(_getPageAssetPathByType(EHeroSuitAndPowerTabType.SUIT), wnd.pageParent);
                _m_wSuitDetailPage.load(() =>
                {
                    if (_m_wSuitDetailPage == null)
                        return;
                    _m_wSuitDetailPage.showWnd();
                    _m_wSuitDetailPage.setInfo(_m_heroInfo);
                });
            }
        }

        //显示战力页面
        private void _showPowerPage()
        {
            if (wnd == null || _m_heroInfo == null)
                return;
            
            if (_m_wPowerDetailPage != null)
            {
                _m_wPowerDetailPage.showWnd();
                _m_wPowerDetailPage.setInfo(_m_heroInfo);
            }
            else
            {
                _m_wPowerDetailPage = new GGUIWndHeroPowerDetailPage(_getPageAssetPathByType(EHeroSuitAndPowerTabType.POWER), wnd.pageParent);
                _m_wPowerDetailPage.load(() =>
                {
                    if (_m_wPowerDetailPage == null)
                        return;
                    _m_wPowerDetailPage.showWnd();
                    _m_wPowerDetailPage.setInfo(_m_heroInfo);
                });
            }
        }

        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(EHeroSuitAndPowerTabType _type)
        {
            if (wnd == null || wnd.monoTabList == null)
                return null;

            foreach (var mono in wnd.monoTabList)
            {
                if (mono.tabType == _type)
                    return NPCommonAssetPathInfo.readFromUiResId(mono.tabSubWndAssetId);
            }

            return null;
        }

        #endregion

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_SUIT_AND_POWER);
        }
    }
}