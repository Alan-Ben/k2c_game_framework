using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴未解锁技能展示页签
    /// </summary>
    public class GGUIWndHeroLockInfoSkillPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroLockInfoSkillPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴配置数据
        private HeroRefObj _m_heroRef;
        //套系列表
        private GGUIWndHeroConsortSimpleIconContainer _m_wSuitContainer;
        //加护列表
        private GGUIWndHeroConsortSimpleIconContainer _m_wConsortContainer;
        //经营技能图标
        private NPGGuiWndTexture _m_wBusinessIcon;
        //觉醒技能列表
        private GGUIWndHeroStarSkillContainer _m_wStarSkillContainer;
        //资质技能
        private GGUIWndHeroTalentSkillContainer _m_wTalentSkillContainer;

        public GGUIWndHeroLockInfoSkillPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }
        
        protected override void _onHideWnd()
        {
            _m_wSuitContainer?.hideWnd();
            _m_wConsortContainer?.hideWnd();
            _m_wBusinessIcon?.hideWnd();
            _m_wStarSkillContainer?.hideWnd();
            _m_wTalentSkillContainer?.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_wSuitContainer?.resetWnd();
            _m_wConsortContainer?.resetWnd();
            _m_wBusinessIcon?.discardTexture();
            _m_wStarSkillContainer?.resetWnd();
            _m_wTalentSkillContainer?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wSuitContainer?.discard();
            _m_wSuitContainer = null;
            _m_wConsortContainer?.discard();
            _m_wConsortContainer = null;
            _m_wBusinessIcon?.discard();
            _m_wBusinessIcon = null;
            _m_wStarSkillContainer?.discard();
            _m_wStarSkillContainer = null;
            _m_wTalentSkillContainer?.discard();
            _m_wTalentSkillContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSuitContainer != null)
                _m_wSuitContainer = new GGUIWndHeroConsortSimpleIconContainer(wnd.monoSuitContainer);

            if (wnd.monoConsortContainer != null)
                _m_wConsortContainer = new GGUIWndHeroConsortSimpleIconContainer(wnd.monoConsortContainer);

            if (wnd.imgBusinessIcon != null)
                _m_wBusinessIcon = new NPGGuiWndTexture(wnd.imgBusinessIcon);

            if (wnd.monoStarSkillContainer != null)
                _m_wStarSkillContainer = new GGUIWndHeroStarSkillContainer(wnd.monoStarSkillContainer);

            if (wnd.monoTalentSkillContainer != null)
                _m_wTalentSkillContainer = new GGUIWndHeroTalentSkillContainer(wnd.monoTalentSkillContainer);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        public void setInfo(long _heroId)
        {
            _m_heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_heroId);
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshSuit();
            _refreshConsort();
            _refreshBusiness();
            _refreshStarSkill();
            _refreshTalentSkill();
        }

        //刷新套系信息
        private void _refreshSuit()
        {
            if (wnd == null || _m_heroRef == null)
                return;

            if (_m_heroRef.suit_id > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoSuitHideList, true);
                HeroSuitRefObj suitRef = GRefdataCoreMgr.instance.heroHaloSuitRefCore.getRef(_m_heroRef.suit_id);
                if (suitRef == null || suitRef.heroRefList.Count == 0)
                {
                    ALUGUICommon.setGameObjEnable(wnd.goNoSuitHideList, false);
                    return;
                }
                else
                {
                    List<long> idList = new List<long>();
                    for (int i = 0; i < suitRef.heroRefList.Count; i++)
                    {
                        idList.Add(suitRef.heroRefList[i].id);
                    }

                    _m_wSuitContainer?.showWnd();
                    _m_wSuitContainer?.showItemList(idList,EHeroConsortSimpleIconShowType.HERO);

                    ALUGUICommon.setLabelTxt(wnd.txtSuitName, TextTranslate.instance.getLanguage(suitRef.halo_name));
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoSuitHideList, false);
            }
        }

        //刷新加护列表
        private void _refreshConsort()
        {
            if (wnd == null || _m_heroRef == null || _m_heroRef.relationConsortIdList == null)
                return;

            if (_m_heroRef.relationConsortIdList.Count > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoConsortHideList, true);
                _m_wConsortContainer?.showWnd();
                _m_wConsortContainer?.showItemList(_m_heroRef.relationConsortIdList, EHeroConsortSimpleIconShowType.CONSORT);
            }
            else
                ALUGUICommon.setGameObjEnable(wnd.goNoConsortHideList, false);
        }

        //刷新经营技能
        private void _refreshBusiness()
        {
            if (wnd == null || _m_heroRef == null)
                return;

            HeroBusinessSkillRefObj businessSkillRefObj = GRefdataCoreMgr.instance.heroBusinessSkillRefCore.getRef(_m_heroRef.business_skill_id);
            if (businessSkillRefObj == null)
                return;

            //经营技能
            _m_wBusinessIcon?.showWnd();
            _m_wBusinessIcon?.setTexture(businessSkillRefObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtBusinessName, TextTranslate.instance.getLanguage(businessSkillRefObj.name, businessSkillRefObj.name_args));
            PlayerBonusPropertyContainer bonusPropertyContainer = new PlayerBonusPropertyContainer();
            bonusPropertyContainer.addModifier(businessSkillRefObj.bonus_prop_modifier);
            ALUGUICommon.setLabelTxt(wnd.txtBusinessDesc, TextTranslate.instance.getLanguage(businessSkillRefObj.desc, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, bonusPropertyContainer.getValue(EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f)));

            //额外的经营技能列表
            List<WCGPairInt> addSkillList = _m_heroRef.extra_business_skill_id_list;
            if (addSkillList == null)
                return;
            addSkillList.Sort((_a, _b) => _a.first().CompareTo(_b.first()));
            if (wnd.monoAdditionItemList != null)
            {
                for (int i = 0; i < addSkillList.Count; i++)
                {
                    //ui是否有配置
                    if (wnd.monoAdditionItemList.Count <= i)
                        break;

                    WCGPairInt tempSkillPair = addSkillList[i];
                    if (tempSkillPair == null)
                        continue;

                    ALUGUICommon.setGameObjEnable(wnd.monoAdditionItemList[i]?.goItem, true);
                    //描述
                    HeroBusinessSkillRefObj skillRef = GRefdataCoreMgr.instance.heroBusinessSkillRefCore.getRef(tempSkillPair.second());
                    if (skillRef != null)
                        ALUGUICommon.setLabelTxt(wnd.monoAdditionItemList[i]?.txtDesc, TextTranslate.instance.getLanguage(skillRef.desc, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, skillRef.bonus_prop_modifier?.getPropValue(EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f)));
                }
                //隐藏多余的item
                long addCount = wnd.monoAdditionItemList.Count - addSkillList.Count;
                if (addCount > 0)
                {
                    for (int j = wnd.monoAdditionItemList.Count - 1; j >= wnd.monoAdditionItemList.Count - addCount; j--)
                    {
                        ALUGUICommon.setGameObjEnable(wnd.monoAdditionItemList[j]?.goItem, false);
                    }
                }
            }
        }

        //刷新觉醒技能列表
        private void _refreshStarSkill()
        {
            if (wnd == null || _m_heroRef == null)
                return;

            if (_m_heroRef.star_skill_id_list != null && _m_heroRef.star_skill_id_list.Count > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoStarHideList, true);
                _m_wStarSkillContainer?.showWnd();
                _m_wStarSkillContainer?.showItemList(null, _m_heroRef.star_skill_id_list);
            }
            else
                ALUGUICommon.setGameObjEnable(wnd.goNoStarHideList, false);
        }

        //刷新资质技能列表
        private void _refreshTalentSkill()
        {
            if (wnd == null || _m_heroRef == null)
                return;

            List<long> idList = new List<long>();
            //获取默认资质列表
            if (_m_heroRef.default_talent_skill_id_list != null)
                idList.AddRange(_m_heroRef.default_talent_skill_id_list);

            //剔除跟着升阶升级的资质列表
            if (_m_heroRef.auto_upgrade_talent_skill_id_list != null)
            {
                for (int i = 0; i < _m_heroRef.auto_upgrade_talent_skill_id_list.Count; i++)
                {
                    if (idList.Contains(_m_heroRef.auto_upgrade_talent_skill_id_list[i]))
                        idList.Remove(_m_heroRef.auto_upgrade_talent_skill_id_list[i]);
                }
            }

            _m_wTalentSkillContainer?.showWnd();
            _m_wTalentSkillContainer?.showItemList(_m_heroRef.id, idList);

        }
    }
}