using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴资质技能详情跟随窗口
    /// </summary>
    public class GGUIWndHeroTalentSkillDetailToolTip : _ATNPGGUIWndCommonItemToolTip<GGUIMonoHeroTalentSkillDetailToolTip>
    {

        //图标
        private NPGGuiWndTexture _m_wIcon;
        //伙伴id
        private long _m_lHeroId;
        //资质技能id
        private long _m_lTalentSkillId;
        //解锁星级
        private GGUIWndHeroCommonStar _m_wUnlockStar;

        public GGUIWndHeroTalentSkillDetailToolTip() : base(GGUIMonoHeroTalentSkillDetailToolTip.assetPath, GGUIMonoHeroTalentSkillDetailToolTip.objName)
        {
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            _m_wIcon?.hideWnd();
            _m_wUnlockStar?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            _m_wIcon?.discardTexture();
            _m_wUnlockStar?.resetWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wUnlockStar?.discard();
            _m_wUnlockStar = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.monoStar != null)
                _m_wUnlockStar = new GGUIWndHeroCommonStar(wnd.monoStar);
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeHeroTalentSkillDetailToolTip));
        }
        
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_talentSkillId"></param>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_intervalX"></param>
        /// <param name="_intervalY"></param>
        public void setInfo(long _heroId, long _talentSkillId, RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            _m_lHeroId = _heroId;
            _m_lTalentSkillId = _talentSkillId;
            _refreshWnd();
            setPos(_targetTransRoot,_intervalX, _intervalY);
        }

        //刷新界面
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            HeroTalentSkillRefObj heroTalentSkillRef = GRefdataCoreMgr.instance.heroTalentSkillRefCore.getRef(_m_lTalentSkillId);
            if (heroTalentSkillRef == null)
                return;

            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_lHeroId);
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_lHeroId);
            HeroTalentSkillInfo heroTalentSkillInfo = null;
            if (heroInfo != null)
                heroTalentSkillInfo = heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(_m_lTalentSkillId);
            bool isHeroUnlock = heroInfo != null;
            bool isSkillUnlock = heroTalentSkillInfo != null;

            //设置图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(heroTalentSkillRef.icon);
            }

            if (isSkillUnlock)
            {
                //已解锁
                //设置等级名称
                ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, heroTalentSkillInfo.level));
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(heroTalentSkillRef.name));

                //是否满级
                bool isMaxLevel = heroTalentSkillInfo.isMaxLevel;
                //当前资质
                PlayerAttrPropertyContainer curModifier = new PlayerAttrPropertyContainer();
                curModifier.addModifier(heroTalentSkillInfo.baseTalentSkillLevelRefObj.self_attr_prop_modifier);
                curModifier.addModifier(heroTalentSkillInfo.baseTalentSkillLevelRefObj.self_attr_prop_modifier_per_level, heroTalentSkillInfo.levelBonusStack);
                long curTalent = curModifier.getValue(EBasicAttrType.TALENT);
                //下一级资质
                curModifier.addModifier(heroTalentSkillInfo.baseTalentSkillLevelRefObj.self_attr_prop_modifier_per_level);
                long nextTalent = curModifier.getValue(EBasicAttrType.TALENT);
                if (isMaxLevel)
                    ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentAddValue_num, curTalent));
                else
                    ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentValueAndNextValue_num_num, curTalent, nextTalent));
            }
            else
            {
                //未解锁
                //设置等级名称
                ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, 1));
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(heroTalentSkillRef.name));

                //资质加成描述
                HeroTalentSkillLevelRefObj talentLevelRef = heroTalentSkillRef.getBaseTalentSkillLevelRefByLevel(1);
                //当前资质值
                long talentValue = 0;
                if (talentLevelRef != null && talentLevelRef.self_attr_prop_modifier != null)
                    talentValue = talentLevelRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.TALENT);
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_talentAddValue_num, talentValue));
                //显示星级
                if (_m_wUnlockStar != null)
                {
                    if (heroRef != null && heroRef.extra_talent_skill_id_list != null)
                    {
                        for (int i = 0; i < heroRef.extra_talent_skill_id_list.Count; i++)
                        {
                            if (heroRef.extra_talent_skill_id_list[i].second() == _m_lTalentSkillId)
                            {
                                _m_wUnlockStar.showWnd();
                                _m_wUnlockStar.setInfo(heroRef.extra_talent_skill_id_list[i].first());
                            }
                        }
                    }
                    else
                        _m_wUnlockStar.hideWnd();
                }
            }

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goHeroLockShowList, !isHeroUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goHeroLockHideList, isHeroUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goSkillLockShowList, !isSkillUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goSkillLockHideList, isSkillUnlock);
        }
    }
}