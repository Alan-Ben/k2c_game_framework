using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴未解锁详情页面
    /// </summary>
    public class GGUIWndHeroLockInfo : _ANPGGUIBasicResBarWnd<GGUIMonoHeroLockInfo>
    {
        private static GGUIWndHeroLockInfo _g_instance;
        public static GGUIWndHeroLockInfo instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroLockInfo();
                return _g_instance;
            }
        }

        //当前伙伴信息
        private HeroCardShowInfo _m_heroShowInfo;
        //未解锁伙伴列表
        [NotNull] private List<HeroCardShowInfo> _m_lLockHeroInfoList = new List<HeroCardShowInfo>();
        //页签列表
        private List<GGUIWndHeroLockInfoTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndHeroLockInfoTab _m_wSelectTabWnd;
        //相性图标
        private NPGGuiWndTexture _m_wSpecAttrIcon;
        //伙伴形象
        private NPGGUIWndCommonShowCase _m_commonShowcaseWnd;
        //当前展示的伙伴在列表里的下标
        private int _m_iCurShowIndex;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        //伙伴简介页面
        private GGUIWndHeroInfoIntroductionPage _m_wIntroductionPage;
        //伙伴技能界页面
        private GGUIWndHeroLockInfoSkillPage _m_wSkillPage;
        //伙伴配音气泡附加窗口
        private GGUIWndHeroVoiceBubble _m_wVoiceBubble;

        /// <summary>
        /// 当前伙伴信息
        /// </summary>
        public HeroCardShowInfo curHeroCardShowInfo { get { return _m_heroShowInfo; } }

        public GGUIWndHeroLockInfo() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroLockInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroLockInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshAll();
        }

        protected override void _onHideWnd()
        {
            _hideAllPage();

            if (null != _m_commonShowcaseWnd)
                _m_commonShowcaseWnd.hideWnd();

            if (_m_wSpecAttrIcon != null)
                _m_wSpecAttrIcon.hideWnd();

            if (_m_wVoiceBubble != null)
                _m_wVoiceBubble.hideWnd();

            _pushBackQualityGo();
            HeroVoiceMgr.instance.stopAllVoice();
        }

        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndHeroLockInfoTab tempTabItem = null;
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
            
            if (_m_lLockHeroInfoList != null)
                _m_lLockHeroInfoList.Clear();
            
            if(_m_commonShowcaseWnd != null)
                _m_commonShowcaseWnd.resetWnd();

            if (_m_wSpecAttrIcon != null)
                _m_wSpecAttrIcon.discardTexture();

            if(_m_wIntroductionPage != null)
                _m_wIntroductionPage.resetWnd();

            if(_m_wSkillPage != null)
                _m_wSkillPage.resetWnd();

            if(_m_wVoiceBubble != null)
                _m_wVoiceBubble.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndHeroLockInfoTab tempTabItem = null;
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

            if (_m_lLockHeroInfoList != null)
                _m_lLockHeroInfoList.Clear();
            
            if(_m_commonShowcaseWnd != null)
                _m_commonShowcaseWnd.discard();
            _m_commonShowcaseWnd = null;

            if (_m_wSpecAttrIcon != null)
                _m_wSpecAttrIcon.discard();
            _m_wSpecAttrIcon = null;

            if(_m_wIntroductionPage != null)
                _m_wIntroductionPage.discard();
            _m_wIntroductionPage = null;

            if(_m_wSkillPage != null)
                _m_wSkillPage.discard();
            _m_wSkillPage = null;

            if(_m_wVoiceBubble != null)
                _m_wVoiceBubble.discard();
            _m_wVoiceBubble = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnLeft, _setLastHero);//点击切换上一个伙伴
            ALUGUICommon.uncombineBtnClick(wnd.btnRight, _setNextHero);//点击切换下一个伙伴
            ALUGUICommon.uncombineBtnClick(wnd.btnAccess, _onClickAccess);//点击获取途径
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);//点击关闭按钮
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndHeroLockInfoTab>();
            if(null != wnd.monoTabList)
            {
                GGUIHeroLockInfoTabMono tempTabMono = null;
                GGUIWndHeroLockInfoTab tempTabItem = null;
                for(int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if(tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndHeroLockInfoTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }
            _m_wSelectTabWnd = null;

            if(wnd.monoShowCaseWnd!= null)
                _m_commonShowcaseWnd = new NPGGUIWndCommonShowCase(wnd.monoShowCaseWnd);

            if (wnd.imgSpecAttrIcon != null)
                _m_wSpecAttrIcon = new NPGGuiWndTexture(wnd.imgSpecAttrIcon);

            if (wnd.monoVoiceBubble != null)
                _m_wVoiceBubble = new GGUIWndHeroVoiceBubble(wnd.monoVoiceBubble);

            ALUGUICommon.combineBtnClick(wnd.btnLeft, _setLastHero);//点击切换上一个伙伴
            ALUGUICommon.combineBtnClick(wnd.btnRight, _setNextHero);//点击切换下一个伙伴
            ALUGUICommon.combineBtnClick(wnd.btnAccess, _onClickAccess);//点击获取途径
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);//点击关闭按钮
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroShowInfo"></param>
        /// <param name="_heroShowInfoList"></param>
        public void setInfo(HeroCardShowInfo _heroShowInfo, List<HeroCardShowInfo> _heroShowInfoList)
        {
            if (null == _heroShowInfo)
                return;

            _m_heroShowInfo = _heroShowInfo;
            //筛选未获得的列表
            _m_lLockHeroInfoList.Clear();
            if (_heroShowInfoList != null)
            {
                for (int i = 0; i < _heroShowInfoList.Count; i++)
                {
                    if(_heroShowInfoList[i] != null && !_heroShowInfoList[i].isUnlock)
                        _m_lLockHeroInfoList.Add(_heroShowInfoList[i]);
                }
            }
            _m_iCurShowIndex = _m_lLockHeroInfoList.IndexOf(_heroShowInfo);
            _refreshAll();
            //播放入场配音
            _refreshVoiceBubble();
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
        }

        /// <summary>
        /// 刷新页签窗口
        /// </summary>
        private void _refreshPageWnd()
        {
            if (null == wnd || null == _m_heroShowInfo)
                return;

            //默认页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGUIWndHeroLockInfoTab itemTab in _m_lTabWndList)
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
                _m_wSelectTabWnd.setSelected(true);
                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        /// <summary>
        /// 刷新当前窗口
        /// </summary>
        private void _refreshInfo()
        {
            if (_m_heroShowInfo == null || _m_heroShowInfo.heroRefObj == null || wnd == null)
                return;

            //伙伴名字
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.HERO, _m_heroShowInfo.id));
            //伙伴称号（皮肤名）
            ALUGUICommon.setLabelTxt(wnd.txtSkinName, GCommon.getItemName(ENPItemType.HERO_SKIN, _m_heroShowInfo.heroRefObj.default_skin_id));
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

            //展示形象
            if (_m_commonShowcaseWnd != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[4];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_heroShowInfo.getTdShow()), 0);
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_heroShowInfo.getTdBg()), 3);
                _m_commonShowcaseWnd.showWnd(showCaseUnitInfoObjList);
            }
            //初始资质
            ALUGUICommon.setLabelTxt(wnd.txtTalent, HeroCommon.calInitTalent(_m_heroShowInfo.heroRefObj.id));

            //设置左右切换按钮显隐
            bool needShowSwitchBtn = _m_lLockHeroInfoList.Count > 1;
            ALUGUICommon.setGameObjEnable(wnd.btnLeft, needShowSwitchBtn);
            ALUGUICommon.setGameObjEnable(wnd.btnRight, needShowSwitchBtn);
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

        /// <summary>
        /// 刷新配音气泡
        /// </summary>
        private void _refreshVoiceBubble()
        {
            if (_m_heroShowInfo == null)
                return;

            //设置配音气泡
            if (_m_wVoiceBubble != null)
            {
                _m_wVoiceBubble.showWnd();
                _m_wVoiceBubble.setInfo(_m_heroShowInfo.id, EHeroVoiceType.LOCK);
            }
        }

        #region 页签页面处理

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndHeroLockInfoTab _tabItemWnd)
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
        private void _refreshTabView(EHeroLockInfoTabType _tabView)
        {
            _hideAllPage();
            
            switch(_tabView)
            {
                case EHeroLockInfoTabType.INTRODUCTION://简介
                    _showIntroductionPage();
                    break;
                case EHeroLockInfoTabType.SKILL://技能
                    _showSkillPage();
                    break;
            }
        }
        
        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wIntroductionPage?.hideWnd();
            _m_wSkillPage?.hideWnd();
        }

        //显示简介页面
        private void _showIntroductionPage()
        {
            if (wnd == null || _m_heroShowInfo == null)
                return;
            
            if (_m_wIntroductionPage != null)
            {
                _m_wIntroductionPage.showWnd();
                _m_wIntroductionPage.setInfo(_m_heroShowInfo.id, null);
            }
            else
            {
                _m_wIntroductionPage = new GGUIWndHeroInfoIntroductionPage(_getPageAssetPathByType(EHeroLockInfoTabType.INTRODUCTION), wnd.pageParent);
                _m_wIntroductionPage.load(() =>
                {
                    if (_m_wIntroductionPage == null)
                        return;
                    _m_wIntroductionPage.showWnd();
                    _m_wIntroductionPage.setInfo(_m_heroShowInfo.id, null);
                });
            }
        }

        //显示技能页面
        private void _showSkillPage()
        {
            if (wnd == null || _m_heroShowInfo == null)
                return;
            
            if (_m_wSkillPage != null)
            {
                _m_wSkillPage.showWnd();
                _m_wSkillPage.setInfo(_m_heroShowInfo.id);
            }
            else
            {
                _m_wSkillPage = new GGUIWndHeroLockInfoSkillPage(_getPageAssetPathByType(EHeroLockInfoTabType.SKILL), wnd.pageParent);
                _m_wSkillPage.load(() =>
                {
                    if (_m_wSkillPage == null)
                        return;
                    _m_wSkillPage.showWnd();
                    _m_wSkillPage.setInfo(_m_heroShowInfo.id);
                });
            }
        }

        //根据页签类型获取对应的子页面的加载路径
        private NPCommonAssetPathInfo _getPageAssetPathByType(EHeroLockInfoTabType _type)
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

        //切换列表上一个伙伴
        private void _setLastHero(GameObject _gameObject)
        {
            if (_m_lLockHeroInfoList == null)
                return;

            _m_iCurShowIndex--;
            if (_m_iCurShowIndex < 0)
                _m_iCurShowIndex = _m_lLockHeroInfoList.Count - 1;

            _m_heroShowInfo = _m_lLockHeroInfoList[_m_iCurShowIndex];
            _refreshAll();
            //播放入场配音
            _refreshVoiceBubble();
        }

        //切换列表下一个伙伴
        private void _setNextHero(GameObject _gameObject)
        {
            if (_m_lLockHeroInfoList == null)
                return;

            _m_iCurShowIndex++;
            if (_m_iCurShowIndex >= _m_lLockHeroInfoList.Count)
                _m_iCurShowIndex = 0;

            _m_heroShowInfo = _m_lLockHeroInfoList[_m_iCurShowIndex];
            _refreshAll();
            //播放入场配音
            _refreshVoiceBubble();
        }

        //点击获取途径
        private void _onClickAccess(GameObject _gameObject)
        {
            if (wnd == null || _m_heroShowInfo == null || _m_heroShowInfo.heroRefObj == null)
                return;

            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                TextTranslate.instance.getLanguage(TransKeyConst.hero_accessWayDesc_str, TextTranslate.instance.getLanguage(_m_heroShowInfo.heroRefObj.transSource)),
                (RectTransform)_gameObject.transform, wnd.accessTipIntervalX, wnd.accessTipIntervalY));
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_UNLOCK_INFO);
        }

        #endregion
    }
}