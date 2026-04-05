using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 招募大臣窗口
    /// </summary>
    public class GGUIWndRecruitHero : _ANPGGUIBasicWnd<GGUIMonoRecruitHero>
    {
        private static GGUIWndRecruitHero _g_instance;
        public static GGUIWndRecruitHero instance { get { return _g_instance ??= new GGUIWndRecruitHero(); } }

        private RecruitShopInfo _m_rRecruitShopInfo;
        private RecruitHeroItemInfo _m_iRecruitHeroInfo;
        
        //相性图标
        private NPGGuiWndTexture _m_wSpecAttrIcon;
        //伙伴形象
        private NPGGUIWndCommonShowCase _m_commonShowcaseWnd;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        // 经营技能图标
        private NPGGuiWndTexture _m_wBusinessSkilIcon;
        // 觉醒技能列表
        private GGUIWndHeroStarSkillContainer _m_wHeroStarSkillContainer;
        
        private GGUISubWndRecruitExchangeBtn _m_wndRecruitExchangeBtn;
        
        public GGUIWndRecruitHero() : base(EALUIWndLayer.NORMAL)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoRecruitHero.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoRecruitHero.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.imgSpecAttrIcon != null)
                _m_wSpecAttrIcon = new NPGGuiWndTexture(wnd.imgSpecAttrIcon);

            if (wnd.monoShowCaseWnd != null)
                _m_commonShowcaseWnd = new NPGGUIWndCommonShowCase(wnd.monoShowCaseWnd);

            if (wnd.imgBusinessIcon != null)
                _m_wBusinessSkilIcon = new NPGGuiWndTexture(wnd.imgBusinessIcon);

            if (wnd.monoStarSkillContainer != null)
                _m_wHeroStarSkillContainer = new GGUIWndHeroStarSkillContainer(wnd.monoStarSkillContainer);
            
            if (wnd.monoExchangeBtn != null)
                _m_wndRecruitExchangeBtn = new GGUISubWndRecruitExchangeBtn(wnd.monoExchangeBtn);
            
            ALUGUICommon.combineBtnClick(wnd.btnMoreInfo, _onMoreInfoBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnMoreInfo, _onMoreInfoBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
            
            _m_wSpecAttrIcon?.discard();
            _m_wSpecAttrIcon = null;
            
            _m_commonShowcaseWnd?.discard();
            _m_commonShowcaseWnd = null;
            
            _m_wBusinessSkilIcon?.discard();
            _m_wBusinessSkilIcon = null;
            
            _m_wHeroStarSkillContainer?.discard();
            _m_wHeroStarSkillContainer = null;
            
            _m_wndRecruitExchangeBtn?.discard();
            _m_wndRecruitExchangeBtn = null;
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);

            _m_wSpecAttrIcon?.hideWnd();
            _m_commonShowcaseWnd?.hideWnd();
            _pushBackQualityGo();
            _m_wBusinessSkilIcon?.hideWnd();
            _m_wHeroStarSkillContainer?.hideWnd();
            
            _m_wndRecruitExchangeBtn?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSpecAttrIcon?.discardTexture();
            _m_commonShowcaseWnd?.resetWnd();
            _m_wBusinessSkilIcon?.discardTexture();
            _m_wHeroStarSkillContainer?.resetWnd();

            _m_wndRecruitExchangeBtn?.resetWnd();
        }

        public void setData(RecruitShopInfo _recruitShopInfo, RecruitHeroItemInfo _recruitHeroItemInfo)
        {
            _m_rRecruitShopInfo = _recruitShopInfo;
            _m_iRecruitHeroInfo = _recruitHeroItemInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            _refreshHeroInfo();
            
            if (_m_wndRecruitExchangeBtn != null)
            {
                _m_wndRecruitExchangeBtn.showWnd();
                _m_wndRecruitExchangeBtn.setData(_m_rRecruitShopInfo, _m_iRecruitHeroInfo);
            }
        }

        private void _refreshHeroInfo()
        {
            if (_m_iRecruitHeroInfo == null || wnd == null)
                return;

            HeroCardShowInfo heroShowInfo = _m_iRecruitHeroInfo.getHeroShowInfo(true);
            if(heroShowInfo == null || heroShowInfo.heroRefObj == null)
                return;

            //伙伴名字
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.HERO, heroShowInfo.id));
            //伙伴称号（皮肤名）
            ALUGUICommon.setLabelTxt(wnd.txtSkinName, GCommon.getItemName(ENPItemType.HERO_SKIN, heroShowInfo.heroRefObj.default_skin_id));
            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo(heroShowInfo);
            }
            //相性图标
            BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) heroShowInfo.heroRefObj.spec_attr_type);
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
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(heroShowInfo.getTdShow()), 0);
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(heroShowInfo.getTdBg()), 3);
                _m_commonShowcaseWnd.showWnd(showCaseUnitInfoObjList);
            }
            //初始资质
            ALUGUICommon.setLabelTxt(wnd.txtTalent, HeroCommon.calInitTalent(heroShowInfo.heroRefObj.id));
            
            HeroBusinessSkillRefObj businessSkillRefObj = GRefdataCoreMgr.instance.heroBusinessSkillRefCore.getRef(heroShowInfo.heroRefObj.business_skill_id);
            if (businessSkillRefObj == null)
                return;

            //经营技能
            if (_m_wBusinessSkilIcon != null)
            {
                _m_wBusinessSkilIcon.showWnd();
                _m_wBusinessSkilIcon.setTexture(businessSkillRefObj.icon);    
            }
            ALUGUICommon.setLabelTxt(wnd.txtBusinessName, TextTranslate.instance.getLanguage(businessSkillRefObj.name, businessSkillRefObj.name_args));
            PlayerBonusPropertyContainer bonusPropertyContainer = new PlayerBonusPropertyContainer();
            bonusPropertyContainer.addModifier(businessSkillRefObj.bonus_prop_modifier);
            ALUGUICommon.setLabelTxt(wnd.txtBusinessDesc, TextTranslate.instance.getLanguage(businessSkillRefObj.desc, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, bonusPropertyContainer.getValue(EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f)));
            
            //觉醒技能
            if (_m_wHeroStarSkillContainer != null)
            {
                _m_wHeroStarSkillContainer.showWnd();
                _m_wHeroStarSkillContainer.showItemList(null, heroShowInfo.heroRefObj.star_skill_id_list);
            }
        }
        
        //加载品质GO
        private void _popQualityGo(HeroCardShowInfo heroShowInfo)
        {
            if (wnd == null || wnd.goQualityIconParent == null || heroShowInfo == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, heroShowInfo.id);
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
        /// 兑换成功消息
        /// </summary>
        private void _onRecruitExchangeSucc()
        {
            // 刷新窗口
            _refreshWnd();
        }
        
        /// <summary>
        /// 更多信息按钮点击
        /// </summary>
        private void _onMoreInfoBtnClick(GameObject _go)
        {
            if(_m_iRecruitHeroInfo == null)
                return;
            
            QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(_m_iRecruitHeroInfo.getHeroShowInfo(true), null));
        }
        
        /// <summary>
        /// 返回按钮点击
        /// </summary>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RECRUIT_HERO);
        }
    }
}