using System;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴详情页面
    /// </summary>
    public class GGUIWndHeroInfo : _ANPGGUIBasicResBarWnd<GGUIMonoHeroInfo>
    {
        private static GGUIWndHeroInfo _g_instance;
        public static GGUIWndHeroInfo instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroInfo();
                return _g_instance;
            }
        }

        //是否显示入场动画
        private bool _m_bShowEnterAni;
        //当前伙伴信息
        private HeroCardShowInfo _m_heroShowInfo;
        //已拥有伙伴列表
        [NotNull] private List<HeroCardShowInfo> _m_lOwnHeroInfoList = new List<HeroCardShowInfo>();
        //页签列表
        private List<GGUIWndHeroInfoTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndHeroInfoTab _m_wSelectTabWnd;
        //相性图标
        private NPGGuiWndTexture _m_wSpecAttrIcon;
        //觉醒星级
        private GGUIWndHeroCommonStar _m_wStar;
        //伙伴形象
        private NPGGUIWndCommonShowCase _m_commonShowcaseWnd;
        //伙伴配音气泡附加窗口
        private GGUIWndHeroVoiceBubble _m_wVoiceBubble;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;
        //当前展示的伙伴在列表里的下标
        private int _m_iCurShowIndex;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        //伙伴信息详情页
        private GGUIWndHeroInfoDetailPage _m_wDetailPage;
        //伙伴经营详情页
        private GGUIWndHeroInfoBusinessPage _m_wBusinessPage;
        //伙伴资质详情页
        private GGUIWndHeroInfoTalentPage _m_wTalentPage;
        //伙伴觉醒详情页
        private GGUIWndHeroInfoStarPage _m_wStarPage;
        //伙伴星辉详情页
        private GGUIWndHeroInfoHaloPage _m_wHaloPage;
        //伙伴加护详情页
        private GGUIWndHeroBlessPage _m_wBlessPage;
        //是否正在播放升级音效
        private bool _m_bIsPlayingLevelUpVoice;
        //是否正在播放资质、经营技能升级音效
        private bool _m_bIsPlayingSkillUpVoice;
        //是否正在播放觉醒音效
        private bool _m_bIsPlayingStarVoice;

        /// <summary>
        /// 当前伙伴信息
        /// </summary>
        public HeroCardShowInfo curHeroCardShowInfo { get { return _m_heroShowInfo; } }

        public GGUIWndHeroInfo() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        /// <summary>
        /// 当前展示的伙伴是否还可以升级
        /// </summary>
        public bool curShowHeroCanUpgrade
        {
            get
            {
                return isShow && 
                       _m_wDetailPage != null &&
                       _m_wDetailPage.isShow &&
                       _m_heroShowInfo != null &&
                       _m_heroShowInfo.heroInfo != null &&
                       !_m_heroShowInfo.heroInfo.isStepLvlLimit() &&
                       _m_heroShowInfo.heroInfo.nextHeroLvlRef != null;
            }
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);//伙伴实力变更
            WinMsg.RegisterMsg(WinMsgType.HERO_INFO_GUIDE_TAB, _onTabGuide);//伙伴信息界面引导点击页签
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_GET_NEW_BUSINESS_SKILL, _onGetNewBusinessSkill);//伙伴新增经营技能
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_STAR_CHG, _onHeroStarChg);//觉醒技能变更
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_LEVEL_CHG, _onHeroLevelChg);//伙伴等级变更
            WinMsg.RegisterMsg(WinMsgType.SWITCH_HERO_INFO_TAB, _onSwitchHeroInfoTab);//切换伙伴信息页签
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_BUSINESS_SKILL_CHG, _onBusinessSkillChg);//经营技能变更
            WinMsg.RegisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshTabRedTip);//刷新红点

            HeroVoiceMgr.instance.regAllVoicePlayDone(_onVoicePlayDone);

            _refreshAll();
            if(_m_bShowEnterAni) 
                _playHeroShowCaseAni(wnd?.heroEnterAniName);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);//伙伴实力变更
            WinMsg.UnregisterMsg(WinMsgType.HERO_INFO_GUIDE_TAB, _onTabGuide);//伙伴信息界面引导点击页签
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_GET_NEW_BUSINESS_SKILL, _onGetNewBusinessSkill);//伙伴新增经营技能
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_STAR_CHG, _onHeroStarChg);//觉醒技能变更
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_LEVEL_CHG, _onHeroLevelChg);//伙伴等级变更
            WinMsg.UnregisterMsg(WinMsgType.SWITCH_HERO_INFO_TAB, _onSwitchHeroInfoTab);//切换伙伴信息页签
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_BUSINESS_SKILL_CHG, _onBusinessSkillChg);//经营技能变更
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshTabRedTip);//刷新红点
            _hideAllPage();

            if (null != _m_commonShowcaseWnd)
                _m_commonShowcaseWnd.hideWnd();

            if (_m_wSpecAttrIcon != null)
                _m_wSpecAttrIcon.hideWnd();

            if(_m_wStar != null)
                _m_wStar.hideWnd();

            if(_m_wVoiceBubble != null)
                _m_wVoiceBubble.hideWnd();

            HeroVoiceMgr.instance.unRegAllVoicePlayDone(_onVoicePlayDone);
            HeroVoiceMgr.instance.stopAllVoice();

            _discardSfx();
            _pushBackQualityGo();
        }

        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndHeroInfoTab tempTabItem = null;
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

            if (_m_wStar != null)
                _m_wStar.resetWnd();

            if (_m_wVoiceBubble != null)
                _m_wVoiceBubble.resetWnd();

            if (_m_lOwnHeroInfoList != null)
                _m_lOwnHeroInfoList.Clear();
            
            if(_m_commonShowcaseWnd != null)
                _m_commonShowcaseWnd.resetWnd();

            if (_m_wSpecAttrIcon != null)
                _m_wSpecAttrIcon.discardTexture();

            if(_m_wDetailPage != null)
                _m_wDetailPage.resetWnd();

            if(_m_wBusinessPage != null)
                _m_wBusinessPage.resetWnd();

            if(_m_wTalentPage != null)
                _m_wTalentPage.resetWnd();

            if(_m_wStarPage != null)
                _m_wStarPage.resetWnd();

            if(_m_wHaloPage != null)
                _m_wHaloPage.resetWnd();

            if(_m_wBlessPage != null)
                _m_wBlessPage.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndHeroInfoTab tempTabItem = null;
                for(int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if(tempTabItem == null)
                        continue;
                    tempTabItem.discard();
                }
                _m_lTabWndList.Clear();
                _m_lTabWndList = null;
            }
            _m_wSelectTabWnd = null;

            if (_m_lOwnHeroInfoList != null)
                _m_lOwnHeroInfoList.Clear();

            if (_m_wStar != null)
                _m_wStar.discard();
            _m_wStar = null;

            if (_m_wVoiceBubble != null)
                _m_wVoiceBubble.discard();
            _m_wVoiceBubble = null;

            if (_m_commonShowcaseWnd != null)
                _m_commonShowcaseWnd.discard();
            _m_commonShowcaseWnd = null;

            if (_m_wSpecAttrIcon != null)
                _m_wSpecAttrIcon.discard();
            _m_wSpecAttrIcon = null;

            if(_m_wDetailPage != null)
                _m_wDetailPage.discard();
            _m_wDetailPage = null;

            if(_m_wBusinessPage != null)
                _m_wBusinessPage.discard();
            _m_wBusinessPage = null;

            if(_m_wTalentPage != null)
                _m_wTalentPage.discard();
            _m_wTalentPage = null;

            if(_m_wStarPage != null)
                _m_wStarPage.discard();
            _m_wStarPage = null;

            if(_m_wHaloPage != null)
                _m_wHaloPage.discard();
            _m_wHaloPage = null;

            if(_m_wBlessPage != null)
                _m_wBlessPage.discard();
            _m_wBlessPage = null;

            _discardSfx();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.uncombineBtnClick(wnd.btnLeft, _setLastHero);//点击切换上一个伙伴
            ALUGUICommon.uncombineBtnClick(wnd.btnRight, _setNextHero);//点击切换下一个伙伴
            ALUGUICommon.uncombineBtnClick(wnd.btnPreview, _onClickPreview);//点击仅展示形象按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnPreviewClose, _onClickPreviewClose);//点击关闭仅展示形象
            ALUGUICommon.uncombineBtnClick(wnd.btnSuitDetail, _onClickSuitDetail);//点击套系按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnIntroduction, _onClickIntroduction);//点击简介按钮
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndHeroInfoTab>();
            if(null != wnd.monoTabList)
            {
                GGUIHeroInfoTabMono tempTabMono = null;
                GGUIWndHeroInfoTab tempTabItem = null;
                for(int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if(tempTabMono == null)
                        continue;
                    tempTabItem = new GGUIWndHeroInfoTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }
            _m_wSelectTabWnd = null;

            if (wnd.monoStar != null)
                _m_wStar = new GGUIWndHeroCommonStar(wnd.monoStar);

            if (wnd.monoVoiceBubble != null)
            {
                _m_wVoiceBubble = new GGUIWndHeroVoiceBubble(wnd.monoVoiceBubble);
                _m_wVoiceBubble.onClick += _onVoicePlayDone;//点击气泡时重置状态
            }

            if(wnd.monoShowCaseWnd!= null)
            {
                _m_commonShowcaseWnd = new NPGGUIWndCommonShowCase(wnd.monoShowCaseWnd);
                _m_commonShowcaseWnd.onClickItem += _onClickShowcase;
            }

            if (wnd.imgSpecAttrIcon != null)
                _m_wSpecAttrIcon = new NPGGuiWndTexture(wnd.imgSpecAttrIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.combineBtnClick(wnd.btnLeft, _setLastHero);//点击切换上一个伙伴
            ALUGUICommon.combineBtnClick(wnd.btnRight, _setNextHero);//点击切换下一个伙伴
            ALUGUICommon.combineBtnClick(wnd.btnPreview, _onClickPreview);//点击仅展示形象按钮
            ALUGUICommon.combineBtnClick(wnd.btnPreviewClose, _onClickPreviewClose);//点击关闭仅展示形象
            ALUGUICommon.combineBtnClick(wnd.btnSuitDetail, _onClickSuitDetail);//点击套系按钮
            ALUGUICommon.combineBtnClick(wnd.btnIntroduction, _onClickIntroduction);//点击简介按钮
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroShowInfo"></param>
        /// <param name="_heroShowInfoList"></param>
        public void setInfo(HeroCardShowInfo _heroShowInfo, List<HeroCardShowInfo> _heroShowInfoList, bool _showEnterAni)
        {
            if (null == _heroShowInfo)
                return;

            _m_bShowEnterAni = _showEnterAni;
            _m_heroShowInfo = _heroShowInfo;
            //筛选已获得的列表
            _m_lOwnHeroInfoList.Clear();
            if (_heroShowInfoList != null)
            {
                for (int i = 0; i < _heroShowInfoList.Count; i++)
                {
                    if(_heroShowInfoList[i] != null && _heroShowInfoList[i].isUnlock)
                        _m_lOwnHeroInfoList.Add(_heroShowInfoList[i]);
                }
            }
            _m_iCurShowIndex = _m_lOwnHeroInfoList.IndexOf(_heroShowInfo);
        }

        /// <summary>
        /// 刷新所有
        /// </summary>
        private void _refreshAll()
        {
            //刷新页签
            _refreshPageWnd();
            //刷新当前窗口
            _refreshInfo();
            //刷新页签红点
            _refreshTabRedTip();
        }

        
        /// <summary>
        /// 刷新页签窗口
        /// </summary>
        private void _refreshPageWnd()
        {
            if (null == wnd || null == _m_heroShowInfo || null == _m_heroShowInfo.heroInfo)
                return;

            bool haveStarSkill = _m_heroShowInfo.heroInfo.haveStarSkill;
            bool haveHalo = _m_heroShowInfo.heroInfo.haveHalo;
            bool haveBless = _m_heroShowInfo.heroRefObj.relationConsortIdList.Count > 0;
            GGUIWndHeroInfoTab tempTabItem = null;
            for (int i = 0; i < _m_lTabWndList.Count; ++i)
            {
                tempTabItem = _m_lTabWndList[i];
                if (tempTabItem == null)
                    continue;
                tempTabItem.showWnd();

                //检查是否有觉醒技能，否则隐藏页签
                if(tempTabItem.tabType == EHeroInfoTabType.STAR && !haveStarSkill)
                    tempTabItem.hideWnd();
                //检查是否有星辉，否则隐藏页签
                if(tempTabItem.tabType == EHeroInfoTabType.HALO && !haveHalo)
                    tempTabItem.hideWnd();
                //检查是否有加护，否则隐藏页签
                if(tempTabItem.tabType == EHeroInfoTabType.BLESS && !haveBless)
                    tempTabItem.hideWnd();
            }

            //如果当前未选中，或者页签未解锁，使用策划配置的默认页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGUIWndHeroInfoTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabType == wnd.tabDefaultShow)
                    {
                        _onTabSelect(itemTab);
                        break;
                    }
                }
            }
            else
            {
                //如果没有这些页签，则默认切换回详情升级页面
                if ((_m_wSelectTabWnd.tabType == EHeroInfoTabType.STAR && !haveStarSkill) ||
                    (_m_wSelectTabWnd.tabType == EHeroInfoTabType.HALO && !haveHalo) ||
                    (_m_wSelectTabWnd.tabType == EHeroInfoTabType.BLESS && !haveBless))
                {
                    _setSelectDetailPage();
                }
                else
                {
                    _m_wSelectTabWnd.setSelected(true);
                    //根据页签刷新列表内容
                    _refreshTabView(_m_wSelectTabWnd.tabType);
                }
            }
        }

        /// <summary>
        /// 刷新当前窗口
        /// </summary>
        private void _refreshInfo()
        {
            if (_m_heroShowInfo == null || _m_heroShowInfo.heroInfo == null || _m_heroShowInfo.heroRefObj == null || wnd == null)
                return;

            //伙伴名字
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.HERO, _m_heroShowInfo.id));
            //伙伴称号（皮肤名）
            ALUGUICommon.setLabelTxt(wnd.txtSkinName, GCommon.getItemName(ENPItemType.HERO_SKIN, _m_heroShowInfo.heroInfo.curSkinId));
            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }
            //相性图标
            BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) _m_heroShowInfo.heroRefObj.spec_attr_type);
            if (_m_wSpecAttrIcon != null)
            {
                _m_wSpecAttrIcon.showWnd();
                _m_wSpecAttrIcon.setTexture(basicAttrRef?.icon);
            }
            //相性名称
            ALUGUICommon.setLabelTxt(wnd.txtSpecAttrName, TextTranslate.instance.getLanguage(basicAttrRef?.name));

            //套系数量
            long suitId = _m_heroShowInfo.heroInfo.belongSuitId;
            HeroSuitInfo suitInfo = NPPlayer.instance.heroComponent.getHeroSuitInfo(suitId);
            if (suitInfo != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoSuitHideList, true);
                long curCount = suitInfo.suitOwnHeroCount;
                long totalCount = suitInfo.suitTotalHeroCount;
                ALUGUICommon.setLabelTxt(wnd.txtSuitCount, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, curCount, totalCount));
            }
            else
                ALUGUICommon.setGameObjEnable(wnd.goNoSuitHideList, false);

            //展示形象
            if (_m_commonShowcaseWnd != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[4];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_heroShowInfo.getTdShow()), 0);
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_heroShowInfo.getTdBg()), 3);
                _m_commonShowcaseWnd.showWnd(showCaseUnitInfoObjList);
                if(_m_bShowEnterAni)
                    _playHeroShowCaseAni(wnd?.heroEnterAniName);
            }

            //刷新觉醒星级
            _refreshStar();
            //刷新配音气泡
            _refreshVoiceBubble();

            //设置左右切换按钮显隐
            bool needShowSwitchBtn = _m_lOwnHeroInfoList.Count > 1;
            ALUGUICommon.setGameObjEnable(wnd.btnLeft, needShowSwitchBtn);
            ALUGUICommon.setGameObjEnable(wnd.btnRight, needShowSwitchBtn);

            //设置新获得红点已读
            NPPlayer.instance.heroComponent.setReadRedTip(RedTipConst.RED_HERO_FIRST_GET, _m_heroShowInfo.id);
        }

        //刷新页签红点
        private void _refreshTabRedTip()
        {
            if (_m_lTabWndList == null || _m_heroShowInfo == null)
                return;

            for (int i = 0; i < _m_lTabWndList.Count; i++)
            {
                bool needShow = false;
                switch (_m_lTabWndList[i].tabType)
                {
                    case EHeroInfoTabType.INFO:
                        needShow = NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_LEVEL_UP, _m_heroShowInfo.id) ||
                                   NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_STEP_UP, _m_heroShowInfo.id);
                        break;
                    case EHeroInfoTabType.BUSINESS:
                        needShow = NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_BUSINESS_UPGRADE, _m_heroShowInfo.id);
                        break;
                    case EHeroInfoTabType.TALENT:
                        needShow = NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_TALENT_UPGRADE, _m_heroShowInfo.id);
                        break;
                    case EHeroInfoTabType.STAR:
                        needShow = NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_STAR_UPGRADE, _m_heroShowInfo.id);
                        break;
                    case EHeroInfoTabType.HALO:
                        needShow = NPPlayer.instance.heroComponent.needShowRedTip(RedTipConst.RED_HERO_HALO_UPGRADE, _m_heroShowInfo.id);
                        break;
                }

                _m_lTabWndList[i].showRedTipNum(needShow ? 1 : 0);
            }
        }

        //刷新配音气泡
        private void _refreshVoiceBubble()
        {
            if (_m_heroShowInfo == null)
                return;

            //设置配音气泡
            if (_m_wVoiceBubble != null)
            {
                _m_wVoiceBubble.showWnd();
                _m_wVoiceBubble.setInfo(_m_heroShowInfo.id, EHeroVoiceType.VIEW);
            }
        }

        //播放伙伴形象动画
        private void _playHeroShowCaseAni(string _aniName)
        {
            if (_m_commonShowcaseWnd == null || string.IsNullOrEmpty(_aniName))
                return;

            _m_commonShowcaseWnd.regInitDoneDelegate(() =>
            {
                _m_commonShowcaseWnd.playAnim(0, _aniName);
            });
        }

        //刷新觉醒星级
        private void _refreshStar()
        {
            //觉醒星级
            if (_m_wStar != null)
            {
                if (_m_heroShowInfo.heroInfo.haveStarSkill)
                {
                    _m_wStar.showWnd();
                    _m_wStar.setInfo(_m_heroShowInfo.heroInfo.star);
                }
                else
                {
                    _m_wStar.hideWnd();
                }
            }
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_heroShowInfo == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_heroShowInfo.id);
            if (qualityExtRef != null && qualityExtRef.quality_go_index != null)
            {
                _m_qualityGoIndex = qualityExtRef.quality_go_index;
                GGoIndexCacheMgr.instance.popItem(_m_qualityGoIndex, _go =>
                {
                    if (_go == null || wnd == null || wnd.goQualityIconParent == null)
                        return;

                    _go.transform.SetParent(wnd.goQualityIconParent);
                    _go.transform.localPosition = Vector3.zero;
                    _go.transform.localScale = Vector3.one;
                    _m_qualityGo = _go;
                });
            }
        }

        //回收品质GO
        private void _pushBackQualityGo()
        {
            if (_m_qualityGoIndex != null && _m_qualityGo != null)
                GGoIndexCacheMgr.instance.pushbackItem(_m_qualityGoIndex, _m_qualityGo);
            _m_qualityGoIndex = null;
            _m_qualityGo = null;
        }

        //消耗特效
        private void _discardSfx()
        {
            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }

        #region 配音

        /// <summary>
        /// 播放配音
        /// </summary>
        private bool _playHeroVoice(EHeroVoiceType _voiceType, bool _playRandom = true, bool _playBaseOnPre = true)
        {
            if (_m_heroShowInfo == null || _m_heroShowInfo.heroRefObj == null)
                return false;

            //当前有音效播放时都设置为false
            _m_bIsPlayingLevelUpVoice = false;
            _m_bIsPlayingSkillUpVoice = false;
            _m_bIsPlayingStarVoice = false;

            //升级和资质、经营技能升级播放气泡和配音
            if ((_voiceType == EHeroVoiceType.LEVEL_UP || _voiceType == EHeroVoiceType.SKILL_UP) && _m_wVoiceBubble != null)
                return _m_wVoiceBubble.forcePlayVoiceAndBubble(_m_heroShowInfo.heroRefObj.id, _voiceType);
            else
                return HeroVoiceMgr.instance.playVoice(_m_heroShowInfo.heroRefObj.id, _voiceType, _playRandom, _playBaseOnPre);
        }

        /// <summary>
        /// 播放升级配音
        /// </summary>
        private void _playLevelUpVoice()
        {
            if (_m_bIsPlayingLevelUpVoice)
                return;

            _m_bIsPlayingLevelUpVoice = _playHeroVoice(EHeroVoiceType.LEVEL_UP);
        }

        /// <summary>
        /// 播放资质、经营技能升级配音
        /// </summary>
        private void _playSkillUpVoice()
        {
            if (_m_bIsPlayingSkillUpVoice)
                return;

            _m_bIsPlayingSkillUpVoice = _playHeroVoice(EHeroVoiceType.SKILL_UP);
        }

        /// <summary>
        /// 播放觉醒配音
        /// </summary>
        private void _playStarVoice()
        {
            if (_m_bIsPlayingStarVoice)
                return;

            _m_bIsPlayingStarVoice = _playHeroVoice(EHeroVoiceType.ADVANCE);
        }

        /// <summary>
        /// 当音效播放完成
        /// </summary>
        private void _onVoicePlayDone()
        {
            _m_bIsPlayingLevelUpVoice = false;
            _m_bIsPlayingSkillUpVoice = false;
            _m_bIsPlayingStarVoice = false;
        }

        #endregion

        #region 页签页面处理

        //选中加护页面
        public void setSelectBlessPage()
        {
            if (_m_lTabWndList == null)
                return;

            foreach (GGUIWndHeroInfoTab itemTab in _m_lTabWndList)
            {
                if (itemTab.tabType == EHeroInfoTabType.BLESS)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }
        }

        //设置默认选中升级信息页
        private void _setSelectDetailPage()
        {
            if (_m_lTabWndList == null)
                return;

            foreach (GGUIWndHeroInfoTab itemTab in _m_lTabWndList)
            {
                if (itemTab.tabType == EHeroInfoTabType.INFO)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }
        }

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndHeroInfoTab _tabItemWnd)
        {
            if(null == _tabItemWnd || _m_wSelectTabWnd == _tabItemWnd)
                return;

            //取消原来的选择
            if (null != _m_wSelectTabWnd)
                _m_wSelectTabWnd.setSelected(false);

            //设置新对象
            _m_wSelectTabWnd = _tabItemWnd;

            if(null != _m_wSelectTabWnd)
            {
                _m_wSelectTabWnd.setSelected(true);

                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        //根据页签刷新列表内容
        private void _refreshTabView(EHeroInfoTabType _tabView)
        {
            _hideAllPage();
            
            switch(_tabView)
            {
                case EHeroInfoTabType.INFO://信息
                    _showDetailPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_HERO_INFO);
                    break;
                case EHeroInfoTabType.BUSINESS://经营
                    _showBusinessPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_HERO_INFO_BUSINESS_PAGE);
                    break;
                case EHeroInfoTabType.TALENT://资质
                    _showTalentPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_HERO_INFO_TALENT_PAGE);
                    break;
                case EHeroInfoTabType.STAR://觉醒
                    _showStarPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_HERO_INFO_STAR_PAGE);
                    break;
                case EHeroInfoTabType.HALO://星辉
                    _showHaloPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_HERO_INFO_HALO_PAGE);
                    break;
                case EHeroInfoTabType.BLESS://加护
                    _showBlessPage();
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_HERO_INFO_BLESS_PAGE);
                    break;
            }
        }
        
        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wDetailPage?.hideWnd();
            _m_wBusinessPage?.hideWnd();
            _m_wTalentPage?.hideWnd();
            _m_wStarPage?.hideWnd();
            _m_wHaloPage?.hideWnd();
            _m_wBlessPage?.hideWnd();
        }
        
        //显示详情页面
        private void _showDetailPage()
        {
            if(wnd == null || _m_heroShowInfo == null)
                return;

            if(_m_wDetailPage != null)
            {
                _m_wDetailPage.showWnd();
                _m_wDetailPage.setInfo(_m_heroShowInfo.heroInfo);
            }
            else
            {
                _m_wDetailPage = new GGUIWndHeroInfoDetailPage(_getPageAssetPathByType(EHeroInfoTabType.INFO), wnd.pageParent);
                _m_wDetailPage.load(() =>
                {
                    if(_m_wDetailPage == null)
                        return;

                    _m_wDetailPage.showWnd();
                    _m_wDetailPage.setInfo(_m_heroShowInfo.heroInfo);
                });
            }
        }

        //显示经营页面
        private void _showBusinessPage()
        {
            if (wnd == null || _m_heroShowInfo == null)
                return;

            if (_m_wBusinessPage != null)
            {
                _m_wBusinessPage.showWnd();
                _m_wBusinessPage.setInfo(_m_heroShowInfo.heroInfo, _setSelectDetailPage);
            }
            else
            {
                _m_wBusinessPage = new GGUIWndHeroInfoBusinessPage(_getPageAssetPathByType(EHeroInfoTabType.BUSINESS), wnd.pageParent);
                _m_wBusinessPage.load(() =>
                {
                    if (_m_wBusinessPage == null)
                        return;
                    _m_wBusinessPage.showWnd();
                    _m_wBusinessPage.setInfo(_m_heroShowInfo.heroInfo, _setSelectDetailPage);
                });
            }
        }
        
        //显示资质页面
        private void _showTalentPage()
        {
            if(wnd == null || _m_heroShowInfo == null)
                return;

            if(_m_wTalentPage != null)
            {
                _m_wTalentPage.showWnd();
                _m_wTalentPage.setInfo(_m_heroShowInfo.heroInfo, _setSelectDetailPage, _playSkillUpVoice);
            }
            else
            {
                _m_wTalentPage = new GGUIWndHeroInfoTalentPage(_getPageAssetPathByType(EHeroInfoTabType.TALENT), wnd.pageParent);
                _m_wTalentPage.load(() =>
                {
                    if(_m_wTalentPage == null)
                        return;
                    _m_wTalentPage.showWnd();
                    _m_wTalentPage.setInfo(_m_heroShowInfo.heroInfo, _setSelectDetailPage, _playSkillUpVoice);
                });
            }
        }
        
        //显示觉醒页面
        private void _showStarPage()
        {
            if(wnd == null || _m_heroShowInfo == null)
                return;

            if (_m_wStarPage != null)
            {
                _m_wStarPage.showWnd();
                _m_wStarPage.setInfo(_m_heroShowInfo.heroInfo, _setSelectDetailPage);
            }
            else
            {
                _m_wStarPage = new GGUIWndHeroInfoStarPage(_getPageAssetPathByType(EHeroInfoTabType.STAR), wnd.pageParent);
                _m_wStarPage.load(() =>
                {
                    if (_m_wStarPage == null)
                        return;
                    _m_wStarPage.showWnd();
                    _m_wStarPage.setInfo(_m_heroShowInfo.heroInfo, _setSelectDetailPage);
                });
            }
        }

        //显示星辉页面
        private void _showHaloPage()
        {
            if (wnd == null || _m_heroShowInfo == null)
                return;

            if (_m_wHaloPage != null)
            {
                _m_wHaloPage.showWnd();
                _m_wHaloPage.setInfo(_m_heroShowInfo.heroInfo, _setSelectDetailPage, _playStarVoice);
            }
            else
            {
                _m_wHaloPage = new GGUIWndHeroInfoHaloPage(_getPageAssetPathByType(EHeroInfoTabType.HALO), wnd.pageParent);
                _m_wHaloPage.load(() =>
                {
                    if (_m_wHaloPage == null)
                        return;
                    _m_wHaloPage.showWnd();
                    _m_wHaloPage.setInfo(_m_heroShowInfo.heroInfo, _setSelectDetailPage, _playStarVoice);
                });
            }
        }

        //显示加护页面
        private void _showBlessPage()
        {
            if (wnd == null || _m_heroShowInfo == null)
                return;

            if (_m_wBlessPage != null)
            {
                _m_wBlessPage.showWnd();
                _m_wBlessPage.setInfo(_m_heroShowInfo.heroRefObj, _setSelectDetailPage);
            }
            else
            {
                _m_wBlessPage = new GGUIWndHeroBlessPage(_getPageAssetPathByType(EHeroInfoTabType.BLESS), wnd.pageParent);
                _m_wBlessPage.load(() =>
                {
                    if (_m_wBlessPage == null)
                        return;
                    _m_wBlessPage.showWnd();
                    _m_wBlessPage.setInfo(_m_heroShowInfo.heroRefObj, _setSelectDetailPage);
                });
            }
        }

        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(EHeroInfoTabType _type)
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

        #region 点击事件

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_INFO);
        }

        //切换列表上一个伙伴
        private void _setLastHero(GameObject _gameObject)
        {
            if (_m_lOwnHeroInfoList == null)
                return;

            _m_iCurShowIndex--;
            if (_m_iCurShowIndex < 0)
                _m_iCurShowIndex = _m_lOwnHeroInfoList.Count - 1;

            _m_heroShowInfo = _m_lOwnHeroInfoList[_m_iCurShowIndex];
            _discardSfx();
            _refreshAll();
            _playHeroShowCaseAni(wnd?.heroIdleAniName);
            //刷新custom mono
            WinMsg.SendMsg(WinMsgType.CUSTOM_RELOAD);
        }

        //切换列表下一个伙伴
        private void _setNextHero(GameObject _gameObject)
        {
            if (_m_lOwnHeroInfoList == null)
                return;

            _m_iCurShowIndex++;
            if (_m_iCurShowIndex >= _m_lOwnHeroInfoList.Count)
                _m_iCurShowIndex = 0;

            _m_heroShowInfo = _m_lOwnHeroInfoList[_m_iCurShowIndex];
            _discardSfx();
            _refreshAll();
            _playHeroShowCaseAni(wnd?.heroIdleAniName);
            //刷新custom mono
            WinMsg.SendMsg(WinMsgType.CUSTOM_RELOAD);
        }

        //点击仅展示形象按钮
        private void _onClickPreview(GameObject _go)
        {
            if (wnd != null && wnd.previewAni != null)
                wnd.previewAni.forcePlay(EHeroPreviewAniType.START_PREVIEW);
        }

        //点击关闭仅展示形象按钮
        private void _onClickPreviewClose(GameObject _go)
        {
            if (wnd != null && wnd.previewAni != null)
                wnd.previewAni.forcePlay(EHeroPreviewAniType.CLOSE_PREVIEW);
        }

        //点击套系详情按钮
        private void _onClickSuitDetail(GameObject _go)
        {
            if (_m_heroShowInfo == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroSuitAndPower.instance, () =>
            {
                GGUIWndHeroSuitAndPower.instance.showWnd();
                GGUIWndHeroSuitAndPower.instance.setInfo(_m_heroShowInfo.heroInfo, EHeroSuitAndPowerTabType.SUIT);
            }, UINodeTagConst.C_HERO_SUIT_AND_POWER);
        }

        //点击简介按钮
        private void _onClickIntroduction(GameObject _go)
        {
            if (_m_heroShowInfo == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroInfoIntroduction.instance, () =>
            {
                GGUIWndHeroInfoIntroduction.instance.showWnd();
                GGUIWndHeroInfoIntroduction.instance.setInfo(_m_heroShowInfo.id);
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_HERO_INTRODUCTION, false, false);
        }

        //点击形象
        private void _onClickShowcase(ShowcaseInfo _showcaseInfo)
        {
            if(null == wnd || null == wnd.clickRandomAniNameList || wnd.clickRandomAniNameList.Count == 0)
                return;
            
            if(null == _showcaseInfo)
                return;
            
            _showcaseInfo.playAnim(0, wnd.clickRandomAniNameList.GetRandomItem());
        }

        #endregion

        #region 消息事件

        //伙伴实力变化
        private void _onHeroPowerChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3 || wnd == null)
                return;

            HeroInfo heroInfo = _objects[0] as HeroInfo;
            long oldPower = (long) _objects[1];
            long newPower = (long) _objects[2];
            if (heroInfo != null && _m_heroShowInfo != null && heroInfo.id == _m_heroShowInfo.id && newPower > oldPower && wnd.powerCenterTipId > 0)
            {
                long value = newPower - oldPower;
                NPGUIAddSceneCenterTip.instance.showTextTip(value.ToString(), wnd.powerCenterTipId);
            }
        }

        //通知页签引导
        private void _onTabGuide(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] == null)
                return;

            EHeroInfoTabType tabType = (EHeroInfoTabType) _objects[0];

            if (wnd != null && wnd.monoTabList != null)
            {
                for (int i = 0; i < wnd.monoTabList.Count; i++)
                {
                    if (wnd.monoTabList[i] != null && wnd.monoTabList[i].tabType == tabType)
                    {
                        //显示指引
                        ALUGUICommon.setGameObjEnable(wnd.monoTabList[i].goGuide, false);
                        ALUGUICommon.setGameObjEnable(wnd.monoTabList[i].goGuide, true);
                        break;
                    }
                }
            }
        }

        //新增经营技能
        private void _onGetNewBusinessSkill(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2 || _m_heroShowInfo == null || _m_heroShowInfo.heroRefObj == null)
                return;

            long heroId = (long) _objects[0];
            long businessSkillId = (long) _objects[1];

            if (heroId != _m_heroShowInfo.heroRefObj.id)
                return;

            List<WCGPairInt> businessSkillList = _m_heroShowInfo.heroRefObj.extra_business_skill_id_list;
            if (businessSkillList != null)
            {
                for (int i = 0; i < businessSkillList.Count; i++)
                {
                    if (businessSkillId == businessSkillList[i].second())
                    {
                        //弹出解锁提示弹窗
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroBusinessSkillUnlock.instance, () =>
                        {
                            GGUIWndHeroBusinessSkillUnlock.instance.showWnd();
                            GGUIWndHeroBusinessSkillUnlock.instance.setInfo(heroId, businessSkillId);
                        }, UINodeTagConst.C_HERO_BUSINESS_SKILL_UNLOCK);
                        break;
                    }
                }
            }
        }

        //觉醒技能变更
        private void _onHeroStarChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_heroShowInfo == null)
                return;

            long heroId = (long)_objects[0];
            long oriStar = (long)_objects[1];
            long curStar = (long)_objects[2];
            if (_m_heroShowInfo != null && _m_heroShowInfo.id == heroId)
            {
                _refreshStar();

                if (oriStar != curStar)
                {
                    //播放星星显示动画
                    _m_wStar?.playStarAniByStar(curStar);
                    //播放觉醒配音
                    _playStarVoice();
                }
            }
        }

        //伙伴等级变更
        private void _onHeroLevelChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3 || _m_heroShowInfo == null)
                return;

            HeroInfo heroInfo = _objects[0] as HeroInfo;
            long oriLevel = (long) _objects[1];
            long newLevel = (long) _objects[2];
            if (_m_heroShowInfo != null && heroInfo != null && _m_heroShowInfo.id == heroInfo.id)
            {
                //播放特效
                if (_m_lSfxObjList == null)
                    _m_lSfxObjList = new List<CommonUISfxObj>();

                if (wnd != null && wnd.upgradeSfxParent != null)
                {
                    CommonUISfxObj sfxObj = null;
                    if (newLevel - oriLevel == 1 && wnd.singleUpgradeSfxId > 0)//单次升级成功特效
                        sfxObj = PlaySfxMgr.instance.playUISfx(wnd.singleUpgradeSfxId, wnd.upgradeSfxParent);
                    else if (newLevel - oriLevel > 1 && wnd.tenUpgradeSfxId > 0)//十连升级成功特效
                        sfxObj = PlaySfxMgr.instance.playUISfx(wnd.tenUpgradeSfxId, wnd.upgradeSfxParent);

                    if(sfxObj != null)
                        _m_lSfxObjList.Add(sfxObj);
                }

                //播放升级配音
                _playLevelUpVoice();
            }
        }

        //切换伙伴信息页签
        private void _onSwitchHeroInfoTab(params object[] _objs)
        {
            if (_objs == null || _objs.Length == 0)
                return;

            string tabStr = (string) _objs[0];
            if (string.IsNullOrEmpty(tabStr))
                return;

            ALCommon.TryEnumParse(typeof(EHeroInfoTabType), tabStr,out EHeroInfoTabType targetTabType);

            if (null == _m_heroShowInfo || null == _m_heroShowInfo.heroInfo)
                return;

            bool haveStarSkill = _m_heroShowInfo.heroInfo.haveStarSkill;
            bool haveHalo = _m_heroShowInfo.heroInfo.haveHalo;
            bool haveBless = _m_heroShowInfo.heroRefObj.relationConsortIdList.Count > 0;

            //如果没有这些页签，则不处理
            if ((targetTabType == EHeroInfoTabType.STAR && !haveStarSkill) ||
                (targetTabType == EHeroInfoTabType.HALO && !haveHalo) ||
                (targetTabType == EHeroInfoTabType.BLESS && !haveBless))
                return;

            foreach (GGUIWndHeroInfoTab itemTab in _m_lTabWndList)
            {
                if (itemTab?.tabType == targetTabType)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }
        }

        //经营技能变更
        private void _onBusinessSkillChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length == 0 || _m_heroShowInfo == null)
                return;

            long heroId = (long) _objs[0];
            if (heroId == _m_heroShowInfo.id)
                _playSkillUpVoice();
        }

        #endregion
    }
}