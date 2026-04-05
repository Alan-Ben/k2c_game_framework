using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品详情页签管理界面
    /// </summary>
    public class GGUIWndEquipDetailPageControl : _ATALBasicUIWnd<GGUIMonoEquipDetailPageControl>
    {
        private static GGUIWndEquipDetailPageControl _g_instance;
        public static GGUIWndEquipDetailPageControl instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndEquipDetailPageControl();
                return _g_instance;
            }
        }

        //藏品信息
        private EquipInfo _m_equipInfo;
        //显示序列号
        private long _m_lShowSerialize;
        //页签列表
        private List<GGUIWndEquipDetailPageTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndEquipDetailPageTab _m_wSelectTabWnd;
        //佩戴的伙伴头像
        private NPGGuiWndTexture _m_wHeroIcon;
        //佩戴的伙伴头像背景
        private GGuiWndSprite _m_wHeroBg;
        //升级页面
        private GGUIWndEquipUpgradePage _m_wUpgradePage;
        //技能页面
        private GGUIWndEquipSkillPage _m_wSkillPage;
        //技能列表
        private GGUIWndEquipSkillContainer _m_wEquipSkillContainer;
        //是否已经获得技能列表
        private bool _m_bIsGetSkillList;
        //当前选中的技能下标
        private int _m_iCurSelectSkillIndex;


        public GGUIWndEquipDetailPageControl() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEquipDetailPageControl.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEquipDetailPageControl.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipBaseInfoChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_EQUIP_OPEN_SKILL_PAGE, _onSimulateClickEquipOpenSkillPage);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_iCurSelectSkillIndex = 0;
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipBaseInfoChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_EQUIP_OPEN_SKILL_PAGE, _onSimulateClickEquipOpenSkillPage);
            _m_wHeroIcon?.hideWnd();
            _m_wHeroBg?.hideWnd();
            _m_wEquipSkillContainer?.hideWnd();
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_iCurSelectSkillIndex = 0;

            _hideAllPage();
        }

        protected override void _onReset()
        { 
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndEquipDetailPageTab tempTabItem = null;
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

            _m_wHeroIcon?.discardTexture();
            _m_wHeroBg?.discardTexture();

            _m_wUpgradePage?.resetWnd();
            _m_wSkillPage?.resetWnd();
            _m_wEquipSkillContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSelectTabWnd = null;

            if (_m_lTabWndList != null)
            {
                GGUIWndEquipDetailPageTab tempTabItem = null;
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

            _m_wHeroIcon?.discard();
            _m_wHeroIcon = null;

            _m_wHeroBg?.discard();
            _m_wHeroBg = null;

            _m_wUpgradePage?.discard();
            _m_wUpgradePage = null;

            _m_wSkillPage?.discard();
            _m_wSkillPage = null;

            _m_wEquipSkillContainer?.discard();
            _m_wEquipSkillContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnHero, _onClickHero);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_wSelectTabWnd = null;

            _m_lTabWndList = new List<GGUIWndEquipDetailPageTab>();
            if (null != wnd.monoTabList)
            {
                GGUIEquipDetailPageTabMono tempTabMono = null;
                GGUIWndEquipDetailPageTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null)
                        continue;
                    tempTabItem = new GGUIWndEquipDetailPageTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onClickTab;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            if (wnd.imgHeroIcon != null)
                _m_wHeroIcon = new NPGGuiWndTexture(wnd.imgHeroIcon);

            if (wnd.imgHeroBg != null)
                _m_wHeroBg = new GGuiWndSprite(wnd.imgHeroBg);

            if (wnd.monoSkillContainer != null)
            {
                _m_wEquipSkillContainer = new GGUIWndEquipSkillContainer(wnd.monoSkillContainer);
                _m_wEquipSkillContainer.onSelectItemChg += _onClickSkillItem;
            }

            ALUGUICommon.combineBtnClick(wnd.btnHero, _onClickHero);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(EquipInfo _info)
        {
            if (_info == null)
                return;

            _m_equipInfo = _info;
            _m_iCurSelectSkillIndex = 0;
            _m_wEquipSkillContainer?.moveToLeft();

            //TODO 判断是否有该页签，目前只有两个页签不用处理

            if (_m_wSelectTabWnd != null)
            {
                //选中当前页签
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
            else
            {
                //设置默认选择
                foreach (GGUIWndEquipDetailPageTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabType == EEquipDetailTabType.UPGRADE)
                    {
                        _onTabSelect(itemTab);
                        break;
                    }
                }
            }

            //刷新佩戴伙伴信息
            _refreshHeroInfo();
            //刷新技能列表
            _refreshSkillContainer();
        }

        /// <summary>
        /// 根据技能位置打开技能详情页面
        /// </summary>
        /// <param name="_skillIndex"></param>
        public void setShowSkillPageBySkillIndex(int _skillIndex)
        {
            //重置选择
            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;
            //设置页签选择技能
            foreach (GGUIWndEquipDetailPageTab itemTab in _m_lTabWndList)
            {
                if (itemTab.tabType == EEquipDetailTabType.SKILL)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }
            //设置选中技能item
            _m_iCurSelectSkillIndex = _skillIndex;
            if (_m_bIsGetSkillList)
                _m_wEquipSkillContainer?.setSelectItemBySkillIndex(_skillIndex);
        }

        //设置默认选中升级页
        private void _setSelectDetailPage()
        {
            if (_m_lTabWndList == null)
                return;

            foreach (GGUIWndEquipDetailPageTab itemTab in _m_lTabWndList)
            {
                if (itemTab.tabType == EEquipDetailTabType.UPGRADE)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }
        }

        //刷新佩戴伙伴信息
        private void _refreshHeroInfo()
        {
            if (wnd == null || _m_equipInfo == null)
                return;

            bool haveHero = _m_equipInfo.wearHeroId > 0;

            ALUGUICommon.setGameObjEnable(wnd.goHaveHeroHideList, !haveHero);
            ALUGUICommon.setGameObjEnable(wnd.goHaveHeroShowList, haveHero);

            if (haveHero)
            {
                HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_equipInfo.wearHeroId);
                HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_equipInfo.wearHeroId);
                if (heroRef == null)
                    return;

                BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) heroRef.spec_attr_type);
                if (basicAttrRef == null)
                    return;

                //设置头像
                if (_m_wHeroIcon != null)
                {
                    _m_wHeroIcon.showWnd();
                    _m_wHeroIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, heroInfo != null ? heroInfo.curSkinId : heroRef.default_skin_id));
                }

                //设置背景
                if (_m_wHeroBg != null)
                {
                    _m_wHeroBg.showWnd();
                    _m_wHeroBg.setTexture(GCommon.getQualityExtRefObj(ENPItemType.HERO, heroRef.id)?.hero_head_bg);
                }

                //设置伙伴名称
                ALUGUICommon.setLabelTxt(wnd.txtHeroName, heroRef.transName);

                //设置加成值
                ALUGUICommon.setLabelTxt(wnd.txtAddValue, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num,_m_equipInfo.skillAddValue / 100f));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtHeroName, "--");
                ALUGUICommon.setLabelTxt(wnd.txtAddValue, "--");
            }
        }

        //刷新技能列表
        private void _refreshSkillContainer()
        {
            if (wnd == null || _m_equipInfo == null)
                return;

            //由于技能详情需要请求获取，列表默认先隐藏
            _m_wEquipSkillContainer?.hideWnd();

            List<EquipSkillInfo> equipSkillList = null;
            long serialize = _m_lShowSerialize;
            //技能详情需要请求获取，并且会在请求完后缓存下来
            _m_equipInfo.getEquipSkillInfoList(_skillList =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize || _skillList == null)
                    return;

                _m_bIsGetSkillList = true;
                equipSkillList = _skillList;
                equipSkillList.Sort((_a,_b)=>_a.index.CompareTo(_b.index));
                _m_wEquipSkillContainer?.showWnd();
                _m_wEquipSkillContainer?.showItemList(equipSkillList);

                if(_m_wSelectTabWnd == null || _m_wSelectTabWnd.tabType == EEquipDetailTabType.UPGRADE)
                    _m_wEquipSkillContainer?.resetSelect();
                else if(_m_wSelectTabWnd.tabType == EEquipDetailTabType.SKILL)
                    _m_wEquipSkillContainer?.setSelectItemBySkillIndex(_m_iCurSelectSkillIndex);
            });
        }

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndEquipDetailPageTab _tabItemWnd)
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
        private void _refreshTabView(EEquipDetailTabType _tabView)
        {
            _hideAllPage();
            switch (_tabView)
            {
                case EEquipDetailTabType.UPGRADE:
                    //重置技能选择
                    _m_wEquipSkillContainer?.resetSelect();
                    _showUpgradePage();
                    break;
                case EEquipDetailTabType.SKILL:
                    _showSkillPage();
                    break;
            }
        }

        //显示升级页面
        private void _showUpgradePage()
        {
            if (wnd == null || _m_equipInfo == null)
                return;

            if (_m_wUpgradePage != null)
            {
                _m_wUpgradePage.showWnd();
                _m_wUpgradePage.setInfo(_m_equipInfo);
            }
            else
            {
                _m_wUpgradePage = new GGUIWndEquipUpgradePage(_getPageAssetPathByType(EEquipDetailTabType.UPGRADE), wnd.pageParent);
                _m_wUpgradePage.load(() =>
                {
                    if (_m_wUpgradePage == null)
                        return;
                    _m_wUpgradePage.showWnd();
                    _m_wUpgradePage.setInfo(_m_equipInfo);
                });
            }
        }

        //显示技能页面
        private void _showSkillPage()
        {
            if (wnd == null || _m_equipInfo == null)
                return;

            if (_m_wSkillPage != null)
            {
                _m_wSkillPage.showWnd();
                _m_wSkillPage.setInfo(_m_equipInfo, _setSelectDetailPage, null);
            }
            else
            {
                _m_wSkillPage = new GGUIWndEquipSkillPage(_getPageAssetPathByType(EEquipDetailTabType.SKILL), wnd.pageParent);
                _m_wSkillPage.load(() =>
                {
                    if (_m_wSkillPage == null)
                        return;
                    _m_wSkillPage.showWnd();
                    _m_wSkillPage.setInfo(_m_equipInfo, _setSelectDetailPage, null);
                });
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wUpgradePage?.hideWnd();
            _m_wSkillPage?.hideWnd();
        }

        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(EEquipDetailTabType _type)
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

        // //播放替换特效
        // private void _onPlayItemSfx(bool _isReplaceSuc)
        // {
        //     if (_isReplaceSuc)
        //         _m_wEquipSkillContainer?.curSelectItemWnd?.playReplaceSfx();
        //     else
        //         _m_wEquipSkillContainer?.curSelectItemWnd?.playFailSfx();
        // }

        //点击技能item
        private void _onClickSkillItem(GGUIWndEquipSkillContainerItem _item)
        {
            if (_m_wSelectTabWnd == null || _item == null || _item.equipSkillInfo == null)
                return;

            switch (_m_wSelectTabWnd.tabType)
            {
                case EEquipDetailTabType.UPGRADE:
                    setShowSkillPageBySkillIndex(_item.equipSkillInfo.index);
                    break;
                case EEquipDetailTabType.SKILL:
                    _m_wSkillPage?.onClickSkillItem(_item);
                    break;
            }
        }

        #region 点击事件

        //点击页签
        private void _onClickTab(GGUIWndEquipDetailPageTab _tabItemWnd)
        {
            if (null == _tabItemWnd || _m_wSelectTabWnd == _tabItemWnd)
                return;

            _onTabSelect(_tabItemWnd);
            //如果是技能页签，重置选择
            if (_tabItemWnd != null && _tabItemWnd.tabType == EEquipDetailTabType.SKILL)
            {
                _m_iCurSelectSkillIndex = 0;
                _refreshSkillContainer();
            }
        }

        //点击伙伴
        private void _onClickHero(GameObject _go)
        {
            if (_m_equipInfo == null)
                return;

            bool haveHero = _m_equipInfo.wearHeroId > 0;
            if (haveHero)
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.equip_equipByHero_name, GCommon.getItemName(ENPItemType.HERO, _m_equipInfo.wearHeroId)));
            else
            {
                //当前无人佩戴，是否要前往伙伴界面进行操作？
                NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.equip_goToHeroDesc_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                    null,
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        QueueMgr.instance.AddNode(new GNodeBuilding());
                        QueueMgr.instance.AddNode(new GNodeHero());
                    });
            }
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EQUIP_DETAIL);
        }

        //模拟点击打开藏品技能重塑页面
        private void _onSimulateClickEquipOpenSkillPage()
        {
            if (_m_lTabWndList == null)
                return;

            foreach (GGUIWndEquipDetailPageTab itemTab in _m_lTabWndList)
            {
                if (itemTab.tabType == EEquipDetailTabType.SKILL)
                {
                    _onClickTab(itemTab);
                    break;
                }
            }
        }

        #endregion

        #region 消息事件

        //藏品基础信息变更
        private void _onEquipBaseInfoChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long dbId = (long)_objects[0];
            if (_m_equipInfo != null && _m_equipInfo.dbId == dbId)
                _refreshHeroInfo();
        }

        #endregion
    }
}