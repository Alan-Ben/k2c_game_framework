using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴皮肤主界面
    /// </summary>
    public class GGUIWndHeroSkinMain : _ANPGGUIBasicWnd<GGUIMonoHeroSkinMain>
    {
        private static GGUIWndHeroSkinMain _g_instance = new GGUIWndHeroSkinMain();
        public static GGUIWndHeroSkinMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndHeroSkinMain();
                return _g_instance;
            }
        }

        //伙伴配置
        private HeroRefObj _m_heroRef;
        //当前选中的皮肤配置
        private HeroSkinRefObj _m_curSelectSkinRef;
        //当前选中的页签
        private GGUIWndHeroSkinTab _m_wSelectTabWnd;
        //皮肤列表
        private GGUIWndHeroSkinContainer _m_skinContainer;
        //伙伴形象
        private NPGGUIWndCommonShowCase _m_commonShowcaseWnd;
        //页签列表
        private List<GGUIWndHeroSkinTab> _m_lTabWndList;
        //衣柜页面
        private GGUIWndHeroSkinClosetPage _m_wClosetPage;

        public GGUIWndHeroSkinMain() : base(EALUIWndLayer.NORMAL)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroSkinMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroSkinMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_curSelectSkinRef = null;
            _m_skinContainer?.hideWnd();
            _m_commonShowcaseWnd?.hideWnd();
            _hideAllPage();
        }

        protected override void _onReset()
        {
            _m_skinContainer?.resetWnd();
            _m_commonShowcaseWnd?.resetWnd();
            _m_wClosetPage?.resetWnd();

            _m_wSelectTabWnd = null;

            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndHeroSkinTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    //重置状态
                    tempTabItem.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            _m_skinContainer?.discard();
            _m_skinContainer = null;

            _m_commonShowcaseWnd?.discard();
            _m_commonShowcaseWnd = null;

            _m_wClosetPage?.discard();
            _m_wClosetPage = null;

            _m_wSelectTabWnd = null;

            if (_m_lTabWndList != null)
            {
                GGUIWndHeroSkinTab tempTabItem = null;
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
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSkinContainer != null)
            {
                _m_skinContainer = new GGUIWndHeroSkinContainer(wnd.monoSkinContainer);
                _m_skinContainer.onSelectItemChg += _onSelectSkinItem;
            }

            if (wnd.monoShowCaseWnd != null)
                _m_commonShowcaseWnd = new NPGGUIWndCommonShowCase(wnd.monoShowCaseWnd);

            _m_lTabWndList = new List<GGUIWndHeroSkinTab>();
            if (null != wnd.monoTabList)
            {
                GGUIHeroSkinTabMono tempTabMono = null;
                GGUIWndHeroSkinTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndHeroSkinTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(HeroRefObj _heroRef)
        {
            if (_heroRef == null)
                return;

            _m_heroRef = _heroRef;
            _refreshSkinContainer();
        }

        //刷新皮肤列表
        private void _refreshSkinContainer()
        {
            if (_m_heroRef == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_heroRef.id);
            List<HeroSkinRefObj> skinRefList = new List<HeroSkinRefObj>();
            if (_m_heroRef.heroSkinRefList != null)
            {
                for (int i = 0; i < _m_heroRef.heroSkinRefList.Count; i++)
                {
                    if(_m_heroRef.heroSkinRefList[i] != null && !_m_heroRef.heroSkinRefList[i].is_hide)
                        skinRefList.Add(_m_heroRef.heroSkinRefList[i]);
                }
            }

            //排序：初始皮肤，第二优先级：根据皮肤星级从低到高排序，已获得皮肤>未获得皮肤，根据id排序
            skinRefList.Sort((_a, _b) =>
            {
                bool isDefaultA = _m_heroRef.default_skin_id == _a.id;
                bool isDefaultB = _m_heroRef.default_skin_id == _b.id;
                if (isDefaultA.CompareTo(isDefaultB) != 0)
                    return -isDefaultA.CompareTo(isDefaultB);

                EQuality qualityA = GCommon.getItemQuality(ENPItemType.HERO_SKIN, _a.id);
                EQuality qualityB = GCommon.getItemQuality(ENPItemType.HERO_SKIN, _b.id);
                if (qualityA.CompareTo(qualityB) != 0)
                    return -qualityA.CompareTo(qualityB);

                bool isUnlockA = heroInfo != null && heroInfo.heroSkinInfoMgr.getSkinInfo(_a.id) != null;
                bool isUnlockB = heroInfo != null && heroInfo.heroSkinInfoMgr.getSkinInfo(_b.id) != null;
                if (isUnlockA.CompareTo(isUnlockB) != 0)
                    return -isUnlockA.CompareTo(isUnlockB);

                return _a.id.CompareTo(_b.id);
            });

            if (_m_skinContainer != null)
            {
                _m_skinContainer.showWnd();
                _m_skinContainer.showItemList(skinRefList);

                //设置默认选中，第一优先级可解锁皮肤，第二优先级当前穿戴
                HeroSkinRefObj selectRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(heroInfo == null ? _m_heroRef.default_skin_id : heroInfo.curSkinId);
                for (int i = 0; i < skinRefList.Count; i++)
                {
                    bool isUnlock = heroInfo != null && heroInfo.heroSkinInfoMgr.getSkinInfo(skinRefList[i].id) != null;
                    //可解锁皮肤
                    if (!isUnlock && GCommon.isItemEnough(skinRefList[i].unlock_item, false))
                    {
                        selectRef = skinRefList[i];
                        break;
                    }
                }
                _m_skinContainer.setSelect(selectRef);
            }
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshShowCase();
            _refreshPageWnd();
        }

        //刷新形象展示
        private void _refreshShowCase()
        {
            if (wnd == null || _m_curSelectSkinRef == null)
                return;

            //展示形象
            if (_m_commonShowcaseWnd != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[4];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_curSelectSkinRef.td_show), 0);
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_curSelectSkinRef.td_bg_index), 3);
                _m_commonShowcaseWnd.showWnd(showCaseUnitInfoObjList);
            }

            //设置名字称号
            ALUGUICommon.setLabelTxt(wnd.txtTitle, GCommon.getItemName(ENPItemType.HERO_SKIN, _m_curSelectSkinRef.id));
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.HERO, _m_curSelectSkinRef.hero_id));
        }

        //刷新页签窗口
        private void _refreshPageWnd()
        {
            if (null == wnd)
                return;

            //默认页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGUIWndHeroSkinTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabType == EHeroSkinTabType.CLOSET)
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

        //选中皮肤item
        private void _onSelectSkinItem(GGUIWndHeroSkinContainerItem _item)
        {
            if (_item == null || _item.skinRef == null)
                return;

            if (_m_curSelectSkinRef != null && _m_curSelectSkinRef.id == _item.skinRef.id)
                return;

            _m_curSelectSkinRef = _item.skinRef;
            _refreshWnd();
        }

        #region 页签页面处理

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndHeroSkinTab _tabItemWnd)
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
        private void _refreshTabView(EHeroSkinTabType _tabView)
        {
            _hideAllPage();
            switch (_tabView)
            {
                case EHeroSkinTabType.CLOSET://衣柜
                    _showClosetPage();
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wClosetPage?.hideWnd();
        }

        //显示衣柜页面
        private void _showClosetPage()
        {
            if (wnd == null || _m_curSelectSkinRef == null)
                return;
            
            if (_m_wClosetPage != null)
            {
                _m_wClosetPage.showWnd();
                _m_wClosetPage.setInfo(_m_curSelectSkinRef);
            }
            else
            {
                _m_wClosetPage = new GGUIWndHeroSkinClosetPage(_getPageAssetPathByType(EHeroSkinTabType.CLOSET), wnd.pageParent);
                _m_wClosetPage.load(() =>
                {
                    if (_m_wClosetPage == null)
                        return;
                    _m_wClosetPage.showWnd();
                    _m_wClosetPage.setInfo(_m_curSelectSkinRef);
                });
            }
        }

        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(EHeroSkinTabType _type)
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
    }
}
